# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

BQuery is a Blazor helper library for JavaScript interop, inspired by jQuery. It provides:
- DOM manipulation (attributes, classes, styles)
- Viewport measurements
- Window event handling (resize, scroll, mouse, keyboard, touch)
- Element drag functionality

The library is multi-targeted: .NET 8.0 and .NET 10.0.

## Build Commands

### .NET (C# library)
```bash
dotnet build                    # Build the solution
dotnet build -c Release         # Release build (triggers JS build automatically)
dotnet pack src/BQuery/BQuery.csproj -c Release   # Create NuGet package
```

### TypeScript/JavaScript
```bash
cd src/BQuery/wwwroot
pnpm install                    # Install dependencies
pnpm run build                  # Production build (minified, no sourcemap)
pnpm run dev                    # Development build (with sourcemap, watch mode)
```

The release build automatically runs `pnpm run build` via MSBuild target.

## Architecture

### C# Side (`src/BQuery/`)

| File | Purpose |
|------|---------|
| `Bq.cs` | Main scoped service exposing `Events`, `Viewport`, drag APIs, and window listener registration |
| `BqEvents.cs` | Window event hub - partial class augmented by source generator |
| `BqViewport.cs` | Viewport measurement APIs (width, height, scroll positions) |
| `ElementReferenceExtensions.cs` | Extension methods on `ElementReference` for DOM operations |
| `Constants/JsModuleConstants.cs` | JavaScript function name constants for interop calls |
| `Constants/WindowEvents.cs` | `WindowEvent` struct with event definitions decorated with `[WindowEventHandler]` |
| `SourceGeneration/` | Source-generator attributes consumed by the main library |
| `AspNetExtensions/` | `UseBQuery()` startup extensions for WASM and Server hosting |

### Source Generators (`src/BQuery.SourceGenerators/`)

| Generator | Purpose |
|-----------|---------|
| `JsInteropMethodsGenerator.cs` | Generates `*Method` constants from `JsModuleConstants` nested groups via `[GenerateJsInteropMethods]` |
| `WindowEventsGenerator.cs` | Generates event members and `[JSInvokable]` callbacks in `BqEvents` from `WindowEvent` fields decorated with `[WindowEventHandler]` |

### TypeScript Side (`src/BQuery/wwwroot/src/`)

| Module | Purpose |
|--------|---------|
| `index.ts` | Entry point - constructs and exports the `bQuery` object |
| `module/Viewport.ts` | Viewport measurement functions |
| `module/HtmlElementHelper.ts` | Element dimension and position helpers |
| `module/DragHelper.ts` | Drag-and-drop functionality |
| `module/eventHelper.ts` | Window event binding for Blazor callbacks |
| `module/domHelper.ts` | DOM attribute, class, and style manipulation |
| `module/common.ts` | `throttle` and `debounce` utilities |

### Interop Pattern

1. **C# → JS**: C# calls `IJSRuntime.InvokeAsync` using generated `*Method` constants derived from `JsModuleConstants`
2. **JS → C#**: JavaScript calls `DotNetObjectReference.invokeMethodAsync` on the scoped `BqEvents` instance, which raises the matching .NET events

The JavaScript module path is `./_content/BQuery/dist/bQuery.min.mjs` (ES module format).

## Key Patterns

### ElementReference JSRuntime Resolution
The library uses `UnsafeAccessor` to extract `IJSRuntime` from `WebElementReferenceContext`, allowing extension methods to work without explicit JSRuntime injection:
```csharp
[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<JSRuntime>k__BackingField")]
private static extern ref IJSRuntime GetJsRuntime(WebElementReferenceContext context);
```

### Event Naming Convention
- JavaScript function names: camelCase (e.g., `getWidth`, `getScrollTop`)
- C# method names: PascalCase with `Async` suffix (e.g., `GetWidthAsync`, `GetScrollTopAsync`)

### Source Generator Rules

#### JS Interop Generator
- Prefer the JS interop source generator over manually calling `JsModuleConstants.GetMethod(...)`.
- To enable generation for a constant group, declare a local partial marker class with `[GenerateJsInteropMethods(typeof(...))]`.
- Use generated fields in the form `<MethodName>Method`, for example `ElementConstants.GetWidthMethod` or `DragConstants.BindDragMethod`.
- For nested constant groups such as `JsModuleConstants.ElementExtensions` or `JsModuleConstants.Drag`, create a dedicated marker class such as `ElementConstants` or `DragConstants`.
- For top-level methods on `JsModuleConstants`, use a marker class such as `BqConstants`.
- Keep marker classes close to the consuming code unless there is a clear shared location that improves discoverability.
- When adding new JS interop APIs, add the method name to `JsModuleConstants` first, then consume it through the generated constant class rather than string concatenation.
- Do not introduce new direct `JsModuleConstants.GetMethod(...)` calls in application code unless the usage is too dynamic for source generation.

#### Window Events Generator
- Define new window events as `static readonly WindowEvent` fields in `WindowEvents.cs`.
- Decorate each field with `[WindowEventHandler(typeof(EventArgsType))]` to trigger source generation.
- The generator produces sync (`Action<T>`) and async (`Func<T, Task>`) events plus `[JSInvokable]` callback methods in `BqEvents`.
- For events requiring two arguments, use `[WindowEventHandler(typeof(T1), typeof(T2))]`.
- When a generator depends on constants metadata, prefer extending the constants definition with attributes over hard-coding parallel lookup tables inside the generator.

### Sample Projects
The `Sample/` directory contains:
- `BQuery.Sample.Wasm` - WebAssembly demo
- `BQuery.Sample.Server` - Blazor Server demo
- `BQuery.Sample.Common` - Shared Razor components
- `BlazorAppAuto/` - Auto render mode demo

## CI/CD

The GitHub workflow (`.github/workflows/dotnet-tag.yml`) triggers on tag push, builds for .NET 8/10, and publishes to NuGet.