using BlazorSeven;
using BlazorSeven.Data;
using BlazorSeven.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMasaBlazor();
builder.Services.AddSingleton(_ => new Manager("��Ŀ¼"));
builder.Services.AddSingleton<VersionInfo>();
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddSingleton<WeatherForecastService>();

await builder.Build().RunAsync();
