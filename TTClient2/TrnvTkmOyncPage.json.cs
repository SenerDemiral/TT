using System.Linq;
using Starcounter;

namespace TTClient2
{
	partial class TrnvTkmOyncPage : Json
	{
		int idx = 0;

		[TrnvTkmOyncPage_json]
		protected override void OnData()
		{
			base.OnData();
			
			var trnvObj = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
			TurnuvaInfo = trnvObj.Ad;
			var tkmObj = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TakimID));
			TakimInfo = tkmObj.Ad;

			//TrnvTkmOync = Db.SQL<TTDB.TakimOyuncu>("SELECT tt FROM TakimOyuncu tt WHERE tt.Turnuva = ? AND tt.Takim = ?", trnvObj, tkmObj);//.OrderByDescending(x => x.Ozet.TrnPuan);

			//TrnvTkmOync.Data = TTDB.Hlpr.TurnuvaTakimOyuncularOzet(TurnuvaID, TakimID).OrderByDescending(x => (x.MacGS - x.MacMS) + (x.MacGD - x.MacMD));
			TrnvTkmOync.Data = TTDB.Hlpr.TurnuvaTakimOyuncularOzet(TurnuvaID, TakimID)
				.OrderByDescending(x => (x.MacGS - x.MacMS) + (x.MacGD - x.MacMD));

		}

		[TrnvTkmOyncPage_json.TrnvTkmOync]
		partial class TrnvTkmOyncPageElementJson : Json
		{
			protected override void OnData()
			{
				base.OnData();

				//var parent = (TrnvTkmOyncPage)this.Parent.Parent;
				Idx = ++((TrnvTkmOyncPage)this.Parent.Parent).idx;
			}
		}
	}
}
