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

namespace RecipesApp.Controls
{
    /// <summary>
    /// Interaction logic for FeedCard.xaml
    /// </summary>
    public partial class FeedCard : UserControl
    {
        public ImageSource ImagePath
        {
            get => RecepieImage.ImageSource;

            set => RecepieImage.ImageSource = value;
        }

        public string Username 
        {
            get => UsernameTextBlock.Text;
            set => UsernameTextBlock.Text = value; 
        }

        public string Caption 
        { 
            get => CaptionTextBlock.Text; 
            set => CaptionTextBlock.Text = value; 
        }
        public FeedCard()
        {
            InitializeComponent();
        }
    }
}
