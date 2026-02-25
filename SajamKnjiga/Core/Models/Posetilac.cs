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

        public List<Knjiga> ListaZelja { get; set; } = new List<Knjiga>();

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

        
        public Posetilac()
        {
            KupljeneKnjige = new List<Kupovina>();
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
            
        }

        public override string ToString()
        {
            return string.Format(
                "{0,-15} | {1,-15} | {2,-12:yyyy-MM-dd} | {3,-15} | {4,-25} | {5,-15} | {6,13} | {7,-10} | {8,6:F1} |",
                Ime,
                Prezime,
                DatumRodjenja,
                Telefon,
                Email,
                BrClanskeKarte,
                TrenutnaGodClanstva,
                Status,
                ProsecnaOcenaRec
            );
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

        //parsiranje clankse karte za sortiranje
        public int GodinaClanskeKarte
        {
            get
            {
                var parts = BrClanskeKarte.Split('-'); 
                if (parts.Length == 3 && int.TryParse(parts[2], out int godina))
                    return godina;
                return 0;
            }
        }

        public int RedniBroj
        {
            get
            {
                var parts = BrClanskeKarte.Split('-');
                if (parts.Length >= 2 && int.TryParse(parts[1], out int broj))
                    return broj;
                return 0;
            }
        }

        public int AdresaID
        {
            get
            {
                return Adresa?.Sifra ?? 0;
            }
        }
    }
}
