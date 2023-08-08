using PP.MAUI.ViewModels;

namespace PP.MAUI.Views
{
    [QueryProperty(nameof(ClientId), "clientId")]
    public partial class ClientDetailView : ContentPage
    {
        private ClientDetailViewModel viewModel;

        public int ClientId
        {
            set
            {
                viewModel.ClientId = value;
                Task.Run(viewModel.GetClient);
            }
        }

        public ClientDetailView()
        {
            InitializeComponent();
            viewModel = new ClientDetailViewModel();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void OkClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Client");
        }
    }
}
