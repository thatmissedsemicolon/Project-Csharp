using PP.MAUI.ViewModels;

namespace PP.MAUI.Views
{
    public partial class EmployeeView : ContentPage
    {
        public EmployeeView()
        {
            InitializeComponent();
            BindingContext = new EmployeeViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as EmployeeViewModel).LoadEmployeesCommand.Execute(null);
        }

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = e.NewTextValue;
            await (BindingContext as EmployeeViewModel).Search(searchText);
        }

        private void OkClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Home");
        }
    }
}
