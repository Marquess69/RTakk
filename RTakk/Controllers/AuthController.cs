using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RTakk.Data;
using RTakk.Data.Interface;
using RTakk.Data.Service;
using RTakk.Dtos;
using RTakk.Models;

namespace RTakk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepo _repo;
        private readonly JWTService _jwtSerivce;
        private readonly ApplicationDbContext _context;

        public AuthController(IUserRepo repo, JWTService jwtService, ApplicationDbContext context)
        {
            _repo = repo;
            _jwtSerivce = jwtService;
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            try {
                var user = new User
                {
                    Username = dto.Name,
                    Email = dto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
                };

                return Created("Created", _repo.Create(user));
            }
            catch
            {
                return StatusCode(500, "Error during registration try again.");
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            try
            {
                var user = _repo.GetByEmail(dto.Email!);

                if (user == null)
                {
                    return BadRequest("Invalid Creds");
                }

                //Checking passwd
                if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                {
                    return BadRequest("Invalid Creds");
                }
                //Validation passed, JWT token time

                var jwt = _jwtSerivce.Generate(user.Id);

                //access ony from back api
                Response.Cookies.Append("jwt", jwt, new CookieOptions { HttpOnly = true });

                return Ok("Good");
            }
            catch (Exception ex)
            {
                return BadRequest("Problem druing loging in.");
            }
        }

        [HttpGet("user")]
        public IActionResult Userr()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jwtSerivce.Verify(jwt);

                int userId = int.Parse(token.Issuer);

                var user = _repo.GetById(userId);

                return Ok(user);
            }

            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try {
                Response.Cookies.Delete("jwt");

                return Ok("You are logged out");
            }
            catch
            {
                return StatusCode(500, "Problem during logging out.");
            }
        }

        [HttpGet("getUserList")]
        public IActionResult GetUsers()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jwtSerivce.Verify(jwt);

                int userId = int.Parse(token.Issuer);

                var user = _repo.GetById(userId);

                var users = _repo.GetAllUsers();

                return Ok(users);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPost("Createusers")]
        public IActionResult CreateUser(RegisterDto dto)
        {
            var user = new User
            {
                Username = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            var createdUser = _repo.CreateUser(user);

            return Created("Created", createdUser);
        }

        [HttpDelete("users/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            _repo.DeleteUser(userId);

             return NoContent();
        }
        //method for searching users in db
        [HttpGet("searchUser")]
        public IActionResult GetUsers(string searchQuery)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtSerivce.Verify(jwt);
                int loggedInUserId = int.Parse(token.Issuer);

                var user = _repo.GetById(loggedInUserId);

                var users = _repo.GetUsersBySearchQuery(searchQuery);

                return Ok(users);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

    }
}
