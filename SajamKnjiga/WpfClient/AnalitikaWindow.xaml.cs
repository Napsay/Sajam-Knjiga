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
    /// Interaction logic for AnalitikaWindow.xaml
    /// </summary>
    public partial class AnalitikaWindow : Window
    {
        private List<Posetilac> _sviPosetioci;
        private List<Knjiga> _sveKnjige;
        private List<Posetilac> _rezultatObe = new List<Posetilac>();
        private List<Posetilac> _rezultatKupili = new List<Posetilac>();
        public AnalitikaWindow(List<Posetilac> posetioci, List<Knjiga> knjige)
        {
            InitializeComponent();
            _sviPosetioci = posetioci;
            _sveKnjige = knjige;

            cmbPrvaKnjiga.ItemsSource = _sveKnjige;
            cmbDrugaKnjiga.ItemsSource = _sveKnjige;
        }

        private void BtnPrikazi_Click(object sender, RoutedEventArgs e)
        {
           

            if (cmbPrvaKnjiga.SelectedItem == null || cmbDrugaKnjiga.SelectedItem == null)
            {
                MessageBox.Show(
                    "Morate izabrati obe knjige.",
                    "Upozorenje",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }


            Knjiga prvaKnjiga = (Knjiga)cmbPrvaKnjiga.SelectedItem;
            Knjiga drugaKnjiga = (Knjiga)cmbDrugaKnjiga.SelectedItem;
            if (prvaKnjiga.ISBN == drugaKnjiga.ISBN)
            {
                MessageBox.Show(
                    "Morate izabrati dve različite knjige.",
                    "Upozorenje",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            List<Posetilac> imajuObeNaListiZelja = new List<Posetilac>();
            List<Posetilac> kupiliPrvuNeDrugu = new List<Posetilac>();

            foreach (Posetilac posetilac in _sviPosetioci)
            {
                bool imaPrvuUListiZelja = false;
                bool imaDruguUListiZelja = false;

                foreach (Knjiga knjiga in posetilac.ListaZelja)
                {
                    if (knjiga.ISBN == prvaKnjiga.ISBN)
                    {
                        imaPrvuUListiZelja = true;
                    }

                    if (knjiga.ISBN == drugaKnjiga.ISBN)
                    {
                        imaDruguUListiZelja = true;
                    }
                }

                if (imaPrvuUListiZelja && imaDruguUListiZelja)
                {
                    imajuObeNaListiZelja.Add(posetilac);
                }

                bool kupioPrvu = false;
                bool kupioDrugu = false;

                foreach (Kupovina kupovina in posetilac.KupljeneKnjige)
                {
                    if (kupovina.Knjiga != null)
                    {
                        if (kupovina.Knjiga.ISBN == prvaKnjiga.ISBN)
                        {
                            kupioPrvu = true;
                        }

                        if (kupovina.Knjiga.ISBN == drugaKnjiga.ISBN)
                        {
                            kupioDrugu = true;
                        }
                    }
                }

                if (kupioPrvu && !kupioDrugu)
                {
                    kupiliPrvuNeDrugu.Add(posetilac);
                }
            }

            _rezultatObe = imajuObeNaListiZelja;
            _rezultatKupili = kupiliPrvuNeDrugu;

            dgObeNaListiZelja.ItemsSource = _rezultatObe;
            dgKupiliPrvuNeDrugu.ItemsSource = _rezultatKupili;
        }

        private void BtnPretrazi_Click(object sender, RoutedEventArgs e)
        {
            string unos = txtPretraga.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(unos))
            {
                dgObeNaListiZelja.ItemsSource = _rezultatObe;
                dgKupiliPrvuNeDrugu.ItemsSource = _rezultatKupili;
                return;
            }

            List<Posetilac> filtriraniObe = new List<Posetilac>();
            List<Posetilac> filtriraniKupili = new List<Posetilac>();

            foreach (Posetilac p in _rezultatObe)
            {
                if (p.BrClanskeKarte.ToLower().Contains(unos) ||
                    p.Ime.ToLower().Contains(unos) ||
                    p.Prezime.ToLower().Contains(unos) ||
                    p.Status.ToString().ToLower().Contains(unos))
                {
                    filtriraniObe.Add(p);
                }
            }

            foreach (Posetilac p in _rezultatKupili)
            {
                if (p.BrClanskeKarte.ToLower().Contains(unos) ||
                    p.Ime.ToLower().Contains(unos) ||
                    p.Prezime.ToLower().Contains(unos) ||
                    p.Status.ToString().ToLower().Contains(unos))
                {
                    filtriraniKupili.Add(p);
                }
            }

            dgObeNaListiZelja.ItemsSource = filtriraniObe;
            dgKupiliPrvuNeDrugu.ItemsSource = filtriraniKupili;

        }
    }
}
