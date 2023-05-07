using Fast.Core;
using Fast.Infrastructure;
using Fast.Snowflake;
using Microsoft.Extensions.DependencyInjection;

namespace Fast.EntityFrameworkCore;

[Using(
    typeof(FastSnowflakeModule),
    typeof(FastInfrastructureModule)
)]
public class FastEntityFrameworkCoreModule : IFastModule
{
    public void ConfigureService(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
    }
}