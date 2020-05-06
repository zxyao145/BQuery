#  BQuery

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/BQuery)](https://www.nuget.org/packages/BQuery/)

An extended library of interaction between blazor and js. And The name mimics jQuery.

# Usage

## Add js:

<script src="_content/BQuery/bQuery.js"></script>

## UseBQuery

change 

```c#
await builder.Build().RunAsync();
```

to

```c#
await builder.Build().UseBQuery().RunAsync();
```

in main method.

## using namespace

```c#
using BQuery;
```

# Gif

Window on resize

![onrezise](./files/onresize.gif)

Window on scroll

![onscroll](./files/scroll.gif)

# Api

## `Bq Static member (not contains extension methods)`

| **name**                           | describe                    | return    |
| ---------------------------------- | --------------------------- | --------- |
| `Task<string> GetUserAgentAsync()` | get browser ueragent        | useragent |
| `Viewport`                         | See **Bq.Viewport.*** below | --        |
| `Events`                           | See **Bq.Events.*** below   | --        |

## `Bq.Viewport.*`

| name                                            | describe                             | return          |
| ----------------------------------------------- | ------------------------------------ | --------------- |
| `Task<double> Bq.Viewport.GetWidthAsync()`      | get viewport width                   | width           |
| `Task<double> GetHeightAsync()`                 | get viewport height                  | height          |
| `Task<double[]> GetWidthAndHeightAsync()`       | get viewport width and height        | [width, height] |
| `Task<double> GetScrollWidthAsync()`            | get viewport scroll width            | width           |
| `GetScrollHeightAsync()`                        | get viewport scroll height           | height          |
| `Task<double[]> GetScrollWidthAndHeightAsync()` | get viewport scroll width and height | [width, height] |
| `Task<double> GetScrollLeftAsync()`             | get viewport scroll left             | left            |
| `Task<double> GetScrollTopAsync()`              | get viewport scroll top              | top             |
| `Task<double[]> GetScrollLeftAndTopAsync()`     | get viewport scroll left and top     | [left, top]     |

## ElementReference Extension methods

note: all the method not show the first patameter:  *this ElementReference element*

| name                                                         | describe                            | parameters                                       | return             |
| ------------------------------------------------------------ | ----------------------------------- | ------------------------------------------------ | ------------------ |
| `Task<double> GetWidthAsync(bool isOuter = true)`            | get element width                   | the width is outerwidth or innerwidth            | width              |
| `Task<double> GetHeightAsync(bool isOuter = true)`           | get element height                  | the height is outerheight or innerheight         | height             |
| `Task<double[]> GetWidthAndHeightAsync(bool isOuter = true)` | get element width and height        | the width and height is outerwidth or innerwidth | [width, height]    |
| `Task<double> GetScrollWidthAsync()`                         | get element scroll width            | --                                               | width              |
| `Task<double> GetScrollHeightAsync()`                        | get element scroll height           | --                                               | height             |
| `Task<double[]> GetScrollWidthAndHeightAsync(this ElementReference element)` | get element scroll width and height | --                                               | [width, height]    |
| `Task<double> GetScrollLeftAsync()`                          | get element scroll left             | --                                               | left               |
| `Task<double> GetScrollTopAsync()`                           | get element scroll top              | --                                               | top                |
| `Task<double[]> GetScrollLeftAndTopAsync()`                  | get element scroll left and top     | --                                               | [left, top]        |
| `Task<ElePosition> GetPositionInViewport()`                  | get element position in Viewport    | --                                               | ElePosition object |
| `Task<ElePosition> GetPositionInDoc()`                       | get element position in document    | --                                               | ElePosition object |

## `Bq.Events`.*

| name                         | describe                     | parameters        |
| ---------------------------- | ---------------------------- | ----------------- |
| `event Action OnResize`      | window.onresize event.       | 1: width 2:height |
| `event Func OnResizeAsync`   | async window.onresize event. | 1: width 2:height |
| `event Action OnScroll`      | window.onscroll event        | EventArgs         |
| `event Action OnScrollasync` | async window.onscroll event  | EventArgs         |
| `event Action<MouseEventArgs> OnMouseOver` | window.onmouseover event | MouseEventArgs |
| `event Func<MouseEventArgs Task> OnMouseOverAsync` | async window.onmouseover event | MouseEventArgs |
| `event Action<MouseEventArgs> OnMouseOut` | window.onmouseout event | MouseEventArgs |
| `event Func<MouseEventArgs Task> OnMouseOutAsync` | async window.onmouseout event | MouseEventArgs |
| `event Action<MouseEventArgs> OnContextMenu` | window.oncontextmenu event | MouseEventArgs |
| `event Func<MouseEventArgs Task> OnContextMenuAsync` | async window.oncontextmenu event | MouseEventArgs |
| `event Action<MouseEventArgs> OnMouseDown` | window.onmousedown event | MouseEventArgs |
| `event Func<MouseEventArgs Task> OnMouseDownAsync` | async window.onmousedown event | MouseEventArgs |
| `event Action<MouseEventArgs> OnMouseUp` | window.onmouseup event | MouseEventArgs |
| `event Func<MouseEventArgs Task> OnMouseUpAsync` | async window.onmouseup event | MouseEventArgs |
| `event Action<MouseEventArgs> OnMouseMove` | window.onmousemove event | MouseEventArgs |
| `event Func<MouseEventArgs Task> OnMouseMoveAsync` | async window.onmousemove event | MouseEventArgs |
| `event Action<MouseEventArgs> OnDbClick` | window.ondbclick event | MouseEventArgs |
| `event Func<MouseEventArgs Task> OnDbClickAsync` | async window.ondbclick event | MouseEventArgs |
| `event Action<MouseEventArgs> OnClick` | window.onclick event | MouseEventArgs |
| `event Func<MouseEventArgs Task> OnClickAsync` | async window.onclick event | MouseEventArgs |
| `event Action<EventArgs> OnClose` | window.onclose event | EventArgs |
| `event Func<EventArgs Task> OnCloseAsync` | async window.onclose event | EventArgs |
| `event Action<FocusEventArgs> OnFocus` | window.onfocus event | FocusEventArgs |
| `event Func<FocusEventArgs Task> OnFocusAsync` | async window.onfocus event | FocusEventArgs |
| `event Action<FocusEventArgs> OnBlur` | window.onblur event | FocusEventArgs |
| `event Func<FocusEventArgs Task> OnBlurAsync` | async window.onblur event | FocusEventArgs |
| `event Action<TouchEventArgs> OnTouchStart` | window.ontouchstart event | TouchEventArgs |
| `event Func<TouchEventArgs Task> OnTouchStartAsync` | async window.ontouchstart event | TouchEventArgs |
| `event Action<TouchEventArgs> OnTouchMove` | window.ontouchmove event | TouchEventArgs |
| `event Func<TouchEventArgs Task> OnTouchMoveAsync` | async window.ontouchmove event | TouchEventArgs |
| `event Action<TouchEventArgs> OnTouchEnd` | window.ontouchend event | TouchEventArgs |
| `event Func<TouchEventArgs Task> OnTouchEndAsync` | async window.ontouchend event | TouchEventArgs |
| `event Action<TouchEventArgs> OnTouchCancel` | window.ontouchcancel event | TouchEventArgs |
| `event Func<TouchEventArgs Task> OnTouchCancelAsync` | async window.ontouchcancel event | TouchEventArgs |
| `event Action<KeyboardEventArgs> OnKeyDown` | window.onkeydown event | KeyboardEventArgs |
| `event Func<KeyboardEventArgs Task> OnKeyDownAsync` | async window.onkeydown event | KeyboardEventArgs |
| `event Action<KeyboardEventArgs> OnKeyPress` | window.onkeypress event | KeyboardEventArgs |
| `event Func<KeyboardEventArgs Task> OnKeyPressAsync` | async window.onkeypress event | KeyboardEventArgs |
| `event Action<KeyboardEventArgs> OnKeyUp` | window.onkeyup event | KeyboardEventArgs |
| `event Func<KeyboardEventArgs Task> OnKeyUpAsync` | async window.onkeyup event | KeyboardEventArgs |

# Developer

zxyao

# License

MIT
