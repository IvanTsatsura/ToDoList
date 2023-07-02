using Microsoft.EntityFrameworkCore;

namespace ToDoList.Common.Db;

public class ToDoListContext : DbContext
{
    public DbSet<TaskEntity> Tasks { get; set; }

    public ToDoListContext()
    {
    }
    public ToDoListContext(DbContextOptions<ToDoListContext> options) : base(options) 
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connection = "Data Source=.;" +
                    "Initial Catalog=ToDoList;" +
                    "Integrated Security=true;" +
                    "MultipleActiveResultSets=true;" +
                    "TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connection);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}