using Core.Models;
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
    /// Interaction logic for DodavanjeKnjigeWindow.xaml
    /// </summary>
    public partial class DodavanjeKnjigeWindow : Window
    {
        public Knjiga IzabranaKnjiga { get; private set; }
        public DodavanjeKnjigeWindow(List<Knjiga> dostupneKnjige)
        {
            InitializeComponent();
            lbKnjige.ItemsSource = dostupneKnjige;
            lbKnjige.DisplayMemberPath = "Naziv";
        }

        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (lbKnjige.SelectedItem == null)
            {
                MessageBox.Show("Izaberite knjigu.");
                return;
            }

            IzabranaKnjiga = (Knjiga)lbKnjige.SelectedItem;
            DialogResult = true;
            Close();

        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
