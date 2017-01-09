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
			var ccc = TTDB.Hlpr.TurnuvaOyuncularOzet(TurnuvaID).OrderByDescending(x => x.Puan).ThenBy(y => y.MacM).OrderByDescending(y => y.SetA * 2);
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

				var trnObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(parent.TurnuvaID));
				var oynObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(OyuncuID));
				parent.CurOyuncuMaclari.Data =
					Db.SQL<TTDB.Mac>("SELECT mm FROM MAC mm WHERE mm.Turnuva = ? AND (mm.HomeOyuncu = ? OR mm.GuestOyuncu = ? OR mm.HomeOyuncu2 = ? OR mm.GuestOyuncu2 = ?)", 
						trnObj, oynObj, oynObj, oynObj, oynObj)
					.OrderByDescending(x => x.Musabaka.Trh);

				parent.OyuncuMacOpened = true;
			}
		}

	}

}
