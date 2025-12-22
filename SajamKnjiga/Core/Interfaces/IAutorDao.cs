using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    internal interface IAutorDao
    {
        void Add(Autor autor);
        void Update(Autor autor);
        void Delete(int sifra);
        Autor GetBySifra(int sifra);
        List<Autor> GetAll();
    }
}
