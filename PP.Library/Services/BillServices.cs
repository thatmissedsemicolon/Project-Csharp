using Newtonsoft.Json;
using PP.Library.DTO;
using PP.Library.Utilities;

namespace PP.Library.Services
{
    public class BillService
    {
        private static BillService? _instance;
        public static BillService Instance => _instance ??= new BillService();

        //private List<Bill> Bills { get; set; }

        private List<BillDTO> bills;
        public List<BillDTO> Bills
        {
            get
            {
                return bills ?? new List<BillDTO>();
            }
        }

        private BillService()
        {
            var response = new WebRequestHandler()
                   .Get("/Bill")
                   .Result;

            bills = JsonConvert
                .DeserializeObject<List<BillDTO>>(response)
                ?? new List<BillDTO>();
        }

        public async Task AddBill(BillDTO bill)
        {
            if (bill != null)
            {
                var response = new WebRequestHandler()
                     .Post("/Bill/add", bill)
                     .Result;

                if (response != "ERROR")
                {
                    try
                    {
                        var createdBill = JsonConvert.DeserializeObject<BillDTO>(response);
                        Bills.Add(createdBill);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public async Task<List<BillDTO>> GetAllBills()
        {
            string response = await new WebRequestHandler().Get("/Bill");

            if (response != null)
            {
                bills = JsonConvert.DeserializeObject<List<BillDTO>>(response);
                bills.Sort((bill1, bill2) => bill1.Id.CompareTo(bill2.Id));

                return bills;
            }

            else
            {
                return new List<BillDTO>();
            }
        }

        public async Task DeleteBill(string id)
        {
            string result = await new WebRequestHandler().Delete($"/Bill/delete/{id}");

            if (result == "ERROR")
            {
                throw new Exception("Failed to delete the bill on the server.");
            }
        }

        public async Task UpdateBill(BillDTO bill)
        {
            var response = new WebRequestHandler()
                        .Post("/Bill/update", bill)
                        .Result;

            if (response == "ERROR")
            {
                throw new Exception("Error occurred while updating the bill.");
            }
        }

        public async Task<BillDTO> GetBillById(int id)
        {
            string result = await new WebRequestHandler().Get($"/Bill/{id}");

            if (!string.IsNullOrEmpty(result))
            {
                var bill = JsonConvert.DeserializeObject<List<BillDTO>>(result);

                return bill.FirstOrDefault();
            }

            return null;
        }

        public async Task<List<BillDTO>> SearchBills(string queryText)
        {
            QueryMessage queryMessage = new QueryMessage
            {
                Query = queryText
            };

            string result = await new WebRequestHandler().Post($"/Bill/search", queryMessage);

            if (result != null)
            {
                return JsonConvert.DeserializeObject<List<BillDTO>>(result);
            }

            return null;
        }
    }
}
