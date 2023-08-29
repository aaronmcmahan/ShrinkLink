using System.Data;
using Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace Web.Data;

public class ApplicationDbContext : DbContext
{
    private IDbContextTransaction? _currentTransaction;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    public DbSet<Link> Links { get; set; } = null!;

    // protected override void OnModelCreating(DbCommandBuilder builder)
    // {

    // }
}