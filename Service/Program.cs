using k8s;
using k8s.LeaderElection;
using k8s.LeaderElection.ResourceLock;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
builder.Services.AddSingleton<IKubernetes>(serviceProvider => new Kubernetes(KubernetesClientConfiguration.BuildDefaultConfig()));
builder.Services.AddSingleton<LeaderElector>(serviceProvider => 
{
    var leaderId = Environment.GetEnvironmentVariable("POD_NAME")!;
    var componentName = "leaderelection";
    var componentNamespace = "leaderelection-namespace";
    var client = serviceProvider.GetRequiredService<IKubernetes>();
    var leaseLock = new LeaseLock(client, componentNamespace, componentName, leaderId);
    var leaderElectionConfig = new LeaderElectionConfig(leaseLock);
    var leaderElector = new LeaderElector(leaderElectionConfig);
    return leaderElector;
});
builder.Services.AddSingleton<Leader>();
builder.Services.AddHostedService<LeaderTask>();
builder.Services.AddHostedService<LeaderTask2>();
builder.Services.AddHostedService<LeaderTask3>();
builder.Services.AddHostedService<LeaderTask4>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
