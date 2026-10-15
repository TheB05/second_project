namespace second_project.Models;

public sealed record SumadoraRequest
{
    public required decimal Numero1 { get; init; }

    public required decimal Numero2 { get; init; }
}
