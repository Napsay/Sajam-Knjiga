using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
namespace ConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Adresa adresa = new Adresa("Bulevar Kralja Aleksandra", 73, "Beograd", "Srbija");
            Console.WriteLine(adresa);
        }
    }
}
