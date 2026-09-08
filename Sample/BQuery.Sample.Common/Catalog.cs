namespace BQuery.Sample.Common;

public static class Catalog
{
    public static readonly (string Path, string Title)[] Pages =
    [
        ("/", "Browser Info"),
        ("/dom", "DOM"),
        ("/elements", "Element Geometry"),
        ("/viewport", "Viewport & Scroll"),
        ("/events", "Window Events"),
        ("/drag", "Drag"),
        ("/utilities", "Throttle & Debounce"),
        ("/getting-started", "Setup")
    ];
}
