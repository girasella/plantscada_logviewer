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
            if (!int.TryParse(e.Text, out int number))
                e.Handled = true;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(tbox.Text))
                tbox.Text = "00";

            tbox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbox = sender as ComboBox;

            cbox.SelectedIndex = -1;

            
        }

        private void DateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTimeChanged();
        }

        private void TimeChanged(object sender, TextChangedEventArgs e)
        {
            DateTimeChanged();
        }

        private void DateTimeChanged()
        {
            MainViewModel mvm = this.DataContext as MainViewModel;
            mvm.ApplyTimeRangeEnabled = true;
        }
        
    }
}
