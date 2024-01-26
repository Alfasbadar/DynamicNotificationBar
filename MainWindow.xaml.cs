using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DynamicNotificationBar;
    public partial class MainWindow : Window
    {
        private bool isNotificationVisible = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Show the notification card after a delay
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!isNotificationVisible)
            {
                // Show the notification card
                ShowNotification();
                isNotificationVisible = true;

                // Start the fade-out animation after a certain duration
                DispatcherTimer fadeOutTimer = new DispatcherTimer();
                fadeOutTimer.Interval = TimeSpan.FromSeconds(3); // Adjust the duration as needed
                fadeOutTimer.Tick += (s, _) => FadeOutNotification();
                fadeOutTimer.Start();
            }
        }

        private void ShowNotification()
        {
            notificationCard.Visibility = Visibility.Visible;

            // Start the slide-in and zoom-in animation
            DoubleAnimation animationX = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new QuadraticEase()
            };

            DoubleAnimation animationY = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new QuadraticEase()
            };

            Storyboard.SetTarget(animationX, notificationCard);
            Storyboard.SetTargetProperty(animationX, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));

            Storyboard.SetTarget(animationY, notificationCard);
            Storyboard.SetTargetProperty(animationY, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animationX);
            storyboard.Children.Add(animationY);
            storyboard.Begin();
        }

        private void FadeOutNotification()
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(1), // Adjust the duration as needed
                EasingFunction = new QuadraticEase()
            };

            fadeOutAnimation.Completed += (s, _) => notificationCard.Visibility = Visibility.Collapsed;

            Storyboard.SetTarget(fadeOutAnimation, notificationCard);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath("(UIElement.Opacity)"));

            Storyboard fadeOutStoryboard = new Storyboard();
            fadeOutStoryboard.Children.Add(fadeOutAnimation);
            fadeOutStoryboard.Begin();
        }
    }