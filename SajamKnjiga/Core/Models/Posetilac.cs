using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Models
{

    public enum StatusPosetioca
    {
        P,  //Redovan
        B  //VIP
    }

    public class Posetilac: ISerializable
    {

        private string ime;
        private string prezime;
        private DateTime datumRodjenja;
        private Adresa adresa;
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

        public Adresa Adresa
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
            Adresa adresa, string telefon, string email, string brClanskeKarte,
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

        public override string ToString()
        {
            return
                $"{Ime,-12} | " +
                $"{Prezime,-12} | " +
                $"{DatumRodjenja:yyyy-MM-dd,-12} | " +
                $"{Adresa?.ToString() ?? "N/A",-20} | " +
                $"{Telefon,-12} | " +
                $"{Email,-25} | " +
                $"{BrClanskeKarte,-10} | " +
                $"{TrenutnaGodClanstva,4} | " +
                $"{Status,-3} | " +
                $"{ProsecnaOcenaRec,5:F2} |";
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Ime,
                Prezime,
                datumRodjenja.ToString("yyyy-MM-dd"),
                $"{adresa.Sifra}",
                Telefon,
                Email,
                BrClanskeKarte,
                TrenutnaGodClanstva.ToString(),
                Status.ToString(),
                ProsecnaOcenaRec.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Ime = values[0];
            Prezime = values[1];
            datumRodjenja = DateTime.Parse(values[2]);
            Adresa = new Adresa { Sifra = int.Parse(values[3]) };
            Telefon = values[4];
            Email = values[5];
            BrClanskeKarte = values[6];
            TrenutnaGodClanstva = int.Parse(values[7]);
            Enum.TryParse(values[8], out StatusPosetioca statusValue);
            Status = statusValue;
            ProsecnaOcenaRec = double.Parse(values[9]);

        }
    }
}
