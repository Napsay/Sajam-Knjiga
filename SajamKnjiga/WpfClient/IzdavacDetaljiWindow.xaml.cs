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
    /// Interaction logic for IzdavacDetaljiWindow.xaml
    /// </summary>
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
                  "Molimo prvo izaberite autora.",
                  "Upozorenje",
                  MessageBoxButton.OK,
                  MessageBoxImage.Warning);
                return;
            }

            if (autor.GodineIskustva < 5)
            {
                MessageBox.Show(
                  "Sef mora imati najmanje 5 godina iskustva.",
                  "Upozorenje",
                  MessageBoxButton.OK,
                  MessageBoxImage.Warning);
                return;
            }

            _izdavac.Sef = autor;
            _izdavacDao.SaveAll();  

            PrikaziSefa();
            MessageBox.Show("Šef je uspešno postavljen.");
        }
        private void BtnUkloniSefa_Click(object sender, RoutedEventArgs e)
        {
            if (_izdavac.Sef == null)
            {
                MessageBox.Show(
                   "Izdavac nema sefa, ne moze se ukloniti.",
                   "Upozorenje",
                   MessageBoxButton.OK,
                   MessageBoxImage.Warning);
                return;
            }

            _izdavac.Sef = null;
            _izdavacDao.SaveAll();   

            PrikaziSefa();
            MessageBox.Show("Šef je uspešno uklonjen.");
        }

        private void BtnZatvori_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PrikaziSefa()
        {
            if (_izdavac.Sef == null)
                txtSef.Text = "Nema šefa";
            else
                txtSef.Text = $"{_izdavac.Sef.Ime} {_izdavac.Sef.Prezime}";
        }
    }
}
