using API.Middlewares;
using API.Models;
using API.Models.DTO;
using API.Models.Responses;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using AutoMapper;
using Azure.Core;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<PostClientDTO> _clientValidator;
        private readonly PasswordHasher<Client> _hasher = new();
        private readonly TokenService _tokenService;

        public AuthService(ITokenRepository tokenRepository, IAdminRepository adminRepository, IClientRepository clientRepository, IEmployeeRepository employeeRepository, IMapper mapper, IValidator<PostClientDTO> validator, TokenService token)
        {
            _tokenRepository = tokenRepository;
            _adminRepository = adminRepository;
            _clientRepository = clientRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _clientValidator = validator;
            _tokenService = token;
        }

        public ResponseDefault<ClientTokenDTO> Login(ClientLoginDTO loginDTO)
        {
            var result = _clientValidator.Validate(_mapper.Map<ClientLoginDTO, PostClientDTO>(loginDTO), opt =>
            {
                opt.IncludeRuleSets("Login");
            });

            if (!result.IsValid)
            {
                List<string> errors = [];
                foreach (var error in result.Errors)
                    errors.Add(error.ErrorMessage);

                return new ResponseDefault<ClientTokenDTO>
                {
                    Status = 400,
                    Message = "Login failed",
                    Errors = errors,
                };
            }

            var client = GetClientByEmail(loginDTO.Email);

            if (client == null)
                return new ResponseDefault<ClientTokenDTO>
                {
                    Status = 404,
                    Message = "User not found",
                    Errors = [$"User with email {loginDTO.Email} not found"]
                };

            if (_hasher.VerifyHashedPassword(client, client.Password, loginDTO.Password) == PasswordVerificationResult.Failed)
                return new ResponseDefault<ClientTokenDTO>
                {
                    Status = 401,
                    Message = "Login failed",
                    Errors = ["Incorrect passsword"]
                };

            var Access_Token = _tokenService.GenerateToken(loginDTO.Email);

            var token = new Token
            {
                IdClient = client.IdClient,
                Refresh_Token = Guid.NewGuid().ToString(),
                Expires_In = DateTime.UtcNow.AddDays(7),
                Used = false
            };

            _tokenRepository.PostToken(token);

            return new ResponseDefault<ClientTokenDTO>
            {
                Data = new ClientTokenDTO
                {
                    Client = _mapper.Map<Client, ClientDTO>(client),
                    Token = new TokenDTO { Access_Token = Access_Token, Expires_In = DateTime.Now.AddMinutes(60), Refresh_Token = token.Refresh_Token }
                },
                Status = 200,
                Message = "Login successfully"
            };
        }

        public ResponseDefault<ClientTokenDTO> RefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken.Trim()))
                return new ResponseDefault<ClientTokenDTO>
                {
                    Status = 400,
                    Message = "Invalid refresh token",
                    Errors = ["Refresh Token is required"]
                };

            var token = _tokenRepository.GetTokenByToken(refreshToken);

            if (token == null || token.Expires_In < DateTime.UtcNow || token.Used)
                return new ResponseDefault<ClientTokenDTO>
                {
                    Status = 401,
                    Message = "Invalid refresh token",
                    Errors = ["Refresh Token is invalid"]
                };

            token.Used = true;
            _tokenRepository.PatchToken(token);

            var Access_Token = _tokenService.GenerateToken(token.Client.Email);
            var newToken = new Token
            {
                IdClient = token.Client.IdClient,
                Refresh_Token = Guid.NewGuid().ToString(),
                Expires_In = DateTime.UtcNow.AddDays(7),
                Used = false
            };

            _tokenRepository.PostToken(newToken);

            return new ResponseDefault<ClientTokenDTO>
            {
                Data = new ClientTokenDTO
                {
                    Client = _mapper.Map<Client, ClientDTO>(token.Client),
                    Token = new TokenDTO { Access_Token = Access_Token, Expires_In = DateTime.UtcNow.AddMinutes(60), Refresh_Token = newToken.Refresh_Token }
                },
                Status = 200,
                Message = "Success"
            };
        }

        private Client? GetClientByEmail(string email)
        {
            return _adminRepository.GetAdminByEmail(email) ??
                    _employeeRepository.GetEmployeeByEmail(email) ??
                    _clientRepository.GetClientByEmail(email);
        }
    }
}
