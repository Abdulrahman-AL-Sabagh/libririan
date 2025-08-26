using Libriran.Models;

public class Relationship
{
    public string Name { get; set; }
    public ModelField SourceField { get; set; }
    public ModelField TargetField { get; set; }
    public RelationshipType RelationType {get; set; } // e.g., "OneToMany", "ManyToMany"
 
}