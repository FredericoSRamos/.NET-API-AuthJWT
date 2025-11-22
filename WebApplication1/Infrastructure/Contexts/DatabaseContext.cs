using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

namespace WebApplication1.Infrastructure.Contexts;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Vehicle>  Vehicles { get; set; }
}