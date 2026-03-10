namespace BQuery;

public partial class BqObject
{

    private readonly IJSRuntime _jsRuntime;

    public BqObject(IJSRuntime jsRuntime, BqEvents events)
    {
        this._jsRuntime = jsRuntime;
        Viewport = new BqViewport(jsRuntime);
        Drag = new BqDrag(jsRuntime);
        WindowEvents = events;
    }

    #region modules

    /// <summary>
    /// Viewport operation
    /// </summary>
    public BqViewport Viewport { get; }

    public BqDrag Drag { get; }

    #endregion


    #region WindowEvents

    /// <summary>
    /// Window event hub for the current DI scope.
    /// </summary>
    public BqEvents WindowEvents { get; }


    public async Task AddWindowEventListeners(params WindowEvent[] windowEvents)
    {
        await WindowEvents.AddWindowEventListeners(windowEvents);
    }


    public async Task RemoveWindowEventListeners(params WindowEvent[] windowEvents)
    {
        await WindowEvents.RemoveWindowEventListeners(windowEvents);
    }

    public async Task AddWindowEventListener<T>(WindowEvent windowEvent, Func<T, Task> func)
    {
        await WindowEvents.AddWindowEventListener<T>(windowEvent, func);
    }

    public async Task RemoveWindowEventListener<T>(WindowEvent windowEvent, Func<T, Task> func)
    {
        await WindowEvents.RemoveWindowEventListener<T>(windowEvent, func);
    }
    #endregion


    /// <summary>
    /// get browser useragent
    /// </summary>
    public async Task<string> GetUserAgentAsync()
    {
        return await _jsRuntime.InvokeAsync<string>(NavigatorConstants.GetUserAgentMethod);
    }
}
