public class LeaderTask3 : MyBackgroundService, ILeaderTask
{
    private readonly ILogger<LeaderTask3> logger;

    public LeaderTask3(Leader leader, ILogger<LeaderTask3> logger)
    {
        MyBackgroundService.leader = MyBackgroundService.leader ?? leader;
        this.logger = logger;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation($"{MyBackgroundService.leader.GetLeader()} is leader:  {MyBackgroundService.leader.IsLeader()}");
        while (!cancellationToken.IsCancellationRequested && MyBackgroundService.leader.IsLeader())
        {
            this.logger.LogInformation($"{MyBackgroundService.leader.GetLeader()} is doing work");
            await Task.Delay(TimeSpan.FromSeconds(6));
            this.logger.LogInformation($"{MyBackgroundService.leader.GetLeader()} is done");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await MyBackgroundService.leader.RunAsLeaderAsync(cancellationToken, this);
    }
}