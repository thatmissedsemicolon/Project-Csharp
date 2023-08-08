using PP.MAUI.ViewModels;

namespace PP.MAUI.Views
{
    public partial class BillView : ContentPage
    {
        public BillView()
        {
            InitializeComponent();
            BindingContext = new BillViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as BillViewModel).LoadBillsCommand.Execute(null);
            (BindingContext as BillViewModel).LoadProjectsCommand.Execute(null);
        }

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = e.NewTextValue;
            await (BindingContext as BillViewModel).Search(searchText);
        }

        private void OkClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Home");
        }
    }
}
