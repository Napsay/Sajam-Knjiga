using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    internal interface IKnjigaDao
    {
        Knjiga addKnjiga(Knjiga knjiga);
        Knjiga deleteKnjiga(string knjigaId);
        Knjiga updateKnjiga(Knjiga knjiga);
        List<Knjiga> getAllKnjige();
        Knjiga GetByISBN(string knjigaISBN);
    }
}
