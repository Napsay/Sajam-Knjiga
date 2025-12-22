using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{

    public enum StatusPosetioca
    {
        P,  //Redovan
        B  //VIP
    }

    public class Posetilac
    {

        private string ime;
        private string prezime;
        private DateTime datumRodjenja;
        private string adresa;
        private string telefon;
        private string email;
        private string brClanskeKarte;
        private int trenutnaGodClanstva;
        private StatusPosetioca status;
        private double prosecnaOcenaRec;
        private List<Kupovina> kupljeneKnjige;
        private List<Knjiga> listaZelja;


        public string Ime
        {
            get { return ime; }
            set { ime = value; }
        }


        public string Prezime
        {
            get { return prezime; }
            set { prezime = value; }
        }


        public DateTime DatumRodjenja
        {
            get { return datumRodjenja; }
            set { datumRodjenja = value; }
        }

        public string Adresa
        {
            get { return adresa; }
            set { adresa = value; }
        }

        public string Telefon
        {
            get { return telefon; }
            set { telefon = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string BrClanskeKarte
        {
            get { return brClanskeKarte; }
            set { brClanskeKarte = value; }
        }

        public int TrenutnaGodClanstva
        {
            get { return trenutnaGodClanstva; }
            set { trenutnaGodClanstva = value; }
        }

        public StatusPosetioca Status
        {
            get { return status; }
            set { status = value; }
        }

        public double ProsecnaOcenaRec
        {
            get { return prosecnaOcenaRec; }
            set { prosecnaOcenaRec = value; }
        }

        public List<Kupovina> KupljeneKnjige { get; set; }

        public List<Knjiga> ListaZelja { get; set; }


        public Posetilac()
        {

        }

        public Posetilac(string ime, string prezime, DateTime datumRodjenja,
            string adresa, string telefon, string email, string brClanskeKarte,
            int trenutnaGodClanstva, StatusPosetioca status, double prosecnaOcenaRec)

        {
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            Adresa = adresa;
            Telefon = telefon;
            Email = email;
            BrClanskeKarte = brClanskeKarte;
            TrenutnaGodClanstva = trenutnaGodClanstva;
            Status = status;
            ProsecnaOcenaRec = prosecnaOcenaRec;
            KupljeneKnjige = new List<Kupovina>();
            ListaZelja = new List<Knjiga>();
        }

    }
}
