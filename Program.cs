using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DemoWebServiceEntityFramework.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TodoContext>(options => {    //lifetime default "scoped" (só existe para responder as requisicoes do controlador, não permanece na memoria)
    //options.UseSqlServer("name=ConnectionStrings:DefaultConnection") //string connectionString
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); //outra opcao usando "using Microsoft.Extensions.Configuration;"
    options.LogTo(Console.WriteLine).EnableSensitiveDataLogging(); //quero logar no console para observer os comandos que estao sendo enviados ao banco de dados

}); 

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); //vai estabelecer os detalhes mais aprofundados de erros com o EFCore

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage(); //informacoes mais detalhadas de excesoes
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
