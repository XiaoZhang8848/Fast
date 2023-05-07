using Microsoft.AspNetCore.Builder;

namespace Fast.UnitOfWork;

public static class IApplicationBuilderExtension
{
    public static void UseUnitOfWork(this IApplicationBuilder app)
    {
        app.UseMiddleware<UnitOfWorkMiddleware>();
    }
}