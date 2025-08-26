
using DotNetEnv;
using Libriran.Models;
using Libriran.Util.Parser;
using Libririan;

namespace Libriran
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var envFilePath = Path.Combine(DatabaseConfigurator.projectRoot, ".env.development");
            Env.Load(envFilePath);
            bool setupExists = File.Exists(Path.Combine(DatabaseConfigurator.projectRoot, ".env.development"));
            var dbConfigurator = new DatabaseConfigurator(!setupExists);

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApplicationDbContext>(options => { dbConfigurator.ConnectToEf(options); });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (context.Database.CanConnect())
                {
                    Console.WriteLine("Database is ready");
                }
                else
                {
                    Console.WriteLine("Database is not ready");
                }
            }

            var _filePath = Directory.GetCurrentDirectory();
            var dirPath = Path.Combine(_filePath, "GeneratedProjects");
            Directory.CreateDirectory(dirPath);
            string filePath = Path.Combine(_filePath, "GeneratedProjects/models.json");


            var ser = new Serializer();
            
            Model model1 = new Model(
                                            "User",
                
                [
                    new ModelField { Name = "Id", Type = "int", IsNullable = false, IsSecret = false },
                    new ModelField { Name = "Username", Type = "string", IsNullable = false, IsSecret = false },
                    new ModelField { Name = "Password", Type = "string", IsNullable = false, IsSecret = true }
                ]
                                          
                                          )
            {
              
            };
            Model model2 = new Model(

                 "Post",

                [
                    new ModelField { Name = "Id", Type = "int", IsNullable = false, IsSecret = false },
                    new ModelField { Name = "Title", Type = "string", IsNullable = false, IsSecret = false },
                    new ModelField { Name = "Content", Type = "string", IsNullable = false, IsSecret = false },
                    new ModelField { Name = "UserId", Type = "int", IsNullable = false, IsSecret = false }
                ]
           );
            
            model2.Relations.Add(new Relationship
            {
                Name = "UserPosts",
                TargetModelName = "User",
                RelationType = RelationshipType.ManyToOne
            });

            ser.Serialize([model1, model2], filePath);
            
            var parser = new Parser();
            parser.Parse(filePath,dirPath);

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
