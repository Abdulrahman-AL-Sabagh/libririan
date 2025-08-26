namespace Libriran.Models
{
    public class Relationship
    {
        public string Name { get; set; } = null!;

        public string TargetModelName { get; set; } = null!;
        public RelationshipType RelationType { get; set; } // e.g., "OneToMany", "ManyToMany"

    }
}