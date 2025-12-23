using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    internal interface IIzdavacDao
    {
        Izdavac Add(Izdavac izdavac);
        Izdavac Update(Izdavac izdavac);
        Izdavac Delete(int sifra);
        Izdavac GetBySifra(int sifra);
        List<Izdavac> GetAll();
        
        
    }
}
