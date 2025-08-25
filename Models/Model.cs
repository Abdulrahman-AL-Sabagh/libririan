public class Model
{
    public string? Name { get; set; }
    public HashSet<ModelField> Fields { get; set; } = new();
    public HashSet<Relationship> Relations { get; set; } = new();
}