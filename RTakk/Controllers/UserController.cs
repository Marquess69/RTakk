using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RTakk.Data.Interface;
using RTakk.Data.Service;
using RTakk.Data;

namespace RTakk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _repo;
        private readonly JWTService _jwtSerivce;
        private readonly ApplicationDbContext _context;

        public UserController(IUserRepo repo, JWTService jwtService, ApplicationDbContext context)
        {
            _repo = repo;
            _jwtSerivce = jwtService;
            _context = context;
        }
        [HttpGet("FormUsers")]
        public IActionResult GetUsers()
        {
            try {
                var users = _repo.GetUsers();
                return Ok(users);
            } 
            catch(Exception)
            {
                return StatusCode(500,"User error");
            }
        }
    }
}
