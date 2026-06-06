namespace backend.Models;

public record ChatRequest
{
    public string Message { get; init; } = string.Empty;
}

public record ChatResponse
{
    public string Reply { get; init; } = string.Empty;
}