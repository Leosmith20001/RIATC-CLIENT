using System;
using System.Media;
using System.Threading.Tasks;
using System.Windows;

namespace RIATCClient
{
    public partial class LoadingWindow : Window
    {
        public LoadingWindow()
        {
            InitializeComponent();
            Loaded += LoadingWindow_Loaded;
        }

        private async void LoadingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(7000); // Simulate 7-second loading time

            try
            {
                SoundPlayer player = new SoundPlayer("Assets/loaded.wav");
                player.Play();
            }
            catch
            {
                // optional: ignore if sound missing
            }

            ClientWindow client = new ClientWindow();
            client.Show();
            this.Close();
        }
    }
}
