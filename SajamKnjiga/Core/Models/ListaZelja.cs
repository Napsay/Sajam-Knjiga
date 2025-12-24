using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utils;
namespace Core.Models
{
    public class ListaZelja:ISerializable
    {
        public string BrClanskeKarte { get; set; }
        public string ISBN { get; set; }

        public ListaZelja() { }

        public ListaZelja(string brClanskeKarte, string isbn)
        {
            BrClanskeKarte = brClanskeKarte;
            ISBN = isbn;
        }

        public void FromCSV(string[] values)
        {
            BrClanskeKarte = values[0];
            ISBN = values[1];
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                BrClanskeKarte,
                ISBN
            };
        }
    }
}
