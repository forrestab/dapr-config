using Dapr.Client;
using Dapr.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;

var builder = WebApplication.CreateBuilder(args);

var daprClient = new DaprClientBuilder().Build();
builder.Configuration
    .AddDaprConfigurationStore("configstore", ReadOnlyCollection<string>.Empty, daprClient, TimeSpan.FromSeconds(20),
        new Dictionary<string, string>
        {
            { "pgNotifyChannel", "config" }
        }
    )
    .AddStreamingDaprConfigurationStore("configstore", ReadOnlyCollection<string>.Empty, daprClient, TimeSpan.FromSeconds(20),
        new Dictionary<string, string>
        {
            { "pgNotifyChannel", "config" }
        }
    );

builder.Services.Configure<AppSettingsOptions>(builder.Configuration.GetSection("AppSettings"));

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/config", (IConfiguration configuration, IOptionsMonitor<AppSettingsOptions> options) =>
{
    return new
    {
        fromOptions = options,
        fromConfiguration = new
        {
            Value1 = configuration.GetValue<string>("AppSettings:Value1"),
            Value2 = configuration.GetValue<string>("AppSettings:Value2")
        }
    };
});

app.Run();

internal record AppSettingsOptions
{
    public string Value1 { get; set; }
    public string Value2 { get; set; }
}
