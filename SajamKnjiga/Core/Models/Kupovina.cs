using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{ 
    internal class Kupovina
    {

        private Posetilac kupac;
        private Knjiga knjiga;
        private DateTime datumKupovine;
        private double ocena;
        private string komentar;

        public Posetilac Kupac
        {
            get { return kupac; }
            set { kupac = value; }
        }

        public Knjiga Knjiga
        {
            get { return knjiga; }
            set { knjiga = value; }
        }

        public DateTime DatumKupovine
        {
            get { return datumKupovine; }
            set { datumKupovine = value; }
        }

        public double Ocena
        {
            get { return ocena; }
            set
            {
                if (value < 1 || value > 5)
                    throw new ArgumentOutOfRangeException(nameof(value), "Ocena mora biti izmedju 1 i 5.");
                ocena = value;
            }
        }

        public string Komentar
        {
            get { return komentar; }
            set { komentar = value; }
        }

        public Kupovina()
        {

        }

        public Kupovina(Posetilac kupac, Knjiga knjiga, DateTime datum,
            double ocena, string komentar)
        {
            Kupac = kupac;
            Knjiga = knjiga;
            DatumKupovine = datum;
            Ocena = ocena;
            Komentar = komentar;
        }

    }
}
