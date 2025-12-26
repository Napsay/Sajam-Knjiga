using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utils;
namespace Core.Models
{
    public class AutorKnjiga : ISerializable
    {
        public int AutorId { get; set; }
        public string ISBN { get; set; }

        public AutorKnjiga() { }
        public AutorKnjiga(int autorId, string isbn)
        {
            AutorId = autorId;
            ISBN = isbn;
        }
        public void FromCSV(string[] values)
        {
            AutorId = int.Parse(values[0]);
            ISBN = values[1];
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                AutorId.ToString(),
                ISBN
            };
        }
    }
}
