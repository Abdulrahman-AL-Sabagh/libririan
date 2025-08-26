using System.Text.Json.Serialization;

namespace Libriran.Models
{
  

    public class Model : DomainObject
    {
        public HashSet<ModelField> Fields { get; set; } = new();
        public HashSet<Relationship> Relations { get; set; } = new();

        public Model(string Name, HashSet<ModelField>? fields, HashSet<Relationship>? relations = null)
        {
            base.Name = Name;
            Fields = fields ?? [];
            Relations = relations ?? [];


        }

        public override string ToString()
        {
            return $"Model: {Name}, Fields: [{string.Join(", ", Fields)}], Relations: [{string.Join(", ", Relations)}]";
        }
    }
}