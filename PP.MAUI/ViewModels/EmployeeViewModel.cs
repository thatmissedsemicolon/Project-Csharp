using PP.Library.DTO;
using PP.Library.Models;
using PP.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PP.MAUI.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private EmployeeService employeeService;
        private EmployeeDTO employeeDTOModel;
        public Employee employeeModel { get; set; }
        private EmployeeDTO selectedEmployee;
        private ObservableCollection<EmployeeDTO> employees;

        public EmployeeViewModel()
        {
            employeeService = EmployeeService.Instance;
            employeeDTOModel = new EmployeeDTO();
            employeeModel = new Employee(employeeDTOModel);
            employees = new ObservableCollection<EmployeeDTO>();
            AddEmployeeCommand = new Command(async () => await AddEmployee());
            UpdateEmployeeCommand = new Command(async () => await UpdateEmployee(), () => SelectedEmployee != null);
            DeleteEmployeeCommand = new Command(async () => await DeleteEmployee(), () => SelectedEmployee != null);
            EmployeeDetailCommand = new Command(EmployeeDetail, () => SelectedEmployee != null);
            LoadEmployeesCommand = new Command(async () => await LoadEmployees());
            //SearchCommand = new Command<string>(Search);
        }

        public async Task InitializeAsync()
        {
            Employees = new ObservableCollection<EmployeeDTO>(await employeeService.GetAllEmployees());
        }

        public ObservableCollection<EmployeeDTO> Employees
        {
            get => employees;
            set
            {
                employees = value;
                OnPropertyChanged();
            }
        }

        public EmployeeDTO employeeDTO
        {
            get => employeeDTOModel;
            set
            {
                employeeDTOModel = value;
                OnPropertyChanged();
            }
        }

        public EmployeeDTO SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                if (selectedEmployee != value)
                {
                    selectedEmployee = value;
                    OnPropertyChanged();
                    ((Command)UpdateEmployeeCommand).ChangeCanExecute();
                    ((Command)DeleteEmployeeCommand).ChangeCanExecute();
                    ((Command)EmployeeDetailCommand).ChangeCanExecute();
                }
            }
        }

        public ICommand AddEmployeeCommand { get; private set; }
        public ICommand UpdateEmployeeCommand { get; private set; }
        public ICommand DeleteEmployeeCommand { get; private set; }
        public ICommand LoadEmployeesCommand { get; private set; }
        public ICommand EmployeeDetailCommand { get; private set; }
        public ICommand SearchCommand { get; set; }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                Task.Run(() => Search(_searchText));
            }
        }

        public async Task Search(string searchText)
        {
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchResults = await employeeService.SearchEmployee(searchText);
                Employees = new ObservableCollection<EmployeeDTO>(searchResults);
            }
            else
            {
                await LoadEmployees();
            }
        }

        private async Task AddEmployee()
        {
            if (!string.IsNullOrEmpty(employeeModel.Name)) 
            {
                try
                {
                    var employeeDto = new EmployeeDTO
                    {
                        Id = employeeModel.Id,
                        Name = employeeModel.Name,
                        Rate = employeeModel.Rate
                    };

                    await employeeService.AddEmployee(employeeDto);
                    await LoadEmployees();
                    employeeDTO = new EmployeeDTO();
                    employeeModel = new Employee(employeeDTOModel);
                    SelectedEmployee = null;

                    OnPropertyChanged(nameof(employeeDTO));
                    OnPropertyChanged(nameof(employeeModel));
                }
                catch (Exception e)
                {
                    Shell.Current.DisplayAlert("Error", e.Message, "OK");
                }
            }
            else
            {
                Shell.Current.DisplayAlert("Error", "Employee name cannot be empty", "OK");
            }
        }

        private async Task UpdateEmployee()
        {
            if (selectedEmployee != null)
            {
                try
                {
                    var employeeToUpdate = new EmployeeDTO
                    {
                        Id = SelectedEmployee.Id,
                        Name = employeeModel.Name ?? SelectedEmployee.Name,
                        Rate = employeeModel.Rate,
                    };

                    await employeeService.UpdateEmployee(employeeToUpdate);
                    await LoadEmployees();
                    employeeDTO = new EmployeeDTO();
                    employeeModel = new Employee(employeeDTOModel);
                    SelectedEmployee = null;

                    OnPropertyChanged(nameof(employeeDTO));
                    OnPropertyChanged(nameof(employeeModel));
                }
                catch (Exception e)
                {
                    Shell.Current.DisplayAlert("Error", e.Message, "OK");
                }
            }
            else
            {
                Shell.Current.DisplayAlert("Error", "An employee must be selected", "OK");
            }
        }

        private async Task DeleteEmployee()
        {
            if (selectedEmployee != null)
            {
                try
                {
                    await employeeService.DeleteEmployee(selectedEmployee.Id.ToString());
                    await LoadEmployees();
                    employeeDTO = new EmployeeDTO();
                    employeeModel = new Employee(employeeDTOModel);
                    SelectedEmployee = null;

                    OnPropertyChanged(nameof(employeeDTO));
                    OnPropertyChanged(nameof(employeeModel));
                }
                catch (Exception e)
                {
                    Shell.Current.DisplayAlert("Error", e.Message, "OK");
                }
            }
            else
            {
                Shell.Current.DisplayAlert("Error", "An employee must be selected", "OK");
            }
        }

        private void EmployeeDetail()
        {
            if (SelectedEmployee != null)
            {
                Shell.Current.GoToAsync($"//EmployeeDetails?employeeId={SelectedEmployee.Id}");
                employeeDTO = new EmployeeDTO();
                employeeModel = new Employee(employeeDTOModel);
                SelectedEmployee = null;

                OnPropertyChanged(nameof(employeeDTO));
                OnPropertyChanged(nameof(employeeModel));
            }
        }

        private async Task LoadEmployees()
        {
            SelectedEmployee = null;

            employeeDTO = new EmployeeDTO();
            employeeModel = new Employee(employeeDTOModel);

            OnPropertyChanged(nameof(employeeDTO));
            OnPropertyChanged(nameof(employeeModel));

            Employees = new ObservableCollection<EmployeeDTO>(await employeeService.GetAllEmployees());
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
