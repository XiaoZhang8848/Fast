using Fast.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Fast.DependencyInjection;

[Using]
public class FastDependencyInjectionModule : IFastModule
{
    private readonly IEnumerable<Type> _exportedTypes = AppDomain.CurrentDomain.GetAssemblies()
        .Where(x => !x.FullName!.StartsWith("System.") && !x.FullName!.StartsWith("Microsoft."))
        .SelectMany(x => x.GetExportedTypes())
        .Where(x => !x.IsAbstract);

    public void ConfigureService(IServiceCollection services)
    {
        RegisterService(typeof(ITransientTag), services.AddTransient);
        RegisterService(typeof(IScopedTag), services.AddScoped);
        RegisterService(typeof(ISingletonTag), services.AddSingleton);
    }

    private void RegisterService(Type tag, Func<Type, Type, IServiceCollection> addService)
    {
        foreach (var implement in _exportedTypes.Where(x => x.IsAssignableTo(tag)))
        {
            var typeIsGenericTypeDefinition = implement.IsGenericTypeDefinition;
            foreach (var @interface in implement.GetInterfaces().Where(x => x != tag))
            {
                if (typeIsGenericTypeDefinition)
                {
                    addService(@interface.GetGenericTypeDefinition(), implement.GetGenericTypeDefinition());
                }
                else
                {
                    addService(@interface, implement);
                }
            }
        }
    }
}