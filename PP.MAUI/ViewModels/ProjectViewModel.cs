using PP.Library.DTO;
using PP.Library.Models;
using PP.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PP.MAUI.ViewModels
{
    public class ProjectViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ClientService clientService;
        private ProjectDTO projectDTOModel;
        public Project projectModel { get; set; }
        private ProjectDTO selectedProject;
        private ObservableCollection<ProjectDTO> projects;
        private ObservableCollection<ClientDTO> clients;
        private ClientDTO selectedClient;

        public ProjectViewModel()
        {
            clientService = ClientService.Instance;
            projectDTOModel = new ProjectDTO();
            projectModel = new Project(projectDTOModel);
            projects = new ObservableCollection<ProjectDTO>();
            clients = new ObservableCollection<ClientDTO>();
            AddProjectCommand = new Command(async () => await AddProject());
            UpdateProjectCommand = new Command(async () => await UpdateProject(), () => SelectedProject != null);
            DeleteProjectCommand = new Command(async () => await DeleteProject(), () => SelectedProject != null);
            ProjectDetailCommand = new Command(ProjectDetail, () => SelectedProject != null);
            LoadProjectsCommand = new Command(async () => await LoadProjects());
            LoadClientsCommand = new Command(async () => await LoadClients());
            //SearchCommand = new Command<string>(async (query) => await Search(query));
        }

        public async Task InitializeAsync()
        {
            Projects = new ObservableCollection<ProjectDTO>(await clientService.GetAllProjects());
            Clients = new ObservableCollection<ClientDTO>(await clientService.GetAllClients());
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

        public ProjectDTO projectDTO
        {
            get => projectDTOModel;
            set
            {
                projectDTOModel = value;
                OnPropertyChanged();
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
                    OnPropertyChanged();
                    ((Command)UpdateProjectCommand).ChangeCanExecute();
                    ((Command)DeleteProjectCommand).ChangeCanExecute();
                    ((Command)ProjectDetailCommand).ChangeCanExecute();
                }
            }
        }

        public ObservableCollection<ClientDTO> Clients
        {
            get => clients;
            set
            {
                clients = value;
                OnPropertyChanged();
            }
        }

        public ClientDTO SelectedClient
        {
            get => selectedClient;
            set
            {
                if (selectedClient != value)
                {
                    selectedClient = value;
                    if (selectedClient != null)
                        projectModel.ClientId = selectedClient.Id;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddProjectCommand { get; private set; }
        public ICommand UpdateProjectCommand { get; private set; }
        public ICommand DeleteProjectCommand { get; private set; }
        public ICommand LoadProjectsCommand { get; private set; }
        public ICommand ProjectDetailCommand { get; private set; }
        public ICommand LoadClientsCommand { get; private set; }
        //public ICommand SearchCommand { get; set; }

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
                var searchResults = await clientService.SearchProjects(searchText);
                Projects = new ObservableCollection<ProjectDTO>(searchResults);
            }
            else
            {
                await LoadProjects();
                await LoadClients();
            }
        }

        private async Task AddProject()
        {
            if (SelectedClient != null && !string.IsNullOrEmpty(projectModel.Name))
            {
                projectModel.OpenDate = DateTime.Now;

                var projectDto = new ProjectDTO
                {
                    Id = projectModel.Id,
                    Name = projectModel.Name,
                    OpenDate = projectModel.OpenDate,
                    IsClosed = projectModel.IsClosed,
                    ShortName = projectModel.ShortName,
                    LongName = projectModel.LongName,
                    ClientId = SelectedClient.Id
                };

                await clientService.AddProjectToClient(SelectedClient.Id, projectDto);
                await LoadProjects();
                await LoadClients();
                projectDTO = new ProjectDTO();
                projectModel = new Project(projectDTOModel);
                SelectedProject = null;
                SelectedClient = null;

                OnPropertyChanged(nameof(projectDTO));
                OnPropertyChanged(nameof(projectModel));
            }
        }

        private async Task UpdateProject()
        {
            if (SelectedProject != null)
            {
                var projectToUpdate = new ProjectDTO
                {
                    Id = SelectedProject.Id,
                    Name = !string.IsNullOrEmpty(projectModel.Name) ? projectModel.Name : SelectedProject.Name,
                    IsClosed = projectModel.IsClosed,
                    ShortName = projectModel.ShortName ?? selectedProject.ShortName,
                    LongName = projectModel.LongName ?? selectedProject.LongName
                };

                await clientService.UpdateProjectInClient(SelectedProject.ClientId, projectToUpdate);
                await LoadProjects();
                await LoadClients();
                projectDTO = new ProjectDTO();
                projectModel = new Project(projectDTOModel);
                SelectedProject = null;
                SelectedClient = null;

                OnPropertyChanged(nameof(projectDTO));
                OnPropertyChanged(nameof(projectModel));
            }
        }


        private async Task DeleteProject()
        {
            if (SelectedProject != null)
            {
                await clientService.DeleteProjectFromClient(SelectedProject.Id);
                await LoadProjects();
                await LoadClients();
                projectDTO = new ProjectDTO();
                projectModel = new Project(projectDTOModel);
                SelectedProject = null;
                SelectedClient = null;

                OnPropertyChanged(nameof(projectDTO));
                OnPropertyChanged(nameof(projectModel));
            }
        }

        private void ProjectDetail()
        {
            if (SelectedProject != null)
            {
                Shell.Current.GoToAsync($"//ProjectDetails?projectId={SelectedProject.Id}");
                projectDTO = new ProjectDTO();
                projectModel = new Project(projectDTOModel);
                SelectedProject = null;
                SelectedClient = null;

                OnPropertyChanged(nameof(projectDTO));
                OnPropertyChanged(nameof(projectModel));
            }
        }

        private async Task LoadProjects()
        {
            SelectedProject = null;
            SelectedClient = null;
            projectDTO = new ProjectDTO();
            projectModel = new Project(projectDTOModel);

            OnPropertyChanged(nameof(projectDTO));
            OnPropertyChanged(nameof(projectModel));
            Projects = new ObservableCollection<ProjectDTO>(await clientService.GetAllProjects());
        }

        private async Task LoadClients()
        {
            SelectedProject = null;
            SelectedClient = null;
            projectDTO = new ProjectDTO();
            projectModel = new Project(projectDTOModel);

            OnPropertyChanged(nameof(projectDTO));
            OnPropertyChanged(nameof(projectModel));
            Clients = new ObservableCollection<ClientDTO>(await clientService.GetAllClients());
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
