using System.Windows;

namespace RIATCClient
{
    public partial class ConnectWindow : Window
    {
        public string Callsign { get; private set; } = string.Empty;
        public string TypeCode { get; private set; } = string.Empty;
        public bool ObserverMode { get; private set; } = false;

        public ConnectWindow()
        {
            InitializeComponent();
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            Callsign = CallsignBox.Text.Trim();
            TypeCode = TypeBox.Text.Trim();
            ObserverMode = ObserverCheck.IsChecked == true;

            if (string.IsNullOrWhiteSpace(Callsign) || string.IsNullOrWhiteSpace(TypeCode))
                return; // Simply do nothing if fields are empty

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
