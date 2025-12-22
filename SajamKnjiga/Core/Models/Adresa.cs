using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Adresa:ISerializable
    {
        private int sifra;

        

        private string ulica;
        private int broj;
        private string grad;
        private string drzava;

        public int Sifra
        {
            get { return sifra; }
            set { sifra = value; }
        }
        public string Drzava
        {
            get { return drzava; }
            set { drzava = value; }
        }
        public string Grad
        {
            get { return grad; }
            set { grad = value; }
        }
        public int Broj
        {
            get { return broj; }
            set { broj = value; }
        }
        public string Ulica
        {
            get { return ulica; }
            set { ulica = value; }
        }

        public Adresa(string ulica,int broj,string grad,string drzava)
        {

            Ulica = ulica;
            Broj = broj;
            Grad = grad;
            Drzava = drzava;
        }

        public override string ToString()
        {
            return $"{Ulica} {Broj}, {Grad}, {Drzava}";
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Sifra.ToString(),
                Ulica,
                Broj.ToString(),
                Grad,
                Drzava
            };
            return csvValues;
        }
        public Adresa()
        {
                
        }
        public void FromCSV(string[] values)
        {
            Sifra = int.Parse(values[0]);
            Ulica = values[1];
            Broj = int.Parse(values[2]);
            Grad = values[3];
            Drzava = values[4];
        }
    }
}
