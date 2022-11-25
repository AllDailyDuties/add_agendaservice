using AllDailyDuties_AgendaService.Helpers;
using AllDailyDuties_AgendaService.Middleware.Authorization;
using AllDailyDuties_AgendaService.Middleware.Messaging;
using AllDailyDuties_AgendaService.Services;
using AllDailyDuties_AgendaService.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var factory = new ConnectionFactory
{
    HostName = "localhost"
};
//Create the RabbitMQ connection using connection factory details as i mentioned above
var connection = factory.CreateConnection();
//Here we create channel with session and model
using var channel = connection.CreateModel();
//declare the queue after mentioning name and a few property related to that
channel.QueueDeclare("user_object", exclusive: false);
//Set Event object which listen message from chanel which is sent by producer
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Token message received: {message}");
};
//read the message
channel.BasicConsume(queue: "user_object", autoAck: true, consumer: consumer);
//Console.ReadKey();
// Add services to the container.
builder.Services.AddDbContext<DataContext>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();
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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dbContext.Database.EnsureCreated();
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