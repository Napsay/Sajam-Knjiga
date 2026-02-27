using Core.DAO;
using Core.Models;
using System.Windows;
using System.Windows.Controls;
using WpfClient.Resources; 

namespace WpfClient
{
    public partial class IzdavacDetaljiWindow : Window
    {
        private Izdavac _izdavac;
        private readonly IzdavacDao _izdavacDao;

        public IzdavacDetaljiWindow(Izdavac izdavac, IzdavacDao izdavacDao)
        {
            InitializeComponent();

            _izdavac = izdavac;
            _izdavacDao = izdavacDao;

            dgAutori.ItemsSource = _izdavac.SpisakAutora;
            PrikaziSefa();
        }

        private void BtnDodajSefa_Click(object sender, RoutedEventArgs e)
        {
            Autor autor = dgAutori.SelectedItem as Autor;
            if (autor == null)
            {
                MessageBox.Show(
                    Strings.Msg_SelectAuthorFirst,
                    Strings.Msg_WarningTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (autor.GodineIskustva < 5)
            {
                MessageBox.Show(
                    Strings.Msg_ChiefMinExperience,
                    Strings.Msg_WarningTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            _izdavac.Sef = autor;
            _izdavacDao.SaveAll();

            PrikaziSefa();

            MessageBox.Show(
                Strings.Msg_ChiefSetSuccess,
                Strings.Msg_OK,
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void BtnUkloniSefa_Click(object sender, RoutedEventArgs e)
        {
            if (_izdavac.Sef == null)
            {
                MessageBox.Show(
                    Strings.Msg_NoChiefToRemove,
                    Strings.Msg_WarningTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            _izdavac.Sef = null;
            _izdavacDao.SaveAll();

            PrikaziSefa();

            MessageBox.Show(
                Strings.Msg_ChiefRemovedSuccess,
                Strings.Msg_OK,
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void BtnZatvori_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PrikaziSefa()
        {
            if (_izdavac.Sef == null)
                txtSef.Text = Strings.Izdavac_NoChief;
            else
                txtSef.Text = $"{_izdavac.Sef.Ime} {_izdavac.Sef.Prezime}";
        }
    }
}