using Microsoft.Extensions.Hosting;

namespace Philiprehberger.BackgroundTaskQueue;

/// <summary>
/// A hosted service that continuously dequeues and executes work items
/// from an <see cref="IBackgroundTaskQueue"/>.
/// </summary>
public sealed class QueuedHostedService : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly SemaphoreSlim _semaphore;
    private readonly Action<Exception>? _onError;

    /// <summary>
    /// Creates a new <see cref="QueuedHostedService"/>.
    /// </summary>
    /// <param name="taskQueue">The background task queue to process.</param>
    /// <param name="options">Configuration options for concurrency and error handling.</param>
    public QueuedHostedService(IBackgroundTaskQueue taskQueue, BackgroundQueueOptions options)
    {
        ArgumentNullException.ThrowIfNull(taskQueue);
        ArgumentNullException.ThrowIfNull(options);

        _taskQueue = taskQueue;
        _semaphore = new SemaphoreSlim(options.MaxConcurrency);
        _onError = options.OnError;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await _taskQueue.DequeueAsync(stoppingToken);

            await _semaphore.WaitAsync(stoppingToken);

            _ = ExecuteWorkItemAsync(workItem, stoppingToken);
        }
    }

    private async Task ExecuteWorkItemAsync(Func<CancellationToken, Task> workItem, CancellationToken stoppingToken)
    {
        try
        {
            await workItem(stoppingToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _onError?.Invoke(ex);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
