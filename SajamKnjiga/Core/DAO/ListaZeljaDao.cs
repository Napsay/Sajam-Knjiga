using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO
{
    public class ListaZeljaDao
    {
        private readonly List<ListaZelja> _listaZelja;
        private readonly Storage<ListaZelja> _storage;

        public ListaZeljaDao()
        {
            _storage = new Storage<ListaZelja>("lista_zelja.csv");
            _listaZelja = _storage.Load();
        }

        public List<ListaZelja> GetAll()
        {
            return _listaZelja;
        }

        public void Add(ListaZelja veza)
        {
            if (_listaZelja.Any(l =>
                l.BrClanskeKarte == veza.BrClanskeKarte &&
                l.ISBN == veza.ISBN))
                return;

            _listaZelja.Add(veza);
            _storage.Save(_listaZelja);
        }

        public void Remove(string brClanskeKarte, string isbn)
        {
            var veza = _listaZelja.FirstOrDefault(l =>l.BrClanskeKarte == brClanskeKarte && l.ISBN == isbn);

            if (veza != null)
            {
                _listaZelja.Remove(veza);
                _storage.Save(_listaZelja);
            }
        }
        //metoda koja vraca sve knjige koje su na listi zelja datog posetioca
        public List<string> GetKnjigeZaPosetioca(string brClanskeKarte)
        {
            // Vraca sve ISBN-ove knjiga za datog posetioca
            return _listaZelja
                .Where(l => l.BrClanskeKarte == brClanskeKarte)
                .Select(l => l.ISBN)
                .ToList();
        }

    }
}
