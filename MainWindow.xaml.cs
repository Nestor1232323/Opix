using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.Web.WebView2.Core;
using System;
using Microsoft.UI.Windowing; // ��� AppWindow
using Windows.UI; // ��� ������������� ������
using WinRT.Interop;
using Microsoft.UI.Xaml.Media;
using WinRT;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Composition;
using Opix;

namespace Opix
{

    public sealed partial class MainWindow : Window
    {
        private MicaController? micaController; // ��������� NULL
        private SystemBackdropConfiguration? backdropConfiguration; // ��������� NULL

        public MainWindow()
        {
            this.InitializeComponent();
            TrySetMicaBackdrop();
            CustomizeTitleBar();
            StartWelcomeAnimation();
            MainFrame.Navigate(typeof(MainInterface));
        }

        private void TrySetMicaBackdrop()
        {
            if (MicaController.IsSupported()) // ��������� ��������� Mica (Windows 11+)
            {
                backdropConfiguration = new SystemBackdropConfiguration
                {
                    IsInputActive = true,
                    Theme = SystemBackdropTheme.Default
                };

                micaController = new MicaController();

                // ��������� �������� �� ����� ����
                micaController.AddSystemBackdropTarget(this.As<ICompositionSupportsSystemBackdrop>());
                micaController.SetSystemBackdropConfiguration(backdropConfiguration);
            }
        }

        private void CustomizeTitleBar()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

            if (appWindow != null)
            {
                var titleBar = appWindow.TitleBar;
                titleBar.ExtendsContentIntoTitleBar = true; // ������� ����������� ���������

                // ������ ��� ������ ���������� ����������
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                titleBar.ButtonHoverBackgroundColor = Colors.Transparent;
                titleBar.ButtonPressedBackgroundColor = Colors.Transparent;

                // ������ ������ ������ ������
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonInactiveForegroundColor = Colors.Gray;

                // �������� ������ ����������
                titleBar.IconShowOptions = IconShowOptions.HideIconAndSystemMenu;
            }

            // ������ ��������� ���������������
            this.SetTitleBar(TitleBarGrid);
        }

        private void StartWelcomeAnimation()
        {
            // �������� ��������� �������� (Opacity)
            var fadeInLogo = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };

            // �������� �������� �������� (TranslateY)
            var moveInLogo = new DoubleAnimation
            {
                From = 50,  // ��������� ������� (���� ������)
                To = 0,       // �������� ������� (�����)
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            // �������� ��������� ������ (Opacity) � ���������
            var fadeInText = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                BeginTime = TimeSpan.FromSeconds(0.5), // �������� � 1.5 ���
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };

            // �������� ��������� WelcomeText2 � ���������
            var fadeInText2 = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                BeginTime = TimeSpan.FromSeconds(0.5), // �������� � 1 ������� ����� ������
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };
            var fadeInButton = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                BeginTime = TimeSpan.FromSeconds(0.5), // �������� � 1 ������� ����� ������
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };

            // ������ ��������
            var storyboard = new Storyboard();
            storyboard.Children.Add(fadeInLogo);
            storyboard.Children.Add(moveInLogo);
            storyboard.Children.Add(fadeInText); // ��������� �����
            storyboard.Children.Add(fadeInButton); // ��������� �����
            storyboard.Children.Add(fadeInText2); // ��������� WelcomeText2

            // ����������� �������� � ��������
            Storyboard.SetTarget(fadeInLogo, WelcomeImage);
            Storyboard.SetTargetProperty(fadeInLogo, "Opacity");

            Storyboard.SetTarget(moveInLogo, ImageTransform);
            Storyboard.SetTargetProperty(moveInLogo, "Y");

            Storyboard.SetTarget(fadeInText, WelcomeText);
            Storyboard.SetTargetProperty(fadeInText, "Opacity");

            Storyboard.SetTarget(fadeInText2, WelcomeText2); // ����������� �������� � WelcomeText2
            Storyboard.SetTargetProperty(fadeInText2, "Opacity");

            Storyboard.SetTarget(fadeInButton, WelcomeButton); // ����������� �������� � WelcomeText2
            Storyboard.SetTargetProperty(fadeInButton, "Opacity");

            // ������ ��������
            storyboard.Begin();
        }

        private void WelcomeButton_Click(object sender, RoutedEventArgs e)
        {
            // �������� ��� ��������
            WelcomeButton.Visibility = Visibility.Collapsed;
            WelcomeText.Visibility = Visibility.Collapsed;
            WelcomeText2.Visibility = Visibility.Collapsed;
            WelcomeImage.Visibility = Visibility.Collapsed;

            // ���������� Frame � MainInterface
            MainFrame.Visibility = Visibility.Visible;

            // ������� � MainInterface
            MainFrame.Navigate(typeof(MainInterface));
        }
    }
}