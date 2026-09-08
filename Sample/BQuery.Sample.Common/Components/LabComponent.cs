using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
namespace BQuery.Sample.Common.Components;

public abstract class LabComponent : ComponentBase, IAsyncDisposable
{
    [Inject] protected Bq Bq { get; set; } = default!;
    [Inject] protected IJSRuntime JS { get; set; } = default!;
    protected bool Ready;
    protected bool Disposed;
    protected string Error = "";
    protected IJSObjectReference? Module;
    private Task initialization = Task.CompletedTask;
    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return Task.CompletedTask;
        return initialization = InitializeBrowser();
    }
    private async Task InitializeBrowser()
    {
        await Run(async () =>
        {
            Module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/BQuery.Sample.Common/lab.js");
            if (!Disposed) { Ready = true; await Initialize(); }
        });
        if (!Disposed) StateHasChanged();
    }
    protected virtual Task Initialize() => Task.CompletedTask;
    protected async Task Run(Func<Task> action)
    {
        Error = "";
        try { await action(); }
        catch (Exception ex) when (ex is JSException or TaskCanceledException or ObjectDisposedException) { if (!Disposed) Error = ex.Message; }
    }
    protected virtual ValueTask Cleanup() => ValueTask.CompletedTask;
    public async ValueTask DisposeAsync()
    {
        Disposed = true;
        await initialization;
        try { await Cleanup(); }
        catch (Exception ex) when (ex is JSDisconnectedException or TaskCanceledException or ObjectDisposedException) { }
        if (Module is not null)
        {
            try { await Module.DisposeAsync(); }
            catch (Exception ex) when (ex is JSDisconnectedException or TaskCanceledException or ObjectDisposedException) { }
        }
    }
}
