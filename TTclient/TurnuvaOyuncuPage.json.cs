using Starcounter;
using System.Linq;

namespace TTclient
{
	partial class TurnuvaOyuncuPage : Json
	{
		protected override void OnData()
		{
			base.OnData();

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

				parent.CurOyuncuMaclari.Data = 
					Db.SQL<TTDB.Mac>("SELECT mm FROM MAC mm WHERE mm.Turnuva.ObjectId = ? AND (mm.HomeOyuncu.ObjectId = ? OR mm.GuestOyuncu.ObjectId = ? OR mm.HomeOyuncu2.ObjectId = ? OR mm.GuestOyuncu2.ObjectId = ?)", 
						parent.TurnuvaID, OyuncuID, OyuncuID, OyuncuID, OyuncuID)
					.OrderByDescending(x => x.Musabaka.Trh);

				parent.OyuncuMacOpened = true;
			}
		}

	}

}
