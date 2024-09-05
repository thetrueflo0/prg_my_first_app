using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace my_first_app
{
    /// <summary>
    /// Interaktionslogik für DashboardWindow.xaml
    /// </summary>
    public partial class DashboardWindow : Window
    {
        private DispatcherTimer _timer;

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the current window or application
            this.Close();
        }

        public DashboardWindow()
        {
            InitializeComponent();

            // Initialize and configure the DispatcherTimer
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(30);  // Set the interval to 30 seconds
            _timer.Tick += Timer_Tick;  // Assign the Tick event handler
            _timer.Start();  // Start the timer

            // Optionally, fetch the price once when the window loads
            FetchBitcoinPrice();
        }

        // Event handler for the DispatcherTimer Tick event
        private async void Timer_Tick(object sender, EventArgs e)
        {
            await FetchBitcoinPrice();
        }

        // Event handler for the Button Click event (Manual Refresh)
        private async void FetchBitcoinPrice_Click(object sender, RoutedEventArgs e)
        {
            await FetchBitcoinPrice();
        }

        // Fetches Bitcoin price data from the CoinDesk API
        private async Task FetchBitcoinPrice()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("https://api.coindesk.com/v1/bpi/currentprice.json");
                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var bitcoinData = JsonSerializer.Deserialize<BitcoinPrice>(jsonResponse);

                    // Update UI with the fetched Bitcoin prices
                    UsdPriceText.Text = $"USD: ${bitcoinData.bpi.USD.rate}";
                    GbpPriceText.Text = $"GBP: £{bitcoinData.bpi.GBP.rate}";
                    EurPriceText.Text = $"EUR: €{bitcoinData.bpi.EUR.rate}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching Bitcoin price: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Classes for deserializing JSON response from CoinDesk API
    public class BitcoinPrice
    {
        public Bpi bpi { get; set; }
    }

    public class Bpi
    {
        public Currency USD { get; set; }
        public Currency GBP { get; set; }
        public Currency EUR { get; set; }
    }

    public class Currency
    {
        public string rate { get; set; }
    }

}