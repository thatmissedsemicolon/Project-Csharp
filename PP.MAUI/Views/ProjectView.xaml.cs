using PP.MAUI.ViewModels;

namespace PP.MAUI.Views
{
    public partial class ProjectView : ContentPage
    {
        public ProjectView()
        {
            InitializeComponent();
            BindingContext = new ProjectViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as ProjectViewModel).LoadProjectsCommand.Execute(null);
            (BindingContext as ProjectViewModel).LoadClientsCommand.Execute(null);
        }

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = e.NewTextValue;
            await (BindingContext as ProjectViewModel).Search(searchText);
        }

        private void OkClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Home");
        }
    }
}
