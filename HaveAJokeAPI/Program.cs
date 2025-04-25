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
        try
        {
            var joke = await jokeService.GetRandomJokeAsync();
            return Results.Ok(joke);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    })
    .WithName("RandomJoke")
    .WithOpenApi();

app.MapGet("/search", async ([FromKeyedServices("joke-service")] IJokeService jokeService) =>
    {
        try
        {
            var jokes = await jokeService.SearchJokesAsync(1,30, "man bar");
            return Results.Ok(jokes);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    })
    .WithName("SearchJokes")
    .WithOpenApi();

app.Run();