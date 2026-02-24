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
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BtnPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtOcena.Text, out int ocena) || ocena < 1 || ocena > 5)
            {
                MessageBox.Show("Ocena mora biti broj od 1 do 5!");
                return;
            }
            if (dpDatum.SelectedDate == null)
            {
                MessageBox.Show("Morate uneti datum kupovine!");
                return;
            }

            Ocena = ocena;
            DatumKupovine = dpDatum.SelectedDate.Value;

            DialogResult = true;
        }
    }
}
