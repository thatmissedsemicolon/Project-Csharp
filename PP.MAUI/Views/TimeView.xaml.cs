using PP.MAUI.ViewModels;

namespace PP.MAUI.Views
{
    public partial class TimeView : ContentPage
    {
        public TimeView()
        {
            InitializeComponent();
            BindingContext = new TimeViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = BindingContext as TimeViewModel;
            viewModel.LoadTimesCommand.Execute(null);
            viewModel.LoadProjectsCommand.Execute(null);
            viewModel.LoadEmployeesCommand.Execute(null);
        }

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = e.NewTextValue;
            await (BindingContext as TimeViewModel).Search(searchText);
        }

        private void OkClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Home");
        }
    }
}
