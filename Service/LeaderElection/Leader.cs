using k8s;
using k8s.LeaderElection;
using k8s.LeaderElection.ResourceLock;

public sealed class Leader
{
    private readonly ILogger<Leader> logger;
    private static LeaderElector? leaderElector;
    private static SemaphoreSlim semaphore = new SemaphoreSlim(1);

    public Leader(LeaderElector leaderElector, ILogger<Leader> logger)
    {
        Leader.leaderElector = leaderElector;
        this.logger = logger;
    }

    public async Task RunAsLeaderAsync(CancellationToken cancellationToken, ILeaderTask leaderTask)
    {
        this.logger.LogInformation("starting leader election");
        Leader.leaderElector!.OnStartedLeading += async () =>
        {
            await leaderTask.RunAsync(cancellationToken);
        };

        await Leader.semaphore.WaitAsync();
        try
        {
            await Leader.leaderElector!.RunAsync(cancellationToken);
        }
        finally
        {
            Leader.semaphore.Release();
        }
    }

    public bool IsLeader()
    {
        return Leader.leaderElector!.IsLeader();
    }

    public string GetLeader()
    {
        return Leader.leaderElector!.GetLeader();
    }
}