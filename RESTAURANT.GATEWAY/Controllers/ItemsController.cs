using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RESTAURANT.GATEWAY.Data.DTOs;
using RESTAURANT.GATEWAY.Data.Models;
using RESTAURANT.GATEWAY.Services;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;

namespace RESTAURANT.GATEWAY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly Urls _urls;
        private readonly JWTService _jwtService;

        public ItemsController(HttpClient client, IOptions<Urls> urls, JWTService jwtService)
        {
            _client = client;
            _urls = urls.Value;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var response = await _client.GetAsync(_urls.Items + "/show/items/getAll");
            if (response.IsSuccessStatusCode)
            {
                var items = response.Content.ReadAsStringAsync().Result;
                var itemsList = JsonConvert.DeserializeObject<List<Item>>(items);
                return Ok(itemsList);
            }
            return NotFound();
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(string id)
        {
            var response = await _client.GetAsync(_urls.Items + "/show/items/getItemById/" + id);
            if (response.IsSuccessStatusCode)
            {
                var item = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<Item>(item);
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost("create"), Authorize(Roles = "employee")]
        public async Task<IActionResult> CreateItem(ItemDTO item)
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

                var employeeId = _jwtService.ExtractUserIdFromToken(token);
                Console.WriteLine(employeeId);
                if (employeeId == null)
                {
                    return Unauthorized();
                }
                 
                var response = await _client.PostAsJsonAsync(_urls.Items + "/show/items/addItem", item);
                if (response.IsSuccessStatusCode)
                {
                    var itemResponse = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<Item>(itemResponse);
                    return Ok(result);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteItem(string id)
        //{
        //    try
        //    {
        //         var header = Request.Headers["Authorization"].ToString();
                //if (string.IsNullOrEmpty(header) || !header.ToString().StartsWith("Bearer "))
                //{
                //    return Unauthorized();
                // }

                // var token = header.ToString().Substring(7).Trim();
    //        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer ", token);

    //        var employeeId = _jwtService.ExtractUserIdFromToken(token);

    //        if (employeeId == null)
    //        {
    //            return Unauthorized();
    //        }

    //        var response = await _client.DeleteAsync(_urls.Items + "/show/items/deleteItem/" + id);
    //        if (response.IsSuccessStatusCode)
    //        {
    //            return Ok();
    //        }
    //        return BadRequest();
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

}
}
