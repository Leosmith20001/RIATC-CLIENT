using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace RIATCClient
{
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            ErrorText.Visibility = Visibility.Collapsed;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ErrorText.Text = "Please enter both email and password.";
                ErrorText.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                bool success = await FirebaseLogin(email, password);

                if (success)
                {
                    LoadingWindow loading = new LoadingWindow();
                    loading.Show();
                    this.Close();
                }
                else
                {
                    ErrorText.Text = "Invalid login details.";
                    ErrorText.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                ErrorText.Text = $"Error: {ex.Message}";
                ErrorText.Visibility = Visibility.Visible;
            }
        }

        private async Task<bool> FirebaseLogin(string email, string password)
        {
            var payload = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            string apiKey = "AIzaSyB8d406Jh5x8fDx0Ib168KgxMZa5SuCOdo";
            string url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}";

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            return response.IsSuccessStatusCode;
        }
    }
}
