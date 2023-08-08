using PP.Library.Models;
using PP.Library.DTO;
using PP.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PP.MAUI.ViewModels
{
    public class BillViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime dueDate = DateTime.Now;

        private BillService billService;
        private ClientService clientService;
        private BillDTO billDTOModel;
        public Bill billModel { get; set; }
        private BillDTO selectedBill;
        private ProjectDTO selectedProject;
        private ObservableCollection<BillDTO> bills;
        private ObservableCollection<ProjectDTO> projects;

        public BillViewModel()
        {
            billService = BillService.Instance;
            clientService = ClientService.Instance;
            billDTOModel = new BillDTO();
            billModel = new Bill(billDTOModel);
            bills = new ObservableCollection<BillDTO>();
            projects = new ObservableCollection<ProjectDTO>();
            AddBillCommand = new Command(async () => await AddBill());
            UpdateBillCommand = new Command(async () => await UpdateBill(), () => SelectedBill != null);
            DeleteBillCommand = new Command(async () => await DeleteBill(), () => SelectedBill != null);
            BillDetailCommand = new Command(BillDetail, () => SelectedBill != null);
            LoadBillsCommand = new Command(async () => await LoadBills());
            LoadProjectsCommand = new Command(async () => await LoadProjects());
        }

        public async Task InitializeAsync()
        {
            Bills = new ObservableCollection<BillDTO>(await billService.GetAllBills());
        }

        public ObservableCollection<BillDTO> Bills
        {
            get => bills;
            set
            {
                bills = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ProjectDTO> Projects
        {
            get => projects;
            set
            {
                projects = value;
                OnPropertyChanged();
            }
        }

        public BillDTO billDTO
        {
            get => billDTOModel;
            set
            {
                billDTOModel = value;
                OnPropertyChanged();
            }
        }

        public BillDTO SelectedBill
        {
            get => selectedBill;
            set
            {
                if (selectedBill != value)
                {
                    selectedBill = value;
                    OnPropertyChanged();
                    ((Command)UpdateBillCommand).ChangeCanExecute();
                    ((Command)DeleteBillCommand).ChangeCanExecute();
                    ((Command)BillDetailCommand).ChangeCanExecute();
                }
            }
        }

        public ProjectDTO SelectedProject
        {
            get => selectedProject;
            set
            {
                if (selectedProject != value)
                {
                    selectedProject = value;
                    if (selectedProject != null)
                    {
                        billDTO = new BillDTO
                        {
                            ProjectId = selectedProject.Id,
                        };
                    }
                    CalculateTotalAmount();
                    OnPropertyChanged(nameof(Bill));
                    OnPropertyChanged();
                }
            }
        }

        public decimal TotalAmount
        {
            get => billModel?.TotalAmount ?? 0;
            private set
            {
                if (billModel != null)
                {
                    billModel.TotalAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        private async void CalculateTotalAmount()
        {
            if (selectedProject == null)
            {
                return;
            }

            if (selectedProject.Id <= 0)
            {
                return;
            }

            var times = await TimeService.Instance.GetAllTimes();

            if (times == null)
            {
                return;
            }

            decimal totalAmount = 0;

            var projectTimes = times.Where(t => t.ProjectId == selectedProject.Id);

            foreach (var timeRecord in projectTimes)
            {
                var employee = await EmployeeService.Instance.GetEmployeeById(timeRecord.EmployeeId);
                if (employee != null)
                {
                    totalAmount += timeRecord.Hours * employee.Rate;
                }
            }

            TotalAmount = totalAmount;
        }

        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                dueDate = value;
                OnPropertyChanged(nameof(DueDate));
            }
        }

        public ICommand AddBillCommand { get; private set; }
        public ICommand UpdateBillCommand { get; private set; }
        public ICommand DeleteBillCommand { get; private set; }
        public ICommand LoadBillsCommand { get; private set; }
        public ICommand BillDetailCommand { get; private set; }
        public ICommand LoadProjectsCommand { get; private set; }
        public ICommand LoadEmployeesCommand { get; private set; }

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
                var searchResults = await billService.SearchBills(searchText);
                Bills = new ObservableCollection<BillDTO>(searchResults);
            }
            else
            {
                await LoadProjects();
                await LoadBills();
            }
        }
        private async Task AddBill()
        {
            if (TotalAmount > 0)
            {
                try
                {
                    BillDTO newBill = new BillDTO()
                    {
                        TotalAmount = TotalAmount,
                        DueDate = DueDate,
                        ProjectId = selectedProject.Id,
                    };

                    await billService.AddBill(newBill);
                    await LoadBills();

                    billDTO = new BillDTO();
                    billModel = new Bill(billDTOModel);
                    SelectedProject = null;
                    TotalAmount = 0;
                    DueDate = DateTime.Now;
                    SelectedBill = null;
                    OnPropertyChanged(nameof(billDTO));
                    OnPropertyChanged(nameof(billModel));
                }
                catch (Exception e)
                {
                    Shell.Current.DisplayAlert("Error", e.Message, "OK");
                }
            }
            else
            {
                Shell.Current.DisplayAlert("Error", "Bill total amount cannot be zero", "OK");
            }
        }

        private async Task UpdateBill()
        {
            if (SelectedBill != null && billModel.TotalAmount > 0)
            {
                try
                {
                    var updatedBill = new BillDTO
                    {
                        Id = selectedBill.Id,
                        ProjectId = selectedBill.ProjectId,
                        TotalAmount = TotalAmount,
                        DueDate = DueDate != DateTime.Now ? DueDate : selectedBill.DueDate,
                    };

                    await billService.UpdateBill(updatedBill);
                    await LoadBills();

                    billDTO = new BillDTO();
                    billModel = new Bill(billDTOModel);
                    SelectedProject = null;
                    TotalAmount = 0;
                    DueDate = DateTime.Now;
                    SelectedBill = null;
                    OnPropertyChanged(nameof(billDTO));
                    OnPropertyChanged(nameof(billModel));
                }
                catch (Exception e)
                {
                    Shell.Current.DisplayAlert("Error", e.Message, "OK");
                }
            }
            else
            {
                Shell.Current.DisplayAlert("Error", "Bill total amount cannot be zero", "OK");
            }
        }

        private async Task DeleteBill()
        {
            if (SelectedBill != null)
            {
                try
                {
                    await billService.DeleteBill(SelectedBill.Id.ToString());
                    await LoadBills();

                    billDTO = new BillDTO();
                    billModel = new Bill(billDTOModel);
                    SelectedProject = null;
                    TotalAmount = 0;
                    DueDate = DateTime.Now;
                    SelectedBill = null;
                    OnPropertyChanged(nameof(billDTO));
                    OnPropertyChanged(nameof(billModel));
                }
                catch (Exception e)
                {
                    Shell.Current.DisplayAlert("Error", e.Message, "OK");
                }
            }
            else
            {
                Shell.Current.DisplayAlert("Error", "A Bill must be selected", "OK");
            }
        }

        private void BillDetail()
        {
            Shell.Current.GoToAsync($"//BillDetails?billId={SelectedBill.Id}");

            billDTO = new BillDTO();
            billModel = new Bill(billDTOModel);
            SelectedProject = null;
            TotalAmount = 0;
            DueDate = DateTime.Now;
            SelectedBill = null;
            OnPropertyChanged(nameof(billDTO));
            OnPropertyChanged(nameof(billModel));
        }

        private async Task LoadProjects()
        {
            var allClients = await clientService.GetAllClients();
            Projects = new ObservableCollection<ProjectDTO>(allClients.SelectMany(c => c.Projects));
        }

        private async Task LoadBills()
        {
            billDTO = new BillDTO();
            billModel = new Bill(billDTOModel);
            SelectedProject = null;
            TotalAmount = 0;
            DueDate = DateTime.Now;
            SelectedBill = null;
            OnPropertyChanged(nameof(billDTO));
            OnPropertyChanged(nameof(billModel));
            Bills = new ObservableCollection<BillDTO>(await billService.GetAllBills());
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
