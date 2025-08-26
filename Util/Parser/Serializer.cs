using Libriran.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Libriran.Util.Parser
{
    public class Serializer
    {

        private readonly string _filePath = Directory.GetCurrentDirectory();

        private string _generateJsonContent(HashSet<DomainObject> objects)
        {

            
            var data = new { Objects = objects };
            JsonSerializerOptions options = new()
            { WriteIndented = true, Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            };
            return JsonSerializer.Serialize(data, options);
        }

        public void Serialize(HashSet<DomainObject> objects, string path)
        {
            string jsonContent = _generateJsonContent(objects);
            File.WriteAllText(path, jsonContent);
            Console.WriteLine($"Models serialized to {path}");
        }
    }
}