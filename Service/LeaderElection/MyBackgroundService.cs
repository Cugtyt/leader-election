public abstract class MyBackgroundService : BackgroundService
{
    private static object leaderLock = new object();
    
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        lock (leaderLock)
        {
            return base.StartAsync(cancellationToken);
        }
    }
}