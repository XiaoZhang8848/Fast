using Microsoft.Extensions.DependencyInjection;

namespace Fast.Core;

public interface IFastModule
{
    void ConfigureService(IServiceCollection services);
}