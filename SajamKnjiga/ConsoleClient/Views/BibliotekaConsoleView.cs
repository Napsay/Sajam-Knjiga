using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.DAO;
using System.Threading;
using Core.Interfaces;
using Core;
using Core.Utils;
namespace ConsoleClient.Views
{
    internal class BibliotekaConsoleView
    {
        private readonly AutorDao _autorDao;
        private readonly AdresaDao _adresaDao;
        private readonly KnjigaDao _knjigaDao;
        private readonly PosetilacDao _posetilacDao;
        private readonly KupovinaDao _kupovinaDao;
        private readonly IzdavacDao _izdavacDao;
        private readonly ListaZeljaDao _listaZeljaDao;
       
        private readonly AutorKnjigaDao _autorKnjigaDao;

        public BibliotekaConsoleView(AutorDao autorDao, AdresaDao adresaDao, KnjigaDao knjigaDao, PosetilacDao posetilacDao,
                                        KupovinaDao kupovinaDao, IzdavacDao izdavacDao,ListaZeljaDao listzelja, AutorKnjigaDao autorKnjigaDao)
        {
            _autorDao = autorDao;
            _adresaDao = adresaDao;
            _knjigaDao = knjigaDao;
            _posetilacDao = posetilacDao;
            _kupovinaDao = kupovinaDao;
            _izdavacDao = izdavacDao;
            _listaZeljaDao = listzelja;
            _autorKnjigaDao = autorKnjigaDao;
           
        }

        public void RunMenu()
        {
            while (true)
            {
                ShowMenu();
                string userInput = System.Console.ReadLine() ?? "0";
                if (userInput == "0") break;
                HandleMenuInput(userInput);
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("\nChoose an option: ");
            Console.WriteLine("1: Show all Autors");
            Console.WriteLine("2: Add autor");
            Console.WriteLine("3: Update autor");
            Console.WriteLine("4: Remove autor");
            Console.WriteLine("5: Show all Books");
            Console.WriteLine("6: Add book");
            Console.WriteLine("7: Remove book");
            Console.WriteLine("8: Update book");
            Console.WriteLine("9: Show all visitors");
            Console.WriteLine("10: Add visitor");
            Console.WriteLine("11: Update visitor");
            Console.WriteLine("12: Remove visitor");
            Console.WriteLine("13: Show all purchases");
            Console.WriteLine("14: Add purchase");
            Console.WriteLine("15: Update purchase");
            Console.WriteLine("16: Remove purchase");
            Console.WriteLine("17: Show all publishers");
            Console.WriteLine("18: Add publisher");
            Console.WriteLine("19: Update publisher");
            Console.WriteLine("20: Remove publisher");
            Console.WriteLine("21: Add book to wishlist");

            Console.WriteLine("0: Close");
        }

        private void HandleMenuInput(string input)
        {
            switch (input)
            {
                case "1":
                    ShowAutor();
                    break;
                case "2":
                    AddAutor();
                    break;
                case "3":
                    UpdateAutor();
                    break;
                case "4":
                    RemoveAutor();
                    break;
                case "5":
                    ShowBooks();
                    break;
                case "6":
                    AddKnjiga();
                    break;
                case "7":
                    RemoveKnjiga();
                    break;
                case "8":
                    UpdateKnjiga();
                    break;
                case "9":
                    ShowPosetioci();
                    break;
                case "10":
                    AddPosetilac();
                    break;
                case "11":
                    UpdatePosetilac();
                    break;
                case "12":
                    RemovePosetilac();
                    break;
                case "13":
                    ShowKupovine();
                    break;
                case "14":
                    AddKupovina();
                    break;
                case "15":
                    UpdateKupovina();
                    break;
                case "16":
                    RemoveKupovina();
                    break;
                case "17":
                    ShowIzdavac();
                    break;
                case "18":
                    addIzdavac();
                    break;
                case "19":
                    UpdateIzdavac();
                    break;
                case "20":
                    RemoveIzdavac();
                    break;
                case "21":
                    AddToListaZelja();
                    break;
            }
        }

        private void AddToListaZelja()
        {
            Console.WriteLine("Enter membership card number:");
            string brKarte = Console.ReadLine();

            var posetilac = _posetilacDao
                .GetAllPosetilac()
                .FirstOrDefault(p => p.BrClanskeKarte == brKarte);

            if (posetilac == null)
            {
                Console.WriteLine("Visitor not found.");
                return;
            }

            Console.WriteLine("Enter book ISBN:");
            string isbn = Console.ReadLine();

            var knjiga = _knjigaDao.GetByISBN(isbn);

            if (knjiga == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }
            _listaZeljaDao.Add(new ListaZelja(brKarte, isbn));

            Console.WriteLine("Book added to wishlist.");
        }
        private void ShowAutor()
        {
            List<Autor> autori = _autorDao.GetAll();
            PrinAutor(autori);
        }
        private void PrinAutor(List<Autor> autori)
        {

            Console.WriteLine("AUTORS:");
            Console.WriteLine(
                $"{"ID",4} | {"Ime",-12} | {"Prezime",-15} | {"Datum rođenja",-12} | {"Telefon",-12} | {"Email",-25} | {"Isk.",4} |"
            );
            Console.WriteLine(new string('-', 100));

            foreach (Autor a in autori)
            {
                Console.WriteLine(a);
                if (a.Knjige != null && a.Knjige.Count > 0)
                {
                    Console.WriteLine("   Knjige:");
                    foreach (Knjiga k in a.Knjige)
                    {
                        Console.WriteLine("    - " + k.Naziv);
                    }
                }
                else
                {
                    Console.WriteLine("   Knjige: nema");
                }
                Console.WriteLine(new string('-', 100));
            }
        }


        private void AddAutor()
        {
            Autor autor = InputAutor();
            _autorDao.Add(autor);
            System.Console.WriteLine("Autor added successfully.");
        }


        private Adresa InputAdresa()
        {
            Console.WriteLine("Enter street name: ");
            string ulica = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Enter street number: ");
            int broj;
            while (!int.TryParse(Console.ReadLine(), out broj))
            {
                Console.WriteLine("Invalid number. Please enter an integer: ");
            }

            Console.WriteLine("Enter city: ");
            string grad = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Enter country: ");
            string drzava = Console.ReadLine() ?? string.Empty;

    
            var existing = _adresaDao.GetAll()
                .FirstOrDefault(a => a.Ulica == ulica && a.Broj == broj && a.Grad == grad && a.Drzava == drzava);

            if (existing != null)
                return existing; 

            Adresa adresa = new Adresa { Ulica = ulica, Broj = broj, Grad = grad, Drzava = drzava };
            return _adresaDao.Add(adresa);
        }

        private Autor InputAutor()
        {
            System.Console.WriteLine("Enter first name: ");
            string ime = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter last name: ");
            string prezime = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter date of birth (yyyy-MM-dd): ");
            DateTime datumRodjenja;
            while (!DateTime.TryParse(Console.ReadLine(), out datumRodjenja))
            {
                System.Console.WriteLine("Invalid date. Please enter in format yyyy-MM-dd: ");
            }

            Adresa adresa = InputAdresa();
            
            System.Console.WriteLine("Enter phone: ");
            string telefon = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter ID card number: ");
            string brojLicneKarte = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter years of experience: ");
            int godineIskustva;
            while (!int.TryParse(Console.ReadLine(), out godineIskustva))
            {
                System.Console.WriteLine("Invalid number. Please enter an integer: ");
            }

            System.Console.WriteLine("Enter email: ");
            string email = System.Console.ReadLine() ?? string.Empty;

            return new Autor
            {
                Ime = ime,
                Prezime = prezime,
                DatumRodjenja = datumRodjenja,
                Adresa = adresa,
                Telefon = telefon,
                BrojLicneKarte = brojLicneKarte,
                GodineIskustva = godineIskustva,
                Email = email
            };
        }



        private void RemoveAutor()
        {
            System.Console.WriteLine("Enter Autor ID to remove: ");
            int autorId;
            while (!int.TryParse(Console.ReadLine(), out autorId))
            {
                System.Console.WriteLine("Invalid ID. Please enter an integer: ");
            }
            try
            {
                _autorDao.Delete(autorId);
                System.Console.WriteLine("Autor removed successfully.");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void UpdateAutor()
        {
            Console.WriteLine("Enter ID of autor to update");
            int id = int.Parse(Console.ReadLine());
            var existingAutor = _autorDao.GetBySifra(id);
            if (existingAutor == null)
            {
                Console.WriteLine("Autor not found.");
                return;
            }

            Console.WriteLine("Enter new autor details (leave blank to keep existing):");

            
            Console.WriteLine($"First name ({existingAutor.Ime}):");
            string ime = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(ime)) existingAutor.Ime = ime;

            Console.WriteLine($"Last name ({existingAutor.Prezime}):");
            string prezime = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(prezime)) existingAutor.Prezime = prezime;

            Console.WriteLine("Update address? (y/n)");
            string input = Console.ReadLine() ?? "n";
            if (input.ToLower() == "y")
            {
             
                Adresa adresa = existingAutor.Adresa != null
                    ? _adresaDao.GetBySifra(existingAutor.Adresa.Sifra)
                    : InputAdresa();

                Console.WriteLine($"Street ({adresa.Ulica}):");
                string ulica = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(ulica)) adresa.Ulica = ulica;

                Console.WriteLine($"Number ({adresa.Broj}):");
                if (int.TryParse(Console.ReadLine(), out int broj)) adresa.Broj = broj;

                Console.WriteLine($"City ({adresa.Grad}):");
                string grad = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(grad)) adresa.Grad = grad;

                Console.WriteLine($"Country ({adresa.Drzava}):");
                string drzava = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(drzava)) adresa.Drzava = drzava;

            
                _adresaDao.Update(adresa);
                existingAutor.Adresa = adresa;
            }


            Console.WriteLine($"Phone ({existingAutor.Telefon}):");
            string telefon = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(telefon)) existingAutor.Telefon = telefon;

            Console.WriteLine($"ID card ({existingAutor.BrojLicneKarte}):");
            string brojKarte = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(brojKarte)) existingAutor.BrojLicneKarte = brojKarte;

            Console.WriteLine($"Years of experience ({existingAutor.GodineIskustva}):");
            if (int.TryParse(Console.ReadLine(), out int godine)) existingAutor.GodineIskustva = godine;

            Console.WriteLine($"Email ({existingAutor.Email}):");
            string email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email)) existingAutor.Email = email;

            Console.WriteLine($"Date of birth ({existingAutor.DatumRodjenja:yyyy-MM-dd}):");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime datum)) existingAutor.DatumRodjenja = datum;

            _autorDao.Update(existingAutor);
            Console.WriteLine("Autor updated successfully.");
        }

        private void ShowBooks()
        {
            List<Knjiga> knjige = _knjigaDao.getAllKnjige();
            PrintKnjiga(knjige);
        }

        private void PrintKnjiga(List<Knjiga> knjige)
        {
            Console.WriteLine("KNJIGE:");
            Console.WriteLine(
                $"{"ISBN",-15} | {"Naziv",-25} | {"God.",4} | {"Cena",6}"
            );
            Console.WriteLine(new string('-', 70));

            foreach (Knjiga k in knjige)
            {
                Console.WriteLine(
                    $"{k.ISBN,-15} | {k.Naziv,-25} | {k.GodinaIzdanja,4} | {k.Cena,6}"
                );

                Console.Write("   Autori: ");
                if (k.Autori.Count > 0)
                    Console.WriteLine(string.Join(", ", k.Autori.Select(a => $"{a.AutorID} {a.Ime} {a.Prezime}")));
                else
                    Console.WriteLine("nema");

                Console.WriteLine($"Kupili: {k.Posetioci.Count} posetilaca");
                Console.WriteLine($"U listi želja: {k.PosetiociListaZelja.Count} posetilaca");
                Console.WriteLine(new string('-', 100));
            }
        }

        private Knjiga InputKnjiga()
        {
            Console.WriteLine("Enter ISBN: ");
            string isbn = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Enter book title: ");
            string naziv = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Enter genre: ");
            string zanr = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Enter publication year: ");
            int godinaIzdanja;
            while (!int.TryParse(Console.ReadLine(), out godinaIzdanja))
            {
                Console.WriteLine("Invalid number. Please enter an integer: ");
            }

            Console.WriteLine("Enter price: ");
            int cena;
            while (!int.TryParse(Console.ReadLine(), out cena))
            {
                Console.WriteLine("Invalid number. Please enter an integer: ");
            }

            Console.WriteLine("Enter number of pages: ");
            int brojStrana;
            while (!int.TryParse(Console.ReadLine(), out brojStrana))
            {
                Console.WriteLine("Invalid number. Please enter an integer: ");
            }

            Console.WriteLine("Enter the author id of the book: ");
            int AutorID;
            while(!int.TryParse(Console.ReadLine(), out AutorID))
            {
                Console.WriteLine("Invalid number. Please enter an integer for author ID: ");
            }

            Autor autor = _autorDao.GetBySifra(AutorID);
            if (autor == null)
            {
                Console.WriteLine("Author not found. Setting as null.");
            }

            Console.WriteLine("Enter publisher ID: ");
            int sifraIzdavaca;
            while (!int.TryParse(Console.ReadLine(), out sifraIzdavaca))
            {
                Console.WriteLine("Invalid number. Please enter an integer for publisher ID: ");
            }

            
            Izdavac izdavac = _izdavacDao.GetBySifra(sifraIzdavaca);
            if (izdavac == null)
            {
                Console.WriteLine("Publisher not found. Setting as null.");
            }
            List<Autor> autori = new List<Autor>();
            List<Posetilac> posetioci = new List<Posetilac>();
            List<Posetilac> posetiociListaZelja = new List<Posetilac>();

          
            if (autor != null)
            {
                autori.Add(autor);
            }

            Knjiga novaKnjiga = new Knjiga
            {
                ISBN = isbn,
                Naziv = naziv,
                Zanr = zanr,
                GodinaIzdanja = godinaIzdanja,
                Cena = cena,
                BrojStrana = brojStrana,
                Izdavac = izdavac,
                Autori = autori,
                Posetioci = posetioci,
                PosetiociListaZelja = posetiociListaZelja
            };

            if (autor != null)
            {
                autor.Knjige.Add(novaKnjiga);
            }
            _autorKnjigaDao.AddVeza(autor.AutorID, novaKnjiga.ISBN);

            return novaKnjiga;
        }


        private void AddKnjiga()
        {
            Knjiga knjiga = InputKnjiga();

            _knjigaDao.addKnjiga(knjiga);

            if (knjiga.Autori != null)
            {
                foreach (var autor in knjiga.Autori)
                {
                    _autorKnjigaDao.AddVeza(autor.AutorID, knjiga.ISBN);
                }
            }

            Console.WriteLine("Knjiga added successfully.");
        }


        private void RemoveKnjiga()
        {
            Console.WriteLine("Enter ISBN of the book to remove: ");
            string isbn = Console.ReadLine() ?? string.Empty;

            var knjiga = _knjigaDao.GetByISBN(isbn);
            if (knjiga == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            if (knjiga.Autori != null)
            {
                foreach (var autor in knjiga.Autori)
                {
                    autor.Knjige.Remove(knjiga);
                }
            }

            if (knjiga.Izdavac != null)
            {
                knjiga.Izdavac.SpisakKnjiga.Remove(knjiga);
            }

            try
            {
                _knjigaDao.deleteKnjiga(isbn);
                foreach (var izdavac in _izdavacDao.GetAll())
                {
                    izdavac.SpisakKnjiga.RemoveAll(k => k.ISBN == isbn);
                }

                _izdavacDao.SaveAll();
                _autorKnjigaDao.RemoveByISBN(isbn);
                Console.WriteLine("Knjiga removed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        private void UpdateKnjiga()
        {
            Console.WriteLine("Enter ISBN of the book to update: ");
            string isbn = Console.ReadLine() ?? string.Empty;

            var existingKnjiga = _knjigaDao.GetByISBN(isbn);
            if (existingKnjiga == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            Console.WriteLine($"Current title: {existingKnjiga.Naziv}");
            Console.Write("Enter new title (leave blank to keep): ");
            string naziv = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(naziv)) existingKnjiga.Naziv = naziv;

            Console.WriteLine($"Current genre: {existingKnjiga.Zanr}");
            Console.Write("Enter new genre (leave blank to keep): ");
            string zanr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(zanr)) existingKnjiga.Zanr = zanr;

            Console.WriteLine($"Current year: {existingKnjiga.GodinaIzdanja}");
            if (int.TryParse(Console.ReadLine(), out int godina)) existingKnjiga.GodinaIzdanja = godina;

            Console.WriteLine($"Current price: {existingKnjiga.Cena}");
            if (int.TryParse(Console.ReadLine(), out int cena)) existingKnjiga.Cena = cena;

            Console.WriteLine($"Current number of pages: {existingKnjiga.BrojStrana}");
            if (int.TryParse(Console.ReadLine(), out int brStrana)) existingKnjiga.BrojStrana = brStrana;

            _knjigaDao.updateKnjiga(existingKnjiga);
          
            Console.WriteLine("Book updated successfully.");
        }

        private void ShowPosetioci()
        {
            List<Posetilac> posetioci = _posetilacDao.GetAllPosetilac();
            PrintPosetilac(posetioci);
        }

        private void PrintPosetilac(List<Posetilac> posetioci)
        {
            Console.WriteLine("POSETIOCI:");
            Console.WriteLine(new string('-', 150));

            foreach (Posetilac p in posetioci)
            {
                Console.WriteLine(p);
                var zelje = _listaZeljaDao.GetAll().Where(z => z.BrClanskeKarte == p.BrClanskeKarte).ToList();

                if (zelje.Count > 0)
                {
                    Console.WriteLine("   Lista želja:");
                    foreach (var z in zelje)
                    {
                        var knjiga = _knjigaDao.GetByISBN(z.ISBN);
                        if (knjiga != null)
                            Console.WriteLine("    - " + knjiga.Naziv);
                    }
                }
                else
                {
                    Console.WriteLine("Lista želja: nema");
                }
                Console.WriteLine(new string('-', 100)); 
            }
        }

        private void AddPosetilac()
        {
            Posetilac posetilac = InputPosetilac();
            _posetilacDao.AddPosetilac(posetilac);
            System.Console.WriteLine("Posetilac added successfully.");
        }


        private Posetilac InputPosetilac()
        {
            System.Console.WriteLine("Enter first name: ");
            string ime = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter last name: ");
            string prezime = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter date of birth (yyyy-MM-dd): ");
            DateTime datumRodjenja;
            while (!DateTime.TryParse(Console.ReadLine(), out datumRodjenja))
            {
                System.Console.WriteLine("Invalid date. Please enter in format yyyy-MM-dd: ");
            }

            Adresa adresa = InputAdresa();

            System.Console.WriteLine("Enter phone: ");
            string telefon = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter email: ");
            string email = System.Console.ReadLine() ?? string.Empty;


            System.Console.WriteLine("Enter current membership year: ");
            int trenutnaGodClanstva;
            while (!int.TryParse(Console.ReadLine(), out trenutnaGodClanstva))
            {
                System.Console.WriteLine("Invalid number. Please enter an integer: ");
            }

            System.Console.WriteLine("Enter visitor status (P - Normal Status,B - VIP Status): ");
            StatusPosetioca status;
            while (!Enum.TryParse(Console.ReadLine(), true, out status))
            {
                System.Console.WriteLine("Invalid status. Try again: ");
            }

            System.Console.WriteLine("Enter average review rating: ");
            double prosecnaOcenaRec;
            while (!double.TryParse(Console.ReadLine(), out prosecnaOcenaRec))
            {
                System.Console.WriteLine("Invalid number. Please enter a decimal value: ");
            }

            return new Posetilac
            {
                Ime = ime,
                Prezime = prezime,
                DatumRodjenja = datumRodjenja,
                Adresa = adresa,
                Telefon = telefon,
                Email = email,
                TrenutnaGodClanstva = trenutnaGodClanstva,
                Status = status,
                ProsecnaOcenaRec = prosecnaOcenaRec,
                KupljeneKnjige = new List<Kupovina>()
            };
        }
        private void RemovePosetilac()
        {
            System.Console.WriteLine("Enter visitor membership card number to remove: ");
            string brClanskeKarte = System.Console.ReadLine() ?? string.Empty;
            try
            {
                _posetilacDao.DeletePosetilac(brClanskeKarte);
                System.Console.WriteLine("Visitor removed successfully.");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void UpdatePosetilac()
        {
            Console.WriteLine("Enter ID card number of visitor to update:");
            string brKarte = Console.ReadLine() ?? string.Empty;

            var existingPosetilac = _posetilacDao.GetByBrClanskeKarte(brKarte);
            if (existingPosetilac == null)
            {
                Console.WriteLine("Visitor not found.");
                return;
            }

            Console.WriteLine("Enter new visitor details (leave blank to keep existing):");

          
            Console.WriteLine($"First name ({existingPosetilac.Ime}):");
            string ime = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(ime)) existingPosetilac.Ime = ime;

            Console.WriteLine($"Last name ({existingPosetilac.Prezime}):");
            string prezime = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(prezime)) existingPosetilac.Prezime = prezime;

            Console.WriteLine("Update address? (y/n)");
            string input = Console.ReadLine() ?? "n";
            if (input.ToLower() == "y")
            {
                
                Adresa adresa = existingPosetilac.Adresa != null
                    ? _adresaDao.GetBySifra(existingPosetilac.Adresa.Sifra)
                    : InputAdresa();

                Console.WriteLine($"Street ({adresa.Ulica}):");
                string ulica = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(ulica)) adresa.Ulica = ulica;

                Console.WriteLine($"Number ({adresa.Broj}):");
                if (int.TryParse(Console.ReadLine(), out int broj)) adresa.Broj = broj;

                Console.WriteLine($"City ({adresa.Grad}):");
                string grad = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(grad)) adresa.Grad = grad;

                Console.WriteLine($"Country ({adresa.Drzava}):");
                string drzava = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(drzava)) adresa.Drzava = drzava;

                _adresaDao.Update(adresa);
                existingPosetilac.Adresa = adresa;
            }


            Console.WriteLine($"Phone ({existingPosetilac.Telefon}):");
            string telefon = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(telefon)) existingPosetilac.Telefon = telefon;

            Console.WriteLine($"Email ({existingPosetilac.Email}):");
            string email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email)) existingPosetilac.Email = email;


            _posetilacDao.UpdatePosetilac(existingPosetilac);

            Console.WriteLine("Visitor updated successfully.");
        }


        private void ShowKupovine()
        {
            List<Kupovina> kupovine = _kupovinaDao.getAllKupovine();
            PrintKupovina(kupovine);
        }

        private void PrintKupovina(List<Kupovina> kupovine)
        {
            Console.WriteLine("KUPOVINE:");
            Console.WriteLine(new string('-', 60));

            foreach (Kupovina k in kupovine)
            {
                if (k.Kupac == null || k.Knjiga == null)
                {
                    Console.WriteLine("NEISPRAVNA KUPOVINA (nepovezani podaci)");
                    continue;
                }

                Console.WriteLine(
                    $"{k.Kupac.Ime} {k.Kupac.Prezime} -> {k.Knjiga.Naziv} | {k.Ocena}/5"
                );

                Console.WriteLine(
                    $"   {k.DatumKupovine:dd.MM.yyyy} | {k.Komentar}"
                );
                Console.WriteLine(new string('-', 100));
            }
        }


        private Kupovina InputKupovina()
        {
            Posetilac kupac = null;
            while (kupac == null)
            {
                Console.WriteLine("Enter the customer's membership card number");
                string brKarte = Console.ReadLine();


                foreach (Posetilac p in _posetilacDao.GetAllPosetilac())
                {
                    if (p.BrClanskeKarte == brKarte)
                    {
                        kupac = p;
                        break;
                    }
                }

                if (kupac == null)
                    Console.WriteLine("The customer was not found in the system. Try again.");
            }

            Knjiga knjiga = null;
            while (knjiga == null)
            {
                Console.WriteLine("Enter the ISBN of the book:");
                string isbn = Console.ReadLine();


                foreach (Knjiga k in _knjigaDao.getAllKnjige())
                {
                    if (k.ISBN == isbn)
                    {
                        knjiga = k; break;
                    }
                }

                if (knjiga == null)
                    Console.WriteLine("The book does not exist in the system. Try again.");
            }

            Console.WriteLine("Enter the date of purchase (yyyy-MM-dd):");
            DateTime datumKupovine;
            while (!DateTime.TryParse(Console.ReadLine(), out datumKupovine))
            {
                Console.WriteLine("Incorrect date format, please try again:");
            }

            Console.WriteLine("Enter a book rating.");
            double ocena = double.Parse(Console.ReadLine());

            Console.WriteLine("Leave a comment: ");
            string komenatar = Console.ReadLine();

            Kupovina kupovina = new Kupovina();
            kupovina.Kupac = kupac;
            kupovina.Knjiga = knjiga;

  
            kupovina.BrClanskeKarteKupca = kupac.BrClanskeKarte;
            kupovina.ISBNKnjige = knjiga.ISBN;

            kupovina.DatumKupovine = datumKupovine;
            kupovina.Ocena = ocena;
            kupovina.Komentar = komenatar;

            return kupovina;

        }

        private void AddKupovina()
        {
            Kupovina kupovina = InputKupovina();
            _kupovinaDao.addKupovina(kupovina);
            System.Console.WriteLine("Purchase added successfully.");
        }


        private void RemoveKupovina()
        {
            System.Console.WriteLine("Enter ID of purchase to remove: ");
            int kupovinaId;
            while (!int.TryParse(Console.ReadLine(), out kupovinaId))
            {
                System.Console.WriteLine("Invalid ID. Please enter an integer: ");
            }
            try
            {
                _kupovinaDao.deleteKupovina(kupovinaId);
                System.Console.WriteLine("Purchase removed successfully.");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void UpdateKupovina()
        {
            Console.WriteLine("Enter purchase id for update");
            int id = int.Parse(Console.ReadLine());
            var existingKupovina = _kupovinaDao.GetById(id);
            if (existingKupovina == null)
            {
                Console.WriteLine("Kupovina not found");
                return;
            }

            Console.WriteLine("Enter new visitor details:");
            Kupovina updatedData = InputKupovina();

            existingKupovina.Kupac = updatedData.Kupac;
            existingKupovina.Knjiga = updatedData.Knjiga;


            existingKupovina.BrClanskeKarteKupca = updatedData.BrClanskeKarteKupca;
            existingKupovina.ISBNKnjige = updatedData.ISBNKnjige;

            existingKupovina.DatumKupovine = updatedData.DatumKupovine;
            existingKupovina.Ocena = updatedData.Ocena;
            existingKupovina.Komentar = updatedData.Komentar;

            _kupovinaDao.updateKupovina(existingKupovina);

            Console.WriteLine("Purchase has been successfully changed.");
        }

        private void RemoveIzdavac()
        {
            Console.WriteLine("Enter publisher ID to remove: ");
            int sifra;
            while (!int.TryParse(Console.ReadLine(), out sifra))
            {
                Console.WriteLine("Invalid ID. Enter integer:");
            }
            try
            {
                _izdavacDao.Delete(sifra);
                Console.WriteLine("Izdavac removed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private Izdavac InputIzdavac()
        {
            Console.WriteLine("Enter publisher name: ");
            string naziv = Console.ReadLine();

            Console.WriteLine("Enter chief ID (Autor ID): ");
            int sefId = Convert.ToInt32(Console.ReadLine());
            Autor sef = _autorDao.GetBySifra(sefId);

            Console.WriteLine("Enter autor IDs: ");
            string autorIdsInput = Console.ReadLine();
            List<Autor> spisakAutora = new List<Autor>();
            if (autorIdsInput != null)
            {
                string[] ids = autorIdsInput.Split(',');
                for (int i = 0; i < ids.Length; i++)
                {
                    int id = Convert.ToInt32(ids[i]);
                    Autor a = _autorDao.GetBySifra(id);
                    spisakAutora.Add(a);
                }
            }
            Console.WriteLine("Enter book ISBNs: ");
            string knjigeInput = Console.ReadLine();
            List<Knjiga> knjige = new List<Knjiga>();

            if (knjigeInput != null)
            {
                string[] isbns = knjigeInput.Split(',');
                for (int i = 0; i < isbns.Length; i++)
                {
                    Knjiga k = _knjigaDao.GetByISBN(isbns[i]);
                    knjige.Add(k);
                }
            }

            Izdavac izdavac = new Izdavac();
            izdavac.Naziv = naziv;
            izdavac.Sef = sef;
            izdavac.SpisakAutora = spisakAutora;
            izdavac.SpisakKnjiga = knjige;
            return izdavac; ;
        }

        private void addIzdavac()
        {
            Izdavac izdavac = InputIzdavac();
            _izdavacDao.Add(izdavac);
            System.Console.WriteLine("Publisher added successfully.");
        }

        private void UpdateIzdavac()
        {
            Console.WriteLine("Enter publisher ID to update: ");
            int sifra = int.Parse(Console.ReadLine());
            var existingIzdavac = _izdavacDao.GetBySifra(sifra);
            if (existingIzdavac == null)
            {
                Console.WriteLine("Publisher not found.");
                return;
            }
            Console.WriteLine("Enter new publisher details:");
            Izdavac updatedData = InputIzdavac();
            existingIzdavac.Naziv = updatedData.Naziv;
            existingIzdavac.Sef = updatedData.Sef;
            existingIzdavac.SpisakAutora = updatedData.SpisakAutora;
            existingIzdavac.SpisakKnjiga = updatedData.SpisakKnjiga;
            _izdavacDao.Update(existingIzdavac);
            Console.WriteLine("Publisher updated successfully.");
        }

        private void ShowIzdavac()
        {
            List<Izdavac> izdavaci = _izdavacDao.GetAll();
            PrintIzdavac(izdavaci);
        }

        private void PrintIzdavac(List<Izdavac> izdavaci)
        {
            Console.WriteLine("IZDAVAČI:");
            Console.WriteLine(new string('-', 60));

            foreach (Izdavac i in izdavaci)
            {
                Console.WriteLine($"{i.Naziv} (ID: {i.Sifra})");

                if (i.Sef != null)
                    Console.WriteLine($"   Šef: {i.Sef.Ime} {i.Sef.Prezime}");
                else
                    Console.WriteLine("   Šef: nije dodeljen");

                Console.Write("   Autori: ");
                if (i.SpisakAutora != null && i.SpisakAutora.Any(a => !string.IsNullOrEmpty(a?.Ime)))
                    Console.WriteLine(string.Join(", ", i.SpisakAutora
                                                       .Where(a => a != null && !string.IsNullOrEmpty(a.Ime))
                                                       .Select(a => $"{a.Ime} {a.Prezime}")));
                else
                    Console.WriteLine("nema");

                Console.Write("   Knjige: ");
                if (i.SpisakKnjiga != null && i.SpisakKnjiga.Any(k => !string.IsNullOrEmpty(k?.Naziv)))
                    Console.WriteLine(string.Join(", ", i.SpisakKnjiga
                                                       .Where(k => k != null && !string.IsNullOrEmpty(k.Naziv))
                                                       .Select(k => k.Naziv)));
                else
                    Console.WriteLine("nema");
                Console.WriteLine(new string('-', 100));
            }
            
        }
    }
}
