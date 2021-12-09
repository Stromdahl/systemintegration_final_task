using Microsoft.AspNetCore.Mvc;
using VehicleHotSpotBackend.Web.Models;
using Microsoft.Data.SqlClient;

namespace VehicleHotSpotBackend.Web.Controllers
{
    [Route("api/vehicle")]
    public class VehicleController : ControllerBase
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

        [HttpGet("{vin}")]
        public async Task<ActionResult<VehicleItem>> GetVehicleItem(string vin)
        {
            SqlConnection connection = connectToSqldb();

            SqlCommand command;
            SqlDataReader dataReader;
            string sql;

            connection.Open();

            sql = $"SELECT * from [dbo].[vehicle] WHERE vin = '{vin}'";
            command = new SqlCommand(sql, connection);

            dataReader = command.ExecuteReader();

            if (!dataReader.HasRows)
            {
                return new NotFoundResult();
            }

            dataReader.Read();
            VehicleItem vehicle = new VehicleItem();
            vehicle.vin = dataReader.GetString(0);
            vehicle.regNo = dataReader.GetString(1);

            connection.Close();

            return new OkObjectResult(vehicle);
        }

        [HttpPost]
        public async Task<ActionResult<UserItem>> PostVehicleItem(VehicleItem vehicleItem)
        {

            SqlConnection connection = connectToSqldb();
            connection.Open();

            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = $"INSERT INTO dbo.vehicle (vin, regNo)" +
                $"VALUES('{vehicleItem.vin}', '{vehicleItem.regNo}')";

            command = new SqlCommand(sql, connection);

            adapter.InsertCommand = command;

            try
            {
                adapter.InsertCommand.ExecuteNonQuery();
            }
            catch (SqlException _)
            {
                return new BadRequestResult();
            }
            finally
            {
                command.Dispose();
                connection.Close();
            }

            return new OkObjectResult(vehicleItem);
        }


        [HttpDelete("{vin}")]
        public async Task<IActionResult> DeleteVehicleItem(string vin)
        {
            SqlConnection connection = connectToSqldb();
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = 
                $"DELETE FROM dbo.vehicle WHERE vin='{vin}';" +
                $"DELETE FROM dbo.vehicleUserRelation WHERE vin='{vin}'";

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
            return new OkObjectResult($"Vehicle with vin: {vin} deleted");
        }
    }
}
