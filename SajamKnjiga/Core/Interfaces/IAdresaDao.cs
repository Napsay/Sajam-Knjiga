using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    internal interface IAdresaDao
    {
        Adresa Add(Adresa adresa);
        Adresa Update(Adresa adresa);
        Adresa Delete(int sifra);
        Adresa GetBySifra(int sifra);
        List<Adresa> GetAll();

    }
}
