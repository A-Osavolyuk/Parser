namespace Parser.Mediator;

public static class MediatorExtensions
{
    public static void AddMediator<TMarker>(this IServiceCollection services)
    {
        services.AddSingleton<ISender, Sender>();
        
        var types = typeof(TMarker).Assembly.GetTypes()
            .Where(x => x is { IsAbstract: false, IsInterface: false });

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces()
                .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

            foreach (var @interface in interfaces)
            {
                services.AddTransient(@interface, type);
            }
        }
    }
}