using Microsoft.AspNetCore.Mvc;
using PP.API.EC;
using PP.Library.DTO;
using PP.Library.Utilities;

namespace PP.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimeController : ControllerBase
    {
        private readonly ILogger<TimeController> _logger;

        public TimeController(ILogger<TimeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<TimeDTO> Get(string? query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return new TimeEC().GetAllTimes();
            }
            else
            {
                return new TimeEC().Search(query);
            }
        }

        [HttpPost("/Time/add")]
        public TimeDTO AddTime([FromBody] TimeDTO time)
        {
            return new TimeEC().Add(time);
        }

        [HttpPost("/Time/update")]
        public TimeDTO UpdateTime([FromBody] TimeDTO time)
        {
            return new TimeEC().Update(time);
        }

        [HttpGet("/Time/{id}")]
        public TimeDTO GetTimeById(int id)
        {
            return new TimeEC().GetTimeById(id);
        }

        [HttpDelete("/Time/delete/{timeId}")]
        public bool DeleteTime(string timeId)
        {
            return new TimeEC().DeleteById(timeId);
        }

        [HttpPost("/Time/search")]
        public IEnumerable<TimeDTO> SearchTimes([FromBody] QueryMessage query)
        {
            return new TimeEC().Search(query.Query);
        }
    }
}
