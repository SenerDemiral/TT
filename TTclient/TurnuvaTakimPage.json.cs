using Starcounter;
using System.Linq;

namespace TTclient
{
	partial class TurnuvaTakimPage : Json
	{

		protected override void OnData()
		{
			base.OnData();
			
			var trnObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
			Takimlar.Data = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TTDB.TurnuvaTakim o WHERE o.Turnuva = ?", trnObj).OrderByDescending(x => x.Ozet.Puan);
		}

		[TurnuvaTakimPage_json.Takimlar]
		partial class TakimlarElementJson : Json 
		{
			void Handle(Input.TakimClick inp)
			{
				var parent = (TurnuvaTakimPage)this.Parent.Parent;
				parent.CurRowID = this.TakimID;
				parent.CurRowTakimAd = this.TakimAd;

				var trnObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(parent.TurnuvaID));
				var tkmObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(TakimID));
				parent.CurTakimMusabakalari.Data =
					Db.SQL<TTDB.Musabaka>("SELECT mm FROM Musabaka mm WHERE mm.Turnuva = ? AND (mm.HomeTakim = ? OR mm.GuestTakim = ?)",
						trnObj, tkmObj, tkmObj)
					.OrderBy(x => x.Trh);

				foreach(var msbk in parent.CurTakimMusabakalari)
				{
					var msbkObj = (TTDB.Musabaka)DbHelper.FromID(DbHelper.Base64DecodeObjectID(msbk.ID));
					
					msbk.IsHome = false;
					if(msbkObj.HomeTakim.GetObjectNo() == tkmObj.GetObjectNo())
						msbk.IsHome = true;

					msbk.IsWinner = false;
					if(msbk.IsHome) {
						if(msbkObj.Ozet.HomePuan > msbkObj.Ozet.GuestPuan)
							msbk.IsWinner = true;							
					}
					else {
						if(msbkObj.Ozet.GuestPuan > msbkObj.Ozet.HomePuan)
							msbk.IsWinner = true;
					}
				}
				parent.TakimMusabakaOpened = true;
			}

			void Handle(Input.TakimMapClick inp) {
				var tkmObj = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TakimID));
				
				var parent = (TurnuvaTakimPage)this.Parent.Parent;
				parent.TakimLat = tkmObj.Lat;
				parent.TakimLon = tkmObj.Lon;
				parent.TakimMapOpened = true;
			}
		}
	}
}
