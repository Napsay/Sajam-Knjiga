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
namespace ConsoleClient.Views
{
    internal class BibliotekaConsoleView
    {
        private readonly AutorDao _autorDao;
        private readonly AdresaDao _adresaDao;
        private readonly KnjigaDao _knjigaDao;
        private readonly PosetilacDao _posetilacDao;
        private readonly KupovinaDao _kupovinaDao;

        public BibliotekaConsoleView(AutorDao autorDao, AdresaDao adresaDao, KnjigaDao knjigaDao, PosetilacDao posetilacDao,
                                        KupovinaDao kupovinaDao)
        {
            _autorDao = autorDao;
            _adresaDao = adresaDao;
            _knjigaDao = knjigaDao;
            _posetilacDao = posetilacDao;
            _kupovinaDao = kupovinaDao;

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
            }
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
            // Unos adrese

            int idUlice = 1;

            System.Console.WriteLine("Enter street name: ");
            string ulica = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter street number: ");
            int broj;
            while (!int.TryParse(Console.ReadLine(), out broj))
            {
                System.Console.WriteLine("Invalid number. Please enter an integer: ");
            }

            System.Console.WriteLine("Enter city: ");
            string grad = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter country: ");
            string drzava = System.Console.ReadLine() ?? string.Empty;

            Adresa adresa = new Adresa(idUlice, ulica, broj, grad, drzava);

            return adresa;
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
            _adresaDao.Add(adresa);
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
            Console.WriteLine("Enter new autor details:");
            Autor updatedData = InputAutor();
            existingAutor.Ime = updatedData.Ime;
            existingAutor.Prezime = updatedData.Prezime;
            existingAutor.Adresa = updatedData.Adresa;
            existingAutor.Telefon = updatedData.Telefon;
            existingAutor.BrojLicneKarte = updatedData.BrojLicneKarte;
            existingAutor.GodineIskustva = updatedData.GodineIskustva;
            existingAutor.Email = updatedData.Email;
            existingAutor.DatumRodjenja = updatedData.DatumRodjenja;

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
                $"{"ISBN",-15} | {"Naziv",-25} | {"Žanr",-15} | {"Godina",6} | {"Cena",6} | {"Strana",5} | {"Izdavač",-20} |"
            );
            Console.WriteLine(new string('-', 110));

            foreach (Knjiga k in knjige)
            {
                Console.WriteLine(k);
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

            Console.WriteLine("Enter publisher: ");
            string izdavac = Console.ReadLine() ?? string.Empty;

            List<string> autori = new List<string>();
            List<Posetilac> posetioci = new List<Posetilac>();
            List<Posetilac> posetiociListaZelja = new List<Posetilac>();

            return new Knjiga
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
        }

       
        private void AddKnjiga()
        {
            Knjiga knjiga = InputKnjiga();
            _knjigaDao.addKnjiga(knjiga);
            Console.WriteLine("Knjiga added successfully.");

        }


        private void RemoveKnjiga()
        {
            Console.WriteLine("Enter ISBN of the book to remove: ");
            string isbn = Console.ReadLine() ?? string.Empty;

            try
            {
                _knjigaDao.deleteKnjiga(isbn);
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

            Console.WriteLine("Enter new book details:");
            Knjiga updatedData = InputKnjiga();


            existingKnjiga.Naziv = updatedData.Naziv;
            existingKnjiga.Zanr = updatedData.Zanr;
            existingKnjiga.GodinaIzdanja = updatedData.GodinaIzdanja;
            existingKnjiga.Cena = updatedData.Cena;
            existingKnjiga.BrojStrana = updatedData.BrojStrana;
            existingKnjiga.Izdavac = updatedData.Izdavac;

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
            Console.WriteLine(
                $"{"Ime",-15} | {"Prezime",-15} | {"Datum rođenja",-12} | {"Telefon",-15} | {"Email",-25} | {"Članska karta",-15} | {"God. članstva",13} | {"Status",-10} | {"Ocena",6} |"
            );
            Console.WriteLine(new string('-', 150));

            foreach (Posetilac p in posetioci)
            {
                Console.WriteLine(p);
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
            _adresaDao.Add(adresa);

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
                KupljeneKnjige = new List<Kupovina>(),
                ListaZelja = new List<Knjiga>()
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
            Console.WriteLine("Enter visitor membership of the visitor to update: ");
            string brClanskeKarte = Console.ReadLine() ?? string.Empty;

            var existingVisitor = _posetilacDao.GetByBrClanskeKarte(brClanskeKarte);
            if (existingVisitor == null)
            {
                Console.WriteLine("Visitor not found.");
                return;
            }

            Console.WriteLine("Enter new visitor details:");
            Posetilac updatedData = InputPosetilac();
            existingVisitor.Ime = updatedData.Ime;
            existingVisitor.Prezime = updatedData.Prezime;
            existingVisitor.Adresa = updatedData.Adresa;
            existingVisitor.Telefon = updatedData.Telefon;
            existingVisitor.Email = updatedData.Email;
            existingVisitor.TrenutnaGodClanstva = updatedData.TrenutnaGodClanstva;
            existingVisitor.Status = updatedData.Status;
            existingVisitor.ProsecnaOcenaRec = updatedData.ProsecnaOcenaRec;
            existingVisitor.DatumRodjenja = updatedData.DatumRodjenja;


            _posetilacDao.UpdatePosetilac(existingVisitor);

            Console.WriteLine("Visitor updated successfully.");
        }

        private void ShowKupovine()
        {
            List<Kupovina> kupovine = _kupovinaDao.getAllKupovine();
            PrintKupovina(kupovine);
        }

        private void PrintKupovina(List<Kupovina> kupovine)
        {
            Console.WriteLine("KUPOVINE: ");

            foreach (Kupovina k in kupovine)
            {
                Console.WriteLine(new string('-', 50));
                Console.WriteLine(k.ToString());
            }

            Console.WriteLine(new string('-', 50));
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
            if(existingKupovina == null)
            {
                Console.WriteLine("Kupovina not found");
                return;
            }

            Console.WriteLine("Enter new visitor details:");
            Kupovina updatedData = InputKupovina();

            existingKupovina.Kupac = updatedData.Kupac;
            existingKupovina.Knjiga = updatedData.Knjiga;
            existingKupovina.DatumKupovine = updatedData.DatumKupovine;
            existingKupovina.Ocena = updatedData.Ocena;
            existingKupovina.Komentar = updatedData.Komentar;

           _kupovinaDao.updateKupovina(existingKupovina);
           
            Console.WriteLine("Purchase has been successfully changed.");
        }


    }
}
