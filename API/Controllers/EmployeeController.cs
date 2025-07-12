using API.Models.DTO;
using API.Models.Requests;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/Users/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Authorize]
        [HttpGet(Name = "GetAllEmployees")]
        public ActionResult GetAllEmployees([FromQuery] UrlQuery query)
        {
            var response = _employeeService.GetAllEmployees(query);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("{Id}", Name = "GetEmployee")]
        public ActionResult GetEmployee(int Id)
        {
            var response = _employeeService.GetEmployee(Id);

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
        [HttpPost(Name = "PostEmployee")]
        public ActionResult PostEmployee([FromBody] PostUserDTO employeeDTO)
        {
            var response = _employeeService.PostEmployee(employeeDTO);

            switch (response.Status)
            {
                default:
                case 201:
                    return CreatedAtRoute(routeName: "GetEmployee", routeValues: new { Id = response.Data?.IdClient }, value: response.Data);

                case 400:
                    return BadRequest(response);
            }
        }

        [Authorize]
        [HttpPatch("{Id}", Name = "PatchEmployee")]
        public ActionResult PatchEmployee([FromRoute] int Id, [FromBody] PostUserDTO employeeDTO)
        {
            var response = _employeeService.PatchEmployee(Id, employeeDTO);

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
        [HttpDelete("{Id}", Name = "DeleteEmployee")]
        public ActionResult DeleteEmployee(int Id)
        {
            var response = _employeeService.DeleteEmployee(Id);

            return NoContent();
        }
    }
}
