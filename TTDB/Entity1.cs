using System;
using Starcounter;
//using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace TTDB
{
	public static class Constants {
		public const string sepID = "·";		// Ad ·ID
		public const string sepTkm = "•";		// Oyuncu(lar) • Takim
		public const string sepDblOyn = "+";	// Oyuncu + Oyuncu2
		public const string sepSayi = "•";      // 11-05 ▪ 11-08
												//public const string sepSayi = "■▪↔≡";      // 11-05 ▪ 11-08
		public static readonly char[] charsToTrim = { ',', '.', '·', '•', '●', '▪', '│', ' ' };

	}

	public class OyuncuOzet
	{
		public int oMac;
		public int aMac;
		public int aSet;
		public int vSet;
		public int aSayi;
		public int vSayi;
		
		public OyuncuOzet()
		{
			oMac = 0;
			aMac = 0;
			aSet = 0;
			vSet = 0;
			aSayi = 0;
			vSayi = 0;
		}
	}

	[Database]
	public class Oyuncu
	{
		public ulong PK => this.GetObjectNo();
		public string ID => this.GetObjectID();
		public string Ad;
		public string Sex;
		public string Tel;
		public string eMail;
		public Int16 DgmYil;
		public OyuncuOzet Ozet {
			get {
			   OyuncuOzet ozet = new OyuncuOzet();
			
				QueryResultRows<TakimOyuncu> TO = Db.SQL<TakimOyuncu>("select m from TakimOyuncu m where m.Oyuncu = ?", this);
				foreach (var to in TO) {
					QueryResultRows<Mac> hMac = Db.SQL<Mac>("select m from Mac m where m.HomeTakim.Oyuncu = ?", to);
					foreach (var m in hMac) {
						//Console.WriteLine(string.Format("    Mac<{2}-{3}> Set<{4}-{5}> Sayi<{6}-{7}> {0}/{1}", m.GuestOyuncuAd, m.GuestTakimAd, m.Ozet.HomeMac, m.Ozet.GuestMac, m.Ozet.HomeSet, m.Ozet.GuestSet, m.Ozet.HomeSayi, m.Ozet.GuestSayi));
						ozet.oMac++;
						ozet.aMac += m.Ozet.HomeMac;
						ozet.aSet += m.Ozet.HomeSet;
						ozet.vSet += m.Ozet.GuestSet;
						ozet.aSayi += m.Ozet.HomeSayi;
						ozet.vSayi += m.Ozet.GuestSayi;
					}
					QueryResultRows<Mac> gMac = Db.SQL<Mac>("select m from Mac m where m.GuestTakim.Oyuncu = ?", to);
					foreach (var m in gMac) {
						//Console.WriteLine(string.Format("    Mac<{3}-{2}> Set<{5}-{4}> Sayi<{7}-{6}> {0}/{1}", m.HomeOyuncuAd, m.HomeTakimAd, m.Ozet.HomeMac, m.Ozet.GuestMac, m.Ozet.HomeSet, m.Ozet.GuestSet, m.Ozet.HomeSayi, m.Ozet.GuestSayi));
						ozet.oMac++;
						ozet.aMac += m.Ozet.GuestMac;
						ozet.aSet += m.Ozet.GuestSet;
						ozet.vSet += m.Ozet.HomeSet;
						ozet.aSayi += m.Ozet.GuestSayi;
						ozet.vSayi += m.Ozet.HomeSayi;
					}
				}
				return ozet;
			}
		}
		public Oyuncu()
		{
			Ad = "";
			Sex = "E";
		}
	}

	[Database]
	public class Takim
	{
		public ulong PK => this.GetObjectNo();
		public string ID => this.GetObjectID();
		public string Ad;
		public string Tel;
		public string Adres;
		public Int16 KurYil;
		public string Lat;
		public string Lon;
	}

	[Database]
	public class Turnuva
	{
		public ulong PK => this.GetObjectNo();
		public string ID => this.GetObjectID();
		public string Ad;
		public DateTime Trh;

		public string Tarih {
			get {
				return string.Format("{0:dd.MM.yy}", Trh);
			}
		}

		public string TurnuvaInfo {
			get {
				return string.Format("Tarih<{0:dd.MM.yy}> ID<{1}>", Trh, this.GetObjectNo());
			}
		}
	}

	public class TurnuvaTakimOzet
	{
		public Int16 Puan;
		public Int16 MusabakaWin;
		public Int16 MusabakaLost;
		public Int16 MusabakaTie;
		public Int16 MusabakaOynadigi;
		public int MacCnt;

		public TurnuvaTakimOzet()
		{
			Puan = 0;
			MusabakaWin = 0;
			MusabakaLost = 0;
			MusabakaTie = 0;
			MusabakaOynadigi = 0;
			MacCnt = 0;
		}
	}
	public class TT
	{
		public ulong PK;
		public string TakimAd;
		public TurnuvaTakimOzet Ozet;
	}

	[Database]
	public class TurnuvaTakim
	{
		public ulong PK => this.GetObjectNo();
		public string ID => this.GetObjectID();
		public Turnuva Turnuva;
		public Takim Takim;
		public string TakimID => this.Takim.GetObjectID();
		public string TakimAd => Takim != null ? Takim.Ad : "[null]"; // Takim.Ad;
		
		public TurnuvaTakimOzet Ozet {
			get {
				TurnuvaTakimOzet ozet = new TurnuvaTakimOzet();
				// Evinde oynadiklari
				QueryResultRows<Musabaka> hta = Db.SQL<Musabaka>("SELECT ta FROM TTDB.Musabaka ta WHERE ta.Turnuva = ? AND ta.HomeTakim = ?", this.Turnuva, this.Takim);
				foreach(var ta in hta) {
					Ozet o = ta.Ozet;
					ozet.Puan += o.HomePuan;
					if(o.HomePuan > o.GuestPuan)
						ozet.MusabakaWin++;
					else if(o.HomePuan < o.GuestPuan)
						ozet.MusabakaLost++;
					else if(o.HomePuan > 0 || o.GuestPuan > 0)
						ozet.MusabakaTie++;
					ozet.MacCnt += o.Cnt;
				}
				// Misafir oynadiklari
				QueryResultRows<Musabaka> gta = Db.SQL<Musabaka>("SELECT ta FROM TTDB.Musabaka ta WHERE ta.Turnuva = ? AND ta.GuestTakim = ?", this.Turnuva, this.Takim);
				foreach(var ta in gta) {
					Ozet o = ta.Ozet;
					ozet.Puan += o.GuestPuan;
					if(o.GuestPuan > o.HomePuan)
						ozet.MusabakaWin++;
					else if(o.GuestPuan < o.HomePuan)
						ozet.MusabakaLost++;
					else if(o.HomePuan > 0 || o.GuestPuan > 0)
						ozet.MusabakaTie++;
					ozet.MacCnt += o.Cnt;
				}
				ozet.MusabakaOynadigi = (short)(ozet.MusabakaWin + ozet.MusabakaLost + ozet.MusabakaTie);
				var trnNo = this.Turnuva.GetObjectNo();
				string tkm = string.Format("{0} {1}", this.Takim.GetObjectNo(), this.TakimAd);
				return ozet;
			}
		}
	}

   [Database]
   public class TakimOyuncu
   {
		public string ID => this.GetObjectID();
		public Turnuva Turnuva;
		public Takim Takim;
		public Oyuncu Oyuncu;
		public string OyuncuAd => Oyuncu != null ? Oyuncu.Ad : "";
 		//public string TakimAd => Takim.Ad;
		//public string TurnuvaAd => Turnuva.Ad;
	}

	[Database]
	public class Musabaka
	{
		public string ID => this.GetObjectID();
		public Turnuva Turnuva;
		public Takim HomeTakim;
		public Takim GuestTakim;
		public DateTime Trh;
		public string Yeri;
		public string HomeTakimAd => HomeTakim.Ad;
		public string GuestTakimAd => GuestTakim.Ad;
		public string Tarih {
			get {
				return string.Format("{0:dd.MM.yy}", Trh);
			}
		}
		//public string TurnuvaAd => Turnuva.Ad;
		//public string HomeTakimAd => HomeTakim != null ? HomeTakim.Ad : "[null]";
		//public string GuestTakimAd => GuestTakim != null ? GuestTakim.Ad : "[null]";

		public string MusabakaAd {
			get {
				return string.Format("{4:dd.MM.yy} {0} <{1}-{2}> {3}", HomeTakim.Ad, Ozet.HomePuan, Ozet.GuestPuan, GuestTakim.Ad, Trh);
			}
	    }
	   
		public string MusabakaInfo {
			get {
				//return string.Format("Puan<{0}-{1}> Maç<{2}-{3}> Set<{4}-{5}> Sayı<{6}-{7}> Tarih<{8:dd.MM.yy}> ID<{9}>", Ozet.HomePuan, Ozet.GuestPuan, Ozet.HomeMac, Ozet.GuestMac, Ozet.HomeSet, Ozet.GuestSet, Ozet.HomeSayi, Ozet.GuestSayi, Trh, this.GetObjectNo());
				return string.Format("P<{0}-{1}> M<{2}-{3}> S<{4}-{5}> #<{6}-{7}>", Ozet.HomePuan, Ozet.GuestPuan, Ozet.HomeMac, Ozet.GuestMac, Ozet.HomeSet, Ozet.GuestSet, Ozet.HomeSayi, Ozet.GuestSayi);
			}
		}
	
		public Ozet Ozet {
			get {
				Ozet ozet = new Ozet();
				
				QueryResultRows<Mac> ms = Db.SQL<Mac>("SELECT ms FROM TTDB.Mac ms WHERE ms.Musabaka = ?", this);
				foreach (var m in ms) {
					Ozet o = m.Ozet;

					ozet.HomePuan += o.HomePuan;
					ozet.HomeMac += o.HomeMac;
					ozet.HomeSet += o.HomeSet;
					ozet.HomeSayi += o.HomeSayi;
					
					ozet.GuestPuan += o.GuestPuan;
					ozet.GuestMac += o.GuestMac;
					ozet.GuestSet += o.GuestSet;
					ozet.GuestSayi += o.GuestSayi;

					ozet.Cnt += o.Cnt;
				}
				
				var aaaa = this.GetObjectNo();
				var bbbb = this.HomeTakimAd + " " + this.GuestTakimAd;
				return ozet;
			}
		}
	}

	public class Ozet {
		public Int16 HomePuan;
		public Int16 GuestPuan;
		public Int16 HomeMac;
		public Int16 HomeSet;
		public Int16 GuestMac;
		public Int16 GuestSet;
		
		public Int16 HomeSayi;
		public Int16 GuestSayi;
		public string Puanlar;   // H:G
		public string Setler;   // H:G
		public string Sayilar;  // H:G, H:G,...
		public int Cnt;
		public Ozet()
		{
			Cnt = 0;
			HomePuan = 0;
			HomeMac = 0;
			HomeSet = 0;
			GuestPuan = 0;
			GuestMac = 0;
			GuestSet = 0;
			
			Puanlar = "";
			Setler = "";
			Sayilar = "";
		}
	}

	[Database]
	public partial class Mac
	{
		public string ID => this.GetObjectID();
		public Turnuva Turnuva;
		public Musabaka Musabaka;
		public Oyuncu HomeOyuncu;
		public Oyuncu HomeOyuncu2;
		public Oyuncu GuestOyuncu;
		public Oyuncu GuestOyuncu2;
		public string Skl;  // Single/Double/MixDouble
		public Int16 Sira;

		public string HomeOyuncuAd {
			get {
				return $"{(HomeOyuncu == null ? "" : Hlpr.GetFirstName(HomeOyuncu.Ad))}{(HomeOyuncu2 == null ? "" : Constants.sepDblOyn + Hlpr.GetFirstName(HomeOyuncu2.Ad))}";
			}
		}
		public string GuestOyuncuAd {
			get {
				return $"{(GuestOyuncu == null ? "" : Hlpr.GetFirstName(GuestOyuncu.Ad))}{(GuestOyuncu2 == null ? "" : Constants.sepDblOyn + Hlpr.GetFirstName(GuestOyuncu2.Ad))}";
			}
		}

		public string HomeOyuncuInfo {
			get {
				return $"{(HomeOyuncu == null ? "" : Hlpr.GetFirstName(HomeOyuncu.Ad))}{(HomeOyuncu2 == null ? "" : Constants.sepDblOyn + Hlpr.GetFirstName(HomeOyuncu2.Ad))}{(Musabaka == null ? "" : Constants.sepTkm + Musabaka.HomeTakim.Ad)}";
			}
		}
		
		public string GuestOyuncuInfo {
			get {
				string info = "";
				if(GuestOyuncu != null)
					info = Hlpr.GetFirstName(GuestOyuncu.Ad);
				if(GuestOyuncu2 != null)
					info += Constants.sepDblOyn + Hlpr.GetFirstName(GuestOyuncu2.Ad);
				if(Musabaka != null)
					info += Constants.sepTkm + Musabaka.GuestTakim.Ad;
				return info;
			}
		}

		public string MusabakaTarih {
			get {
				return string.Format("{0:dd.MM.yy}", Musabaka.Trh);
			}
		}

		public Ozet Ozet {
			get {
				Ozet ozet = new Ozet();
				string sayilar = "";
				Int16 hSet = 0;
				Int16 gSet = 0;
				int i = 0;
				QueryResultRows<MacSonuc> ms = Db.SQL<MacSonuc>("SELECT ms FROM TTDB.MacSonuc ms WHERE ms.Mac = ?", this);
				foreach (var m in ms) {
					ozet.HomeSayi += m.HomeSayi;
					ozet.GuestSayi += m.GuestSayi;

					sayilar += string.Format("{0}-{1}{2}", m.HomeSayi.ToString().PadLeft(2, '0'), m.GuestSayi.ToString().PadLeft(2, '0'), Constants.sepSayi);
					if (m.HomeSayi > m.GuestSayi)
					   hSet++;
					else
					   gSet++;

					ozet.Cnt++;
				}
				
				if (hSet > gSet) {
					ozet.HomeMac = 1;
					ozet.HomePuan = 2;
					if(this.Skl == "D")
						ozet.HomePuan = 3;
				}
				else if (hSet < gSet) {
					ozet.GuestMac = 1;
					ozet.GuestPuan = 2;
					if(this.Skl == "D")
						ozet.GuestPuan = 3;
				}
				ozet.HomeSet = hSet;
				ozet.GuestSet = gSet;
				
				ozet.Puanlar = string.Format("{0}-{1}", ozet.HomePuan, ozet.GuestPuan);
				ozet.Setler = string.Format("{0}-{1} ", hSet, gSet);
				ozet.Sayilar = sayilar.TrimEnd(Constants.charsToTrim);
				
				return ozet;
			}
		}

		public static Ozet deneme (string txt) {
			Ozet ozet = new Ozet();
			string sayilar = "";
			Int16 hSet = 0;
			Int16 gSet = 0;
			return ozet;
		}
	}

	[Database]
	public class MacSonuc
	{
		public string ID => this.GetObjectID();
		public Mac Mac;
		public Int16 SetNo;
		public Int16 HomeSayi;
		public Int16 GuestSayi;
	}

	public class TurnuvaOyuncuOzet
	{
		public string OyuncuID;
		public string OyuncuAd;
		public string TakimAd;
		public Int16 Puan;
		public Int16 MacO;
		public Int16 MacG;
		public Int16 MacM;
		public Int16 SetA;
		public Int16 SetV;
		public Int16 SayiA;
		public Int16 SayiV;
		
		public TurnuvaOyuncuOzet()
		{
			OyuncuID = "";	
			OyuncuAd = "";
			TakimAd = "";
			Puan = 0;
			MacO = 0;
			MacG = 0;
			MacM = 0;
			SetA = 0;
			SetV = 0;
			SayiA = 0;
			SayiV = 0;
		}
	}

	public class Hooks
	{
		public void AddHooks()
		{
			Hook<Oyuncu>.BeforeDelete += (s, obj) => {
				Console.WriteLine("Hooked: Object {0} is to be deleted", obj.GetObjectNo());
			};
		}
	}


	public static class Hlpr
	{
		public static void sener()
		{
			foreach(var value in TurnuvaOyuncularOzet("VC")) {
				Console.Write(value);
				Console.Write(" ");
			}
			Console.WriteLine();
		}

		public static string GetFirstName(string ad)
		{
			return ad.Substring(0, (ad+" ").IndexOf(' '));
		}

		public static string GetIdFromText(string txt)
		{
			return txt.Substring(txt.IndexOf('·') + 1);
		}

		public static Tuple<string, string> GetIdsFromText(string txt)
		{
			var fp = txt.Substring(0, txt.IndexOf('·'));
			var id = txt.Substring(txt.IndexOf('·') + 1);
			var tuple = new Tuple<string, string>(fp, id);
			return tuple;
		}

		public static void DeleteTrnMsbMac(string MusabakaID)
		{
			var msbObj = (Musabaka)DbHelper.FromID(DbHelper.Base64DecodeObjectID(MusabakaID));
			var maclar = Db.SQL<Mac>("SELECT m FROM Mac m WHERE m.Musabaka = ?", msbObj);
			Db.Transact(() => {
				foreach(var mac in maclar) {
					var macSonuclar = Db.SQL<MacSonuc>("SELECT m FROM MacSonuc m WHERE m.Mac = ?", mac);
					foreach(var macSonuc in macSonuclar) {
						macSonuc.Delete();
					}
					mac.Delete();
				}
			});
		}

		public static void CreateTrnMsbMac(string MusabakaID)
		{
			var msbObj = (Musabaka)DbHelper.FromID(DbHelper.Base64DecodeObjectID(MusabakaID));
			if(Db.SQL<Mac>("SELECT m FROM Mac m WHERE m.Musabaka = ?", msbObj).First == null) {
				Db.Transact(() => {
					for(int i = 0; i < 8; i++) {
						var mac = new Mac();
						mac.Turnuva = msbObj.Turnuva;
						mac.Musabaka = msbObj;
						mac.Skl = "S";
						mac.Sira = (short)(i + 1);
						for(int k = 1; k < 4; k++) {
							var snc = new MacSonuc();
							snc.Mac = mac;
							snc.SetNo = (short)k;
						}
					}
					for(int i = 0; i < 4; i++) {
						var mac = new Mac();
						mac.Turnuva = msbObj.Turnuva;
						mac.Musabaka = msbObj;
						mac.Skl = "D";
						mac.Sira = (short)(i + 1);
						for(int k = 1; k < 4; k++) {
							var snc = new MacSonuc();
							snc.Mac = mac;
							snc.SetNo = (short)k;
						}
					}
				});
			}
		}

		public static IEnumerable<TurnuvaOyuncuOzet> TurnuvaOyuncularOzet(string turnuvaID)
		{
			var trnObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(turnuvaID));

			QueryResultRows<TakimOyuncu> to = Db.SQL<TakimOyuncu>("select m from TakimOyuncu m where m.Turnuva = ?", trnObj);

			foreach(var t in to) {
				TurnuvaOyuncuOzet too = new TurnuvaOyuncuOzet();

				int oMac = 0,   // Oynadigi
				   aMac = 0,   // Aldigi
				   aSet = 0,   // Aldigi
				   vSet = 0,   // Verdigi
				   aSay = 0,
				   vSay = 0;

				//Console.WriteLine(string.Format("    {0}/{1}", t.OyuncuAd, t.TakimAd));
				if(t.Oyuncu != null) {
					too.OyuncuID = t.Oyuncu.GetObjectID();
					too.OyuncuAd = t.Oyuncu.Ad;
				}
				too.TakimAd = t.Takim.Ad;

				// Home olarak oynadiklari
				QueryResultRows<Mac> hMac = Db.SQL<Mac>("select m from Mac m where m.HomeOyuncu = ?", t.Oyuncu);

				foreach(var m in hMac) {
					oMac++;
					Ozet o = m.Ozet;
					aMac += o.HomeMac;
					aSet += o.HomeSet;
					vSet += o.GuestSet;
					aSay += o.HomeSayi;
					vSay += o.GuestSayi;
				}
				// Guest olarak oynadiklar
				QueryResultRows<Mac> gMac = Db.SQL<Mac>("select m from Mac m where m.GuestOyuncu = ?", t.Oyuncu);

				foreach(var m in gMac) {
					oMac++;
					Ozet o = m.Ozet;
					aMac += o.GuestMac;
					aSet += o.GuestSet;
					vSet += o.HomeSet;
					aSay += o.GuestSayi;
					vSay += o.HomeSayi;
				}

				too.Puan = (Int16)(aMac * 2 + (oMac - aMac));
				too.MacO = (Int16)oMac;
				too.MacG = (Int16)aMac;
				too.MacM = (Int16)(oMac - aMac);
				too.SetA = (Int16)aSet;
				too.SetV = (Int16)vSet;
				too.SayiA = (Int16)aSay;
				too.SayiV = (Int16)vSay;

				//ltoo.Add(too);
				yield return too;
			}
		}
   }
}