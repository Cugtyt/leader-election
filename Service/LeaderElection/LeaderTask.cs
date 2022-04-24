public class LeaderTask : BackgroundService, ILeaderTask
{
    private readonly Leader leader;
    private readonly ILogger<LeaderTask> logger;

    public LeaderTask(Leader leader, ILogger<LeaderTask> logger)
    {
        this.leader = leader;
        this.logger = logger;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation($"{this.leader.GetLeader()} is leader:  {this.leader.IsLeader()}");
        while (!cancellationToken.IsCancellationRequested && this.leader.IsLeader())
        {
            this.logger.LogInformation($"{this.leader.GetLeader()} is doing work");
            await Task.Delay(TimeSpan.FromSeconds(5));
            this.logger.LogInformation($"{this.leader.GetLeader()} is done");
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await this.leader.RunAsLeaderAsync(cancellationToken, Environment.GetEnvironmentVariable("POD_NAME")!, "leaderelection", "leaderelection-namespace", this);
    }
}