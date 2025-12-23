using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Core.Utils;
namespace Core.DAO
{
    public class KnjigaDao:IKnjigaDao
    {

        private readonly List<Knjiga> _books;
        private readonly Storage<Knjiga> _storage;

        public KnjigaDao()
        {
            _storage = new Storage<Knjiga>("knjige.csv");
            _books = _storage.Load();
        }

        public Knjiga addKnjiga(Knjiga knjiga)
        {
            if (_books.Any(k => k.ISBN == knjiga.ISBN))
                throw new Exception("Књига са датим ISBN-ом већ постоји.");

            _books.Add(knjiga);
            _storage.Save(_books);
            return knjiga;
        }

        public Knjiga GetByISBN(string isbn)
        {
            foreach (var k in _books)
            {
                if (k.ISBN == isbn)
                    return k;
            }
            return null;
        }

        public Knjiga updateKnjiga(Knjiga knjiga)
        {
            var existingKnjiga = GetByISBN(knjiga.ISBN);
            if (existingKnjiga == null)
                throw new Exception("Knjiga not found");

            existingKnjiga.Naziv = knjiga.Naziv;
            existingKnjiga.Zanr = knjiga.Zanr;
            existingKnjiga.GodinaIzdanja = knjiga.GodinaIzdanja;
            existingKnjiga.Cena = knjiga.Cena;
            existingKnjiga.BrojStrana = knjiga.BrojStrana;
            existingKnjiga.Izdavac = knjiga.Izdavac;

            _storage.Save(_books);
            return existingKnjiga;
        }

        public Knjiga deleteKnjiga(string isbn)
        {
            var knjiga = GetByISBN(isbn);
            if (knjiga == null)
                throw new Exception("Knjiga not found");

            _books.Remove(knjiga);
            _storage.Save(_books);
            return knjiga;
        }

        public List<Knjiga> getAllKnjige()
        {
            return _books;
        }
    }
}
