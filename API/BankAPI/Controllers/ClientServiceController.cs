using Microsoft.AspNetCore.Mvc;
using ServicesDb;
using ModelsDb;
using Microsoft.AspNetCore.Http.Extensions;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientServiceController : ControllerBase
    {

        private ClientService clientService;

        public ClientServiceController()
        {
            clientService = new ClientService();
        }

        [HttpGet("{id}")]
        public IActionResult GetClient(Guid id)
        {
            Client client = clientService.GetClientAsync(id).Result;
            return Ok(client);
        }
        [HttpPost]
        public IActionResult AddClient([FromBody] Client cl)
        {
            clientService.AddClientAsync(cl);
            return Created(new Uri(Request.GetEncodedUrl() + "/" + cl.id), cl);
        }
        [HttpPut("{id}")]
        public IActionResult ChangeClient(Guid id)
        {
            clientService.ChangeClientAsync(id);
            return Ok();
            
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteClient(Guid id)
        {
            clientService.DeleteClientAsync(id);
            return NoContent();
        }
    }
}