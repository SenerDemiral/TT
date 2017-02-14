using Starcounter;

namespace TTClient2
{
	partial class TrnvTkmMsbkPage : Json
	{
		[TrnvTkmMsbkPage_json]
		protected override void OnData()
		{
			base.OnData();
			
			var trnvObj = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
			TurnuvaInfo = trnvObj.Ad;
			var tkmObj = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TakimID));
			TakimInfo = tkmObj.Ad;

			TrnvTkmMsbk = Db.SQL<TTDB.Musabaka>("SELECT tt FROM Musabaka tt WHERE tt.Turnuva = ? AND (tt.HomeTakim = ? OR tt.GuestTakim = ?) ORDER BY tt.Trh", trnvObj, tkmObj, tkmObj);
		}

	}
}
