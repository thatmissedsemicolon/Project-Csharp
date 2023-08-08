using PP.MAUI.ViewModels;

namespace PP.MAUI.Views
{
    [QueryProperty(nameof(BillId), "billId")]
    public partial class BillDetailView : ContentPage
    {
        private BillDetailViewModel viewModel;

        public int BillId
        {
            set
            {
                viewModel.BillId = value;
                viewModel.GetBill();
            }
        }

        public BillDetailView()
        {
            InitializeComponent();
            viewModel = new BillDetailViewModel();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void OkClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Bill");
        }
    }
}
