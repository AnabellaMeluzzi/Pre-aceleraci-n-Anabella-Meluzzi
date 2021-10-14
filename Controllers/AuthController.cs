using DisneyAPI.Entities;
using DisneyAPI.Interfaces;
using DisneyAPI.ViewModel.Auth.Login;
using DisneyAPI.ViewModel.Auth.Register;
using DisneyAPI.ViewModel.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DisneyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailService _mailService;

        public AuthController(UserManager<User> userManager, 
                              SignInManager<User> signInManager,
                              RoleManager<IdentityRole> roleManager,
                              IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mailService = mailService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromQuery] RegisterReqVM model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);

            if (userExists is not null) return BadRequest(new
                                                {
                                                    Status = "Error",
                                                    Message = $"User creation failed for username {model.Username}. User already in the database."
                                                });

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                IsActive = true
            };
            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, 
                                      new
                                        {
                                            Status = "Error",
                                            Message = $"The user couldn't be created. Errors: {string.Join(" ,", result.Errors.Select(x => x.Description ))}"
                                        });
                }

                MailServiceReqVM mailServiceReqVM = new MailServiceReqVM
                {
                    Title = "Welcome to Disney",
                    Username = user.UserName,
                    Email = user.Email
                };
                await _mailService.SendEmail(mailServiceReqVM);

                return Ok(new
                            {
                                Status = "Ok",
                                Message = $"User {user.UserName} created succesfully."
                            });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = "Error",
                    Message = $"User creation failed for username {model.Username}."
                });
            }

        }


        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromQuery] RegisterReqVM model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);

            if (userExists is not null) return BadRequest(new
            {
                Status = "Error",
                Message = $"User creation failed for username {model.Username}. User already in the database."
            });

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                IsActive = true
            };

            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                                      new
                                      {
                                          Status = "Error",
                                          Message = $"The user couldn't be created. Errors: {string.Join(" ,", result.Errors.Select(x => x.Description))}"
                                      });
                }

                if (!await _roleManager.RoleExistsAsync("Admin"))
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));

                await _userManager.AddToRoleAsync(user, "Admin");

                MailServiceReqVM mailServiceReqVM = new MailServiceReqVM
                {
                    Title = "Welcome to Disney - ADMIN",
                    Username = user.UserName,
                    Email = user.Email
                };
                await _mailService.SendEmail(mailServiceReqVM);

                return Ok(new
                            {
                                Status = "Ok",
                                Message = $"User {model.Username} created succesfully."
                            });
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    Status = "Error",
                    Message = $"User ADMIN creation failed."
                });
            }
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromQuery] LoginReqVM model)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
                if (result.Succeeded)
                {
                    var currentUser = await _userManager.FindByNameAsync(model.Username);
                    if (currentUser.IsActive)
                    {
                        return Ok(await GetToken(currentUser));
                    }
                }

                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    Status = "Error",
                    Message = $"The user {model.Username} is not authorized."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //Get Token
        private async Task<LoginResVM> GetToken(User currentUser)
        {
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            var authClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, currentUser.UserName ),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            authClaim.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("VeryLongANDSecureKeyfORtESTING"));

            var token = new JwtSecurityToken(issuer: "http://localhost:5000",
                                             audience: "http://localhost:5000",
                                             expires: DateTime.Now.AddHours(1),
                                             claims: authClaim,
                                             signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
            return new LoginResVM
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ValidTo = token.ValidTo
            };
        }
    }
}
