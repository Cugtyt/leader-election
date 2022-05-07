public class LeaderTask2 : BackgroundService, ILeaderTask
{
    private readonly Leader leader;
    private readonly ILogger<LeaderTask2> logger;

    public LeaderTask2(Leader leader, ILogger<LeaderTask2> logger)
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
            await Task.Delay(TimeSpan.FromSeconds(3));
            this.logger.LogInformation($"{this.leader.GetLeader()} is done");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await this.leader.RunAsLeaderAsync(cancellationToken, this);
    }
}