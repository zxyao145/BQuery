namespace BQuery;

public class BqDrag
{
    private readonly IJSRuntime _jsRuntime;

    internal BqDrag(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// bind drag for <param name="element">element</param> 
    /// </summary>
    public async Task BindDragAsync(ElementReference element, DragOptions? options = null)
    {
        await _jsRuntime.InvokeVoidAsync(DragConstants.BindDragMethod, element, options);
    }

    /// <summary>
    /// remove drag binding for the target element or its configured drag trigger
    /// </summary>
    public async Task RemoveDragAsync(ElementReference element, DragOptions? options = null)
    {
        await _jsRuntime.InvokeVoidAsync(DragConstants.RemoveDraggableMethod, GetDragTrigger(element, options));
    }

    /// <summary>
    /// reset drag position for the target element or its configured drag trigger
    /// </summary>
    public async Task ResetDragPositionAsync(ElementReference element, DragOptions? options = null)
    {
        await _jsRuntime.InvokeVoidAsync(DragConstants.ResetDraggableElePositionMethod, GetDragTrigger(element, options));
    }


    private static ElementReference GetDragTrigger(ElementReference element, DragOptions? options)
    {
        return options?.DragElement ?? element;
    }
}
