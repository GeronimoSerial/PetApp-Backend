using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;
using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly BootcampDbContext _db;
        public UsersController(BootcampDbContext db)
        {
            _db = db;
        }
        [HttpPost("PostUser")]
        public async Task<ActionResult> PostUser(User user)
        {
             _db.Users.Add(user);
             _db.SaveChanges();
            return Ok();
        //     await _db.SaveChangesAsync();
        //     return Ok(await _db.Users.ToListAsync());
        }


        [HttpGet("GetUser/{userId}")]
        public async Task<ActionResult<User>> GetUser(int userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);

            return Ok(user);
        
            // return await _db.Users.ToListAsync();
        }
    }
}
