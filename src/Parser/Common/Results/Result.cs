namespace Parser.Common.Results;

public sealed class Result
{
    private Result() { }

    public bool IsSucceeded { get; private set; }
    public object? Value { get; private set; }
    private Error? Error { get; set; }
    
    public static Result Success(object? value = null) => new() { IsSucceeded = true, Value = value };
    public static Result Fail(Error error) => new() { IsSucceeded = false, Error = error };

    public Error GetError() => Error!;
}