using ContactBook.Views.Components;
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

namespace ContactBook.Views
{
    /// <summary>
    /// Interaction logic for ItemContactView.xaml
    /// </summary>
    public partial class ContactListView : UserControl
    {
        public ContactListView()
        {
            InitializeComponent();
        }

        private void ItemClick(object sender, MouseButtonEventArgs e)
        {
            var list = sender as ListView;
            if (list.SelectedItem != null)
            {
                
                
                var item = list.SelectedItem as ListContactItem;
                
            }
        }
    }
}
