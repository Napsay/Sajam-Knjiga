using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    internal class Adresa
    {
        private string ulica;
        private int broj;
        private string grad;
        private string drzava;

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
    }
}
