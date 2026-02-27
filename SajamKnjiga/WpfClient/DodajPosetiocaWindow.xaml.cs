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
    /// Interaction logic for DodajPosetiocaWindow.xaml
    /// </summary>
    public partial class DodajPosetiocaWindow : Window
    {
        public DodajPosetiocaWindow()
        {
            InitializeComponent();
            btnPotvrdi.IsEnabled = false;

            txtIme.TextChanged += Provera;
            txtPrezime.TextChanged += Provera;
            txtTelefon.TextChanged += Provera;
            txtEmail.TextChanged += Provera;
            txtUlica.TextChanged += Provera;
            txtBroj.TextChanged += Provera;
            txtGrad.TextChanged += Provera;
            txtDrzava.TextChanged += Provera;
            cmbStatus.SelectionChanged += Provera;
            dpDatumRodjenja.SelectedDateChanged += Provera;
        }

        private void Provera(object sender, EventArgs e)
        {
                 btnPotvrdi.IsEnabled =
                !string.IsNullOrWhiteSpace(txtIme.Text) &&
                !string.IsNullOrWhiteSpace(txtPrezime.Text) &&
                dpDatumRodjenja.SelectedDate != null &&
                !string.IsNullOrWhiteSpace(txtTelefon.Text) &&
                !string.IsNullOrWhiteSpace(txtEmail.Text) &&
                cmbStatus.SelectedItem != null &&
                !string.IsNullOrWhiteSpace(txtUlica.Text) &&
                !string.IsNullOrWhiteSpace(txtBroj.Text) &&
                !string.IsNullOrWhiteSpace(txtGrad.Text) &&
                !string.IsNullOrWhiteSpace(txtDrzava.Text);
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void BtnPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Adresa adresa = new Adresa();
                adresa.Ulica = txtUlica.Text;
                adresa.Broj = int.Parse(txtBroj.Text);
                adresa.Grad = txtGrad.Text;
                adresa.Drzava = txtDrzava.Text;

                StatusPosetioca status;
                if (cmbStatus.Text == "VIP")
                    status = StatusPosetioca.B;
                else
                    status = StatusPosetioca.P;

                Posetilac novi = new Posetilac();
                novi.Ime = txtIme.Text;
                novi.Prezime = txtPrezime.Text;
                novi.DatumRodjenja = dpDatumRodjenja.SelectedDate.Value;
                novi.Telefon = txtTelefon.Text;
                novi.Email = txtEmail.Text;
                novi.Adresa = adresa;
                novi.Status = status;
                novi.TrenutnaGodClanstva = DateTime.Now.Year;
                novi.ProsecnaOcenaRec = 0;

                AdresaDao adresaDao = new AdresaDao();
                PosetilacDao posetilacDao = new PosetilacDao(adresaDao);

                adresaDao.Add(adresa);
                posetilacDao.AddPosetilac(novi);

                DialogResult = true;
            }
            catch (Exception)
            {
               MessageBox.Show(
               LocalizationManager.GetString("Msg_DataEntry"),
               LocalizationManager.GetString("Msg_WarningTitle"),
               MessageBoxButton.OK,
               MessageBoxImage.Warning
               );
            }
        }


    }

}
