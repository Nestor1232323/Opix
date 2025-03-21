using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Microsoft.Web.WebView2.Core;
using System;
using Microsoft.UI.Xaml.Input;

namespace Opix
{
    public sealed partial class MainInterface : Page
    {
        public MainInterface()
        {
            this.InitializeComponent();
            InitializeWebView();
            LoadSettings();
        }


        // ���������� ������ "�����"
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (WebView.CoreWebView2.CanGoBack)
            {
                WebView.CoreWebView2.GoBack();
            }
        }

        // ���������� ������ "������"
        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (WebView.CoreWebView2.CanGoForward)
            {
                WebView.CoreWebView2.GoForward();
            }
        }

        // ���������� ������ "��������"
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            WebView.CoreWebView2.Reload();
        }

        // ���������� ������ "���������" (���� ���������)
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            // ������ ��� �������� �� �������� ��������, ���� ���������
            Frame.Navigate(typeof(SettingsPage));
        }

        // ���������� ������� ������� Enter � TextBox
        // ���������� ������� ������� Enter � TextBox
        private void UrlTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                string input = UrlTextBox.Text.Trim();
                NavigateToUrl(input);  // ��� ����� ��� �������� �� URL ��� ���������� ������
            }
        }


        private async void InitializeWebView()
        {
            var webView2Environment = await CoreWebView2Environment.CreateAsync();
            await WebView.EnsureCoreWebView2Async(webView2Environment);

            // ��������� �������� �������� ��� ������
            string? homePage = ApplicationData.Current.LocalSettings.Values["HomePage"]?.ToString();
            if (!string.IsNullOrEmpty(homePage))
            {
                WebView.CoreWebView2.Navigate(homePage);
            }
            else
            {
                WebView.CoreWebView2.Navigate("https://www.google.com"); // �������� �� ���������
            }
        }

        // �������� ��������
        private void LoadSettings()
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            // ��������� ���������
            string? searchEngine = localSettings.Values["SearchEngine"]?.ToString();
            if (!string.IsNullOrEmpty(searchEngine))
            {
                // ��������� ��������� ��������� (���� �����)
            }
        }



        private void NavigateToUrl(string input)
        {
            // ���� ��� URL, ��������� ���
            if (Uri.IsWellFormedUriString(input, UriKind.Absolute))
            {
                if (!input.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                    !input.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    input = "https://" + input; // ��������� https, ���� ���
                }

                WebView.CoreWebView2.Navigate(input);
            }
            else
            {
                // ����� ��������� ����� ����� ��������� ���������
                // ������������� localSettings
                var localSettings = ApplicationData.Current.LocalSettings;
                string searchEngine = localSettings.Values["SearchEngine"]?.ToString() ?? "google"; // ���� �������� null, �� ��������� ����� 'google'
                string searchUrl = searchEngine switch
                {
                    "Yandex" => "https://yandex.com/search/?text=",
                    "Bing" => "https://www.bing.com/search?q=",
                    "DuckDuckGo" => "https://duckduckgo.com/?q=",
                    _ => "https://www.google.com/search?q=", // �� ��������� Google
                };

                WebView.CoreWebView2.Navigate(searchUrl + Uri.EscapeDataString(input));
            }
        }
    }
}
