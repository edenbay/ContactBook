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

namespace ContactBook.Views.Components
{
    /// <summary>
    /// Interaction logic for SearchBar.xaml
    /// </summary>
    public partial class SearchBar : UserControl
    {



        private Visibility _showPreview
        {
            get { return (Visibility)GetValue(_showPreviewProperty); }
            set { SetValue(_showPreviewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for _showPreview.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _showPreviewProperty =
            DependencyProperty.Register("_showPreview", typeof(Visibility), typeof(SearchBar), new PropertyMetadata(Visibility.Visible));

        public SearchBar()
        {
            InitializeComponent();
        }

        private void SearchStart(object sender, MouseButtonEventArgs e)
        {

        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            _showPreview = (searchBox.Text.Length > 0) ? Visibility.Hidden : Visibility.Visible;
        }

        private void textBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void SearchText(object sender, KeyEventArgs e)
        {


            if (e.Key == Key.Enter)
            {
               
            }
        }
    }
}
