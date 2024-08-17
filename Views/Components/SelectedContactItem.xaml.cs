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
    /// Interaction logic for SelectedContactItem.xaml
    /// </summary>
    public partial class SelectedContactItem : UserControl
    {
        public SelectedContactItem()
        {
            InitializeComponent();
            image.ImageSource = new BitmapImage(new Uri("https://as1.ftcdn.net/v2/jpg/03/02/88/46/1000_F_302884605_actpipOdPOQHDTnFtp4zg4RtlWzhOASp.jpg"));
        }
    }
}
