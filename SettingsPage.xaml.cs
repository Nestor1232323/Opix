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

            // �������� ������� ��������
            LoadSettings();
        }

        // �������� ������� �������� �� ApplicationData
        private void LoadSettings()
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            // ��������� �������� ��������
            if (localSettings.Values.ContainsKey("HomePage"))
            {
                HomePageTextBox.Text = localSettings.Values["HomePage"].ToString();
            }

            // ��������� ���������
            if (localSettings.Values.ContainsKey("SearchEngine"))
            {
                object searchEngineValue = localSettings.Values["SearchEngine"];
                string searchEngine = localSettings.Values["SearchEngine"]?.ToString() ?? "google"; // ��������� �������� �� null
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

        // ���������� ��������
        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            // ���������� �������� ��������
            localSettings.Values["HomePage"] = HomePageTextBox.Text;

            // ���������� ���������� ����������
            var selectedSearchEngine = (ComboBoxItem)SearchEngineComboBox.SelectedItem;
            if (selectedSearchEngine != null)
            {
                localSettings.Values["SearchEngine"] = selectedSearchEngine.Content.ToString();
            }

            // ����� ���������� ������������ �� ������� ��������
            Frame.GoBack();
        }
    }
}
