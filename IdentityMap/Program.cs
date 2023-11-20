using Data.Base;
using Data.Repositories;
using Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DatabaseContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IIdentityMapManager, IdentityMapManager>();
builder.Services.AddScoped<IIdentityMapQueryCommand, IdentityMapQueryCommand>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
