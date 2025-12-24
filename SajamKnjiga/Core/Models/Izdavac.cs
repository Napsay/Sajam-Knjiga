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

            // Šef (samo ID)
            if (!string.IsNullOrEmpty(values[2]))
                Sef = new Autor { AutorID = int.Parse(values[2]) };

            // Autori (samo ID-jevi)
            SpisakAutora = new List<Autor>();
            if (!string.IsNullOrEmpty(values[3]))
            {
                foreach (var id in values[3].Split(','))
                    SpisakAutora.Add(new Autor { AutorID = int.Parse(id) });
            }

            // Knjige (samo ISBN)
            SpisakKnjiga = new List<Knjiga>();
            if (!string.IsNullOrEmpty(values[4]))
            {
                foreach (var isbn in values[4].Split(','))
                    SpisakKnjiga.Add(new Knjiga { ISBN = isbn });
            }
        }
        public string[] ToCSV()
        {
            string sefId = "";
            if (sef != null)
                sefId = sef.AutorID.ToString();

            string autorsId = "";
            if (spisakAutora != null && spisakAutora.Count > 0)
                autorsId = string.Join(",", spisakAutora.Select(a => a.AutorID));
            string knjigeIsbn = "";
            if (spisakKnjiga != null && spisakKnjiga.Count > 0)
                knjigeIsbn = string.Join(",", spisakKnjiga.Select(k => k.ISBN));

            return new string[]
                {
                    sifra.ToString(),
                    naziv,
                    sefId,
                    autorsId,
                    knjigeIsbn
                };

        }


        public override string ToString()
        {
            string sefID = "-";
            if (Sef != null)
                sefID = Sef.AutorID.ToString();

            string autori = "-";
            if (spisakAutora != null && spisakAutora.Count > 0)
                autori = string.Join(", ", spisakAutora.Select(a => a.AutorID));
            string knjige = "-";
            if (spisakKnjiga != null && spisakKnjiga.Count > 0)
                knjige = string.Join(", ", spisakKnjiga.Select(k => k.ISBN));


            return
                $"{Sifra,-6} | " +
                $"{Naziv,-20} | " +
                $"{sefID,-15} | " +
                $"{autori,-20} | " +
                $"{knjige,-30} |";
        }
    }
}
