using Microsoft.AspNetCore.Mvc;
using Dapr.AspNetCore;
using Dapr.Client;
using DaprLab.Models;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

using static DaprLab.Controllers.ProductsController;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DaprLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DaprClient _daprClient;


        public ProductsController(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }
        public record Order([property: JsonPropertyName("orderId")] int OrderId);
        [HttpPost("myEndpoint")]
        public async Task<IActionResult> CreateChatRoom(ChatRoom chatRoom)
        {
            var client = new DaprClientBuilder().Build();
            await client.PublishEventAsync("orderpubsub", "orders", chatRoom);
            Console.WriteLine("Published data: " + chatRoom);

            return Ok();
        }
        /*
        [HttpGet]
        public async Task<IActionResult> GetChatRooms()
        {
            // Get all chat rooms from state store
           // var chatRooms = await _daprClient.GetBulkStateAsync<ChatRoom>("state-store", "");

            return Ok(chatRooms.Select(kv => kv.Value));
        }
        */


        [HttpGet("{id}")]
        public async Task<IActionResult> GetChatRoom(string id)
        {
            // Get chat room by id from state store
            var chatRoom = await _daprClient.GetStateAsync<ChatRoom>("state-store", id);

            return Ok(chatRoom);
        }



       
    }
}
