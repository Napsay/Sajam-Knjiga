using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    internal class Knjiga
    {
        private string isbn;
        private string naziv;
        private string zanr;
        private int godinaIzdanja;
        private int cena;
        private int brojStrana;
        private List<string> autori;
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


        public List<string> Autori
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
            Autori = new List<string>();
            Posetioci = new List<Posetilac>();
            PosetiociListaZelja = new List<Posetilac>();
             
        }

    }
}
