# BQuery

[![Nuget (con versiones previas)](https://img.shields.io/nuget/vpre/BQuery)](https://www.nuget.org/packages/BQuery/)

BQuery es una biblioteca de ayuda para Blazor que facilita la interoperabilidad con JavaScript (inspirada en jQuery).

Ofrece:

- Funciones de ayuda para elementos DOM (atributos, clases y estilos)
- APIs para medir y obtener la posición de elementos
- APIs para medir el viewport
- Vinculación de eventos de ventana con callbacks en .NET
- Funciones de ayuda para arrastrar elementos

[Demo en vivo](https://zxyao145.github.io/BQuery/)

## Frameworks compatibles

| BQuery | .NET |
| --- | --- |
| 4.x | .NET 8.0 / .NET 10.0 |
| 3.x | .NET 6 |
| 2.x | .NET 5 |

## Cambios importantes en la versión 4.0.0

Si estás actualizando desde la versión 3.x, revisa los siguientes cambios en la API y la plataforma:

- **Cambio en los frameworks compatibles**: BQuery 4.x es compatible únicamente con **.NET 8.0** y **.NET 10.0**. .NET 6 ya no es compatible.
- **Eliminación de extensiones de inicio**: el antiguo paquete `AspNetExtensions` (por ejemplo, `UseBQuery(...)`) ha sido eliminado. Utiliza el registro de DI con `AddBQuery()` y carga el recurso estático desde `_content/BQuery/dist/...`.
- **Cambio en el payload de los callbacks de redimensionamiento**: los manejadores de redimensionamiento ahora utilizan `ResizeEventArgs` (con propiedades con nombre como width/height) en lugar de patrones de payload posicional.

### Lista de verificación para la actualización

1. Actualiza el TFM de tu aplicación a .NET 8 o superior.
2. Reemplaza el uso de la extensión de inicio heredada con `builder.Services.AddBQuery();`.
3. Asegúrate de que tu página de host (o el flujo de scripts de inicio) cargue `_content/BQuery/dist/bQuery.min.js`.
4. Actualiza los manejadores de eventos de redimensionamiento a `OnResizeAsync(ResizeEventArgs args)` / `OnResize(ResizeEventArgs args)`.

## Instalación

```bash
dotnet add package BQuery
```

## Guía rápida de inicio

### 1. Registrar servicios

```csharp
builder.Services.AddBQuery();
```

> [!NOTE]
> Si utilizas el modo de renderizado **Interactive Auto**, el servicio debe registrarse tanto en `App` como en `App.Client` simultáneamente.

### 2. Cargar el script

Agrega este script a tu página de host:

```html
<script src="_content/BQuery/dist/bQuery.min.js"></script>
```

Los ejemplos del repositorio utilizan la compilación UMD (`bQuery.min.js`).  

#### Ejemplo WASM (`wwwroot/index.html`)

```html
<script src="_content/BQuery/dist/bQuery.min.js"></script>
<script src="_framework/blazor.webassembly.js"></script>
```

#### Ejemplo de servidor (`Pages/_Host.cshtml`)

```html
<script src="_content/BQuery/dist/bQuery.min.js"></script>
<script src="_framework/blazor.server.js"></script>
```

#### Modo Auto (App.razor)

```html
<script src="_content/BQuery/dist/bQuery.min.js"></script>
<script src="@Assets["_framework/blazor.web.js"]"></script>
```

### 3. Utilizar `Bq` en los componentes

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
        // Si estás escuchando eventos de ventana
        await bq.RemoveWindowEventListeners();
    }
}
```

## Usos comunes

### Viewport

```csharp
var width = await bq.Viewport.GetWidthAsync();
var height = await bq.Viewport.GetHeightAsync();
var scrollTop = await bq.Viewport.GetScrollTopAsync();
```

### APIs de elementos (extensiones de ElementReference)

```csharp
var size = await element.GetWidthAndHeightAsync();
var docPos = await element.GetPositionInDocAsync();

await element.AddCls("is-active");
await element.Css("display", "none");
await element.RemoveCls("is-active");
```

### Arrastrar

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

### Agente de usuario

```csharp
var ua1 = await bq.GetUserAgentAsync();
var ua2 = await jsRuntime.GetUserAgentAsync(); // Extensión de IJSRuntime
```

## Documentación de la API

- API de C#: [API.md](./API.md)
- API de TypeScript: [TS-API.md](./TS-API.md)

## Ejemplos

- `Sample/BQuery.Sample.Wasm`
- `Sample/BQuery.Sample.Server`
- `Sample/BQuery.Sample.Common`
- `Sample/BlazorAppAuto`

## Licencia

MIT
