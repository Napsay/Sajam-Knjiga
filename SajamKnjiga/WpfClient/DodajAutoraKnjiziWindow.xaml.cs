using Core.Models;
using Core.Utils;
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
using System.Windows.Shapes;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for DodajAutoraKnjiziWindow.xaml
    /// </summary>
    public partial class DodajAutoraKnjiziWindow : Window
    {
        public Autor SelectedAutor { get; private set; }

        public DodajAutoraKnjiziWindow(List<Autor> dostupniAutori)
        {
            InitializeComponent();
            lstAutori.ItemsSource = dostupniAutori;
        }

        private void BtnPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            SelectedAutor = lstAutori.SelectedItem as Autor;
            if (SelectedAutor != null)
                DialogResult = true; 
            else
                MessageBox.Show(
                LocalizationManager.GetString("Msg_SelectAuthor"),
                LocalizationManager.GetString("Msg_WarningTitle"),
                MessageBoxButton.OK,
                MessageBoxImage.Warning
                );
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
