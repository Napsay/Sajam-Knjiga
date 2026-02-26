using Core.DAO;
using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
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
using WpfClient.Properties;
using WpfClient.Resources;


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

            this.Title = Strings.MainWindow_Title;
            InitialLanguageSetup("sr-Latn-RS");
        }

        private void InitialLanguageSetup(string cultureCode)
        {
            LocalizationManager.SetLanguage(cultureCode);

            var culture = new CultureInfo(cultureCode);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            UpdateUI();
        }

        private void UpdateUI()
        {
            MenuItem_Language.Header = LocalizationManager.GetString("Menu_Language");
            this.Title = LocalizationManager.GetString("MainWindow_Title");

            MenuItem_File.Header = LocalizationManager.GetString("Menu_File");
            MenuItem_File_New.Header = LocalizationManager.GetString("Menu_File_New");
            MenuItem_File_Open.Header = LocalizationManager.GetString("Menu_File_Open");
            MenuItem_File_Open_Posetioci.Header = LocalizationManager.GetString("Menu_File_Open_Posetioci");
            MenuItem_File_Open_Knjige.Header = LocalizationManager.GetString("Menu_File_Open_Knjige");
            MenuItem_File_Open_Autori.Header = LocalizationManager.GetString("Menu_File_Open_Autori");
            MenuItem_File_Open_Izdavaci.Header = LocalizationManager.GetString("Menu_File_Open_Izdavaci");
            MenuItem_File_Save.Header = LocalizationManager.GetString("Menu_File_Save");
            MenuItem_File_Close.Header = LocalizationManager.GetString("Menu_File_Close");

            MenuItem_Edit.Header = LocalizationManager.GetString("Menu_Edit");
            MenuItem_Edit_Edit.Header = LocalizationManager.GetString("Menu_Edit_Edit");
            MenuItem_Edit_Delete.Header = LocalizationManager.GetString("Menu_Edit_Delete");

            MenuItem_Analitika.Header = LocalizationManager.GetString("Menu_Analitika");
            MenuItem_Analitika_Knjige.Header = LocalizationManager.GetString("Menu_Analitika_Knjige");
            MenuItem_Analitika_Izdavaci.Header = LocalizationManager.GetString("Menu_Analitika_Izdavaci");

            MenuItem_Help.Header = LocalizationManager.GetString("Menu_Help");
            MenuItem_Help_About.Header = LocalizationManager.GetString("Menu_Help_About");

            TabItem_Posetioci.Header = LocalizationManager.GetString("Tab_Posetioci");
            TabItem_Autori.Header = LocalizationManager.GetString("Tab_Autori");
            TabItem_Knjige.Header = LocalizationManager.GetString("Tab_Knjige");
            txtPretraga.Text = LocalizationManager.GetString("Search_Placeholder");

            Posetilac_ClanskaKarta.Header = LocalizationManager.GetString("Posetilac_ClanskaKarta");
            Posetilac_Ime.Header = LocalizationManager.GetString("Posetilac_Ime");
            Posetilac_Prezime.Header = LocalizationManager.GetString("Posetilac_Prezime");
            Posetilac_Adresa.Header = LocalizationManager.GetString("Posetilac_Adresa");
            Posetilac_Status.Header = LocalizationManager.GetString("Posetilac_Status");
            Posetilac_GodinaClanstva.Header = LocalizationManager.GetString("Posetilac_GodinaClanstva");
            Posetilac_ProsecnaOcena.Header = LocalizationManager.GetString("Posetilac_ProsecnaOcena");

            Kolona_IDAutora.Header = LocalizationManager.GetString("Kolona_IDAutora");
            Autor_Ime.Header = LocalizationManager.GetString("Autor_Ime");
            Autor_Prezime.Header = LocalizationManager.GetString("Autor_Prezime");
            Autor_DatumRodjenja.Header = LocalizationManager.GetString("Autor_DatumRodjenja");
            Autor_Adresa.Header = LocalizationManager.GetString("Autor_Adresa");
            Autor_Telefon.Header = LocalizationManager.GetString("Autor_Telefon");
            Autor_Email.Header = LocalizationManager.GetString("Autor_Email");
            Autor_GodIskustva.Header = LocalizationManager.GetString("Autor_GodIskustva");

            Knjiga_ISBN.Header = LocalizationManager.GetString("Knjiga_ISBN");
            Knjiga_Naziv.Header = LocalizationManager.GetString("Knjiga_Naziv");
            Knjiga_Zanr.Header = LocalizationManager.GetString("Knjiga_Zanr");
            Knjiga_GodIzdavanja.Header = LocalizationManager.GetString("Knjiga_GodIzdavanja");
            Knjiga_Cena.Header = LocalizationManager.GetString("Knjiga_Cena");
            Knjiga_BrStrana.Header = LocalizationManager.GetString("Knjiga_BrStrana");
            Knjiga_Izdavac.Header = LocalizationManager.GetString("Knjiga_Izdavac");
            Knjiga_Autori.Header = LocalizationManager.GetString("Knjiga_Autori");

            btnFirstPageAutori.Content = LocalizationManager.GetString("btnFirstPageAutori");
            btnLastPageAutori.Content = LocalizationManager.GetString("btnLastPageAutori");

            btnFirstPagePosetioci.Content = LocalizationManager.GetString("btnFirstPagePosetioci");
            btnLastPagePosetioci.Content = LocalizationManager.GetString("btnLastPagePosetioci");

            btnFirstPageKnjige.Content = LocalizationManager.GetString("btnFirstPageKnjige");
            btnLastPageKnjige.Content = LocalizationManager.GetString("btnLastPageKnjige");

            Btn_GridPosetioci.Content = LocalizationManager.GetString("Btn_GridPosetioci");
            Btn_GridAutori.Content = LocalizationManager.GetString("Btn_GridAutori");

            OsveziStatusBar();
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
            txtPosetiociPageInfo.Text = string.Format(
            LocalizationManager.GetString("Status_PageInfo"),
            currentPagePosetioci,
            totalPagesPosetioci
            );
        }

        private void RefreshAutoriGrid()
        {
            if (_filteredAutori == null) return;

            totalPagesAutori = (int)Math.Ceiling((double)_filteredAutori.Count / PageSize);
            var pageItems = GetPage(_filteredAutori, currentPageAutori);
            dgAutori.ItemsSource = pageItems;
            txtAutoriPageInfo.Text = string.Format(
            LocalizationManager.GetString("Status_PageInfo"),
            currentPageAutori,
            totalPagesAutori
            );
        }

        private void RefreshKnjigeGrid()
        {
            if (_filteredKnjige == null) return;

            totalPagesKnjige = (int)Math.Ceiling((double)_filteredKnjige.Count / PageSize);
            var pageItems = GetPage(_filteredKnjige, currentPageKnjige);
            dgKnjige.ItemsSource = pageItems;
            txtKnjigePageInfo.Text = string.Format(
            LocalizationManager.GetString("Status_PageInfo"),
            currentPageKnjige,
            totalPagesKnjige
            );
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
                        MessageBox.Show(
                        LocalizationManager.GetString("Msg_SelectVisitorForEdit"),
                        LocalizationManager.GetString("Msg_WarningTitle"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                        );
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
                        MessageBox.Show(
                        LocalizationManager.GetString("Msg_SelectAuthorForEdit"),
                        LocalizationManager.GetString("Msg_WarningTitle"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                        );
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
                        MessageBox.Show(
                        LocalizationManager.GetString("Msg_SelectBookForEdit"),
                        LocalizationManager.GetString("Msg_WarningTitle"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                        );
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
                string sajamskaPoruka = LocalizationManager.GetString("Status_BookFair");

                txtStatusLevo.Text = $"{sajamskaPoruka} - {tab.Header}";
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

            foreach (var p in sviPosetioci)
            {
                if (p.KupljeneKnjige != null && p.KupljeneKnjige.Any())
                {
                    p.ProsecnaOcenaRec = p.KupljeneKnjige
                        .Select(k => k.Ocena)
                        .DefaultIfEmpty(0)
                        .Average();
                }
                else
                {
                    p.ProsecnaOcenaRec = 0;
                }
            }

            _filteredPosetioci = sviPosetioci;
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
                    LocalizationManager.GetString("Msg_SelectVisitor"),
                    LocalizationManager.GetString("Msg_WarningTitle"),
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                     );
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
                     LocalizationManager.GetString("Msg_SelectAuthor"),
                     LocalizationManager.GetString("Msg_WarningTitle"),
                     MessageBoxButton.OK,
                     MessageBoxImage.Warning
                     );
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
                    LocalizationManager.GetString("Msg_SelectBook"),
                    LocalizationManager.GetString("Msg_WarningTitle"),
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                    );
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
               LocalizationManager.GetString("Msg_SelectVisitor"),
               LocalizationManager.GetString("Msg_WarningTitle"),
               MessageBoxButton.OK,
               MessageBoxImage.Warning
               );
                return;
            }

            PotvrdaBrisanjaWindow dlg = new PotvrdaBrisanjaWindow(
            LocalizationManager.GetString("Msg_ConfirmDeleteVisitor"),
            LocalizationManager.GetString("Msg_DeleteVisitorTitle")
            );

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
                LocalizationManager.GetString("Msg_SelectAuthor"),
                LocalizationManager.GetString("Msg_WarningTitle"),
                MessageBoxButton.OK,
                MessageBoxImage.Warning
                );
                return;
            }

            PotvrdaBrisanjaWindow dlg = new PotvrdaBrisanjaWindow(
            LocalizationManager.GetString("Msg_ConfirmDeleteAuthor"),
            LocalizationManager.GetString("Msg_DeleteAuthorTitle")
            );
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
                LocalizationManager.GetString("Msg_SelectBook"),
                LocalizationManager.GetString("Msg_WarningTitle"),
                MessageBoxButton.OK,
                MessageBoxImage.Warning
                );
                return;
            }

            PotvrdaBrisanjaWindow dlg = new PotvrdaBrisanjaWindow(
            LocalizationManager.GetString("Msg_ConfirmDeleteBook"),
            LocalizationManager.GetString("Msg_DeleteBookTitle")
            );

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

                MessageBox.Show(
                LocalizationManager.GetString("Msg_DataSaved"),
                LocalizationManager.GetString("Msg_SaveTitle"),           
                MessageBoxButton.OK,
                MessageBoxImage.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                string.Format(LocalizationManager.GetString("Msg_ErrorSavingData"), ex.Message),
                LocalizationManager.GetString("Msg_ErrorTitle"),
                MessageBoxButton.OK,
                MessageBoxImage.Error
                );
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

            switch (e.Column.SortMemberPath)
            {
                case "BrClanskeKarte":
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

                case "Godina članstva":
                    int trenutnaGodina = DateTime.Now.Year;
                    _filteredPosetioci = (direction == ListSortDirection.Ascending) ?
                        _filteredPosetioci.OrderBy(p => trenutnaGodina - p.GodinaClanskeKarte + 1).ToList() :
                        _filteredPosetioci.OrderByDescending(p => trenutnaGodina - p.GodinaClanskeKarte + 1).ToList();
                    break;

                case "Prosečna ocena":
                    _filteredPosetioci = (direction == ListSortDirection.Ascending) ?
                        _filteredPosetioci.OrderBy(p => p.ProsecnaOcenaRec).ToList() :
                        _filteredPosetioci.OrderByDescending(p => p.ProsecnaOcenaRec).ToList();
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

            switch (e.Column.SortMemberPath)
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

            switch (e.Column.SortMemberPath)
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
                LocalizationManager.GetString("Msg_SelectAuthor"),
                LocalizationManager.GetString("Msg_WarningTitle"),
                MessageBoxButton.OK,
                MessageBoxImage.Warning
                );
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
                LocalizationManager.GetString("Msg_NoVisitorsWithAuthorsBooks"),
                LocalizationManager.GetString("Msg_WarningTitle"),            
                MessageBoxButton.OK,
                MessageBoxImage.Warning
);
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
                LocalizationManager.GetString("Msg_SelectVisitor"),
                LocalizationManager.GetString("Msg_WarningTitle"),        
                MessageBoxButton.OK,
                MessageBoxImage.Warning
                );
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
                LocalizationManager.GetString("Msg_NoAuthorsForVisitorWishlist"),
                LocalizationManager.GetString("Msg_WarningTitle"),                 
                MessageBoxButton.OK,
                MessageBoxImage.Warning
 );
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
                MessageBox.Show(
                LocalizationManager.GetString("Msg_DataNotLoaded"),
                LocalizationManager.GetString("Msg_ErrorTitle"),
                MessageBoxButton.OK,
                MessageBoxImage.Error
   );
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
                MessageBox.Show(
                LocalizationManager.GetString("Msg_DataNotLoaded"),
                LocalizationManager.GetString("Msg_ErrorTitle"),
                MessageBoxButton.OK,
                MessageBoxImage.Error
                );   
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

        private void ChangeLanguage_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag != null)
            {
                string noviJezik = element.Tag.ToString();

                LocalizationManager.SetLanguage(noviJezik);

                var culture = new CultureInfo(noviJezik);
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;

                UpdateUI();

                RefreshPosetiociGrid();
                RefreshAutoriGrid();
                RefreshKnjigeGrid();
            }
        }
    }
    
}
