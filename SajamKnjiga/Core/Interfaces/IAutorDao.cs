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
        Autor Add(Autor autor);
        Autor Update(Autor autor);
        Autor Delete(int sifra);
        Autor GetBySifra(int sifra);
        List<Autor> GetAll();
    }
}
