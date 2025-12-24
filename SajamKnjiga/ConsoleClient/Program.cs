using ConsoleClient.Views;
using Core.DAO;
using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            AutorKnjigaDao autorKnjiga = new AutorKnjigaDao();
            ListaZeljaDao listaZelja = new ListaZeljaDao();


            KupovinaDao kupovine = new KupovinaDao(posetioci, knjige);
            DataBinder.PoveziAutoreIKnjige(
            autori.GetAll(),
            knjige.getAllKnjige(),
            autorKnjiga.GetAll()
            );

            

            BibliotekaConsoleView view = new BibliotekaConsoleView(autori,adrese, knjige, posetioci, kupovine,izdavaci,listaZelja);
            view.RunMenu();
        }
    }
}
