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
using WpfClient.Resources;
using WpfClient.Resources;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for IzmenaKnjigeWindow.xaml
    /// </summary>
    public partial class IzmenaKnjigeWindow : Window
    {
        private Knjiga _knjiga;
        private List<Autor> _sviAutori;
        private List<Izdavac> _sviIzdavaci;
        public IzmenaKnjigeWindow(Knjiga knjiga)
        {
            InitializeComponent();
            _knjiga = knjiga;

            UcitajPodatke();
        }
        private void UcitajPodatke()
        {
            var adresaDao = new AdresaDao();
            var izdavacDao = new IzdavacDao(adresaDao);
            var autorDao = new AutorDao(adresaDao);

            _sviAutori = autorDao.GetAll();
            _sviIzdavaci = izdavacDao.GetAll();

            cmbIzdavac.ItemsSource = _sviIzdavaci;
            lstAutori.ItemsSource = _knjiga.Autori;
            txtISBN.Text = _knjiga.ISBN;
            txtNaziv.Text = _knjiga.Naziv;
            txtZanr.Text = _knjiga.Zanr;
            txtGodina.Text = _knjiga.GodinaIzdanja.ToString();
            txtCena.Text = _knjiga.Cena.ToString();
            txtStrana.Text = _knjiga.BrojStrana.ToString();

            cmbIzdavac.SelectedItem = _sviIzdavaci
                .FirstOrDefault(i => i.Sifra == _knjiga.Izdavac.Sifra);

            foreach (var autor in _knjiga.Autori)
            {
                var match = _sviAutori.FirstOrDefault(a => a.AutorID == autor.AutorID);
                if (match != null)
                    lstAutori.SelectedItems.Add(match);
            }
        }

        private void BtnPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                _knjiga.Naziv = txtNaziv.Text;
                _knjiga.Zanr = txtZanr.Text;
                _knjiga.GodinaIzdanja = int.Parse(txtGodina.Text);
                _knjiga.Cena = int.Parse(txtCena.Text);
                _knjiga.BrojStrana = int.Parse(txtStrana.Text);
                _knjiga.Izdavac = (Izdavac)cmbIzdavac.SelectedItem;
                var adresaDao = new AdresaDao();
                var izdavacDao = new IzdavacDao(adresaDao);
                var knjigaDao = new KnjigaDao(izdavacDao);
                var autorKnjigaDao = new AutorKnjigaDao();
                knjigaDao.updateKnjiga(_knjiga);
                autorKnjigaDao.RemoveByISBN(_knjiga.ISBN);
                foreach (Autor a in lstAutori.Items)
                {
                    autorKnjigaDao.AddVeza(a.AutorID, _knjiga.ISBN);
                }

                DialogResult = true;
            }
            catch
            {
                MessageBox.Show(
                    Strings.Msg_EditBookError,
                    Strings.Msg_ErrorTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                            }
        }
        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            var dostupniAutori = _sviAutori
                .Where(a => !_knjiga.Autori.Any(ka => ka.AutorID == a.AutorID))
                .ToList();

            
            if (dostupniAutori.Count == 0)
            {
                MessageBox.Show(
                Strings.Msg_AllAuthorsAlreadyAdded,
                Strings.Msg_WarningTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Information);
                return;
            }

            var prozor = new DodajAutoraKnjiziWindow(dostupniAutori)
            {
                Owner = this 
            };

            if (prozor.ShowDialog() == true)
            {
                var noviAutor = prozor.SelectedAutor;
                if (noviAutor != null)
                {
                    _knjiga.Autori.Add(noviAutor);
                    lstAutori.Items.Refresh(); 

                    var autorKnjigaDao = new AutorKnjigaDao();
                    autorKnjigaDao.AddVeza(noviAutor.AutorID, _knjiga.ISBN);
                }
            }
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (lstAutori.SelectedItem == null)
            {
                MessageBox.Show(
                    Strings.Msg_SelectAuthorForRemoval,
                    Strings.Msg_WarningTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var autorZaBrisanje = (Autor)lstAutori.SelectedItem;


            var poruka = string.Format(
            Strings.Msg_RemoveAuthorFromBookConfirm,
            autorZaBrisanje.Ime,
            autorZaBrisanje.Prezime);

            var potvrda = new PotvrdaBrisanjaWindow(
                poruka,
                Strings.Msg_RemoveAuthorTitle
            )
            {
                Owner = this
            };
            
            if (potvrda.ShowDialog() == true)
            {
             
                _knjiga.Autori.Remove(autorZaBrisanje);
                lstAutori.Items.Refresh();

                var autorKnjigaDao = new AutorKnjigaDao();
                autorKnjigaDao.RemoveVeza(autorZaBrisanje.AutorID, _knjiga.ISBN);
            }
        }
    }
}
