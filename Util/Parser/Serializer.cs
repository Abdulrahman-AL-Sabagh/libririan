using System.Text.Json;
using System.Text.Json.Serialization;

namespace Libriran.Util.Parser
{
    public class Serializer
    {

        private readonly string _filePath = Directory.GetCurrentDirectory();

        private string _generateJsonContent(HashSet<Model> models)
        {

            
            var data = new { models };
            JsonSerializerOptions options = new()
            { WriteIndented = true };
            return JsonSerializer.Serialize(data, options);
        }

        public void Serialize(HashSet<Model> models, string path)
        {
            string jsonContent = _generateJsonContent(models);
            File.WriteAllText(path, jsonContent);
            Console.WriteLine($"Models serialized to {path}");
        }





    }
}