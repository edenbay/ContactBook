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
using ContactBook.Commands;
using MahApps.Metro.IconPacks;
using Microsoft.Xaml.Behaviors;

namespace ContactBook.Views.Components
{
    /// <summary>
    /// Interaction logic for IconButton.xaml
    /// </summary>
    public partial class IconButton : UserControl
    {

        public PackIconUniconsKind IconKind
        {
            get { return (PackIconUniconsKind)GetValue(IconKindProperty); }
            set { SetValue(IconKindProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconKind.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconKindProperty =
            DependencyProperty.Register("IconKind", typeof(PackIconUniconsKind), typeof(IconButton), new PropertyMetadata(default));

        public VerticalAlignment IconVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(IconVerticalAlignmentProperty); }
            set { SetValue(IconVerticalAlignmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconVerticalAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconVerticalAlignmentProperty =
            DependencyProperty.Register("IconVerticalAlignment", typeof(VerticalAlignment), typeof(IconButton), new PropertyMetadata(VerticalAlignment.Center));



        public Thickness IconPadding
        {
            get { return (Thickness)GetValue(IconPaddingProperty); }
            set { SetValue(IconPaddingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconPadding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconPaddingProperty =
            DependencyProperty.Register("IconPadding", typeof(Thickness), typeof(IconButton), new PropertyMetadata(new Thickness(3)));



        public Color ActionColor
        {
            get { return (Color)GetValue(ActionColorProperty); }
            set { SetValue(ActionColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActionColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionColorProperty =
            DependencyProperty.Register("ActionColor", typeof(Color), typeof(IconButton), new PropertyMetadata(Colors.DimGray));



        public Thickness IconMargin
        {
            get { return (Thickness)GetValue(IconMarginProperty); }
            set { SetValue(IconMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconMarginProperty =
            DependencyProperty.Register("IconMargin", typeof(Thickness), typeof(IconButton), new PropertyMetadata(new Thickness(0)));

        public CornerRadius BorderRadius
        {
            get { return (CornerRadius)GetValue(BorderRadiusProperty); }
            set { SetValue(BorderRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderRadiusProperty =
            DependencyProperty.Register("BorderRadius", typeof(CornerRadius), typeof(IconButton), new PropertyMetadata(new CornerRadius(0)));


        public IconButton()
        {
            InitializeComponent();
        }

    }
}
