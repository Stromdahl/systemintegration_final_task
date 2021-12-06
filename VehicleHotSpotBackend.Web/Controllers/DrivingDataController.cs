using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleHotSpotBackend.Web.Services;
using VehicleHotSpotBackend.Web.Models;

namespace VehicleHotSpotBackend.Web.Controllers
{
    [ApiController]
    [Route("api/vehicle")]
    public class DrivingDataController : ControllerBase
    {
        private readonly DrivingDataService _context;

        public DrivingDataController(DrivingDataService context)
        {
            _context = context;
        }

        [HttpGet("drivingData")]
        public async Task<List<DrivingDataItem>> Get() =>
        await _context.GetAsync();

        [HttpGet("{vin}/drivingData")]
        public async Task<ActionResult<DrivingDataItem>> Get(string vin)
        {
            var drivingData = await _context.GetAsync(vin);

            if (drivingData == null)
            {
                return NotFound();
            }

            return drivingData;
        }

        [HttpPost("drivingData")]
        public async Task<IActionResult> Post(DrivingDataItem newDrivingData)
        {
            await _context.CreateAsync(newDrivingData);

            return CreatedAtAction(nameof(Get), new {id = newDrivingData.Id}, newDrivingData);
        }
    }
}
