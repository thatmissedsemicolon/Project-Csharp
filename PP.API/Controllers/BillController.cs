using Microsoft.AspNetCore.Mvc;
using PP.API.EC;
using PP.Library.Utilities;
using PP.Library.DTO;

namespace PP.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BillController : ControllerBase
    {
        private readonly ILogger<BillController> _logger;

        public BillController(ILogger<BillController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<BillDTO> Get(string? query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return new BillEC().GetAllBills();
            }
            else
            {
                return new BillEC().Search(query);
            }
        }

        [HttpPost("/Bill/add")]
        public BillDTO AddBill([FromBody] BillDTO bill)
        {
            return new BillEC().Add(bill);
        }

        [HttpPost("/Bill/update")]
        public BillDTO UpdateBill([FromBody] BillDTO bill)
        {
            return new BillEC().Update(bill);
        }

        [HttpGet("/Bill/{id}")]
        public IEnumerable<BillDTO> GetBillById(int id)
        {
            return new BillEC().GetById(id);
        }

        [HttpDelete("/Bill/delete/{id}")]
        public bool Delete(string id)
        {
            return new BillEC().DeleteById(id);
        }

        [HttpPost("/Bill/search")]
        public IEnumerable<BillDTO> SearchClients([FromBody] QueryMessage query)
        {
            return new BillEC().Search(query.Query);
        }
    }
}
