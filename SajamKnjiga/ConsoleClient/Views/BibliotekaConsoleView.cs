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

        public BibliotekaConsoleView(AutorDao autorDao,AdresaDao adresaDao)
        {
                _autorDao = autorDao;
                _adresaDao = adresaDao;

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


    }
}
