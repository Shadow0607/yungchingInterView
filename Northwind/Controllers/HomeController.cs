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
        private static async Task<IEnumerable<Home>> SelectAllShippers(SqlConnection connection)
        {
            return await connection.QueryAsync<Home>("select * from Shippers");
        }
    }
}

