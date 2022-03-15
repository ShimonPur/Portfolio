using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ServerGUI.Controls
{
    /// <summary>
    /// Interaction logic for LogBox.xaml
    /// </summary>
    public partial class LogBox : UserControl
    {
        public LogBox()
        {
            InitializeComponent();
        }

        public void AddLog(string logMessage)
        {
            _ = LogMessages.Children.Add(new TextBlock()
            {
                Margin = new Thickness(5, 2, 5, 0),
                Text = $"[{GetCurrentDateAndTime()}] {logMessage}",
                Foreground = new BrushConverter().ConvertFrom("#e0e0e0") as Brush,
                FontFamily = new FontFamily("CaskaydiaCove Nerd Font"),
                FontSize = 15
            });
        }

        public void ClearLogs() => LogMessages.Children.Clear();

        public static string GetCurrentDateAndTime() => DateTime.Now.ToString(new CultureInfo("de-DE"));
    }
}
