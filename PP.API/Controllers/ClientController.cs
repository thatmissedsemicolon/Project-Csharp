using Microsoft.AspNetCore.Mvc;
using PP.API.EC;
using PP.Library.Utilities;
using PP.Library.DTO;

namespace PP.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;

        public ClientController(ILogger<ClientController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<ClientDTO> Get(string? query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return new ClientEC().GetAllClients();
            }
            else
            {
                return new ClientEC().Search(query);
            }
        }

        [HttpPost("/Client/add")]
        public ClientDTO AddClient([FromBody] ClientDTO client)
        {
            return new ClientEC().Add(client);
        }

        [HttpPost("/Client/update")]
        public ClientDTO UpdateClient([FromBody] ClientDTO client)
        {
            return new ClientEC().Update(client);
        }

        [HttpGet("/Client/{id}")]
        public IEnumerable<ClientDTO> GetId(int id)
        {
            return new ClientEC().GetById(id);
        }

        [HttpDelete("/Client/delete/{id}")]
        public bool Delete(string id)
        {
            return new ClientEC().DeleteById(id);
        }

        [HttpPost("/Client/search")]
        public IEnumerable<ClientDTO> SearchClients([FromBody] QueryMessage query)
        {
            return new ClientEC().Search(query.Query);
        }
    }
}