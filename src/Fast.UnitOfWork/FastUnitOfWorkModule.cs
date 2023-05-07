using Fast.Core;
using Fast.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fast.UnitOfWork;

[Using(
    typeof(FastEntityFrameworkCoreModule)
)]
public class FastUnitOfWorkModule : IFastModule
{
    public void ConfigureService(IServiceCollection services)
    {
        services.AddSingleton<UnitOfWorkMiddleware>();
    }
}