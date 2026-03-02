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
dotnet pack src/BQuery.csproj -c Release   # Create NuGet package
```

### TypeScript/JavaScript
```bash
cd src/wwwroot
pnpm install                    # Install dependencies
pnpm run build                  # Production build (minified, no sourcemap)
pnpm run dev                    # Development build (with sourcemap, watch mode)
```

The release build automatically runs `pnpm run build` via MSBuild target.

## Architecture

### C# Side (`src/`)

| File | Purpose |
|------|---------|
| `Bq.cs` | Main static entry point with `Events` and `Viewport` properties |
| `BqEvents.cs` | Window event hub - exposes sync `Action` and async `Func<Task>` events |
| `BqInterop.cs` | `[JSInvokable]` callbacks that JavaScript calls to forward browser events |
| `BqViewport.cs` | Viewport measurement APIs (width, height, scroll positions) |
| `ElementReferenceExtensions.cs` | Extension methods on `ElementReference` for DOM operations |
| `JsModuleConstants.cs` | JavaScript function name constants for interop calls |
| `AspNetExtensions/` | `UseBQuery()` startup extensions for WASM and Server hosting |

### TypeScript Side (`src/wwwroot/src/`)

| Module | Purpose |
|--------|---------|
| `index.ts` | Entry point - constructs and exports the `bQuery` object |
| `Viewport.ts` | Viewport measurement functions |
| `HtmlElementHelper.ts` | Element dimension and position helpers |
| `DragHelper.ts` | Drag-and-drop functionality |
| `eventHelper.ts` | Window event binding for Blazor callbacks |
| `domHelper.ts` | DOM attribute, class, and style manipulation |
| `common.ts` | `throttle` and `debounce` utilities |

### Interop Pattern

1. **C# → JS**: C# calls `IJSRuntime.InvokeAsync` using method names from `JsModuleConstants`
2. **JS → C#**: JavaScript calls `DotNet.invokeMethod` to trigger `[JSInvokable]` methods in `BqInterop`, which then raises events on `Bq.Events`

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

### Sample Projects
The `Sample/` directory contains:
- `BQuery.Sample.Wasm` - WebAssembly demo
- `BQuery.Sample.Server` - Blazor Server demo
- `BQuery.Sample.Common` - Shared Razor components
- `BlazorAppAuto/` - Auto render mode demo

## CI/CD

The GitHub workflow (`.github/workflows/dotnet-tag.yml`) triggers on tag push, builds for .NET 6/8/10, and publishes to NuGet.