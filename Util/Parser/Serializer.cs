using System.Text.Json;
using System.Text.Json.Serialization;

namespace Libriran.Util.Parser
{
    public class Serializer
    {

        private readonly string _filePath = Directory.GetCurrentDirectory();

        private string _generateJsonContent(HashSet<Model> models)
        {

            var allModels = models.ToDictionary(
                model => model.Name!,
                model => new
                {
                    fields = model.Fields.ToDictionary(
                        field => field.Name,
                        field => new
                        {
                            type = field.Type,
                            isNullable = field.IsNullable,
                            isSecret = field.IsSecret
                        }
                    ),
                    relations = model.Relations.ToDictionary(
                            relation => relation.Name,
                            relation => new
                            {
                                sourceField = relation.SourceField.Name,
                                targetField = relation.TargetField.Name,
                                relationType = relation.RelationType.ToString()
                            }
                        )

                }


            );
            var data = new { models = allModels };
            var options = new JsonSerializerOptions

            { WriteIndented = true };
            return JsonSerializer.Serialize(data, options);
        }

        public void Serialize(HashSet<Model> models)
        {
            string jsonContent = _generateJsonContent(models);
            Directory.CreateDirectory(Path.Combine(_filePath, "GeneratedProjects"));
            string filePath = Path.Combine(_filePath, "GeneratedProjects/models.json");
            File.WriteAllText(filePath, jsonContent);
            Console.WriteLine($"Models serialized to {filePath}");
        }





    }
}