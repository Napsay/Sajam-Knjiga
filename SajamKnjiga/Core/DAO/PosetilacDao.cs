using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public  class PosetilacDao : IPosetilacDao
    {
        private readonly List<Posetilac> _posetioci;
        private readonly Storage<Posetilac> _storage;

        public PosetilacDao()
        {
            _storage = new Storage<Posetilac>("posetioci.csv");
            _posetioci = _storage.Load();
        }
        

        public  Posetilac AddPosetilac(Posetilac posetilac)
        {
            if (_posetioci.Any(p => p.BrClanskeKarte == posetilac.BrClanskeKarte))
                throw new Exception("Posetilac sa ovom članskom kartom već postoji.");

            _posetioci.Add(posetilac);
            _storage.Save(_posetioci);
            return posetilac;

        }

        public Posetilac UpdatePosetilac(Posetilac posetilac)
        {
            var existing = GetByBrClanskeKarte(posetilac.BrClanskeKarte);
            if (existing == null)
                throw new Exception("Posetilac not found");

            existing.Ime = posetilac.Ime;
            existing.Prezime = posetilac.Prezime;
            existing.DatumRodjenja = posetilac.DatumRodjenja;
            existing.Adresa = posetilac.Adresa;
            existing.Telefon = posetilac.Telefon;
            existing.Email = posetilac.Email;
            existing.TrenutnaGodClanstva = posetilac.TrenutnaGodClanstva;
            existing.Status = posetilac.Status;
            existing.ProsecnaOcenaRec = posetilac.ProsecnaOcenaRec;
            

            _storage.Save(_posetioci);
            return existing;
        }
        public Posetilac DeletePosetilac(string BrClanskeKarte)
        {
            var existing = GetByBrClanskeKarte(BrClanskeKarte);
            if (existing == null)
                throw new Exception("Posetilac not found");

            _posetioci.Remove(existing);
            _storage.Save(_posetioci);
            return existing;

        }

        public Posetilac GetByBrClanskeKarte(string brClanskeKarte)
        {
            return _posetioci.FirstOrDefault(p => p.BrClanskeKarte == brClanskeKarte);
        }

        public List<Posetilac> GetAllPosetilac()
        {
            return _posetioci;
        }

    }
}
