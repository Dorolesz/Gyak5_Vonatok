using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyak5_Vonatok
{
	internal class Program
	{
		static List<Vonat> vonatok = new List<Vonat>();
		static void Main(string[] args)
		{
			string filePath = "vonatok.csv";

			try
			{
				using (StreamReader sr = new StreamReader(filePath))
				{
					string line;
					while ((line = sr.ReadLine()) != null)
					{
						var parts = line.Split(',');

						if (parts.Length == 4 &&
							DateTime.TryParse(parts[1], out DateTime indulasiIdo) &&
							DateTime.TryParse(parts[2], out DateTime erkezesiIdo))
						{
							var vonat = new Vonat(parts[0], indulasiIdo, erkezesiIdo, parts[3]);
							vonatok.Add(vonat);
						}
					}
				}
				foreach (var vonat in vonatok)
				{
                    Console.WriteLine($"Vonatszám: {vonat.VonatSzam}\n Indulás: {vonat.IndulasiIdo}\n Érkezés:{vonat.ErkezesiIdo}\n Útvonal: {vonat.Utvonal}\n Becsült utazási idő: {vonat.UtazasiIdo}\n");
                }
			}
			catch (Exception e)
			{
                Console.WriteLine($"Hiba történt: {e.Message}");
            }

			Vonat legkozelebbiVonat = KovIndVonat();
			Console.WriteLine(legkozelebbiVonat != null ?
			   $"Következő induló vonat: {legkozelebbiVonat.VonatSzam}" :
			   "Nincs induló vonat.");

			var leghosszabbUtazas = LeghosszabbIdo();
			Console.WriteLine($"Leghosszabb utazás: Vonatszám: {leghosszabbUtazas.VonatSzam}, Időtartam: {leghosszabbUtazas.UtazasiIdo}");

			Console.ReadKey();
		}
		static Vonat KovIndVonat()
		{
			foreach (var vonat in vonatok)
			{
				if (vonat.IndulasiIdo > DateTime.Now)
				{
					return vonat;
				}
			}
			return null;
		}

		static Vonat LeghosszabbIdo()
		{
			Vonat leghosszabbVonat = null;

            foreach (var vonat in vonatok)
            {
				if (leghosszabbVonat == null || vonat.UtazasiIdo > leghosszabbVonat.UtazasiIdo)
				{
					leghosszabbVonat = vonat;
				}
            }
			return leghosszabbVonat;
        } 
	}
}
