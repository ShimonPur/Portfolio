using System;
using System.Windows;
using System.Windows.Controls;

namespace RecipesApp.Pages
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Page
    {
        private static readonly Lazy<About> instance = new(() => new About());

        public About()
        {
            InitializeComponent();
        }

        public static About Instance
        {
            get => instance.Value;
        }

        #region Open Contact Pages
        private void OpenInstagram_Click(object sender, RoutedEventArgs e) { }

        private void OpenGmail_Click(object sender, RoutedEventArgs e) { }

        private void OpenGithub_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://github.com/ShimonPur",
                UseShellExecute = true
            });
        }
        #endregion
    }
}
