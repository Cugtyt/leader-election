public class LeaderTask3 : BackgroundService, ILeaderTask
{
    private readonly Leader leader;
    private readonly ILogger<LeaderTask3> logger;

    public LeaderTask3(Leader leader, ILogger<LeaderTask3> logger)
    {
        this.leader = leader ;
        this.logger = logger;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation($"{this.leader.GetLeader()} is leader:  {this.leader.IsLeader()}");
        while (!cancellationToken.IsCancellationRequested && this.leader.IsLeader())
        {
            this.logger.LogInformation($"{this.leader.GetLeader()} is doing work");
            await Task.Delay(TimeSpan.FromSeconds(6));
            this.logger.LogInformation($"{this.leader.GetLeader()} is done");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await this.leader.RunAsLeaderAsync(cancellationToken, this);
    }
}