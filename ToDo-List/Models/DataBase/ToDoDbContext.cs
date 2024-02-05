
using Microsoft.EntityFrameworkCore;
using ToDo_List.Models.DataBase.Entities;

namespace ToDo_List.Models.DataBase
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options) {
        }

        public DbSet<TaskCard> TaskCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ToDoDbContext).Assembly);
        }
    }
}
