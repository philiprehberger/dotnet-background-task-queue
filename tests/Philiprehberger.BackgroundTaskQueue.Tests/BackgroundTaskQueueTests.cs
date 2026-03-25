using Xunit;
using Philiprehberger.BackgroundTaskQueue;

namespace Philiprehberger.BackgroundTaskQueue.Tests;

public class BackgroundTaskQueueTests
{
    [Fact]
    public void Enqueue_ValidWorkItem_IncrementsCount()
    {
        var queue = new BackgroundTaskQueue(new BackgroundQueueOptions());

        queue.Enqueue(_ => Task.CompletedTask);

        Assert.Equal(1, queue.Count);
    }

    [Fact]
    public void Enqueue_NullWorkItem_ThrowsArgumentNullException()
    {
        var queue = new BackgroundTaskQueue(new BackgroundQueueOptions());

        Assert.Throws<ArgumentNullException>(() => queue.Enqueue(null!));
    }

    [Fact]
    public async Task DequeueAsync_ReturnsEnqueuedItem()
    {
        var queue = new BackgroundTaskQueue(new BackgroundQueueOptions());
        var executed = false;
        queue.Enqueue(_ => { executed = true; return Task.CompletedTask; });

        var workItem = await queue.DequeueAsync(CancellationToken.None);
        await workItem(CancellationToken.None);

        Assert.True(executed);
    }

    [Fact]
    public async Task DequeueAsync_DecrementsCount()
    {
        var queue = new BackgroundTaskQueue(new BackgroundQueueOptions());
        queue.Enqueue(_ => Task.CompletedTask);

        await queue.DequeueAsync(CancellationToken.None);

        Assert.Equal(0, queue.Count);
    }

    [Fact]
    public void Count_EmptyQueue_ReturnsZero()
    {
        var queue = new BackgroundTaskQueue(new BackgroundQueueOptions());

        Assert.Equal(0, queue.Count);
    }
}
