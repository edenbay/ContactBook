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
        public ListContactItem SelectedContact
        {
            get { return (ListContactItem)GetValue(SelectedContactProperty); }
            set { SetValue(SelectedContactProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedContactProperty =
            DependencyProperty.Register("SelectedContact", typeof(ListContactItem), typeof(ContactListView), new PropertyMetadata(null));


        public ContactListView()
        {
            InitializeComponent();
        }

        public void ItemClick(object sender, MouseButtonEventArgs e)
        {
            //PreviewMouseLeftButtonUp = "ItemClick"
            var list = sender as ListView;
            if (list.SelectedItem != null)
            {
                var item = list.SelectedItem as ListContactItem;
                SelectedContact = item;  
            }
        }


    }
}
