using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utils;

namespace Core.Models
{
    public class Izdavac : ISerializable
    {
        private int sifra;
        private string naziv;
        private Autor sef;
        private List<Autor> spisakAutora;
        private List<Knjiga> spisakKnjiga;

        public int Sifra
        {
            get { return sifra; }
            set { sifra = value; }
        }

        public string Naziv
        {
            get { return naziv; }
            set { naziv = value; }
        }

        public Autor Sef
        {
            get { return sef; }
            set
            {
                sef = value;
            }
        }

        public List<Autor> SpisakAutora { get; set; }
        public List<Knjiga> SpisakKnjiga { get; set; }

        public Izdavac()
        {
            SpisakAutora = new List<Autor>();
            SpisakKnjiga = new List<Knjiga>();
        }

        public Izdavac(int sifra, string naziv, Autor sef)
        {
            Sifra = sifra;
            Naziv = naziv;
            Sef = sef;  
            SpisakAutora = new List<Autor>();
            SpisakKnjiga = new List<Knjiga>();
        }

        public void FromCSV(string[] values)
        {
            Sifra = int.Parse(values[0]);
            Naziv = values[1];
            if (!string.IsNullOrEmpty(values[2]))
                Sef = new Autor { AutorID = int.Parse(values[2]) };

            SpisakAutora = new List<Autor>();
            if (!string.IsNullOrEmpty(values[3]))
                foreach (var id in values[3].Split(','))
                    SpisakAutora.Add(new Autor { AutorID = int.Parse(id) });

            SpisakKnjiga = new List<Knjiga>();
            if (!string.IsNullOrEmpty(values[4]))
            {
                foreach (var isbn in values[4].Split(','))
                {
                    SpisakKnjiga.Add(new Knjiga { ISBN = isbn });
                }
            }
        }
        public string[] ToCSV()
        {
            string sefId = Sef != null ? Sef.AutorID.ToString() : "";

            string autoriId = SpisakAutora != null && SpisakAutora.Count > 0
                ? string.Join(",", SpisakAutora.Select(a => a.AutorID))
                : "";

            string knjigeIsbn = SpisakKnjiga != null && SpisakKnjiga.Count > 0
                ? string.Join(",", SpisakKnjiga.Select(k => k.ISBN))
                : "";

            return new string[]
            {
                   Sifra.ToString(),
                   Naziv,
                   sefId,
                   autoriId,
                   knjigeIsbn
            };
        }

        public override string ToString()
        {
            string autoriStr = SpisakAutora != null
            ? string.Join(", ", SpisakAutora.Where(a => a != null && !string.IsNullOrEmpty(a.Ime))
            .Select(a => $"{a.Ime} {a.Prezime}"))
            : "";

            string knjigeStr = SpisakKnjiga != null
            ? string.Join(", ", SpisakKnjiga.Where(k => k != null && !string.IsNullOrEmpty(k.Naziv)).Select(k => k.Naziv))
            : "";

            string sefStr = Sef != null ? Sef.Ime : "";

            return $"{Naziv} (ID: {Sifra})\n" +
                   $"   Sef: {sefStr}\n" +
                   $"   Autori: {autoriStr}\n" +
                   $"   Knjige: {knjigeStr}";
        }
    }
}
