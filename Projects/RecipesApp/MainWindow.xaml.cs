using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using RecipesApp.Client;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RecipesApp.Pages;
using System.Diagnostics;

namespace RecipesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static class Pages
        {
            public const int HOME = 0;
            public const int FEED = 1;
            public const int UPLOAD = 2;
            public const int PROFILE = 3;
            public const int ABOUT = 4;
        }

        public bool IsDarkTheme { get; set; } = true;
        private readonly PaletteHelper paletteHelper = new();


        private readonly string Username = "";

        public MainWindow(string username)
        {
            Username = username;
            InitializeComponent();
            Username_TextBox.Text = Username;
            Theme_Toggle.IsChecked = paletteHelper.GetTheme().GetBaseTheme() != BaseTheme.Dark;
        }

        #region Window Functionality
        private void Top_Bar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                if (WindowState == WindowState.Maximized)
                {
                    Point point = PointToScreen(e.MouseDevice.GetPosition(this));

                    if (point.X <= RestoreBounds.Width / 2)
                    {
                        Left = 0;
                    }
                    else if (point.X >= RestoreBounds.Width)
                    {
                        Left = point.X - (RestoreBounds.Width - (ActualWidth - point.X));
                    }
                    else
                    {
                        Left = point.X - (RestoreBounds.Width / 2);
                    }

                    Top = point.Y - (((FrameworkElement)sender).ActualHeight / 2);
                    WindowState = WindowState.Normal;
                }

                DragMove();
            }
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
            {
                WindowState = WindowState.Maximized;
                ButtonMaximaizeIcon.Kind = PackIconKind.WindowRestore;
            }
            else
            {
                WindowState = WindowState.Normal;
                ButtonMaximaizeIcon.Kind = PackIconKind.WindowMaximize;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Communicator.Logout(Username);
            Application.Current.Shutdown();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                BorderThickness = new Thickness(6);
            }
            else
            {
                BorderThickness = new Thickness(0);
            }
        }

        private void Theme_Toggle_Click(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();

            if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                IsDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }

            paletteHelper.SetTheme(theme);
        }

        #endregion

        #region Image Upload

        private void Upload_Image(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Title = "Select an image",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                         "Portable Network Graphic (*.png)|*.png"
            };

            if (dialog.ShowDialog() == true)
            {
                BitmapImage btpImg = new();
                btpImg.BeginInit();
                btpImg.UriSource = new Uri(dialog.FileName);
                btpImg.EndInit();

                ((Border)((Button)sender).Parent).Background = new ImageBrush()
                {
                    ImageSource = btpImg
                };

                ((Button)sender).Visibility = Visibility.Hidden;
            }
            else
            {
                MyDialogHost.ShowDialog(new StackPanel()
                {
                    Children =
                        {
                            new TextBlock()
                            {
                                Text = "Select an image",
                                Margin = new Thickness(15)
                            },
                            CreateButton("Ok")
                        }
                });
            }
        }

        #endregion

        #region Util
        private static Button CreateButton(string Text)
            {
                Button button = new()
                {
                    Style = Application.Current.FindResource("MaterialDesignFlatButton") as Style,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(15),
                    Content = new TextBlock()
                    {
                        Text = Text
                    }
                };

                button.Command = DialogHost.CloseDialogCommand;

                return button;

            }
        private static StackPanel CreateFeedCard()
            {
                return new StackPanel()
                {
                    Children =
                {
                    new Border()
                    {
                        Margin = new Thickness(0,10,0,5),
                        BorderBrush = Application.Current.FindResource("PrimaryHueMidBrush") as Brush,
                        BorderThickness = new Thickness(3),
                        CornerRadius = new CornerRadius(10),
                        Height = 400
                    },
                    new StackPanel()
                    {
                        Orientation = Orientation.Horizontal,
                        Children =
                        {
                            new RadioButton()
                            {
                                Content = new PackIcon()
                                {
                                    Kind = PackIconKind.Like,
                                    FontSize = 20
                                },
                                Width = 30,
                                Height = 30,
                                Style = Application.Current.FindResource("MaterialDesignFlatPrimaryToggleButton") as Style,
                                ToolTip = "Like",
                                Margin = new Thickness(5,0,0,0)
                            },
                            new RadioButton()
                            {
                                Content = new PackIcon()
                                {
                                    Kind = PackIconKind.Dislike,
                                    FontSize = 20
                                },
                                Width = 30,
                                Height = 30,
                                Style = Application.Current.FindResource("MaterialDesignFlatPrimaryToggleButton") as Style,
                                ToolTip = "Like",
                                Margin = new Thickness(5,0,0,0)
                            }
                        }
                    }
                }
                };
            }
        private static Page GetPageByIndex(int index) => index switch
        {
            Pages.HOME => Home.Instance,
            Pages.FEED => Feed.Instance,
            Pages.UPLOAD => Upload.Instance,
            Pages.PROFILE => Profile.Instance,
            Pages.ABOUT => About.Instance,
            _ => throw new ArgumentException("Index Out of range")
        };
        #endregion

        #region Navigate
        private void NavigationButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                NavigationPager.Content = GetPageByIndex(NavigationButtons.SelectedIndex);
            }
            catch (ArgumentException argex)
            {
                Trace.WriteLine("Only GOD knows what happend");
                Trace.WriteLine(argex.Message);
            }
            DrawerHost.IsLeftDrawerOpen = false;
        }

        private void NavigateToUpload_Click(object sender, RoutedEventArgs e)
        {
            NavigationPager.Content = Upload.Instance;
        }

        private void NavigateToProfile_Click(object sender, RoutedEventArgs e)
        {
            NavigationPager.Content = Profile.Instance; 
        }

        #endregion
    }
}
