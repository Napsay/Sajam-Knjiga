using Core.DAO;
using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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
                MessageBox.Show(
                LocalizationManager.GetString("Msg_DataEntry"),
                LocalizationManager.GetString("Msg_WarningTitle"),
                MessageBoxButton.OK,
                MessageBoxImage.Warning
                );
            }
        }
        private void Provera(object sender, EventArgs e)
        {
            btnPotvrdi.IsEnabled = ValidirajFormu();
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
            if (SadrziBroj(txtIme.Text))
                return false;
            if (SadrziBroj(txtPrezime.Text))
                return false;
            if (SadrziBroj(txtGrad.Text))
                return false;
            if (!int.TryParse(txtGodineIskustva.Text, out int godine) || godine < 0)
                return false;
            if (!int.TryParse(txtBroj.Text, out int broj) || broj <= 0)
                return false;
            if (!ValidanEmail(txtEmail.Text))
                return false;

            return true;
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}