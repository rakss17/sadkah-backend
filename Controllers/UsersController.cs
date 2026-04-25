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
        private readonly ITokenService _tokenService;

        public UsersController(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
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
                    Role = registerDto.Role ?? UserRole.Unassigned
                };

                var createdUser = await _userManager.CreateAsync(user, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var addToRole = await _userManager.AddToRoleAsync(user, registerDto.Role.ToString() ?? UserRole.Unassigned.ToString());
                    if (addToRole.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                Email = user.Email ?? string.Empty,
                                FullName = user.FirstName + " " + user.LastName,
                                Token = _tokenService.CreateToken(user)
                            }
                        );
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