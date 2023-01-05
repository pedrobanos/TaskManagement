using Microsoft.EntityFrameworkCore;
using TaskManagement.Entities;

namespace TaskManagement
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Tarea> Tareas { get; set; }
    }
}
