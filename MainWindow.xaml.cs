using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.Web.WebView2.Core;
using System;
using Microsoft.UI.Windowing; // Для AppWindow
using Windows.UI; // Для использования цветов
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
        private MicaController? micaController; // Разрешаем NULL
        private SystemBackdropConfiguration? backdropConfiguration; // Разрешаем NULL

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
            if (MicaController.IsSupported()) // Проверяем поддержку Mica (Windows 11+)
            {
                backdropConfiguration = new SystemBackdropConfiguration
                {
                    IsInputActive = true,
                    Theme = SystemBackdropTheme.Default
                };

                micaController = new MicaController();

                // Добавляем размытие ко всему окну
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
                titleBar.ExtendsContentIntoTitleBar = true; // Убираем стандартный заголовок

                // Делаем фон кнопок управления прозрачным
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                titleBar.ButtonHoverBackgroundColor = Colors.Transparent;
                titleBar.ButtonPressedBackgroundColor = Colors.Transparent;

                // Делаем иконки кнопок белыми
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonInactiveForegroundColor = Colors.Gray;

                // Скрываем иконку приложения
                titleBar.IconShowOptions = IconShowOptions.HideIconAndSystemMenu;
            }

            // Делаем заголовок перетаскиваемым
            this.SetTitleBar(TitleBarGrid);
        }

        private void StartWelcomeAnimation()
        {
            // Анимация появления ЛОГОТИПА (Opacity)
            var fadeInLogo = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };

            // Анимация движения ЛОГОТИПА (TranslateY)
            var moveInLogo = new DoubleAnimation
            {
                From = 50,  // Начальная позиция (выше экрана)
                To = 0,       // Конечная позиция (центр)
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            // Анимация появления ТЕКСТА (Opacity) с задержкой
            var fadeInText = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                BeginTime = TimeSpan.FromSeconds(0.5), // Задержка в 1.5 сек
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };

            // Анимация появления WelcomeText2 с задержкой
            var fadeInText2 = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                BeginTime = TimeSpan.FromSeconds(0.5), // Задержка в 1 секунда после текста
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };
            var fadeInButton = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                BeginTime = TimeSpan.FromSeconds(0.5), // Задержка в 1 секунда после текста
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };

            // Создаём анимацию
            var storyboard = new Storyboard();
            storyboard.Children.Add(fadeInLogo);
            storyboard.Children.Add(moveInLogo);
            storyboard.Children.Add(fadeInText); // Добавляем текст
            storyboard.Children.Add(fadeInButton); // Добавляем текст
            storyboard.Children.Add(fadeInText2); // Добавляем WelcomeText2

            // Привязываем анимации к объектам
            Storyboard.SetTarget(fadeInLogo, WelcomeImage);
            Storyboard.SetTargetProperty(fadeInLogo, "Opacity");

            Storyboard.SetTarget(moveInLogo, ImageTransform);
            Storyboard.SetTargetProperty(moveInLogo, "Y");

            Storyboard.SetTarget(fadeInText, WelcomeText);
            Storyboard.SetTargetProperty(fadeInText, "Opacity");

            Storyboard.SetTarget(fadeInText2, WelcomeText2); // Привязываем анимацию к WelcomeText2
            Storyboard.SetTargetProperty(fadeInText2, "Opacity");

            Storyboard.SetTarget(fadeInButton, WelcomeButton); // Привязываем анимацию к WelcomeText2
            Storyboard.SetTargetProperty(fadeInButton, "Opacity");

            // Запуск анимации
            storyboard.Begin();
        }

        private void WelcomeButton_Click(object sender, RoutedEventArgs e)
        {
            // Скрываем все элементы
            WelcomeButton.Visibility = Visibility.Collapsed;
            WelcomeText.Visibility = Visibility.Collapsed;
            WelcomeText2.Visibility = Visibility.Collapsed;
            WelcomeImage.Visibility = Visibility.Collapsed;

            // Показываем Frame с MainInterface
            MainFrame.Visibility = Visibility.Visible;

            // Переход к MainInterface
            MainFrame.Navigate(typeof(MainInterface));
        }
    }
}