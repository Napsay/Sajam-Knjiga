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
            KupovinaDao kupovine = new KupovinaDao();
            IzdavacDao izdavaci = new IzdavacDao();
            BibliotekaConsoleView view = new BibliotekaConsoleView(autori,adrese, knjige, posetioci, kupovine,izdavaci);
            view.RunMenu();
        }
    }
}
