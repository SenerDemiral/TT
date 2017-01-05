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
		public void Deneme()
		{
			Takim takim = null;
			Db.Transact(() => {
				takim = Db.SQL<Takim>("SELECT o FROM Takim o WHERE Ad = ?", "Promil").First;
				takim.Lat = "37.041987";
				takim.Lon = "27.428861";

				takim = Db.SQL<Takim>("SELECT o FROM Takim o WHERE Ad = ?", "Delfi").First;
				takim.Lat = "37.034957";
				takim.Lon = "27.438069";

				takim = Db.SQL<Takim>("SELECT o FROM Takim o WHERE Ad = ?", "KutayReno").First;
				takim.Lat = "37.026259";
				takim.Lon = "27.250050";
			});


			/*
			Db.Transact(() => {
				new Oyuncu() { Ad = "*" };
			});
			
			var oyuncuBos = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o WHERE Ad = ?", "*").First;
			Db.Transact(() => {
				var maclar = Db.SQL<Mac>("SELECT m FROM Mac m WHERE m.HomeOyuncu IS NULL");
				foreach(var mac in maclar) {
					mac.HomeOyuncu = oyuncuBos;
					mac.GuestOyuncu = oyuncuBos;
				}
			});
			*/
		}

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

				Takim meat = new Takim() { Ad = "mEAT United" };

				Takim promil    = new Takim() { Ad = "Promil",    Lat = "37.041987", Lon = "27.428861", Adres = "Yokuşbaşı Mahallesi, mEAT Burger & Steak House, Kıbrıs Şehitleri Caddesi, Bodrum/Muğla" };
				Takim delfi     = new Takim() { Ad = "Delfi",     Lat = "37.034957", Lon = "27.438069", Adres = "Kumbahçe Mahallesi, Delfi Hotel Spa & Wellness, Dere Sokak, Bodrum/Muğla" };
				Takim kutay     = new Takim() { Ad = "KutayReno", Lat = "37.026259", Lon = "27.250050", Adres = "Bahçelievler Mahallesi, Avta Restaurant, Turgutreis/Bodrum/Muğla" };
				
				Takim gumusluk  = new Takim() { Ad = "Gümüşlük",  Lat = "37.054979", Lon = "27.233205" };   // ?
				Takim yaliKavak = new Takim() { Ad = "YalıKavak", Lat = "37.104153", Lon = "27.287466" };	// ?
				Takim ponpin    = new Takim() { Ad = "Ponpin",    Lat = "37.029388", Lon = "27.408416" };	// ?
				Takim dragon    = new Takim() { Ad = "Dragon",    Lat = "37.033123", Lon = "27.429544" }; // ?
				Takim peksimet  = new Takim() { Ad = "Peksimet",  Lat = "37.033123", Lon = "27.429544" }; // ?
				Takim telmisos  = new Takim() { Ad = "Telmisos",  Lat = "37.033123", Lon = "27.429544" };	// ?
				Takim kekik     = new Takim() { Ad = "Kekik",     Lat = "37.033123", Lon = "27.429544" };	// ?
				Takim nane      = new Takim() { Ad = "Nane",      Lat = "37.033123", Lon = "27.429544" };	// ?

				Turnuva trn1 = new Turnuva() { Ad = "2016 Bodrum 1.Lig", Trh = DateTime.Now };
				Turnuva turnuva2 = new Turnuva() { Ad = "2016 Bodrum MT 2.Lig", Trh = DateTime.Now };

				TurnuvaTakim turnuvaTakim11 = new TurnuvaTakim() { Turnuva = trn1, Takim = promil };
				TurnuvaTakim turnuvaTakim12 = new TurnuvaTakim() { Turnuva = trn1, Takim = ponpin };
				TurnuvaTakim turnuvaTakim13 = new TurnuvaTakim() { Turnuva = trn1, Takim = dragon };
				TurnuvaTakim turnuvaTakim14 = new TurnuvaTakim() { Turnuva = trn1, Takim = delfi };
				TurnuvaTakim turnuvaTakim15 = new TurnuvaTakim() { Turnuva = trn1, Takim = gumusluk };
				TurnuvaTakim turnuvaTakim16 = new TurnuvaTakim() { Turnuva = trn1, Takim = peksimet };
				TurnuvaTakim turnuvaTakim17 = new TurnuvaTakim() { Turnuva = trn1, Takim = yaliKavak };
				TurnuvaTakim turnuvaTakim18 = new TurnuvaTakim() { Turnuva = trn1, Takim = telmisos };
				TurnuvaTakim turnuvaTakim19 = new TurnuvaTakim() { Turnuva = trn1, Takim = kekik };
				TurnuvaTakim turnuvaTakim20 = new TurnuvaTakim() { Turnuva = trn1, Takim = kutay };
				TurnuvaTakim turnuvaTakim21 = new TurnuvaTakim() { Turnuva = trn1, Takim = nane };

				TurnuvaTakim turnuvaTakim24 = new TurnuvaTakim() { Turnuva = turnuva2, Takim = meat };

				//Mac m = Db.SQL<Mac>("select m from TTDB.Mac m where m = ?", mac111).First;
				//Console.WriteLine(m.MacOzet.Puanlar);
				//Console.WriteLine(m.MacOzet.Setler);
				//Console.WriteLine(m.MacOzet.Sayilar);

				new TakimOyuncu() { Turnuva = trn1, Takim = ponpin, Oyuncu = new Oyuncu() { Ad = "Hayri CANGİL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = ponpin, Oyuncu = new Oyuncu() { Ad = "Cengiz ÇORUMLU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = ponpin, Oyuncu = new Oyuncu() { Ad = "Kenan TURSAK" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = ponpin, Oyuncu = new Oyuncu() { Ad = "Uğur KESİCİ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = ponpin, Oyuncu = new Oyuncu() { Ad = "Ertuğ TUTAL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = ponpin, Oyuncu = new Oyuncu() { Ad = "Bülent AKDERE" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = ponpin, Oyuncu = new Oyuncu() { Ad = "İlyas ERBEM" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = ponpin, Oyuncu = new Oyuncu() { Ad = "Mustafa YILDIZ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = ponpin, Oyuncu = new Oyuncu() { Ad = "Turan BARAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = ponpin, Oyuncu = new Oyuncu() { Ad = "Tansel ERGUN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = ponpin, Oyuncu = new Oyuncu() { Ad = "Çağlar ULUDAMAR" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = ponpin, Oyuncu = new Oyuncu() { Ad = "Erkan UZMAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = ponpin, Oyuncu = new Oyuncu() { Ad = "İrfan YAVUZ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = ponpin, Oyuncu = new Oyuncu() { Ad = "HalUk ARIĞ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = ponpin, Oyuncu = new Oyuncu() { Ad = "Ali HAGİ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = ponpin, Oyuncu = new Oyuncu() { Ad = "Rüştü TEZCAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = ponpin, Oyuncu = new Oyuncu() { Ad = "Şebnem KAPANAKİ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = ponpin, Oyuncu = new Oyuncu() { Ad = "Atilla OKTÜRK" } };

				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Erkan ARI" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Mehmet BARUTCU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Çetin ÖNCÜ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Ahmet KURŞUNCU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Atilla OĞUZ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Rıza KARAKAYA" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Aytuğ DOGER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Nadi ATASOY" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Yücel NİEGO" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Adnan KİSTAK" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Ayhan BOSTAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Mustafa KADER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Mehmet DAGOGLU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Mehmet KORKMAZ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Hüseyin GÖZÜTOK" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Ünal PEKTAŞ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Mesut Batur ÇAT" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Semih NALBANTOGLU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Ahmet ŞAHİN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Metin GÜR" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Levent BULUT" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = delfi, Oyuncu = new Oyuncu() { Ad = "Erkan VARLI" } };


				new TakimOyuncu() { Turnuva = trn1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Nihat ÜMMETOĞLU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Tunç HIZAL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Vahi GÜNER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Çağatay ŞAŞMAZ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Ercan YEŞİLÇİMEN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Ali BİLGİN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Recai ÇAKIR" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Gökhan HAMURCU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Selahattin Rıdvan GÖKBEL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Yasin YILDIRIM" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Edip AYDOĞAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Ahmet KÜLAHÇIOĞLU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Heiko MAUSEZAHL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Ferit CİLSİN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Necip GÖKBEL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Esen ODABAŞ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = dragon, Oyuncu = new Oyuncu() { Ad = "Mustafa GÜN" } };

				new TakimOyuncu() { Turnuva = trn1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Serdar KAYA" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Faruk GEDİKOĞLU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Akif KURTULUŞ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Arif ERGUVAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Yasin YENİCEOĞLU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Mustafa ÖZASLAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Ümit YEKREK" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Hakkı ZIRH" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Behçet AK" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Levent ALTAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Serkan İLHAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Fırat POLAT" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Metin SOYSAL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Mustafa FİL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Hamit MENGÜÇ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Süleyman KARTAL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = gumusluk, Oyuncu = new Oyuncu() { Ad = "Dora ŞAŞMAZ" } };

				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Hünkar YÜCEL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Mahir TUFAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Bahadır TULUMCUOĞLU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Göksun ALAY" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Yaşar BULMUŞ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Can DENİZ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Hasan ACIOLUK" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Gürdal DALKILIÇ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Murat ÇAKIR" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Hakan DEBBAĞ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Ramazan İZMİRLİ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Serdar YILDIRIM" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Göktuğ TIK" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Sadun İŞMAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Özgür ÇAM" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Ali Murat GÜLDOĞAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Nezih YAĞAR" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Turan KUŞÇU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Emre CÖMERT" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Aykut KEMER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Nuran KANSU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Yasemin ILGIN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Gülsevin ILGIN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Özlem KARAALP" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Osman KARAALP" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Hatice AKTAŞ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Tuğçe SAVAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Özgen DİKER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Songül AÇA" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Ersoy KOÇER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Kadir ÇİFTÇİ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kekik, Oyuncu = new Oyuncu() { Ad = "Mert CÖMERT" } };

				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Yıldız KÜNEY" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Metin ÖZDEMİR" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Volkan NAMRUK" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Atalay SÜTÇÜ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Ümit ÜNSAL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Baris DALDAL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Sefer NAMRUK" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Ali TÜRKELİ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Özge MELEŞ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Serdar PAMUKKALE" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Kaya ERGİN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Nuh BAŞ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Gül DİLBER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Recep DİLBER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Barış NAMRUK" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Çağla" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Özlem SÜTÇÜ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Kadriye BOZDAĞ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Cem DEMİRCAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Kaya BOZDAĞ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Bülent TÜRKSAL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Şebnem KAYA" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Emre KAYA" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = kutay, Oyuncu = new Oyuncu() { Ad = "Mehmet ÖZDOĞAN" } };
				
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "İsmail DÜZTAŞ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Hüseyin ÖZGÜL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Ömer AYTAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Yaşar ASLAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Nick MUHLBACHER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Meral MUHLBACHER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Erol DANACI" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "NAMIK SARITUNÇ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Kemal GÖKAL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Eren EŞKİKARA" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Feridun BÜYÜKYILDIZ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Şule BÜYÜKYILDIZ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Selin TÜRKOĞLU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Ferhun ERBİR" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Dilek ERBİR" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Davut DİRİL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Gamze DİRİL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Abdullah KOCAMAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Nazım EMRULLAH" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Meltem EMRULLAH" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "İzzet DANACI" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Mustafa ÖZEKEN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Muhammet GEDİK" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Ergin DEMİRTAŞ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Tülay BİBER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "İpek DOĞANAY" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Şükran ÖCAL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Günay" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = peksimet, Oyuncu = new Oyuncu() { Ad = "Şakir ÖZGÜL" } };

				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Marina CLAES" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Tarik TURFAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Sabri GOKTAS" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Faruk ULUSOY" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Necip KUTLUAY" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Suavi DEMİRCİOĞLU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Hanifi DEMİR" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Yavuz KAYNAR" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Cem BASARGAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Mete CAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Arda OZTURK" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Mahmut MURTEZAOGLU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Ayhan ARAL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Erdem HODUL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Onur ALKAYA" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Ilhan KURNAZ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Mehmet ÇUHADAR" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Necati ILGUN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Abdi GUZER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Avni OKCUER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Birgul KUTLUAY" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Yasar KAYA" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Eray GUNAL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Vedat GOKALP" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Afşin HAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Dilek ULUSOY" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Hikmet ÇİTKAP" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Mehmet Emrah METİN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = telmisos, Oyuncu = new Oyuncu() { Ad = "Vahitumu TUNÇ" } };

				new TakimOyuncu() { Turnuva = trn1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Hadis BAYRAKTAR" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Görkem ONUR" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Derin ÖNER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Kutluğhan TENGİZ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Nadir UYSAL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Ahmet KÖK" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Serdar ÖNER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Oğuz DEVELİ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Tahir ÇELİKKESEN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Serkan ŞAHİN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Balamir GÜLER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Sinan LİMONCUOĞLU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Aziz OKYAY" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Selçuk YENER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Mustafa TOZAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Sayim AKAÇIK" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Sadullah TEOMAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = yaliKavak, Oyuncu = new Oyuncu() { Ad = "Selçuk ÜNSAL" } };

				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Burak KANAT" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Metin KOÇER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Kader ONAY" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Ercan AYVAZOĞLU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Hakan DÖRTER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Tufan ÖZMEN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Oktay AKÇA" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Levent DİGİLİ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "İsmail KURT" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Ragıp FERSOY" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Sadettin YENER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Zekeriya YAĞIZ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Cemal YİLDİRİM" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Erdinç ER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Uğur TUFAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Gülsen Saim KÖROĞLU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Osman AKIL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Mehmet KAPTAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Sennur ONAY" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Fatoş KARAKAYA" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Feray ŞİMŞEK" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Ayşin KAYAALP" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Deniz Sude ER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Hürrem Demirel AYVAZOĞLU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Nilufer OWEN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Handan AKÇA" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Mehtap KAPTAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Işıl AKTI" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = nane, Oyuncu = new Oyuncu() { Ad = "Ebru BARAN" } };

				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Erhan DOĞRU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Çağdaş DURANSOY" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Mustafa TUTAM" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Yenal EGE" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Göksan AKAY" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Yusuf BABAYİĞİT" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Emre ESMER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Hakan UĞURLU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Ahmethan ACET" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Şener DEMİRAL" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Ümit ÇETİNALP" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Şevket TAYHAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Emre UZER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Tuncer ÖLMEZ" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "İsamil UNCU" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Metehan KARADAN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Zafer SEZER" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Ali ÇİÇEK" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Erhan AKIN" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Alptekin ARAT" } };
				new TakimOyuncu() { Turnuva = trn1, Takim = promil, Oyuncu = new Oyuncu() { Ad = "Tolga TAŞKIN" } };

				/*
				var tt = Db.SQL<TurnuvaTakim>("SELECT tt FROM TurnuvaTakim tt WHERE tt.Turnuva = ?", trn1);
				var d = 0;
				foreach(var tm in tt) {
					foreach(var td in tt) {
						if(tm.Takim.GetObjectID() != td.Takim.GetObjectID()) {
							new Musabaka() { Turnuva = trn1, HomeTakim = tm.Takim, GuestTakim = td.Takim };
							d += 7;
						}
					}
				}
				*/
				new Musabaka() { Turnuva = trn1, HomeTakim = ponpin, GuestTakim = kekik, Trh = DateTime.ParseExact("11.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = ponpin, GuestTakim = delfi, Trh = DateTime.ParseExact("14.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = ponpin, GuestTakim = dragon, Trh = DateTime.ParseExact("06.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = ponpin, GuestTakim = promil, Trh = DateTime.ParseExact("28.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = ponpin, GuestTakim = gumusluk, Trh = DateTime.ParseExact("27.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = ponpin, GuestTakim = yaliKavak, Trh = DateTime.ParseExact("19.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = ponpin, GuestTakim = telmisos, Trh = DateTime.ParseExact("13.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = ponpin, GuestTakim = kutay, Trh = DateTime.ParseExact("01.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = ponpin, GuestTakim = nane, Trh = DateTime.ParseExact("27.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = ponpin, GuestTakim = peksimet, Trh = DateTime.ParseExact("23.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };

				new Musabaka() { Turnuva = trn1, HomeTakim = kekik, GuestTakim = ponpin, Trh = DateTime.ParseExact("16.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = kekik, GuestTakim = delfi, Trh = DateTime.ParseExact("26.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = kekik, GuestTakim = dragon, Trh = DateTime.ParseExact("20.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = kekik, GuestTakim = promil, Trh = DateTime.ParseExact("09.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = kekik, GuestTakim = gumusluk, Trh = DateTime.ParseExact("08.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = kekik, GuestTakim = yaliKavak, Trh = DateTime.ParseExact("30.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = kekik, GuestTakim = telmisos, Trh = DateTime.ParseExact("21.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = kekik, GuestTakim = kutay, Trh = DateTime.ParseExact("13.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = kekik, GuestTakim = nane, Trh = DateTime.ParseExact("05.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = kekik, GuestTakim = peksimet, Trh = DateTime.ParseExact("06.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };

				new Musabaka() { Turnuva = trn1, HomeTakim = delfi, GuestTakim = ponpin, Trh = DateTime.ParseExact("31.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = delfi, GuestTakim = kekik, Trh = DateTime.ParseExact("15.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = delfi, GuestTakim = dragon, Trh = DateTime.ParseExact("17.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = delfi, GuestTakim = promil, Trh = DateTime.ParseExact("08.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = delfi, GuestTakim = gumusluk, Trh = DateTime.ParseExact("22.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = delfi, GuestTakim = yaliKavak, Trh = DateTime.ParseExact("14.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = delfi, GuestTakim = telmisos, Trh = DateTime.ParseExact("06.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = delfi, GuestTakim = kutay, Trh = DateTime.ParseExact("26.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = delfi, GuestTakim = nane, Trh = DateTime.ParseExact("20.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = delfi, GuestTakim = peksimet, Trh = DateTime.ParseExact("03.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };

				new Musabaka() { Turnuva = trn1, HomeTakim = dragon, GuestTakim = ponpin, Trh = DateTime.ParseExact("21.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = dragon, GuestTakim = kekik, Trh = DateTime.ParseExact("05.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = dragon, GuestTakim = delfi, Trh = DateTime.ParseExact("03.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = dragon, GuestTakim = promil, Trh = DateTime.ParseExact("02.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = dragon, GuestTakim = gumusluk, Trh = DateTime.ParseExact("16.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = dragon, GuestTakim = yaliKavak, Trh = DateTime.ParseExact("08.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = dragon, GuestTakim = telmisos, Trh = DateTime.ParseExact("02.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = dragon, GuestTakim = kutay, Trh = DateTime.ParseExact("22.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = dragon, GuestTakim = nane, Trh = DateTime.ParseExact("16.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = dragon, GuestTakim = peksimet, Trh = DateTime.ParseExact("30.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };

				new Musabaka() { Turnuva = trn1, HomeTakim = promil, GuestTakim = ponpin, Trh = DateTime.ParseExact("16.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = promil, GuestTakim = kekik, Trh = DateTime.ParseExact("30.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = promil, GuestTakim = delfi, Trh = DateTime.ParseExact("26.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = promil, GuestTakim = dragon, Trh = DateTime.ParseExact("16.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = promil, GuestTakim = gumusluk, Trh = DateTime.ParseExact("08.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = promil, GuestTakim = yaliKavak, Trh = DateTime.ParseExact("02.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = promil, GuestTakim = telmisos, Trh = DateTime.ParseExact("22.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = promil, GuestTakim = kutay, Trh = DateTime.ParseExact("16.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = promil, GuestTakim = nane, Trh = DateTime.ParseExact("12.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = promil, GuestTakim = peksimet, Trh = DateTime.ParseExact("01.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };

				new Musabaka() { Turnuva = trn1, HomeTakim = gumusluk, GuestTakim = ponpin, Trh = DateTime.ParseExact("11.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = gumusluk, GuestTakim = kekik, Trh = DateTime.ParseExact("23.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = gumusluk, GuestTakim = delfi, Trh = DateTime.ParseExact("06.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = gumusluk, GuestTakim = dragon, Trh = DateTime.ParseExact("28.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = gumusluk, GuestTakim = promil, Trh = DateTime.ParseExact("20.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = gumusluk, GuestTakim = yaliKavak, Trh = DateTime.ParseExact("13.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = gumusluk, GuestTakim = telmisos, Trh = DateTime.ParseExact("02.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = gumusluk, GuestTakim = kutay, Trh = DateTime.ParseExact("27.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = gumusluk, GuestTakim = nane, Trh = DateTime.ParseExact("02.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = gumusluk, GuestTakim = peksimet, Trh = DateTime.ParseExact("14.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };

				new Musabaka() { Turnuva = trn1, HomeTakim = yaliKavak, GuestTakim = ponpin, Trh = DateTime.ParseExact("10.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = yaliKavak, GuestTakim = kekik, Trh = DateTime.ParseExact("18.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = yaliKavak, GuestTakim = delfi, Trh = DateTime.ParseExact("02.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = yaliKavak, GuestTakim = dragon, Trh = DateTime.ParseExact("24.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = yaliKavak, GuestTakim = promil, Trh = DateTime.ParseExact("16.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = yaliKavak, GuestTakim = gumusluk, Trh = DateTime.ParseExact("06.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = yaliKavak, GuestTakim = telmisos, Trh = DateTime.ParseExact("31.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = yaliKavak, GuestTakim = kutay, Trh = DateTime.ParseExact("04.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = yaliKavak, GuestTakim = nane, Trh = DateTime.ParseExact("27.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = yaliKavak, GuestTakim = peksimet, Trh = DateTime.ParseExact("10.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };

				new Musabaka() { Turnuva = trn1, HomeTakim = telmisos, GuestTakim = ponpin, Trh = DateTime.ParseExact("26.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = telmisos, GuestTakim = kekik, Trh = DateTime.ParseExact("08.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = telmisos, GuestTakim = delfi, Trh = DateTime.ParseExact("22.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = telmisos, GuestTakim = dragon, Trh = DateTime.ParseExact("14.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = telmisos, GuestTakim = promil, Trh = DateTime.ParseExact("08.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = telmisos, GuestTakim = gumusluk, Trh = DateTime.ParseExact("22.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = telmisos, GuestTakim = yaliKavak, Trh = DateTime.ParseExact("10.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = telmisos, GuestTakim = kutay, Trh = DateTime.ParseExact("24.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = telmisos, GuestTakim = nane, Trh = DateTime.ParseExact("16.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = telmisos, GuestTakim = peksimet, Trh = DateTime.ParseExact("30.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };

				new Musabaka() { Turnuva = trn1, HomeTakim = kutay, GuestTakim = ponpin, Trh = DateTime.ParseExact("24.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = kutay, GuestTakim = kekik, Trh = DateTime.ParseExact("02.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = kutay, GuestTakim = delfi, Trh = DateTime.ParseExact("16.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = kutay, GuestTakim = dragon, Trh = DateTime.ParseExact("10.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = kutay, GuestTakim = promil, Trh = DateTime.ParseExact("30.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = kutay, GuestTakim = gumusluk, Trh = DateTime.ParseExact("13.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = kutay, GuestTakim = yaliKavak, Trh = DateTime.ParseExact("20.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = kutay, GuestTakim = telmisos, Trh = DateTime.ParseExact("11.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = kutay, GuestTakim = nane, Trh = DateTime.ParseExact("10.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = kutay, GuestTakim = peksimet, Trh = DateTime.ParseExact("24.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };

				new Musabaka() { Turnuva = trn1, HomeTakim = nane, GuestTakim = ponpin, Trh = DateTime.ParseExact("13.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = nane, GuestTakim = kekik, Trh = DateTime.ParseExact("24.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = nane, GuestTakim = delfi, Trh = DateTime.ParseExact("10.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = nane, GuestTakim = dragon, Trh = DateTime.ParseExact("30.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = nane, GuestTakim = promil, Trh = DateTime.ParseExact("24.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = nane, GuestTakim = gumusluk, Trh = DateTime.ParseExact("20.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = nane, GuestTakim = yaliKavak, Trh = DateTime.ParseExact("11.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = nane, GuestTakim = telmisos, Trh = DateTime.ParseExact("03.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = nane, GuestTakim = kutay, Trh = DateTime.ParseExact("25.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = nane, GuestTakim = peksimet, Trh = DateTime.ParseExact("16.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };

				new Musabaka() { Turnuva = trn1, HomeTakim = peksimet, GuestTakim = ponpin, Trh = DateTime.ParseExact("08.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = peksimet, GuestTakim = kekik, Trh = DateTime.ParseExact("21.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = peksimet, GuestTakim = delfi, Trh = DateTime.ParseExact("21.03.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = peksimet, GuestTakim = dragon, Trh = DateTime.ParseExact("03.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = peksimet, GuestTakim = promil, Trh = DateTime.ParseExact("17.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = peksimet, GuestTakim = gumusluk, Trh = DateTime.ParseExact("31.01.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = peksimet, GuestTakim = yaliKavak, Trh = DateTime.ParseExact("23.11.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = peksimet, GuestTakim = telmisos, Trh = DateTime.ParseExact("14.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = peksimet, GuestTakim = kutay, Trh = DateTime.ParseExact("06.12.16", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };
				new Musabaka() { Turnuva = trn1, HomeTakim = peksimet, GuestTakim = nane, Trh = DateTime.ParseExact("28.02.17", "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture) };


				/*
				//Musabaka musabaka11 = (Musabaka)DbHelper.FromID(trn1.GetObjectNo());
				Musabaka promil_delfi = Db.SQL<Musabaka>("SELECT m FROM Musabaka m where m.Turnuva = ? AND m.HomeTakim = ? AND m.GuestTakim = ?", trn1, promil, delfi).First;
				Oyuncu promilSener = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o WHERE o.Ad = ?", "Şener DEMİRAL").First;
				Oyuncu promilHakan = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o WHERE o.Ad = ?", "Hakan UĞURLU").First;
				Oyuncu promilSevket = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o WHERE o.Ad = ?", "Şevket TAYHAN").First;
				Oyuncu delfiAhmet = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o WHERE o.Ad = ?", "Tuna KURT").First;
				Oyuncu delfiMete = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o WHERE o.Ad = ?", "Mete ARIK").First;
				Oyuncu delfiRiza = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o WHERE o.Ad = ?", "Rıza KARAKAYA").First;

				//Musabaka musabaka11 = new Musabaka() { Turnuva = trn1, HomeTakim = promil, GuestTakim = ponpin, Trh = DateTime.Now };
				Mac mac111 = new Mac() { Turnuva = trn1, Musabaka = promil_delfi, Sira = 1, HomeOyuncu = promilSener, GuestOyuncu = delfiAhmet, Skl = "S" };
				new MacSonuc() { Mac = mac111, SetNo = 1, HomeSayi = 11, GuestSayi = 7 };
				new MacSonuc() { Mac = mac111, SetNo = 2, HomeSayi = 9, GuestSayi = 11 };
				new MacSonuc() { Mac = mac111, SetNo = 3, HomeSayi = 15, GuestSayi = 13 };
				Mac mac112 = new Mac() { Turnuva = trn1, Musabaka = promil_delfi, Sira = 2, HomeOyuncu = promilHakan, GuestOyuncu = delfiMete, Skl = "S" };
				new MacSonuc() { Mac = mac112, SetNo = 1, HomeSayi = 6, GuestSayi = 11 };
				new MacSonuc() { Mac = mac112, SetNo = 2, HomeSayi = 9, GuestSayi = 11 };
				Mac mac113 = new Mac() { Turnuva = trn1, Musabaka = promil_delfi, Sira = 3, HomeOyuncu = promilSevket, GuestOyuncu = delfiRiza, Skl = "S" };
				new MacSonuc() { Mac = mac113, SetNo = 1, HomeSayi = 11, GuestSayi = 3 };
				new MacSonuc() { Mac = mac113, SetNo = 2, HomeSayi = 11, GuestSayi = 5 };
				*/
				//var delfi_promil = new Musabaka() { Turnuva = trn1, HomeTakim = delfi, GuestTakim = promil, Trh = DateTime.Today };
				//var promil_gumusluk = new Musabaka() { Turnuva = trn1, HomeTakim = promil, GuestTakim = gumusluk, Trh = DateTime.Today.AddDays(5) };
				//var promil_peksimet = new Musabaka() { Turnuva = trn1, HomeTakim = promil, GuestTakim = peksimet, Trh = DateTime.Today.AddDays(10) };
				//var promil_dragon = new Musabaka() { Turnuva = trn1, HomeTakim = promil, GuestTakim = dragon, Trh = DateTime.Today.AddDays(15) };
				//var promil_yaliKavak = new Musabaka() { Turnuva = trn1, HomeTakim = promil, GuestTakim = yaliKavak, Trh = DateTime.Today.AddDays(20) };
				//var dragon_yaliKavak = new Musabaka() { Turnuva = trn1, HomeTakim = dragon, GuestTakim = yaliKavak, Trh = DateTime.Today.AddDays(25) };

				//new Mac() { Turnuva = trn1, Musabaka = delfi_promil, Skl = "S", Sira = 8, HomeOyuncu = }
			});
		}
	}
}
