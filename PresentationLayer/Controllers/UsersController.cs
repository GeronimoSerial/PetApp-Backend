using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;
using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
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
            try
            {
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                return Ok();
                //     await _db.SaveChangesAsync();
                //     return Ok(await _db.Users.ToListAsync());
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetUser/{userId}")]
        public async Task<ActionResult<User>> GetUser(int userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);

            return Ok(user);
        
            
        }

        //Method to delete a user from Database
        [Authorize]
        [HttpDelete("DeleteUser/{userId}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (user!= null)
                {
                    _db.Users.Remove(user);
                    await _db.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }      

        }
    }  
}

    //      //Obtener todos los usuarios
    //     [Authorize]
    //     [HttpGet("GetAllUsers")]
    //     public async Task<ActionResult<List<User>>> GetUsers()
    //     {
    //         return await _db.Users.ToListAsync();
    //     }

