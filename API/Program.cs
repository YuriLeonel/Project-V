using API.Database;
using API.Helpers;
using API.Models.DTO;
using API.Models.Validators;
using API.Repositories;
using API.Repositories.Interfaces;
using API.Services;
using API.Services.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//MAPPER
var mapConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new MapperDTO()); });
IMapper mapper = mapConfig.CreateMapper();

//CORS
builder.Services.AddCors(opt =>
{
    //Frontent with Next.Js
    opt.AddPolicy("AllowFrontend", policy => { policy.WithOrigins("https://localhost:3000").AllowAnyHeader().AllowAnyMethod(); });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ProjectV",
        Version = "v1",
        Description = "API de gerenciamento de agendamentos"
    });
});
builder.Services.AddRouting();

//DB
builder.Services.AddDbContext<ProjectVContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton(mapper);

//Repositories
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();

//Services
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IClientService, ClientService>();

//Validators
builder.Services.AddScoped<IValidator<PostClientDTO>, ClientValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
