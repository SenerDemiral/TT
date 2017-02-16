using System.Linq;
using Starcounter;

namespace TTClient2
{
	partial class TrnvTkmOyncPage : Json
	{
		[TrnvTkmOyncPage_json]
		protected override void OnData()
		{
			base.OnData();
			
			var trnvObj = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
			TurnuvaInfo = trnvObj.Ad;
			var tkmObj = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TakimID));
			TakimInfo = tkmObj.Ad;

			TrnvTkmOync = Db.SQL<TTDB.TakimOyuncu>("SELECT tt FROM TakimOyuncu tt WHERE tt.Turnuva = ? AND tt.Takim = ?", trnvObj, tkmObj);//.OrderByDescending(x => x.Ozet.TrnPuan);
		}

	}
}
