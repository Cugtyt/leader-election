public abstract class MyBackgroundService : BackgroundService
{
    private static SemaphoreSlim semaphore = new SemaphoreSlim(1);
    protected static Leader leader;

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await semaphore.WaitAsync();
        try {
            await base.StartAsync(cancellationToken);
        } finally {
            semaphore.Release();
        }
    }
}