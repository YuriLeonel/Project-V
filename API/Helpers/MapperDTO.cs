using API.Models;
using API.Models.DTO;
using AutoMapper;

namespace API.Helpers
{
    public class MapperDTO : Profile
    {
        public MapperDTO()
        {
            //Admins/Employees/Clients
            CreateMap<Client, ClientDTO>();
            CreateMap<Client, CompleteClientDTO>();
            CreateMap<Client, CompleteAdminDTO>();
            CreateMap<Client, CompleteEmployeeDTO>();
            CreateMap<ClientDTO, Client>();
            CreateMap<PostClientDTO, Client>();
            CreateMap<PostUserDTO, Client>();
            CreateMap<ClientLoginDTO, PostClientDTO>();

            //Companies
            CreateMap<Company, CompanyDTO>();
            CreateMap<CompanyDTO, Company>();
            CreateMap<PostCompanyDTO, Company>();
        }
    }
}
