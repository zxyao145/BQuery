# `Window.bQuery` API

Source: [BQuery/wwwroot/src/bQuery.ts](/mnt/d/source/repos/BQuery/BQuery/wwwroot/src/bQuery.ts)

## Initialization

`window.bQuery` is assigned only when `window.bqInit()` runs.

- On script load, `window.bqInit` is set to a one-time initializer.
- When called, it assigns `window.bQuery`, logs `"bQuery is Ready"`, binds internal events, and sets `window.bqInit` back to `null`.

Relevant sources:

- [BQuery/wwwroot/src/bQuery.ts](/mnt/d/source/repos/BQuery/BQuery/wwwroot/src/bQuery.ts)
- [BQuery/wwwroot/src/global.ts](/mnt/d/source/repos/BQuery/BQuery/wwwroot/src/global.ts)

## Shape

```ts
interface WindowBQuery {
  version: string;

  getWidth(element: HTMLElement, outer: boolean): number;
  getHeight(element: HTMLElement, outer: boolean): number;
  getWidthAndHeight(element: HTMLElement, outer: boolean): [number, number];

  getScrollWidth(element: HTMLElement): number;
  getScrollHeight(element: HTMLElement): number;
  getScrollWidthAndHeight(element: HTMLElement): [number, number];

  getScrollLeft(element: HTMLElement): number;
  getScrollTop(element: HTMLElement): number;
  getScrollLeftAndTop(element: HTMLElement): [number, number];

  getPositionInViewport(element: HTMLElement): {
    x: number;
    y: number;
    width: number;
    height: number;
  };
  getElementLeftInDoc(element: HTMLElement): number;
  getElementTopInDoc(element: HTMLElement): number;
  getPositionInDoc(element: HTMLElement): {
    x: number;
    y: number;
    width: number;
    height: number;
  };

  focus(element: HTMLElement): void;

  bindDrag(
    dom: HTMLElement,
    options: {
      inViewport: boolean;
      dragElement: HTMLElement | string | null;
    }
  ): void;
  addDraggable(
    trigger: HTMLElement | string,
    container: HTMLElement | string,
    dragInViewport?: boolean
  ): void;
  removeDraggable(trigger: HTMLElement | string): void;
  resetDraggableElePosition(trigger: HTMLElement | string): void;

  domHelper: {
    getDom(element: string | Element | null): Element | null;
    attr(selector: string | Element, key: string, value?: string | null): string | null;
    addCls(selector: Element | string, className: string | string[]): void;
    removeCls(selector: Element | string, clsName: string | string[]): void;
    css(element: HTMLElement, name: string | object, value?: string | null): void;
  };

  viewport: {
    getWidth(): number;
    getHeight(): number;
    getWidthAndHeight(): [number, number];
    getScrollWidth(): number;
    getScrollHeight(): number;
    getScrollWidthAndHeight(): [number, number];
    getScrollLeft(): number;
    getScrollTop(): number;
    getScrollLeftAndTop(): [number, number];
    getScrollDistToTop(): number;
    getScrollDistToBottom(): number;
  };

  throttle(fn: Function, threshold?: number): (...args: any[]) => void;
  debounce(fn: Function, wait?: number): (...args: any[]) => void;

  getUserAgent(): string;
}
```

## Top-Level Properties

### `version: string`

Current implementation value: `"3.1.0"`.

Source:

- [BQuery/wwwroot/src/bQuery.ts](/mnt/d/source/repos/BQuery/BQuery/wwwroot/src/bQuery.ts)

### `domHelper`

Namespace exposing DOM utility functions.

Source:

- [BQuery/wwwroot/src/module/domHelper.ts](/mnt/d/source/repos/BQuery/BQuery/wwwroot/src/module/domHelper.ts)

### `viewport`

Viewport and document measurement helpers exposed as a static class object.

Source:

- [BQuery/wwwroot/src/module/Viewport.ts](/mnt/d/source/repos/BQuery/BQuery/wwwroot/src/module/Viewport.ts)

### `throttle(fn, threshold?)`

Returns a throttled wrapper function. Default threshold is `160`.

Source:

- [BQuery/wwwroot/src/module/common.ts](/mnt/d/source/repos/BQuery/BQuery/wwwroot/src/module/common.ts)

### `debounce(fn, wait?)`

Returns a debounced wrapper function. Default wait is `1000`.

Source:

- [BQuery/wwwroot/src/module/common.ts](/mnt/d/source/repos/BQuery/BQuery/wwwroot/src/module/common.ts)

### `getUserAgent()`

Returns `navigator.userAgent`.

Source:

- [BQuery/wwwroot/src/bQuery.ts](/mnt/d/source/repos/BQuery/BQuery/wwwroot/src/bQuery.ts)

## Element Measurement APIs

These methods are spread onto `window.bQuery` from `HtmlElementHelper`.

Source:

- [BQuery/wwwroot/src/module/HtmlElementHelper.ts](/mnt/d/source/repos/BQuery/BQuery/wwwroot/src/module/HtmlElementHelper.ts)

### `getWidth(element, outer)`

Returns element width.

- `outer = true`: uses `element.offsetWidth`
- `outer = false`: uses `element.clientWidth`

### `getHeight(element, outer)`

Returns element height.

- `outer = true`: uses `element.offsetHeight`
- `outer = false`: uses `element.clientHeight`

### `getWidthAndHeight(element, outer)`

Returns `[width, height]`.

### `getScrollWidth(element)`

Returns `element.scrollWidth`.

### `getScrollHeight(element)`

Returns `element.scrollHeight`.

### `getScrollWidthAndHeight(element)`

Returns `[scrollWidth, scrollHeight]`.

### `getScrollLeft(element)`

Returns `element.scrollLeft`.

### `getScrollTop(element)`

Returns `element.scrollTop`.

### `getScrollLeftAndTop(element)`

Returns `[scrollLeft, scrollTop]`.

### `getPositionInViewport(element)`

Returns the bounding rectangle in viewport coordinates:

```ts
{
  x: number;
  y: number;
  width: number;
  height: number;
}
```

This is derived from `element.getBoundingClientRect()`.

### `getElementLeftInDoc(element)`

Returns the element's left offset relative to the document by walking the `offsetParent` chain.

### `getElementTopInDoc(element)`

Returns the element's top offset relative to the document by walking the `offsetParent` chain.

### `getPositionInDoc(element)`

Returns document-relative position and outer size:

```ts
{
  x: number;
  y: number;
  width: number;
  height: number;
}
```

### `focus(element)`

Calls `element.focus()`.

Note: the source marks this as obsolete because newer Blazor versions provide native support.

## Drag APIs

These methods are spread onto `window.bQuery` from `DragHelper`.

Source:

- [BQuery/wwwroot/src/module/DragHelper.ts](/mnt/d/source/repos/BQuery/BQuery/wwwroot/src/module/DragHelper.ts)

### `addDraggable(trigger, container, dragInViewport = true)`

Makes `container` draggable and uses `trigger` as the drag handle.

Parameters:

- `trigger`: `HTMLElement` or CSS selector string
- `container`: `HTMLElement` or CSS selector string
- `dragInViewport`: when `true`, dragging is clamped to the viewport

### `removeDraggable(trigger)`

Unbinds drag handlers associated with the given trigger.

### `resetDraggableElePosition(trigger)`

Restores the dragged container's original inline `style` captured on first drag.

### `bindDrag(dom, options)`

Convenience wrapper around `addDraggable`:

```ts
bindDrag(dom, {
  inViewport: boolean,
  dragElement: HTMLElement | string | null
})
```

Behavior:

- `dom` is the draggable container.
- `options.dragElement` is the drag handle.
- `options.inViewport` controls viewport clamping.

## `domHelper` Namespace

Source:

- [BQuery/wwwroot/src/module/domHelper.ts](/mnt/d/source/repos/BQuery/BQuery/wwwroot/src/module/domHelper.ts)

### `domHelper.getDom(element)`

Resolves a selector or returns the element directly.

- If `element` is a string, uses `document.querySelector(element)`.
- Otherwise returns the passed element.

### `domHelper.attr(selector, key, value?)`

Gets or sets an attribute.

- If `value` is truthy, sets the attribute and returns `value`.
- Otherwise returns `getAttribute(key)`.
- Returns `null` if the target element is not found.

Note: because the implementation checks `if (value)`, falsy values such as `""` are treated as reads, not writes.

### `domHelper.addCls(selector, className)`

Adds one or more CSS classes.

- `className` may be a string or `string[]`.

### `domHelper.removeCls(selector, clsName)`

Removes one or more CSS classes.

- `clsName` may be a string or `string[]`.

### `domHelper.css(element, name, value?)`

Sets inline styles.

Supported forms:

```ts
domHelper.css(el, "color", "red")
domHelper.css(el, { color: "red", display: "block" })
domHelper.css(el, "color:red;display:block")
```

Notes:

- There is no getter form.
- When `name` is a string and `value` is `null` or omitted, the string is parsed as a semicolon-delimited inline style declaration list.

## `viewport` Namespace

Source:

- [BQuery/wwwroot/src/module/Viewport.ts](/mnt/d/source/repos/BQuery/BQuery/wwwroot/src/module/Viewport.ts)

### `viewport.getWidth()`

Returns `document.documentElement.clientWidth`.

### `viewport.getHeight()`

Returns `document.documentElement.clientHeight`.

### `viewport.getWidthAndHeight()`

Returns `[width, height]`.

### `viewport.getScrollWidth()`

Returns `document.documentElement.scrollWidth`.

### `viewport.getScrollHeight()`

Returns `document.documentElement.scrollHeight`.

### `viewport.getScrollWidthAndHeight()`

Returns `[scrollWidth, scrollHeight]`.

### `viewport.getScrollLeft()`

Returns `document.documentElement.scrollLeft`.

### `viewport.getScrollTop()`

Returns `document.documentElement.scrollTop`.

### `viewport.getScrollLeftAndTop()`

Returns `[scrollLeft, scrollTop]`.

### `viewport.getScrollDistToTop()`

Returns the rounded current scroll distance from the top of the page.

### `viewport.getScrollDistToBottom()`

Returns the rounded remaining scroll distance to the bottom of the page.
