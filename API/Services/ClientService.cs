using API.Models;
using API.Models.DTO;
using API.Models.Requests;
using API.Models.Responses;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace API.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<PostClientDTO> _clientValidator;
        private readonly PasswordHasher<Client> _hasher = new();

        public ClientService(IClientRepository clientRepository, IMapper mapper, IValidator<PostClientDTO> clientValidator)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
            _clientValidator = clientValidator;
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

            clients = [.. clients.Skip((query.Page - 1) * query.Limit).Take(query.Limit)];

            var clientDTO = _mapper.Map<List<Client>, List<ClientDTO>>(clients);

            return new ResponsePaginationDefault<List<ClientDTO>>
            {
                Status = 200,
                Message = "Success",
                Data = clientDTO,
                Pagination = pagination
            };
        }

        public ResponseDefault<CompleteClientDTO> GetClient(int Id)
        {
            var client = _clientRepository.GetCompleteClient(Id);

            if (client == null)
                return new ResponseDefault<CompleteClientDTO>
                {
                    Status = 404,
                    Message = "User not found",
                    Errors = [$"User with code {Id} not found"]
                };

            var clientDTO = _mapper.Map<Client, CompleteClientDTO>(client);
            return new ResponseDefault<CompleteClientDTO>
            {
                Status = 200,
                Message = "Success",
                Data = clientDTO
            };
        }

        public ResponseDefault<ClientDTO> PostClient(PostClientDTO clientDTO)
        {
            var result = _clientValidator.Validate(clientDTO);

            if (!result.IsValid)
            {
                List<string> errors = [];
                foreach (var error in result.Errors)
                    errors.Add(error.ErrorMessage);

                return new ResponseDefault<ClientDTO>
                {
                    Status = 400,
                    Message = "Register user failed",
                    Errors = errors,
                };
            }

            if (_clientRepository.GetClientByEmail(clientDTO.Email) != null)
                return new ResponseDefault<ClientDTO>
                {
                    Status = 400,
                    Message = "User already exists",
                    Errors = [$"User already registered with email {clientDTO.Email}"]
                };

            var client = _mapper.Map<PostClientDTO, Client>(clientDTO);

            client.Password = _hasher.HashPassword(client, client.Password);
            client.Active = true;
            client.CreatedAt = DateTime.UtcNow;
            client.ClientType = Models.Enums.ClientTypeEnum.Client;

            _clientRepository.PostClient(client);

            var clientResponse = _mapper.Map<Client, ClientDTO>(client);

            return new ResponseDefault<ClientDTO>
            {
                Status = 201,
                Message = "User registered",
                Data = clientResponse
            };
        }

        public ResponseDefault<ClientDTO> PatchClient(int Id, PostClientDTO clientDTO)
        {
            var obj = _clientRepository.GetClient(Id);
            if (obj == null)
                return new ResponseDefault<ClientDTO>
                {
                    Status = 404,
                    Message = "User not found",
                    Errors = [$"User with code {Id} not found"]
                };

            var client = new Client
            {
                IdClient = obj.IdClient,
                Name = string.IsNullOrEmpty(clientDTO.Name.Trim()) ? obj.Name : clientDTO.Name,
                Email = string.IsNullOrEmpty(clientDTO.Email.Trim()) ? obj.Email : clientDTO.Email,
                ClientType = obj.ClientType,
                CreatedAt = obj.CreatedAt,
                UpdatedAt = DateTime.UtcNow,
                Active = obj.Active
            };

            client.Password = string.IsNullOrEmpty(clientDTO.Password.Trim()) ? obj.Password : _hasher.HashPassword(client, clientDTO.Password);

            _clientRepository.PatchClient(client);

            var clientResponse = _mapper.Map<Client, ClientDTO>(client);

            return new ResponseDefault<ClientDTO>
            {
                Status = 200,
                Message = "User changed successsfully",
                Data = clientResponse
            };
        }

        public ResponseDefault<ClientDTO> DeleteClient(int Id)
        {
            var client = _clientRepository.GetClient(Id);
            if (client == null)
                return new ResponseDefault<ClientDTO>
                {
                    Status = 404,
                    Message = "User not found",
                    Errors = [$"User with code {Id} not found"]
                };

            _clientRepository.DeleteClient(client);

            return new ResponseDefault<ClientDTO>
            {
                Status = 204,
                Message = "User deleted successfully"
            };
        }
    }
}
