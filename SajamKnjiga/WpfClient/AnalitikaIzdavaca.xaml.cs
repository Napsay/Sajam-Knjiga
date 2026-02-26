using Core.DAO;
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
    /// Interaction logic for AnalitikaIzdavaca.xaml
    /// </summary>
    public partial class AnalitikaIzdavaca : Window
    {
        private IzdavacDao _izdavacDao;
        private List<Knjiga> _sveKnjige;
        private List<Izdavac> _sviIzdavaci;
        private List<Autor> _sviAutori;
        public AnalitikaIzdavaca(List<Izdavac> izdavaci, List<Knjiga> knjige, List<Autor> sviAutori)
        {
            InitializeComponent();
            _izdavacDao = new IzdavacDao(new AdresaDao());
            _sviIzdavaci = izdavaci;
            _sveKnjige = knjige;
            dgIzdavaci.ItemsSource = _sviIzdavaci;
            UcitajIzdavace();
            _sviAutori = sviAutori;
        }

        private void UcitajIzdavace()
        {
            dgIzdavaci.ItemsSource = null;
            dgIzdavaci.ItemsSource = _izdavacDao.GetAll();
        }

        private void BtnZatvori_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnDetalji_Click(object sender, RoutedEventArgs e)
        {
            Izdavac selected = dgIzdavaci.SelectedItem as Izdavac;
            if (selected == null)
            {
                MessageBox.Show(
                   "Molimo prvo izaberite izdavaca.",
                   "Upozorenje",
                   MessageBoxButton.OK,
                   MessageBoxImage.Warning);
                return;
            }

            IzdavacDetaljiWindow detalji =
                 new IzdavacDetaljiWindow(selected, _izdavacDao);

            detalji.ShowDialog();
            UcitajIzdavace();
        }

        private void BtnPrikaziKnjige_Click(object sender, RoutedEventArgs e)
        {
            if (dgIzdavaci.SelectedItem == null)
            {
                MessageBox.Show(
                    "Morate izabrati izdavača.",
                    "Upozorenje",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            Izdavac selektovaniIzdavac = (Izdavac)dgIzdavaci.SelectedItem;

            List<Knjiga> knjigeIzdavaca = new List<Knjiga>();

            foreach (Knjiga knjiga in _sveKnjige)
            {
                if (knjiga.Izdavac != null &&
                    knjiga.Izdavac.Sifra == selektovaniIzdavac.Sifra)
                {
                    knjigeIzdavaca.Add(knjiga);
                }
            }

            dgKnjigeIzdavaca.ItemsSource = null;
            dgKnjigeIzdavaca.ItemsSource = knjigeIzdavaca;
        }

        private void BtnPrikaziAutore_Click(object sender, RoutedEventArgs e)
        {
            if (dgIzdavaci.SelectedItem == null)
            {
                MessageBox.Show(
                    "Morate izabrati izdavača.",
                    "Upozorenje",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            Izdavac selektovaniIzdavac = (Izdavac)dgIzdavaci.SelectedItem;

            List<Autor> autoriIzdavaca = new List<Autor>();

            foreach (Knjiga knjiga in _sveKnjige)
            {
                if (knjiga.Izdavac != null &&
                    knjiga.Izdavac.Sifra == selektovaniIzdavac.Sifra)
                {
                    foreach (Autor autor in knjiga.Autori)
                    {
                        bool vecPostoji = false;

                        foreach (Autor a in autoriIzdavaca)
                        {
                            if (a.AutorID == autor.AutorID)
                            {
                                vecPostoji = true;
                                break;
                            }
                        }

                        if (!vecPostoji)
                        {
                            autoriIzdavaca.Add(autor);
                        }
                    }
                }
            }

            dgAutoriIzdavaca.ItemsSource = null;
            dgAutoriIzdavaca.ItemsSource = autoriIzdavaca;
        }
    }

}
