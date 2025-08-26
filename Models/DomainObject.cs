using System.Text.Json.Serialization;

namespace Libriran.Models
{

    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(Model), "model")]
    //[JsonDerivedType(typeof(Relationship), "relationship")]

    public abstract class DomainObject
    {
      public string Name { get; set; } = null!;
    

    }
}
