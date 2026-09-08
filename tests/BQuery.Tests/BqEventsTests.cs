using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.JSInterop;
using Xunit;

namespace BQuery.Tests;

public class BqEventsTests
{
    [Fact]
    public async Task AddListener_WhenRetriedAfterFailure_InvokesHandlerOnce()
    {
        // Arrange
        var runtime = new TestJsRuntime();
        await using var events = CreateEvents(runtime);
        int calls = 0;
        Func<MouseEventArgs, Task> handler = _ => { calls++; return Task.CompletedTask; };
        runtime.Failure = new JSException("Registration failed");

        // Act
        await Assert.ThrowsAsync<JSException>(() => events.AddWindowEventListener(WindowEvent.OnClick, handler));
        await events.AddWindowEventListener(WindowEvent.OnClick, handler);
        await events.WindowClick(new MouseEventArgs());

        // Assert
        Assert.Equal(1, calls);
    }

    [Fact]
    public async Task RemoveListener_WhenInteropFails_PreservesHandlerUntilRetry()
    {
        // Arrange
        var runtime = new TestJsRuntime();
        await using var events = CreateEvents(runtime);
        int calls = 0;
        Func<MouseEventArgs, Task> handler = _ => { calls++; return Task.CompletedTask; };
        await events.AddWindowEventListener(WindowEvent.OnClick, handler);
        runtime.Failure = new JSException("Removal failed");

        // Act
        await Assert.ThrowsAsync<JSException>(() => events.RemoveWindowEventListener(WindowEvent.OnClick, handler));
        await events.WindowClick(new MouseEventArgs());
        await events.RemoveWindowEventListener(WindowEvent.OnClick, handler);
        await events.WindowClick(new MouseEventArgs());

        // Assert
        Assert.Equal(1, calls);
    }

    [Fact]
    public async Task RemoveListeners_WhenInteropFails_DisposeStillUnregistersScope()
    {
        // Arrange
        var runtime = new TestJsRuntime();
        var events = CreateEvents(runtime);
        await events.AddWindowEventListeners(WindowEvent.OnClick);
        runtime.Failure = new JSException("Removal failed");

        // Act
        await Assert.ThrowsAsync<JSException>(() => events.RemoveWindowEventListeners(WindowEvent.OnClick));
        await events.DisposeAsync();

        // Assert
        Assert.Equal("bQuery.windowEvents.disposeWindowEventsListener", runtime.Calls[^1].Identifier);
        Assert.Throws<ObjectDisposedException>(() => runtime.Reference!.Value);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Dispose_WhenInteropFails_ReleasesReferenceAndHandlers(bool disconnected)
    {
        // Arrange
        var runtime = new TestJsRuntime();
        var events = CreateEvents(runtime);
        int calls = 0;
        await events.AddWindowEventListener<MouseEventArgs>(WindowEvent.OnClick, _ => { calls++; return Task.CompletedTask; });
        runtime.Failure = disconnected ? new JSDisconnectedException("Disconnected") : new JSException("Disposal failed");

        // Act
        if (disconnected)
        {
            await events.DisposeAsync();
        }
        else
        {
            await Assert.ThrowsAsync<JSException>(async () => await events.DisposeAsync());
        }
        await events.DisposeAsync();
        await events.WindowClick(new MouseEventArgs());

        // Assert
        Assert.Throws<ObjectDisposedException>(() => runtime.Reference!.Value);
        Assert.Equal(0, calls);
        Assert.Equal(2, runtime.Calls.Count);
    }

    [Fact]
    public async Task AddListener_AfterDisposal_RejectsRegistration()
    {
        // Arrange
        var runtime = new TestJsRuntime();
        var events = CreateEvents(runtime);
        await events.DisposeAsync();

        // Act
        await Assert.ThrowsAsync<ObjectDisposedException>(() => events.AddWindowEventListener<MouseEventArgs>(WindowEvent.OnClick, _ => Task.CompletedTask));

        // Assert
        Assert.Empty(runtime.Calls);
    }

    [Fact]
    public async Task RemoveListener_AfterDisposal_DoesNotCallJavascript()
    {
        // Arrange
        var runtime = new TestJsRuntime();
        var events = CreateEvents(runtime);
        await events.DisposeAsync();

        // Act
        await events.RemoveWindowEventListener<MouseEventArgs>(WindowEvent.OnClick, _ => Task.CompletedTask);

        // Assert
        Assert.Empty(runtime.Calls);
    }

    [Fact]
    public async Task RemoveListener_WithAnotherSubscriber_PreservesOtherHandler()
    {
        // Arrange
        var runtime = new TestJsRuntime();
        await using var events = CreateEvents(runtime);
        int first = 0, second = 0;
        Func<MouseEventArgs, Task> firstHandler = _ => { first++; return Task.CompletedTask; };
        await events.AddWindowEventListener(WindowEvent.OnClick, firstHandler);
        await events.AddWindowEventListener<MouseEventArgs>(WindowEvent.OnClick, _ => { second++; return Task.CompletedTask; });

        // Act
        await events.RemoveWindowEventListener(WindowEvent.OnClick, firstHandler);
        await events.WindowClick(new MouseEventArgs());

        // Assert
        Assert.Equal(0, first);
        Assert.Equal(1, second);
        Assert.DoesNotContain(runtime.Calls, call => call.Identifier.Contains("removeWindow"));
    }

    [Fact]
    public async Task Dispose_WithAnotherScope_PreservesOtherScope()
    {
        // Arrange
        var firstRuntime = new TestJsRuntime();
        var first = CreateEvents(firstRuntime);
        var secondRuntime = new TestJsRuntime();
        await using var second = CreateEvents(secondRuntime);
        int calls = 0;
        await first.AddWindowEventListeners(WindowEvent.OnClick);
        await second.AddWindowEventListener<MouseEventArgs>(WindowEvent.OnClick, _ => { calls++; return Task.CompletedTask; });

        // Act
        await first.DisposeAsync();
        await second.WindowClick(new MouseEventArgs());

        // Assert
        Assert.NotEqual(firstRuntime.Calls[0].Arguments[1], secondRuntime.Calls[0].Arguments[1]);
        Assert.Same(second, secondRuntime.Reference!.Value);
        Assert.Equal(1, calls);
    }

    [Fact]
    public async Task Dispose_DuringRegistration_UnregistersCompletedRegistration()
    {
        // Arrange
        var runtime = new TestJsRuntime { PendingCall = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously) };
        var events = CreateEvents(runtime);
        Task registration = events.AddWindowEventListeners(WindowEvent.OnClick);

        // Act
        Task disposal = events.DisposeAsync().AsTask();
        Assert.Single(runtime.Calls);
        runtime.PendingCall.SetResult();
        await Task.WhenAll(registration, disposal);

        // Assert
        Assert.Equal("bQuery.windowEvents.disposeWindowEventsListener", runtime.Calls[^1].Identifier);
        Assert.Throws<ObjectDisposedException>(() => runtime.Reference!.Value);
    }

    [Fact]
    public async Task ElementExtension_WithRenderedContext_ResolvesJavascriptRuntime()
    {
        // Arrange
        var runtime = new TestJsRuntime();
        var element = new ElementReference("test-element", new WebElementReferenceContext(runtime));

        // Act
        await element.GetWidthAsync();

        // Assert
        Assert.Equal("bQuery.elementExtensions.getWidth", Assert.Single(runtime.Calls).Identifier);
        Assert.Equal(element, runtime.Calls[0].Arguments[0]);
    }

    private static BqEvents CreateEvents(TestJsRuntime runtime) => new(runtime, NullLogger<BqEvents>.Instance);

    private sealed class TestJsRuntime : IJSRuntime
    {
        public List<(string Identifier, object?[] Arguments)> Calls { get; } = [];
        public Exception? Failure { get; set; }
        public TaskCompletionSource? PendingCall { get; set; }
        public DotNetObjectReference<BqEvents>? Reference { get; private set; }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => InvokeAsync<TValue>(identifier, CancellationToken.None, args);

        public async ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        {
            Calls.Add((identifier, args ?? []));
            Reference = args?.OfType<DotNetObjectReference<BqEvents>>().FirstOrDefault() ?? Reference;
            if (Failure is { } failure)
            {
                Failure = null;
                throw failure;
            }
            if (PendingCall is { Task.IsCompleted: false } pending)
            {
                await pending.Task;
            }
            return default!;
        }
    }
}
