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

namespace PlantSCADA_Logviewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MainViewModel.Instance;
        }

        private void CheckTimeInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tbox = sender as TextBox;

            string content = tbox.Text + e.Text;

            int limit = tbox.Tag.ToString() == "H" ? 23 : 59;
            
            if (content.Length > 2)
            {
                e.Handled = true;
                return;
            }

            if (int.TryParse(content, out int parsedContent))
            {
                if (parsedContent > limit)
                    e.Handled = true;
            }
            else
            {
                e.Handled = true;
            }
            


        }
    }
}
