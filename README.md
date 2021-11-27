#  1.BQuery

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/BQuery)](https://www.nuget.org/packages/BQuery/)

An extended library of interaction between blazor and js. And The name mimics jQuery.  Now upgrade to .NET 6.

[Live demo]( https://zxyao145.github.io/BQuery/)

## 1.1. Support

1. WASM （friendly）
2. Server mode：（not friendly）
   1. ServerPrerendered
   2. Server

## 1.2. version

| BQuery version | .NET version |
| -------------- | ------------ |
| 3.x            | .NET 6       |
| 2.x            | .NET 5       |

# 2.Usage

## 2.1. Be careful !!!

This is **friendly to WASM** and **not friendly to Server** mode, because the Server mode is used, `IJSRuntime` needs to be injected manually, this is to be compatible with both ServerPrerendered and Server render-mode.

## 2.2.For WASM

### 2.2.1.Add js to wwwroot/index.html

```js
<script src="_content/BQuery/bQuery.min.js"></script>
```

### 2.2.2.Modify the `Main ` method in Program.cs 

change 

```c#
await builder.Build().RunAsync();
```

to

```c#
await builder.Build()
+	.UseBQuery()
	.RunAsync();
```

### 2.2.3.using namespace

```c#
using BQuery;
```

See "**Sample\BQuery.Sample.Wasm**" and "**Sample\BQuery.Sample.Common**" for details.



## 2.3.For server side

### 2.3.1Add js to Pages/_host.html

In server side, you must manually initialize bquery as follows:

```js
<script src="_framework/blazor.server.js" autostart="false"></script>
<script src="_content/BQuery/bQuery.min.js"></script>
<script>
    function start() {
        Blazor.start({})
            .then(() => {
                window.bqInit();
            });
    }
    start();
</script>
```

**Please note** that the `blazor.server.js` set the property of  **`autostart="false"`**.



### 2.3.2.Modify the `Main ` method in Program.cs 

change 

```c#
CreateHostBuilder(args).Build().Run();
```

to

```c#
CreateHostBuilder(args).Build()
+                .UseBQuery()
                .Run();
```


### 2.3.3.inject `IJSRuntime`

To get the `IJSRuntime` in the context of mounting DOM, you must inject `IJSRuntime` in your component and set the optional parameter (named jsRuntime) of the method as follows:

```
@inject IJSRuntime JsRuntime

....

await draggable.BindDragAsync(dargOptions, jsRuntime: JsRuntime);

```

### 2.3.4.using namespace

```c#
using BQuery;
```

See "**Sample\BQuery.Sample.Server**" and "**Sample\BQuery.Sample.Common**" for details.



# 3.Gif

Window on resize

![onrezise](./files/onresize.gif)

Window on scroll

![onscroll](./files/scroll.gif)

# 4.Api

## 4.1.`Bq Static member (not contains extension methods)`

| **name**                           | describe                    | return    |
| ---------------------------------- | --------------------------- | --------- |
| `Task<string> GetUserAgentAsync()` | get browser ueragent        | useragent |
| `Viewport`                         | See **Bq.Viewport.*** below | --        |
| `Events`                           | See **Bq.Events.*** below   | --        |

## 4.2.`Bq.Viewport.*`

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

## 4.3.ElementReference Extension methods

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
| `Task<ElePosition> GetPositionInViewportAsync()`             | get element position in Viewport    | --                                               | ElePosition object |
| `Task<ElePosition> GetPositionInDocAsync()`                  | get element position in document    | --                                               | ElePosition object |
| ~~`Task FocusAsync()`~~   | focus element                       | Blazor already has a native implementation       | --                 |
| `Task BindDragAsync(DragOptions options = null)`             | Allow element drag                  | DragOptions                                      | --                 |

## 4.4.`Bq.Events`.*

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

# 5.Developer

zxyao

# 6.License

MIT
