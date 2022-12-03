using AllDailyDuties_AgendaService.Helpers;
using AllDailyDuties_AgendaService.Middleware.Authorization;
using AllDailyDuties_AgendaService.Middleware.Messaging;
using AllDailyDuties_AgendaService.Middleware.Messaging.Interfaces;
using AllDailyDuties_AgendaService.Services;
using AllDailyDuties_AgendaService.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StackExchange.Redis;
using System.Text;
using System.Configuration;
using AllDailyDuties_AgendaService.Repositories.Interfaces;
using AllDailyDuties_AgendaService.Repositories;

var builder = WebApplication.CreateBuilder(args);

//Console.ReadKey();
// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<DataContext>(options =>
options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
//builder.Services.AddDbContextFactory<DataContext>(lifetime: ServiceLifetime.Scoped);
//builder.Services.AddDbContext<DataContext>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskItemRepo, TaskItemRepo>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();
builder.Services.AddScoped<IRabbitMQConsumer, RabbitMQConsumer>();
//builder.Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = TokenService.CreateTokenValidationParameters();
});

var app = builder.Build();

using var channel = RabbitMQConnection.Instance.Connection.CreateModel();
using (var scope = app.Services.CreateScope())
{
    var rabbiqMq = scope.ServiceProvider.GetRequiredService<IRabbitMQConsumer>();
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dbContext.Database.EnsureCreated();
    rabbiqMq.ConsumeMessage(channel, "user_object");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapControllers();

app.Run("http://+:9001");