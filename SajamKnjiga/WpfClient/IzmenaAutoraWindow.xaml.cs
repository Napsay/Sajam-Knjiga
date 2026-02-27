using Core.DAO;
using Core.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfClient.Resources; 

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
    }
}