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
            string query = @"
                    insert into dbo.Requests values
                    (@Username,@Product_id,@Product_name,@Product_price)
                    ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {

                    myCommand.Parameters.AddWithValue("@Username", request.Username);
                    myCommand.Parameters.AddWithValue("@Product_id", request.Product_id);
                    myCommand.Parameters.AddWithValue("@Product_name", request.Product_name);
                    myCommand.Parameters.AddWithValue("@Product_price", request.Product_price);
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

        //entering the username in the path gets the requests
        [HttpGet("{username}")]
        public async Task<IActionResult> GetRequests(String username)
        {
            string query = @"
                    select * from dbo.Requests where username = @Username";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    //get the username from the path

                    myCommand.Parameters.AddWithValue("@Username",username);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(table);
        }

        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            string query = @"
                    select * from dbo.Requests";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            
        }
            return Ok(table);
        }


    }
}
