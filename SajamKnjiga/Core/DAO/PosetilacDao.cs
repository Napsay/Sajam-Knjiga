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
        private int GenerateClanskaKartaNumber()
        {
            int max = 0;

            foreach (var p in _posetioci)
            {
                if (string.IsNullOrWhiteSpace(p.BrClanskeKarte))
                    continue;

                var parts = p.BrClanskeKarte.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[1], out int broj))
                {
                    if (broj > max)
                        max = broj;
                }
            }

            return max + 1;
        }


        private string GenerateClanskaKarta()
        {
            int broj = GenerateClanskaKartaNumber();
            int godina = DateTime.Now.Year;

            return $"CK-{broj}-{godina}";
        }

        public Posetilac AddPosetilac(Posetilac posetilac)
        {
            posetilac.BrClanskeKarte = GenerateClanskaKarta();
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
