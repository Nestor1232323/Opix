using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

namespace Opix
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();

            // Загрузка текущих настроек
            LoadSettings();
        }

        // Загрузка текущих настроек из ApplicationData
        private void LoadSettings()
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            // Загружаем домашнюю страницу
            if (localSettings.Values.ContainsKey("HomePage"))
            {
                HomePageTextBox.Text = localSettings.Values["HomePage"].ToString();
            }

            // Загружаем поисковик
            if (localSettings.Values.ContainsKey("SearchEngine"))
            {
                object searchEngineValue = localSettings.Values["SearchEngine"];
                string searchEngine = localSettings.Values["SearchEngine"]?.ToString() ?? "google"; // Добавлена проверка на null
                foreach (ComboBoxItem item in SearchEngineComboBox.Items)
                {
                    if (item.Content.ToString() == searchEngine)
                    {
                        SearchEngineComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        // Сохранение настроек
        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            // Сохранение домашней страницы
            localSettings.Values["HomePage"] = HomePageTextBox.Text;

            // Сохранение выбранного поисковика
            var selectedSearchEngine = (ComboBoxItem)SearchEngineComboBox.SelectedItem;
            if (selectedSearchEngine != null)
            {
                localSettings.Values["SearchEngine"] = selectedSearchEngine.Content.ToString();
            }

            // После сохранения возвращаемся на главную страницу
            Frame.GoBack();
        }
    }
}
