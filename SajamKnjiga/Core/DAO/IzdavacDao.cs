using Core;
using Core.Interfaces;
using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Core.DAO
{
    public class IzdavacDao : IIzdavacDao
    {
        private readonly List<Izdavac> _izdavaci;
        private readonly Storage<Izdavac> _storage;
        private readonly AdresaDao _adresaDao;

        private readonly AutorDao _autorDao;
        private readonly KnjigaDao _knjigaDao;
        

        public IzdavacDao(AdresaDao adresaDao)
        {
            _adresaDao = adresaDao;
            _storage = new Storage<Izdavac>("izdavaci.csv");
            _izdavaci = _storage.Load();

            _autorDao = new AutorDao(_adresaDao);

            PoveziAutore();
        }

       

        public void AddBookToIzdavac(Knjiga knjiga)
        {
            if (knjiga.Izdavac == null)
                throw new ArgumentException("Knjiga nema dodeljenog izdavača.");
            var izdavac = _izdavaci.FirstOrDefault(i => i.Sifra == knjiga.Izdavac.Sifra);
            if (izdavac != null)
            {
                if (!izdavac.SpisakKnjiga.Contains(knjiga))
                    izdavac.SpisakKnjiga.Add(knjiga);

                _storage.Save(_izdavaci);
            }
        }
        private int GenerateId()
        {
            if (_izdavaci.Count == 0)
                return 1;

            return _izdavaci.Max(i => i.Sifra) + 1;
        }

        public Izdavac Add(Izdavac izdavac)
        {
            izdavac.Sifra = GenerateId();
            _izdavaci.Add(izdavac);
            _storage.Save(_izdavaci);
            return izdavac;
        }

        public Izdavac Update(Izdavac izdavac)
        {
            var existing = GetBySifra(izdavac.Sifra);
            if (existing == null)
                throw new Exception("Izdavac not found");

            existing.Naziv = izdavac.Naziv;
            existing.Sef = izdavac.Sef;
            existing.SpisakAutora = izdavac.SpisakAutora;
            existing.SpisakKnjiga = izdavac.SpisakKnjiga;

            _storage.Save(_izdavaci);
            return existing;
        }

        public Izdavac Delete(int sifra)
        {
            var izdavac = GetBySifra(sifra);
            if (izdavac == null)
                throw new Exception("Izdavac not found");

            _izdavaci.Remove(izdavac);
            _storage.Save(_izdavaci);
            return izdavac;
        }

        public Izdavac GetBySifra(int sifra)
        {
            return _izdavaci.FirstOrDefault(i => i.Sifra == sifra);
        }

        public List<Izdavac> GetAll()
        {
            return _izdavaci;
        }

        public void SaveAll()
        {
            _storage.Save(_izdavaci);
        }

        private void PoveziAutore()
        {
            foreach (var izdavac in _izdavaci)
            {
                if (izdavac.Sef != null)
                {
                    var praviAutor = _autorDao.GetBySifra(izdavac.Sef.AutorID);
                    izdavac.Sef = praviAutor;
                }

                for (int i = 0; i < izdavac.SpisakAutora.Count; i++)
                {
                    var praviAutor = _autorDao.GetBySifra(izdavac.SpisakAutora[i].AutorID);
                    izdavac.SpisakAutora[i] = praviAutor;
                }
            }
        }
    }
}
