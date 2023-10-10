using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);



/*** START OF IMPORTANT CONFIGURATION ***/
var serviceName = "MyTracer";
var serviceVersion = "1.0.0";

builder.Services.AddOpenTelemetry().WithTracing(tcb =>
{
    tcb
        .AddSource(serviceName)
        .AddZipkinExporter(c =>
        {
            c.Endpoint = new Uri("http://zipkin:9411/api/v2/spans");
        })
        .AddConsoleExporter()
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter();
});

builder.Services.AddSingleton(TracerProvider.Default.GetTracer(serviceName));
/*** END OF IMPORTANT CONFIGURATION ***/

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();