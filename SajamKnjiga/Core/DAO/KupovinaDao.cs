using Core.Models;
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
    public class KupovinaDao : IKupovinaDao
    {

        private readonly List<Kupovina> _purchases;
        private readonly Storage<Kupovina> _storage;

        public KupovinaDao()
        {
            _storage = new Storage<Kupovina>("kupovine.csv");
            _purchases = _storage.Load();
        }

        private int GenerateId()
        {
            if (_purchases.Count == 0)
                return 1;
            return _purchases.Max(p => p.IDKupovine) + 1;
        }

        public Kupovina addKupovina(Kupovina kupovina)
        {
            kupovina.IDKupovine = GenerateId();
            _purchases.Add(kupovina);
            _storage.Save(_purchases);
            return kupovina;
        }

        public Kupovina GetById(int kupovinaId)
        {
            foreach (var k in _purchases)
            {
                if(k.IDKupovine == kupovinaId)
                    return k;
            }
            return null;
        }

        public  Kupovina deleteKupovina(int kupovinaId)
        {
            var kupovina  = GetById(kupovinaId);
            if (kupovina == null)
                throw new Exception("There is no purchase with that id!");

            _purchases.Remove(kupovina);
            _storage.Save(_purchases);
            return kupovina;
        }
        public Kupovina updateKupovina(Kupovina kupovina)
        {
            var existingKupovina = GetById(kupovina.IDKupovine);
            if (existingKupovina == null)
                throw new Exception("Kupovina not found");

            existingKupovina.Kupac = kupovina.Kupac;
            existingKupovina.Knjiga = kupovina.Knjiga;
            existingKupovina.DatumKupovine = kupovina.DatumKupovine;
            existingKupovina.Ocena = kupovina.Ocena;
            existingKupovina.Komentar = kupovina.Komentar;

            _storage.Save(_purchases);
            return existingKupovina;
        }
        public List<Kupovina> getAllKupovine()
        {
            return _purchases; 
        }
        
    }
}
