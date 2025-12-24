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
        public static void PoveziAutoreIKnjige(List<Autor> autori, List<Knjiga> knjige,List<AutorKnjiga> veze)
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
        private static void PoveziListuZelja(List<Posetilac> posetioci,List<Knjiga> knjige,List<ListaZelja> listaZelja)
        {
            foreach (var veza in listaZelja)
            {
                var posetilac = posetioci.FirstOrDefault(p =>p.BrClanskeKarte == veza.BrClanskeKarte);

                var knjiga = knjige.FirstOrDefault(k => k.ISBN == veza.ISBN);

                if (posetilac != null && knjiga != null)
                {
                    knjiga.PosetiociListaZelja.Add(posetilac);
                }
            }
        }
        public static void PoveziPosetioceIKnjige(List<Posetilac> posetioci,List<Knjiga> knjige,List<Kupovina> kupovine, List<ListaZelja> listaZelja)
        {
            foreach (var kupovina in kupovine)
            {
                var posetilac = posetioci.FirstOrDefault(p => p.BrClanskeKarte == kupovina.BrClanskeKarteKupca);

                var knjiga = knjige.FirstOrDefault(k => k.ISBN == kupovina.ISBNKnjige);

                if (posetilac != null && knjiga != null)
                {
                    kupovina.Kupac = posetilac;
                    kupovina.Knjiga = knjiga;
                    posetilac.KupljeneKnjige.Add(kupovina);
                }
            }
            foreach (var veza in listaZelja)
            {
                var posetilac = posetioci.FirstOrDefault(p => p.BrClanskeKarte == veza.BrClanskeKarte);

                var knjiga = knjige.FirstOrDefault(k => k.ISBN == veza.ISBN);

                if (posetilac != null && knjiga != null)
                {
                    knjiga.PosetiociListaZelja.Add(posetilac);
                }
            }
        }
    }
}
