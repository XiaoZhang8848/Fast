using Fast.EntityFrameworkCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fast.EntityFrameworkCore.Extensions;

public static class IServiceCollectionExtension
{
    public static void AddFastDbContext<TDbContext>(this IServiceCollection services,
        Action<DbContextOptionsBuilder> optionsAction) where TDbContext : FastDbContext
    {
        services.AddDbContext<FastDbContext, TDbContext>(optionsAction);
    }
}