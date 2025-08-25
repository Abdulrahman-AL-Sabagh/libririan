using Libriran.Models;
using System.Text.Json;

class Parser
{

    private string _projectFolder = Directory.GetCurrentDirectory();

    public void Parse(string path)
    {

        ParsedModels models;
        if (path.CompareTo(string.Empty) == 0  || !File.Exists(path))
        {
            throw new FileNotFoundException($"The file at path {path} was not found.");
        }


        using StreamReader r = new(path);
        string json = r.ReadToEnd();
        Console.WriteLine(json);
        models = JsonSerializer.Deserialize<ParsedModels>(json,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        foreach (var model in models.Models)
        {
            Console.WriteLine($"Model: {model.Name}");
            foreach (var field in model.Fields)
            {
                Console.WriteLine($"  Field: {field.Name}, Type: {field.Type}, IsNullable: {field.IsNullable}, IsSecret: {field.IsSecret}");
            }
            foreach (var relation in model.Relations)
            {
                Console.WriteLine($"  Relation: {relation.Name}, SourceField: {relation.SourceField.Name}, TargetField: {relation.TargetField.Name}, RelationType: {relation.RelationType}");
            }
        }
    }


}