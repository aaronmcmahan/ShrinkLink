using Web.Data;
using Microsoft.EntityFrameworkCore;
using Web.Models;
using HashidsNet;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder);

var app = builder.Build();

await ConfigureApplicationAsync(app);

app.Run();

static void RegisterServices(WebApplicationBuilder builder)
{
    var services = builder.Services;

    var settings = builder.Configuration.GetSection(nameof(LinkGeneration)).Get<LinkGeneration>();
    services.AddSingleton<IHashids>(_ => new Hashids(settings.Salt, settings.MinLength));

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

    app.MapGet("/{code}", async (string code, ApplicationDbContext db, IHashids hashids) =>
        await db.Links.FindAsync(hashids.DecodeSingle(code))
            is Link link
                ? Results.Redirect(link.FullUrl, true, true)
                : Results.NotFound());
}

public record LinkGeneration(string Salt, int MinLength);