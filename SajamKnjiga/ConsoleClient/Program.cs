using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleClient.Views;
using Core.DAO;
using Core.Models;
namespace ConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AutorDao autori = new AutorDao();
            AdresaDao adrese = new AdresaDao();
            KnjigaDao knjige = new KnjigaDao();
            PosetilacDao posetioci = new PosetilacDao();
            IzdavacDao izdavaci = new IzdavacDao();

            // Initialize KupovinaDao with references to PosetilacDao and KnjigaDao
            KupovinaDao kupovine = new KupovinaDao(posetioci, knjige);
         
            BibliotekaConsoleView view = new BibliotekaConsoleView(autori,adrese, knjige, posetioci, kupovine,izdavaci);
            view.RunMenu();
        }
    }
}
