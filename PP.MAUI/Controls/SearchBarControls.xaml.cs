using System.Windows.Input;

namespace PP.MAUI.Controls
{
    public partial class SearchBarControls : ContentView
    {
        public SearchBarControls()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public static readonly BindableProperty QueryTextProperty = BindableProperty.Create(
             nameof(QueryText),
             typeof(string),
             typeof(SearchBarControls),
             default(string),
             BindingMode.TwoWay);

        public static readonly BindableProperty SearchCommandProperty = BindableProperty.Create(
            nameof(SearchCommand),
            typeof(ICommand),
            typeof(SearchBarControls),
            default(ICommand),
            BindingMode.TwoWay);

        public static readonly BindableProperty SearchCommandParameterProperty = BindableProperty.Create(
            nameof(SearchCommandParameter),
            typeof(object),
            typeof(SearchBarControls),
            default(object));

        public string QueryText
        {
            get => (string)GetValue(QueryTextProperty);
            set => SetValue(QueryTextProperty, value);
        }


        public ICommand SearchCommand
        {
            get => (ICommand)GetValue(SearchCommandProperty);
            set => SetValue(SearchCommandProperty, value);
        }

        public object SearchCommandParameter
        {
            get => GetValue(SearchCommandParameterProperty);
            set => SetValue(SearchCommandParameterProperty, value);
        }
    }
}
