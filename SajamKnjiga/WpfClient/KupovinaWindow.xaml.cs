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
using WpfClient.Resources;


namespace WpfClient
{
    /// <summary>
    /// Interaction logic for KupovinaWindow.xaml
    /// </summary>
    public partial class KupovinaWindow : Window
    {
        public int Ocena { get; private set; }
        public DateTime DatumKupovine { get; private set; }
        public KupovinaWindow(Knjiga knjiga)
        {
            InitializeComponent();
            txtISBN.Text = knjiga.ISBN;
            txtNaziv.Text = knjiga.Naziv;
            UpdateUI();
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BtnPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtOcena.Text, out int ocena) || ocena < 1 || ocena > 5)
            {
                MessageBox.Show(
                LocalizationManager.GetString("Messg_Ocena"),        
                LocalizationManager.GetString("Msg_ErrorTitle"),      
                MessageBoxButton.OK,
                MessageBoxImage.Error
                );
                return;
            }
            if (dpDatum.SelectedDate == null)
            {
                MessageBox.Show(
                LocalizationManager.GetString("Msg_DatumKupovina"),     
                LocalizationManager.GetString("Msg_ErrorTitle"),  
                MessageBoxButton.OK,
                MessageBoxImage.Error
                );
                return;
            }

            Ocena = ocena;
            DatumKupovine = dpDatum.SelectedDate.Value;

            DialogResult = true;
        }

        private void UpdateUI()
        {
            Title = LocalizationManager.GetString("KupovinaKnjige_Title");

            Knjiga_ISBN.Text = LocalizationManager.GetString("Knjiga_ISBN");
            Knjiga_Naziv.Text = LocalizationManager.GetString("Knjiga_Naziv");
            Kupovina_Ocena.Text = LocalizationManager.GetString("Kupovina_Ocena");
            Kupovina_Datum.Text = LocalizationManager.GetString("Kupovina_Datum");

            Btn_Confirm.Content = LocalizationManager.GetString("Btn_Confirm");
            Btn_Cancel.Content = LocalizationManager.GetString("Btn_Cancel");
        }
    }
}
