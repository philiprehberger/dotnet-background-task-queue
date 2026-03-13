namespace Philiprehberger.BackgroundTaskQueue;

/// <summary>
/// Configuration options for the background task queue.
/// </summary>
public class BackgroundQueueOptions
{
    /// <summary>
    /// Gets or sets the maximum number of work items that can execute concurrently.
    /// Defaults to 1 (sequential processing).
    /// </summary>
    public int MaxConcurrency { get; set; } = 1;

    /// <summary>
    /// Gets or sets the maximum number of work items the queue can hold.
    /// Set to 0 for an unbounded queue. Defaults to 0.
    /// </summary>
    public int MaxQueueSize { get; set; } = 0;

    /// <summary>
    /// Gets or sets a callback that is invoked when a work item throws an exception.
    /// </summary>
    public Action<Exception>? OnError { get; set; }
}
