using System.Linq.Expressions;

namespace Fast.Infrastructure.Extensions;

public static class IQueryableExtension
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition,
        Expression<Func<T, bool>> expression)
    {
        return condition ? source.Where(expression) : source;
    }
}