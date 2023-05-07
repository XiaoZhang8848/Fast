using Fast.EntityFrameworkCore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fast.UnitOfWork;

public class UnitOfWorkMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next(context);

        var dbContexts = context.RequestServices.GetServices<FastDbContext>();
        foreach (var dbContext in dbContexts)
        {
            await dbContext.SaveChangesAsync();
        }
    }
}