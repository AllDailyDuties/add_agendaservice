using AllDailyDuties_AgendaService.Models.Tasks;
using AllDailyDuties_AgendaService.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using AllDailyDuties_AgendaService.Middleware.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using AllDailyDuties_AgendaService.Models.Shared;
using AllDailyDuties_AgendaService.Middleware.Messaging;
using AllDailyDuties_AgendaService.Middleware.Messaging.Interfaces;

using Newtonsoft.Json;

namespace AllDailyDuties_AgendaService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskItemController : ControllerBase
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings { };
        private readonly ILogger<TaskItemController> _logger;
        private ITaskService _taskService;
        private IMapper _mapper;
        private IJwtUtils _jwtUtils;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRabbitMQProducer _rabbit;
        private readonly IRabbitMQConsumer _rabbitConsumer;
        public TaskItemController(ILogger<TaskItemController> logger, ITaskService taskservice,
        IMapper mapper, IJwtUtils jwtUtils, IHttpContextAccessor httpContextAccessor, IRabbitMQProducer rabbit, IRabbitMQConsumer rabbitConsumer)
        {
            _logger = logger;
            _taskService = taskservice;
            _mapper = mapper;
            _jwtUtils = jwtUtils;
            _httpContextAccessor = httpContextAccessor;
            _rabbit = rabbit;
            
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _taskService.GetAll();
            return Ok(users);
        }

        [HttpGet]
        [Route("/GetOne/{*id}")]
        public IActionResult GetOne(Guid id)
        {
            var users = _taskService.GetById(id);
            return Ok(users);
        }

        [HttpPost]
        public IActionResult Create(CreateRequest model)
        {
            _taskService.Create(model);
            return Ok(new { message = "Task created" });
        }
        [HttpPost]
        [Route("/message")]
        public async Task<IActionResult> CreateTask(string title, DateTime createdAt, DateTime scheduledAt)
        {
            var result = new TaskItemMessage()
            {
                Title = title,
                CreatedAt = createdAt,
                ScheduledAt = scheduledAt
            };

            object test = Serialize(result);
            string encodedToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var uid = _jwtUtils.ValidateToken(encodedToken);
            _rabbit.SendMessage(uid, "auth_token", result);

            
            return Ok();
        }
        private static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }
    }
}