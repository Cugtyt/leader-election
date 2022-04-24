public interface ILeaderTask
{
    Task RunAsync(CancellationToken cancellationToken);
}