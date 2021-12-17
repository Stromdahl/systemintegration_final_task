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

        [HttpPost("honk/{vin}")]
        public ActionResult honk(string vin, VehiclePositionItem vehiclePositionItem)
        {
            SqlConnection connection = connectToSqldb();
            
            SqlCommand command;
            SqlDataReader dataReader;
            string sql;

            connection.Open();

            sql =
                "DECLARE @point geography " +
                "SET @point = geography::Point(@longitude, @latitude, 4326) " +
                "SELECT dbo.vehicle.vin, dbo.vehicle.latitude, dbo.vehicle.longitude, geography::Point(dbo.vehicle.latitude, dbo.vehicle.longitude, 4326).STDistance(@point) FROM dbo.vehicle " +
                "WHERE dbo.vehicle.vin = @vin";

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
                return new NotFoundObjectResult("Not found");
            }


            var distance = dataReader.GetDouble(3);


            if (distance > 200)
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

        [HttpGet("position/{vin}")]
        public ActionResult GetVehiclePosition(string vin)
        {
            SqlConnection connection = connectToSqldb();

            SqlCommand command;
            SqlDataReader dataReader;
            string sql;

            connection.Open();

            sql = $"SELECT longitude, latitude from [dbo].[vehicle] WHERE vin = @vin";
            command = new SqlCommand(sql, connection);

            command.Parameters.Add(createParameter("@vin", vin));

            dataReader = command.ExecuteReader();

            dataReader.Read();


            if (!dataReader.HasRows)
            {
                command.Dispose();
                connection.Close();

                return new NotFoundResult();
            }

            double longitude = dataReader.GetDouble(0);
            double latitude = dataReader.GetDouble(1);

            return new OkObjectResult($"{longitude}, {latitude}");
        }


        [HttpGet("status/{vin}")]
        public ActionResult GetVehicleStatus(string vin)
        {
            SqlConnection connection = connectToSqldb();

            SqlCommand command;
            SqlDataReader dataReader;
            string sql;

            connection.Open();

            sql = $"SELECT * from [dbo].[vehicle] WHERE vin = @vin";
            command = new SqlCommand(sql, connection);

            command.Parameters.Add(createParameter("@vin", vin));

            dataReader = command.ExecuteReader();

            dataReader.Read();

            if (!dataReader.HasRows)
            {
                command.Dispose();
                connection.Close();

                return new NotFoundResult();
            }


            VehicleStatusItem vehicleStatus = new VehicleStatusItem()
            {
                batteryPercentage = 50,
                milage = 2000,
                tirePressure = new float[4] { 3f, 3f, 3f, 3f },
                locked = true,
                alarmArmed = false,
                longitude = 57.705500033177884f,
                latitude = 11.968206796607f
            };
            return new OkObjectResult(vehicleStatus);
        }
    }
}
