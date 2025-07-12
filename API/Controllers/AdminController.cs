using API.Models.DTO;
using API.Models.Requests;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/Users/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [Authorize]
        [HttpGet(Name = "GetAllAdmins")]
        public ActionResult GetAllAdmins([FromQuery] UrlQuery query)
        {
            var response = _adminService.GetAllAdmins(query);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("{Id}", Name = "GetAdmin")]
        public ActionResult GetAdmin(int Id)
        {
            var response = _adminService.GetAdmin(Id);

            switch (response.Status)
            {
                default:
                case 200:
                    return Ok(response);

                case 404:
                    return NotFound(response);
            }
        }

        [Authorize]
        [HttpPost(Name = "PostAdmin")]
        public ActionResult PostAdmin([FromBody] PostUserDTO adminDTO)
        {
            var response = _adminService.PostAdmin(adminDTO);

            switch (response.Status)
            {
                default:
                case 201:
                    return CreatedAtRoute(routeName: "GetAdmin", routeValues: new { Id = response.Data?.IdClient }, value: response.Data);

                case 400:
                    return BadRequest(response);
            }
        }

        [Authorize]
        [HttpPatch("{Id}", Name = "PatchAdmin")]
        public ActionResult PatchAdmin([FromRoute] int Id, [FromBody] PostUserDTO adminDTO)
        {
            var response = _adminService.PatchAdmin(Id, adminDTO);

            switch (response.Status)
            {
                default:
                case 200:
                    return Ok(response);

                case 404:
                    return NotFound(response);
            }
        }

        [Authorize]
        [HttpDelete("{Id}", Name = "DeleteAdmin")]
        public ActionResult DeleteAdmin(int Id)
        {
            var response = _adminService.DeleteAdmin(Id);

            return NoContent();
        }        
    }
}
