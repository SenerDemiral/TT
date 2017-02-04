using System;
using Starcounter;
//using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TTDB
{
	[Database]
	public class Parent
	{
		public string Name { get; set; }

		QueryResultRows<Child> Children {
			get {
				return Db.SQL<Child>("SELECT c FROM TTDB.Child c WHERE c.Parent = ?", this);
			}
		}
	}

	[Database]
	public class Child
	{
		public string Name { get; set; }
		public Parent Parent { get; set; }
	}



	public static class Constants {
		public const string sepID = "·";		// Ad ·ID
		public const string sepTkm = " ";		// Oyuncu(lar) • Takim
		public const string sepDblOyn = "+";	// Oyuncu + Oyuncu2
		public const string sepSayi = " ";      // 11-05 • 11-08
											   //public const string sepSayi = "■▪↔≡";      // 11-05 ▪ 11-08
		public static readonly char[] charsToTrim = { ',', '.', '·', '•', '●', '▪', '│', ' ' };
	}

	public static class RatingChart 
	{
		public static void InitRank() {
			Db.Transact(() => {
				QueryResultRows<Oyuncu> Oyuncus = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o");
				foreach(var o in Oyuncus) {
					o.Rank = 1900;
					o.NOPX = 0;
					o.NOPXtxt = "";
				}
			});
		}

		public static void InitRank2()
		{
			Db.Transact(() => {
				QueryResultRows<Oyuncu> Oyuncus = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o");
				foreach(var o in Oyuncus) {
					o.Rank2 = 1900;
					o.NOPX2 = 0;
					o.NOPX2txt = "";

					o.oMacS = 0;
					o.aMacS = 0;
					o.avMacS = 0;
					o.avSetS = 0;

					o.Rank3 = 0;
				}
			});
		}
		public static void RankCalcultion2(DateTime startDate, DateTime endDate)
		{
			int nopx = 0;
			int avMacS = 0;
			int avSetS = 0;

			Db.Transact(() => {
				QueryResultRows<Mac> Macs = Db.SQL<Mac>("SELECT m FROM Mac m WHERE m.Skl = ? AND m.Musabaka.Trh >= ? AND m.Musabaka.Trh <= ? AND m.HomeOyuncu IS NOT NULL AND m.GuestOyuncu IS NOT NULL", "S", startDate, endDate);

				foreach(var m in Macs) {
					var ozt = m.Ozet;
					// Winner +Sira, Looser -Sira
					nopx = (11 - m.Sira);
					m.HomeOyuncu.oMacS += 1;
					m.GuestOyuncu.oMacS += 1;
					avMacS = 1;
					avSetS = Math.Abs(ozt.HomeSetS - ozt.GuestSetS);
					m.HomeOyuncu.NOPX2txt += string.Format("{0} ", m.Sira);
					m.GuestOyuncu.NOPX2txt += string.Format("{0} ", m.Sira);
					if(ozt.HomePuan > ozt.GuestPuan) {
						m.HomeOyuncu.Rank2 += nopx;
						m.GuestOyuncu.Rank2 -= nopx;

						m.HomeOyuncu.aMacS += 1;
						m.HomeOyuncu.avMacS += avMacS;
						m.HomeOyuncu.avSetS += avSetS;
						m.GuestOyuncu.avMacS -= avMacS;
						m.GuestOyuncu.avSetS -= avSetS;

						m.HomeOyuncu.Rank3 += nopx;
						m.GuestOyuncu.Rank3 += 1; //-= avMacS + nopx;
												  //m.HomeOyuncu.Rank3 += avMacS + nopx;
												  //m.GuestOyuncu.Rank3 -= m.Sira;
												  //m.GuestOyuncu.Rank3 -= nopx;
					}
					else {
						m.HomeOyuncu.Rank2 -= nopx;
						m.GuestOyuncu.Rank2 += nopx;

						m.GuestOyuncu.aMacS += 1;
						m.HomeOyuncu.avMacS -= avMacS;
						m.HomeOyuncu.avSetS -= avSetS;
						m.GuestOyuncu.avMacS += avMacS;
						m.GuestOyuncu.avSetS += avSetS;

						m.HomeOyuncu.Rank3 += 1; //-= avMacS + nopx;
						m.GuestOyuncu.Rank3 += nopx;
						//m.HomeOyuncu.Rank3 -= m.Sira;
						//m.HomeOyuncu.Rank3 -= nopx;
						//m.GuestOyuncu.Rank3 += avMacS + nopx;
					}
				}

				QueryResultRows<Oyuncu> Oyuncu = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o");
				foreach(var o in Oyuncu) {
					o.Rank = o.Rank3 + 1900;
				}
			});

		}

		public static void Write2File() {
			//QueryResultRows<Oyuncu> Oyuncu = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o ORDER BY o.Rank3 DESC, o.avMacS DESC");
			QueryResultRows<Oyuncu> Oyuncu = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o WHERE o.oMacS > ? ORDER BY o.Rank DESC", 0);

			using(System.IO.StreamWriter writer = new System.IO.StreamWriter("C:\\Starcounter\\log.txt", true)) {
				writer.WriteLine("Oyuncu,oMacS,aMacS,avMacS,avSetS,RankS,OynadigiSiraS,Rank,RankPuan");
				foreach(var o in Oyuncu) {
					writer.WriteLine($"{o.Ad},{o.oMacS},{o.aMacS},{o.avMacS},{o.avSetS},{o.Rank3},{o.NOPX2txt},{o.Rank},{o.NOPXtxt}");
				}
			}
		}

		// Rank calculation 
		public static void RankCalcultion(DateTime startDate, int idx) {
			DateTime endDate = startDate.AddDays(5);
			int ER = 0;
			int UR = 0;
			int nopx = 0;
			int hRank = 0;
			int gRank = 0;

			Db.Transact(() => {
				QueryResultRows<Mac> Macs = Db.SQL<Mac>("SELECT m FROM Mac m WHERE m.Skl = ? AND m.Musabaka.Trh >= ? AND m.Musabaka.Trh <= ? AND m.HomeOyuncu IS NOT NULL AND m.GuestOyuncu IS NOT NULL", "S", startDate, endDate);

				foreach(var m in Macs) {
					var ozt = m.Ozet;

					hRank = m.HomeOyuncu == null ? 1900 : m.HomeOyuncu.Rank;
					gRank = m.GuestOyuncu == null ? 1900 : m.GuestOyuncu.Rank;

					var result = NOPX(hRank, gRank);
					ER = result.Item1;
					UR = result.Item2;

					if(hRank >= gRank) {
						if(ozt.HomePuan > ozt.GuestPuan) {
							nopx = ER;
							m.HomeOyuncu.NOPX += ER;
							m.GuestOyuncu.NOPX -= ER;
						}
						else {
							nopx = UR;
							m.HomeOyuncu.NOPX -= UR;
							m.GuestOyuncu.NOPX += UR;
						}
					}
					else {
						if(ozt.GuestPuan > ozt.HomePuan) {
							nopx = ER;
							m.HomeOyuncu.NOPX -= ER;
							m.GuestOyuncu.NOPX += ER;
						}
						else {
							nopx = UR;
							m.HomeOyuncu.NOPX += UR;
							m.GuestOyuncu.NOPX -= UR;
						}
					}

					Console.WriteLine(string.Format("{0}/{6} {1} <> {2}  {3}-{4} {5}", m.Musabaka.Trh, m.HomeOyuncuAd, m.GuestOyuncuAd, ozt.HomePuan, ozt.GuestPuan, nopx, idx));
					//Console.WriteLine(string.Format("{0}/{6} {1} <> {2}  {3}-{4} {5}", m.Musabaka.Trh, m.HomeOyuncuAd, m.GuestOyuncuAd, ozt.HomePuan, ozt.GuestPuan, nopx, System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(m.Musabaka.Trh, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday)));
				}

				// Add NOPX to Rank
				QueryResultRows<Oyuncu> Oyuncus = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o");
				foreach(var o in Oyuncus) {
					o.Rank += o.NOPX;
					//o.NOPXtxt += string.Format("{0} ", o.NOPX);
					//o.NOPXtxt += $"{(o.NOPX == 0 ? "±" : "")}{(o.NOPX > 0 ? "+" : "")}{o.NOPX}│";
					if(o.NOPX == 0)
						o.NOPXtxt += " • ";
					else if(o.NOPX > 0)
						o.NOPXtxt += "+" + o.NOPX;
					else
						o.NOPXtxt += o.NOPX;
					o.NOPXtxt += "│";
					//o.NOPXtxt += $"{(o.NOPX == 0 ? "XX" : "")}{(o.NOPX > 0 ? "+"+{o.NOPX} : "")}{(o.NOPX < 0 ? "{o.NOPX}" : "")}│";
					o.NOPX = 0;
				}
			});

		}



		//NumberOfPointsExchange between players
		public static Tuple<int, int> NOPX(int HomePlayerRate, int GusetPlayerRate) {
			int PS = Math.Abs(HomePlayerRate - GusetPlayerRate);	// Point Spread between players
			int ER = 0; // ExpectedResult
			int UR = 0;	// UpsetResult
			
			if(PS < 13) {
				ER = 8;
				UR = 8;
			}
			else if(PS < 38) {
				ER = 7;
				UR = 10;
			}
			else if(PS < 63) {
				ER = 6;
				UR = 13;
			}
			else if(PS < 88) {
				ER = 5;
				UR = 16;
			}
			else if(PS < 113) {
				ER = 4;
				UR = 20;
			}
			else if(PS < 138) {
				ER = 3;
				UR = 25;
			}
			else if(PS < 163) {
				ER = 2;
				UR = 30;
			}
			else if(PS < 188) {
				ER = 2;
				UR = 35;
			}
			else if(PS < 213) {
				ER = 1;
				UR = 40;
			}
			else if(PS < 238) {
				ER = 1;
				UR = 45;
			}
			else {
				ER = 0;
				UR = 50;
			}

			var tuple = new Tuple<int, int>(ER, UR);
			return tuple;

			//var result = TTDB.Hlpr.GetIdsFromText(takimAd);
			// result.Item1 -> ER
			// result.Item2 -> UR
			//oyuncular.TakimID = result.Item2;
			//oyuncular.Heading = result.Item1 + " Oyuncuları"; */

		}
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

		public int Rank;
		public int NOPX;
		public string NOPXtxt;
		public int Rank2;
		public int NOPX2;
		public string NOPX2txt;
		// Single oynadiklari
		public int Rank3;
		public int oMacS;
		public int aMacS;
		
		public int avMacS;
		public int avSetS;	// aSet - vSet

		//public int InitialRating;			// Ilk Rating,  manuel entry
		//public int CurrentRating;			// Guncel Rating, computed per request
		//public DateTime CurrentRatingDate;	// Rating son guncelleme tarihi. 1.Yari bitiminde, 2.yari bitiminde vs.

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
		public int TrnPuan;
		public int PuanAV;
		public int PuanA;
		public int PuanV;
		public int MsbkO;
		public int MsbkA;
		public int MsbkB;
		public int MsbkV;
		public int MacA;
		public int MacV;
		public int SetA;
		public int SetV;
		public int SayiA;
		public int SayiV;
		public int MacCnt;

		public TurnuvaTakimOzet()
		{
			TrnPuan = 0;
			PuanAV = 0;
			PuanA = 0;
			PuanV = 0 ;
			MsbkO = 0;
			MsbkA = 0;
			MsbkB = 0;
			MsbkV = 0;
			MacA = 0;
			MacV = 0;
			SetA = 0;
			SetV = 0;
			SayiA = 0;
			SayiV = 0;

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

					ozet.PuanA += o.HomePuan;
					ozet.PuanV += o.GuestPuan;
					ozet.MacA += o.HomeMac;
					ozet.MacV += o.GuestMac;
					ozet.SetA += o.HomeSet;
					ozet.SetV += o.GuestSet;
					ozet.SayiA += o.HomeSayi;
					ozet.SayiV += o.GuestSayi;
					if(o.HomePuan > o.GuestPuan)
						ozet.MsbkA++;
					else if(o.HomePuan < o.GuestPuan)
						ozet.MsbkV++;
					else if(o.HomePuan > 0 || o.GuestPuan > 0)
						ozet.MsbkB++;
					ozet.MacCnt += o.Cnt;
				}
				// Misafir oynadiklari
				QueryResultRows<Musabaka> gta = Db.SQL<Musabaka>("SELECT ta FROM TTDB.Musabaka ta WHERE ta.Turnuva = ? AND ta.GuestTakim = ?", this.Turnuva, this.Takim);
				foreach(var ta in gta) {
					Ozet o = ta.Ozet;
					
					ozet.PuanA += o.GuestPuan;
					ozet.PuanV += o.HomePuan;
					ozet.MacA += o.GuestMac;
					ozet.MacV += o.HomeMac;
					ozet.SetA += o.GuestSet;
					ozet.SetV += o.HomeSet;
					ozet.SayiA += o.GuestSayi;
					ozet.SayiV += o.HomeSayi;
					if(o.GuestPuan > o.HomePuan)
						ozet.MsbkA++;
					else if(o.GuestPuan < o.HomePuan)
						ozet.MsbkV++;
					else if(o.HomePuan > 0 || o.GuestPuan > 0)
						ozet.MsbkB++;
					ozet.MacCnt += o.Cnt;
				}
				ozet.MsbkO = ozet.MsbkA + ozet.MsbkB + ozet.MsbkV;
				var trnNo = this.Turnuva.GetObjectNo();
				string tkm = string.Format("{0} {1}", this.Takim.GetObjectNo(), this.TakimAd);

				ozet.TrnPuan = ozet.MsbkA * 3 + ozet.MsbkB;
				ozet.PuanAV = ozet.PuanA - ozet.PuanV;
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
		public int HomePuan;
		public int HomeMac;
		public int HomeSet;
		public int HomeSayi;
		public int GuestPuan;
		public int GuestMac;
		public int GuestSet;
		public int GuestSayi;

		public int HomePuanD;
		public int HomeMacOD;
		public int HomeMacGD;
		public int HomeSetD;
		public int HomeSayiD;
		public int GuestPuanD;
		public int GuestMacOD;
		public int GuestMacGD;
		public int GuestSetD;
		public int GuestSayiD;

		public int HomePuanS;
		public int HomeMacOS;
		public int HomeMacGS;
		public int HomeSetS;
		public int HomeSayiS;
		public int GuestPuanS;
		public int GuestMacOS;
		public int GuestMacGS;
		public int GuestSetS;
		public int GuestSayiS;

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
			HomeSayi = 0;
			GuestPuan = 0;
			GuestMac = 0;
			GuestSet = 0;
			GuestSayi = 0;

			
			HomePuanD = 0;
			HomeMacOD = 0;
			HomeMacGD = 0;
			HomeSetD = 0;
			HomeSayiD = 0;
			GuestPuanD = 0;
			GuestMacOD = 0;
			GuestMacGD = 0;
			GuestSetD = 0;
			GuestSayiD = 0;

			HomePuanS = 0;
			HomeMacOS = 0;
			HomeMacGS = 0;
			HomeSetS = 0;
			HomeSayiS = 0;
			GuestPuanS = 0;
			GuestMacOS = 0;
			GuestMacGS = 0;
			GuestSetS = 0;
			GuestSayiS = 0;

			Puanlar = "";
			Setler = "";
			Sayilar = "";
		}
	}

	public class OyuncuMac
	{

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
				QueryResultRows<MacSonuc> ms = Db.SQL<MacSonuc>("SELECT ms FROM TTDB.MacSonuc ms WHERE ms.Mac = ?", this);
				foreach (var m in ms) {
					ozet.HomeSayi += m.HomeSayi;
					ozet.GuestSayi += m.GuestSayi;

					sayilar += string.Format("{0}-{1}{2}", m.HomeSayi.ToString().PadLeft(2, '0'), m.GuestSayi.ToString().PadLeft(2, '0'), Constants.sepSayi);
					if(m.HomeSayi > m.GuestSayi) {
						ozet.HomeSet++;
						if(Skl == "D")
							ozet.HomeSetD++;
						else
							ozet.HomeSetS++;
					}
					else { 
						ozet.GuestSet++;
						if(Skl == "D")
							ozet.GuestSetD++;
						else
							ozet.GuestSetS++;
					}

					ozet.Cnt++;
					if(Skl == "D") {
						ozet.HomeSayiD += m.HomeSayi;
						ozet.GuestSayiD += m.GuestSayi;
					}
					else {
						ozet.HomeSayiS += m.HomeSayi;
						ozet.GuestSayiS += m.GuestSayi;
					}
				}

				if(Skl == "D") {
					ozet.HomeMacOD++;
					ozet.GuestMacOD++;
				}
				else {
					ozet.HomeMacOS++;
					ozet.GuestMacOS++;
				}

				if(ozet.HomeSet > ozet.GuestSet) {
					ozet.HomeMac = 1;
					if(Skl == "D") {
						ozet.HomePuan = 3;
						ozet.HomePuanD = 3;
						ozet.HomeMacGD = 1;
					}
					else {
						ozet.HomePuan = 2;
						ozet.HomePuanS = 2;
						ozet.HomeMacGS = 1;
					}
				}
				else if (ozet.HomeSet < ozet.GuestSet) {
					ozet.GuestMac = 1;
					if(Skl == "D") {
						ozet.GuestPuan = 3;
						ozet.GuestPuanD = 3;
						ozet.GuestMacGD = 1;
					}
					else {
						ozet.GuestPuan = 2;
						ozet.GuestPuanS = 2;
						ozet.GuestMacGS = 1;
					}
				}
				
				ozet.Puanlar = string.Format("{0}-{1}", ozet.HomePuan, ozet.GuestPuan);
				ozet.Setler = string.Format("{0}-{1} ", ozet.HomeSet, ozet.GuestSet);
				ozet.Sayilar = sayilar.TrimEnd(Constants.charsToTrim);
				
				return ozet;
			}
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

	public class TurnuvaOyuncuMacOzet
	{
		public string MusabakaTarih;
		public string Skl;
		public Int16 Sira;
		public string Sonuc;
		public bool IsHome;
		public string HomeOyuncuInfo;
		public string GuestOyuncuInfo;
		public string Setler;
		public string Sayilar;

		public TurnuvaOyuncuMacOzet() {
			MusabakaTarih = "";
			Skl = "";
			Sira = 0;
			Sonuc = "X";
			IsHome = false;
			HomeOyuncuInfo = "";
			GuestOyuncuInfo = "";
			Setler = "";
			Sayilar = "";
		}
	}

	public class TurnuvaOyuncuOzet
	{
		public string OyuncuID;
		public string OyuncuAd;
		public string TakimAd;
		public int Puan;
		public int MacO;
		public int MacG;
		public int MacM;
		public int SetA;
		public int SetV;
		public int SayiA;
		public int SayiV;

		public int PuanD;
		public int MacOD;
		public int MacGD;
		public int MacMD;
		public int SetAD;
		public int SetVD;
		public int SayiAD;
		public int SayiVD;

		public int PuanS;
		public int MacOS;
		public int MacGS;
		public int MacMS;
		public int SetAS;
		public int SetVS;
		public int SayiAS;
		public int SayiVS;

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

			PuanD = 0;
			MacOD = 0;
			MacGD = 0;
			MacMD = 0;
			SetAD = 0;
			SetVD = 0;
			SayiAD = 0;
			SayiVD = 0;

			PuanS = 0;
			MacOS = 0;
			MacGS = 0;
			MacMS = 0;
			SetAS = 0;
			SetVS = 0;
			SayiAS = 0;
			SayiVS = 0;
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

		public static IEnumerable<TurnuvaOyuncuMacOzet> TurnuvaOyuncuMaclarOzet(string turnuvaID, string oyuncuID) 
		{
			
			var trnObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(turnuvaID));
			var oynObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(oyuncuID));
			var oynNo = oynObj.GetObjectNo();

			var maclar = Db.SQL<Mac>("SELECT mm FROM MAC mm WHERE mm.Turnuva = ? AND (mm.HomeOyuncu = ? OR mm.GuestOyuncu = ? OR mm.HomeOyuncu2 = ? OR mm.GuestOyuncu2 = ?)",
						trnObj, oynObj, oynObj, oynObj, oynObj)
					.OrderByDescending(x => x.Musabaka.Trh);

			foreach(var mac in maclar) {
				TurnuvaOyuncuMacOzet tomo = new TurnuvaOyuncuMacOzet();
				tomo.IsHome = false;
				if((mac.HomeOyuncu != null && oynNo == mac.HomeOyuncu.GetObjectNo()) || (mac.HomeOyuncu2 != null && oynNo == mac.HomeOyuncu2.GetObjectNo()))     // HomeOyuncu degilse Guestdir
					tomo.IsHome = true;
				// Beraberlik olmaz
				var ozet = mac.Ozet;
				tomo.Sonuc = "X";
				if(tomo.IsHome) {
					if(ozet.HomeSet > ozet.GuestSet)
						tomo.Sonuc = "G";
					else if(ozet.HomeSet < ozet.GuestSet)
						tomo.Sonuc = "M";
					else if(ozet.HomeSet == ozet.GuestSet && ozet.HomeSet != 0)
						tomo.Sonuc = "B";
				}
				else {
					if(ozet.HomeSet > ozet.GuestSet)
						tomo.Sonuc = "M";
					else if(ozet.HomeSet < ozet.GuestSet)
						tomo.Sonuc = "G";
					else if(ozet.HomeSet == ozet.GuestSet && ozet.HomeSet != 0)
						tomo.Sonuc = "B";
				}

				tomo.MusabakaTarih = mac.MusabakaTarih;
				tomo.Skl = mac.Skl;
				tomo.Sira = mac.Sira;
				tomo.HomeOyuncuInfo = mac.HomeOyuncuInfo;
				tomo.GuestOyuncuInfo = mac.GuestOyuncuInfo;
				tomo.Setler = ozet.Setler;
				tomo.Sayilar = ozet.Sayilar;

				yield return tomo;
			}
		}
		public static IEnumerable<TurnuvaOyuncuOzet> TurnuvaTakimOyuncularOzet(string turnuvaID, string takimID)
		{
			var trnObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(turnuvaID));
			var tkmObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(takimID));

			var oyuncular = Db.SQL<TakimOyuncu>("select m from TakimOyuncu m where m.Turnuva = ? AND m.Takim = ?", trnObj, tkmObj).Select(m => m.Oyuncu).Distinct();    // return IEnumerable<Oyuncu>

			foreach(var oyuncu in oyuncular) {
				TurnuvaOyuncuOzet too = new TurnuvaOyuncuOzet();
				if(oyuncu != null) {
					too.OyuncuID = oyuncu.GetObjectID();
					too.OyuncuAd = oyuncu.Ad;
				}
				too.TakimAd = "";

				// Home olarak oynadiklari
				QueryResultRows<Mac> hMac = Db.SQL<Mac>("select m from Mac m where m.Turnuva = ? AND m.Musabaka.HomeTakim = ? AND (m.HomeOyuncu = ? OR m.HomeOyuncu2 = ?)", trnObj, tkmObj, oyuncu, oyuncu);
				foreach(var m in hMac) {
					Ozet ozt = m.Ozet;

					too.MacO++;
					too.MacOD += ozt.HomeMacOD;
					too.MacOS += ozt.HomeMacOS;

					too.Puan += ozt.HomePuan;
					too.MacG += ozt.HomeMac;
					too.SetA += ozt.HomeSet;
					too.SetV += ozt.GuestSet;
					too.SayiA += ozt.HomeSayi;
					too.SayiV += ozt.GuestSayi;

					too.PuanD += ozt.HomePuanD;
					too.MacGD += ozt.HomeMacGD;
					too.SetAD += ozt.HomeSetD;
					too.SetVD += ozt.GuestSetD;
					too.SayiAD += ozt.HomeSayiD;
					too.SayiVD += ozt.GuestSayiD;

					too.PuanS += ozt.HomePuanS;
					too.MacGS += ozt.HomeMacGS;
					too.SetAS += ozt.HomeSetS;
					too.SetVS += ozt.GuestSetS;
					too.SayiAS += ozt.HomeSayiS;
					too.SayiVS += ozt.GuestSayiS;
				}

				// Guest olarak oynadiklari
				QueryResultRows<Mac> gMac = Db.SQL<Mac>("select m from Mac m where m.Turnuva = ? AND m.Musabaka.GuestTakim = ? AND (m.GuestOyuncu = ? OR m.GuestOyuncu2 = ?)", trnObj, tkmObj, oyuncu, oyuncu);
				foreach(var m in gMac) {
					Ozet ozt = m.Ozet;

					too.MacO++;
					too.MacOD += ozt.GuestMacOD;
					too.MacOS += ozt.GuestMacOS;

					too.Puan += ozt.GuestPuan;
					too.MacG += ozt.GuestMac;

					too.SetA += ozt.GuestSet;
					too.SetV += ozt.HomeSet;
					too.SayiA += ozt.GuestSayi;
					too.SayiV += ozt.HomeSayi;

					too.PuanD += ozt.GuestPuanD;
					too.MacGD += ozt.GuestMacGD;

					too.SetAD += ozt.GuestSetD;
					too.SetVD += ozt.HomeSetD;
					too.SayiAD += ozt.GuestSayiD;
					too.SayiVD += ozt.HomeSayiD;

					too.PuanS += ozt.GuestPuanS;
					too.MacGS += ozt.GuestMacGS;

					too.SetAS += ozt.GuestSetS;
					too.SetVS += ozt.HomeSetS;
					too.SayiAS += ozt.GuestSayiS;
					too.SayiVS += ozt.HomeSayiS;
				}
				
				too.MacM = too.MacO - too.MacG;
				too.MacMD = too.MacOD - too.MacGD;
				too.MacMS = too.MacOS - too.MacGS;

				yield return too;
			}
		}

		public static IEnumerable<TurnuvaOyuncuOzet> TurnuvaOyuncularOzet(string turnuvaID)
		{
			var trnObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(turnuvaID));

			// Bir oyuncu her devre baska takimda oynayabilir
			// Db.SQL distinct support etmiyor
			// var deneme = Db.SQL("select distinct m.Oyuncu.ObjectNo from TakimOyuncu m where m.Turnuva = ?", trnObj);

			//QueryResultRows<TakimOyuncu> to = Db.SQL<TakimOyuncu>("select m from TakimOyuncu m where m.Turnuva = ?", trnObj);
			var oyuncular = Db.SQL<TakimOyuncu>("select m from TakimOyuncu m where m.Turnuva = ?", trnObj).Select(m => m.Oyuncu).Distinct();	// return IEnumerable<Oyuncu>

			foreach(var oyuncu in oyuncular) {
				TurnuvaOyuncuOzet too = new TurnuvaOyuncuOzet();

				//Console.WriteLine(string.Format("    {0}/{1}", t.OyuncuAd, t.TakimAd));
				/*
				if(t.Oyuncu != null) {
					too.OyuncuID = t.Oyuncu.GetObjectID();
					too.OyuncuAd = t.Oyuncu.Ad;
				}
				too.TakimAd = t.Takim.Ad;
				*/
				if(oyuncu != null) {
					too.OyuncuID = oyuncu.GetObjectID();
					too.OyuncuAd = oyuncu.Ad;
				}
				too.TakimAd = "";

				// Home olarak oynadiklari
				//QueryResultRows<Mac> hMac = Db.SQL<Mac>("select m from Mac m where m.Turnuva = ? AND m.HomeOyuncu = ?", trnObj, t.Oyuncu);
				QueryResultRows<Mac> hMac = Db.SQL<Mac>("select m from Mac m where m.Turnuva = ? AND (m.HomeOyuncu = ? OR m.HomeOyuncu2 = ?)", trnObj, oyuncu, oyuncu);

				foreach(var m in hMac) {
					Ozet ozt = m.Ozet;

					too.MacO++;
					too.MacOD += ozt.HomeMacOD;
					too.MacOS += ozt.HomeMacOS;

					too.Puan += ozt.HomePuan;
					too.MacG += ozt.HomeMac;
					too.SetA += ozt.HomeSet;
					too.SetV += ozt.GuestSet;
					too.SayiA += ozt.HomeSayi;
					too.SayiV += ozt.GuestSayi;

					too.PuanD += ozt.HomePuanD;
					too.MacGD += ozt.HomeMacGD;
					too.SetAD += ozt.HomeSetD;
					too.SetVD += ozt.GuestSetD;
					too.SayiAD += ozt.HomeSayiD;
					too.SayiVD += ozt.GuestSayiD;

					too.PuanS += ozt.HomePuanS;
					too.MacGS += ozt.HomeMacGS;
					too.SetAS += ozt.HomeSetS;
					too.SetVS += ozt.GuestSetS;
					too.SayiAS += ozt.HomeSayiS;
					too.SayiVS += ozt.GuestSayiS;
				}
				// Guest olarak oynadiklar
				//QueryResultRows<Mac> gMac = Db.SQL<Mac>("select m from Mac m where m.Turnuva = ? AND m.GuestOyuncu = ?", trnObj, t.Oyuncu);
				QueryResultRows<Mac> gMac = Db.SQL<Mac>("select m from Mac m where m.Turnuva = ? AND (m.GuestOyuncu = ? OR m.GuestOyuncu2 = ?)", trnObj, oyuncu, oyuncu);

				foreach(var m in gMac) {
					Ozet ozt = m.Ozet;

					too.MacO++;
					too.MacOD += ozt.GuestMacOD;
					too.MacOS += ozt.GuestMacOS;

					too.Puan += ozt.GuestPuan;
					too.MacG += ozt.GuestMac;

					too.SetA += ozt.GuestSet;
					too.SetV += ozt.HomeSet;
					too.SayiA += ozt.GuestSayi;
					too.SayiV += ozt.HomeSayi;

					too.PuanD += ozt.GuestPuanD;
					too.MacGD += ozt.GuestMacGD;
					
					too.SetAD += ozt.GuestSetD;
					too.SetVD += ozt.HomeSetD;
					too.SayiAD += ozt.GuestSayiD;
					too.SayiVD += ozt.HomeSayiD;

					too.PuanS += ozt.GuestPuanS;
					too.MacGS += ozt.GuestMacGS;
					
					too.SetAS += ozt.GuestSetS;
					too.SetVS += ozt.HomeSetS;
					too.SayiAS += ozt.GuestSayiS;
					too.SayiVS += ozt.HomeSayiS;
				}

				too.MacM = too.MacO - too.MacG;
				too.MacMD = too.MacOD - too.MacGD;
				too.MacMS = too.MacOS - too.MacGS;

				//ltoo.Add(too);
				yield return too;
			}
		}
   }
}