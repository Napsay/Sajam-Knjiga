using Core.DAO;
using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for PosetiociListaZeljaAutorWindow.xaml
    /// </summary>
    public partial class PosetiociListaZeljaAutorWindow : Window
    {
        private List<Posetilac> sviPosetioci;

        public PosetiociListaZeljaAutorWindow(List<Posetilac> posetioci)
        {
            InitializeComponent();

            AdresaDao adresaDao = new AdresaDao();
            var sveAdrese = adresaDao.GetAll();

            foreach (var p in posetioci)
            {
                if (p.Adresa == null && p.AdresaID != 0)
                {
                    p.Adresa = sveAdrese.FirstOrDefault(a => a.Sifra == p.AdresaID);
                }
            }

            sviPosetioci = posetioci;
            dgPosetioci.ItemsSource = sviPosetioci;

            UpdateUI();
        }

        private void txtPretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = txtPretraga.Text.ToLower();

            var filtrirani = sviPosetioci
                .Where(p =>
                    p.Ime.ToLower().Contains(filter) ||
                    p.Prezime.ToLower().Contains(filter) ||
                    p.BrClanskeKarte.ToLower().Contains(filter) ||
                    p.Status.ToString().ToLower().Contains(filter) ||
                    (p.Adresa != null && p.Adresa.ToString().ToLower().Contains(filter))
                )
                .ToList();

            dgPosetioci.ItemsSource = filtrirani;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Pretrazi_Click(object sender, RoutedEventArgs e)
        {
            PretraziPosetioce();
        }

        private void PretraziPosetioce()
        {
            string unos = txtPretraga.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(unos))
            {
                dgPosetioci.ItemsSource = sviPosetioci;
                return;
            }

            string[] delovi = unos
                .Split(',')
                .Select(x => x.Trim())
                .Where(x => x != "")
                .ToArray();

            List<Posetilac> rezultat = new List<Posetilac>();

            if (delovi.Length == 1)
            {
                rezultat = sviPosetioci
                    .Where(p => p.Prezime.ToLower().Contains(delovi[0]))
                    .ToList();
            }
            else if (delovi.Length == 2)
            {
                rezultat = sviPosetioci
                    .Where(p =>
                        p.Prezime.ToLower().Contains(delovi[0]) &&
                        p.Ime.ToLower().Contains(delovi[1]))
                    .ToList();
            }
            else if (delovi.Length == 3)
            {
                rezultat = sviPosetioci
                    .Where(p =>
                        p.BrClanskeKarte.ToLower().Contains(delovi[0]) &&
                        p.Ime.ToLower().Contains(delovi[1]) &&
                        p.Prezime.ToLower().Contains(delovi[2]))
                    .ToList();
            }

            dgPosetioci.ItemsSource = rezultat;
        }

        private void UpdateUI()
        {
            this.Title = LocalizationManager.GetString("PosetiociListaZeljaAutorWindow_Title");

            Posetilac_ClanskaKarta.Header = LocalizationManager.GetString("Posetilac_ClanskaKarta");
            Posetilac_Ime.Header = LocalizationManager.GetString("Posetilac_Ime");
            Posetilac_Prezime.Header = LocalizationManager.GetString("Posetilac_Prezime");
            Posetilac_Adresa.Header = LocalizationManager.GetString("Posetilac_Adresa");
            Posetilac_Status.Header = LocalizationManager.GetString("Posetilac_Status");

            Btn_Close.Content = LocalizationManager.GetString("Btn_Close");
            txtPretraga.Text = LocalizationManager.GetString("Search_Placeholder");
        }
    }
}
