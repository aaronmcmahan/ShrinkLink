using Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web.Models;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder);

var app = builder.Build();

await ConfigureApplicationAsync(app);

app.Run();

static void RegisterServices(WebApplicationBuilder builder)
{
    var services = builder.Services;

    services.AddRazorPages();
    string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString));
}

static async Task ConfigureApplicationAsync(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var scopedProvider = scope.ServiceProvider;
        try
        {
            var context = scopedProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred with the DB.");
        }
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();
    app.MapRazorPages();

    app.MapGet("/{slug}", async (string slug, ApplicationDbContext db) =>
        await db.Links.SingleOrDefaultAsync(x => x.Slug == slug)
            is Link link
                ? Results.Redirect(link.FullUrl, true, true)
                : Results.NotFound());
}
