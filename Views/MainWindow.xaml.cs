using ContactBook.DAL;
using ContactBook.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ContactBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DBRepository _dB;

        public MainWindow()
        {
            InitializeComponent();
            _dB = new DBRepository();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //var tuple = DBRepository.SwapTupleItems(1, "string");

            var name = "FirstName";
            name = _dB.ToSQLFormat(name);

            var contact = await _dB.SelectEntityAsync<Contact>(1);


            contact = await _dB.SelectEntityAsync<Contact>(2);
        }
    }
}