"""Verify a first Release pack and consume its assets from a fresh published app."""

import json
import shutil
import subprocess
import tempfile
import uuid
import zipfile
from pathlib import Path


def run(*command, cwd):
    subprocess.run(command, cwd=cwd, check=True)


repository = Path(__file__).resolve().parent.parent
with tempfile.TemporaryDirectory(prefix="bquery-package-") as directory:
    clean = Path(directory)
    shutil.copytree(
        repository / "src",
        clean / "src",
        ignore=shutil.ignore_patterns("bin", "obj", "node_modules", "dist", "bQuery.min.js"),
    )
    shutil.copy2(repository / "README.md", clean / "README.md")
    frontend = clean / "src/BQuery/wwwroot"
    run("pnpm", "install", "--frozen-lockfile", cwd=frontend)
    version = "0.0.0-regression." + uuid.uuid4().hex
    output = clean / "packages"
    run(
        "dotnet", "pack", "src/BQuery/BQuery.csproj", "-c", "Release",
        "--nologo", "--verbosity", "minimal", "--output", str(output),
        "-p:PackageVersion=" + version, cwd=clean,
    )
    with zipfile.ZipFile(output / f"BQuery.{version}.nupkg") as package:
        names = package.namelist()
        for expected in ["staticwebassets/dist/bQuery.min.js", "staticwebassets/bQuery.min.js"]:
            assert expected in names, f"First pack omitted {expected}"
        assert any(name.endswith(".lib.module.js") for name in names), "Initializer is missing"
        assert not any(
            name.startswith("staticwebassets/") and (
                name.endswith((".ts", ".map", ".yaml"))
                or "/tests/" in name
                or name.endswith(("package.json", "vite.config.js", "biome.json"))
            ) for name in names
        ), "Development files leaked into the package"

    for framework in ["net8.0", "net10.0"]:
        manifest = clean / "src/BQuery/obj/Release" / framework / "staticwebassets.build.json"
        assets = json.loads(manifest.read_text())["Assets"]
        assert any(
            Path(asset["Identity"]).name == "bQuery.min.js"
            and Path(asset["Identity"]).parent.name == "dist"
            for asset in assets
        ), f"{framework} manifest omitted the runtime script"

    consumer = clean / "Consumer"
    consumer.mkdir()
    (consumer / "Consumer.csproj").write_text(f"""<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup><TargetFramework>net10.0</TargetFramework><ImplicitUsings>enable</ImplicitUsings></PropertyGroup>
  <ItemGroup><PackageReference Include="BQuery" Version="{version}" /></ItemGroup>
</Project>
""")
    (consumer / "Program.cs").write_text("""var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapStaticAssets();
app.Run();
""")
    published = clean / "published"
    run(
        "dotnet", "publish", "-c", "Release", "--nologo", "--verbosity", "minimal",
        "--output", str(published), "-p:RestoreAdditionalProjectSources=" + str(output),
        cwd=consumer,
    )
    script = published / "wwwroot/_content/BQuery/dist/bQuery.min.js"
    assert script.is_file() and script.stat().st_size > 0, "Consumer publish omitted the script"
    print("PASS: first pack, both framework manifests, and fresh consumer publish contain BQuery assets.")
