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
    /// <summary>
    /// Interaction logic for IzmenaPosetiocaWindow.xaml
    /// </summary>
    public partial class IzmenaPosetiocaWindow : Window
    {
        private Posetilac _posetilac;
        public IzmenaPosetiocaWindow(Posetilac posetilac)
        {

            InitializeComponent();
            _posetilac = posetilac;

            UcitajPodatke();
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

                // DAO update
                AdresaDao adresaDao = new AdresaDao();
                PosetilacDao posetilacDao = new PosetilacDao(adresaDao);

                adresaDao.Update(_posetilac.Adresa);
                posetilacDao.UpdatePosetilac(_posetilac);

                DialogResult = true;
            }
            catch
            {
                MessageBox.Show("Greška pri izmeni podataka!");
            }
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
