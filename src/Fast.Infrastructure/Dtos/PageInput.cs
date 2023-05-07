namespace Fast.Infrastructure.Dtos;

public class PageInput
{
    public int SkipCount { get; set; }
    public int TakeCount { get; set; }
    public string Sort { get; set; } = "Id DESC";
}