using MaterialDesignThemes.Wpf;
using ServerCS;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ServerGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Server server = Server.Instance;
        private bool running = false;
        public MainWindow()
        {
            InitializeComponent();
            server.GUI = true;
            server.LogMessage += OnLog;
        }
        private void OnLog(object sender, LogMessageEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if(e.LogMessage is not null)
                    LogBoxServer.AddLog(e.LogMessage);
            });
        }

        #region Window Functionality

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

        private void Top_Bar_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                ButtonMaximaizeIcon.Kind = PackIconKind.WindowRestore;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                ButtonMaximaizeIcon.Kind = PackIconKind.WindowMaximize;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            ExitButton_Click(sender, e);  
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }


        #endregion

        #region Click Events
        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            if(!running)
            {
                Thread thread = new(() => server.Run());
                thread.Start();
                running = true;
            }
            else
            {
                MyDialogHost.ShowDialog(new StackPanel()
                {
                    Children =
                    {
                        new TextBlock()
                        {
                            Text = "Server already running",
                            Margin = new Thickness(15)
                        },
                        CreateButton("OK", (object sender, RoutedEventArgs args) => { }),
                    }
                });
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            if(running)
            {
                    MyDialogHost.ShowDialog(new StackPanel()
                    {
                        Children =
                        {
                            new TextBlock()
                            {
                                Text = "Server will Shutdown",
                                Margin = new Thickness(15)
                            },
                            new StackPanel()
                            {
                                Orientation = Orientation.Horizontal,
                                Children =
                                {
                                    CreateButton("Confirm", (object sender, RoutedEventArgs args) =>
                                    {
                                        server.Close = true;
                                    }),
                                    CreateButton("Cancel", (object sender, RoutedEventArgs args) =>
                                    {
                                        server.Close = false;
                                    })
                                }
                            }
                        }
                    });
            }
            else
            {
                MyDialogHost.ShowDialog(new StackPanel()
                {
                    Children =
                    {
                        CreateButton("Goddbye!", (object sender, RoutedEventArgs args) =>
                        {
                            Application.Current.Shutdown();
                        })
                    }
                });           
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e) => LogBoxServer.ClearLogs();
        #endregion

        #region Util
        private static Button CreateButton(string Text, RoutedEventHandler handler)
        {
            Button button = new()
            {
                Style = Application.Current.FindResource("MaterialDesignFlatButton") as Style,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(15),
                Content = new TextBlock()
                {
                    Text = Text
                },
                Command = DialogHost.CloseDialogCommand
            };

            button.Click += handler;

            return button;
        }
        #endregion
    }
}
