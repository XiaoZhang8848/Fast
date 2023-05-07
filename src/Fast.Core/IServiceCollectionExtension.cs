using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Fast.Core;

public static class IServiceCollectionExtension
{
    private static readonly List<Type> _fastModuleTypes = new();

    public static void AddModule<T>(this IServiceCollection services) where T : IFastModule
    {
        _fastModuleTypes.Add(typeof(T));
        GetModules(typeof(T));
        foreach (var moduleType in _fastModuleTypes)
        {
            Console.WriteLine(moduleType.FullName);
            var module = (IFastModule)Activator.CreateInstance(moduleType)!;
            module.ConfigureService(services);
        }
    }

    private static void GetModules(Type type)
    {
        var moduleTypes = type.GetCustomAttributes<UsingAttribute>()
            .SelectMany(x => x.ModuleTypes)
            .Where(x => !_fastModuleTypes.Contains(x)); // 去重

        foreach (var moduleType in moduleTypes)
        {
            _fastModuleTypes.Add(moduleType);
            GetModules(moduleType);
        }
    }
}