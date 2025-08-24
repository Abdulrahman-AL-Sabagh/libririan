
using DotNetEnv;
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



            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
