using WorkerService1;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

GeneratePdf.Generate();

var host = builder.Build();
host.Run();