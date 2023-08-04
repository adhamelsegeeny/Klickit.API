using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using System.Drawing.Text;

namespace ProductsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private IConfiguration _configuration;
        public RequestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]

        public async Task<IActionResult> AddRequest(Request request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            string query = @"
                    insert into dbo.Requests (username, product_id) values (@Username, @Product)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Username", request.Username);
                    myCommand.Parameters.AddWithValue("@Product", request.Product_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(new
            {
                message = "Request added successfully"
            });
        }

    }
}
