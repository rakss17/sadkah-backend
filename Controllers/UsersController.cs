using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sadkah.Backend.Dtos.User;

namespace Sadkah.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto)
        {
            try {
                var user = new User
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName ?? string.Empty,
                    LastName = registerDto.LastName ?? string.Empty,
                    Role = registerDto.Role
                };

                var createdUser = await _userManager.CreateAsync(user, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var addToRole = await _userManager.AddToRoleAsync(user, registerDto.Role.ToString());
                    if (addToRole.Succeeded)
                    {
                        return Ok(new { message = "User registered successfully." });
                    }
                    else
                    {
                        return StatusCode(500, addToRole.Errors.Select(e => e.Description));
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors.Select(e => e.Description));
                }

            } catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
    }
}