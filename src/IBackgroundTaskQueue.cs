namespace Philiprehberger.BackgroundTaskQueue;

/// <summary>
/// Represents a queue of background work items to be processed by a hosted service.
/// </summary>
public interface IBackgroundTaskQueue
{
    /// <summary>
    /// Gets the number of work items currently waiting in the queue.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Enqueues a background work item.
    /// </summary>
    /// <param name="workItem">The work item to execute in the background.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="workItem"/> is null.</exception>
    void Enqueue(Func<CancellationToken, Task> workItem);

    /// <summary>
    /// Enqueues a background work item with an optional name for identification.
    /// </summary>
    /// <param name="workItem">The work item to execute in the background.</param>
    /// <param name="name">An optional name to identify this work item.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="workItem"/> is null.</exception>
    void Enqueue(Func<CancellationToken, Task> workItem, string? name);

    /// <summary>
    /// Waits for and dequeues the next available work item.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the dequeue operation.</param>
    /// <returns>The next work item from the queue.</returns>
    Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
}
