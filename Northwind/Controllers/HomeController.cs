using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace Northwind.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Home>>> GetAllShippers()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<Home> heroes = await SelectAllShippers(connection);
            return Ok(heroes);
        }
        [HttpGet("{ShipperID}")]
        public async Task<ActionResult<Home>> GetShippers(int ShipperID)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var hero = await connection.QueryFirstAsync<Home>("select * from Shippers where ShipperID = @ShipperID",
                    new { ShipperID = ShipperID });
            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<List<Home>>> CreateShippers(Home Shippers)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into  Shippers(CompanyName, Phone) values (@CompanyName, @Phone)", Shippers);
            return Ok("Insert Database OK");
        }

        [HttpPut]
        public async Task<ActionResult<List<Home>>> UpdateHero(Home Shippers)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("update Shippers set CompanyName = @CompanyName, Phone = @Phone where ShipperID = @ShipperID", Shippers);
            return Ok(await SelectAllShippers(connection));
        }

        [HttpDelete("{ShipperID}")]
        public async Task<ActionResult<List<Home>>> DeleteHero(int ShipperID)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("delete from Shippers where ShipperID = @ShipperID", new { ShipperID = ShipperID });
            return Ok(await SelectAllShippers(connection));
        }

        private static async Task<IEnumerable<Home>> SelectAllShippers(SqlConnection connection)
        {
            return await connection.QueryAsync<Home>("select * from Shippers");
        }
    }
}

