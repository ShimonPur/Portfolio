using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecipesApp.Pages
{
    /// <summary>
    /// Interaction logic for Upload.xaml
    /// </summary>
    public partial class Upload : Page
    {
        private static readonly Lazy<Upload> instance = new(() => new Upload());

        public Upload()
        {
            InitializeComponent();
        }

        public static Upload Instance
        {
            get
            {
                return instance.Value;
            }
        }

        #region Image uploading 
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
                (Window.GetWindow(this) as MainWindow)!.ShowDialog(new StackPanel()
                {
                    Children =
                        {
                            new TextBlock()
                            {
                                Text = "Select Image for uploading the Recepie",
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
        #endregion
    }
}
