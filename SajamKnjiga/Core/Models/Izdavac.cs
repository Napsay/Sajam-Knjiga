using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    internal class Izdavac
    {

        private int sifra;
        private string naziv;
        private Autor sef;
        private List<Autor> spisakAutora;
        private List<Autor> spisakKnjiga;

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
                if (value.GodineIskustva < 5)
                    throw new ArgumentOutOfRangeException("Sef mora imati najmanje 5 godina iskustva.");
                sef = value;
            }
        }

        public List<Autor> SpisakAutora { get; set; }
        public List<Autor> SpisakKnjiga { get; set; }

        public Izdavac()
        {

        }

        public Izdavac(int sifra, string naziv, Autor sef)
        {
            Sifra = sifra;
            Naziv = naziv;
            Sef = sef;   //seter proverava godine iskustva
            SpisakAutora = new List<Autor>();
            SpisakKnjiga = new List<Knjiga>();
        }


    }
}
