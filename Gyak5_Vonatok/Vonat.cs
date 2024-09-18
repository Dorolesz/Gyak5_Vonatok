using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyak5_Vonatok
{
	internal class Vonat
	{
		//Adattag
		private string vonatSzam;
		private DateTime indulasiIdo;
		private DateTime erkezesiIdo;
		private string utvonal;

		//konstruktor
		public Vonat(string vonatSzam, DateTime indulasiIdo, DateTime erkezesiIdo, string utvonal)
		{
			this.VonatSzam = vonatSzam;
			this.IndulasiIdo = indulasiIdo;
			this.ErkezesiIdo = erkezesiIdo;
			this.Utvonal = utvonal;
		}

		//Property
		public string VonatSzam { get => vonatSzam; set => vonatSzam = value; }
		public DateTime IndulasiIdo { get => indulasiIdo; set => indulasiIdo = value; }
		public DateTime ErkezesiIdo { get => erkezesiIdo; set => erkezesiIdo = value; }
		public string Utvonal { get => utvonal; set => utvonal = value; }

		public TimeSpan UtazasiIdo => erkezesiIdo - indulasiIdo;


		public override string ToString()
		{
			return $"Vonatszám: {vonatSzam}, Indulás: {indulasiIdo}, Érkezés:{erkezesiIdo}, Útvonal: {utvonal}, Becsült utazási idő: {UtazasiIdo}";
		}

	}
}
