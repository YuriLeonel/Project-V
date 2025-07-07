using API.Models.DTO;
using API.Models.Requests;
using API.Models.Responses;

namespace API.Services.Interfaces
{
    public interface IEmployeeService
    {
        public ResponsePaginationDefault<List<ClientDTO>> GetAllEmployees(UrlQuery query);
        public ResponseDefault<CompleteEmployeeDTO> GetEmployee(int Id);
        public ResponseDefault<ClientDTO> PostEmployee(PostUserDTO employeeDTO);
        public ResponseDefault<ClientDTO> PatchEmployee(int Id, PostUserDTO employeeDTO);
        public ResponseDefault<ClientDTO> DeleteEmployee(int Id);
    }
}
