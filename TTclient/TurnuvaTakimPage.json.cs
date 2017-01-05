using Starcounter;
using System.Linq;

namespace TTclient
{
	partial class TurnuvaTakimPage : Json
	{

		protected override void OnData()
		{
			base.OnData();
			
			Takimlar.Data = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TTDB.TurnuvaTakim o WHERE o.Turnuva.ObjectId = ?", TurnuvaID).OrderByDescending(x => x.Ozet.Puan);
		}

		[TurnuvaTakimPage_json.Takimlar]
		partial class TakimlarElementJson : Json 
		{
			void Handle(Input.TakimClick inp)
			{
				var parent = (TurnuvaTakimPage)this.Parent.Parent;
				parent.CurRowID = this.TakimID;
				parent.CurRowTakimAd = this.TakimAd;

				parent.CurTakimMusabakalari.Data =
					Db.SQL<TTDB.Musabaka>("SELECT mm FROM Musabaka mm WHERE mm.Turnuva.ObjectId = ? AND (mm.HomeTakim.ObjectId = ? OR mm.GuestTakim.ObjectId = ?)",
						parent.TurnuvaID, TakimID, TakimID)
					.OrderBy(x => x.Trh);

				parent.TakimMusabakaOpened = true;
			}

			void Handle(Input.TakimMapClick inp) {
				var takim = Db.SQL<TTDB.Takim>("SELECT o FROM Takim o WHERE o.ObjectId = ?", TakimID).First;
				
				var parent = (TurnuvaTakimPage)this.Parent.Parent;
				parent.TakimLat = takim.Lat;
				parent.TakimLon = takim.Lon;
				parent.TakimMapOpened = true;
			}
		}
	}
}
