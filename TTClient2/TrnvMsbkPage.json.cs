using Starcounter;

namespace TTClient2
{
	partial class TrnvMsbkPage : Json
	{
		[TrnvMsbkPage_json]
		protected override void OnData()
		{
			base.OnData();
			
			var trnvObj = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
			TurnuvaInfo = trnvObj.Ad;

			TrnvMsbk = Db.SQL<TTDB.Musabaka>("SELECT tt FROM Musabaka tt WHERE tt.Turnuva = ? ORDER BY tt.Trh", trnvObj);
		}
}
}
