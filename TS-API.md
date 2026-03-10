# `window.bQuery` TypeScript API

This document reflects the current source under `src/BQuery/wwwroot/src`.

Primary sources:

- `src/BQuery/wwwroot/src/index.ts`
- `src/BQuery/wwwroot/src/module/Viewport.ts`
- `src/BQuery/wwwroot/src/module/HtmlElementHelper.ts`
- `src/BQuery/wwwroot/src/module/DragHelper.ts`
- `src/BQuery/wwwroot/src/module/eventHelper.ts`
- `src/BQuery/wwwroot/src/module/domHelper.ts`
- `src/BQuery/wwwroot/src/module/common.ts`

## Initialization

When the script loads, it assigns `window.bQuery`.

Current version constant:

- `version = "4.0.0"`

## Root shape

```ts
type ViewportNamespace = typeof import("./module/Viewport").default;
type DragNamespace = typeof import("./module/DragHelper");
type WindowEventsNamespace = typeof import("./module/eventHelper");
type DomHelperNamespace = typeof import("./module/domHelper");
type ElementExtensionsNamespace = Record<string, (...args: any[]) => any>;

interface WindowBQuery {
  version: string;

  viewport: ViewportNamespace;
  drag: DragNamespace;
  windowEvents: WindowEventsNamespace;
  domHelper: DomHelperNamespace;
  elementExtensions: ElementExtensionsNamespace;
  navigator: {
    getUserAgent(): string;
  };

  throttle(fn: Function, threshold?: number): (...args: any[]) => void;
  debounce(fn: Function, wait?: number): (...args: any[]) => void;
}
```

Only `window.bQuery` is assigned by the package at runtime.

## `viewport` namespace

From `module/Viewport.ts`.

| Method | Returns |
| --- | --- |
| `getWidth()` | `number` |
| `getHeight()` | `number` |
| `getWidthAndHeight()` | `[number, number]` |
| `getScrollWidth()` | `number` |
| `getScrollHeight()` | `number` |
| `getScrollWidthAndHeight()` | `[number, number]` |
| `getScrollLeft()` | `number` |
| `getScrollTop()` | `number` |
| `getScrollLeftAndTop()` | `[number, number]` |
| `getScrollDistToTop()` | `number` |
| `getScrollDistToBottom()` | `number` |

Notes:

- Width/height and scroll values are read from `document.documentElement`.
- Distance methods return rounded values.

## `elementExtensions` namespace

From `module/HtmlElementHelper.ts`.

| Method | Returns | Notes |
| --- | --- | --- |
| `getWidth(element, outer)` | `number` | `outer=true` uses `offsetWidth`, else `clientWidth`. |
| `getHeight(element, outer)` | `number` | `outer=true` uses `offsetHeight`, else `clientHeight`. |
| `getWidthAndHeight(element, outer)` | `[number, number]` | |
| `getScrollWidth(element)` | `number` | |
| `getScrollHeight(element)` | `number` | |
| `getScrollWidthAndHeight(element)` | `[number, number]` | |
| `getScrollLeft(element)` | `number` | |
| `getScrollTop(element)` | `number` | |
| `getScrollLeftAndTop(element)` | `[number, number]` | |
| `getPositionInViewport(element)` | `{ x: number; y: number; width: number; height: number }` | Uses `getBoundingClientRect()`. |
| `getElementLeftInDoc(element)` | `number` | Walks `offsetParent` chain. |
| `getElementTopInDoc(element)` | `number` | Walks `offsetParent` chain. |
| `getPositionInDoc(element)` | `{ x: number; y: number; width: number; height: number }` | Returns zeros if element is null/undefined. |
| `focus(element)` | `void` | Kept for compatibility. |

## `drag` namespace

From `module/DragHelper.ts`.

### Types

```ts
interface DragOptions {
  inViewport?: boolean;               // default true
  dragElement?: HTMLElement | string | null;
}
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `bindDrag(dom: HTMLElement, options?: DragOptions | null)` | `void` | Binds drag to `dom`. Uses `options.dragElement ?? dom` as trigger. |
| `addDraggable(trigger: HTMLElement | string | null, container: HTMLElement | string, dragInViewport?: boolean)` | `void` | Low-level binding helper. |
| `removeDraggable(trigger: HTMLElement | string)` | `void` | Unbinds and removes dragger for trigger. |
| `resetDraggableElePosition(trigger: HTMLElement | string)` | `void` | Restores container style captured at first drag init. |

Behavior notes:

- Uses Pointer Events (`pointerdown`, `pointermove`, `pointerup`, `pointercancel`).
- Movement is throttled (`10ms`) and can be clamped to viewport bounds.

## `windowEvents` namespace

From `module/eventHelper.ts`.

### Types

```ts
interface EventInfo {
  name: string;
}

interface DotNetEventSink {
  invokeMethodAsync(methodIdentifier: string, ...args: unknown[]): Promise<unknown>;
}
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `addWindowEventListener(evt: EventInfo, listenerId: string, dotNetRef: DotNetEventSink)` | `void` | Adds one event for one listener ID. |
| `removeWindowEventListener(evt: EventInfo, listenerId: string)` | `void` | Removes one event for one listener ID. |
| `addWindowEventsListener(events: EventInfo[], listenerId: string, dotNetRef: DotNetEventSink)` | `void` | Adds many events at once; supports `"*"` wildcard to bind all supported events. |
| `removeWindowEventsListener(events: EventInfo[], listenerId: string)` | `void` | Removes many events at once; supports `"*"` wildcard. |
| `disposeWindowEventsListener(listenerId: string)` | `void` | Removes all event bindings for a listener ID. |

Supported event names:

- `mousedown`
- `mouseup`
- `click`
- `dblclick`
- `mouseover`
- `mouseout`
- `mousemove`
- `contextmenu`
- `resize`
- `scroll`
- `focus`
- `blur`
- `touchstart`
- `touchmove`
- `touchend`
- `touchcancel`
- `keydown`
- `keypress`
- `keyup`

Interop notes:

- `resize`, `scroll`, and some high-frequency events are throttled (`50ms` default in this module).
- Events are converted to Blazor-friendly payloads and forwarded through `dotNetRef.invokeMethodAsync(...)`.

## `domHelper` namespace

From `module/domHelper.ts`.

| Method | Returns | Description |
| --- | --- | --- |
| `getDom(element: string | Element)` | `Element` | Resolves CSS selector or returns element; throws if not found. |
| `attr(selector: string | Element, key: string, value?: string | null)` | `string | null` | Legacy attribute get/set helper. |
| `setAttr(selector: string | Element, key: string, value: string)` | `void` | Sets an attribute. |
| `getAttr(selector: string | Element, key: string)` | `string | null` | Gets an attribute value. |
| `removeAttr(selector: string | Element, key: string)` | `void` | Removes an attribute. |
| `addCls(selector: string | Element, className: string | string[])` | `void` | Adds one or more classes. |
| `removeCls(selector: string | Element, clsName: string | string[])` | `void` | Removes one or more classes. |
| `css(element: HTMLElement, name: string | object, value?: string | null)` | `void` | Sets inline styles. |

`css(...)` supported forms:

```ts
domHelper.css(el, "color", "red");
domHelper.css(el, { color: "red", display: "block" });
domHelper.css(el, "color:red;display:block");
```

`attr(...)` note:

- The current implementation checks `if (value)`, so falsy values (such as `""`) are treated as read mode instead of write mode.
- Prefer `setAttr/getAttr/removeAttr` for explicit attribute operations.

## `navigator`

| Member | Returns |
| --- | --- |
| `navigator.getUserAgent()` | `string` |

## `throttle` and `debounce`

From `module/common.ts`.

| Function | Default | Description |
| --- | --- | --- |
| `throttle(fn, threshold?)` | `threshold = 160` | Executes at a controlled rate and also schedules a trailing call. |
| `debounce(fn, wait?)` | `wait = 1000` | Delays execution until calls stop for `wait` ms. |

Additional note:

- The function returned by `throttle` includes a `cancel()` function used by drag/event cleanup code.
