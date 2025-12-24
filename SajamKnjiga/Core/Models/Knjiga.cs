using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utils;

namespace Core.Models
{
    public class Knjiga:ISerializable
    {
       
        private string isbn;
        private string naziv;
        private string zanr;
        private int godinaIzdanja;
        private int cena;
        private int brojStrana;
        private List<Autor> autori;
        private string izdavac;
        private List<Posetilac> posetioci;
        private List<Posetilac> posetiociListaZelja;

        public List<Posetilac> PosetiociListaZelja
        {
            get { return posetiociListaZelja; }
            set { posetiociListaZelja = value; }
        }


        public List<Posetilac> Posetioci
        {
            get { return posetioci; }
            set { posetioci = value; }
        }

        public string Izdavac
        {
            get { return izdavac; }
            set { izdavac = value; }
        }


        public List<Autor> Autori
        {
            get { return autori; }
            set { autori = value; }
        }

        public int BrojStrana
        {
            get { return brojStrana; }
            set { brojStrana = value; }
        }

        public int Cena
        {
            get { return cena; }
            set { cena = value; }
        }


        public int GodinaIzdanja
        {
            get { return godinaIzdanja; }
            set { godinaIzdanja = value; }
        }

        public string Zanr
        {
            get { return zanr; }
            set { zanr = value; }
        }


        public string Naziv
        {
            get { return naziv; }
            set { naziv = value; }
        }

        public string ISBN
        {
            get { return isbn; }
            set { isbn = value; }
        }

        public Knjiga()
        {
            Autori = new List<Autor>();
            Posetioci = new List<Posetilac>();
            PosetiociListaZelja = new List<Posetilac>();
        }
        public Knjiga(string isbn,string naziv,string zanr,int godinaIzdavanja,int cena,int brojStrana,string izdavac)
        {
            ISBN = isbn;
            Naziv = naziv;
            Zanr = zanr;
            GodinaIzdanja = godinaIzdavanja;
            Cena = cena;
            BrojStrana = brojStrana;
            Izdavac = izdavac;
            Autori = new List<Autor>();
            Posetioci = new List<Posetilac>();
            PosetiociListaZelja = new List<Posetilac>();
             
        }

        public void FromCSV(string[] values)
        {
            isbn = values[0];
            naziv = values[1];
            zanr = values[2];
            godinaIzdanja = int.Parse(values[3]);
            cena = int.Parse(values[4]);
            brojStrana = int.Parse(values[5]);

            autori = new List<Autor>();

            izdavac = values[6];

            posetioci = new List<Posetilac>();
            posetiociListaZelja = new List<Posetilac>();

          
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                isbn,
                naziv,
                zanr,
                godinaIzdanja.ToString(),
                cena.ToString(),
                brojStrana.ToString(),
                izdavac
            };
            return csvValues;
        }

        public override string ToString()
        {
            return
                $"{isbn,-15} | " +
                $"{naziv,-25} | " +
                $"{zanr,-15} | " +
                $"{godinaIzdanja,4} | " +
                $"{cena,6} | " +
                $"{brojStrana,5} | " +
                $"{izdavac,-20} |";
        }

    }
}
