using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RESTAURANT.GATEWAY.Data.DTOs;
using RESTAURANT.GATEWAY.Data.Models;
using RESTAURANT.GATEWAY.Services;

namespace RESTAURANT.GATEWAY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Urls _urls;
        private readonly HttpClient _client;
        private readonly JWTService _jwtService;

        public UsersController(HttpClient client, IOptions<Urls> urls, JWTService jwtService)
        {
            _urls = urls.Value;
            _client = client;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _client.GetAsync(_urls.Users + "/api/Users");
            if (response.IsSuccessStatusCode)
            {
                var users = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<List<User>>(users);
                return Ok(result);
            }
            return BadRequest();

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var response = await _client.GetAsync(_urls.Users + "/api/Users/" + id);
            if (response.IsSuccessStatusCode)
            {
                var user = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<User>(user);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserRegisterDTO model)
        {
            var response = await _client.PostAsJsonAsync(_urls.Users + "/api/Users/register", model);
            if (response.IsSuccessStatusCode)
            {
                var user = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<User>(user);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(UserLoginDTO model)
        {
            var response = _client.PostAsJsonAsync(_urls.Users + "/api/Users/login", model).Result;
            if (response.IsSuccessStatusCode)
            {
                var user = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<User>(user);
                var token = _jwtService.GenerateSecurityToken(result);
                return Ok(new { user, token });
            }
            return BadRequest();
        }
    }
}
