
using Microsoft.AspNetCore.Mvc;
using VehicleHotSpotBackend.Web.Models;
using Microsoft.Data.SqlClient;

namespace VehicleHotSpotBackend.Web.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {


        private SqlConnection connectToSqldb()
        {
            string connectionString;
            SqlConnection connection;

            connectionString = @"Data Source=DESKTOP-2S8VSFC\SQLEXPRESS;Initial Catalog=VehicleHotSpotDb; 
                        User ID=User;Password=password";

            connection = new SqlConnection(connectionString);

            return connection;
        }

        [HttpGet("authenticate")]
        public async Task<ActionResult<UserItem>> Login(string userName, string pwd)
        {
            HttpClient client = new HttpClient();
            UserItem user = null;
            string baseUrl = "https://kyhdev.hiqcloud.net/api/cds/v1.0/user/authenticate";

            HttpResponseMessage response = await client.GetAsync(baseUrl + $"?userName={userName}&pwd={pwd}");

            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<UserItem>();
                return user;
            }

            return NotFound();
        }



        // GET: api/UserItems/5
        [HttpGet("{customerId}")]
        public async Task<ActionResult<UserItem>> GetUserItem(string customerId)
        {
            
            SqlConnection connection = connectToSqldb();

            SqlCommand command;
            SqlDataReader dataReader;
            string sql;

            connection.Open();

            sql = $"SELECT * from [dbo].[user] WHERE customerId = '{customerId}'";
            command = new SqlCommand(sql, connection);

            dataReader = command.ExecuteReader();

            if(!dataReader.HasRows)
            {
                return new NotFoundResult();
            }

            dataReader.Read();
            UserItem user = new UserItem();
            user.customerId = (Guid)dataReader.GetValue(0);
            user.firstName = dataReader.GetString(1);
            user.lastName = dataReader.GetString(2);

            connection.Close();
            
            return new OkObjectResult(user);
        }

        // PUT: api/UserItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{customerId}")]
        public async Task<IActionResult> PutUserItem(Guid customerId, String firstName, String lastName)
        {
            UserItem userItem = new UserItem();
            userItem.customerId = customerId;
            userItem.firstName = firstName;
            userItem.lastName = lastName;

            SqlConnection connection = connectToSqldb();
            connection.Open();

            string sql = 
                $"UPDATE [dbo].[user] " +
                $"SET firstName = '{firstName}', lastName = '{lastName}'" +
                $"WHERE customerId = '{customerId}'";

            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();

            command = new SqlCommand(sql, connection);
            adapter.UpdateCommand = command;

            int rows = adapter.UpdateCommand.ExecuteNonQuery();

            command.Dispose();
            connection.Close();

            if(rows == 0)
            {
                return new BadRequestResult();
            }
            return new OkObjectResult(userItem);
        }

        // POST: api/UserItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserItem>> PostUserItem(UserItem userItem)
        {

            SqlConnection connection = connectToSqldb();
            connection.Open();

            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = $"INSERT INTO [dbo].[user] (customerId, firstName, lastName)" +
                $"VALUES('{userItem.customerId}', '{userItem.firstName}', '{userItem.lastName}')";

            command = new SqlCommand(sql, connection);

            adapter.InsertCommand = command;

            try
            {
                adapter.InsertCommand.ExecuteNonQuery();
            } catch (SqlException _)
            {
                return new BadRequestResult();
            } finally {
                command.Dispose();
                connection.Close();
            }
            
            return new OkObjectResult(userItem);
        }

        // DELETE: api/UserItems/5
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteUserItem(Guid customerId)
        {

            SqlConnection connection = connectToSqldb();
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = $"DELETE FROM [dbo].[user] WHERE customerId='{customerId}'";

            connection.Open();

            command = new SqlCommand(sql, connection);

            adapter.DeleteCommand = command;


            int rows = adapter.DeleteCommand.ExecuteNonQuery();

            command.Dispose();
            connection.Close();

            if (rows == 0)
            {
                return new BadRequestResult();
            }
            return new OkObjectResult($"User with id: {customerId} deleted");
        }
    }
}
