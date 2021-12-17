using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using VehicleHotSpotBackend.Web.Models;

namespace VehicleHotSpotBackend.Web.Controllers
{
    [Route("api/VehicleActions")]
    public class VehicleActionsController: ControllerBase
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

        private SqlParameter createParameter<T>(string parameterName, T value)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = parameterName;
            param.Value = value;
            return param;
        }

        [HttpPost("{vin}")]
        public ActionResult honk(string vin, VehiclePositionItem vehiclePositionItem)
        {
            SqlConnection connection = connectToSqldb();
            
            SqlCommand command;
            SqlDataReader dataReader;
            string sql;

            connection.Open();

            sql =
                "DECLARE @point geography " +
                "SET @point = geography::Point(@latitude, @longitude, 4326) " +
                "SELECT v.vin, v.latitude, v.longitude, geography::Point(v.latitude, v.longitude, 4326).STDistance(@point) FROM dbo.vehicle AS v " +
                "WHERE geography::Point(v.latitude, v.longitude, 4326).STDistance(@point) <= 100 and v.vin = @vin";

            command = new SqlCommand(sql, connection);

            command.Parameters.Add(createParameter("@vin", vin));
            command.Parameters.Add(createParameter("@longitude", vehiclePositionItem.longitude));
            command.Parameters.Add(createParameter("@latitude", vehiclePositionItem.latitude));

            dataReader = command.ExecuteReader();
            dataReader.Read();

            if (!dataReader.HasRows)
            {
                command.Dispose();
                connection.Close();
                return new ObjectResult("Vehicle is to far away") { StatusCode = StatusCodes.Status403Forbidden };
            }
            var result = dataReader.GetString(0);
            command.Dispose();
            connection.Close();
            return new OkObjectResult(result);
        }
    }
}
