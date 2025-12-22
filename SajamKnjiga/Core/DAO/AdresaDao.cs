using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Core.Utils;

namespace Core.DAO
{
    internal class AdresaDao:IAdresaDao
    {
        private readonly List<Adresa> _addresses;
        private readonly Storage<Adresa> _storage;

        public AdresaDao(Storage<Adresa> storage)
        {
            _storage = new Storage<Adresa>("addresses.txt");
            _addresses = _storage.Load(); _storage = storage;
        }

        private int generateId()
        {
            if (_addresses.Count == 0)
                return 1;
            return _addresses.Max(a => a.Sifra) + 1;
        }

        public Adresa Add(Adresa address)
        {
            address.Sifra = generateId();
            _addresses.Add(address);
            _storage.Save(_addresses);
            return address;
        }

        public Adresa GetBySifra(int sifra)
        {
            return _addresses.FirstOrDefault(a => a.Sifra == sifra);
        }
        public Adresa Update(Adresa address)
        {
            var existingAddress = GetBySifra(address.Sifra);
            if (existingAddress == null)
                throw new Exception("Address not found");
            existingAddress.Ulica = address.Ulica;
            existingAddress.Broj = address.Broj;
            existingAddress.Grad = address.Grad;
            existingAddress.Drzava = address.Drzava;
            
            _storage.Save(_addresses);
            return existingAddress;
        }

        public Adresa Delete(int sifra)
        {
            var address = GetBySifra(sifra);
            if (address == null)
                throw new Exception("Address not found");
            _addresses.Remove(address);
            _storage.Save(_addresses);
            return address;
        }
        public List<Adresa> GetAll()
        {
            return _addresses;
        }
    }
}
