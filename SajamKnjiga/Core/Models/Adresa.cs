using System;

public class Adresa
{
	public Adresa()
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

	}
}
