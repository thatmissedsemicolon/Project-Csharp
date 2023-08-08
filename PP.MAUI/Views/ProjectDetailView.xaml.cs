using PP.MAUI.ViewModels;

namespace PP.MAUI.Views
{
    [QueryProperty(nameof(ProjectId), "projectId")]
    public partial class ProjectDetailView : ContentPage
    {
        private ProjectDetailViewModel viewModel;

        public int ProjectId
        {
            set
            {
                viewModel.ProjectId = value;
                viewModel.GetProject();
            }
        }

        public ProjectDetailView()
        {
            InitializeComponent();
            viewModel = new ProjectDetailViewModel();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void OkClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Project");
        }
    }
}
