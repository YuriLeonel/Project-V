using API.Models.DTO;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("auth/login", Name = "Login")]
        public ActionResult Login([FromBody] ClientLoginDTO loginDTO)
        {
            var response = _authService.Login(loginDTO);

            switch (response.Status)
            {
                default:
                case 200:
                    return Ok(response);

                case 400:
                    return BadRequest(response);

                case 401:
                    return Unauthorized(response);

                case 404:
                    return NotFound(response);
            }
        }

        [Authorize]
        [HttpGet("auth", Name = "RefreshToken")]
        public ActionResult RefreshToken([FromQuery] string refreshToken)
        {
            var response = _authService.RefreshToken(refreshToken);

            switch (Response.StatusCode)
            {
                default:
                case 200:
                    return Ok(response);

                case 400:
                    return BadRequest(response);

                case 401:
                    return Unauthorized(response);
            }
        }
    }
}
