namespace Parser.Mapping;

public interface IMapper<in TInput, out TOutput>
{
    public TOutput Map(TInput input);
}