using PP.Library.Models;
using PP.Library.DTO;
using PP.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PP.MAUI.ViewModels
{
    public class TimeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private TimeService timeService;
        private ClientService clientService;
        private EmployeeService employeeService;
        private TimeDTO timeDTOModel;
        public Time timeModel { get; set; }
        private TimeDTO selectedTime;
        private ProjectDTO selectedProject;
        private EmployeeDTO selectedEmployee;
        //private Stopwatch stopwatch;
        private ObservableCollection<TimeDTO> times;
        private ObservableCollection<ProjectDTO> projects;
        private ObservableCollection<EmployeeDTO> employees;
        //private IDispatcherTimer timer;

        public TimeViewModel()
        {
            timeService = TimeService.Instance;
            clientService = ClientService.Instance;
            employeeService = EmployeeService.Instance;
            timeDTOModel = new TimeDTO();
            timeModel = new Time(timeDTOModel);
            times = new ObservableCollection<TimeDTO>();
            projects = new ObservableCollection<ProjectDTO>();
            employees = new ObservableCollection<EmployeeDTO>();

            //stopwatch = new Stopwatch();
            //timer = Application.Current.Dispatcher.CreateTimer();
            //timer.Interval = new TimeSpan(0, 0, 0, 1);
            //timer.IsRepeating = true;
            //timer.Tick += Timer_Tick;

            SetupCommands();
            LoadTimesCommand = new Command(async () => await LoadTimes());
            LoadProjectsCommand = new Command(async () => await LoadProjects());
            LoadEmployeesCommand = new Command(async () => await LoadEmployees());
        }

        public ObservableCollection<TimeDTO> Times
        {
            get => times;
            set
            {
                times = value;
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

        public ObservableCollection<EmployeeDTO> Employees
        {
            get => employees;
            set
            {
                employees = value;
                OnPropertyChanged();
            }
        }

        public TimeDTO timeDTO
        {
            get => timeDTOModel;
            set
            {
                timeDTOModel = value;
                OnPropertyChanged();
            }
        }

        public TimeDTO SelectedTime
        {
            get => selectedTime;
            set
            {
                if (selectedTime != value)
                {
                    selectedTime = value;
                    OnPropertyChanged();
                    ((Command)UpdateTimeCommand).ChangeCanExecute();
                    ((Command)DeleteTimeCommand).ChangeCanExecute();
                    ((Command)TimeDetailCommand).ChangeCanExecute();
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
                        timeDTO = new TimeDTO
                        {
                            ProjectId = selectedProject.Id,
                            EmployeeId = timeModel.EmployeeId,
                            Narrative = timeModel.Narrative,
                            Hours = timeModel.Hours
                        };
                    }
                    OnPropertyChanged();
                }
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
                    if (selectedEmployee != null)
                    {
                        timeDTO = new TimeDTO
                        {
                            ProjectId = timeModel.ProjectId,
                            EmployeeId = selectedEmployee.Id,
                            Narrative = timeModel.Narrative,
                            Hours = timeModel.Hours
                        };
                    }
                    OnPropertyChanged();
                }
            }
        }

        //public string TimerDisplay
        //{
        //    get
        //    {
        //        var time = stopwatch.Elapsed;
        //        var str = string.Format("{0:00}:{1:00}:{2:00}", time.Hours, time.Minutes, time.Seconds);
        //        return str;
        //    }
        //}

        //public ICommand StartCommand { get; private set; }
        //public ICommand StopCommand { get; private set; }
        public ICommand AddTimeCommand { get; private set; }
        public ICommand UpdateTimeCommand { get; private set; }
        public ICommand DeleteTimeCommand { get; private set; }
        public ICommand LoadTimesCommand { get; private set; }
        public ICommand TimeDetailCommand { get; private set; }
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
                var searchResults = await timeService.SearchTimes(searchText);
                Times = new ObservableCollection<TimeDTO>(searchResults);
            }
            else
            {
                await LoadProjects();
                await LoadEmployees();
                await LoadTimes();
            }
        }

        private void SetupCommands()
        {
            //StartCommand = new Command(ExecuteStart);
            //StopCommand = new Command(ExecuteStop);
            AddTimeCommand = new Command(async () => await AddTime());
            UpdateTimeCommand = new Command(async () => await UpdateTime(), () => SelectedTime != null);
            DeleteTimeCommand = new Command(async () => await DeleteTime(), () => SelectedTime != null);
            TimeDetailCommand = new Command(TimeDetail, () => SelectedTime != null);
        }

        //public void ExecuteStart()
        //{
        //    stopwatch.Start();
        //    timer.Start();
        //}

        //public void ExecuteStop()
        //{
        //    stopwatch.Stop();
        //    timer.Stop();
        //    decimal elapsedHours = Math.Round((decimal)stopwatch.Elapsed.TotalHours, 2);
        //    Time.Hours = elapsedHours;
        //    stopwatch.Reset();
        //    OnPropertyChanged(nameof(Time));
        //}

        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    if (timer.IsRunning)
        //    {
        //        OnPropertyChanged(nameof(TimerDisplay));
        //    }
        //}

        private async Task AddTime()
        {
            if (timeModel.Hours > 0)
            {
                try
                {
                    timeModel.ProjectId = selectedProject.Id;
                    timeModel.EmployeeId = selectedEmployee.Id;
                    timeModel.Date = DateTime.Now;
                    timeModel.ProjectName = selectedProject.Name;

                    var timeDto = new TimeDTO
                    {
                        Id = timeModel.Id,
                        Date = timeModel.Date,
                        Narrative = timeModel.Narrative,
                        Hours = timeModel.Hours,
                        ProjectId = timeModel.ProjectId,
                        EmployeeId = timeModel.EmployeeId,
                        ProjectName = timeModel.ProjectName
                    };

                    await timeService.AddTime(timeDto);
                    //stopwatch.Reset();
                    await LoadTimes();
                    timeDTO = new TimeDTO();
                    timeModel = new Time(timeDTOModel);
                    //OnPropertyChanged(nameof(Time));
                    //OnPropertyChanged(nameof(TimerDisplay));
                    SelectedTime = null;
                    SelectedProject = null; 
                    SelectedEmployee = null;
                    OnPropertyChanged(nameof(timeDTO));
                    OnPropertyChanged(nameof(timeModel));
                }
                catch (Exception e)
                {
                    Shell.Current.DisplayAlert("Error", e.Message, "OK");
                }
            }
            else
            {
                Shell.Current.DisplayAlert("Error", "Hours cannot be zero", "OK");
            }
        }

        private async Task UpdateTime()
        {
            if (selectedTime != null && timeModel.Hours > 0)
            {
                try
                {
                    var updatedTime = new TimeDTO
                    {
                        Id = selectedTime.Id,
                        Date = DateTime.Now,
                        Narrative = timeModel.Narrative ?? selectedTime.Narrative,
                        Hours = timeModel.Hours > 0 ? timeModel.Hours : selectedTime.Hours,
                        ProjectId = selectedTime.ProjectId,
                        EmployeeId = selectedTime.EmployeeId,
                        ProjectName = selectedProject.Name ?? selectedTime.ProjectName
                    };

                    await timeService.UpdateTime(updatedTime);
                    //stopwatch.Reset();
                    await LoadTimes();
                    timeDTO = new TimeDTO();
                    timeModel = new Time(timeDTOModel);
                    //OnPropertyChanged(nameof(Time));
                    //OnPropertyChanged(nameof(TimerDisplay));
                    SelectedTime = null;
                    SelectedProject = null;
                    SelectedEmployee = null;
                    OnPropertyChanged(nameof(timeDTO));
                    OnPropertyChanged(nameof(timeModel));
                }
                catch (Exception e)
                {
                    Shell.Current.DisplayAlert("Error", e.Message, "OK");
                }
            }
            else
            {
                Shell.Current.DisplayAlert("Error", "A time must be selected", "OK");
            }
        }

        private async Task DeleteTime()
        {
            if (selectedTime != null)
            {
                try
                {
                    await timeService.DeleteTime(selectedTime.Id.ToString());
                    await LoadTimes();
                    timeDTO = new TimeDTO();
                    timeModel = new Time(timeDTOModel);
                    //OnPropertyChanged(nameof(Time));
                    //OnPropertyChanged(nameof(TimerDisplay));
                    SelectedTime = null;
                    SelectedProject = null;
                    SelectedEmployee = null;
                    OnPropertyChanged(nameof(timeDTO));
                    OnPropertyChanged(nameof(timeModel));
                }
                catch (Exception e)
                {
                    Shell.Current.DisplayAlert("Error", e.Message, "OK");
                }
            }
            else
            {
                Shell.Current.DisplayAlert("Error", "A time must be selected", "OK");
            }
        }

        private void TimeDetail()
        {
            if (SelectedTime != null)
            {
                Shell.Current.GoToAsync($"//TimeDetails?timeId={SelectedTime.Id}");
                timeDTO = new TimeDTO();
                timeModel = new Time(timeDTOModel);
                //OnPropertyChanged(nameof(Time));
                //OnPropertyChanged(nameof(TimerDisplay));
                SelectedTime = null;
                SelectedProject = null;
                SelectedEmployee = null;
                OnPropertyChanged(nameof(timeDTO));
                OnPropertyChanged(nameof(timeModel));
            }
        }

        private async Task LoadProjects()
        {
            timeDTO = new TimeDTO();
            timeModel = new Time(timeDTOModel);
            //OnPropertyChanged(nameof(Time));
            //OnPropertyChanged(nameof(TimerDisplay));
            SelectedTime = null;
            SelectedProject = null;
            SelectedEmployee = null;
            OnPropertyChanged(nameof(timeDTO));
            OnPropertyChanged(nameof(timeModel));
            var allClients = await clientService.GetAllClients();
            Projects = new ObservableCollection<ProjectDTO>(allClients.SelectMany(c => c.Projects));
        }

        private async Task LoadEmployees()
        {
            timeDTO = new TimeDTO();
            timeModel = new Time(timeDTOModel);
            //OnPropertyChanged(nameof(Time));
            //OnPropertyChanged(nameof(TimerDisplay));
            SelectedTime = null;
            SelectedProject = null;
            SelectedEmployee = null;
            OnPropertyChanged(nameof(timeDTO));
            OnPropertyChanged(nameof(timeModel));
            var allEmployees = await employeeService.GetAllEmployees();
            Employees = new ObservableCollection<EmployeeDTO>(allEmployees);
        }

        private async Task LoadTimes()
        {
            timeDTO = new TimeDTO();
            timeModel = new Time(timeDTOModel);
            //OnPropertyChanged(nameof(Time));
            //OnPropertyChanged(nameof(TimerDisplay));
            SelectedTime = null;
            SelectedProject = null;
            SelectedEmployee = null;
            OnPropertyChanged(nameof(timeDTO));
            OnPropertyChanged(nameof(timeModel));
            var allTimes = await timeService.GetAllTimes();
            Times = new ObservableCollection<TimeDTO>(allTimes);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}