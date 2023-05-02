using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Monitorings;
using UserSignUp.Data;
using UserSignUp.Models;

namespace UserSignUp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SqlDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public UserController(SqlDbContext ctx, IConfiguration configuration)
        {
            _dbContext = ctx;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> Post(User payload)
        {
            MonitorService.Log.Information("Entered post method for user creation");
            if (payload is not null)
            {
                User userData = new User()
                {
                    FirstName = payload.FirstName,
                    LastName = payload.LastName,
                    Email = payload.Email,
                    Phone = payload.Phone,
                    AddressLine1 = payload.AddressLine1,
                    AddressLine2 = payload.AddressLine2,
                    Country = payload.Country,
                    City = payload.City,
                    ZipCode = payload.ZipCode
                };

                _dbContext.Users.Add(userData);
                await _dbContext.SaveChangesAsync();
                MonitorService.Log.Information("New user created");
                return Ok(payload);
            }
            else
            {
                MonitorService.Log.Information("Could not create new user");
                return BadRequest(payload);
            }
        }

        [HttpGet]
        [Route("ViewUsers")]
        public async Task<IActionResult> GetAll()
        {
            var result = await(from user in _dbContext.Users
                          select new
                          {
                              Id = user.Id,
                              FirstName = user.FirstName,
                              LastName = user.LastName,
                              Phone = user.Phone,
                              Email = user.Email,
                              AddressLine1 = user.AddressLine1,
                              AddressLine2 = user.AddressLine2,
                              Country = user.Country,
                              City = user.City,
                              ZipCode = user.ZipCode

                          }).ToListAsync();

            MonitorService.Log.Error("Entered get method for user view");
            return Ok(result);
        }
    }
}
