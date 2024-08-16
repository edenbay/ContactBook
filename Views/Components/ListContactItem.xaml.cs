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
    /// Interaction logic for ListContactItem.xaml
    /// </summary>
    public partial class ListContactItem : UserControl
    {

        internal Visibility _initialsVisibility;

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(ListContactItem), new PropertyMetadata(null));



        public string Initials
        {
            get { return (string)GetValue(InitialsProperty); }
            set { SetValue(InitialsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Initials.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InitialsProperty =
            DependencyProperty.Register("Initials", typeof(string), typeof(ListContactItem), new PropertyMetadata("NN"));




        public string FullName
        {
            get { return (string)GetValue(FullNameProperty); }
            set { SetValue(FullNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FullName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FullNameProperty =
            DependencyProperty.Register("FullName", typeof(string), typeof(ListContactItem), new PropertyMetadata("Name Nameson"));



        public string Relationship
        {
            get { return (string)GetValue(RelationshipProperty); }
            set { SetValue(RelationshipProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Relationship.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RelationshipProperty =
            DependencyProperty.Register("Relationship", typeof(string), typeof(ListContactItem), new PropertyMetadata("Relationship"));

        public ListContactItem()
        {
            
            InitializeComponent();
        }
    }
}
