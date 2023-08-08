using Newtonsoft.Json;
using PP.Library.DTO;
using PP.Library.Utilities;

namespace PP.Library.Services
{
    public class TimeService
    {
        private static TimeService? _instance;
        public static TimeService Instance => _instance ??= new TimeService();

        //private List<Time> Times { get; set; }

        private List<TimeDTO> times;
        public List<TimeDTO> Times
        {
            get
            {
                return times ?? new List<TimeDTO>();
            }
        }

        private TimeService()
        {
            var response = new WebRequestHandler()
                   .Get("/Time")
                   .Result;

            times = JsonConvert
                .DeserializeObject<List<TimeDTO>>(response)
                ?? new List<TimeDTO>();
        }

        public async Task AddTime(TimeDTO time)
        {
            if (time != null)
            {
                var response = new WebRequestHandler()
                     .Post("/Time/add", time)
                     .Result;

                if (response != "ERROR")
                {
                    try
                    {
                        var createdTime = JsonConvert.DeserializeObject<TimeDTO>(response);
                        Times.Add(createdTime);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public async Task<List<TimeDTO>> GetAllTimes()
        {
            string response = await new WebRequestHandler().Get("/Time");

            if (response != null)
            {
                times = JsonConvert.DeserializeObject<List<TimeDTO>>(response);
                times.Sort((time1, time2) => time1.Id.CompareTo(time2.Id));

                return Times;
            }

            else
            {
                return new List<TimeDTO>();
            }
        }

        public async Task DeleteTime(string id)
        {
            string result = await new WebRequestHandler().Delete($"/Time/delete/{id}");

            if (result == "ERROR")
            {
                throw new Exception("Failed to delete the time on the server.");
            }
        }

        public async Task UpdateTime(TimeDTO time)
        {
            var response = new WebRequestHandler()
                        .Post("/Time/update", time)
                        .Result;

            if (response == "ERROR")
            {
                throw new Exception("Error occurred while updating the client.");
            }
        }

        public async Task<TimeDTO> GetTime(int id)
        {
            string result = await new WebRequestHandler().Get($"/Time/{id}");

            if (!string.IsNullOrEmpty(result))
            {
                var time = JsonConvert.DeserializeObject<TimeDTO>(result);

                return time;
            }

            return null;
        }

        public async Task<List<TimeDTO>> SearchTimes(string queryText)
        {
            QueryMessage queryMessage = new QueryMessage
            {
                Query = queryText
            };

            string result = await new WebRequestHandler().Post($"/Time/search", queryMessage);

            if (result != null)
            {
                return JsonConvert.DeserializeObject<List<TimeDTO>>(result);
            }

            return null;
        }

        public async Task<TimeDTO> GetTimeById(int id)
        {
            string result = await new WebRequestHandler().Get($"/Time/{id}");

            if (!string.IsNullOrEmpty(result))
            {
                var times = JsonConvert.DeserializeObject<TimeDTO>(result);

                return times;
            }

            return null;
        }
    }
}
