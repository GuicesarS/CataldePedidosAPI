using Catalde.Pedidos.Api.Midldleware;
using Catalde.Pedidos.Application.Services;
using Catalde.Pedidos.Infrastructure.Context;
using Catalde.Pedidos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

builder.Services.AddDbContext<PedidoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

