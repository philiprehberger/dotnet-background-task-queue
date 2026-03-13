using System.Threading.Channels;

namespace Philiprehberger.BackgroundTaskQueue;

/// <summary>
/// Thread-safe in-memory implementation of <see cref="IBackgroundTaskQueue"/>
/// backed by a <see cref="Channel{T}"/>.
/// </summary>
public sealed class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<CancellationToken, Task>> _channel;
    private int _count;

    /// <summary>
    /// Creates a new <see cref="BackgroundTaskQueue"/> with the specified options.
    /// </summary>
    /// <param name="options">Configuration options for the queue.</param>
    public BackgroundTaskQueue(BackgroundQueueOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _channel = options.MaxQueueSize > 0
            ? Channel.CreateBounded<Func<CancellationToken, Task>>(new BoundedChannelOptions(options.MaxQueueSize)
            {
                FullMode = BoundedChannelFullMode.Wait
            })
            : Channel.CreateUnbounded<Func<CancellationToken, Task>>(new UnboundedChannelOptions
            {
                SingleReader = false,
                SingleWriter = false
            });
    }

    /// <inheritdoc />
    public int Count => Volatile.Read(ref _count);

    /// <inheritdoc />
    public void Enqueue(Func<CancellationToken, Task> workItem)
    {
        Enqueue(workItem, name: null);
    }

    /// <inheritdoc />
    public void Enqueue(Func<CancellationToken, Task> workItem, string? name)
    {
        ArgumentNullException.ThrowIfNull(workItem);

        if (!_channel.Writer.TryWrite(workItem))
        {
            throw new InvalidOperationException("Unable to enqueue work item. The queue may be full.");
        }

        Interlocked.Increment(ref _count);
    }

    /// <inheritdoc />
    public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
    {
        var workItem = await _channel.Reader.ReadAsync(cancellationToken);
        Interlocked.Decrement(ref _count);
        return workItem;
    }
}
