using API.Models.DTO;
using API.Models.Requests;
using API.Services;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [Authorize]
        [HttpGet(Name = "GetAllCompanies")]
        public ActionResult GetAllCompanies([FromQuery] UrlQuery query)
        {
            var response = _companyService.GetAllCompanies(query);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("{Id}", Name = "GetCompany")]
        public ActionResult GetCompany(int Id)
        {
            var response = _companyService.GetCompany(Id);

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
        [HttpPost(Name = "PostCompany")]
        public ActionResult PostCompany([FromBody] PostCompanyDTO companyDTO)
        {
            var response = _companyService.PostCompany(companyDTO);

            switch (response.Status)
            {
                default:
                case 200:
                    return CreatedAtRoute(routeName: "GetCompany", routeValues: new { Id = response.Data?.IdCompany }, value: response.Data);

                case 400:
                    return BadRequest(response);
            }
        }

        [Authorize]
        [HttpPatch("{Id}", Name = "PatchCompany")]
        public ActionResult PatchCompany([FromRoute] int Id, [FromBody] PostCompanyDTO companyDTO)
        {
            var response = _companyService.PatchCompany(Id, companyDTO);

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
        [HttpDelete("{Id}", Name = "DeleteCompany")]
        public ActionResult DeleteCompany(int Id)
        {
            var response = _companyService.DeleteCompany(Id);

            return NoContent();
        }
    }
}
