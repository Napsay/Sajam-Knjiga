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
            BibliotekaConsoleView view = new BibliotekaConsoleView(autori,adrese);
            view.RunMenu();
        }
    }
}
