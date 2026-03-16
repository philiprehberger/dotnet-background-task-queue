# Philiprehberger.BackgroundTaskQueue

[![CI](https://github.com/philiprehberger/dotnet-background-task-queue/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-background-task-queue/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.BackgroundTaskQueue.svg)](https://www.nuget.org/packages/Philiprehberger.BackgroundTaskQueue)
[![License](https://img.shields.io/github/license/philiprehberger/dotnet-background-task-queue)](LICENSE)

Simple in-memory background job queue for ASP.NET Core with concurrency control.

## Install

```bash
dotnet add package Philiprehberger.BackgroundTaskQueue
```

## Usage

### Register the queue

```csharp
using Philiprehberger.BackgroundTaskQueue;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBackgroundTaskQueue(options =>
{
    options.MaxConcurrency = 4;
    options.MaxQueueSize = 100;
    options.OnError = ex => Console.Error.WriteLine($"Background task failed: {ex}");
});

var app = builder.Build();
app.Run();
```

### Enqueue work from a controller

```csharp
using Microsoft.AspNetCore.Mvc;
using Philiprehberger.BackgroundTaskQueue;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IBackgroundTaskQueue _queue;

    public ReportsController(IBackgroundTaskQueue queue) => _queue = queue;

    [HttpPost]
    public IActionResult Generate()
    {
        _queue.Enqueue(async ct =>
        {
            // Long-running work here
            await Task.Delay(5000, ct);
        }, name: "generate-report");

        return Accepted();
    }
}
```

### Default options (sequential, unbounded)

```csharp
builder.Services.AddBackgroundTaskQueue();
```

## API

### `IBackgroundTaskQueue`

| Member | Description |
|--------|-------------|
| `Enqueue(Func<CancellationToken, Task>)` | Adds a work item to the queue |
| `Enqueue(Func<CancellationToken, Task>, string?)` | Adds a named work item to the queue |
| `DequeueAsync(CancellationToken)` | Waits for and returns the next work item |
| `Count` | Number of items currently in the queue |

### `BackgroundQueueOptions`

| Property | Default | Description |
|----------|---------|-------------|
| `MaxConcurrency` | `1` | Maximum concurrent work items |
| `MaxQueueSize` | `0` | Max queue capacity (0 = unbounded) |
| `OnError` | `null` | Callback invoked when a work item throws |

### `ServiceCollectionExtensions`

| Method | Description |
|--------|-------------|
| `AddBackgroundTaskQueue(Action<BackgroundQueueOptions>?)` | Registers queue and hosted service |

## License

MIT
