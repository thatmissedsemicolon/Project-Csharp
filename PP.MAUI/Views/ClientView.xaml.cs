using PP.MAUI.ViewModels;

namespace PP.MAUI.Views
{
    public partial class ClientView : ContentPage
    {
        public ClientView()
        {
            InitializeComponent();
            BindingContext = new ClientViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as ClientViewModel).LoadClientsCommand.Execute(null);
        }

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = e.NewTextValue;
            await (BindingContext as ClientViewModel).Search(searchText);
        }

        private void OkClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Home");
        }
    }
}
