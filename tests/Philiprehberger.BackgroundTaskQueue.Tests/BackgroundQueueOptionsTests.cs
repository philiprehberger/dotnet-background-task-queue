using Xunit;
using Philiprehberger.BackgroundTaskQueue;

namespace Philiprehberger.BackgroundTaskQueue.Tests;

public class BackgroundQueueOptionsTests
{
    [Fact]
    public void MaxConcurrency_DefaultIsOne()
    {
        var options = new BackgroundQueueOptions();

        Assert.Equal(1, options.MaxConcurrency);
    }

    [Fact]
    public void MaxQueueSize_DefaultIsZero()
    {
        var options = new BackgroundQueueOptions();

        Assert.Equal(0, options.MaxQueueSize);
    }

    [Fact]
    public void OnError_DefaultIsNull()
    {
        var options = new BackgroundQueueOptions();

        Assert.Null(options.OnError);
    }

    [Fact]
    public void Properties_CanBeSet()
    {
        var handler = (Exception _) => { };
        var options = new BackgroundQueueOptions
        {
            MaxConcurrency = 4,
            MaxQueueSize = 100,
            OnError = handler
        };

        Assert.Equal(4, options.MaxConcurrency);
        Assert.Equal(100, options.MaxQueueSize);
        Assert.Same(handler, options.OnError);
    }
}
