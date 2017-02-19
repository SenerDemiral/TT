using Starcounter;
using System.Linq;
using System;
using System.Diagnostics;

namespace TTclient
{
	partial class TurnuvaOyuncuPage : Json
	{
		protected override void OnData()
		{
			base.OnData();

			var sw = Stopwatch.StartNew();
			//var ccc = TTDB.Hlpr.TurnuvaOyuncularOzet(TurnuvaID).OrderByDescending(x => x.Puan).ThenByDescending(y => y.MacG - y.MacM);
			var ccc = TTDB.Hlpr.TurnuvaOyuncularOzet(TurnuvaID).OrderByDescending(x => x.Rank);
			foreach(var o in ccc) {
				OyuncularElementJson item = new OyuncularElementJson();

				item.OyuncuID = o.OyuncuID;
				item.OyuncuAd = o.OyuncuAd;
				item.TakimAd = o.TakimAd;
				item.Puan = o.Puan;
				item.MacO = o.MacO;
				item.MacG = o.MacG;
				item.MacM = o.MacM;
				item.SetA = o.SetA;
				item.SetV = o.SetV;
				item.SayiA = o.SayiA;
				item.SayiV = o.SayiV;

				item.PuanD = o.PuanD;
				item.MacOD = o.MacOD;
				item.MacGD = o.MacGD;
				item.MacMD = o.MacMD;
				item.SetAD = o.SetAD;
				item.SetVD = o.SetVD;
				item.SayiAD = o.SayiAD;
				item.SayiVD = o.SayiVD;

				item.PuanS = o.PuanS;
				item.MacOS = o.MacOS;
				item.MacGS = o.MacGS;
				item.MacMS = o.MacMS;
				item.SetAS = o.SetAS;
				item.SetVS = o.SetVS;
				item.SayiAS = o.SayiAS;
				item.SayiVS = o.SayiVS;

				Oyuncular.Add(item);
			}
			Console.WriteLine(string.Format("OyuncuPage.OnData-LinqSort ms:{0}, tick:{1}", sw.ElapsedMilliseconds, sw.ElapsedTicks));
		}

		void Handle(Input.OyuncuMacOpened inp)
		{
			if(inp.Value == false)
				CurOyuncuMaclari.Data = null;
		}

		[TurnuvaOyuncuPage_json.Oyuncular]
		partial class OyuncularElementJson : Json 
		{
			void Handle(Input.OyuncuAd inp) {

			}

			void Handle(Input.OyuncuClick inp)
			{
				var parent = (TurnuvaOyuncuPage)this.Parent.Parent;
				parent.CurRowID = this.OyuncuID;
				parent.CurRowOyuncuAd = this.OyuncuAd;

				parent.CurOyuncuMaclari.Clear();
				foreach(var mac in TTDB.Hlpr.TurnuvaOyuncuMaclarOzet(parent.TurnuvaID, OyuncuID)) {
					CurOyuncuMaclariElementJson item = new CurOyuncuMaclariElementJson();
					item.IsHome = mac.IsHome;
					item.Sonuc = mac.Sonuc;
					item.MusabakaTarih = mac.MusabakaTarih;
					item.Skl = mac.Skl;
					item.Sira = mac.Sira;
					item.HomeOyuncuInfo = mac.HomeOyuncuInfo;
					item.GuestOyuncuInfo = mac.GuestOyuncuInfo;
					item.Ozet.Setler = mac.Setler;
					item.Ozet.Sayilar = mac.Sayilar;

					parent.CurOyuncuMaclari.Add(item);

				}

				/*
				var trnObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(parent.TurnuvaID));
				var oynObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(OyuncuID));
				var oynNo = (long)oynObj.GetObjectNo();
				parent.CurOyuncuMaclari.Data =
					Db.SQL<TTDB.Mac>("SELECT mm FROM MAC mm WHERE mm.Turnuva = ? AND (mm.HomeOyuncu = ? OR mm.GuestOyuncu = ? OR mm.HomeOyuncu2 = ? OR mm.GuestOyuncu2 = ?)",
						trnObj, oynObj, oynObj, oynObj, oynObj)
					.OrderByDescending(x => x.Musabaka.Trh);

				foreach(var mac in parent.CurOyuncuMaclari)
				{
					mac.IsHome = false;
					if(oynNo == mac.HomeOyuncuNo || oynNo == mac.HomeOyuncu2No)		// HomeOyuncu degilse Guestdir
						mac.IsHome = true;
					
					if(mac.IsHome) {
						if(mac.Home)
					}

				}
				*/
				parent.OyuncuMacOpened = true;
			}
		}

	}

}
