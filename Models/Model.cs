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
            Kind = "Model";
            Fields = fields ?? [];
            Relations = relations ?? [];


        }

        public override string ToString()
        {
            return $"Model: {Name}, Kind: ${this.Kind}, Fields: [{string.Join(", ", Fields)}], Relations: [{string.Join(", ", Relations)}]";
        }
    }
}