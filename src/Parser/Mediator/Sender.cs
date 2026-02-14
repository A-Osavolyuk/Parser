namespace Parser.Mediator;

public sealed class Sender(IServiceProvider serviceProvider) : ISender
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async ValueTask<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, 
        CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = _serviceProvider.GetRequiredService(handlerType);
        var method = handlerType.GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.HandleAsync));
        if (method is null) throw new Exception("Handler does not implement IRequestHandler<,>");

        if (method.Invoke(handler, [request, cancellationToken]) is not ValueTask<TResponse> result)
            throw new Exception("Invalid handler"); 
        
        return await result;
    }
}