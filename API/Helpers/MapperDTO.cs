using API.Models;
using API.Models.DTO;
using AutoMapper;

namespace API.Helpers
{
    public class MapperDTO : Profile
    {
        public MapperDTO()
        {
            CreateMap<Client, ClientDTO>();
            CreateMap<ClientDTO, Client>();
        }
    }
}
