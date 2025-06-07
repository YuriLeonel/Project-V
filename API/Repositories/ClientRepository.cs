using API.Database;
using API.Models.DTO;
using API.Repositories.Interfaces;
using AutoMapper;

namespace API.Repositories
{
    public class ClientRepository : IClient
    {
        private ProjectVContext _db;
        private IMapper _mapper;

        public ClientRepository(ProjectVContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<ClientDTO> GetAllClients()
        {
            throw new NotImplementedException();
        }

        public ClientDTO GetClient(int Id)
        {
            throw new NotImplementedException();
        }

        public ClientDTO PostClient(ClientDTO clientDTO)
        {
            throw new NotImplementedException();
        }

        public ClientDTO PutClient(ClientDTO clientDTO)
        {
            throw new NotImplementedException();
        }

        public void DeleteClient(int Id)
        {
            throw new NotImplementedException();
        }

        public ClientTokenDTO Login(ClientLoginDTO loginDTO)
        {
            throw new NotImplementedException();
        }
    }
}
