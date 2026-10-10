namespace Tasker.Domain.Pagination;

public class PaginationResult<TEntity>(
    int pageNumber,
    int pageSize,
    long count,
    IEnumerable<TEntity> data
)
    where TEntity : class
{
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
    public long Count { get; } = count;
    public int TotalPages => (int)Math.Ceiling((double)Count / PageSize);

    public bool HasNextPage => PageNumber < TotalPages;
    public IEnumerable<TEntity> Data { get; } = data;
}
