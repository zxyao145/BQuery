# BQuery API Reference

This document is derived from the public C# surface under `BQuery/*.cs`.

## Overview

`BQuery` is a Blazor helper library that exposes:

- `Bq` static helpers for browser and element operations
- `Bq.Viewport` for viewport measurements
- `Bq.Events` for window-level browser events
- `DomHelper` for DOM attribute, class, and style mutation
- `UseBQuery(...)` startup extensions for WebAssembly and Server hosting

Most APIs are asynchronous and optionally accept an `IJSRuntime`. When `jsRuntime` is omitted, BQuery resolves it from its initialization path.

## Setup

### `WebAssemblyHostExtension`

```csharp
public static class WebAssemblyHostExtension
```

Methods:

| Member | Description |
| --- | --- |
| `WebAssemblyHost UseBQuery(this WebAssemblyHost webAssemblyHost)` | Initializes BQuery for Blazor WebAssembly. |
| `IHost UseBQuery(this IHost webAssemblyHost)` | Initializes BQuery for Blazor Server hosting. |

Typical usage:

```csharp
await builder.Build()
    .UseBQuery()
    .RunAsync();
```

## `Bq`

```csharp
public static class Bq
```

Properties:

| Member | Type | Description |
| --- | --- | --- |
| `IsServerSide` | `bool` | Indicates whether BQuery was initialized in server mode. |
| `JsRuntime` | `IJSRuntime` | Explicit runtime slot used by server-side initialization scenarios. |
| `Viewport` | `BqViewport` | Singleton viewport helper. |
| `Events` | `BqEvents` | Singleton window event hub. |

Static methods:

| Member | Returns | Description |
| --- | --- | --- |
| `GetUserAgentAsync(IJSRuntime jsRuntime = null)` | `Task<string>` | Returns `navigator.userAgent`. |

### `ElementReference` extension methods on `Bq`

All members below are declared as extension methods on `ElementReference`.

| Member | Returns | Description |
| --- | --- | --- |
| `GetWidthAsync(bool isOuter = true, IJSRuntime jsRuntime = null)` | `Task<double>` | Gets element width. `isOuter` selects outer vs. inner measurement. |
| `GetHeightAsync(bool isOuter = true, IJSRuntime jsRuntime = null)` | `Task<double>` | Gets element height. `isOuter` selects outer vs. inner measurement. |
| `GetWidthAndHeightAsync(bool isOuter = true, IJSRuntime jsRuntime = null)` | `Task<double[]>` | Returns `[width, height]`. |
| `GetScrollWidthAsync(IJSRuntime jsRuntime = null)` | `Task<double>` | Gets scrollable width. |
| `GetScrollHeightAsync(IJSRuntime jsRuntime = null)` | `Task<double>` | Gets scrollable height. |
| `GetScrollWidthAndHeightAsync(IJSRuntime jsRuntime = null)` | `Task<double[]>` | Returns `[scrollWidth, scrollHeight]`. |
| `GetScrollLeftAsync(IJSRuntime jsRuntime = null)` | `Task<double>` | Gets horizontal scroll offset. |
| `GetScrollTopAsync(IJSRuntime jsRuntime = null)` | `Task<double>` | Gets vertical scroll offset. |
| `GetScrollLeftAndTopAsync(IJSRuntime jsRuntime = null)` | `Task<double[]>` | Returns `[left, top]`. |
| `GetPositionInViewportAsync(IJSRuntime jsRuntime = null)` | `Task<ElePosition>` | Gets element position relative to the viewport. |
| `GetPositionInDocAsync(IJSRuntime jsRuntime = null)` | `Task<ElePosition>` | Gets element position relative to the document. |
| `BindDragAsync(DragOptions options = null, IJSRuntime jsRuntime = null)` | `Task` | Enables dragging for the element using the provided options. |

## `BqViewport`

```csharp
public class BqViewport
```

Access this instance through `Bq.Viewport`.

Methods:

| Member | Returns | Description |
| --- | --- | --- |
| `GetWidthAsync(IJSRuntime jsRuntime = null)` | `Task<double>` | Gets viewport width. |
| `GetHeightAsync(IJSRuntime jsRuntime = null)` | `Task<double>` | Gets viewport height. |
| `GetWidthAndHeightAsync(IJSRuntime jsRuntime = null)` | `Task<double[]>` | Returns `[width, height]`. |
| `GetScrollWidthAsync(IJSRuntime jsRuntime = null)` | `Task<double>` | Gets document scroll width. |
| `GetScrollHeightAsync(IJSRuntime jsRuntime = null)` | `Task<double>` | Gets document scroll height. |
| `GetScrollWidthAndHeightAsync(IJSRuntime jsRuntime = null)` | `Task<double[]>` | Returns `[scrollWidth, scrollHeight]`. |
| `GetScrollLeftAsync(IJSRuntime jsRuntime = null)` | `Task<double>` | Gets viewport horizontal scroll offset. |
| `GetScrollTopAsync(IJSRuntime jsRuntime = null)` | `Task<double>` | Gets viewport vertical scroll offset. |
| `GetScrollLeftAndTopAsync(IJSRuntime jsRuntime = null)` | `Task<double[]>` | Returns `[left, top]`. |

## `BqEvents`

```csharp
public class BqEvents
```

Access this instance through `Bq.Events`.

Each browser event is exposed in two forms:

- synchronous `Action<...>` event
- asynchronous `Func<..., Task>` event

Events:

| Event | Handler type | Browser event |
| --- | --- | --- |
| `OnResize` | `Action<double, double>` | `window.onresize` |
| `OnResizeAsync` | `Func<double, double, Task>` | `window.onresize` |
| `OnScroll` | `Action<EventArgs>` | `window.onscroll` |
| `OnScrollAsync` | `Func<EventArgs, Task>` | `window.onscroll` |
| `OnMouseOver` | `Action<MouseEventArgs>` | `window.onmouseover` |
| `OnMouseOverAsync` | `Func<MouseEventArgs, Task>` | `window.onmouseover` |
| `OnMouseOut` | `Action<MouseEventArgs>` | `window.onmouseout` |
| `OnMouseOutAsync` | `Func<MouseEventArgs, Task>` | `window.onmouseout` |
| `OnContextMenu` | `Action<MouseEventArgs>` | `window.oncontextmenu` |
| `OnContextMenuAsync` | `Func<MouseEventArgs, Task>` | `window.oncontextmenu` |
| `OnMouseDown` | `Action<MouseEventArgs>` | `window.onmousedown` |
| `OnMouseDownAsync` | `Func<MouseEventArgs, Task>` | `window.onmousedown` |
| `OnMouseUp` | `Action<MouseEventArgs>` | `window.onmouseup` |
| `OnMouseUpAsync` | `Func<MouseEventArgs, Task>` | `window.onmouseup` |
| `OnMouseMove` | `Action<MouseEventArgs>` | `window.onmousemove` |
| `OnMouseMoveAsync` | `Func<MouseEventArgs, Task>` | `window.onmousemove` |
| `OnDbClick` | `Action<MouseEventArgs>` | `window.ondblclick` |
| `OnDbClickAsync` | `Func<MouseEventArgs, Task>` | `window.ondblclick` |
| `OnClick` | `Action<MouseEventArgs>` | `window.onclick` |
| `OnClickAsync` | `Func<MouseEventArgs, Task>` | `window.onclick` |
| `OnClose` | `Action<EventArgs>` | `window.onclose` |
| `OnCloseAsync` | `Func<EventArgs, Task>` | `window.onclose` |
| `OnFocus` | `Action<FocusEventArgs>` | `window.onfocus` |
| `OnFocusAsync` | `Func<FocusEventArgs, Task>` | `window.onfocus` |
| `OnBlur` | `Action<FocusEventArgs>` | `window.onblur` |
| `OnBlurAsync` | `Func<FocusEventArgs, Task>` | `window.onblur` |
| `OnTouchStart` | `Action<TouchEventArgs>` | `window.ontouchstart` |
| `OnTouchStartAsync` | `Func<TouchEventArgs, Task>` | `window.ontouchstart` |
| `OnTouchMove` | `Action<TouchEventArgs>` | `window.ontouchmove` |
| `OnTouchMoveAsync` | `Func<TouchEventArgs, Task>` | `window.ontouchmove` |
| `OnTouchEnd` | `Action<TouchEventArgs>` | `window.ontouchend` |
| `OnTouchEndAsync` | `Func<TouchEventArgs, Task>` | `window.ontouchend` |
| `OnTouchCancel` | `Action<TouchEventArgs>` | `window.ontouchcancel` |
| `OnTouchCancelAsync` | `Func<TouchEventArgs, Task>` | `window.ontouchcancel` |
| `OnKeyDown` | `Action<KeyboardEventArgs>` | `window.onkeydown` |
| `OnKeyDownAsync` | `Func<KeyboardEventArgs, Task>` | `window.onkeydown` |
| `OnKeyPress` | `Action<KeyboardEventArgs>` | `window.onkeypress` |
| `OnKeyPressAsync` | `Func<KeyboardEventArgs, Task>` | `window.onkeypress` |
| `OnKeyUp` | `Action<KeyboardEventArgs>` | `window.onkeyup` |
| `OnKeyUpAsync` | `Func<KeyboardEventArgs, Task>` | `window.onkeyup` |

Example:

```csharp
Bq.Events.OnResize += (width, height) =>
{
    Console.WriteLine($"Viewport: {width} x {height}");
};
```

## `DomHelper`

```csharp
public class DomHelper
```

`DomHelper` provides imperative DOM mutation helpers.

Methods:

| Member | Returns | Description |
| --- | --- | --- |
| `Attr(ElementReference element, string key, string? value = null, IJSRuntime? jsRuntime = null)` | `Task` | Sets or updates an attribute. |
| `AddCls(ElementReference element, string className, IJSRuntime? jsRuntime = null)` | `Task` | Adds one CSS class. |
| `AddCls(ElementReference element, List<string> classNames, IJSRuntime? jsRuntime = null)` | `Task` | Adds multiple CSS classes. |
| `RemoveCls(ElementReference element, string className, IJSRuntime? jsRuntime = null)` | `Task` | Removes one CSS class. |
| `RemoveCls(ElementReference element, List<string> classNames, IJSRuntime? jsRuntime = null)` | `Task` | Removes multiple CSS classes. |
| `AddCss(ElementReference element, string name, string value, IJSRuntime? jsRuntime = null)` | `Task` | Sets a CSS property. |
| `RemoveCss(ElementReference element, string name, IJSRuntime? jsRuntime = null)` | `Task` | Removes a CSS property. |
| `Css(ElementReference element, string name, string? value = null, IJSRuntime? jsRuntime = null)` | `Task` | General CSS property setter/remover. |

Example:

```csharp
var dom = new DomHelper();
await dom.AddCls(element, "active");
await dom.Css(element, "display", "none");
```

## Data types

### `ElePosition`

```csharp
public class ElePosition
```

Properties:

| Property | Type | Description |
| --- | --- | --- |
| `X` | `double` | X coordinate. |
| `Y` | `double` | Y coordinate. |
| `Width` | `double` | Element width. |
| `Height` | `double` | Element height. |

### `DragOptions`

```csharp
public class DragOptions
```

Properties:

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `InViewport` | `bool` | `true` | Constrains dragging to the viewport. |
| `DragElement` | `ElementReference?` | `null` | Optional handle element that triggers drag behavior. |

## JS interop support types

These types are public in the assembly, but they are primarily infrastructure for the JS bridge rather than normal app entry points.

### `BqInterop`

```csharp
public static class BqInterop
```

`BqInterop` exposes `[JSInvokable]` callbacks used by the JavaScript side to forward browser events into `Bq.Events`.

Public callback methods:

- `WindowDbClick(MouseEventArgs e)`
- `WindowClick(MouseEventArgs e)`
- `WindowContextMenu(MouseEventArgs e)`
- `WindowMouseDown(MouseEventArgs e)`
- `WindowMouseUp(MouseEventArgs e)`
- `WindowMouseOver(MouseEventArgs e)`
- `WindowMouseOut(MouseEventArgs e)`
- `WindowMouseMove(MouseEventArgs e)`
- `WindowResize(double width, double height)`
- `WindowScroll(EventArgs e)`
- `WindowClose(EventArgs e)`
- `WindowFocus(FocusEventArgs e)`
- `WindowBlur(FocusEventArgs e)`
- `WindowTouchStart(TouchEventArgs e)`
- `WindowTouchMove(TouchEventArgs e)`
- `WindowTouchEnd(TouchEventArgs e)`
- `WindowTouchCancel(TouchEventArgs e)`
- `WindowKeyDown(KeyboardEventArgs e)`
- `WindowKeyPress(KeyboardEventArgs e)`
- `WindowKeyUp(KeyboardEventArgs e)`

### `JsInteropConstants`

```csharp
public class JsInteropConstants
```

This class contains the JavaScript function names used internally by BQuery. It is public, but generally only needed when extending or debugging the JS bridge.

Examples:

| Member | Value shape |
| --- | --- |
| `Prefix` | `bQuery.` |
| `ViewportPrefix` | `bQuery.viewport.` |
| `BQueryReady` | `bqInit` |
| `GetUserAgent` | `bQuery.getUserAgent` |
| `BindDrag` | `bQuery.bindDrag` |

## Notes

- All measurement and DOM APIs are async because they execute through JS interop.
- The library exposes both consumer-facing APIs (`Bq`, `BqViewport`, `BqEvents`, `DomHelper`) and public interop plumbing (`BqInterop`, `JsInteropConstants`).
- `Bq.Events` and `Bq.Viewport` are initialized by `UseBQuery(...)`.
