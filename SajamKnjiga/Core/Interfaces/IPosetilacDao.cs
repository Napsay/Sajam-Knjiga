using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IPosetilacDao
    {
        List<Posetilac> GetAllPosetilac();

        Posetilac AddPosetilac(Posetilac posetilac);

        Posetilac UpdatePosetilac(Posetilac posetilac);
        Posetilac DeletePosetilac(string BrClanskeKarte);
    }
}
