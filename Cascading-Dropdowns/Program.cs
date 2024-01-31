using Cascading_Dropdowns.Services;
using Cascading_Dropdowns.Components;

namespace Cascading_Dropdowns;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpClient<IAggregationsService, AggregationsService>(client =>
        {
            client.BaseAddress = new Uri("https://app.aggregations.io/api/v1/");
            client.DefaultRequestHeaders.Add("x-api-token",
                builder.Configuration["Aggregations:ApiToken"]);
        });
        
        builder.Services.AddRazorComponents();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>();

        app.Run();
    }
}