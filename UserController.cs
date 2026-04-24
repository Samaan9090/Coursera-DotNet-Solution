using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static List<User> Users = new List<User>
        {
            new User { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
            new User { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com" }
        };

        // GET: api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(Users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { error = "User not found." });
            }
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public ActionResult<User> CreateUser(User user)
        {
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email))
            {
                return BadRequest(new { error = "Name and Email are required." });
            }

            user.Id = Users.Max(u => u.Id) + 1;
            Users.Add(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            var existingUser = Users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                return NotFound(new { error = "User not found." });
            }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { error = "User not found." });
            }

            Users.Remove(user);
            return NoContent();
        }
    }
}