using Core.DAO;
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


        private readonly AdresaDao adresaDao = new AdresaDao();
        private readonly PosetilacDao posetilacDao;
        private readonly KupovinaDao kupovinaDao = new KupovinaDao();
        private readonly ListaZeljaDao listaZeljaDao = new ListaZeljaDao();
        private readonly IzdavacDao izdavacDao;
        private readonly AutorDao autorDao;

        public DataBinder()
        {
            autorDao = new AutorDao(adresaDao);
            izdavacDao = new IzdavacDao(adresaDao);
            posetilacDao = new PosetilacDao(adresaDao);
        }

        public static void PoveziSve(List<Autor> autori, List<Knjiga> knjige, List<AutorKnjiga> autorKnjige, List<Posetilac> posetioci, List<Kupovina> kupovine, List<ListaZelja> listaZelja, List<Izdavac> izdavaci, List<Adresa> adrese)
        {
            PoveziAutoreIAdrese(autori, adrese);
            PoveziPosetioceIAdrese(posetioci, adrese);
            PoveziAutoreIKnjige(autori, knjige, autorKnjige);
            PoveziKupovine(posetioci, knjige, kupovine);
            PoveziListuZelja(posetioci, knjige, listaZelja);
            PoveziIzdavace(izdavaci, autori, knjige);
        }

        private static void PoveziAutoreIAdrese(List<Autor> autori, List<Adresa> adrese)
        {
            foreach (var autor in autori)
            {
                if (autor.Adresa != null)
                {
                    autor.Adresa = adrese.FirstOrDefault(a => a.Sifra == autor.Adresa.Sifra);
                }
            }
        }
        private static void PoveziKupovine(List<Posetilac> posetioci, List<Knjiga> knjige, List<Kupovina> kupovine)
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
                    knjiga.Posetioci.Add(posetilac);
                }
            }
        }
        private static void PoveziPosetioceIAdrese(List<Posetilac> posetioci, List<Adresa> adrese)
        {
            foreach (var posetilac in posetioci)
            {
                if (posetilac.Adresa != null)
                {
                    posetilac.Adresa = adrese.FirstOrDefault(a => a.Sifra == posetilac.Adresa.Sifra);
                }
            }
        }
        public static void PoveziAutoreIKnjige(List<Autor> autori, List<Knjiga> knjige, List<AutorKnjiga> veze)
        {
            foreach (var veza in veze)
            {
                var autor = autori.FirstOrDefault(a => a.AutorID == veza.AutorId);
                var knjiga = knjige.FirstOrDefault(k => k.ISBN == veza.ISBN);

                if (autor != null && knjiga != null)
                {
                    autor.Knjige.Add(knjiga);
                    knjiga.Autori.Add(autor);
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
                    if (!posetilac.ListaZelja.Contains(knjiga))
                        posetilac.ListaZelja.Add(knjiga);

                    if (!knjiga.PosetiociListaZelja.Contains(posetilac))
                        knjiga.PosetiociListaZelja.Add(posetilac);
                }
            }
        }
        public static void PoveziPosetioceIKnjige(List<Posetilac> posetioci, List<Knjiga> knjige, List<Kupovina> kupovine, List<ListaZelja> listaZelja)
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
        }

        private static void PoveziIzdavace(List<Izdavac> izdavaci, List<Autor> autori, List<Knjiga> knjige)
        {
            foreach (var izdavac in izdavaci)
            {
                if (izdavac.Sef != null)
                {
                    izdavac.Sef = autori.FirstOrDefault(a => a.AutorID == izdavac.Sef.AutorID);
                }

                if (izdavac.SpisakAutora != null)
                {
                    for (int i = 0; i < izdavac.SpisakAutora.Count; i++)
                    {
                        var autor = autori.FirstOrDefault(a => a.AutorID == izdavac.SpisakAutora[i].AutorID);
                        if (autor != null)
                            izdavac.SpisakAutora[i] = autor;
                    }
                }

                izdavac.SpisakKnjiga = new List<Knjiga>();
            }
            
         
            foreach (var knjiga in knjige)
            {
                if (knjiga.Izdavac != null && int.TryParse(knjiga.Izdavac.Sifra.ToString(), out int sifraIzdavaca))
                {
                    var izdavac = izdavaci.FirstOrDefault(i => i.Sifra == sifraIzdavaca);
                    if (izdavac != null)
                    {
                        knjiga.Izdavac = izdavac;
                        if (izdavac.SpisakKnjiga == null)
                            izdavac.SpisakKnjiga = new List<Knjiga>();

                        if (!izdavac.SpisakKnjiga.Contains(knjiga))
                            izdavac.SpisakKnjiga.Add(knjiga);
                    }
                }
            }
        }


    }
}
