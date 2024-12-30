using TSP.Domain.Primitives;

namespace TSP.Domain.Entities;

public sealed class Rank : SystemTable
{
    public string Title { get; set; } = null!;
}
