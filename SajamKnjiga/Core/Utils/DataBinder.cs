using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public  class DataBinder
    {
        public static void PoveziAutoreIKnjige(
        List<Autor> autori,
        List<Knjiga> knjige,
        List<AutorKnjiga> veze)
        {
            foreach (var veza in veze)
            {
                var autor = autori.FirstOrDefault(a => a.AutorID == veza.AutorId);
                var knjiga = knjige.FirstOrDefault(k => k.ISBN == veza.ISBN);

                if (autor != null && knjiga != null)
                {
                    autor.Knjige.Add(knjiga);
                }
            }
        }
    }
}
