using Starcounter;

namespace TTClient2
{
	partial class MsbkMacPage : Json
	{
		[MsbkMacPage_json]
		protected override void OnData()
		{
			base.OnData();
			
			var msbkObj = (TTDB.Musabaka)DbHelper.FromID(DbHelper.Base64DecodeObjectID(MusabakaID));
			MusabakaInfo = $"{msbkObj.Tarih} : {msbkObj.HomeTakimAd} - {msbkObj.GuestTakimAd}";

			//MsbkMac = Db.SQL<TTDB.Musabaka>("SELECT tt FROM Musabaka tt WHERE tt.Turnuva = ? AND (tt.HomeTakim = ? OR tt.GuestTakim = ?) ORDER BY tt.Trh", trnvObj, tkmObj, tkmObj);
		}
	}
}
