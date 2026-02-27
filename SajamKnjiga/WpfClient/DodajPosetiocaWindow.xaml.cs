using Core.DAO;
using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Text.RegularExpressions;
using System.Net.Mail;

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
            
            if (string.IsNullOrWhiteSpace(txtBrClanskeKarte.Text))
               return false;
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
            if (cmbStatus.SelectedItem == null)
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

            if (!int.TryParse(txtBroj.Text, out _))
                return false;

            if (!ValidanEmail(txtEmail.Text))
                return false;

            return true;
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
