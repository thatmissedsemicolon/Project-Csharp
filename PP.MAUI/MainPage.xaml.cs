using PP.MAUI.ViewModels;

namespace PP.MAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new ClientViewModel();
        }

        void OnClientButtonClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Client");
        }

        void OnProjectButtonClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Project");
        }

        void OnEmployeesButtonClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Employee");
        }

        void OnTimeButtonClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Time");
        }

        void OnBillButtonClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Bill");
        }
    }
}