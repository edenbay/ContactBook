using ContactBook.Commands;
using ContactBook.DAL;
using ContactBook.Models;
using ContactBook.ViewModels;
using ContactBook.Views.Components;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
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
            DataContext = MainViewModel.Instance;
            
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();

        }

        private void MinimizeWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                WindowState = WindowState.Minimized;
            }
                
        }

        private void CloseWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.Close();
        }

        private void ChangeWindowSize(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (WindowState != WindowState.Maximized)
                    WindowState = WindowState.Maximized;
                else
                    WindowState = WindowState.Normal;
            }
                
        }

        private void HasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                BorderThickness = new Thickness(6);
            else
                BorderThickness = new Thickness(0);
        }


        private void InitiateWindowResize(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                ChangeWindowSize(sender, e);
            }
        }

#warning No logic has been provided.
        private void InitiateSearch(object sender, KeyEventArgs e)
        {

            if (sender is not SearchBar searchBar)
            {
                throw new Exception();
            }

            string searchText = searchBar.searchBox.Text;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                return;
            }

            searchText = searchText.Replace("\r\n", "");

            if (e.Key == Key.Enter)
            {
                MainViewModel.Instance.SearchForContact(searchText);

            }
        }
    }
}