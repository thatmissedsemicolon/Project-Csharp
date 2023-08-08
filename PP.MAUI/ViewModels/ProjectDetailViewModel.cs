using PP.Library.DTO;
using PP.Library.Models;
using PP.Library.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PP.MAUI.ViewModels
{
    public class ProjectDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ClientService clientService;
        private ProjectDTO projectDTOModel;
        private Project projectModel;
        private int projectId;

        public Project ProjectModel
        {
            get => projectModel;
            set
            {
                projectModel = value;
                OnPropertyChanged();
            }
        }

        public ProjectDTO ProjectDTOModel
        {
            get => projectDTOModel;
            set
            {
                if (projectDTOModel != value)
                {
                    projectDTOModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ProjectId
        {
            get => projectId;
            set
            {
                projectId = value;
                OnPropertyChanged();
            }
        }

        public ProjectDetailViewModel()
        {
            clientService = ClientService.Instance;
            projectDTOModel = new ProjectDTO();
            projectModel = new Project(projectDTOModel);
        }

        public async void GetProject()
        {
            foreach (var client in await clientService.GetAllClients())
            {
                var projectDetails = client.Projects.Find(p => p.Id == ProjectId);

                if (projectDetails != null)
                {
                    ProjectDTOModel = projectDetails;
                    ProjectModel = new Project(ProjectDTOModel);
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
