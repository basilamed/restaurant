using Microsoft.EntityFrameworkCore;
using RESTAURANT.ASYNC.Broker;
using RESTAURANT.ASYNC.Data;
using RESTAURANT.ASYNC.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<OrderService>();
builder.Services.AddScoped<IMessageBroker, MessageBroker>();
builder.Services.AddHealthChecks();

builder.Services.AddDbContext<OrderContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MigrateDb.PrepPopulation(app);

app.MapHealthChecks("/health");

app.Run();
