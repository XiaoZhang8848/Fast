using Fast.Core;
using Fast.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fast.Snowflake;

[Using(
    typeof(FastDependencyInjectionModule)
)]
public class FastSnowflakeModule : IFastModule
{
    public void ConfigureService(IServiceCollection services)
    {
    }
}