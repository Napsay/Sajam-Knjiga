using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Models
{
    internal class Autor:ISerializable
    {
        private int autorID;
        private string ime;
        private string prezime;
        private DateTime datumRodjenja;
        private Adresa adresa;
        private string telefon;
        private string brojLicneKarte;
        private int godineIskustva;
        private string email;

        public void FromCSV(string[] values)
        {
            autorID = int.Parse(values[0]);
            Ime = values[1];
            prezime = values[2];
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
            autorID.ToString(),
            ime,prezime
            };

            return csvValues;
        }
        public int AutorID
        {
            get { return autorID; }
            set { autorID = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public List<Knjiga> Knjige { get; set; }
        public int GodineIskustva
        {
            get { return godineIskustva; }
            set { godineIskustva = value; }
        }

        public string BrojLicneKarte
        {
            get { return brojLicneKarte; }
            set { brojLicneKarte = value; }
        }

        public string Telefon
        {
            get { return telefon; }
            set { telefon = value; }
        }

        public Adresa Adresa
        {
            get { return adresa; }
            set { adresa = value; }
        }

        public DateTime DatumRodjenja
        {
            get { return datumRodjenja; }
            set { datumRodjenja = value; }
        }



        public string Prezime
        {
            get { return prezime; }
            set { prezime = value; }
        }


        public string Ime
        {
            get { return ime; }
            set { ime = value; }
        }
        public Autor()
        {
            Knjige = new List<Knjiga>();
        }
        public Autor(int id,string ime, string prezime, DateTime datumRodjenja,
                 Adresa adresa, string telefon, string email,
                 string brojLicneKarte, int godineIskustva)
        {
            AutorID = id;
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            Adresa = adresa;
            Telefon = telefon;
            Email = email;
            BrojLicneKarte = brojLicneKarte;
            GodineIskustva = godineIskustva;
            Knjige = new List<Knjiga>();

        }

        
    }
}
