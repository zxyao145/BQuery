# BQuery

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/BQuery)](https://www.nuget.org/packages/BQuery/)

BQuery is a Blazor helper library for JavaScript interop (inspired by jQuery).

It provides:

- Element DOM helpers (`Attr`, class/style helpers)
- Element measurement and position APIs
- Viewport measurement APIs
- Window event binding with .NET callbacks
- Element drag helpers

[Live demo](https://zxyao145.github.io/BQuery/)

## Target Frameworks

| BQuery | .NET |
| --- | --- |
| 4.x | .NET 8.0 / .NET 10.0 |
| 3.x | .NET 6 |
| 2.x | .NET 5 |

## Install

```bash
dotnet add package BQuery
```

## Quick Start

### 1. Register services

```csharp
builder.Services.AddBQuery();
```

> [!NOTE]
> If using **Interactive Auto** render mode, the service needs to be registered in both the `App` and `App.Client` simultaneously.

### 2. Load the script

Add this script to your host page:

```html
<script src="_content/BQuery/dist/bQuery.min.js"></script>
```

The repository samples use the UMD build (`bQuery.min.js`).  
An ES module build is also published at `_content/BQuery/dist/bQuery.min.mjs`.

#### WASM example (`wwwroot/index.html`)

```html
<script src="_content/BQuery/dist/bQuery.min.js"></script>
<script src="_framework/blazor.webassembly.js"></script>
```

#### Server example (`Pages/_Host.cshtml`)

```html
<script src="_content/BQuery/dist/bQuery.min.js"></script>
<script src="_framework/blazor.server.js"></script>
```

#### Auto Mode (App.razor)

```html
<script src="_content/BQuery/dist/bQuery.min.js"></script>
<script src="@Assets["_framework/blazor.web.js"]"></script>
```

### 3. Use `Bq` in components

```razor
+ @inject Bq bq
@implements IAsyncDisposable

@code {
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        await bq.AddWindowEventListeners(WindowEvent.OnResize, WindowEvent.OnScroll);
        bq.WindowEvents.OnResizeAsync += OnResizeAsync;
    }

    private Task OnResizeAsync(ResizeEventArgs args)
    {
        Console.WriteLine($"Viewport: {args.Width} x {args.Height}");
        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {   
        // If listening for Window events
        await bq.RemoveWindowEventListeners();
    }
}
```

## Common Usage

### Viewport

```csharp
var width = await bq.Viewport.GetWidthAsync();
var height = await bq.Viewport.GetHeightAsync();
var scrollTop = await bq.Viewport.GetScrollTopAsync();
```

### Element APIs (`ElementReference` extensions)

```csharp
var size = await element.GetWidthAndHeightAsync();
var docPos = await element.GetPositionInDocAsync();

await element.AddCls("is-active");
await element.Css("display", "none");
await element.RemoveCls("is-active");
```

### Drag

```csharp
await bq.Drag.BindDragAsync(dialog, new DragOptions
{
    InViewport = true,
    DragElement = dialogHeader
});

await bq.Drag.ResetDragPositionAsync(dialog, new DragOptions
{
    DragElement = dialogHeader
});

await bq.Drag.RemoveDragAsync(dialog, new DragOptions
{
    DragElement = dialogHeader
});
```

### User agent

```csharp
var ua1 = await bq.GetUserAgentAsync();
var ua2 = await jsRuntime.GetUserAgentAsync(); // IJSRuntime extension
```

## API Docs

- C# API: [API.md](./API.md)
- TypeScript API: [TS-API.md](./TS-API.md)

## Samples

- `Sample/BQuery.Sample.Wasm`
- `Sample/BQuery.Sample.Server`
- `Sample/BQuery.Sample.Common`
- `Sample/BlazorAppAuto`

## License

MIT
