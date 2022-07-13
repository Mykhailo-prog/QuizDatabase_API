using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizDatabase_API.DTO;
using QuizDatabase_API.Models;

namespace QuizDatabase_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly QuizContext _context;
        public UserController(QuizContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO userdto)
        {
            var user = new User
            {
                Age = userdto.Age,
                Name = userdto.Name,
                Score = userdto.Score
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(ItemToDTO(user));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(long id, UserDTO userdto)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if ( user != null) return BadRequest();

            user.Name = userdto.Name;
            user.Age = userdto.Age;
            user.Score = userdto.Score;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UserExists(id))
            {
                if (!UserExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(ItemToDTO(user));
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(u => u.Id == id);
        }
        private static UserDTO ItemToDTO(User user) => new UserDTO
        {
            Age = user.Age,
            Name = user.Name,
            Score = user.Score
        };
    }
}
