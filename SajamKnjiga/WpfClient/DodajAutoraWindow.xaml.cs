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

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}