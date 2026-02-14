namespace Parser.Mediator;

public interface ISender
{
    public ValueTask<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, 
        CancellationToken cancellationToken = default);
}