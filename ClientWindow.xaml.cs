using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Firebase.Database;
using Firebase.Database.Query;

namespace RIATCClient
{
    public partial class ClientWindow : Window
    {
        private static readonly HttpClient http = new HttpClient();
        private bool isConnected = false;

        // ✅ Your API base URL (update to your actual deployment)
        private const string API_BASE_URL = "https://riatc-api-vng4.vercel.app/api";

        // ✅ Firebase client (Realtime Database)
        private readonly FirebaseClient firebaseClient;

        public ClientWindow()
        {
            InitializeComponent();

            // Initialize Firebase
            firebaseClient = new FirebaseClient(
                "https://riatc-4ca69-default-rtdb.firebaseio.com/",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult("YOUR_DATABASE_SECRET") // Replace later with proper token
                });
        }

        private async void OpenConnectDialog(object sender, RoutedEventArgs e)
        {
            if (!isConnected)
            {
                var connectWindow = new ConnectWindow { Owner = this };
                bool? result = connectWindow.ShowDialog();

                if (result == true)
                {
                    await ConnectToNetwork(connectWindow.Callsign, connectWindow.TypeCode);
                }
            }
            else
            {
                await DisconnectFromNetwork();
            }
        }

        private async Task ConnectToNetwork(string callsign, string type)
        {
            try
            {
                // 🔹 Call the API to increment pilot count
                await http.PostAsync($"{API_BASE_URL}/connect", null);

                // 🔹 Update Firebase user stats
                await IncrementFirebaseConnections("USER_FIREBASE_UID"); // Replace later with actual auth UID

                // 🔹 Update UI
                isConnected = true;
                ConnectButton.Content = "Connected";
                ConnectButton.Background = Brushes.ForestGreen;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}");
            }
        }

        private async Task DisconnectFromNetwork()
        {
            try
            {
                // 🔹 Call the API to decrement pilot count
                await http.PostAsync($"{API_BASE_URL}/disconnect", null);

                isConnected = false;
                ConnectButton.Content = "Connect";
                ConnectButton.Background = Brushes.DimGray;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Disconnection failed: {ex.Message}");
            }
        }

        private async Task IncrementFirebaseConnections(string userId)
        {
            try
            {
                // 🔹 Get current count
                var currentValue = await firebaseClient
                    .Child("users")
                    .Child(userId)
                    .Child("connections")
                    .OnceSingleAsync<int>();

                // 🔹 Increment
                await firebaseClient
                    .Child("users")
                    .Child(userId)
                    .Child("connections")
                    .PutAsync(currentValue + 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Firebase increment error: {ex.Message}");
            }
        }

        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
