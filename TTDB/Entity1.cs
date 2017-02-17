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



	public static class Constants
	{
		public const string sepID = "·";        // Ad ·ID
		public const string sepTkm = " ";       // Oyuncu(lar) • Takim
		public const string sepDblOyn = "+ ";    // Oyuncu + Oyuncu2
		public const string sepSayi = " ";      // 11-05 • 11-08
												//public const string sepSayi = "■▪↔≡";      // 11-05 ▪ 11-08
		public static readonly char[] charsToTrim = { ',', '.', '·', '•', '●', '▪', '│', ' ' };
	}

	public static class RatingChart
	{	/*
		public static void CreateOyuncuMac()
		{
			Db.Transact(() =>
			{
				QueryResultRows<Mac> Mac = Db.SQL<Mac>("SELECT m FROM Mac m WHERE m.Skl = ? AND m.HomeOyuncu IS NOT NULL AND m.GuestOyuncu IS NOT NULL", "S");
				foreach(var m in Mac)
				{
					var ozt = m.Ozet;

					var om = new OyuncuMac();
					om.Oyuncu = m.HomeOyuncu;
					om.Rakip = m.GuestOyuncu;
					om.aSet = ozt.HomeSetS;
					om.vSet = ozt.GuestSetS;
					om.aSayi = ozt.HomeSayiS;
					om.vSayi = ozt.GuestSayiS;
					om.Trh = m.Musabaka.Trh;
					om.Turnuva = m.Turnuva;

					om = new OyuncuMac();
					om.Oyuncu = m.GuestOyuncu;
					om.Rakip = m.HomeOyuncu;
					om.aSet = ozt.GuestSetS;
					om.vSet = ozt.HomeSetS;
					om.aSayi = ozt.GuestSayiS;
					om.vSayi = ozt.HomeSayiS;
					om.Trh = m.Musabaka.Trh;
					om.Turnuva = m.Turnuva;
				}
			});
		}
		*/
		public static void InitRank()
		{
			Db.Transact(() =>
			{
				QueryResultRows<Oyuncu> Oyuncus = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o");
				foreach(var o in Oyuncus)
				{
					o.Rank = o.BazRank == 0 ? 1900 : o.BazRank;
					o.NOPX = 0;
					o.NopxTxt = "";
				}
			});
		}

		public static void InitRankBaz()
		{
			Db.Transact(() =>
			{
				QueryResultRows<Oyuncu> Oyuncus = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o");
				foreach(var o in Oyuncus)
				{
					o.oMacS = 0;
					o.aMacS = 0;
					o.avMacS = 0;
					o.avSetS = 0;

					o.BazRank = 1900;
					o.BazNopxTxt = "";
				}
			});
		}
		public static void RankCalcultionBaz(DateTime startDate, DateTime endDate)
		{
			int nopx = 0;
			int avMacS = 0;
			int avSetS = 0;

			InitRankBaz();
			InitRank();

			Db.Transact(() =>
			{
				// Iki oyuncu da tanimli olmali!!!
				QueryResultRows<Mac> Macs = Db.SQL<Mac>("SELECT m FROM Mac m WHERE m.Skl = ? AND m.Musabaka.Trh >= ? AND m.Musabaka.Trh <= ? AND m.HomeOyuncu IS NOT NULL AND m.GuestOyuncu IS NOT NULL", "S", startDate, endDate);

				foreach(var m in Macs)
				{
					var ozt = m.Ozet;   // Her seferinde Ozet hesaplamamasi icin

					// Winner +Sira, Looser -Sira
					nopx = (11 - m.Sira);
					m.HomeOyuncu.oMacS += 1;
					m.GuestOyuncu.oMacS += 1;
					avMacS = 1;
					avSetS = Math.Abs(ozt.HomeSetS - ozt.GuestSetS);
					m.HomeOyuncu.BazNopxTxt += string.Format("{0} ", m.Sira);
					m.GuestOyuncu.BazNopxTxt += string.Format("{0} ", m.Sira);
					if(ozt.HomePuan > ozt.GuestPuan)
					{

						m.HomeOyuncu.aMacS += 1;
						m.HomeOyuncu.avMacS += avMacS;
						m.HomeOyuncu.avSetS += avSetS;
						m.GuestOyuncu.avMacS -= avMacS;
						m.GuestOyuncu.avSetS -= avSetS;

						m.HomeOyuncu.BazRank += nopx;
						m.GuestOyuncu.BazRank += 1; //-= avMacS + nopx;
													//m.HomeOyuncu.Rank3 += avMacS + nopx;
													//m.GuestOyuncu.Rank3 -= m.Sira;
													//m.GuestOyuncu.Rank3 -= nopx;
					}
					else
					{
						m.GuestOyuncu.aMacS += 1;
						m.HomeOyuncu.avMacS -= avMacS;
						m.HomeOyuncu.avSetS -= avSetS;
						m.GuestOyuncu.avMacS += avMacS;
						m.GuestOyuncu.avSetS += avSetS;

						m.HomeOyuncu.BazRank += 1; //-= avMacS + nopx;
						m.GuestOyuncu.BazRank += nopx;
						//m.HomeOyuncu.Rank3 -= m.Sira;
						//m.HomeOyuncu.Rank3 -= nopx;
						//m.GuestOyuncu.Rank3 += avMacS + nopx;
					}
				}

				QueryResultRows<Oyuncu> Oyuncu = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o");
				foreach(var o in Oyuncu)
				{
					o.Rank = o.BazRank;
				}
			});

		}

		public static void Write2File(ulong turnuvaNO)
		{
			var trnObj = DbHelper.FromID(turnuvaNO);
			QueryResultRows<Oyuncu> Oyuncu = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o WHERE o.oMacS > ? ORDER BY o.Rank DESC", 0);

			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			using(System.IO.StreamWriter writer = new System.IO.StreamWriter("C:\\Starcounter\\log.txt", true))
			{
				writer.WriteLine("Oyuncu,BazRank,Rank,RankPuan,OynadigiSira");
				foreach(var o in Oyuncu)
				{
					sb.Clear();
					var Macs = Db.SQL<Mac>("SELECT m FROM Mac m WHERE m.Turnuva = ? AND m.Skl = ? AND (m.HomeOyuncu = ? OR m.GuestOyuncu = ?)", trnObj, "S", o, o)
						.OrderByDescending(x => x.Musabaka.Trh);
					foreach(var m in Macs)
					{
						sb.AppendFormat("{0} ", m.Sira);
					}

					writer.WriteLine($"{o.Ad},{o.BazRank},{o.Rank},{o.NopxTxt},{sb.ToString()}");
				}
			}
		}

		// Rank calculation Musabaka Tarihine gore. Her musabaka da Rank degisir.
		public static void RankCalcultionMusabaka(ulong turnuvaNO)
		{
			int ER = 0;
			int UR = 0;
			int nopx = 0;
			int hRank = 0;
			int gRank = 0;

			var trnObj = DbHelper.FromID(turnuvaNO);

			Db.Transact(() =>
			{
				// Iki oyuncu da tanimli olmali!!!
				QueryResultRows<Musabaka> Msbks = Db.SQL<Musabaka>("SELECT m FROM Musabaka m WHERE m.Turnuva = ? ORDER BY m.Trh", trnObj);

				foreach(var msbk in Msbks)
				{
					QueryResultRows<Mac> Macs = Db.SQL<Mac>("SELECT m FROM Mac m WHERE m.Musabaka = ? AND m.Skl = ? AND m.HomeOyuncu IS NOT NULL AND m.GuestOyuncu IS NOT NULL", msbk, "S");

					foreach(var m in Macs)
					{
						var ozt = m.Ozet;

						hRank = m.HomeOyuncu.Rank;
						gRank = m.GuestOyuncu.Rank;

						var result = NOPX(hRank, gRank);
						ER = result.Item1;
						UR = result.Item2;

						if(hRank >= gRank)
						{
							if(ozt.HomePuan > ozt.GuestPuan)
							{
								nopx = ER;
								m.HomeOyuncu.NOPX += ER;
								m.GuestOyuncu.NOPX -= ER;
							}
							else
							{
								nopx = UR;
								m.HomeOyuncu.NOPX -= UR;
								m.GuestOyuncu.NOPX += UR;
							}
						}
						else
						{
							if(ozt.GuestPuan > ozt.HomePuan)
							{
								nopx = ER;
								m.HomeOyuncu.NOPX -= ER;
								m.GuestOyuncu.NOPX += ER;
							}
							else
							{
								nopx = UR;
								m.HomeOyuncu.NOPX += UR;
								m.GuestOyuncu.NOPX -= UR;
							}
						}

						//Console.WriteLine(string.Format("{0}/{6} {1} <> {2}  {3}-{4} {5} {7}", m.Musabaka.Trh, m.HomeOyuncuAd, m.GuestOyuncuAd, ozt.HomePuan, ozt.GuestPuan, nopx, m.HomeOyuncu.Rank, m.GuestOyuncu.Rank));
						//Console.WriteLine(string.Format("{0}/{6} {1} <> {2}  {3}-{4} {5}", m.Musabaka.Trh, m.HomeOyuncuAd, m.GuestOyuncuAd, ozt.HomePuan, ozt.GuestPuan, nopx, System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(m.Musabaka.Trh, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday)));
					}

					// Add NOPX to Rank
					QueryResultRows<Oyuncu> Oyuncus = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o WHERE o.NOPX <> ?", 0);
					foreach(var o in Oyuncus)
					{
						o.Rank += o.NOPX;
						if(o.NOPX == 0)
							o.NopxTxt += " • ";
						else if(o.NOPX > 0)
							o.NopxTxt += "+" + o.NOPX;
						else
							o.NopxTxt += o.NOPX;
						o.NopxTxt += "│";

						o.NOPX = 0;
					}
				}
			});

		}

		// Rank calculation 
		public static void RankCalcultion(ulong turnuvaNO, DateTime startDate, int idx)
		{
			DateTime endDate = startDate.AddDays(5);
			int ER = 0;
			int UR = 0;
			int nopx = 0;
			int hRank = 0;
			int gRank = 0;

			var trnObj = DbHelper.FromID(turnuvaNO);

			Db.Transact(() =>
			{
				// Iki oyuncu da tanimli olmali!!!
				// Eksik/Hatali girisler bitirildikten sonra yapilmali, geri donusu YOK!!!
				// Son yapilan hesaplama tekrarlanabilmeli, yada ilkinden itibaren tekrar hesaplanmali!!
				QueryResultRows<Mac> Macs = Db.SQL<Mac>("SELECT m FROM Mac m WHERE m.Turnuva = ? AND m.Skl = ? AND m.Musabaka.Trh >= ? AND m.Musabaka.Trh <= ? AND m.HomeOyuncu IS NOT NULL AND m.GuestOyuncu IS NOT NULL", trnObj, "S", startDate, endDate);

				foreach(var m in Macs)
				{
					var ozt = m.Ozet;

					hRank = m.HomeOyuncu.Rank;
					gRank = m.GuestOyuncu.Rank;

					var result = NOPX(hRank, gRank);
					ER = result.Item1;
					UR = result.Item2;

					if(hRank >= gRank)
					{
						if(ozt.HomePuan > ozt.GuestPuan)
						{
							nopx = ER;
							m.HomeOyuncu.NOPX += ER;
							m.GuestOyuncu.NOPX -= ER;
						}
						else
						{
							nopx = UR;
							m.HomeOyuncu.NOPX -= UR;
							m.GuestOyuncu.NOPX += UR;
						}
					}
					else
					{
						if(ozt.GuestPuan > ozt.HomePuan)
						{
							nopx = ER;
							m.HomeOyuncu.NOPX -= ER;
							m.GuestOyuncu.NOPX += ER;
						}
						else
						{
							nopx = UR;
							m.HomeOyuncu.NOPX += UR;
							m.GuestOyuncu.NOPX -= UR;
						}
					}

					Console.WriteLine(string.Format("{0}/{6} {1} <> {2}  {3}-{4} {5} {7}-{8}", m.Musabaka.Trh, m.HomeOyuncuAd, m.GuestOyuncuAd, ozt.HomePuan, ozt.GuestPuan, nopx, idx, m.HomeOyuncu.Rank, m.GuestOyuncu.Rank));
					//Console.WriteLine(string.Format("{0}/{6} {1} <> {2}  {3}-{4} {5}", m.Musabaka.Trh, m.HomeOyuncuAd, m.GuestOyuncuAd, ozt.HomePuan, ozt.GuestPuan, nopx, System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(m.Musabaka.Trh, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday)));
				}

				// Add NOPX to Rank
				QueryResultRows<Oyuncu> Oyuncus = Db.SQL<Oyuncu>("SELECT o FROM Oyuncu o");
				foreach(var o in Oyuncus)
				{
					o.Rank += o.NOPX;
					if(o.NOPX == 0)
						o.NopxTxt += " • ";
					else if(o.NOPX > 0)
						o.NopxTxt += "+" + o.NOPX;
					else
						o.NopxTxt += o.NOPX;
					o.NopxTxt += "│";

					o.NOPX = 0;
				}
			});

		}


		//NumberOfPointsExchange between players
		public static Tuple<int, int> NOPX(int HomePlayerRate, int GusetPlayerRate)
		{
			int PS = Math.Abs(HomePlayerRate - GusetPlayerRate);    // Point Spread between players
			int ER = 0; // ExpectedResult
			int UR = 0; // UpsetResult

			if(PS < 13)
			{
				ER = 8;
				UR = 8;
			}
			else if(PS < 38)
			{
				ER = 7;
				UR = 10;
			}
			else if(PS < 63)
			{
				ER = 6;
				UR = 13;
			}
			else if(PS < 88)
			{
				ER = 5;
				UR = 16;
			}
			else if(PS < 113)
			{
				ER = 4;
				UR = 20;
			}
			else if(PS < 138)
			{
				ER = 3;
				UR = 25;
			}
			else if(PS < 163)
			{
				ER = 2;
				UR = 30;
			}
			else if(PS < 188)
			{
				ER = 2;
				UR = 35;
			}
			else if(PS < 213)
			{
				ER = 1;
				UR = 40;
			}
			else if(PS < 238)
			{
				ER = 1;
				UR = 45;
			}
			else
			{
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

		public int BazRank;
		public int Rank;
		public int NOPX;    // NumberOfPointsExcganged : Hesaplamalarda kullaniliyor, sonra sifirlaniyor.
		public string NopxTxt;  // Aldigi/Verdigi puanlar

		public string BazNopxTxt;

		// Single oynadiklari
		public int oMacS;
		public int aMacS;
		public int avMacS;
		public int avSetS;  // aSet - vSet

		//public int InitialRating;			// Ilk Rating,  manuel entry
		//public int CurrentRating;			// Guncel Rating, computed per request
		//public DateTime CurrentRatingDate;	// Rating son guncelleme tarihi. 1.Yari bitiminde, 2.yari bitiminde vs.

		public OyuncuOzet Ozet {
			get {
				OyuncuOzet ozet = new OyuncuOzet();

				QueryResultRows<TakimOyuncu> TO = Db.SQL<TakimOyuncu>("select m from TakimOyuncu m where m.Oyuncu = ?", this);
				foreach(var to in TO)
				{
					QueryResultRows<Mac> hMac = Db.SQL<Mac>("select m from Mac m where m.HomeTakim.Oyuncu = ?", to);
					foreach(var m in hMac)
					{
						//Console.WriteLine(string.Format("    Mac<{2}-{3}> Set<{4}-{5}> Sayi<{6}-{7}> {0}/{1}", m.GuestOyuncuAd, m.GuestTakimAd, m.Ozet.HomeMac, m.Ozet.GuestMac, m.Ozet.HomeSet, m.Ozet.GuestSet, m.Ozet.HomeSayi, m.Ozet.GuestSayi));
						ozet.oMac++;
						ozet.aMac += m.Ozet.HomeMac;
						ozet.aSet += m.Ozet.HomeSet;
						ozet.vSet += m.Ozet.GuestSet;
						ozet.aSayi += m.Ozet.HomeSayi;
						ozet.vSayi += m.Ozet.GuestSayi;
					}
					QueryResultRows<Mac> gMac = Db.SQL<Mac>("select m from Mac m where m.GuestTakim.Oyuncu = ?", to);
					foreach(var m in gMac)
					{
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
			BazRank = 1900;
			Rank = 1900;
			NOPX = 0;
			NopxTxt = "";
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
			PuanV = 0;
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
				foreach(var ta in hta)
				{
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
				foreach(var ta in gta)
				{
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
		public string OyuncuID => this.Oyuncu.GetObjectID();
		public int OyuncuRank => this.Oyuncu.Rank;
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
		public string HomeTakimID => this.HomeTakim.GetObjectID();
		public string GuestTakimID => this.GuestTakim.GetObjectID();
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
				foreach(var m in ms)
				{
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

	public class Ozet
	{
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

		public string HomeOyuncuIsim {
			get {
				return $"{(HomeOyuncu == null ? "" : HomeOyuncu.Ad)}{(HomeOyuncu2 == null ? "" : Constants.sepDblOyn + HomeOyuncu2.Ad)}";
			}
		}
		public string GuestOyuncuIsim {
			get {
				return $"{(GuestOyuncu == null ? "" : GuestOyuncu.Ad)}{(GuestOyuncu2 == null ? "" : Constants.sepDblOyn + GuestOyuncu2.Ad)}";
			}
		}

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
				foreach(var m in ms)
				{
					ozet.HomeSayi += m.HomeSayi;
					ozet.GuestSayi += m.GuestSayi;

					sayilar += string.Format("{0}-{1}{2}", m.HomeSayi.ToString().PadLeft(2, '0'), m.GuestSayi.ToString().PadLeft(2, '0'), Constants.sepSayi);
					if(m.HomeSayi > m.GuestSayi)
					{
						ozet.HomeSet++;
						if(Skl == "D")
							ozet.HomeSetD++;
						else
							ozet.HomeSetS++;
					}
					else
					{
						ozet.GuestSet++;
						if(Skl == "D")
							ozet.GuestSetD++;
						else
							ozet.GuestSetS++;
					}

					ozet.Cnt++;
					if(Skl == "D")
					{
						ozet.HomeSayiD += m.HomeSayi;
						ozet.GuestSayiD += m.GuestSayi;
					}
					else
					{
						ozet.HomeSayiS += m.HomeSayi;
						ozet.GuestSayiS += m.GuestSayi;
					}
				}

				if(Skl == "D")
				{
					ozet.HomeMacOD = 1;
					ozet.GuestMacOD = 1;
				}
				else
				{
					ozet.HomeMacOS = 1;
					ozet.GuestMacOS = 1;
				}

				if(ozet.HomeSet > ozet.GuestSet)
				{
					ozet.HomeMac = 1;
					if(Skl == "D")
					{
						ozet.HomePuan = 3;
						ozet.HomePuanD = 3;
						ozet.HomeMacGD = 1;
					}
					else
					{
						ozet.HomePuan = 2;
						ozet.HomePuanS = 2;
						ozet.HomeMacGS = 1;
					}
				}
				else if(ozet.HomeSet < ozet.GuestSet)
				{
					ozet.GuestMac = 1;
					if(Skl == "D")
					{
						ozet.GuestPuan = 3;
						ozet.GuestPuanD = 3;
						ozet.GuestMacGD = 1;
					}
					else
					{
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

		public TurnuvaOyuncuMacOzet()
		{
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
		public int Idx;
		public int Rank;
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
			Idx = 0;
			Rank = 0;
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
			Hook<Oyuncu>.BeforeDelete += (s, obj) =>
			{
				Console.WriteLine("Hooked: Object {0} is to be deleted", obj.GetObjectNo());
			};
		}
	}


	public static class Hlpr
	{
		public static void sener()
		{
			foreach(var value in TurnuvaOyuncularOzet("VC"))
			{
				Console.Write(value);
				Console.Write(" ");
			}
			Console.WriteLine();
		}

		public static string GetFirstName(string ad)
		{
			return ad.Substring(0, (ad + " ").IndexOf(' '));
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
			Db.Transact(() =>
			{
				foreach(var mac in maclar)
				{
					var macSonuclar = Db.SQL<MacSonuc>("SELECT m FROM MacSonuc m WHERE m.Mac = ?", mac);
					foreach(var macSonuc in macSonuclar)
					{
						macSonuc.Delete();
					}
					mac.Delete();
				}
			});
		}

		public static void CreateTrnMsbMac(string MusabakaID)
		{
			var msbObj = (Musabaka)DbHelper.FromID(DbHelper.Base64DecodeObjectID(MusabakaID));
			if(Db.SQL<Mac>("SELECT m FROM Mac m WHERE m.Musabaka = ?", msbObj).First == null)
			{
				Db.Transact(() =>
				{
					for(int i = 0; i < 8; i++)
					{
						var mac = new Mac();
						mac.Turnuva = msbObj.Turnuva;
						mac.Musabaka = msbObj;
						mac.Skl = "S";
						mac.Sira = (short)(i + 1);
						for(int k = 1; k < 4; k++)
						{
							var snc = new MacSonuc();
							snc.Mac = mac;
							snc.SetNo = (short)k;
						}
					}
					for(int i = 0; i < 4; i++)
					{
						var mac = new Mac();
						mac.Turnuva = msbObj.Turnuva;
						mac.Musabaka = msbObj;
						mac.Skl = "D";
						mac.Sira = (short)(i + 1);
						for(int k = 1; k < 4; k++)
						{
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

			foreach(var mac in maclar)
			{
				TurnuvaOyuncuMacOzet tomo = new TurnuvaOyuncuMacOzet();
				tomo.IsHome = false;
				if((mac.HomeOyuncu != null && oynNo == mac.HomeOyuncu.GetObjectNo()) || (mac.HomeOyuncu2 != null && oynNo == mac.HomeOyuncu2.GetObjectNo()))     // HomeOyuncu degilse Guestdir
					tomo.IsHome = true;
				// Beraberlik olmaz
				var ozet = mac.Ozet;
				tomo.Sonuc = "X";
				if(tomo.IsHome)
				{
					if(ozet.HomeSet > ozet.GuestSet)
						tomo.Sonuc = "G";
					else if(ozet.HomeSet < ozet.GuestSet)
						tomo.Sonuc = "M";
					else if(ozet.HomeSet == ozet.GuestSet && ozet.HomeSet != 0)
						tomo.Sonuc = "B";
				}
				else
				{
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

			foreach(var oyuncu in oyuncular)
			{
				TurnuvaOyuncuOzet too = new TurnuvaOyuncuOzet();
				if(oyuncu != null)
				{
					too.OyuncuID = oyuncu.GetObjectID();
					too.OyuncuAd = oyuncu.Ad;
					too.Rank = oyuncu.Rank;
				}
				too.TakimAd = "";

				// Home olarak oynadiklari
				QueryResultRows<Mac> hMac = Db.SQL<Mac>("select m from Mac m where m.Turnuva = ? AND m.Musabaka.HomeTakim = ? AND (m.HomeOyuncu = ? OR m.HomeOyuncu2 = ?)", trnObj, tkmObj, oyuncu, oyuncu);
				foreach(var m in hMac)
				{
					Ozet ozt = m.Ozet;

					too.MacO++;
					too.MacOS += ozt.HomeMacOS;
					too.MacOD += ozt.HomeMacOD;
					
					too.MacG += ozt.HomeMac;
					too.MacGS += ozt.HomeMacGS;
					too.MacGD += ozt.HomeMacGD;

					too.Puan += ozt.HomePuan;
					too.PuanS += ozt.HomePuanS;
					too.PuanD += ozt.HomePuanD;

					too.SetA += ozt.HomeSet;
					too.SetAS += ozt.HomeSetS;
					too.SetAD += ozt.HomeSetD;

					too.SetV += ozt.GuestSet;
					too.SetVS += ozt.GuestSetS;
					too.SetVD += ozt.GuestSetD;

					too.SayiA += ozt.HomeSayi;
					too.SayiAS += ozt.HomeSayiS;
					too.SayiAD += ozt.HomeSayiD;

					too.SayiV += ozt.GuestSayi;
					too.SayiVS += ozt.GuestSayiS;
					too.SayiVD += ozt.GuestSayiD;
				}

				// Guest olarak oynadiklari
				QueryResultRows<Mac> gMac = Db.SQL<Mac>("select m from Mac m where m.Turnuva = ? AND m.Musabaka.GuestTakim = ? AND (m.GuestOyuncu = ? OR m.GuestOyuncu2 = ?)", trnObj, tkmObj, oyuncu, oyuncu);
				foreach(var m in gMac)
				{
					Ozet ozt = m.Ozet;

					too.MacO++;
					too.MacOS += ozt.GuestMacOS;
					too.MacOD += ozt.GuestMacOD;

					too.MacG += ozt.GuestMac;
					too.MacGS += ozt.GuestMacGS;
					too.MacGD += ozt.GuestMacGD;

					too.Puan += ozt.GuestPuan;
					too.PuanS += ozt.GuestPuanS;
					too.PuanD += ozt.GuestPuanD;

					too.SetA += ozt.GuestSet;
					too.SetAS += ozt.GuestSetS;
					too.SetAD += ozt.GuestSetD;

					too.SetV += ozt.HomeSet;
					too.SetVS += ozt.HomeSetS;
					too.SetVD += ozt.HomeSetD;

					too.SayiA += ozt.GuestSayi;
					too.SayiAS += ozt.GuestSayiS;
					too.SayiAD += ozt.GuestSayiD;

					too.SayiV += ozt.HomeSayi;
					too.SayiVS += ozt.HomeSayiS;
					too.SayiVD += ozt.HomeSayiD;
				}

				too.MacM = too.MacO - too.MacG;
				too.MacMS = too.MacOS - too.MacGS;
				too.MacMD = too.MacOD - too.MacGD;

				if(too.MacO != 0)
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
			var oyuncular = Db.SQL<TakimOyuncu>("select m from TakimOyuncu m where m.Turnuva = ?", trnObj).Select(m => m.Oyuncu).Distinct();    // return IEnumerable<Oyuncu>

			foreach(var oyuncu in oyuncular)
			{
				TurnuvaOyuncuOzet too = new TurnuvaOyuncuOzet();

				//Console.WriteLine(string.Format("    {0}/{1}", t.OyuncuAd, t.TakimAd));
				/*
				if(t.Oyuncu != null) {
					too.OyuncuID = t.Oyuncu.GetObjectID();
					too.OyuncuAd = t.Oyuncu.Ad;
				}
				too.TakimAd = t.Takim.Ad;
				*/
				if(oyuncu != null)
				{
					too.OyuncuID = oyuncu.GetObjectID();
					//too.OyuncuAd = oyuncu.Ad;
					too.OyuncuAd = string.Format("{0} {1}", oyuncu.Ad, oyuncu.Rank);
					too.Rank = oyuncu.Rank;
				}
				too.TakimAd = "";

				// Home olarak oynadiklari
				//QueryResultRows<Mac> hMac = Db.SQL<Mac>("select m from Mac m where m.Turnuva = ? AND m.HomeOyuncu = ?", trnObj, t.Oyuncu);
				QueryResultRows<Mac> hMac = Db.SQL<Mac>("select m from Mac m where m.Turnuva = ? AND (m.HomeOyuncu = ? OR m.HomeOyuncu2 = ?)", trnObj, oyuncu, oyuncu);

				foreach(var m in hMac)
				{
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

				foreach(var m in gMac)
				{
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
				if(too.MacO != 0)
					yield return too;
			}
		}


		public static IEnumerable<OyuncuMac> OyuncuMaclari(string oyuncuID)
		{
			var oyncObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(oyuncuID));

			// Bir oyuncu her devre baska takimda oynayabilir

			QueryResultRows<Mac> HomeSngMac = Db.SQL<Mac>("select m from Mac m where m.Skl = ? AND m.HomeOyuncu = ?", "S", oyncObj);
			foreach(var m in HomeSngMac)
			{
				Ozet ozt = m.Ozet;
				OyuncuMac om = new OyuncuMac()
				{
					Trh = m.Musabaka.Trh,
					Tarih = m.Musabaka.Tarih,
					TakimAd = m.Musabaka.HomeTakimAd,
					RakipAd = m.GuestOyuncu == null ? "" : m.GuestOyuncu.Ad,
					RakipTakimAd = m.Musabaka.GuestTakimAd,

					HG = "H",
					Skl = "S",
					Sira = m.Sira,
					MacA = ozt.HomeMacGS,
					MacV = ozt.GuestMacGS,
					SetA = ozt.HomeSetS,
					SetV = ozt.GuestSetS,
					SayiA = ozt.HomeSayiS,
					SayiV = ozt.GuestSayiS,
				};
				om.GM = om.MacA > om.MacV ? "G" : "M";
				yield return om;
			}
			QueryResultRows<Mac> HomeDblMac = Db.SQL<Mac>("select m from Mac m where m.Skl = ? AND (m.HomeOyuncu = ? OR m.HomeOyuncu2 = ?)", "D", oyncObj, oyncObj);
			foreach(var m in HomeDblMac)
			{
				Ozet ozt = m.Ozet;
				OyuncuMac om = new OyuncuMac()
				{
					Trh = m.Musabaka.Trh,
					Tarih = m.Musabaka.Tarih,
					TakimAd = m.Musabaka.HomeTakimAd,
					RakipAd = m.GuestOyuncu == null ? "" : m.GuestOyuncu.Ad,
					RakipTakimAd = m.Musabaka.GuestTakimAd,

					HG = "H",
					Skl = "D",
					Sira = m.Sira,
					MacA = ozt.HomeMacGD,
					MacV = ozt.GuestMacGD,
					SetA = ozt.HomeSetD,
					SetV = ozt.GuestSetD,
					SayiA = ozt.HomeSayiD,
					SayiV = ozt.GuestSayiD
				};
				om.GM = om.MacA > om.MacV ? "G" : "M";
				yield return om;
			}

			QueryResultRows<Mac> GuestSngMac = Db.SQL<Mac>("select m from Mac m where m.Skl = ? AND m.GuestOyuncu = ?", "S", oyncObj);
			foreach(var m in GuestSngMac)
			{
				Ozet ozt = m.Ozet;
				OyuncuMac om = new OyuncuMac()
				{
					Trh = m.Musabaka.Trh,
					Tarih = m.Musabaka.Tarih,
					TakimAd = m.Musabaka.GuestTakimAd,
					RakipAd = m.HomeOyuncu == null ? "" : m.HomeOyuncu.Ad,
					RakipTakimAd = m.Musabaka.HomeTakimAd,

					HG = "G",
					Skl = "S",
					Sira = m.Sira,
					MacA = ozt.GuestMacGS,
					MacV = ozt.HomeMacGS,
					SetA = ozt.GuestSetS,
					SetV = ozt.HomeSetS,
					SayiA = ozt.GuestSayiS,
					SayiV = ozt.HomeSayiS
				};
				om.GM = om.MacA > om.MacV ? "G" : "M";
				yield return om;
			}
			QueryResultRows<Mac> GuestDblMac = Db.SQL<Mac>("select m from Mac m where m.Skl = ? AND (m.GuestOyuncu = ? OR m.GuestOyuncu2 = ?)", "D", oyncObj, oyncObj);
			foreach(var m in GuestDblMac)
			{
				Ozet ozt = m.Ozet;
				OyuncuMac om = new OyuncuMac()
				{
					Trh = m.Musabaka.Trh,
					Tarih = m.Musabaka.Tarih,
					TakimAd = m.Musabaka.GuestTakimAd,
					RakipAd = m.HomeOyuncu == null ? "" : m.HomeOyuncu.Ad,
					RakipTakimAd = m.Musabaka.HomeTakimAd,

					HG = "G",
					Skl = "D",
					Sira = m.Sira,
					MacA = ozt.GuestMacGD,
					MacV = ozt.HomeMacGD,
					SetA = ozt.GuestSetD,
					SetV = ozt.HomeSetD,
					SayiA = ozt.GuestSayiD,
					SayiV = ozt.HomeSayiD
				};
				om.GM = om.MacA > om.MacV ? "G" : "M";
				yield return om;
			}
		}

		public static IEnumerable<OyuncuMac> TrnvTkmOyncMac(string trnvID, string tkmID, string oyncID)
		{

			var trnvObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(trnvID));
			var tkmObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(tkmID));
			var oyncObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(oyncID));
			
			var msbkList = Db.SQL<Musabaka>("SELECT tt FROM Musabaka tt WHERE tt.Turnuva = ? AND (tt.HomeTakim = ? OR tt.GuestTakim = ?)", trnvObj, tkmObj, tkmObj);
			foreach(var m in msbkList)
			{
				var hMac = Db.SQL<Mac>("SELECT tt FROM Mac tt WHERE tt.Musabaka = ? AND (tt.HomeOyuncu = ? OR tt.HomeOyuncu2 = ?)", m, oyncObj, oyncObj);
				foreach(var mac in hMac)
				{
					var ozt = mac.Ozet;
					var oyncMac = new OyuncuMac();
					oyncMac.Trh = m.Trh;
					oyncMac.Tarih = m.Tarih;
					oyncMac.RakipAd = mac.GuestOyuncuIsim;
					oyncMac.RakipTakimAd = mac.Musabaka.GuestTakimAd;
					oyncMac.Skl = mac.Skl;
					oyncMac.Sira = mac.Sira;
					oyncMac.SetA = ozt.HomeSet;
					oyncMac.SetV = ozt.GuestSet;
					oyncMac.SayiA = ozt.HomeSayi;
					oyncMac.SayiV = ozt.GuestSayi;
					oyncMac.GM = oyncMac.SetA > oyncMac.SetV ? "G" : "M";

					if(mac.Skl == "D")
					{
						if(mac.HomeOyuncu.GetObjectNo() == oyncObj.GetObjectNo())
							oyncMac.PartnerAd = mac.HomeOyuncu2 == null ? "" : mac.HomeOyuncu2.Ad;
						else
							oyncMac.PartnerAd = mac.HomeOyuncu == null ? "" : mac.HomeOyuncu.Ad;
					}
					
					yield return oyncMac;
				}
				var gMac = Db.SQL<Mac>("SELECT tt FROM Mac tt WHERE tt.Musabaka = ? AND (tt.GuestOyuncu = ? OR tt.GuestOyuncu2 = ?)", m, oyncObj, oyncObj);
				foreach(var mac in gMac)
				{
					var ozt = mac.Ozet;
					var oyncMac = new OyuncuMac();
					oyncMac.Trh = m.Trh;
					oyncMac.Tarih = m.Tarih;
					oyncMac.RakipAd = mac.HomeOyuncuIsim;
					oyncMac.RakipTakimAd = mac.Musabaka.HomeTakimAd;
					oyncMac.Skl = mac.Skl;
					oyncMac.Sira = mac.Sira;
					oyncMac.SetA = ozt.GuestSet;
					oyncMac.SetV = ozt.HomeSet;
					oyncMac.SayiA = ozt.GuestSayi;
					oyncMac.SayiV = ozt.HomeSayi;
					oyncMac.GM = oyncMac.SetA > oyncMac.SetV ? "G" : "M";
					
					if(mac.Skl == "D")
					{
						if(mac.GuestOyuncu.GetObjectNo() == oyncObj.GetObjectNo())
							oyncMac.PartnerAd = mac.GuestOyuncu2 == null ? "" : mac.GuestOyuncu2.Ad;
						else
							oyncMac.PartnerAd = mac.GuestOyuncu == null ? "" : mac.GuestOyuncu.Ad;
					}
					yield return oyncMac;
				}
			}

		}
	}

	public class OyuncuMac
	{
		public string OyuncuID;
		public string PartnerAd;
		public string TakimAd;
		public string RakipAd;
		public string RakipTakimAd;
		public DateTime Trh;
		public string Tarih;
		public string TrnvTur;
		public string HG;
		public string Skl;
		public int Sira;

		public int MacA;
		public int MacV;
		public int SetA;
		public int SetV;
		public int SayiA;
		public int SayiV;
		public string GM;   // Galip/Maglup

		public OyuncuMac()
		{
			
			TakimAd = "";
			PartnerAd = "";
			RakipAd = "";
			RakipTakimAd = "";
			Tarih = "";
			TrnvTur = "T";	// Takim
			HG = "?";
			Skl = "?";
			Sira = 0;
			MacA = 0;
			MacV = 0;
			SetA = 0;
			SetV = 0;
			SayiA = 0;
			SayiV = 0;
			GM = "?";

		}
	}
}