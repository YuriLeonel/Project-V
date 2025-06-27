using API.Models.DTO;
using API.Models.Requests;
using API.Services;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet(Name = "GetAllClients")]
        public ActionResult GetAllClients([FromQuery] UrlQuery query)
        {
            var response = _clientService.GetAllClients(query);

            return Ok(response);
        }

        [HttpGet("{Id}", Name = "GetClient")]
        public ActionResult GetClient(int Id)
        {
            var response = _clientService.GetClient(Id);

            switch (response.Status)
            {
                default:
                case 200:
                    return Ok(response);

                case 404:
                    return NotFound(response);
            }
        }

        [HttpPost(Name = "PostClient")]
        public ActionResult PostClient([FromBody] ClientDTO clientDTO)
        {
            var response = _clientService.PostClient(clientDTO);

            switch (response.Status)
            {
                default:
                case 201:
                    return CreatedAtRoute(routeName: "GetClient", routeValues: new { Id = clientDTO.IdClient }, value: clientDTO);

                case 400:
                    return BadRequest(response);
            }
        }

        [HttpPatch("{Id}", Name = "PatchClient")]
        public ActionResult PatchClient([FromRoute] int Id, [FromBody] ClientDTO clientDTO)
        {
            var response = _clientService.PatchClient(Id, clientDTO);

            switch (response.Status)
            {
                default:
                case 200:
                    return Ok(response);

                case 404:
                    return NotFound(response);
            }
        }

        [HttpDelete("{Id}", Name = "DeleteClient")]
        public ActionResult DeleteClient(int Id)
        {
            var response = _clientService.DeleteClient(Id);

            return NoContent();
        }

        //[HttpGet("login", Name = "Login")]
        //public ActionResult Login([FromBody] ClientLoginDTO loginDTO)
        //{

        //}
    }
}
