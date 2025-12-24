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

        public int IDKupovine { get; set; }
        public string BrClanskeKarteKupca { get; set; }
        public string ISBNKnjige { get; set; }

        public Posetilac Kupac { get; set; }
        public Knjiga Knjiga { get; set; }
        public DateTime DatumKupovine { get; set; }
        public double Ocena { get; set; }
        public string Komentar { get; set; }

        public Kupovina()
        {

        }

        public Kupovina(int ID, Posetilac kupac, Knjiga knjiga, DateTime datum,
            double ocena, string komentar)
        {
            IDKupovine = ID;
            Kupac = kupac;
            Knjiga = knjiga;

            //? sprecava NullReferenceException
            BrClanskeKarteKupca = kupac?.BrClanskeKarte;
            ISBNKnjige = knjiga?.ISBN;

            DatumKupovine = datum;
            Ocena = ocena;
            Komentar = komentar;
        }

        public override string ToString()
        {
           string kupacInfo = Kupac != null
                ? $"{Kupac.Ime} {Kupac.Prezime} (BrClanskeKarte: {Kupac.BrClanskeKarte})"
                : "Nepoznat kupac";
            
            string knjigaInfo = Knjiga != null
                ? $"{Knjiga.Naziv} (ISBN: {Knjiga.ISBN})"
                : $"Nepoznata knjiga";
            return
                $"ID Kupovine: {IDKupovine}\n" +
                $"Kupac:  {kupacInfo}\n" +
                $"Knjiga: {knjigaInfo}\n" +
                $"DatumKupovine: {DatumKupovine:dd.MM.yyyy}\n" +
                $"Ocena: {Ocena}\n" +
                $"Komentar: {Komentar}\n";
        }

      
        public void FromCSV(string[] values)
        {
            IDKupovine = int.Parse(values[0]);
            BrClanskeKarteKupca = values[1];
            ISBNKnjige = values[2];
            DatumKupovine = DateTime.Parse(values[3]);
            Ocena = double.Parse(values[4]);
            Komentar = values[5];

        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                IDKupovine.ToString(),
                BrClanskeKarteKupca,
                ISBNKnjige,
                DatumKupovine.ToString("yyyy-MM-dd"),
                Ocena.ToString(),
                Komentar
            };
            return csvValues;
        }

    }
}
