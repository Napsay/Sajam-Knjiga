using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utils;

namespace Core.Models
{
    public class Kupovina: ISerializable
    {

        private int idKupovine;
        private Posetilac kupac;
        private Knjiga knjiga;
        private DateTime datumKupovine;
        private double ocena;
        private string komentar;

        public int IDKupovine
        {
            get { return idKupovine; }
            set { idKupovine = value; }
        }
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

        public Kupovina(int ID, Posetilac kupac, Knjiga knjiga, DateTime datum,
            double ocena, string komentar)
        {
            IDKupovine = ID;
            Kupac = kupac;
            Knjiga = knjiga;
            DatumKupovine = datum;
            Ocena = ocena;
            Komentar = komentar;
        }

        public override string ToString()
        {
            return $"ID Kupovine: {IDKupovine}\n" +
                $"Kupac: {Kupac.Ime} {Kupac.Prezime}, Broj clanske karte: {Kupac.BrClanskeKarte}\n" +
                $"Knjiga: {Knjiga.Naziv}, ISBN: {Knjiga.ISBN}\n" +
                $"Datum Kupovine: {DatumKupovine.ToShortDateString()}\n" +
                $"Ocena: {Ocena}\n" +
                $"Komentar: {Komentar}\n";
        }

        /*
         * Pretpostavljam da CSV fajl izgleda ovako:
         * 0 - ID Kupovine
          1 - Ime kupca
          2 - Prezime kupca
          3 - Broj članske karte
          4 - Naziv knjige
          5 - ISBN
          6 - Datum kupovine
          7 - Ocena
          8 - Komentar*/
        public void FromCSV(string[] values)
        {
            if (values == null || values.Length < 9)
                throw new ArgumentException("Nedovoljno podataka za kreiranje Kupovine iz CSV.");


            Posetilac p = new Posetilac();
            p.Ime = values[1];
            p.Prezime = values[2];
            p.BrClanskeKarte = values[3];

            Knjiga k = new Knjiga();
            k.Naziv = values[4];
            k.ISBN = values[5];

            DateTime datum;
            if (!DateTime.TryParse(values[6], out datum))
                throw new FormatException("Neispravan format datuma u CSV podacima.");

            double parsedOcena;
            if (!double.TryParse(values[7], out parsedOcena))
                throw new FormatException("Neispravan format ocene u CSV podacima.");

            IDKupovine = int.Parse(values[0]);
            Kupac = p;
            Knjiga = k;
            DatumKupovine = datum;
            Ocena = parsedOcena;
            Komentar = values[8];
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                IDKupovine.ToString(),
                Kupac.Ime,
                Kupac.Prezime,
                Kupac.BrClanskeKarte,
                Knjiga.Naziv,
                Knjiga.ISBN,
                DatumKupovine.ToString("yyyy-MM-dd"),
                Ocena.ToString(),
                Komentar
            };
            return csvValues;
        }

    }
}
