namespace Fast.Core;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class UsingAttribute : Attribute
{
    public UsingAttribute(params Type[] moduleTypes)
    {
        foreach (var type in moduleTypes.Where(x => !x.IsAssignableTo(typeof(IFastModule))))
        {
            throw new Exception($"{type.FullName}未实现{nameof(IFastModule)}接口");
        }

        ModuleTypes = moduleTypes;
    }

    public Type[] ModuleTypes { get; }
}