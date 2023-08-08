using PP.MAUI.ViewModels;

namespace PP.MAUI.Views
{
    [QueryProperty(nameof(TimeId), "timeId")]
    public partial class TimeDetailView : ContentPage
    {
        private TimeDetailViewModel viewModel;

        public string TimeId
        {
            set
            {
                viewModel.TimeId = int.Parse(value);
                viewModel.GetTime();
            }
        }

        public TimeDetailView()
        {
            InitializeComponent();
            viewModel = new TimeDetailViewModel();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void OkClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Time");
        }
    }
}
