using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    internal interface IPosetilacDao
    {
        List<Posetilac> GetAll();

        Posetilac Add(Posetilac posetilac);

        Posetilac Update(Posetilac posetilac);
        Posetilac Delete(string BrClanskeKarte);
    }
}
