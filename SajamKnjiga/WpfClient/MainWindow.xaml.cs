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

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UcitajPosetioce();
            UcitajAutore();
            UcitajKnjige();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void UcitajPosetioce()
        {
            // Kreiranje DAO objekata
            AdresaDao adrese = new AdresaDao();
            AutorDao autori = new AutorDao(adrese);
            PosetilacDao posetioci = new PosetilacDao(adrese);
            IzdavacDao izdavaci = new IzdavacDao(adrese);
            KnjigaDao knjige = new KnjigaDao(izdavaci);
            AutorKnjigaDao autorKnjiga = new AutorKnjigaDao();
            ListaZeljaDao listaZelja = new ListaZeljaDao();
            KupovinaDao kupovine = new KupovinaDao();

            // Učitavanje lista iz fajlova
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

            dgKnjige.ItemsSource = listaKnjiga;
        }
    }
}
