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
    internal class AutorDao:IAutorDao
    {
        private readonly List<Autor> _autors;
        private readonly Storage<Autor> _storage;
        //TO DO DODATI ADRESU DAO
        public AutorDao()
        {
            _storage = new Storage<Autor>("autori.csv");
            _autors = _storage.Load();  
        }
        private int GenerateId()
        {
            if (_autors.Count == 0)
                return 1;
            return _autors.Max(a => a.AutorID) + 1;
        }

        public Autor Add(Autor autor)
        {
            autor.AutorID = GenerateId();
            _autors.Add(autor);
            _storage.Save(_autors);
            return autor;
        }

        public Autor Update(Autor autor)
        {
            var existingAutor = GetBySifra(autor.AutorID);
            if (existingAutor == null)
                throw new Exception("Autor not found");
            existingAutor.Ime = autor.Ime;
            existingAutor.Prezime = autor.Prezime;
            existingAutor.DatumRodjenja = autor.DatumRodjenja;
            existingAutor.Adresa = autor.Adresa;
            existingAutor.Telefon = autor.Telefon;
            existingAutor.BrojLicneKarte = autor.BrojLicneKarte;
            existingAutor.GodineIskustva = autor.GodineIskustva;
            existingAutor.Email = autor.Email;
            _storage.Save(_autors);
            return existingAutor;
        }

        public Autor Delete(int sifra)
        {
            var autor = GetBySifra(sifra);
            if (autor == null)
                throw new Exception("Autor not found");
            _autors.Remove(autor);
            _storage.Save(_autors);
            return autor;
        }
        public Autor GetBySifra(int sifra)
        {
            return _autors.FirstOrDefault(a => a.AutorID == sifra);
        }
        public List<Autor> GetAll()
        {
            return _autors;
        }

    }
}
