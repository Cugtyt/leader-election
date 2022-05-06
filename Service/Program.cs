using k8s;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
builder.Services.AddHostedService<LeaderTask>();
builder.Services.AddHostedService<LeaderTask2>();
builder.Services.AddSingleton<Leader>();
builder.Services.AddSingleton<IKubernetes>(serviceProvider => new Kubernetes(KubernetesClientConfiguration.BuildDefaultConfig()));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
