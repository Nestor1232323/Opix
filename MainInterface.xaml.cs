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


        // Обработчик кнопки "Назад"
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (WebView.CoreWebView2.CanGoBack)
            {
                WebView.CoreWebView2.GoBack();
            }
        }

        // Обработчик кнопки "Вперед"
        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (WebView.CoreWebView2.CanGoForward)
            {
                WebView.CoreWebView2.GoForward();
            }
        }

        // Обработчик кнопки "Обновить"
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            WebView.CoreWebView2.Reload();
        }

        // Обработчик кнопки "Настройки" (если требуется)
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для перехода на страницу настроек, если требуется
            Frame.Navigate(typeof(SettingsPage));
        }

        // Обработчик нажатия клавиши Enter в TextBox
        // Обработчик нажатия клавиши Enter в TextBox
        private void UrlTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                string input = UrlTextBox.Text.Trim();
                NavigateToUrl(input);  // Ваш метод для перехода по URL или выполнения поиска
            }
        }


        private async void InitializeWebView()
        {
            var webView2Environment = await CoreWebView2Environment.CreateAsync();
            await WebView.EnsureCoreWebView2Async(webView2Environment);

            // Загружаем домашнюю страницу при старте
            string? homePage = ApplicationData.Current.LocalSettings.Values["HomePage"]?.ToString();
            if (!string.IsNullOrEmpty(homePage))
            {
                WebView.CoreWebView2.Navigate(homePage);
            }
            else
            {
                WebView.CoreWebView2.Navigate("https://www.google.com"); // Значение по умолчанию
            }
        }

        // Загрузка настроек
        private void LoadSettings()
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            // Загружаем поисковик
            string? searchEngine = localSettings.Values["SearchEngine"]?.ToString();
            if (!string.IsNullOrEmpty(searchEngine))
            {
                // Сохраняем выбранный поисковик (если нужно)
            }
        }



        private void NavigateToUrl(string input)
        {
            // Если это URL, загружаем его
            if (Uri.IsWellFormedUriString(input, UriKind.Absolute))
            {
                if (!input.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                    !input.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    input = "https://" + input; // Добавляем https, если нет
                }

                WebView.CoreWebView2.Navigate(input);
            }
            else
            {
                // Иначе выполняем поиск через выбранный поисковик
                // Инициализация localSettings
                var localSettings = ApplicationData.Current.LocalSettings;
                string searchEngine = localSettings.Values["SearchEngine"]?.ToString() ?? "google"; // Если значение null, по умолчанию будет 'google'
                string searchUrl = searchEngine switch
                {
                    "Yandex" => "https://yandex.com/search/?text=",
                    "Bing" => "https://www.bing.com/search?q=",
                    "DuckDuckGo" => "https://duckduckgo.com/?q=",
                    _ => "https://www.google.com/search?q=", // По умолчанию Google
                };

                WebView.CoreWebView2.Navigate(searchUrl + Uri.EscapeDataString(input));
            }
        }
    }
}
