using MaterialDesignThemes.Wpf;
using RecipesApp.Client;
using System.Windows;
using System.Windows.Controls;

namespace RecipesApp.Pages.Connection
{
    /// <summary>
    /// Interaction logic for SignupPage.xaml
    /// </summary>
    public partial class SignupPage : Page
    {
        private MainWindow? mainWindow = null;

        public SignupPage() => InitializeComponent();
        private void Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void OpenLoginPage_Click(object sender, RoutedEventArgs e) => (Window.GetWindow(this) as Login)!.Transistor.SelectedIndex = 0;

        private void Signup_Click(object sender, RoutedEventArgs e)
        {
            try // try to connect to the server
            {
                if (!Communicator.socket.Connected)
                {
                    Communicator.socket.Connect(Communicator.hostEndPoint);
                }
            }
            catch
            {
                Dispatcher.BeginInvoke(() =>
                {
                    MyDialogHost.ShowDialog(new StackPanel()
                    {
                        Children =
                            {
                                new TextBlock()
                                {
                                    Text = "The Server is offline, try again later",
                                    Margin = new Thickness(15)
                                },
                                CreateButton("Ok")
                            }
                    });
                });

                return;
            }

            ServerMsg serverMsg = Communicator.Signup(Username.Text, Email.Text, Password.Password);

            if (serverMsg.status == 30)
            {
                mainWindow = new MainWindow(Username.Text);

                Window.GetWindow(this).Close(); // close the Login window
                mainWindow?.Show();
            }
            else
            {
                if (serverMsg.errorMsg == "General error! only god knows what happend")
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        MyDialogHost.ShowDialog(new StackPanel()
                        {
                            Children =
                            {
                                new TextBlock()
                                {
                                    Text = "SERVER SHUTDOWN",
                                    Margin = new Thickness(15)
                                },
                                CreateButton("Confirm")
                            }
                        });
                    });
                }
                else
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        MyDialogHost.ShowDialog(new StackPanel()
                        {
                            Children =
                            {
                                new TextBlock()
                                {
                                    Text = serverMsg.errorMsg,
                                    Margin = new Thickness(15)
                                },
                                CreateButton("Confirm")
                            }
                        });
                    });
                }
            }
        }

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
        #endregion

    }
}
