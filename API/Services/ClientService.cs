using API.Models;
using API.Models.DTO;
using API.Models.Requests;
using API.Models.Responses;
using API.Repositories;
using API.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Services
{
    public class ClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<ClientDTO> _hasher = new();

        public ClientService(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public ResponsePaginationDefault<List<ClientDTO>> GetAllClients(UrlQuery query)
        {
            var clients = _clientRepository.GetAllClients();

            Pagination pagination = new Pagination
            {
                Page = query.Page,
                Limit = query.Limit,
                Total = clients.Count(),
                TotalPages = clients.Count() > 0 ? (int)Math.Ceiling((double)clients.Count() / query.Limit) : 0
            };

            clients = clients.Skip((query.Page - 1) * query.Limit).Take(query.Limit).ToList();

            var clientDTO = _mapper.Map<List<Client>, List<ClientDTO>>(clients);

            return new ResponsePaginationDefault<List<ClientDTO>>
            {
                Status = 200,
                Message = "Sucesso",
                Data = clientDTO,
                Pagination = pagination
            };
        }

        public ResponseDefault<ClientDTO> GetClient(int Id)
        {
            var client = _clientRepository.GetClient(Id);

            if (client == null)
                return new ResponseDefault<ClientDTO>
                {
                    Status = 404,
                    Message = "Usuário não encontrado"
                };

            var clientDTO = _mapper.Map<Client, ClientDTO>(client);
            return new ResponseDefault<ClientDTO>
            {
                Status = 200,
                Message = "Sucesso",
                Data = clientDTO
            };
        }

        public ResponseDefault<ClientDTO> PostClient(ClientDTO clientDTO)
        {
            if (_clientRepository.GetClientByEmail(clientDTO.Email) == null)
                return new ResponseDefault<ClientDTO>
                {
                    Status = 400,
                    Message = "Usuário já existe"
                };

            clientDTO.Password = _hasher.HashPassword(clientDTO, clientDTO.Password);
            clientDTO.CreatedAt = DateTime.Now;

            var client = _mapper.Map<ClientDTO, Client>(clientDTO);

            _clientRepository.PostClient(client);

            clientDTO.IdClient = client.IdClient;

            return new ResponseDefault<ClientDTO>
            {
                Status = 201,
                Message = "Usuário criado",
                Data = clientDTO
            };
        }

        public ResponseDefault<ClientDTO> PatchClient(int Id, ClientDTO clientDTO)
        {
            var obj = _clientRepository.GetClient(Id);
            if (obj == null)
                return new ResponseDefault<ClientDTO>
                {
                    Status = 404,
                    Message = "Usuário não encontrado"
                };

            var client = new Client
            {
                IdClient = obj.IdClient,
                Name = string.IsNullOrEmpty(clientDTO.Name.Trim()) ? obj.Name : clientDTO.Name,
                Email = string.IsNullOrEmpty(clientDTO.Email.Trim()) ? obj.Email : clientDTO.Email,
                Password = obj.Password,
                CreatedAt = obj.CreatedAt,
                UpdatedAt = DateTime.Now,
                ClientType = ((int)clientDTO.ClientType) > 0 && clientDTO.ClientType != obj.ClientType ? clientDTO.ClientType : obj.ClientType,
            };

            _clientRepository.PatchClient(client);

            clientDTO = _mapper.Map<Client, ClientDTO>(client);

            return new ResponseDefault<ClientDTO>
            {
                Status = 200,
                Message = "Usuário alterado com sucesso",
                Data = clientDTO
            };
        }

        public ResponseDefault<ClientDTO> DeleteClient(int Id)
        {
            var client = _clientRepository.GetClient(Id);
            if (client == null)
                return new ResponseDefault<ClientDTO>
                {
                    Status = 404,
                    Message = "Usuário não encontrado"
                };

            _clientRepository.DeleteClient(client);

            return new ResponseDefault<ClientDTO>
            {
                Status = 204,
                Message = "Usuário deletado com sucesso"
            };
        }
    }
}
