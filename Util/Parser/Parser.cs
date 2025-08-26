using Libriran.Models;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

class Parser
{

    private string _projectFolder = Directory.GetCurrentDirectory();
    private string _contextName = "appContext";

    public void Parse(string jsonPath, string outputDirectoryPath)
    {
        Console.WriteLine($"Parsing file at path: {jsonPath}");

        ParsedDomainObjects objects;
        if (!File.Exists(jsonPath))
        {
            throw new FileNotFoundException($"The file at path {jsonPath} was not found.");
        }


        using StreamReader r = new(jsonPath);
        string json = r.ReadToEnd();
        objects = JsonSerializer.Deserialize<ParsedDomainObjects>(json,
    new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    });
        HashSet<Model> models = [];
        foreach (DomainObject obj in objects.Objects)
        {
           if (obj is Model model)
           {
                models.Add(model);
               generateClass([model],outputDirectoryPath);
            }
           else
           {
               Console.WriteLine($"Unknown object type: {obj.GetType()}");
            }
        }
        generateDbContext(models,outputDirectoryPath);
    }

    private void generateClass(HashSet<Model> models, string outputDirPath)
    {
        foreach (var item in models)
        {
            var fields = _generateFields(item.Fields);
            var relations = _generateRelations(item.Relations);
            var indent = "    ";
        var classCode = $@"
using System.Text.Json.Serialization;

public class {item.Name}
{{
{string.Join(Environment.NewLine, fields.Select(f => indent + f))}
{string.Join(Environment.NewLine, relations.Select(r => indent + r))}
}}";
           var fileName = Path.Combine(outputDirPath, $"{item.Name}.cs");
           File.WriteAllText(fileName, classCode);
        }
    }

    private void generateDbContext(HashSet<Model> models, string outputDir)
    {
        var dbContextCode = $@"
using Microsoft.EntityFrameworkCore;
public class AppContext: DbContext
{{
    {string.Join(Environment.NewLine,models.Select(model => @$"    public DbSet<{model.Name}> {model.Name.ToLower()} {{ get; set; }}").ToList())}
}}
";
        File.WriteAllText(
            path: Path.Combine(outputDir, "AppContext.cs"),
            contents: dbContextCode
            );
           
    }
    private HashSet<string> _generateFields(HashSet<ModelField> fields)
    {

        var fieldSet = new HashSet<string>();
        foreach (var item in fields)
        {
            var field = $"{(item.IsSecret ? "[JsonIgnore]" : "")}" + 
                $"public {(!item.IsNullable ? "required" : "")} {item.Type} {item.Name} {{ get; set; }}";
            fieldSet.Add(field);
        }
        return fieldSet;
    }

    private HashSet<string> _generateRelations(HashSet<Relationship> relations) 
    {
        var relationSet = new HashSet<string>();
        foreach (var item in relations)
        {
            var relation = item.RelationType switch
            {
                RelationshipType.OneToMany => $"public List<{item.TargetModelName}> {item.TargetModelName}s {{ get; set; }} = new();",
                RelationshipType.ManyToOne => $"public {item.TargetModelName} {item.TargetModelName} {{ get; set; }}",
                RelationshipType.OneToOne => $"public {item.TargetModelName} {item.TargetModelName} {{ get; set; }}",
                RelationshipType.ManyToMany => $"public List<{item.TargetModelName}> {item.TargetModelName}s {{ get; set; }} = new();",
                _ => throw new Exception("Unknown relationship type")
            };
            relationSet.Add(relation);
        }
        return relationSet;
    }


    private string generatePostController(Model m)
    {
        var contextName = "";
        var parameterName = m.Name[0];
        var controllerCode = $@"""
[HttpPost]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400Request)]
public ActionResult<{m.Name}> Create(${m.Name} {m.Name[0]})
{{
    if ({parameterName} is null)
    {{
        return this.BadRequest();

    }}

}}
""";

        return controllerCode;


    }

}