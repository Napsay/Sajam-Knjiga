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
            AdresaDao adrese = new AdresaDao();
            AutorDao autori = new AutorDao(adrese);
            PosetilacDao posetioci = new PosetilacDao(adrese);
            IzdavacDao izdavaci = new IzdavacDao(adrese);
            KnjigaDao knjige = new KnjigaDao(izdavaci);
            AutorKnjigaDao autorKnjiga = new AutorKnjigaDao();
            ListaZeljaDao listaZelja = new ListaZeljaDao();
            



            KupovinaDao kupovine = new KupovinaDao();
            DataBinder.PoveziSve(
               autori.GetAll(),
               knjige.getAllKnjige(),
               autorKnjiga.GetAll(),
               posetioci.GetAllPosetilac(),
               kupovine.getAllKupovine(),
               listaZelja.GetAll(),
               izdavaci.GetAll(),
               adrese.GetAll()
           );



            BibliotekaConsoleView view = new BibliotekaConsoleView(autori,adrese, knjige, posetioci, kupovine,izdavaci,listaZelja, autorKnjiga);
            view.RunMenu();
        }
    }
}
