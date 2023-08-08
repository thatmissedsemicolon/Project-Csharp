using Newtonsoft.Json;
using PP.Library.DTO;
using PP.Library.Utilities;

namespace PP.Library.Services
{
    public class EmployeeService
    {
        private static EmployeeService? _instance;
        public static EmployeeService Instance => _instance ??= new EmployeeService();

        //private List<EmployeeDTO> Employees { get; set; }

        private List<EmployeeDTO> employees;
        public List<EmployeeDTO> Employees
        {
            get
            {
                return employees ?? new List<EmployeeDTO>();
            }
        }

        private EmployeeService()
        {
            var response = new WebRequestHandler()
                   .Get("/Employee")
                   .Result;

            employees = JsonConvert
                .DeserializeObject<List<EmployeeDTO>>(response)
                ?? new List<EmployeeDTO>();
        }

        public async Task AddEmployee(EmployeeDTO employee)
        {
            if (employee != null)
            {
                var response = new WebRequestHandler()
                     .Post("/Employee/add", employee)
                     .Result;

                if (response != "ERROR")
                {
                    try
                    {
                        var createdEmpolyee = JsonConvert.DeserializeObject<EmployeeDTO>(response);
                        employees.Add(createdEmpolyee);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public async Task<List<EmployeeDTO>> GetAllEmployees()
        {
            string response = await new WebRequestHandler().Get("/Employee");

            if (response != null)
            {
                employees = JsonConvert.DeserializeObject<List<EmployeeDTO>>(response);
                employees.Sort((emp1, emp2) => emp1.Id.CompareTo(emp2.Id));

                return employees;
            }

            else
            {
                return new List<EmployeeDTO>();
            }
        }

        public async Task DeleteEmployee(string id)
        {
            string result = await new WebRequestHandler().Delete($"/Employee/delete/{id}");

            if (result == "ERROR")
            {
                throw new Exception("Failed to delete the employee on the server.");
            }
        }

        public async Task UpdateEmployee(EmployeeDTO employee)
        {
            var response = new WebRequestHandler()
                        .Post("/Employee/update", employee)
                        .Result;

            if (response == "ERROR")
            {
                throw new Exception("Error occurred while updating the client.");
            }
        }

        public async Task<EmployeeDTO> GetEmployeeById(int id)
        {
            string result = await new WebRequestHandler().Get($"/Employee/{id}");

            if (!string.IsNullOrEmpty(result))
            {
                var employee = JsonConvert.DeserializeObject<EmployeeDTO>(result);

                return employee;
            }

            return null;
        }

        public async Task<List<EmployeeDTO>> SearchEmployee(string queryText)
        {
            QueryMessage queryMessage = new QueryMessage
            {
                Query = queryText
            };

            string result = await new WebRequestHandler().Post($"/Employee/search", queryMessage);

            if (result != null)
            {
                return JsonConvert.DeserializeObject<List<EmployeeDTO>>(result);
            }

            return null;
        }
    }
}
