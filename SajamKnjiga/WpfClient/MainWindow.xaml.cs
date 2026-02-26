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
        private List<Knjiga> _filteredKnjige;
        private List<Autor> _filteredAutori;
        private List<Posetilac> _filteredPosetioci;


        private int currentPagePosetioci = 1;
        private int totalPagesPosetioci = 1;

        private int currentPageAutori = 1;
        private int totalPagesAutori = 1;

        private int currentPageKnjige = 1;
        private int totalPagesKnjige = 1;

        private const int PageSize = 16; 

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

        private List<T> GetPage<T>(List<T> fullList, int pageNumber)
        {
            return fullList
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }


        private void RefreshPosetiociGrid()
        {
            if (_filteredPosetioci == null) return;

            totalPagesPosetioci = (int)Math.Ceiling((double)_filteredPosetioci.Count / PageSize);
            var pageItems = GetPage(_filteredPosetioci, currentPagePosetioci);
            dgPosetioci.ItemsSource = pageItems;
            txtPosetiociPageInfo.Text = $"Strana {currentPagePosetioci} od {totalPagesPosetioci}";
        }

        private void RefreshAutoriGrid()
        {
            if (_filteredAutori == null) return;

            totalPagesAutori = (int)Math.Ceiling((double)_filteredAutori.Count / PageSize);
            var pageItems = GetPage(_filteredAutori, currentPageAutori);
            dgAutori.ItemsSource = pageItems;
            txtAutoriPageInfo.Text = $"Strana {currentPageAutori} od {totalPagesAutori}";
        }

        private void RefreshKnjigeGrid()
        {
            if (_filteredKnjige == null) return;

            totalPagesKnjige = (int)Math.Ceiling((double)_filteredKnjige.Count / PageSize);
            var pageItems = GetPage(_filteredKnjige, currentPageKnjige);
            dgKnjige.ItemsSource = pageItems;
            txtKnjigePageInfo.Text = $"Strana {currentPageKnjige} od {totalPagesKnjige}";
        }

        private void BtnFirstPage_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var tag = button.Tag?.ToString();
            if (string.IsNullOrEmpty(tag)) return;

            switch (tag)
            {
                case "Posetioci":
                    currentPagePosetioci = 1;
                    RefreshPosetiociGrid();
                    break;
                case "Autori":
                    currentPageAutori = 1;
                    RefreshAutoriGrid();
                    break;
                case "Knjige":
                    currentPageKnjige = 1;
                    RefreshKnjigeGrid();
                    break;
            }
        }

        private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var tag = button.Tag?.ToString();
            if (string.IsNullOrEmpty(tag)) return;

            switch (tag)
            {
                case "Posetioci":
                    if (currentPagePosetioci > 1) currentPagePosetioci--;
                    RefreshPosetiociGrid();
                    break;
                case "Autori":
                    if (currentPageAutori > 1) currentPageAutori--;
                    RefreshAutoriGrid();
                    break;
                case "Knjige":
                    if (currentPageKnjige > 1) currentPageKnjige--;
                    RefreshKnjigeGrid();
                    break;
            }
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var tag = button.Tag?.ToString();
            if (string.IsNullOrEmpty(tag)) return;

            switch (tag)
            {
                case "Posetioci":
                    if (currentPagePosetioci < totalPagesPosetioci) currentPagePosetioci++;
                    RefreshPosetiociGrid();
                    break;
                case "Autori":
                    if (currentPageAutori < totalPagesAutori) currentPageAutori++;
                    RefreshAutoriGrid();
                    break;
                case "Knjige":
                    if (currentPageKnjige < totalPagesKnjige) currentPageKnjige++;
                    RefreshKnjigeGrid();
                    break;
            }
        }

        private void BtnLastPage_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var tag = button.Tag?.ToString();
            if (string.IsNullOrEmpty(tag)) return;

            switch (tag)
            {
                case "Posetioci":
                    currentPagePosetioci = totalPagesPosetioci;
                    RefreshPosetiociGrid();
                    break;
                case "Autori":
                    currentPageAutori = totalPagesAutori;
                    RefreshAutoriGrid();
                    break;
                case "Knjige":
                    currentPageKnjige = totalPagesKnjige;
                    RefreshKnjigeGrid();
                    break;
            }
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

            _filteredPosetioci = sviPosetioci;

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
            currentPagePosetioci = 1;
            RefreshPosetiociGrid();
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
            sviAutori = listaAutora;

            _filteredAutori = sviAutori;

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

            
            currentPageAutori = 1;
            RefreshAutoriGrid();
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

            _filteredKnjige = sveKnjige;

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

            currentPageKnjige = 1;
            RefreshKnjigeGrid();
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
            var izdavacWin = new IzdavaciWindow();
            izdavacWin.Owner = this;
            izdavacWin.ShowDialog();
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
                _filteredPosetioci = sviPosetioci;
            }
            else
            {
                string[] delovi = unos.Split(',')
                                      .Select(x => x.Trim())
                                      .Where(x => x != "")
                                      .ToArray();

                _filteredPosetioci = sviPosetioci.Where(p =>
                {
                    if (delovi.Length == 1)
                        return p.Prezime?.ToLower().Contains(delovi[0]) == true;

                    if (delovi.Length == 2)
                        return p.Prezime?.ToLower().Contains(delovi[0]) == true &&
                               p.Ime?.ToLower().Contains(delovi[1]) == true;

                    if (delovi.Length == 3)
                        return p.BrClanskeKarte?.ToLower().Contains(delovi[0]) == true &&
                               p.Ime?.ToLower().Contains(delovi[1]) == true &&
                               p.Prezime?.ToLower().Contains(delovi[2]) == true;

                    return false;
                }).ToList();
            }

            currentPagePosetioci = 1;
            RefreshPosetiociGrid();
        }

        private void PretraziAutore()
        {
            string unos = txtPretraga.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(unos))
            {
                _filteredAutori = sviAutori;
            }
            else
            {
                string[] delovi = unos.Split(',')
                                       .Select(x => x.Trim())
                                       .Where(x => x != "")
                                       .ToArray();

                _filteredAutori = sviAutori.Where(a =>
                {
                    if (delovi.Length == 1)
                        return a.Prezime?.ToLower().Contains(delovi[0]) == true;

                    if (delovi.Length >= 2)
                        return a.Prezime?.ToLower().Contains(delovi[0]) == true &&
                               a.Ime?.ToLower().Contains(delovi[1]) == true;

                    return false;
                }).ToList();
            }

            currentPageAutori = 1;
            RefreshAutoriGrid();
        }

        private void PretraziKnjige()
        {
            string unos = txtPretraga.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(unos))
            {
                _filteredKnjige = sveKnjige;
            }
            else
            {
                _filteredKnjige = sveKnjige.Where(k =>
                    (k.Naziv?.ToLower().Contains(unos) == true) ||
                    (k.ISBN?.ToLower().Contains(unos) == true)
                ).ToList();
            }

            currentPageKnjige = 1;
            RefreshKnjigeGrid();
        }

       
        private void dgPosetioci_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;

            ListSortDirection direction = (e.Column.SortDirection != ListSortDirection.Ascending) ?
                                          ListSortDirection.Ascending :
                                          ListSortDirection.Descending;

            switch (e.Column.Header.ToString())
            {
                case "Broj članske karte":
                    _filteredPosetioci = (direction == ListSortDirection.Ascending) ?
                        _filteredPosetioci
                            .OrderBy(p => p.GodinaClanskeKarte)
                            .ThenBy(p => p.RedniBroj)
                            .ToList() :
                        _filteredPosetioci
                            .OrderByDescending(p => p.GodinaClanskeKarte)
                            .ThenByDescending(p => p.RedniBroj)
                            .ToList();
                    break;

                case "Ime":
                    _filteredPosetioci = (direction == ListSortDirection.Ascending) ?
                        _filteredPosetioci.OrderBy(p => p.Ime).ToList() :
                        _filteredPosetioci.OrderByDescending(p => p.Ime).ToList();
                    break;

                case "Prezime":
                    _filteredPosetioci = (direction == ListSortDirection.Ascending) ?
                        _filteredPosetioci.OrderBy(p => p.Prezime).ToList() :
                        _filteredPosetioci.OrderByDescending(p => p.Prezime).ToList();
                    break;

                case "Status":
                    _filteredPosetioci = (direction == ListSortDirection.Ascending) ?
                        _filteredPosetioci.OrderBy(p => p.Status).ToList() :
                        _filteredPosetioci.OrderByDescending(p => p.Status).ToList();
                    break;
            }

            currentPagePosetioci = 1;
            RefreshPosetiociGrid();

            e.Column.SortDirection = direction;
        }

        private void dgAutori_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;

            ListSortDirection direction = (e.Column.SortDirection != ListSortDirection.Ascending) ?
                                          ListSortDirection.Ascending :
                                          ListSortDirection.Descending;

            switch (e.Column.Header.ToString())
            {
                case "Ime":
                    _filteredAutori = (direction == ListSortDirection.Ascending) ?
                        _filteredAutori.OrderBy(a => a.Ime).ToList() :
                        _filteredAutori.OrderByDescending(a => a.Ime).ToList();
                    break;

                case "Prezime":
                    _filteredAutori = (direction == ListSortDirection.Ascending) ?
                        _filteredAutori.OrderBy(a => a.Prezime).ToList() :
                        _filteredAutori.OrderByDescending(a => a.Prezime).ToList();
                    break;

                case "Email":
                    _filteredAutori = (direction == ListSortDirection.Ascending) ?
                        _filteredAutori.OrderBy(a => a.Email[0]).ToList() :
                        _filteredAutori.OrderByDescending(a => a.Email[0]).ToList();
                    break;
            }

            currentPageAutori = 1;
            RefreshAutoriGrid();

            e.Column.SortDirection = direction;
        }

        private void dgKnjige_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;

            ListSortDirection direction = (e.Column.SortDirection != ListSortDirection.Ascending) ?
                                          ListSortDirection.Ascending :
                                          ListSortDirection.Descending;

            switch (e.Column.Header.ToString())
            {
                case "Naziv":
                    _filteredKnjige = (direction == ListSortDirection.Ascending) ?
                        _filteredKnjige.OrderBy(k => k.Naziv).ToList() :
                        _filteredKnjige.OrderByDescending(k => k.Naziv).ToList();
                    break;

                case "Cena":
                    _filteredKnjige = (direction == ListSortDirection.Ascending) ?
                        _filteredKnjige.OrderBy(k => k.Cena).ToList() :
                        _filteredKnjige.OrderByDescending(k => k.Cena).ToList();
                    break;

                case "Godina":
                    _filteredKnjige = (direction == ListSortDirection.Ascending) ?
                        _filteredKnjige.OrderBy(k => k.GodinaIzdanja).ToList() :
                        _filteredKnjige.OrderByDescending(k => k.GodinaIzdanja).ToList();
                    break;

                case "ISBN":
                    _filteredKnjige = (direction == ListSortDirection.Ascending) ?
                        _filteredKnjige.OrderBy(k => k.ISBN).ToList() :
                        _filteredKnjige.OrderByDescending(k => k.ISBN).ToList();
                    break;

                case "Žanr":
                    _filteredKnjige = (direction == ListSortDirection.Ascending) ?
                        _filteredKnjige.OrderBy(k => k.Zanr).ToList() :
                        _filteredKnjige.OrderByDescending(k => k.Zanr).ToList();
                    break;
            }

            currentPageKnjige = 1;
            RefreshKnjigeGrid();

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

        private void OpenAnalitikaIzdavaca_Click(object sender, RoutedEventArgs e)
        {

            if (listaIzdavaca == null || sveKnjige == null || sviAutori == null)
            {
                MessageBox.Show("Podaci nisu učitani.", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AnalitikaIzdavaca prozor = new AnalitikaIzdavaca(
                listaIzdavaca,
                sveKnjige,
                sviAutori
            );

            prozor.Owner = this;
            prozor.ShowDialog();
        }
    }
    
}
