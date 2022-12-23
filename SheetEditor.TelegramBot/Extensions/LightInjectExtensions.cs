using LightInject;

namespace SheetEditor.Extensions;

public static class LightInjectExtensions
{
    public static void RegisterDependencies(this ServiceContainer container)
    {
        container.RegisterAssembly(typeof(LightInjectExtensions).Assembly,
            lifetimeFactory: () => new PerScopeLifetime());
    }
}