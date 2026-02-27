using Core.DAO;
using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using WpfClient.Resources; // LOKALIZACIJA

namespace WpfClient
{
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
            btnPotvrdi.IsEnabled = false;

            txtIme.TextChanged += (s, e) => AzurirajDugmePotvrdi();
            txtPrezime.TextChanged += (s, e) => AzurirajDugmePotvrdi();
            txtTelefon.TextChanged += (s, e) => AzurirajDugmePotvrdi();
            txtEmail.TextChanged += (s, e) => AzurirajDugmePotvrdi();
            txtUlica.TextChanged += (s, e) => AzurirajDugmePotvrdi();
            txtBroj.TextChanged += (s, e) => AzurirajDugmePotvrdi();
            txtGrad.TextChanged += (s, e) => AzurirajDugmePotvrdi();
            txtDrzava.TextChanged += (s, e) => AzurirajDugmePotvrdi();
            dpDatumRodjenja.SelectedDateChanged += (s, e) => AzurirajDugmePotvrdi();
            cmbStatus.SelectionChanged += (s, e) => AzurirajDugmePotvrdi();

            AzurirajDugmePotvrdi();
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
                txtProsecnaOcena.Text = Strings.Stat_ProsecnaOcenaZero;
                txtUkupnaPotrosnja.Text = Strings.Stat_UkupnaPotrosnjaZero;
                return;
            }

            List<Kupovina> kupovine = _posetilac.KupljeneKnjige;

            if (kupovine == null || kupovine.Count == 0)
            {
                txtProsecnaOcena.Text = Strings.Stat_ProsecnaOcenaZero;
                txtUkupnaPotrosnja.Text = Strings.Stat_UkupnaPotrosnjaZero;
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

            txtProsecnaOcena.Text = string.Format(Strings.Stat_ProsecnaOcena, prosecnaOcena.ToString("F2"));
            txtUkupnaPotrosnja.Text = string.Format(Strings.Stat_UkupnaPotrosnja, ukupnaPotrosnja.ToString("F2"));
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
                MessageBox.Show(
                    Strings.Msg_EditVisitorError,
                    Strings.Msg_ErrorTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
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
                MessageBox.Show(
                    Strings.Msg_SelectPurchasedBook,
                    Strings.Msg_WarningTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            Kupovina selektovanaKupovina = (Kupovina)dgKupljeneKnjige.SelectedItem;

            MessageBoxResult rezultat = MessageBox.Show(
                Strings.Msg_ConfirmCancelPurchase,
                Strings.Msg_CancelPurchaseTitle,
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

        private void BtnObrisiZelju_Click(object sender, RoutedEventArgs e)
        {
            if (dgListaZelja.SelectedItem == null)
            {
                MessageBox.Show(
                    Strings.Msg_SelectWishlistBook,
                    Strings.Msg_WarningTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            Knjiga selektovanaKnjiga = (Knjiga)dgListaZelja.SelectedItem;

            MessageBoxResult rezultat = MessageBox.Show(
                Strings.Msg_RemoveWishlistBookConfirm,
                Strings.Msg_RemoveWishlistBookTitle,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (rezultat != MessageBoxResult.Yes)
                return;

            _posetilac.ListaZelja.Remove(selektovanaKnjiga);
            dgListaZelja.ItemsSource = null;
            dgListaZelja.ItemsSource = _posetilac.ListaZelja;
        }

        private void BtnKupovinaIzZelje_Click(object sender, RoutedEventArgs e)
        {
            if (dgListaZelja.SelectedItem == null)
            {
                MessageBox.Show(
                    Strings.Msg_SelectWishlistBook,
                    Strings.Msg_WarningTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            Knjiga selektovanaKnjiga = (Knjiga)dgListaZelja.SelectedItem;
            KupovinaWindow dialog = new KupovinaWindow(selektovanaKnjiga);
            dialog.Owner = this;

            if (dialog.ShowDialog() == true)
            {
                Kupovina novaKupovina = new Kupovina(0, _posetilac, selektovanaKnjiga, dialog.DatumKupovine, dialog.Ocena, "");
                _posetilac.KupljeneKnjige.Add(novaKupovina);
                _posetilac.ListaZelja.Remove(selektovanaKnjiga);

                dgKupljeneKnjige.ItemsSource = null;
                dgKupljeneKnjige.ItemsSource = _posetilac.KupljeneKnjige;

                dgListaZelja.ItemsSource = null;
                dgListaZelja.ItemsSource = _posetilac.ListaZelja;

                IzracunajStatistiku();
            }
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

        private bool SadrziBroj(string tekst)
        {
            return Regex.IsMatch(tekst, @"\d");
        }

        private bool ValidanEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool ValidirajFormu()
        {
            // Obavezna polja
            if (string.IsNullOrWhiteSpace(txtIme.Text))
                return false;

            if (string.IsNullOrWhiteSpace(txtPrezime.Text))
                return false;

            if (dpDatumRodjenja.SelectedDate == null)
                return false;

            if (string.IsNullOrWhiteSpace(txtTelefon.Text))
                return false;

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
                return false;

            if (string.IsNullOrWhiteSpace(txtUlica.Text))
                return false;

            if (string.IsNullOrWhiteSpace(txtBroj.Text))
                return false;

            if (string.IsNullOrWhiteSpace(txtGrad.Text))
                return false;

            if (string.IsNullOrWhiteSpace(txtDrzava.Text))
                return false;

            // Ime i prezime ne smeju da sadrže brojeve
            if (SadrziBroj(txtIme.Text))
                return false;

            if (SadrziBroj(txtPrezime.Text))
                return false;

            // Grad i država ne smeju imati brojeve
            if (SadrziBroj(txtGrad.Text))
                return false;

            if (SadrziBroj(txtDrzava.Text))
                return false;

            // Broj kuće mora biti broj
            if (!int.TryParse(txtBroj.Text, out _))
                return false;

            // Email format
            if (!ValidanEmail(txtEmail.Text))
                return false;

            return true;
        }

        private void AzurirajDugmePotvrdi()
        {
            if (btnPotvrdi != null)
                btnPotvrdi.IsEnabled = ValidirajFormu();
        }
    }
}