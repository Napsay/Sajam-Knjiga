using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public class DataBinder
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

        private static void PoveziIzdavace(
        List<Izdavac> izdavaci,
        List<Autor> autori,
        List<Knjiga> knjige)
        {
            foreach (var izdavac in izdavaci)
            {
             
                if (izdavac.Sef != null)
                {
                    izdavac.Sef = autori
                        .FirstOrDefault(a => a.AutorID == izdavac.Sef.AutorID);
                }

                for (int i = 0; i < izdavac.SpisakAutora.Count; i++)
                {
                    var autor = autori
                        .FirstOrDefault(a => a.AutorID == izdavac.SpisakAutora[i].AutorID);

                    if (autor != null)
                        izdavac.SpisakAutora[i] = autor;
                }

                for (int i = 0; i < izdavac.SpisakKnjiga.Count; i++)
                {
                    var knjiga = knjige
                        .FirstOrDefault(k => k.ISBN == izdavac.SpisakKnjiga[i].ISBN);

                    if (knjiga != null)
                        izdavac.SpisakKnjiga[i] = knjiga;
                }
            }
        }
    }
}
