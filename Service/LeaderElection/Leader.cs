using k8s;
using k8s.LeaderElection;
using k8s.LeaderElection.ResourceLock;

public class Leader
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
        var leaderId = Environment.GetEnvironmentVariable("POD_NAME")!;
        this.leaderElector.OnNewLeader += (string identity) =>
        {
            this.logger.LogInformation($"{identity} is now the leader, old leader was {leaderId}");
        };
        this.leaderElector.OnStoppedLeading += () =>
        {
            this.logger.LogInformation($"{leaderId} stop leading");
        };
        this.leaderElector.OnStartedLeading += async () =>
        {
            this.logger.LogInformation($"{leaderId} start leading");
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