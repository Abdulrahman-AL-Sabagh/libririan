using Libririan;
using Libririan.Components;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

Env.Load();
var setupExists = File.Exists(Path.Combine(DatabaseConfigurator.projectRoot, ".env.development"));
if (!setupExists)
{
    Console.WriteLine("Do you have a database [y|n]");
    string? dbExists = Console.ReadLine();
    switch (dbExists?.ToLower())
    {
        case "y":
            break;
        case "n":
            Console.WriteLine("Do you want us to setup the database for you? (Only for dev mode!) [y|n]");
            string? wantsToSetup = Console.ReadLine();
            switch (wantsToSetup?.ToLower())


            {
                case "y":

                    Console.WriteLine("Which database do you like to use?");
                    Console.WriteLine("""
                                      - Postgres (Coming Soon)
                                      - MySQL    (Coming Soon)
                                      - MSSQL    
                                      - H2      (Coming Soon)
                                      - MongoDB  (Coming Soon)
                                      """);
                    Console.WriteLine("Enter Database name");
                    string? dbName = Console.ReadLine();
                    break;
                case "n":
                    Console.WriteLine("Which database do you like to use?");
                    break;
            }

            break;
    }
}

var dbConfigurator = new DatabaseConfigurator(!setupExists);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => { dbConfigurator.ConnectToEf(options); });




// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
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

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();