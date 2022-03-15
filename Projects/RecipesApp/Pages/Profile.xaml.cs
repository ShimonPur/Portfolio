using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace RecipesApp.Pages
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        private static readonly Lazy<Profile> instance = new(() => new Profile());

        public Profile()
        {
            InitializeComponent();
        }

        public static Profile Instance
        {
            get
            {
                return instance.Value;
            }
        }

        #region Image Upload
        private void SelectUpload_Click(object sender, RoutedEventArgs e)
        {
            //(Window.GetWindow(this) as MainWindow)!.Menu.SelectedIndex = 2; //upload Menu item
        }
        #endregion
    }
}
