using Core.DAO;
using Core.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfClient.Resources;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace WpfClient
{
    public partial class IzmenaAutoraWindow : Window
    {
        private Autor _autor;
        private ObservableCollection<Knjiga> _knjigeObservable;

        public IzmenaAutoraWindow(Autor autor)
        {
            InitializeComponent();
            _autor = autor;
            UcitajPodatke();
            btnPotvrdi.IsEnabled = false;

            
            txtIme.TextChanged += (s, e) => AzurirajDugmePotvrdi();
            txtPrezime.TextChanged += (s, e) => AzurirajDugmePotvrdi();
            txtTelefon.TextChanged += (s, e) => AzurirajDugmePotvrdi();
            txtEmail.TextChanged += (s, e) => AzurirajDugmePotvrdi();
            txtBrLicne.TextChanged += (s, e) => AzurirajDugmePotvrdi();
            txtGodineIskustva.TextChanged += (s, e) => AzurirajDugmePotvrdi();

            txtUlica.TextChanged += (s, e) => AzurirajDugmePotvrdi();
            txtBroj.TextChanged += (s, e) => AzurirajDugmePotvrdi();
            txtGrad.TextChanged += (s, e) => AzurirajDugmePotvrdi();
            txtDrzava.TextChanged += (s, e) => AzurirajDugmePotvrdi();

            dpDatumRodjenja.SelectedDateChanged += (s, e) => AzurirajDugmePotvrdi();

            
            AzurirajDugmePotvrdi();
        }

        private void UcitajPodatke()
        {
            txtIme.Text = _autor.Ime;
            txtPrezime.Text = _autor.Prezime;
            dpDatumRodjenja.SelectedDate = _autor.DatumRodjenja;
            txtTelefon.Text = _autor.Telefon;
            txtEmail.Text = _autor.Email;
            txtBrLicne.Text = _autor.BrojLicneKarte;
            txtGodineIskustva.Text = _autor.GodineIskustva.ToString();

            if (_autor.Adresa != null)
            {
                txtUlica.Text = _autor.Adresa.Ulica;
                txtBroj.Text = _autor.Adresa.Broj.ToString();
                txtGrad.Text = _autor.Adresa.Grad;
                txtDrzava.Text = _autor.Adresa.Drzava;
            }

            var autorKnjigaDao = new AutorKnjigaDao();
            var isbnovi = autorKnjigaDao.GetKnjigeZaAutora(_autor.AutorID);

            var adresaDao = new AdresaDao();
            var izdavacDao = new IzdavacDao(adresaDao);
            var knjigaDao = new KnjigaDao(izdavacDao);

            var knjige = knjigaDao.getAllKnjige()
                .Where(k => isbnovi.Contains(k.ISBN))
                .ToList();

            _knjigeObservable = new ObservableCollection<Knjiga>(knjige);
            dgKnjige.ItemsSource = _knjigeObservable;
        }

        private void BtnPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _autor.Ime = txtIme.Text;
                _autor.Prezime = txtPrezime.Text;
                _autor.DatumRodjenja = dpDatumRodjenja.SelectedDate.Value;
                _autor.Telefon = txtTelefon.Text;
                _autor.Email = txtEmail.Text;
                _autor.BrojLicneKarte = txtBrLicne.Text;
                _autor.GodineIskustva = int.Parse(txtGodineIskustva.Text);

                if (_autor.Adresa == null)
                    _autor.Adresa = new Adresa();

                _autor.Adresa.Ulica = txtUlica.Text;
                _autor.Adresa.Broj = int.Parse(txtBroj.Text);
                _autor.Adresa.Grad = txtGrad.Text;
                _autor.Adresa.Drzava = txtDrzava.Text;

                AdresaDao adresaDao = new AdresaDao();
                AutorDao autorDao = new AutorDao(adresaDao);

                adresaDao.Update(_autor.Adresa);
                autorDao.Update(_autor);

                DialogResult = true;
            }
            catch
            {
                MessageBox.Show(
                    Strings.Msg_EditAuthorError,
                    Strings.Msg_ErrorTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnDodajKnjigu_Click(object sender, RoutedEventArgs e)
        {
            var autorKnjigaDao = new AutorKnjigaDao();
            var knjigaDao = new KnjigaDao(new IzdavacDao(new AdresaDao()));

            var autorISBN = autorKnjigaDao
                .GetKnjigeZaAutora(_autor.AutorID);

            var dostupneKnjige = knjigaDao.getAllKnjige()
                .Where(k => !autorISBN.Contains(k.ISBN))
                .ToList();

            if (!dostupneKnjige.Any())
            {
                MessageBox.Show(
                    Strings.Msg_AuthorHasAllBooks,
                    Strings.Msg_WarningTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var dlg = new DodajKnjiguAutoruWindow(dostupneKnjige, this);

            if (dlg.ShowDialog() == true)
            {
                foreach (var k in dlg.IzabraneKnjige)
                {
                    autorKnjigaDao.AddVeza(_autor.AutorID, k.ISBN);
                    _knjigeObservable.Add(k);
                }
            }
        }

        private void BtnUkloniKnjigu_Click(object sender, RoutedEventArgs e)
        {
            if (dgKnjige.SelectedItems.Count == 0)
            {
                MessageBox.Show(
                    Strings.Msg_SelectAtLeastOneBook,
                    Strings.Msg_WarningTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var potvrda = new PotvrdaBrisanjaWindow(
                Strings.Msg_RemoveBookConfirm,
                Strings.Msg_DeleteConfirmationTitle);

            potvrda.Owner = this;
            potvrda.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            if (potvrda.ShowDialog() == true)
            {
                var autorKnjigaDao = new AutorKnjigaDao();

                var selektovaneKnjige = dgKnjige.SelectedItems
                    .Cast<Knjiga>()
                    .ToList();

                foreach (var knjiga in selektovaneKnjige)
                {
                    autorKnjigaDao.RemoveVeza(_autor.AutorID, knjiga.ISBN);
                    _knjigeObservable.Remove(knjiga);
                }
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
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool ValidirajFormu()
        {
            
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
            if (string.IsNullOrWhiteSpace(txtBrLicne.Text))
                return false;
            if (string.IsNullOrWhiteSpace(txtGodineIskustva.Text))
                return false;
            if (string.IsNullOrWhiteSpace(txtUlica.Text))
                return false;
            if (string.IsNullOrWhiteSpace(txtBroj.Text))
                return false;
            if (string.IsNullOrWhiteSpace(txtGrad.Text))
                return false;
            if (string.IsNullOrWhiteSpace(txtDrzava.Text))
                return false;
            if (SadrziBroj(txtIme.Text))
                return false;
            if (SadrziBroj(txtPrezime.Text))
                return false;
            if (SadrziBroj(txtGrad.Text))
                return false;
            if (SadrziBroj(txtDrzava.Text))
                return false;

            if (!int.TryParse(txtGodineIskustva.Text, out int godine) || godine < 0)
                return false;
            if (!int.TryParse(txtBroj.Text, out _))
                return false;
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