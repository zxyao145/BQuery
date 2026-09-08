import { defineConfig } from "@playwright/test";
import { fileURLToPath } from "node:url";

const repository = fileURLToPath(new URL("../../../", import.meta.url));

export default defineConfig({
	testDir: "./tests",
	outputDir: "../../../artifacts/browser",
	workers: 1,
	reporter: "list",
	use: { browserName: "chromium", actionTimeout: 10000 },
	webServer: [
		["Sample/BQuery.Sample.Server", 5301],
		["Sample/BQuery.Sample.Wasm", 5302],
		["Sample/BQuery.Sample.Auto", 5303],
	].map(([project, port]) => ({
		command: `dotnet run --project ${project} -c Release --no-build --no-launch-profile --urls http://127.0.0.1:${port}`,
		cwd: repository,
		url: `http://127.0.0.1:${port}`,
		env: { ASPNETCORE_ENVIRONMENT: "Development" },
		gracefulShutdown: { signal: "SIGTERM" as const, timeout: 5000 },
	})),
});
