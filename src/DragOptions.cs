namespace BQuery;

public class DragOptions
{
    /// <summary>
    /// draggable element must in viewport?
    /// </summary>
    public bool InViewport { get; set; } = true;

    /// <summary>
    /// which draggable element's sub element trigger drag event, default is draggable element
    /// </summary>
    public ElementReference? DragElement { get; set; } = null;
}
