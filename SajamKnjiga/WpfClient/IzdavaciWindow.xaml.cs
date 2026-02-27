using Core.DAO;
using Core.Models;
using System.Windows;
using WpfClient.Resources; 

namespace WpfClient
{
    public partial class IzdavaciWindow : Window
    {
        private IzdavacDao _izdavacDao;

        public IzdavaciWindow()
        {
            InitializeComponent();
            _izdavacDao = new IzdavacDao(new AdresaDao());
            UcitajIzdavace();
        }

        private void UcitajIzdavace()
        {
            dgIzdavaci.ItemsSource = null;
            dgIzdavaci.ItemsSource = _izdavacDao.GetAll();
        }

        private void BtnZatvori_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnDetalji_Click(object sender, RoutedEventArgs e)
        {
            Izdavac selected = dgIzdavaci.SelectedItem as Izdavac;
            if (selected == null)
            {
                MessageBox.Show(
                   Strings.Msg_SelectPublisherFirst,
                   Strings.Msg_WarningTitle,
                   MessageBoxButton.OK,
                   MessageBoxImage.Warning);
                return;
            }

            IzdavacDetaljiWindow detalji =
                 new IzdavacDetaljiWindow(selected, _izdavacDao);

            detalji.ShowDialog();
            UcitajIzdavace();
        }
    }
}