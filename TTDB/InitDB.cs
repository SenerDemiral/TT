using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starcounter;

namespace TTDB
{
	public class InitDB
	{
		public void Init()
		{
			//Db.SQL("CREATE INDEX TakimOyuncuIndex ON TakimOyuncu(Turnuva, Takim)");

			Db.Transact(() => {
				Db.SlowSQL("DELETE FROM Turnuva");
				Db.SlowSQL("DELETE FROM Takim");
				Db.SlowSQL("DELETE FROM Oyuncu");
				Db.SlowSQL("DELETE FROM TurnuvaTakim");
				Db.SlowSQL("DELETE FROM TakimOyuncu");
				Db.SlowSQL("DELETE FROM Musabaka");
				Db.SlowSQL("DELETE FROM Mac");
				Db.SlowSQL("DELETE FROM MacSonuc");


				Oyuncu nihat = new Oyuncu() { Ad = "Nihat", DgmYil = 1980, Sex = "E" };
				Oyuncu tunc = new Oyuncu() { Ad = "Tunç", DgmYil = 1980, Sex = "E" };

				Takim promil = new Takim() { Ad = "Promil" };
				Takim ponpin = new Takim() { Ad = "Ponpin" };
				Takim dragon = new Takim() { Ad = "Dragon" };
				Takim meat = new Takim() { Ad = "mEAT United" };
				Takim yaliKavak = new Takim() { Ad = "YalıKavak" };
				Takim gumusluk = new Takim() { Ad = "Gümüşlük" };
				Takim delfi = new Takim() { Ad = "Delfi" };
				Takim peksimet = new Takim() { Ad = "Peksimet" };
				Takim telmisos = new Takim() { Ad = "Telmisos" };

				Turnuva turnuva1 = new Turnuva() { Ad = "2016 Bodrum 1.Lig", Trh = DateTime.Now };
				Turnuva turnuva2 = new Turnuva() { Ad = "2016 Bodrum MT 2.Lig", Trh = DateTime.Now };

				TurnuvaTakim turnuvaTakim11 = new TurnuvaTakim() { Turnuva = turnuva1, Takim = promil };
				TurnuvaTakim turnuvaTakim12 = new TurnuvaTakim() { Turnuva = turnuva1, Takim = ponpin };
				TurnuvaTakim turnuvaTakim13 = new TurnuvaTakim() { Turnuva = turnuva1, Takim = dragon };
				TurnuvaTakim turnuvaTakim14 = new TurnuvaTakim() { Turnuva = turnuva1, Takim = delfi };
				TurnuvaTakim turnuvaTakim15 = new TurnuvaTakim() { Turnuva = turnuva1, Takim = gumusluk };
				TurnuvaTakim turnuvaTakim16 = new TurnuvaTakim() { Turnuva = turnuva1, Takim = peksimet };
				TurnuvaTakim turnuvaTakim17 = new TurnuvaTakim() { Turnuva = turnuva1, Takim = yaliKavak };
				TurnuvaTakim turnuvaTakim18 = new TurnuvaTakim() { Turnuva = turnuva1, Takim = telmisos };

				TurnuvaTakim turnuvaTakim24 = new TurnuvaTakim() { Turnuva = turnuva2, Takim = meat };

				//Mac m = Db.SQL<Mac>("select m from TTDB.Mac m where m = ?", mac111).First;
				//Console.WriteLine(m.MacOzet.Puanlar);
				//Console.WriteLine(m.MacOzet.Setler);
				//Console.WriteLine(m.MacOzet.Sayilar);

				new TakimOyuncu() { Turnuva = turnuva1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Nihat" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Tunç" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Heiko" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Ahmet" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Rıdvan" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Edip" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Çağatay" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Yasin" } };

				new TakimOyuncu() { Turnuva = turnuva1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Tahir" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Oğuz" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Nadir" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Ahmet" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Kutluhan" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Derin" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Hadis" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Görkem" } };

				new TakimOyuncu() { Turnuva = turnuva1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Behçet" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Levent" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Dora" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Hakkı" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Mustafa" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Faruk" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Arif" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Akif" } };

				new TakimOyuncu() { Turnuva = turnuva1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Şener DEMİRAL" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Hakan UĞURLU" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Şevket TAYHAN" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Erhan DOĞRU" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Emre ESMER" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Göksan AKAY" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Yenal" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Mustafa TUTAM" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Ahmethan ACET" } };

				new TakimOyuncu() { Turnuva = turnuva1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Tuna KURT" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Mete ARIK" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Rıza KARAKAYA" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Atilla OĞUZ" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Çetin ÖNCÜ" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Mehmet BARUTCU" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Ahmet KURŞUNCU" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Erkan ARI" } };

				new TakimOyuncu() { Turnuva = turnuva1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Davut" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Erol" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Kemal" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Nick" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Ömer" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "İsmail" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Yaşar" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Hüseyin" } };

				new TakimOyuncu() { Turnuva = turnuva1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Mahmut" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Cem" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Hanifi" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Suavi" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Sabri" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Faruk" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Marina", Sex = "K" } };
				new TakimOyuncu() { Turnuva = turnuva1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Tarık" } };


				var tt = Db.SQL<TurnuvaTakim>("SELECT tt FROM TurnuvaTakim tt WHERE tt.Turnuva = ?", turnuva1);
				var d = 0;
				foreach(var tm in tt) {
					foreach(var td in tt) {
						if(tm.Takim.GetObjectID() != td.Takim.GetObjectID()) {
							new Musabaka() { Turnuva = turnuva1, HomeTakim = tm.Takim, GuestTakim = td.Takim, Trh = DateTime.Today.AddDays(d) };
							d += 7;
						}
					}
				}

				//Musabaka musabaka11 = (Musabaka)DbHelper.FromID(turnuva1.GetObjectNo());
				Musabaka promil_delfi = Db.SQL<Musabaka>("SELECT m FROM Musabaka m where m.Turnuva = ? AND m.HomeTakim = ? AND m.GuestTakim = ?", turnuva1, promil, delfi).First;
				Oyuncu promilSener = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o WHERE o.Ad = ?", "Şener DEMİRAL").First;
				Oyuncu promilHakan = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o WHERE o.Ad = ?", "Hakan UĞURLU").First;
				Oyuncu promilSevket = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o WHERE o.Ad = ?", "Şevket TAYHAN").First;
				Oyuncu delfiAhmet = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o WHERE o.Ad = ?", "Tuna KURT").First;
				Oyuncu delfiMete = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o WHERE o.Ad = ?", "Mete ARIK").First;
				Oyuncu delfiRiza = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o WHERE o.Ad = ?", "Rıza KARAKAYA").First;

				//Musabaka musabaka11 = new Musabaka() { Turnuva = turnuva1, HomeTakim = promil, GuestTakim = ponpin, Trh = DateTime.Now };
				Mac mac111 = new Mac() { Turnuva = turnuva1, Musabaka = promil_delfi, Sira = 1, HomeOyuncu = promilSener, GuestOyuncu = delfiAhmet, Skl = "S" };
				new MacSonuc() { Mac = mac111, SetNo = 1, HomeSayi = 11, GuestSayi = 7 };
				new MacSonuc() { Mac = mac111, SetNo = 2, HomeSayi = 9, GuestSayi = 11 };
				new MacSonuc() { Mac = mac111, SetNo = 3, HomeSayi = 15, GuestSayi = 13 };
				Mac mac112 = new Mac() { Turnuva = turnuva1, Musabaka = promil_delfi, Sira = 2, HomeOyuncu = promilHakan, GuestOyuncu = delfiMete, Skl = "S" };
				new MacSonuc() { Mac = mac112, SetNo = 1, HomeSayi = 6, GuestSayi = 11 };
				new MacSonuc() { Mac = mac112, SetNo = 2, HomeSayi = 9, GuestSayi = 11 };
				Mac mac113 = new Mac() { Turnuva = turnuva1, Musabaka = promil_delfi, Sira = 3, HomeOyuncu = promilSevket, GuestOyuncu = delfiRiza, Skl = "S" };
				new MacSonuc() { Mac = mac113, SetNo = 1, HomeSayi = 11, GuestSayi = 3 };
				new MacSonuc() { Mac = mac113, SetNo = 2, HomeSayi = 11, GuestSayi = 5 };

				//var delfi_promil = new Musabaka() { Turnuva = turnuva1, HomeTakim = delfi, GuestTakim = promil, Trh = DateTime.Today };
				//var promil_gumusluk = new Musabaka() { Turnuva = turnuva1, HomeTakim = promil, GuestTakim = gumusluk, Trh = DateTime.Today.AddDays(5) };
				//var promil_peksimet = new Musabaka() { Turnuva = turnuva1, HomeTakim = promil, GuestTakim = peksimet, Trh = DateTime.Today.AddDays(10) };
				//var promil_dragon = new Musabaka() { Turnuva = turnuva1, HomeTakim = promil, GuestTakim = dragon, Trh = DateTime.Today.AddDays(15) };
				//var promil_yaliKavak = new Musabaka() { Turnuva = turnuva1, HomeTakim = promil, GuestTakim = yaliKavak, Trh = DateTime.Today.AddDays(20) };
				//var dragon_yaliKavak = new Musabaka() { Turnuva = turnuva1, HomeTakim = dragon, GuestTakim = yaliKavak, Trh = DateTime.Today.AddDays(25) };

				//new Mac() { Turnuva = turnuva1, Musabaka = delfi_promil, Skl = "S", Sira = 8, HomeOyuncu = }
			});
		}
	}
}
