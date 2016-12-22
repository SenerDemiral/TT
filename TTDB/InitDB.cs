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

				Oyuncu sener = new Oyuncu() { Ad = "Şener DEMİRAL", DgmYil = 1960, Sex = "E", Tel = "533-271-9797" };
            Oyuncu hakan = new Oyuncu() { Ad = "Hakan UĞURLU", DgmYil = 1965, Sex = "E" };
            Oyuncu sevket = new Oyuncu() { Ad = "Şevket TAYHAN", DgmYil = 1970, Sex = "E" };
            Oyuncu erhan = new Oyuncu() { Ad = "Erhan DOĞRU", DgmYil = 1970, Sex = "E" };
            Oyuncu mehmet = new Oyuncu() { Ad = "Mehmet", DgmYil = 1980, Sex = "E" };
            Oyuncu ali = new Oyuncu() { Ad = "Ali", DgmYil = 1980, Sex = "E" };
            Oyuncu veli = new Oyuncu() { Ad = "Veli", DgmYil = 1980, Sex = "E" };
            
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

            TakimOyuncu takimOyuncu111 = new TakimOyuncu() { Turnuva = turnuva1, Takim = promil, Oyuncu = sener };
            TakimOyuncu takimOyuncu112 = new TakimOyuncu() { Turnuva = turnuva1, Takim = promil, Oyuncu = hakan };
            TakimOyuncu takimOyuncu113 = new TakimOyuncu() { Turnuva = turnuva1, Takim = promil, Oyuncu = sevket };
            TakimOyuncu takimOyuncu114 = new TakimOyuncu() { Turnuva = turnuva1, Takim = promil, Oyuncu = erhan };

            TakimOyuncu takimOyuncu125 = new TakimOyuncu() { Turnuva = turnuva1, Takim = ponpin, Oyuncu = mehmet };
            TakimOyuncu takimOyuncu126 = new TakimOyuncu() { Turnuva = turnuva1, Takim = ponpin, Oyuncu = ali };
            TakimOyuncu takimOyuncu127 = new TakimOyuncu() { Turnuva = turnuva1, Takim = ponpin, Oyuncu = veli };

            TakimOyuncu takimOyuncu137 = new TakimOyuncu() { Turnuva = turnuva1, Takim = dragon, Oyuncu = nihat };
            TakimOyuncu takimOyuncu138 = new TakimOyuncu() { Turnuva = turnuva1, Takim = dragon, Oyuncu = tunc };

            TakimOyuncu takimOyuncu31 = new TakimOyuncu() { Turnuva = turnuva2, Takim = meat, Oyuncu = sener };
            TakimOyuncu takimOyuncu32 = new TakimOyuncu() { Turnuva = turnuva2, Takim = meat, Oyuncu = hakan };

            Musabaka musabaka11 = new Musabaka() { Turnuva = turnuva1, HomeTakim = promil, GuestTakim = ponpin, Trh = DateTime.Now };
            Mac mac111 = new Mac() { Turnuva = turnuva1, Musabaka = musabaka11, Sira = 1, HomeOyuncu = sener, GuestOyuncu = mehmet, Skl = "S" };
            new MacSonuc() { Mac = mac111, SetNo = 1, HomeSayi = 11, GuestSayi = 7 };
            new MacSonuc() { Mac = mac111, SetNo = 2, HomeSayi = 9, GuestSayi = 11 };
            new MacSonuc() { Mac = mac111, SetNo = 3, HomeSayi = 15, GuestSayi = 13 };
            Mac mac112 = new Mac() { Turnuva = turnuva1, Musabaka = musabaka11, Sira = 2, HomeOyuncu = hakan, GuestOyuncu = ali, Skl = "S" };
            new MacSonuc() { Mac = mac112, SetNo = 1, HomeSayi = 6, GuestSayi = 11 };
            new MacSonuc() { Mac = mac112, SetNo = 2, HomeSayi = 9, GuestSayi = 11 };
            Mac mac113 = new Mac() { Turnuva = turnuva1, Musabaka = musabaka11, Sira = 3, HomeOyuncu = sevket, GuestOyuncu = veli, Skl = "S" };
            new MacSonuc() { Mac = mac113, SetNo = 1, HomeSayi = 11, GuestSayi = 3 };
            new MacSonuc() { Mac = mac113, SetNo = 2, HomeSayi = 11, GuestSayi = 5 };

				Musabaka musabaka12 = new Musabaka() { Turnuva = turnuva1, HomeTakim = dragon, GuestTakim = promil, Trh = DateTime.Now };
            Mac mac121 = new Mac() { Turnuva = turnuva1, Musabaka = musabaka12, Sira = 1, HomeOyuncu = veli, GuestOyuncu = sener, Skl = "S" };
            new MacSonuc() { Mac = mac121, SetNo = 1, HomeSayi = 11, GuestSayi = 5 };
            new MacSonuc() { Mac = mac121, SetNo = 2, HomeSayi = 11, GuestSayi = 6 };
            Mac mac122 = new Mac() { Turnuva = turnuva1, Musabaka = musabaka12, Sira = 2, HomeOyuncu = nihat, GuestOyuncu = hakan, Skl = "S" };
            new MacSonuc() { Mac = mac122, SetNo = 1, HomeSayi = 8, GuestSayi = 11 };
            new MacSonuc() { Mac = mac122, SetNo = 2, HomeSayi = 9, GuestSayi = 11 };


            Mac m = Db.SQL<Mac>("select m from TTDB.Mac m where m = ?", mac111).First;
				//Console.WriteLine(m.MacOzet.Puanlar);
				//Console.WriteLine(m.MacOzet.Setler);
				//Console.WriteLine(m.MacOzet.Sayilar);

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

				//var delfi_promil = new Musabaka() { Turnuva = turnuva1, HomeTakim = delfi, GuestTakim = promil, Trh = DateTime.Today };
				//var promil_gumusluk = new Musabaka() { Turnuva = turnuva1, HomeTakim = promil, GuestTakim = gumusluk, Trh = DateTime.Today.AddDays(5) };
				//var promil_peksimet = new Musabaka() { Turnuva = turnuva1, HomeTakim = promil, GuestTakim = peksimet, Trh = DateTime.Today.AddDays(10) };
				//var promil_dragon = new Musabaka() { Turnuva = turnuva1, HomeTakim = promil, GuestTakim = dragon, Trh = DateTime.Today.AddDays(15) };
				//var promil_yaliKavak = new Musabaka() { Turnuva = turnuva1, HomeTakim = promil, GuestTakim = yaliKavak, Trh = DateTime.Today.AddDays(20) };
				//var dragon_yaliKavak = new Musabaka() { Turnuva = turnuva1, HomeTakim = dragon, GuestTakim = yaliKavak, Trh = DateTime.Today.AddDays(25) };

				//new Mac() { Turnuva = turnuva1, Musabaka = delfi_promil, Skl = "S", Sira = 8, HomeOyuncu = }

				var tt = Db.SQL<TurnuvaTakim>("SELECT tt FROM TurnuvaTakim tt WHERE tt.Turnuva = ?", turnuva1);
				var d = 0;
				foreach (var tm in tt) {
					foreach(var td in tt) {
						if(tm.Takim.GetObjectID() != td.Takim.GetObjectID()) {
							new Musabaka() { Turnuva = turnuva1, HomeTakim = tm.Takim, GuestTakim = td.Takim, Trh = DateTime.Today.AddDays(d) };
							d += 7;
						}
					}
				}
			});
      }
   }
}
