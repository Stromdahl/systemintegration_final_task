
using Microsoft.AspNetCore.Mvc;
using VehicleHotSpotBackend.Web.Models;
using VehicleHotSpotBackend.Core;
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
        public async Task<ActionResult<LoginResponse>> Login(string userName, string pwd)
        {
            HttpClient client = new HttpClient();
            LoginResponse user = null;
            string baseUrl = "https://kyhdev.hiqcloud.net/api/cds/v1.0/user/authenticate";

            HttpResponseMessage response = await client.GetAsync(baseUrl + $"?userName={userName}&pwd={pwd}");

            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<LoginResponse>();
                ServiceHandler.Current.InMemoryStorage.AddToken(user.accessToken, user.id);
                return new OkObjectResult(user);
            }
            
            return new UnauthorizedResult();
        }



        // GET: api/UserItems/5
        [HttpGet("{customerId}")]
        public ActionResult GetUserItem(string customerId)
        {   
            SqlConnection connection = connectToSqldb();

            SqlCommand command;
            SqlDataReader dataReader;
            string sql;

            connection.Open();

            sql = $"SELECT * from [dbo].[user] WHERE customerId = '{customerId}'";
            command = new SqlCommand(sql, connection);

            dataReader = command.ExecuteReader();
            command.Dispose();
            connection.Close();

            if (!dataReader.HasRows)
            {
                return new NotFoundResult();
            }

            dataReader.Read();
            UserItem user = new UserItem();
            user.customerId = (Guid)dataReader.GetValue(0);
            user.firstName = dataReader.GetString(1);
            user.lastName = dataReader.GetString(2);

            
            
            return new OkObjectResult(user);
        }



        // PUT: api/UserItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{customerId}")]
        public ActionResult PutUserItem(Guid customerId, String firstName, String lastName)
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
        public ActionResult PostUserItem(UserItem userItem)
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

        [HttpPost("{customerId}/{vin}")]
        public async Task<ActionResult> PersistUserVehicleRelation(Guid customerId, string vin)
        {
            UserWithVehicles user = null;
            string baseUrl = "https://kyhdev.hiqcloud.net/cds/v1.0/customer/";

            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(baseUrl + $"{customerId}"),
                Method = HttpMethod.Get
                
            };

            //request.Headers.Add("kyh-auth", ServiceHandler.Current.InMemoryStorage.GetUserToken(customerId));
            request.Headers.Add("kyh-auth", "i3jsC6xofu8VIc4Znah6Acsr56T8x1GCuwjBAhYtcu2PEskZWwzQfaU6I9owlq9f");
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<UserWithVehicles>();

                foreach (VehicleItem v in user.vehicles)
                {
                    if (vin == v.vin)
                    {
                        SqlConnection connection = connectToSqldb();
                        connection.Open();


                        SqlCommand command;
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        string sql =
                            $"INSERT INTO dbo.vehicleUserRelation (customerId, vin) " +
                            $"VALUES('{customerId}', '{vin}')";

                        command = new SqlCommand(sql, connection);

                        adapter.InsertCommand = command;

                        int rows = adapter.InsertCommand.ExecuteNonQuery();

                        command.Dispose();
                        connection.Close();

                        if (rows == 0)
                        {
                            return new BadRequestResult();
                        }
                        return new OkObjectResult($"User vehicle relation created");
                    }
                }
            }
            return new UnauthorizedResult();
        }

        [HttpGet("{customerId}/vehicle")]
        public ActionResult GetUserVehicles(Guid customerId)
        {
            

            SqlConnection connection = connectToSqldb();
            connection.Open();

            SqlCommand command;
            SqlDataReader dataReader;
            string sql = 
                $"SELECT vu.customerId, vu.vin, v.regNo " +
                $"FROM dbo.vehicleUserRelation vu " +
                $"INNER JOIN dbo.vehicle v on vu.vin = v.vin " +
                $"WHERE vu.customerId = '{customerId}'";

            command = new SqlCommand(sql, connection);
            dataReader = command.ExecuteReader();

            command.Dispose();
            connection.Close();

            List<VehicleUserRelation> vehicleUserRelations = new List<VehicleUserRelation>();

            while (dataReader.Read())
            {
                vehicleUserRelations.Add(new VehicleUserRelation() { 
                    customerId = (Guid)dataReader.GetValue(0), vin = dataReader.GetString(1), regNo = dataReader.GetString(2)});
            }

            if (vehicleUserRelations.Count == 0)
            {
                return new NotFoundResult();
            }

            

            return new OkObjectResult(vehicleUserRelations);
        }

        // DELETE: api/UserItems/5
        [HttpDelete("{customerId}")]
        public ActionResult DeleteUserItem(Guid customerId)
        {

            SqlConnection connection = connectToSqldb();
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = 
                $"DELETE FROM [dbo].[user] WHERE customerId='{customerId}';" +
                $"DELETE FROM [dbo].[vehicleUserRelation] WHERE customerId='{customerId}';";

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
