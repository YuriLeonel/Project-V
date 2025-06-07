using API.Database;
using API.Helpers;
using AutoMapper;
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
builder.Services.AddOpenApi("ProjectV");
builder.Services.AddRouting();

//DB
builder.Services.AddDbContext<ProjectVContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
