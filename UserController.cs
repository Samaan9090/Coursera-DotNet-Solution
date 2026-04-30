using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static readonly ConcurrentDictionary<int, User> _users = new ConcurrentDictionary<int, User>(
            new Dictionary<int, User>
            {
                { 1, new User { Id = 1, Name = "John Doe", Email = "john.doe@example.com" } },
                { 2, new User { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com" } }
            });

        private static int _nextId = 2;

        // GET: api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(_users.Values.ToList());
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            if (!_users.TryGetValue(id, out var user))
            {
                return NotFound(new { error = "User not found." });
            }
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] User user)
        {
            user.Id = Interlocked.Increment(ref _nextId);
            _users[user.Id] = user;
            
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (!_users.TryGetValue(id, out var existingUser))
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
            if (!_users.TryRemove(id, out _))
            {
                return NotFound(new { error = "User not found." });
            }

            return NoContent();
        }
    }
}