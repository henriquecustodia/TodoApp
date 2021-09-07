using Microsoft.EntityFrameworkCore;

namespace Todo
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

        public DbSet<TodoModel> Todo { get; set; }
        public DbSet<UserModel> User { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoModel>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<UserModel>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("getdate()");
        }
    }
}
