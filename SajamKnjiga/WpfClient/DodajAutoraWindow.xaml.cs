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
    public partial class DodajAutoraWindow : Window
    {
        public DodajAutoraWindow()
        {
            InitializeComponent();
            btnPotvrdi.IsEnabled = false; 

            txtIme.TextChanged += Provera;
            txtPrezime.TextChanged += Provera;
            dpDatumRodjenja.SelectedDateChanged += Provera;
            txtTelefon.TextChanged += Provera;
            txtEmail.TextChanged += Provera;
            txtBrLicne.TextChanged += Provera;
            txtGodineIskustva.TextChanged += Provera;
            txtUlica.TextChanged += Provera;
            txtBroj.TextChanged += Provera;
            txtGrad.TextChanged += Provera;
        }

        private void BtnPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Adresa adresa = new Adresa();
                adresa.Ulica = txtUlica.Text;
                adresa.Broj = int.Parse(txtBroj.Text);
                adresa.Grad = txtGrad.Text;
                adresa.Drzava = "Srbija";

                Autor novi = new Autor();
                novi.Ime = txtIme.Text;
                novi.Prezime = txtPrezime.Text;
                novi.DatumRodjenja = dpDatumRodjenja.SelectedDate.Value;
                novi.Telefon = txtTelefon.Text;
                novi.Email = txtEmail.Text;
                novi.BrojLicneKarte = txtBrLicne.Text;
                novi.GodineIskustva = int.Parse(txtGodineIskustva.Text);
                novi.Adresa = adresa;

                AdresaDao adresaDao = new AdresaDao();
                AutorDao autorDao = new AutorDao(adresaDao);

                adresaDao.Add(adresa);
                autorDao.Add(novi);

                DialogResult = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Greška pri unosu podataka!");
            }
        }
        private void Provera(object sender, EventArgs e)
        {
            btnPotvrdi.IsEnabled =
                !string.IsNullOrWhiteSpace(txtIme.Text) &&
                !string.IsNullOrWhiteSpace(txtPrezime.Text) &&
                dpDatumRodjenja.SelectedDate != null &&
                !string.IsNullOrWhiteSpace(txtTelefon.Text) &&
                !string.IsNullOrWhiteSpace(txtEmail.Text) &&
                !string.IsNullOrWhiteSpace(txtBrLicne.Text) &&
                !string.IsNullOrWhiteSpace(txtGodineIskustva.Text) &&
                !string.IsNullOrWhiteSpace(txtUlica.Text) &&
                !string.IsNullOrWhiteSpace(txtBroj.Text) &&
                !string.IsNullOrWhiteSpace(txtGrad.Text);
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}