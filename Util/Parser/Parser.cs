using Libriran.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
                generateClass([model], outputDirectoryPath);
            }
            else
            {
                Console.WriteLine($"Unknown object type: {obj.GetType()}");
            }
        }
        _generateDbContext(models, outputDirectoryPath);
        _generateControllerClassTempalte(models, outputDirectoryPath);
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

    private void _generateDbContext(HashSet<Model> models, string outputDir)
    {
        var dbContextCode = $@"
using Microsoft.EntityFrameworkCore;
public class AppContext: DbContext
{{
    {string.Join(Environment.NewLine, models.Select(model => @$"    public DbSet<{model.Name}> {model.Name}s {{ get; set; }}").ToList())}
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

    private void _generateControllerClassTempalte(HashSet<Model> models, string outputDirPath)
    {

        foreach (var item in models)
        {
            var controllerNmae = $"{item.Name}Controller";
            var template = @$"
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""[controller]"")]
public class {controllerNmae}: ControllerBase {{
    private AppContext _context;
    public {item.Name}Controller(AppContext context) {{
        _context = context;
    }}
   {_generateGetAllControllerTemplate(item)}
   {_generateGetByIdControllerTemplate(item)}
   {_generatePostControllerTemplate(item)}
    {_generatePutControllerTemplate(item)}
    {_generateDeleteControllerTemplate(item)}
}}
";
            var fileName = Path.Combine(outputDirPath, $"{controllerNmae}.cs");
            File.WriteAllText(fileName, template);
        }

    }
    private string _generatePostControllerTemplate(Model m)
    {
        var contextName = "";
        var parameterName = m.Name.ToLower()[0];
        var controllerCode = $@"
[HttpPost]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400Request)]
public ActionResult<{m.Name}> Create({m.Name} {parameterName})
{{
    if ({parameterName} is null)
    {{
        return this.BadRequest();

    }}
    this._context.{m.Name}s.Add({parameterName});
    this._context.SaveChanges();
    return this.CreatedAtAction(nameof(this.GetById), new {{ id = {parameterName}.Id }}, {parameterName});

}}
";

        return controllerCode;


    }

    private string _generateGetByIdControllerTemplate(Model m)
    {
        var parameterName = "id";
        var controllerCode = $@"
        [HttpGet(""{{{parameterName}}}"")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<{m.Name}> GetById(int {parameterName})
        {{
            var item = this._context.{m.Name}s.Find({parameterName});
            if (item is null)
            {{
                return this.NotFound();
            }}
            return this.Ok(item);
        }}

";
        return controllerCode;

    }

    private string _generateGetAllControllerTemplate(Model m)
    {
        var controllerCode = $@"
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<{m.Name}>> GetAll()
        {{
            var items = this._context.{m.Name}s;
            return this.Ok(items);
        }}
            ";
        return controllerCode;
    }

    private string _generateDeleteControllerTemplate(Model m)
    {
        var parameterName = "id";
        var controllerCode = $@"
        [HttpDelete(""{{{parameterName}}}"")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int {parameterName})
        {{
            var item = this._context.{m.Name}s.Find({parameterName});
            if (item is null)
            {{
                return this.NotFound();
            }}
            this._context.{m.Name}s.Remove(item);
            this._context.SaveChanges();
            return this.NoContent();
        }}
";
        return controllerCode;
    }

    private string _generatePutControllerTemplate(Model m)
    {
        var parameterName = m.Name.ToLower()[0];
        var controllerCode = $@"
        [HttpPut(""{{id}}"")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, {m.Name} {parameterName})
        {{
            if (id != {parameterName}.Id)
            {{
                return this.BadRequest();
            }}
            var existingItem = this._context.{m.Name}s.Find(id);
            if (existingItem is null)
            {{
                return this.NotFound();
            }}
            this._context.Entry(existingItem).CurrentValues.SetValues({parameterName});
            this._context.SaveChanges();
            return this.NoContent();
        }}
    ";
        return controllerCode;
    }
}