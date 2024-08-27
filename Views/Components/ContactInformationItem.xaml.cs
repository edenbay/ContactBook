using MahApps.Metro.IconPacks;
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
    /// Interaction logic for ContactInformationItem.xaml
    /// </summary>
    public partial class ContactInformationItem : UserControl
    {

        public PackIconModernKind Icon
        {
            get { return (PackIconModernKind)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(PackIconModernKind), typeof(ContactInformationItem), new PropertyMetadata(default));



        public new string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static new readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(string), typeof(ContactInformationItem), new PropertyMetadata(string.Empty));



        public ContactInformationItem()
        {
            InitializeComponent();
        }
    }
}
