using PP.MAUI.ViewModels;
using Microsoft.Maui.Controls;

namespace PP.MAUI.Views
{
    [QueryProperty(nameof(EmployeeId), "employeeId")]
    public partial class EmployeeDetailView : ContentPage
    {
        private EmployeeDetailViewModel viewModel;

        public int EmployeeId
        {
            set
            {
                viewModel.EmployeeId = value;
                Task.Run(async () => await viewModel.GetEmployee());
            }
        }

        public EmployeeDetailView()
        {
            InitializeComponent();
            viewModel = new EmployeeDetailViewModel();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void OkClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Employee");
        }
    }
}
