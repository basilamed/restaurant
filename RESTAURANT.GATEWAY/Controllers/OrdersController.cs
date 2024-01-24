using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RESTAURANT.GATEWAY.Data.DTOs;
using RESTAURANT.GATEWAY.Data.Models;
using RESTAURANT.GATEWAY.Services;
using System.Net.Http.Headers;

namespace RESTAURANT.GATEWAY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly Urls _urls;
        private readonly JWTService _jwtService;

        public OrdersController(HttpClient client, IOptions<Urls> urls, JWTService jwtService)
        {
            _client = client;
            _urls = urls.Value;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var response = await _client.GetAsync(_urls.Orders + "/api/Orders");
            if (response.IsSuccessStatusCode)
            {
                var orders = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<List<Order>>(orders);
                return Ok(result);
            }
            return BadRequest();
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var response = await _client.GetAsync(_urls.Orders + "/api/Orders/" + id);
            if (response.IsSuccessStatusCode)
            {
                var order = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<Order>(order);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost("create"), Authorize(Roles = "employee")]
        public async Task<IActionResult> CreateOrder(OrderRequestDTO model)
        {
            try
            {
                var header = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(header) || !header.ToString().StartsWith("Bearer "))
                {
                    return Unauthorized();
                }

                var token = header.ToString().Substring(7).Trim();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                var customerId = _jwtService.ExtractUserIdFromToken(token);
                if (customerId == null)
                {
                    return Unauthorized();
                }

                var order = new OrderDTO
                {
                    UserId = customerId,
                    OrderItems = model.OrderItems,
                    PhoneNumber = model.PhoneNumber,
                    TotalPrice = model.TotalPrice
                };

                var response = await _client.PostAsJsonAsync(_urls.Orders + "/api/Orders/create", order);
                if (response.IsSuccessStatusCode)
                {
                    var orderResponse = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<Order>(orderResponse);
                    return Ok(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("user/{userId}"), Authorize(Roles = "customer")]
        public async Task<IActionResult> GetOrderByUserId(string userId)
        {
            try
            {
                var header = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(header) || !header.ToString().StartsWith("Bearer "))
                {
                    return Unauthorized();
                }

                var token = header.ToString().Substring(7).Trim();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var customerId = _jwtService.ExtractUserIdFromToken(token);
                if (customerId == null)
                {
                    return Unauthorized();
                }

                var response = await _client.GetAsync(_urls.Orders + "/api/Orders/user/" + userId);
                if (response.IsSuccessStatusCode)
                {
                    var orders = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<List<Order>>(orders);
                    return Ok(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var response = await _client.DeleteAsync(_urls.Orders + "/api/Orders/" + id);
            if (response.IsSuccessStatusCode)
            {
                var order = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<Order>(order);
                return Ok(result);
            }
            return BadRequest();
        }


    }
}
