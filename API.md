# BQuery C# API

This document reflects the current public API in `src/BQuery`.

## Setup

### Service registration

```csharp
builder.Services.AddBQuery();
```

### Script loading

Load one of the distributed scripts on your host page:

```html
<script src="_content/BQuery/dist/bQuery.min.js"></script>
```

`bQuery.min.mjs` is also available if you prefer ES module loading.

## `ServiceExtension`

```csharp
public static class ServiceExtension
```

| Member | Returns | Description |
| --- | --- | --- |
| `AddBQuery(this IServiceCollection services)` | `IServiceCollection` | Registers `BqEvents` and `Bq` as scoped services. |

## `Bq`

```csharp
public partial class Bq
```

Properties:

| Member | Type | Description |
| --- | --- | --- |
| `Viewport` | `BqViewport` | Viewport helper API. |
| `Drag` | `BqDrag` | Drag helper API. |
| `WindowEvents` | `BqEvents` | Window event hub for the current DI scope. |

Methods:

| Member | Returns | Description |
| --- | --- | --- |
| `GetUserAgentAsync()` | `Task<string>` | Returns `navigator.userAgent`. |
| `AddWindowEventListeners(params WindowEvent[] windowEvents)` | `Task` | Registers one or more window events for this scope. |
| `RemoveWindowEventListeners(params WindowEvent[] windowEvents)` | `Task` | Removes selected events; if empty, removes all listeners for this scope. |
| `AddWindowEventListener<T>(WindowEvent windowEvent, Func<T, Task> func)` | `Task` | Registers one async callback directly for a specific event. |
| `RemoveWindowEventListener<T>(WindowEvent windowEvent, Func<T, Task> func)` | `Task` | Removes one async callback; if no handlers remain, unbinds the JS listener for that event. |

## `BqViewport`

```csharp
public partial class BqViewport
```

Access via `bq.Viewport`.

| Member | Returns |
| --- | --- |
| `GetWidthAsync()` | `Task<double>` |
| `GetHeightAsync()` | `Task<double>` |
| `GetWidthAndHeightAsync()` | `Task<double[]>` |
| `GetScrollWidthAsync()` | `Task<double>` |
| `GetScrollHeightAsync()` | `Task<double>` |
| `GetScrollWidthAndHeightAsync()` | `Task<double[]>` |
| `GetScrollLeftAsync()` | `Task<double>` |
| `GetScrollTopAsync()` | `Task<double>` |
| `GetScrollLeftAndTopAsync()` | `Task<double[]>` |

## `BqDrag`

```csharp
public class BqDrag
```

Access via `bq.Drag`.

| Member | Returns | Description |
| --- | --- | --- |
| `BindDragAsync(ElementReference element, DragOptions? options = null)` | `Task` | Enables drag for `element`. |
| `RemoveDragAsync(ElementReference element, DragOptions? options = null)` | `Task` | Removes drag binding for `options.DragElement` if provided; otherwise `element`. |
| `ResetDragPositionAsync(ElementReference element, DragOptions? options = null)` | `Task` | Restores original style/position for the dragged element. |

## `BqEvents`

```csharp
public partial class BqEvents : IAsyncDisposable
```

Access via `bq.WindowEvents`.

Methods:

| Member | Returns | Description |
| --- | --- | --- |
| `AddWindowEventListeners(params WindowEvent[] windowEvents)` | `Task` | Registers one or more events for this scoped listener ID. |
| `RemoveWindowEventListeners(params WindowEvent[] windowEvents)` | `Task` | Removes selected events; if empty, removes all listeners and clears local handlers. |
| `AddWindowEventListener<T>(WindowEvent windowEvent, Func<T, Task> func)` | `Task` | Adds an async callback to an event slot and ensures JS listener binding. |
| `RemoveWindowEventListener<T>(WindowEvent windowEvent, Func<T, Task> func)` | `Task` | Removes one async callback and unbinds JS event when slot count reaches zero. |
| `DisposeAsync()` | `ValueTask` | Unregisters scoped listeners and disposes DotNet reference. |

Generated events:

Each event exists in two forms:

- Sync form: `event Action<TArgs>? OnX`
- Async form: `event Func<TArgs, Task>? OnXAsync`

| Event pair | Args type | Browser event |
| --- | --- | --- |
| `OnMouseDown` / `OnMouseDownAsync` | `MouseEventArgs` | `mousedown` |
| `OnMouseUp` / `OnMouseUpAsync` | `MouseEventArgs` | `mouseup` |
| `OnClick` / `OnClickAsync` | `MouseEventArgs` | `click` |
| `OnDblClick` / `OnDblClickAsync` | `MouseEventArgs` | `dblclick` |
| `OnMouseOver` / `OnMouseOverAsync` | `MouseEventArgs` | `mouseover` |
| `OnMouseOut` / `OnMouseOutAsync` | `MouseEventArgs` | `mouseout` |
| `OnMouseMove` / `OnMouseMoveAsync` | `MouseEventArgs` | `mousemove` |
| `OnContextMenu` / `OnContextMenuAsync` | `MouseEventArgs` | `contextmenu` |
| `OnResize` / `OnResizeAsync` | `ResizeEventArgs` | `resize` |
| `OnScroll` / `OnScrollAsync` | `EventArgs` | `scroll` |
| `OnFocus` / `OnFocusAsync` | `FocusEventArgs` | `focus` |
| `OnBlur` / `OnBlurAsync` | `FocusEventArgs` | `blur` |
| `OnTouchStart` / `OnTouchStartAsync` | `TouchEventArgs` | `touchstart` |
| `OnTouchMove` / `OnTouchMoveAsync` | `TouchEventArgs` | `touchmove` |
| `OnTouchEnd` / `OnTouchEndAsync` | `TouchEventArgs` | `touchend` |
| `OnTouchCancel` / `OnTouchCancelAsync` | `TouchEventArgs` | `touchcancel` |
| `OnKeyDown` / `OnKeyDownAsync` | `KeyboardEventArgs` | `keydown` |
| `OnKeyPress` / `OnKeyPressAsync` | `KeyboardEventArgs` | `keypress` |
| `OnKeyUp` / `OnKeyUpAsync` | `KeyboardEventArgs` | `keyup` |

## `WindowEvent`

```csharp
public readonly struct WindowEvent : IEquatable<WindowEvent>
```

Represents a browser event name used by `BqEvents`.

Key points:

- `Name` is the underlying string value.
- Equality is case-insensitive (`OrdinalIgnoreCase`).

Predefined static fields:

| Field | JavaScript Event Name |
| --- | --- |
| `WindowEvent.OnMouseDown` | `mousedown` |
| `WindowEvent.OnMouseUp` | `mouseup` |
| `WindowEvent.OnClick` | `click` |
| `WindowEvent.OnDblClick` | `dblclick` |
| `WindowEvent.OnMouseOver` | `mouseover` |
| `WindowEvent.OnMouseOut` | `mouseout` |
| `WindowEvent.OnMouseMove` | `mousemove` |
| `WindowEvent.OnContextMenu` | `contextmenu` |
| `WindowEvent.OnResize` | `resize` |
| `WindowEvent.OnScroll` | `scroll` |
| `WindowEvent.OnFocus` | `focus` |
| `WindowEvent.OnBlur` | `blur` |
| `WindowEvent.OnTouchStart` | `touchstart` |
| `WindowEvent.OnTouchMove` | `touchmove` |
| `WindowEvent.OnTouchEnd` | `touchend` |
| `WindowEvent.OnTouchCancel` | `touchcancel` |
| `WindowEvent.OnKeyDown` | `keydown` |
| `WindowEvent.OnKeyPress` | `keypress` |
| `WindowEvent.OnKeyUp` | `keyup` |

## `ElementReferenceExtensions`

```csharp
public static class ElementReferenceExtensions
```

### DOM helper extensions

| Member | Returns | Description |
| --- | --- | --- |
| `[Obsolete] Attr(this ElementReference element, string key, string? value = null)` | `ValueTask` | Legacy attribute helper. Prefer `SetAttr`, `GetAttr`, and `RemoveAttr`. |
| `SetAttr(this ElementReference element, string key, string value)` | `ValueTask` | Sets an attribute value. |
| `GetAttr(this ElementReference element, string key)` | `ValueTask<string?>` | Reads an attribute value. |
| `RemoveAttr(this ElementReference element, string key)` | `ValueTask` | Removes an attribute. |
| `AddCls(this ElementReference element, string className)` | `ValueTask` | Adds one class. |
| `AddCls(this ElementReference element, List<string> classNames)` | `ValueTask` | Adds multiple classes. |
| `RemoveCls(this ElementReference element, string className)` | `ValueTask` | Removes one class. |
| `RemoveCls(this ElementReference element, List<string> classNames)` | `ValueTask` | Removes multiple classes. |
| `AddCss(this ElementReference element, string name, string value)` | `ValueTask` | Sets one CSS property. |
| `RemoveCss(this ElementReference element, string name)` | `ValueTask` | Compatibility helper that forwards to `Css(name, null)`. |
| `Css(this ElementReference element, string name, string? value = null)` | `ValueTask` | General CSS setter helper. |

### Element measurement and position extensions

| Member | Returns |
| --- | --- |
| `GetWidthAsync(this ElementReference element, bool isOuter = true)` | `ValueTask<double>` |
| `GetHeightAsync(this ElementReference element, bool isOuter = true)` | `ValueTask<double>` |
| `GetWidthAndHeightAsync(this ElementReference element, bool isOuter = true)` | `ValueTask<double[]>` |
| `GetScrollWidthAsync(this ElementReference element)` | `ValueTask<double>` |
| `GetScrollHeightAsync(this ElementReference element)` | `ValueTask<double>` |
| `GetScrollWidthAndHeightAsync(this ElementReference element)` | `ValueTask<double[]>` |
| `GetScrollLeftAsync(this ElementReference element)` | `ValueTask<double>` |
| `GetScrollTopAsync(this ElementReference element)` | `ValueTask<double>` |
| `GetScrollLeftAndTopAsync(this ElementReference element)` | `ValueTask<double[]>` |
| `GetPositionInViewportAsync(this ElementReference element)` | `ValueTask<ElePosition>` |
| `GetPositionInDocAsync(this ElementReference element)` | `ValueTask<ElePosition>` |

## `JSRuntimeExtensions`

```csharp
public static class JSRuntimeExtensions
```

| Member | Returns | Description |
| --- | --- | --- |
| `GetUserAgentAsync(this IJSRuntime jsRuntime)` | `Task<string>` | Returns `navigator.userAgent` without resolving `Bq` from DI. |

## Advanced: `JsModuleConstants`

```csharp
public class JsModuleConstants
```

Holds JS interop naming metadata used by source-generated `*Method` constants.

Nested constant groups:

- `Viewport`
- `Dom`
- `ElementExtensions`
- `Drag`
- `WindowEvents`
- `Navigator`

## Notes

- All browser operations are async because they execute via JS interop.
- `Bq` and `BqEvents` are scoped services.
- `UseBQuery(…)` is not part of the public API surface for startup extensions in versions after 4.x.
