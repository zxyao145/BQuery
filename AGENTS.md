# AGENTS.md

This file defines how coding agents should work in the BQuery repository. Unless stated otherwise, paths are relative to the repository root. Source code and configuration are the source of truth for project structure, build behavior, and API signatures.

## Working Guidelines

- Check `git status` before editing and preserve existing user changes. State key assumptions and the verification approach first; clarify ambiguities that affect implementation.
- Change only what the current task requires. Follow the surrounding code style without unrelated refactoring, dependency upgrades, or cleanup.
- Follow `.editorconfig` for C# and `src/BQuery/wwwroot/biome.json` for TypeScript. Limit formatting to the files and code being changed.
- If `.codegraph/` exists at the root, use CodeGraph to locate code first; otherwise, use `rg`. Do not create an index on your own.
- Do not edit source generator output or JavaScript build artifacts by hand. Update their inputs and regenerate them.
- Do not create Git commits, push, or publish without explicit user authorization.
- Report changes and verification results when finished. Clearly identify checks that failed or were not run.

## Project Overview and Entry Points

BQuery is a C# / JavaScript interop helper library for Blazor. It provides DOM operations, viewport and element measurements, window events, dragging, and browser information.

- Solution: `BQuery.slnx`.
- Main library: `src/BQuery/BQuery.csproj`, targeting both `net8.0` and `net10.0`.
- Roslyn source generators: `src/BQuery.SourceGenerators/BQuery.SourceGenerators.csproj`, targeting `netstandard2.0` and referenced by the main library as an Analyzer.
- Samples: `Sample/`, targeting `net10.0`, with Server, WebAssembly, Auto render mode, and shared components.
- See `README.md` for usage and `API.md` for the public API reference. Check relevant documentation and samples when changing public behavior.

### C# and Source Generators

The following paths are relative to `src/`:

| Path | Responsibility |
| --- | --- |
| `BQuery/Bq.cs` | Main service exposing `Viewport`, `Drag`, `WindowEvents`, and related APIs |
| `BQuery/BqEvents.cs` | Event subscriptions, callback dispatch, and disposal for the current DI scope |
| `BQuery/BqViewport.cs`, `BQuery/BqDrag.cs` | Viewport measurement and drag interop |
| `BQuery/Extensions/` | DOM and measurement extensions for `ElementReference`, plus `IJSRuntime` extensions |
| `BQuery/Constants/JsModuleConstants.cs` | JS module and method name definitions |
| `BQuery/Constants/WindowEvents.cs` | `WindowEvent` definitions and event argument metadata |
| `BQuery/Sg/`, `BQuery/Sg/Constants/` | Generator attributes and partial marker classes for interop constants |
| `BQuery/InputDtos/`, `BQuery/OutputDtos/` | Data received from JS and options passed to JS |
| `BQuery/ServiceExtension.cs` | `AddBQuery()` service registration |
| `BQuery.SourceGenerators/JsInteropMethodsGenerator.cs` | Generates `<MethodName>Method` fields |
| `BQuery.SourceGenerators/WindowEventsGenerator.cs` | Generates events, `EventSlots`, and `[JSInvokable]` callbacks |

### TypeScript and Static Assets

Source files are under `src/BQuery/wwwroot/src/`:

| Path | Responsibility |
| --- | --- |
| `index.ts` | Assembles the modules and assigns `window.bQuery` |
| `module/Viewport.ts`, `module/HtmlElementHelper.ts` | Viewport and element measurements |
| `module/domHelper.ts` | Attribute, class, and style operations |
| `module/DragHelper.ts` | Drag binding, removal, and position reset |
| `module/eventHelper.ts` | Browser window listeners, argument conversion, and .NET callbacks |
| `module/common.ts` | Utilities such as `throttle` and `debounce` |

`src/BQuery/wwwroot/vite.config.js` currently builds only UMD output, producing `dist/bQuery.min.js`. The production build script also copies it to `wwwroot/bQuery.min.js`. Git ignores these generated artifacts.

The host loads `_content/BQuery/dist/bQuery.min.js`, and C# invokes JS through the global `bQuery.*` API. `src/BQuery/wwwroot/BQuery.lib.module.js` is the Blazor initializer; its `beforeWebStart` hook loads that asset. The current configuration does not produce `bQuery.min.mjs`.

## Implementation Guidelines

### C# → JS Interop

1. Implement the functionality in the appropriate TypeScript module and expose it through the `bQuery` object in `index.ts`.
2. Define the method name in the corresponding `JsModuleConstants` group. Keep module names, method names, argument order, and return structures aligned with JS.
3. Use `[GenerateJsInteropMethods(typeof(...))]` to generate and consume `<MethodName>Method` fields, such as `ElementConstants.GetWidthMethod` and `DragConstants.BindDragMethod`.

Reuse existing partial marker classes in `src/BQuery/Sg/Constants/`. `BqViewport` applies the attribute directly to its own partial class. Follow these patterns for new calls; call `JsModuleConstants.GetMethod(...)` directly only when dynamic composition is actually required.

Use camelCase for JS methods and PascalCase with an `Async` suffix for new C# asynchronous methods. Do not rename existing public APIs solely to make naming consistent. Main library changes must remain compatible with both target frameworks.

`ElementReferenceExtensions` uses `UnsafeAccessor` to read `<JSRuntime>k__BackingField` from `WebElementReferenceContext`, allowing extensions to work without explicit `IJSRuntime` injection. Validate changes to this mechanism on both target frameworks, and ensure the element has rendered and has a valid context.

### JS → C# Window Events

1. Add a `static readonly WindowEvent` field in `src/BQuery/Constants/WindowEvents.cs` and annotate it with `[WindowEventHandler(typeof(T))]`. Use `[WindowEventHandler(typeof(T1), typeof(T2))]` for two-argument metadata.
2. Let `WindowEventsGenerator` generate synchronous events, asynchronous events, event slot mappings, and `[JSInvokable]` callbacks for `BqEvents`. Do not duplicate generated members manually.
3. Update `eventMap` and argument conversion in `src/BQuery/wwwroot/src/module/eventHelper.ts`. Browser event names, callback names, and arguments must match C#, for example: `mousedown` → `WindowMouseDown` → `OnMouseDown` / `OnMouseDownAsync`.

When a generator needs additional metadata, prefer extending attributes on the constant definitions over adding a separate hard-coded mapping. Introducing a two-argument event also requires implementing the corresponding `EventSlot<T1, T2>` support; `BqEvents` currently implements only single-argument event slots.

### Lifecycle

- `AddBQuery()` registers `Bq` and `BqEvents` as scoped services. Each `BqEvents` instance owns its `_listenerId`, event slots, and `DotNetObjectReference`. Keep subscriptions isolated between scopes.
- Subscribing directly to `WindowEvents.On...` only adds a .NET handler; the window listener registration API must also be called. See `Sample/BQuery.Sample.Common/Pages/WindowEvents.razor.cs`.
- Run browser interop after interactive rendering. Samples typically register listeners in the `firstRender` branch of `OnAfterRenderAsync`.
- `BqEvents.DisposeAsync()` unregisters the current scope's JS listeners and releases its .NET reference. Preserve handling for repeated disposal and disconnected connections.
- When a component unsubscribes, avoid removing subscriptions owned by other components in the same scope. Calling `RemoveWindowEventListeners()` without arguments clears all window listeners and event handlers in the current scope.

## Build and Verification

Use the .NET 10 SDK, Node.js 26, and pnpm 12. The exact pnpm version is pinned by `packageManager` in `src/BQuery/wwwroot/package.json`; CI reads the same setting. Run the following commands from the repository root.

```bash
# Install frontend dependencies before the first JS or Release build
pnpm --dir src/BQuery/wwwroot install --frozen-lockfile

# Build the main library for both target frameworks, including source generators
dotnet build src/BQuery/BQuery.csproj

# Build the entire solution, including samples
dotnet build

# JS production build: minified, without sourcemaps
pnpm --dir src/BQuery/wwwroot run build

# JS development build: watches for changes and emits sourcemaps; stop manually
pnpm --dir src/BQuery/wwwroot run dev

# TypeScript type checking and read-only lint
pnpm --dir src/BQuery/wwwroot exec tsc --noEmit
pnpm --dir src/BQuery/wwwroot run lint

# Release runs pnpm run build automatically, but does not install frontend dependencies
dotnet build -c Release

# Create a local NuGet package
dotnet pack src/BQuery/BQuery.csproj -c Release
```

Debug builds do not build JS automatically. Generate the latest scripts before running a sample. `pnpm run check`, `format`, and `lint:fix` write to files; use the type-checking and lint commands above for read-only checks.

Choose verification according to the scope of the change:

| Change | Verification |
| --- | --- |
| Documentation only | Check referenced paths, commands, and implementation; run `git diff --check` |
| C# or source generators | Build the main library for both target frameworks; inspect generated output when changing generators |
| TypeScript | Run type checking, lint, and the production build |
| Public APIs, interop, or lifecycle | Build the solution and verify browser behavior in the affected samples |
| Build or packaging configuration | Run a Release build and local packaging; inspect static assets in the package |

The repository currently has no dedicated automated test project. Do not describe a successful `dotnet test` exit as evidence of regression coverage. For behavior fixes, establish a reproducible case first, then verify the fix. For event changes, focus on registration, callbacks, unsubscription, and scope disposal.

Sample entry points:

```bash
dotnet run --project Sample/BQuery.Sample.Server
dotnet run --project Sample/BQuery.Sample.Wasm
dotnet run --project Sample/BlazorAppAuto/BlazorAppAuto
```

`.github/workflows/dotnet-tag.yml` runs the Release build, test command, packaging, and NuGet publishing when a tag is pushed.
