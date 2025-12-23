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

        public BibliotekaConsoleView(AutorDao autorDao,AdresaDao adresaDao, KnjigaDao knjigaDao, PosetilacDao posetilacDao)
        {
                _autorDao = autorDao;
                _adresaDao = adresaDao;
               _knjigaDao = knjigaDao;
               _posetilacDao = posetilacDao;

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
            System.Console.WriteLine("\nChoose an option: ");
            System.Console.WriteLine("1: Show all Autors");
            System.Console.WriteLine("2: Add autor");
            System.Console.WriteLine("3: Update autor");
            System.Console.WriteLine("4: Remove autor");
            System.Console.WriteLine("5: Show all Books");
            System.Console.WriteLine("6: Add book");
            System.Console.WriteLine("7: Remove book");
            System.Console.WriteLine("8: Update book");
            System.Console.WriteLine("9: Show all visitors");

            System.Console.WriteLine("0: Close");
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
                    //UpdateAutor();
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
           
            int idUlice =1 ;

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
            PrintPosetioci(posetioci);
        }

        private void PrintPosetioci(List<Posetilac> posetioci)
        {
            Console.WriteLine("ПОСЕТИОЦИ:");
            Console.WriteLine(
                $"{"Ime",-12} | {"Prezime",-12} | {"Datum rodjenja",-12} | {"Telefon",-12} | {"Email",-25} | {"Clanska karta",-12} | {"God. clanstva",5} | {"Status",-3} | {"Prosecna ocena",5} |"
            );
            Console.WriteLine(new string('-', 130));

            foreach (Posetilac p in posetioci)
            {
                Console.WriteLine(p);
            }
        }

    }
}
