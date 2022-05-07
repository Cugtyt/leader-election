using k8s;
using k8s.LeaderElection;
using k8s.LeaderElection.ResourceLock;

public sealed class Leader
{
    private readonly ILogger<Leader> logger;
    private LeaderElector leaderElector;
    private SemaphoreSlim semaphore = new SemaphoreSlim(1);

    public Leader(LeaderElector leaderElector, ILogger<Leader> logger)
    {
        this.leaderElector = leaderElector;
        this.logger = logger;
    }

    public async Task RunAsLeaderAsync(CancellationToken cancellationToken, ILeaderTask leaderTask)
    {
        this.logger.LogInformation("starting leader election");
        this.leaderElector.OnStartedLeading += async () =>
        {
            await leaderTask.RunAsync(cancellationToken);
        };

        await this.semaphore.WaitAsync();
        try
        {
            await this.leaderElector.RunAsync(cancellationToken);
        }
        finally
        {
            this.semaphore.Release();
        }
    }

    public bool IsLeader()
    {
        return this.leaderElector.IsLeader();
    }

    public string GetLeader()
    {
        return this.leaderElector.GetLeader();
    }
}