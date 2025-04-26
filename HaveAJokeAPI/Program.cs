using HaveAJokeAPI.Configuration;
using HaveAJokeAPI.Repositories;
using HaveAJokeAPI.Services;
using Microsoft.AspNetCore.Mvc;

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
            return Results.Ok(await jokeService.GetRandomJokeAsync());
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    })
    .WithName("RandomJoke")
    .WithOpenApi();

app.MapGet("/search/{page}/{limit}/{token}", async ([FromKeyedServices("joke-service")] IJokeService jokeService, [FromRoute] int page, [FromRoute] int limit, [FromRoute] string token) =>
    {
        try
        {
            return Results.Ok(await jokeService.SearchJokesAsync(page, limit, token));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    })
    .WithName("SearchJokes")
    .WithOpenApi();

app.Run();