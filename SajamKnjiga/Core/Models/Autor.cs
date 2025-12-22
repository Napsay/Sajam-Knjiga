using System;
using System.Collections.Generic;
public class Autor
{
	public Autor()
	{
		private string ime;
		private string prezime;
		private DateTime datumRodjenja;
		private string adresa;
		private string telefon;
		private string brojLicneKarte;
		private int godineIskustva;
		private string email;

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

		public string Adresa
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
		public Autor(string ime, string prezime, DateTime datumRodjenja,
                 string adresa, string telefon, string email,
                 string brojLicneKarte, int godineIskustva)
		{
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
