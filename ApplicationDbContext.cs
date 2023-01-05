using Microsoft.EntityFrameworkCore;
using TaskManagement.Entities;

namespace TaskManagement
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Tarea> Tasks { get; set; }
    }
}
