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
            Posetilac p1 = new Posetilac("Milica", "Stankovic", new DateTime(2025,12,22), adresa, "82635287894", "snjfqwojiure", "vsjduer", 2023, StatusPosetioca.P,  4.5);
        }
    }
}
