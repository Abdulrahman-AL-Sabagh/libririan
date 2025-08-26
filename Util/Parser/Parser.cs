using Libriran.Models;
using System.Text.Json;

class Parser
{

    private string _projectFolder = Directory.GetCurrentDirectory();

    public void Parse(string path)
    {

        ParsedDomainObjects objects;
        if (path.CompareTo(string.Empty) == 0  || !File.Exists(path))
        {
            throw new FileNotFoundException($"The file at path {path} was not found.");
        }


        using StreamReader r = new(path);
        string json = r.ReadToEnd();
        Console.WriteLine(json);
        objects = JsonSerializer.Deserialize<ParsedDomainObjects>(json,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        foreach (var obj in objects.Objects)
        {
           Console.Write(obj.ToString());
        }
    }


}