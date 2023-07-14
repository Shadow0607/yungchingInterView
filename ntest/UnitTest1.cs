namespace ntest;

using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind;
using Northwind.Controllers;

public class Tests
{
    private IConfiguration _config;
    public Tests()
    {
        // Manually create Configuration
        var configBuilder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
        _config = configBuilder.Build();
        var path = Directory.GetCurrentDirectory();
        var connectionString = _config.GetConnectionString("DefaultConnection");
        Console.WriteLine(path);
        Console.WriteLine($"ConnectionString: {connectionString}");
    }
    [Test]
    public async Task Test1()
    {

        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        var deleteshipperid = new Home { CompanyName = "13123", Phone = "13212312312" };
        HomeController controller = new HomeController(_config);
        var result = await controller.CreateShippers(deleteshipperid);

        Console.WriteLine(result);
        Assert.NotNull(result);
    }

}
