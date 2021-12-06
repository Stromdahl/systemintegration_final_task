using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleHotSpotBackend.Web.Models;
using VehicleHotSpotBackend.Core.Integrations;
using Microsoft.Data.SqlClient;

namespace VehicleHotSpotBackend.Web.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }

        // GET: api/UserItems
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<UserItem>>> GetUserItems()
        //{
        //    return await _context.UserItems.ToListAsync();
        //}

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
        [HttpGet("search")]
        public async Task<ActionResult<UserItem>> GetUserItem(string searchString)
        {
            string connectionString;
            SqlConnection connection;

            connectionString = @"Data Source=DESKTOP-2S8VSFC\SQLEXPRESS;Initial Catalog=VehicleHotSpotDb; 
                        User ID=User;Password=password";

            connection = new SqlConnection(connectionString);

            connection.Open();


            Console.WriteLine("Connected");
            connection.Close();
            Console.WriteLine("Disconnected");
            return null;
        }

        // PUT: api/UserItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserItem(Guid id, UserItem userItem)
        {
            if (id != userItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(userItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserItem>> PostUserItem(UserItem userItem)
        {
            string connectionString;
            SqlConnection connection;

            connectionString = @"Data Source=DESKTOP-2S8VSFC\SQLEXPRESS;Initial Catalog=VehicleHotSpotDb; 
                        User ID=User;Password=password";

            connection = new SqlConnection(connectionString);

            connection.Open();

            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "";

            sql = ""

            connection.Close();


            _context.UserItems.Add(userItem);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetUserItem", new { id = userItem.Id }, userItem);
            return CreatedAtAction(nameof(GetUserItem), new { id = userItem.Id }, userItem);
        }

        // DELETE: api/UserItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserItem(long id)
        {
            var userItem = await _context.UserItems.FindAsync(id);
            if (userItem == null)
            {
                return NotFound();
            }

            _context.UserItems.Remove(userItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserItemExists(Guid id)
        {
            return _context.UserItems.Any(e => e.Id == id);
        }
    }
}
