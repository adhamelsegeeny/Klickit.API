using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using System.Drawing.Text;


namespace ProductsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            string query = @"
                    select * from dbo.Users where Username = @Username and Password = @Password";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Username", user.Username);
                    myCommand.Parameters.AddWithValue("@Password", user.Password);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            if (table.Rows.Count > 0)
            {
                return Ok(new
                {
                    message= "Login Success",
                    username= user.Username
                });
            }
            else
            {
                return NotFound(new
                {
                    message = "User not found"
                });
            }
        }
    }
}
