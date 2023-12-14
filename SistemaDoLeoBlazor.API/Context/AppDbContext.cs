using Microsoft.EntityFrameworkCore;
using SistemaDoLeoBlazor.API.Entities;

namespace SistemaDoLeoBlazor.API.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Operador>? Operador { get; set; }
        public DbSet<OperadorTela>? OperadorTela { get; set; }
        public DbSet<OperadorPermissoesTela>? OperadorPermissoesTela { get; set; }
    }
}
