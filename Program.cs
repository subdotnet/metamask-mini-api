using metamask_mini_api.Services;
using Seq.Api;
using Serilog;

// Console.WriteLine("starting...");
// var connection = new SeqConnection("http://localhost:18080", "ZHJOkr6jFfLrcGBZs2ZU");
// await connection.Events.DeleteAsync();

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5035");
var myLogger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .Enrich.With()
    .WriteTo.Console()//outputTemplate:"{HH:mm:ss.fff zzz} [{Level}] ({SourceContext}.{Method}) {Message}{NewLine}{Exception}")
    // .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();
Serilog.Log.Logger = myLogger;
Serilog.SerilogHostBuilderExtensions.UseSerilog(builder.Host);
builder.Services.AddControllers();
builder.Services.AddTransient<MinimalEthApi>();
builder.Services.AddSingleton(SimpleBlockChain.Initialize());
builder.Services.AddCors();
var app = builder.Build();
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyOrigin().WithMethods("GET", "POST"));
app.MapControllers();
app.Run();
