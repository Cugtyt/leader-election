public class LeaderTask4 : MyBackgroundService, ILeaderTask
{
    private readonly Leader leader;
    private readonly ILogger<LeaderTask4> logger;

    public LeaderTask4(Leader leader, ILogger<LeaderTask4> logger)
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
            await Task.Delay(TimeSpan.FromSeconds(1));
            this.logger.LogInformation($"{this.leader.GetLeader()} is done");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await this.leader.RunAsLeaderAsync(cancellationToken, this);
    }
}