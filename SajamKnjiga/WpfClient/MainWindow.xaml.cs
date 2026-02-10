using Core.DAO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            UcitajPosetioce();
            UcitajAutore();
            UcitajKnjige();
            PokreniSat();
            OsveziStatusBar();

        }

        private void PokreniSat()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                txtDateTime.Text = DateTime.Now.ToString("HH:mm  dd.MM.yyyy");
            };
            timer.Start();
        }

        

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OsveziStatusBar();
        }

        private void OsveziStatusBar()
        {
            if (MainTabControl.SelectedItem is TabItem tab)
            {
                txtStatusLevo.Text = $"Sajam knjiga - {tab.Header}";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void UcitajPosetioce()
        {
            
            AdresaDao adrese = new AdresaDao();
            AutorDao autori = new AutorDao(adrese);
            PosetilacDao posetioci = new PosetilacDao(adrese);
            IzdavacDao izdavaci = new IzdavacDao(adrese);
            KnjigaDao knjige = new KnjigaDao(izdavaci);
            AutorKnjigaDao autorKnjiga = new AutorKnjigaDao();
            ListaZeljaDao listaZelja = new ListaZeljaDao();
            KupovinaDao kupovine = new KupovinaDao();

           
            var listaAdresa = adrese.GetAll();
            var listaAutora = autori.GetAll();
            var listaPosetilaca = posetioci.GetAllPosetilac();
            var listaKnjiga = knjige.getAllKnjige();
            var listaKupovina = kupovine.getAllKupovine();
            var listaListaZelja = listaZelja.GetAll();
            var listaIzdavaca = izdavaci.GetAll();
            var vezeAutorKnjiga = autorKnjiga.GetAll();

            DataBinder.PoveziSve(
                listaAutora,
                listaKnjiga,
                vezeAutorKnjiga,
                listaPosetilaca,
                listaKupovina,
                listaListaZelja,
                listaIzdavaca,
                listaAdresa
            );

            dgPosetioci.ItemsSource = listaPosetilaca;
        }

        private void UcitajAutore()
        {
            AdresaDao adrese = new AdresaDao();
            AutorDao autori = new AutorDao(adrese);
            PosetilacDao posetioci = new PosetilacDao(adrese);
            IzdavacDao izdavaci = new IzdavacDao(adrese);
            KnjigaDao knjige = new KnjigaDao(izdavaci);
            AutorKnjigaDao autorKnjiga = new AutorKnjigaDao();
            ListaZeljaDao listaZelja = new ListaZeljaDao();
            KupovinaDao kupovine = new KupovinaDao();

            var listaAdresa = adrese.GetAll();
            var listaAutora = autori.GetAll();
            var listaPosetilaca = posetioci.GetAllPosetilac();
            var listaKnjiga = knjige.getAllKnjige();
            var listaKupovina = kupovine.getAllKupovine();
            var listaListaZelja = listaZelja.GetAll();
            var listaIzdavaca = izdavaci.GetAll();
            var vezeAutorKnjiga = autorKnjiga.GetAll();

            DataBinder.PoveziSve(
                listaAutora,
                listaKnjiga,
                vezeAutorKnjiga,
                listaPosetilaca,
                listaKupovina,
                listaListaZelja,
                listaIzdavaca,
                listaAdresa
            );

            dgAutori.ItemsSource = listaAutora;
        }

        private void UcitajKnjige()
        {
            AdresaDao adrese = new AdresaDao();
            AutorDao autori = new AutorDao(adrese);
            PosetilacDao posetioci = new PosetilacDao(adrese);
            IzdavacDao izdavaci = new IzdavacDao(adrese);
            KnjigaDao knjige = new KnjigaDao(izdavaci);
            AutorKnjigaDao autorKnjiga = new AutorKnjigaDao();
            ListaZeljaDao listaZelja = new ListaZeljaDao();
            KupovinaDao kupovine = new KupovinaDao();

            var listaAdresa = adrese.GetAll();
            var listaAutora = autori.GetAll();
            var listaPosetilaca = posetioci.GetAllPosetilac();
            var listaKnjiga = knjige.getAllKnjige();
            var listaKupovina = kupovine.getAllKupovine();
            var listaListaZelja = listaZelja.GetAll();
            var listaIzdavaca = izdavaci.GetAll();
            var vezeAutorKnjiga = autorKnjiga.GetAll();

            
            DataBinder.PoveziSve(
                listaAutora,
                listaKnjiga,
                vezeAutorKnjiga,
                listaPosetilaca,
                listaKupovina,
                listaListaZelja,
                listaIzdavaca,
                listaAdresa
            );

            dgKnjige.ItemsSource = null;      
            dgKnjige.ItemsSource = listaKnjiga;
        }


        private void DodajEntitet_Click(object sender, RoutedEventArgs e)
        {
            switch (MainTabControl.SelectedIndex)
            {
                case 0: 
                    DodajPosetiocaWindow posetilacWin = new DodajPosetiocaWindow();
                    posetilacWin.Owner = this;

                    if (posetilacWin.ShowDialog() == true)
                        UcitajPosetioce();
                    break;

                case 1: 
                    DodajAutoraWindow autorWin = new DodajAutoraWindow();
                    autorWin.Owner = this;

                    if (autorWin.ShowDialog() == true)
                        UcitajAutore();
                    break;

                case 2: 
                    DodajKnjiguWindow knjigaWin = new DodajKnjiguWindow();
                    knjigaWin.Owner = this;

                    if (knjigaWin.ShowDialog() == true)
                        UcitajKnjige();
                    break;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 0)
            {
                Posetilac p = (Posetilac)dgPosetioci.SelectedItem;
                if (p == null)
                {
                    MessageBox.Show("Izaberite posetioca iz tabele.");
                    return;
                }

                IzmenaPosetiocaWindow win = new IzmenaPosetiocaWindow(p);
                win.Owner = this;

                if (win.ShowDialog() == true)
                    UcitajPosetioce();
            }
            
            else if (MainTabControl.SelectedIndex == 1)
            {
                Autor a = (Autor)dgAutori.SelectedItem;
                if (a == null)
                {
                    MessageBox.Show("Izaberite autora iz tabele.");
                    return;
                }

                IzmenaAutoraWindow win = new IzmenaAutoraWindow(a);
                win.Owner = this;

                if (win.ShowDialog() == true)
                    UcitajAutore();
            }
          
            else if (MainTabControl.SelectedIndex == 2)
            {
                Knjiga k = (Knjiga)dgKnjige.SelectedItem;
                if (k == null)
                {
                    MessageBox.Show("Izaberite knjigu iz tabele.");
                    return;
                }

                IzmenaKnjigeWindow win = new IzmenaKnjigeWindow(k);
                win.Owner = this;

                if (win.ShowDialog() == true)
                    UcitajKnjige();
            }
        }
    }
}
