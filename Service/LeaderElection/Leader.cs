using k8s;
using k8s.LeaderElection;
using k8s.LeaderElection.ResourceLock;

public sealed class Leader
{
    private readonly ILogger<Leader> logger;
    private readonly LeaderElector leaderElector;

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

        await this.leaderElector.RunAsync(cancellationToken);
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