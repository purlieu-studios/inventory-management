using Microsoft.EntityFrameworkCore;

namespace Infrastructure;
public static class PagedListExtensions
{
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> source,
        int pageIndex,
        int pageSize,
        int indexFrom = 0,
        CancellationToken cancellationToken = default)
    {
        if (indexFrom > pageIndex)
            throw new ArgumentException($"indexFrom ({indexFrom}) > pageIndex ({pageIndex})");

        if (pageSize <= 0)
            throw new ArgumentException("pageSize must be greater than zero");

        var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
        var items = await source.Skip((pageIndex - indexFrom) * pageSize)
                                .Take(pageSize)
                                .ToListAsync(cancellationToken)
                                .ConfigureAwait(false);

        return new PagedList<T>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            IndexFrom = indexFrom,
            TotalCount = count,
            Items = items,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };
    }
}
