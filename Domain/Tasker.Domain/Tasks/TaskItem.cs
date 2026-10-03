namespace Tasker.Domain.Tasks;

public class TaskItem
{
    private TaskItem()
    {
        Title = string.Empty;
    }

    public TaskItem(string title, string? description)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public void Complete()
    {
        IsCompleted = true;
    }

    public void Update(string title, string? description)
    {
        Title = title;
        Description = description;
    }
}
