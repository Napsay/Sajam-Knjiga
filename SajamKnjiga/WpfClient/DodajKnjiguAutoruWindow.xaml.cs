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
    /// Interaction logic for DodajKnjiguAutoruWindow.xaml
    /// </summary>
    public partial class DodajKnjiguAutoruWindow : Window

   

    {
        public List<Knjiga> IzabraneKnjige { get; private set; }
        public DodajKnjiguAutoruWindow(List<Knjiga> dostupneKnjige, Window owner)
        {
            InitializeComponent();

            Owner = owner;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            lstKnjige.ItemsSource = dostupneKnjige;
            IzabraneKnjige = new List<Knjiga>();

            btnPotvrdi.IsEnabled = false;
        }

        private void lstKnjige_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnPotvrdi.IsEnabled = lstKnjige.SelectedItems.Count > 0;
        }

        private void BtnPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            IzabraneKnjige = lstKnjige.SelectedItems.Cast<Knjiga>().ToList();
            DialogResult = true;
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
