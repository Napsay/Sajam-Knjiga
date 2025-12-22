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
        void addKnjiga(Knjiga knjiga);
        void deleteKnjiga(string knjigaId);
        void updateKnjiga(Knjiga knjiga);
        List<Knjiga> getAllKnjige();
        Knjiga GetKnjiga(string knjigaId);
    }
}
