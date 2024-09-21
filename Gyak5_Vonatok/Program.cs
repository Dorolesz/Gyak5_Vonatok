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

			// KövetkezŐ induló vonat
			Vonat legkozelebbiVonat = KovIndVonat();
			Console.WriteLine(legkozelebbiVonat != null ?
			   $"Következő induló vonat: {legkozelebbiVonat.VonatSzam}" :
			   "Nincs induló vonat.\n");

			// Vonat a leghosszab utazási idővel
			var leghosszabbUtazas = LeghosszabbIdo();
			Console.WriteLine($"Leghosszabb utazás: Vonatszám: {leghosszabbUtazas.VonatSzam}, Időtartam: {leghosszabbUtazas.UtazasiIdo}\n");

			// Indulások egy adott időpont előtt
			DateTime keresettIdo = DateTime.Now.AddHours(1); // Példa időpont, 1 óra múlva
			ListazIndulElott(keresettIdo);

			// Vonatok listázása egy adott útszakaszra
			string keresettUtvonal = "Budapest-Debrecen"; // Példa útszakasz
			ListazUtszakaszra(keresettUtvonal);

			// Vonatok csoportosítása útvonal szerint
			CsoportositasUtvonalSzerint();

			// Utazások statisztikája
			TimeSpan atlagosUtazasiIdo = AtlagosUtazasiIdo();
			Console.WriteLine($"Átlagos utazási idő: {atlagosUtazasiIdo}");

			// Vonat késése
			IrjKesesFajlba("kesett_vonatok.csv");

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

		static void ListazIndulElott(DateTime ido)
		{
			var korabbiVonatok = vonatok.Where(v => v.IndulasiIdo < ido).ToList();
            Console.WriteLine("Az időpont előtt induló vonatok:");

			foreach (var vonat in korabbiVonatok)
            {
				Console.WriteLine($"Vonatszám: {vonat.VonatSzam}\n Indulás: {vonat.IndulasiIdo}\n Érkezés:{vonat.ErkezesiIdo}\n Útvonal: {vonat.Utvonal}\n Becsült utazási idő: {vonat.UtazasiIdo}\n");
			}
            Console.WriteLine("--------------------------------------------------");
        }

		static void ListazUtszakaszra(string utvonal)
		{
			var talalatok = vonatok.Where(v => v.Utvonal.Equals(utvonal, StringComparison.OrdinalIgnoreCase)).ToList();
			Console.WriteLine($"Az útszakaszra ({utvonal}) közlekedő vonatok:");
			foreach (var vonat in talalatok)
			{
				Console.WriteLine(vonat);
			}
			Console.WriteLine("--------------------------------------------------");

		}

		static void CsoportositasUtvonalSzerint()
		{
			var csoportositas = vonatok.GroupBy(v => v.Utvonal)
										.ToDictionary(g => g.Key, g => g.ToList());

			Console.WriteLine("Vonatok csoportosítva útvonal szerint:");

			foreach (var csoport in csoportositas)
			{
				Console.WriteLine($"Útvonal: {csoport.Key}");
				foreach (var vonat in csoport.Value)
				{
					Console.WriteLine($"Vonatszám: {vonat.VonatSzam}\n Indulás: {vonat.IndulasiIdo}\n Érkezés:{vonat.ErkezesiIdo}\n Útvonal: {vonat.Utvonal}\n Becsült utazási idő: {vonat.UtazasiIdo}\n");
					;
				}
            }
		}

		static TimeSpan AtlagosUtazasiIdo()
		{
			if (vonatok.Count == 0)
				return TimeSpan.Zero;

			TimeSpan osszesUtazasiIdo = vonatok.Aggregate(TimeSpan.Zero, (sum, v) => sum + v.UtazasiIdo);
			return TimeSpan.FromTicks(osszesUtazasiIdo.Ticks / vonatok.Count);
		}

		static void IrjKesesFajlba(string filePath)
		{
			using (StreamWriter sw = new StreamWriter(filePath))
			{
				foreach (var vonat in vonatok)
				{
					DateTime kesettErkezesiIdo = vonat.ErkezesiIdo.AddMinutes(15);
					sw.WriteLine($"{vonat.VonatSzam},{vonat.IndulasiIdo},{kesettErkezesiIdo},{vonat.Utvonal}");
				}
			}
			Console.WriteLine($"\nKéséssel kalkulált érkezési idők kiírva a '{filePath}' fájlba.");
		}
	}
}

//Használtam AI-t
