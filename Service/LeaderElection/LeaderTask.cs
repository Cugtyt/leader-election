public class LeaderTask : MyBackgroundService, ILeaderTask
{
    private readonly ILogger<LeaderTask> logger;

    public LeaderTask(Leader leader, ILogger<LeaderTask> logger)
    {
        MyBackgroundService.leader = MyBackgroundService.leader ?? leader;
        this.logger = logger;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation($"{MyBackgroundService.leader.GetLeader()} is leader: {MyBackgroundService.leader.IsLeader()}");
        while (!cancellationToken.IsCancellationRequested && MyBackgroundService.leader.IsLeader())
        {
            this.logger.LogInformation($"{MyBackgroundService.leader.GetLeader()} is doing work");
            await Task.Delay(TimeSpan.FromSeconds(5));
            this.logger.LogInformation($"{MyBackgroundService.leader.GetLeader()} is done");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await MyBackgroundService.leader.RunAsLeaderAsync(cancellationToken, this);
    }
}