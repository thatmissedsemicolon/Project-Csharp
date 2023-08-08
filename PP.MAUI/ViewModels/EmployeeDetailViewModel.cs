using PP.Library.Models;
using PP.Library.DTO;
using PP.Library.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PP.MAUI.ViewModels
{
    public class EmployeeDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private EmployeeService employeeService;
        private EmployeeDTO employeeDTOModel;
        private Employee employeeModel;
        private int employeeId;

        public Employee EmployeeModel
        {
            get => employeeModel;
            set
            {
                employeeModel = value;
                OnPropertyChanged();
            }
        }

        public EmployeeDTO EmployeeDTOModel
        {
            get => employeeDTOModel;
            set
            {
                if (employeeDTOModel != value)
                {
                    employeeDTOModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public int EmployeeId
        {
            get => employeeId;
            set
            {
                employeeId = value;
                OnPropertyChanged(nameof(EmployeeId));
            }
        }

        public EmployeeDetailViewModel()
        {
            employeeService = EmployeeService.Instance;
            employeeDTOModel = new EmployeeDTO();
            employeeModel = new Employee(employeeDTOModel);
        }

        public async Task GetEmployee()
        {
            var employeeDetails = await employeeService.GetEmployeeById(EmployeeId);

            if (employeeDetails != null)
            {
                employeeDetails.TimeRecords = employeeDetails.TimeRecords.OrderBy(t => t.Id).ToList();

                EmployeeDTOModel = employeeDetails;
                EmployeeModel = new Employee(EmployeeDTOModel);
            }
            else
            {
                employeeDTOModel = null;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
