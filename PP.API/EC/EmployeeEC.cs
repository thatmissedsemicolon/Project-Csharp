using PP.API.Database;
using PP.Library.Models;
using PP.Library.DTO;

namespace PP.API.EC
{
    public class EmployeeEC
    {
        private readonly Filebase _filebase;

        public EmployeeEC()
        {
            _filebase = Filebase.Current;
        }

        public EmployeeDTO Add(EmployeeDTO employee)
        {
            return _filebase.AddEmployee(employee);
        }

        public IEnumerable<EmployeeDTO> GetAllEmployees()
        {
            return _filebase.GetAllEmployees();
        }

        public EmployeeDTO Update(EmployeeDTO employeeToUpdate)
        {
            var existingEmployee = _filebase.Employees.FirstOrDefault(e => e.Id == employeeToUpdate.Id);

            if (existingEmployee == null)
            {
                return null;
            }

            existingEmployee.Name = string.IsNullOrWhiteSpace(employeeToUpdate.Name) ? existingEmployee.Name : employeeToUpdate.Name;
            existingEmployee.Rate = employeeToUpdate.Rate > 0 ? employeeToUpdate.Rate : existingEmployee.Rate;

            return Add(existingEmployee);
        }

        public bool DeleteById(string id)
        {
            var employee = _filebase.Employees.FirstOrDefault(e => e.Id == Convert.ToInt32(id));

            if (employee != null)
            {
                if (_filebase.DeleteEmployee(id))
                {
                    return true;
                }
                else
                {
                    throw new Exception("Employee not found");
                }
            }

            return true;
        }

        public EmployeeDTO GetEmployeeById(int id)
        {
            var employee = _filebase.Employees.FirstOrDefault(e => e.Id == id);

            return employee;
        }

        public IEnumerable<EmployeeDTO> Search(string query)
        {
            return _filebase.Employees.Where(e => e.Name.ToUpper().Contains(query.ToUpper())).Take(1000);
        }
    }
}
