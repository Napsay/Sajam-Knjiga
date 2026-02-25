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
    /// Interaction logic for AutoriKnjigeListaZeljaWindow.xaml
    /// </summary>
    public partial class AutoriKnjigeListaZeljaWindow : Window
    {
        private List<Autor> sviAutori; 

        public AutoriKnjigeListaZeljaWindow(List<Autor> autori)
        {
            InitializeComponent();

            sviAutori = autori;
            dgAutori.ItemsSource = null;
            dgAutori.ItemsSource = sviAutori;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Pretrazi_Click(object sender, RoutedEventArgs e)
        {
            PretraziAutore();
        }

        private void PretraziAutore()
        {
            string unos = txtPretraga.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(unos))
            {
                dgAutori.ItemsSource = sviAutori;
                return;
            }

            string[] delovi = unos
                .Split(',')
                .Select(x => x.Trim())
                .Where(x => x != "")
                .ToArray();

            List<Autor> rezultat;

            if (delovi.Length == 1)
            {
                rezultat = sviAutori
                    .Where(a => a.Prezime.ToLower().Contains(delovi[0]))
                    .ToList();
            }
            else
            {
                rezultat = sviAutori
                    .Where(a =>
                        a.Prezime.ToLower().Contains(delovi[0]) &&
                        a.Ime.ToLower().Contains(delovi[1]))
                    .ToList();
            }

            dgAutori.ItemsSource = rezultat;
        }

    }
}
