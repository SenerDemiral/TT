using Starcounter;
using Starcounter.Templates;
using System;
using System.Diagnostics;

namespace TTclient
{
	partial class TurnuvaPage : Json
	{
		public void RefreshTurnuva() {
			Turnuvalar = Db.SQL<TTDB.Turnuva>("SELECT tt FROM Turnuva tt ORDER BY tt.Ad");

		}

		protected override void OnData()
		{
			base.OnData();

			//Turnuvalar = Db.SQL<TTDB.Turnuva>("SELECT tt FROM Turnuva tt ORDER BY tt.Ad");
			
			var trns = Db.SQL<TTDB.Turnuva>("SELECT tt FROM Turnuva tt ORDER BY tt.Ad");
			
			Turnuvalar.Clear();
			TurnuvaPageElementJson te;
			foreach(var trn in trns) {
				te = Turnuvalar.Add();
				te.ID = trn.GetObjectID();
				te.Ad = trn.Ad;
				te.Tarih = trn.Tarih; // string.Format("{0:dd.MM.yy}", trn.Trh);
			}
			
		}

		[TurnuvaPage_json.Turnuvalar]
		partial class TurnuvaPageElementJson : Json
		{
			protected override void OnData()
			{
				base.OnData();
				//ID = Data.GetObjectID();	//Table'dan zate geliyor
			}
			
			void Handle(Input.Toggle inp)
			{
				Opened = !Opened;
			}

			void Handle(Input.MusabakaToggle inp)
			{
				MusabakaOpened = !MusabakaOpened;

				if(MusabakaOpened) {
					var musabaka = new MusabakaPage();
					musabaka.TurnuvaID = ID;
					musabaka.Data = null;
					RecentMusabakalar = musabaka;
				}
				else
					RecentMusabakalar = null;

			}

			void Handle(Input.TakimToggle inp)
			{
				TakimOpened = !TakimOpened;

				if(TakimOpened) {
					var takim = new TurnuvaTakimPage();
					takim.TurnuvaID = ID;
					var trn = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(ID));

					/*
					var sw = Stopwatch.StartNew();
					int i = 0;
			
					var trnTkms = Db.SQL<TTDB.TurnuvaTakim>("SELECT a FROM TurnuvaTakim a WHERE a.Turnuva = ?", trn);
					foreach(var trnTkm in trnTkms) {
						var TkmMsbs = Db.SQL<TTDB.Musabaka>("SELECT a FROM Musabaka a WHERE a.Turnuva = ? and (a.HomeTakim = ? OR a.GuestTakim = ?)", trn, trnTkm.Takim, trnTkm.Takim);
						foreach(var tkmMsb in TkmMsbs) {
							var MsbMacs = Db.SQL<TTDB.Mac>("SELECT a FROM Mac a WHERE a.Musabaka = ?", tkmMsb);
							foreach(var msbMac in MsbMacs) {
								TTDB.Ozet ozet = new TTDB.Ozet();
								string sayilar = "";
								Int16 hSet = 0;
								Int16 gSet = 0;

								var MacSncs = Db.SQL<TTDB.MacSonuc>("SELECT a FROM MacSonuc a WHERE a.Mac = ?", msbMac);
								foreach(var m in MacSncs) {
									i++;
									ozet.HomeSayi += m.HomeSayi;
									ozet.GuestSayi += m.GuestSayi;

									sayilar += string.Format("{0}-{1}{2}", m.HomeSayi.ToString().PadLeft(2, '0'), m.GuestSayi.ToString().PadLeft(2, '0'), TTDB.Constants.sepSayi);
									if(m.HomeSayi > m.GuestSayi)
										hSet++;
									else
										gSet++;

									ozet.Cnt++;
								}
								if(hSet > gSet) {
									ozet.HomeMac = 1;
									ozet.HomePuan = 2;
									if(msbMac.Skl == "D")
										ozet.HomePuan = 3;
								}
								else if(hSet < gSet) {
									ozet.GuestMac = 1;
									ozet.GuestPuan = 2;
									if(msbMac.Skl == "D")
										ozet.GuestPuan = 3;
								}
								ozet.HomeSet = hSet;
								ozet.GuestSet = gSet;

								ozet.Puanlar = string.Format("{0}-{1}", ozet.HomePuan, ozet.GuestPuan);
								ozet.Setler = string.Format("{0}-{1} ", hSet, gSet);
								ozet.Sayilar = sayilar.TrimEnd(TTDB.Constants.charsToTrim);
							}
						}
					}
					Console.WriteLine(string.Format("TurnuvaChain-1 ms:{0}, tick:{1} Cnt:{2}", sw.ElapsedMilliseconds, sw.ElapsedTicks, i));


					
					sw = Stopwatch.StartNew();
					i = 0;
					trn = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(ID));
					trnTkms = Db.SQL<TTDB.TurnuvaTakim>("SELECT a FROM TurnuvaTakim a WHERE a.Turnuva.ObjectNo = ?", trn.GetObjectNo());
					foreach(var trnTkm in trnTkms) {
						var TkmMsbs = Db.SQL<TTDB.Musabaka>("SELECT a FROM Musabaka a WHERE a.Turnuva.ObjectNo = ? and (a.HomeTakim.ObjectNo = ? OR a.GuestTakim.ObjectNo = ?)", trn.GetObjectNo(), trnTkm.Takim.GetObjectNo(), trnTkm.Takim.GetObjectNo());
						foreach(var tkmMsb in TkmMsbs) {
							var MsbMacs = Db.SQL<TTDB.Mac>("SELECT a FROM Mac a WHERE a.Musabaka.ObjectNo = ?", tkmMsb.GetObjectNo());
							foreach(var msbMac in MsbMacs) {
								var MacSncs = Db.SQL<TTDB.MacSonuc>("SELECT a FROM MacSonuc a WHERE a.Mac.ObjectNo = ?", msbMac.GetObjectNo());
								foreach(var macSnc in MacSncs) {
									i++;
								}
							}
						}
					}
					Console.WriteLine(string.Format("TurnuvaChain-2 ms:{0}, tick:{1} Cnt:{2}", sw.ElapsedMilliseconds, sw.ElapsedTicks, i));
					*/
					takim.TurnuvaInfo = $"{trn.Ad} Takım Sonuçları";
					takim.Data = null;
					RecentTakimlar = takim;
				}
				else {
					RecentTakimlar = null; // new TurnuvaTakimPage();
				}

			}

			void Handle(Input.OyuncuToggle inp)
			{
				OyuncuOpened = !OyuncuOpened;

				if(OyuncuOpened) {
					var oyuncu = new TurnuvaOyuncuPage();
					oyuncu.TurnuvaID = ID;
					var trn = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(ID));

					oyuncu.TurnuvaInfo = $"{trn.Ad} Oyuncu Sonuçları";
					oyuncu.Data = null;
					RecentOyuncular = oyuncu;
				}
				else
					RecentOyuncular = null;

			}
		}
	}
}
