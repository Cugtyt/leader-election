using k8s;
using k8s.LeaderElection;
using k8s.LeaderElection.ResourceLock;

public class Leader
{
    private readonly IKubernetes client;
    private readonly ILogger<Leader> logger;
    private LeaderElector? leaderElector;

    public Leader(IKubernetes client, ILogger<Leader> logger)
    {
        this.client = client;
        this.logger = logger;
    }

    public async Task RunAsLeaderAsync(CancellationToken cancellationToken, string leaderId, string componentName, string componentNamespace, ILeaderTask leaderTask)
    {
        string leaseName = $"leader-lease-{componentName}";
        var leaseLock = new LeaseLock(client, leaseName, componentNamespace, leaderId);
        var leaderElectionConfig = new LeaderElectionConfig(leaseLock);
        this.leaderElector = new LeaderElector(leaderElectionConfig);
        this.leaderElector.OnNewLeader += (string identity) =>
        {
            this.logger.LogInformation($"{identity} is now the leader, old leader was {leaderId}");
        };
        this.leaderElector.OnStartedLeading += async () =>
        {
            this.logger.LogInformation($"{leaderId} start leading");
            await leaderTask.RunAsync(cancellationToken);
        };
        this.leaderElector.OnStoppedLeading += () =>
        {
            this.logger.LogInformation($"{leaderId} stop leading");
        };

        this.logger.LogInformation("starting leader election");
        this.logger.LogInformation($"IsCancellationRequested: {cancellationToken.IsCancellationRequested}");
        await this.leaderElector.RunAsync(cancellationToken);
    }

    public bool IsLeader()
    {
        return this.leaderElector!.IsLeader();
    }

    public string GetLeader()
    {
        return this.leaderElector!.GetLeader();
    }
}