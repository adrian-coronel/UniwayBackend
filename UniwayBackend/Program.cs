using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using UniwayBackend;
using UniwayBackend.Config;
using UniwayBackend.Factories;
using UniwayBackend.Helpers;
using UniwayBackend.Helpers.Filters;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Implements;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.implements;
using UniwayBackend.Services.interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>(); // Agregamos nuestro MIDDLEWARE de JWT
app.MapControllers();

app.Run();
