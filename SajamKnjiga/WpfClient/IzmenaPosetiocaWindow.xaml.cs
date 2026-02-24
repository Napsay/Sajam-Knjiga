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
using Core.Models;
using Core.Utils;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for IzmenaPosetiocaWindow.xaml
    /// </summary>
    public partial class IzmenaPosetiocaWindow : Window
    {
        private Posetilac _posetilac;
        private List<Kupovina> sveKupovine = new List<Kupovina>();
        private KnjigaDao _knjigaDao;
        private List<Knjiga> _sveKnjige;

        public IzmenaPosetiocaWindow(Posetilac posetilac, List<Knjiga> sveKnjige)
        {

            InitializeComponent();
            _posetilac = posetilac;
            AdresaDao adresaDao = new AdresaDao();
            IzdavacDao izdavacDao = new IzdavacDao(adresaDao);
            _sveKnjige = sveKnjige;
            UcitajKupovine();
            UcitajPodatke();
            UcitajListuZelja();
        }
        private void UcitajKupovine()
        {
            if (_posetilac == null)
                return;

            dgKupljeneKnjige.ItemsSource = _posetilac.KupljeneKnjige;
            IzracunajStatistiku();
            Console.WriteLine("Broj kupovina: " + _posetilac.KupljeneKnjige.Count);
        }

        private void UcitajListuZelja()
        {
            if (_posetilac == null)
                return;

            dgListaZelja.ItemsSource = _posetilac.ListaZelja;
        }
        private void UcitajPodatke()
        {
            
            txtBrClanskeKarte.Text = _posetilac.BrClanskeKarte;
            txtIme.Text = _posetilac.Ime;
            txtPrezime.Text = _posetilac.Prezime;
            dpDatumRodjenja.SelectedDate = _posetilac.DatumRodjenja;
            txtTelefon.Text = _posetilac.Telefon;
            txtEmail.Text = _posetilac.Email;

            cmbStatus.SelectedIndex = (int)_posetilac.Status;


            if (_posetilac.Adresa != null)
            {
                txtUlica.Text = _posetilac.Adresa.Ulica;
                txtBroj.Text = _posetilac.Adresa.Broj.ToString();
                txtGrad.Text = _posetilac.Adresa.Grad;
                txtDrzava.Text = _posetilac.Adresa.Drzava;
            }

        }
        private void IzracunajStatistiku()
        {
            
            if (_posetilac == null)
            {
                txtProsecnaOcena.Text = "Prosečna ocena: 0";
                txtUkupnaPotrosnja.Text = "Potrošeno: 0 RSD";
                return;
            }
            List<Kupovina> kupovine = _posetilac.KupljeneKnjige;

            
            if (kupovine == null || kupovine.Count == 0)
            {
                txtProsecnaOcena.Text = "Prosečna ocena: 0";
                txtUkupnaPotrosnja.Text = "Potrošeno: 0 RSD";
                return;
            }

            double zbirOcena = 0;
            double ukupnaPotrosnja = 0;
            int brojKupovina = 0;

            
            foreach (Kupovina kupovina in kupovine)
            { 
                zbirOcena += kupovina.Ocena;
                brojKupovina++;

                
                if (kupovina.Knjiga != null)
                {
                    ukupnaPotrosnja += kupovina.Knjiga.Cena;
                }
            }
            double prosecnaOcena = zbirOcena / brojKupovina;
            txtProsecnaOcena.Text = "Prosečna ocena: " + prosecnaOcena.ToString("F2");
            txtUkupnaPotrosnja.Text = "Potrošeno: " + ukupnaPotrosnja.ToString("F2") + " RSD";
        }

        private void BtnPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                _posetilac.Ime = txtIme.Text;
                _posetilac.Prezime = txtPrezime.Text;
                _posetilac.DatumRodjenja = dpDatumRodjenja.SelectedDate.Value;
                _posetilac.Telefon = txtTelefon.Text;
                _posetilac.Email = txtEmail.Text;

                _posetilac.Status = cmbStatus.SelectedIndex == 1
                    ? StatusPosetioca.B
                    : StatusPosetioca.P;

                if (_posetilac.Adresa == null)
                    _posetilac.Adresa = new Adresa();

                _posetilac.Adresa.Ulica = txtUlica.Text;
                _posetilac.Adresa.Broj = int.Parse(txtBroj.Text);
                _posetilac.Adresa.Grad = txtGrad.Text;
                _posetilac.Adresa.Drzava = txtDrzava.Text;

                AdresaDao adresaDao = new AdresaDao();
                PosetilacDao posetilacDao = new PosetilacDao(adresaDao);

                adresaDao.Update(_posetilac.Adresa);
                posetilacDao.UpdatePosetilac(_posetilac);

                DialogResult = true;
            }
            catch
            {
                MessageBox.Show("Greška pri izmeni podataka!");
            }
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnPonistiKupovinu_Click(object sender, RoutedEventArgs e)
        {
            if (dgKupljeneKnjige.SelectedItem == null)
            {
                MessageBox.Show("Morate označiti knjigu iz tabele kupljenih knjiga.");
                return;
            }

            Kupovina selektovanaKupovina = (Kupovina)dgKupljeneKnjige.SelectedItem;

            MessageBoxResult rezultat = MessageBox.Show(
                "Da li ste sigurni da želite da poništite kupovinu?",
                "Poništi kupovinu",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (rezultat != MessageBoxResult.Yes)
                return;

            _posetilac.KupljeneKnjige.Remove(selektovanaKupovina);

            if (selektovanaKupovina.Knjiga != null)
            {
                _posetilac.ListaZelja.Add(selektovanaKupovina.Knjiga);
            }

            dgKupljeneKnjige.ItemsSource = null;
            dgKupljeneKnjige.ItemsSource = _posetilac.KupljeneKnjige;

            dgListaZelja.ItemsSource = null;
            dgListaZelja.ItemsSource = _posetilac.ListaZelja;

            IzracunajStatistiku();
        }

        private void BtnDodajZelju_Click(object sender, RoutedEventArgs e)
        {
            List<Knjiga> dostupneKnjige = new List<Knjiga>();

            foreach (Knjiga knjiga in _sveKnjige)
            {
                bool uListiZelja = false;
                bool uKupljenim = false;

                foreach (Knjiga k in _posetilac.ListaZelja)
                {
                    if (k.ISBN == knjiga.ISBN)
                    {
                        uListiZelja = true;
                        break;
                    }
                }

                foreach (Kupovina kupovina in _posetilac.KupljeneKnjige)
                {
                    if (kupovina.Knjiga != null &&
                        kupovina.Knjiga.ISBN == knjiga.ISBN)
                    {
                        uKupljenim = true;
                        break;
                    }
                }

                if (!uListiZelja && !uKupljenim)
                {
                    dostupneKnjige.Add(knjiga);
                }
            }

            DodavanjeKnjigeWindow dialog = new DodavanjeKnjigeWindow(dostupneKnjige);
            dialog.Owner = this;

            if (dialog.ShowDialog() == true && dialog.IzabranaKnjiga != null)
            {
                _posetilac.ListaZelja.Add(dialog.IzabranaKnjiga);
                dgListaZelja.ItemsSource = null;
                dgListaZelja.ItemsSource = _posetilac.ListaZelja;
            }

        }

        private void BtnObrisiZelju_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnKupovinaIzZelje_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
