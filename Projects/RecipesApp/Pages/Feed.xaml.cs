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
    /// Interaction logic for Feed.xaml
    /// </summary>
    public partial class Feed : Page
    {
        private static readonly Lazy<Feed> instance = new(() => new Feed());

        public Feed()
        {
            InitializeComponent();
        }

        public static Feed Instance
        {
            get
            {
                return instance.Value;
            }
        }
    }
}
