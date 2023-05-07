namespace Fast.Infrastructure.Dtos;

public class PageOutput<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int Count { get; set; }
}