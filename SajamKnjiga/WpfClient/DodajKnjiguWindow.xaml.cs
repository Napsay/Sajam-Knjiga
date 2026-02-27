using Core.DAO;
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
    /// Interaction logic for DodajKnjiguWindow.xaml
    /// </summary>
    public partial class DodajKnjiguWindow : Window
    {
        public DodajKnjiguWindow()
        {
            InitializeComponent();
            btnPotvrdi.IsEnabled = false;

            txtISBN.TextChanged += Provera;
            txtNaziv.TextChanged += Provera;
            txtZanr.TextChanged += Provera;
            txtGodina.TextChanged += Provera;
            txtCena.TextChanged += Provera;
            txtStrana.TextChanged += Provera;
            cmbIzdavac.SelectionChanged += Provera;
            lstAutori.SelectionChanged += Provera;

            UcitajPodatke();
        }
        private void UcitajPodatke()
        {
            var adresaDao = new AdresaDao();
            var autorDao = new AutorDao(adresaDao);
            var izdavacDao = new IzdavacDao(adresaDao);

            cmbIzdavac.ItemsSource = izdavacDao.GetAll();
            lstAutori.ItemsSource = autorDao.GetAll();
        }
        private void Provera(object sender, EventArgs e)
        {
            btnPotvrdi.IsEnabled =
                !string.IsNullOrWhiteSpace(txtISBN.Text) &&
                !string.IsNullOrWhiteSpace(txtNaziv.Text) &&
                !string.IsNullOrWhiteSpace(txtZanr.Text) &&
                !string.IsNullOrWhiteSpace(txtGodina.Text) &&
                !string.IsNullOrWhiteSpace(txtCena.Text) &&
                !string.IsNullOrWhiteSpace(txtStrana.Text) &&
                cmbIzdavac.SelectedItem != null &&
                lstAutori.SelectedItems.Count > 0;
        }
        private void BtnPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Knjiga nova = new Knjiga
                {
                    ISBN = txtISBN.Text,
                    Naziv = txtNaziv.Text,
                    Zanr = txtZanr.Text,
                    GodinaIzdanja = int.Parse(txtGodina.Text),
                    Cena = int.Parse(txtCena.Text),
                    BrojStrana = int.Parse(txtStrana.Text),
                    Izdavac = (Izdavac)cmbIzdavac.SelectedItem
                };

                var adresaDao = new AdresaDao();
                var izdavacDao = new IzdavacDao(adresaDao);
                var knjigaDao = new KnjigaDao(izdavacDao);
                var autorKnjigaDao = new AutorKnjigaDao();
                knjigaDao.addKnjiga(nova);
                foreach (Autor a in lstAutori.SelectedItems)
                {
                    autorKnjigaDao.AddVeza(a.AutorID, nova.ISBN); 
                    nova.Autori.Add(a);                           
                }

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                LocalizationManager.GetString("Msg_DataEntry"),
                LocalizationManager.GetString("Msg_WarningTitle"),
                MessageBoxButton.OK,
                MessageBoxImage.Warning
                );
            }
        }


        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

