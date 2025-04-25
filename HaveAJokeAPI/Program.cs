using HaveAJokeAPI.Configuration;
using HaveAJokeAPI.Repositories;
using HaveAJokeAPI.Responses;
using HaveAJokeAPI.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// bind config

var apiConfig = builder.Configuration
    .GetSection(nameof(DadJokeApiConfiguration))
    .Get<DadJokeApiConfiguration>();

if (apiConfig != null)
{
    builder.Services.AddSingleton(apiConfig);
}
builder.Services.AddTransient<IDadJokeRepository, DadJokeRepository>();
builder.Services.AddKeyedTransient<IJokeService, JokeService>("joke-service");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/random", async ([FromKeyedServices("joke-service")] IJokeService jokeService) =>
    {
        var joke = await jokeService.GetRandomJokeAsync();
        return Results.Ok(joke);
    })
    .WithName("RandomJoke")
    .WithOpenApi();

app.Run();