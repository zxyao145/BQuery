# Repository Initialization & Analysis

## Initialization status

- Ran JavaScript dependency initialization in `BQuery/wwwroot` with `npm ci`.
- Attempted to initialize/restore .NET projects, but `dotnet` is not installed in this environment.

## High-level architecture

- `BQuery/`: Core reusable Blazor library exposing static helpers (`Bq`), viewport APIs (`BqViewport`), event APIs (`BqEvents`), interop constants, and JS-invokable callback bridge (`BqInterop`).
- `BQuery/wwwroot/`: TypeScript source and Vite build config that bundles browser-side interop script `bQuery.min.js` used by consumers.
- `Sample/BQuery.Sample.Common/`: Shared Razor pages that demonstrate usage patterns for viewport helpers, element helpers, drag support, and window event subscriptions.
- `Sample/BQuery.Sample.Wasm/`: WebAssembly host sample consuming the library.
- `Sample/BQuery.Sample.Server/`: Server-side Blazor host sample consuming the library.

## Runtime model

- Consumer apps call `.UseBQuery()` during host startup (WASM and server overloads are provided).
- `Bq.Init(...)` sets service-provider-backed lookup and initializes singleton-like `Viewport` and `Events` surfaces.
- Browser JS defines `window.bQuery` API and binds browser events, then calls back to C# using `[JSInvokable]` methods in `BqInterop`.
- C# event hub (`BqEvents`) re-exposes browser events as .NET events and async delegates for app code.

## Build and compatibility observations

1. **.NET SDK dependency**
   - The solution targets `.NET 6` projects and cannot be validated in this environment without `dotnet` installed.

2. **Current JS build failure with Vite 6**
   - `npm run build` fails with:
     - warning about `build.outDir` configured as project root (`./`), and
     - runtime error `Cannot convert undefined or null to object` during Vite output resolution.
   - The Vite config appears authored for older Vite behavior and likely needs update for Vite 6 compatibility.

3. **Potential source issue on case-sensitive filesystems**
   - `src/bQuery.ts` imports `./module/ViewPort`, but the file is `Viewport.ts`.
   - This can break builds on Linux/macOS depending on tooling resolution behavior.

## Code quality notes

- Core API surface is straightforward and library-oriented, with clear extension methods on `ElementReference`.
- Event model is broad and useful, but handlers are attached in sample pages without teardown (`IDisposable`/unsubscription) which may lead to duplicate handlers on repeated page mounts.
- `Bq.Init` is `async void`, which can hide initialization exceptions and complicate startup diagnostics.

## Suggested next steps

1. Install .NET 6 SDK (or compatible multi-SDK setup) in CI/dev environment and run full `dotnet restore/build/test`.
2. Make JS build deterministic:
   - either pin Vite to a known working major version, or
   - update `vite.config.js` to Vite 6 conventions (especially output directory config).
3. Fix filename casing mismatch (`ViewPort` vs `Viewport`) to avoid cross-platform failures.
4. Consider replacing `async void` startup initialization with task-returning flow where possible.
5. Add minimal automated smoke tests (at least build checks for solution and JS bundle).
