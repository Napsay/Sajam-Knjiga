using Core.DAO;
using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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


        private List<Posetilac> sviPosetioci = new List<Posetilac>();
        private List<Autor> sviAutori;
        private List<Knjiga> sveKnjige = new List<Knjiga>();
        private AutorKnjigaDao autorKnjigaDao;
        private ListaZeljaDao listaZeljaDao;
        private PosetilacDao posetilacDao;

        private List<Izdavac> listaIzdavaca = new List<Izdavac>();
        private List<Knjiga> _listaKnjiga;


        public MainWindow()
        {
            InitializeComponent();
            UcitajKnjige();
            UcitajPosetioce();
            UcitajAutore();
            
            PokreniSat();
            OsveziStatusBar();
            CommandBindings.Add(new CommandBinding(
            ApplicationCommands.New,
            (s, e) => OtvoriDodavanje()));
            CommandBindings.Add(
            new CommandBinding(
                ApplicationCommands.Delete,
                ObrisiEntitet_Click));


            CommandBindings.Add(new CommandBinding(
            NavigationCommands.Refresh,
            (s, e) => OtvoriIzmenu()));


        }

        private void OtvoriIzmenu()
        {
            if (MainTabControl.SelectedItem is TabItem tab)
            {
                if (tab.Header.ToString() == "Posetioci")
                {
                    if (dgPosetioci.SelectedItem is Posetilac selektovani)
                    {
                        var win = new IzmenaPosetiocaWindow(selektovani,_listaKnjiga);
                        win.Owner = this;
                        win.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Morate izabrati posetioca za izmenu.");
                    }
                }
                else if (tab.Header.ToString() == "Autori")
                {
                    if (dgAutori.SelectedItem is Autor selektovani)
                    {
                        var win = new IzmenaAutoraWindow(selektovani);
                        win.Owner = this;
                        win.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Morate izabrati posetioca za izmenu.");
                    }
                }
                else if (tab.Header.ToString() == "Knjige")
                {
                    if (dgKnjige.SelectedItem is Knjiga selektovani)
                    {
                        var win = new IzmenaKnjigeWindow(selektovani);
                        win.Owner = this;
                        win.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Morate izabrati posetioca za izmenu.");
                    }
                }
            }
        }

        private void OtvoriDodavanje()
        {
            if (MainTabControl.SelectedItem is TabItem tab)
            {
                if (tab.Header.ToString() == "Posetioci")
                {
                    var win = new DodajPosetiocaWindow();
                    win.Owner = this;
                    win.ShowDialog();
                }
                else if (tab.Header.ToString() == "Autori")
                {
                    var win = new DodajAutoraWindow();
                    win.Owner = this;
                    win.ShowDialog();
                }
                else if (tab.Header.ToString() == "Knjige")
                {
                    var win = new DodajKnjiguWindow();
                    win.Owner = this;
                    win.ShowDialog();
                }
            }
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
            autorKnjigaDao = new AutorKnjigaDao();
            listaZeljaDao = new ListaZeljaDao();
            posetilacDao = new PosetilacDao(adrese);

            var listaAdresa = adrese.GetAll();
            var listaAutora = autori.GetAll();
            var listaPosetilaca = posetioci.GetAllPosetilac();
            var listaKnjiga = knjige.getAllKnjige();
            var listaKupovina = kupovine.getAllKupovine();
            var listaListaZelja = listaZelja.GetAll();
            var listaIzdavaca = izdavaci.GetAll();
            var vezeAutorKnjiga = autorKnjiga.GetAll();
            _listaKnjiga = listaKnjiga;

            

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

            sviPosetioci = listaPosetilaca;
            dgPosetioci.ItemsSource = sviPosetioci;
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

            sviAutori = listaAutora;
            dgAutori.ItemsSource = sviAutori;
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
            var listaPosetilacaLocal = posetioci.GetAllPosetilac();
            var listaKupovina = kupovine.getAllKupovine();
            var listaListaZelja = listaZelja.GetAll();
            var listaKnjigaLocal = knjige.getAllKnjige();
            var listaIzdavacaLocal = izdavaci.GetAll();
            var vezeAutorKnjiga = autorKnjiga.GetAll();
            sveKnjige = listaKnjigaLocal;
            listaIzdavaca = listaIzdavacaLocal;
            sviPosetioci = listaPosetilacaLocal;

            DataBinder.PoveziSve(
                listaAutora,
                listaKnjigaLocal,
                vezeAutorKnjiga,
                listaPosetilacaLocal,
                listaKupovina,
                listaListaZelja,
                listaIzdavacaLocal,
                listaAdresa
            );

            dgKnjige.ItemsSource = sveKnjige;
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
                    MessageBox.Show(
                   "Molimo izaberite posetioca iz tabele.",
                   "Upozorenje",
                   MessageBoxButton.OK,
                   MessageBoxImage.Warning);
                    return;
                }

                IzmenaPosetiocaWindow win = new IzmenaPosetiocaWindow(p,_listaKnjiga);
                win.Owner = this;

                if (win.ShowDialog() == true)
                    UcitajPosetioce();
            }
            
            else if (MainTabControl.SelectedIndex == 1)
            {
                Autor a = (Autor)dgAutori.SelectedItem;
                if (a == null)
                {
                    MessageBox.Show(
                   "Molimo izaberite autora iz tabele.",
                   "Upozorenje",
                   MessageBoxButton.OK,
                   MessageBoxImage.Warning);
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
                    MessageBox.Show(
                   "Molimo izaberite knjigu iz tabele.",
                   "Upozorenje",
                   MessageBoxButton.OK,
                   MessageBoxImage.Warning);
                    return;
                }

                IzmenaKnjigeWindow win = new IzmenaKnjigeWindow(k);
                win.Owner = this;

                if (win.ShowDialog() == true)
                    UcitajKnjige();
            }
        }

        private void ObrisiEntitet_Click(object sender, RoutedEventArgs e)
        {
            switch (MainTabControl.SelectedIndex)
            {
                case 0:
                    ObrisiPosetioca();
                    break;

                case 1:
                    ObrisiAutora();
                    break;

                case 2:
                    ObrisiKnjigu();
                    break;
            }
        }

        private void ObrisiPosetioca()
        {
            Posetilac p = dgPosetioci.SelectedItem as Posetilac;

            if (p == null)
            {
                MessageBox.Show(
                    "Molimo izaberite posetioca iz tabele.",
                    "Upozorenje",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            PotvrdaBrisanjaWindow dlg = new PotvrdaBrisanjaWindow(
                "Da li ste sigurni da želite da obrišete posetioca?",
                "Brisanje posetioca");

            dlg.Owner = this;

            if (dlg.ShowDialog() == true)
            {
                PosetilacDao dao = new PosetilacDao(new AdresaDao());
                dao.DeletePosetilac(p.BrClanskeKarte);
                UcitajPosetioce();
            }
        }


        private void ObrisiAutora()
        {
            Autor autor = dgAutori.SelectedItem as Autor;
            if (autor == null)
            {
                MessageBox.Show(
                    "Molimo izaberite autora iz tabele.",
                    "Upozorenje",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            PotvrdaBrisanjaWindow dlg = new PotvrdaBrisanjaWindow(
                "Da li ste sigurni da želite da obrišete autora?",
                "Brisanje autora");
            dlg.Owner = this;

            if (dlg.ShowDialog() == true)
            {
                AdresaDao adresaDao = new AdresaDao();
                AutorDao autorDao = new AutorDao(adresaDao);

                autorDao.Delete(autor.AutorID);
                UcitajAutore();
            }
        }



        private void ObrisiKnjigu()
        {
            Knjiga knjiga = dgKnjige.SelectedItem as Knjiga;
            if (knjiga == null)
            {
                MessageBox.Show(
                    "Molimo izaberite knjigu iz tabele.",
                    "Upozorenje",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            PotvrdaBrisanjaWindow dlg = new PotvrdaBrisanjaWindow(
                "Da li ste sigurni da želite da obrišete knjigu?",
                "Brisanje knjige");

            dlg.Owner = this;

            if (dlg.ShowDialog() == true)
            {
                AdresaDao adresaDao = new AdresaDao();
                IzdavacDao izdavacDao = new IzdavacDao(adresaDao);
                KnjigaDao knjigaDao = new KnjigaDao(izdavacDao);

                knjigaDao.deleteKnjiga(knjiga.ISBN);
                UcitajKnjige();
            }
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            var izbor = new IzborNovogEntitetaWindow();
            izbor.Owner = this;

            if (izbor.ShowDialog() == true)
            {
                
                UcitajPosetioce();
                UcitajAutore();
                UcitajKnjige();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                AdresaDao adresaDao = new AdresaDao();
                PosetilacDao posetilacDao = new PosetilacDao(adresaDao);
                AutorDao autorDao = new AutorDao(adresaDao);
                IzdavacDao izdavacDao = new IzdavacDao(adresaDao);
                KnjigaDao knjigaDao = new KnjigaDao(izdavacDao);
                AutorKnjigaDao autorKnjigaDao = new AutorKnjigaDao();
                ListaZeljaDao listaZeljaDao = new ListaZeljaDao();
                KupovinaDao kupovinaDao = new KupovinaDao();

                
                adresaDao.GetAll();           
                posetilacDao.GetAllPosetilac();
                autorDao.GetAll();
                izdavacDao.SaveAll();        
                knjigaDao.getAllKnjige();
                autorKnjigaDao.GetAll();
                listaZeljaDao.GetAll();
                kupovinaDao.getAllKupovine();

                MessageBox.Show("Svi podaci su uspešno sačuvani!", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri čuvanju podataka: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenPosetioci_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 0; 
            OsveziStatusBar();
        }

        private void OpenKnjige_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 2; 
            OsveziStatusBar();
        }

        private void OpenAutori_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 1; 
            OsveziStatusBar();
        }

        private void OpenIzdavaci_Click(object sender, RoutedEventArgs e)
        {
            if (sveKnjige == null || listaIzdavaca == null)
            {
                MessageBox.Show("Podaci nisu učitani!");
                return;
            }

            var win = new IzdavaciWindow(listaIzdavaca, sveKnjige,sviAutori);
            win.Owner = this;
            win.ShowDialog();
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow win = new AboutWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        private void Pretrazi_Click(object sender, RoutedEventArgs e)
        {
            switch (MainTabControl.SelectedIndex)
            {
                case 0:
                    PretraziPosetioce();
                    break;

                case 1:
                    PretraziAutore();
                    break;

                case 2:
                    PretraziKnjige();
                    break;
            }
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

        private void PretraziKnjige()
        {
            string unos = txtPretraga.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(unos))
            {
                dgKnjige.ItemsSource = sveKnjige;
                return;
            }

            List<Knjiga> rezultat = sveKnjige
                .Where(k =>
                    (!string.IsNullOrEmpty(k.Naziv) &&
                        k.Naziv.ToLower().Contains(unos)) ||

                    (!string.IsNullOrEmpty(k.ISBN) &&
                        k.ISBN.ToLower().Contains(unos)) ||

                    (!string.IsNullOrEmpty(k.Zanr) &&
                        k.Zanr.ToLower().Contains(unos)) ||

                    (k.Izdavac != null &&
                     !string.IsNullOrEmpty(k.Izdavac.Naziv) &&
                        k.Izdavac.Naziv.ToLower().Contains(unos)) ||

                    (!string.IsNullOrEmpty(k.AutoriPrikaz) &&
                        k.AutoriPrikaz.ToLower().Contains(unos))
                )
                .ToList();

            dgKnjige.ItemsSource = rezultat;
        }

        private void dgPosetioci_Sorting(object sender, DataGridSortingEventArgs e)
        {
            var list = dgPosetioci.ItemsSource.Cast<Posetilac>().ToList();

            
            ListSortDirection direction = (e.Column.SortDirection != ListSortDirection.Ascending) ?
                                          ListSortDirection.Ascending :
                                          ListSortDirection.Descending;

            e.Handled = true;

            switch (e.Column.Header.ToString())
            {
                case "Broj članske karte":
                    if (direction == ListSortDirection.Ascending)
                        dgPosetioci.ItemsSource = list
                            .OrderBy(p => p.GodinaClanskeKarte)
                            .ThenBy(p => p.RedniBroj)
                            .ToList();
                    else
                        dgPosetioci.ItemsSource = list
                            .OrderByDescending(p => p.GodinaClanskeKarte)
                            .ThenByDescending(p => p.RedniBroj)
                            .ToList();
                    break;

                case "Ime":
                    if (direction == ListSortDirection.Ascending)
                        dgPosetioci.ItemsSource = list.OrderBy(p => p.Ime).ToList();
                    else
                        dgPosetioci.ItemsSource = list.OrderByDescending(p => p.Ime).ToList();
                    break;

                case "Prezime":
                    if (direction == ListSortDirection.Ascending)
                        dgPosetioci.ItemsSource = list.OrderBy(p => p.Prezime).ToList();
                    else
                        dgPosetioci.ItemsSource = list.OrderByDescending(p => p.Prezime).ToList();
                    break;

                case "Status":
                    if (direction == ListSortDirection.Ascending)
                        dgPosetioci.ItemsSource = list.OrderBy(p => p.Status).ToList();
                    else
                        dgPosetioci.ItemsSource = list.OrderByDescending(p => p.Status).ToList();
                    break;

                   
            }

            e.Column.SortDirection = direction;
        }

        private void dgAutori_Sorting(object sender, DataGridSortingEventArgs e)
        {
            var list = dgAutori.ItemsSource.Cast<Autor>().ToList();
            ListSortDirection direction = (e.Column.SortDirection != ListSortDirection.Ascending) ?
                                          ListSortDirection.Ascending :
                                          ListSortDirection.Descending;

            e.Handled = true; 

            switch (e.Column.Header.ToString())
            {
                case "Ime":
                    if (direction == ListSortDirection.Ascending)
                        dgAutori.ItemsSource = list.OrderBy(a => a.Ime).ToList();
                    else
                        dgAutori.ItemsSource = list.OrderByDescending(a => a.Ime).ToList();
                    break;

                case "Prezime":
                    if (direction == ListSortDirection.Ascending)
                        dgAutori.ItemsSource = list.OrderBy(a => a.Prezime).ToList();
                    else
                        dgAutori.ItemsSource = list.OrderByDescending(a => a.Prezime).ToList();
                    break;

                case "Email":
                    if (direction == ListSortDirection.Ascending)
                        dgAutori.ItemsSource = list.OrderBy(a => a.Email[0]).ToList();
                    else
                        dgAutori.ItemsSource = list.OrderByDescending(a => a.Email[0]).ToList();
                    break;

            }

            e.Column.SortDirection = direction; 
        }

        private void dgKnjige_Sorting(object sender, DataGridSortingEventArgs e)
        {
            var list = dgKnjige.ItemsSource.Cast<Knjiga>().ToList();
            ListSortDirection direction = (e.Column.SortDirection != ListSortDirection.Ascending) ?
                                          ListSortDirection.Ascending :
                                          ListSortDirection.Descending;

            e.Handled = true;

            switch (e.Column.Header.ToString())
            {
                case "Naziv":
                    if (direction == ListSortDirection.Ascending)
                        dgKnjige.ItemsSource = list.OrderBy(k => k.Naziv).ToList();
                    else
                        dgKnjige.ItemsSource = list.OrderByDescending(k => k.Naziv).ToList();
                    break;

                case "Cena":
                    if (direction == ListSortDirection.Ascending)
                        dgKnjige.ItemsSource = list.OrderBy(k => k.Cena).ToList();
                    else
                        dgKnjige.ItemsSource = list.OrderByDescending(k => k.Cena).ToList();
                    break;

                case "Godina":
                    if (direction == ListSortDirection.Ascending)
                        dgKnjige.ItemsSource = list.OrderBy(k => k.GodinaIzdanja).ToList();
                    else
                        dgKnjige.ItemsSource = list.OrderByDescending(k => k.GodinaIzdanja).ToList();
                    break;

                case "ISBN":
                    if (direction == ListSortDirection.Ascending)
                        dgKnjige.ItemsSource = list.OrderBy(k => k.ISBN).ToList();
                    else
                        dgKnjige.ItemsSource = list.OrderByDescending(k => k.ISBN).ToList();
                    break;

                case "Žanr":
                    if (direction == ListSortDirection.Ascending)
                        dgKnjige.ItemsSource = list.OrderBy(k => k.Zanr).ToList();
                    else
                        dgKnjige.ItemsSource = list.OrderByDescending(k => k.Zanr).ToList();
                    break;

            }

            e.Column.SortDirection = direction; 
        }

        private void BtnPrikaziPosetioce_Click(object sender, RoutedEventArgs e)
        {
            Autor izabraniAutor = dgAutori.SelectedItem as Autor;
            if (izabraniAutor == null)
            {
                MessageBox.Show(
                    "Molimo izaberite autora iz tabele.",
                    "Upozorenje",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            List<string> isbnKnjigeAutora = autorKnjigaDao.GetKnjigeZaAutora(izabraniAutor.AutorID);

            var posetiociSaKnjigamaAutora = sviPosetioci
                .Where(p => p.ListaZelja != null &&
                            p.ListaZelja.Any(k => isbnKnjigeAutora.Contains(k.ISBN)))
                .Distinct()
                .ToList();

            if (!posetiociSaKnjigamaAutora.Any())
            {
                MessageBox.Show(
                     "Nema posetilaca sa knjigama ovog autora na listi želja.",
                     "Upozorenje",
                     MessageBoxButton.OK,
                     MessageBoxImage.Warning);
                return;
            }
            
            PosetiociListaZeljaAutorWindow win = new PosetiociListaZeljaAutorWindow(posetiociSaKnjigamaAutora);
            win.ShowDialog();
     
        
    }

        private void BtnPrikaziAutore_Click(object sender, RoutedEventArgs e)
        {

            Posetilac izabraniPosetilac = dgPosetioci.SelectedItem as Posetilac;
            if (izabraniPosetilac == null)
            {
                MessageBox.Show(
                    "Molimo izaberite posetioca iz tabele.",
                    "Upozorenje",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            ListaZeljaDao listaZeljaDao = new ListaZeljaDao();
            List<string> isbnKnjigaNaListiZelja = listaZeljaDao.GetKnjigeZaPosetioca(izabraniPosetilac.BrClanskeKarte);

            var autoriKnjigaNaListiZelja = sviAutori
                .Where(a => a.Knjige != null &&
                            a.Knjige.Any(k => isbnKnjigaNaListiZelja.Contains(k.ISBN)))
                .GroupBy(a => a.AutorID)   
                .Select(g => g.First())    
                .ToList();

            if (!autoriKnjigaNaListiZelja.Any())
            {
                MessageBox.Show(
                     "Nema autora sa knjigama na listi želja ovog posetioca.",
                     "Upozorenje",
                     MessageBoxButton.OK,
                     MessageBoxImage.Warning);
                return;
            }

            AutoriKnjigeListaZeljaWindow win = new AutoriKnjigeListaZeljaWindow(autoriKnjigaNaListiZelja);
            win.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void OpenAnalitika_Click(object sender, RoutedEventArgs e)
        {
            if (sviPosetioci == null || sveKnjige == null)
            {
                MessageBox.Show("Podaci nisu učitani.", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AnalitikaWindow prozor = new AnalitikaWindow(sviPosetioci, sveKnjige);
            prozor.Owner = this;
            prozor.ShowDialog();
        }

        

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
    
}
