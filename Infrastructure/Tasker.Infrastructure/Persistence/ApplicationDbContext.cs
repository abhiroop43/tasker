namespace Tasker.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    private const string DefaultSchema = "tasker";
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DefaultSchema);
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title).IsRequired().HasMaxLength(200);

            entity.Property(x => x.Description).HasMaxLength(2000);

            entity.Property(x => x.IsCompleted).IsRequired();

            entity.Property(x => x.CreatedAt).IsRequired();
        });
        base.OnModelCreating(modelBuilder);
    }
}
