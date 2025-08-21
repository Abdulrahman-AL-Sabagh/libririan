using Microsoft.EntityFrameworkCore;

namespace Libririan;

public class DatabaseConfigurator
{
    private string? dbName;
    private string? username;
    private string? password;
    public DatabaseProvider? dbProvider { get; set; }
    private SelctableCli selctableCli;

    public static string projectRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../..");

    public DatabaseConfigurator(bool doInit = false)
    {
        if (doInit)
        {
            selctableCli = new SelctableCli();

            SetupDatabase();
        }
        else
        {
            username = Environment.GetEnvironmentVariable("Username");
            password = Environment.GetEnvironmentVariable("Password");
            dbName = Environment.GetEnvironmentVariable("dbName");
            dbProvider = Enum.Parse<DatabaseProvider>(Environment.GetEnvironmentVariable("db") ?? throw new InvalidOperationException());
        }
    }

    private void SetupDatabase()
    {
        Console.WriteLine(
            "Do you prefer a dockerized version or an installed version? [d for dockerized/ i for installed]");
        Console.WriteLine("Do you have the database installed");
        SetupDatabaseCredentials();
        CreateEnvFile();
    }

    private void SetupDatabaseCredentials()
    {
        Console.WriteLine("Enter the username: ");
        username = Console.ReadLine();
        Console.WriteLine("Enter the password: ");
        password = Console.ReadLine();
        Console.WriteLine("Enter the database name:");
        dbName = Console.ReadLine();
        Console.WriteLine("Enter the database provider: ");
        dbProvider =
            Enum.Parse<DatabaseProvider>(selctableCli.selectableValue(Enum.GetNames(typeof(DatabaseProvider))));
    }

    private string ConstructConnectionString()
    {
        return dbProvider switch
        {
            DatabaseProvider.MSSQL =>
                $@"Server=localhost;Database={dbName};User Id={username};Password={password};Encrypt=False",
            DatabaseProvider.MYSQL => "",
            DatabaseProvider.MONGO => "",
            DatabaseProvider.SQLITE => "",
            DatabaseProvider.POSTGRESQL => "",
            _ => throw new Exception("Unknown database provider")
        };
    }

    private void createAppDatabase()
    {
    }

    public void ConnectToEf(DbContextOptionsBuilder options)
    {
        var connectionString = ConstructConnectionString();
        Console.WriteLine(connectionString);
        switch (dbProvider)
        {
            // handle this 
            case DatabaseProvider.MONGO:
                //  options.UseMongoDB(connectionString);
                // handle mongo
                break;
            case DatabaseProvider.MSSQL:
                // handle MSSQL
                options.UseSqlServer(connectionString).LogTo(Console.WriteLine, LogLevel.Information);
                break;
            case DatabaseProvider.MYSQL:
                // handle MYSQL
                options.UseMySQL(connectionString);
                break;
            case DatabaseProvider.POSTGRESQL:
                // handle POSTGRES
                options.UseNpgsql(connectionString);

                break;
            case DatabaseProvider.SQLITE:
                // handle SQLITE
                options.UseSqlite(connectionString);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CreateEnvFile()
    {
        var envPath = Path.Combine(projectRoot, ".env.development");
        File.WriteAllText(envPath,
            $"""
             Username="{username}"
             Password="{password}"
             db="{dbProvider.ToString()}"
             dbName="{dbName}"
             """);
        Console.WriteLine($"Created environment file {envPath}");
    }
}