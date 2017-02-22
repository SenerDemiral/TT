using System.Linq;
using Starcounter;

namespace TTClient2
{
	partial class TrnvTkmOyncMacPage : Json
	{
		[TrnvTkmOyncMacPage_json]
		protected override void OnData()
		{
			base.OnData();
			
			var trnvObj = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
			TurnuvaInfo = trnvObj.Ad;
			var tkmObj = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TakimID));
			TakimInfo = tkmObj.Ad;
			var oyncObj = (TTDB.Oyuncu)DbHelper.FromID(DbHelper.Base64DecodeObjectID(OyuncuID));
			OyuncuInfo = oyncObj.Ad;

			//TrnvTkmOyncMac.Data = TTDB.Hlpr.TrnvTkmOyncMac(TurnuvaID, TakimID, OyuncuID).OrderByDescending(x => x.Skl).ThenBy(y => y.Trh);
			var sw = System.Diagnostics.Stopwatch.StartNew();
			TrnvTkmOyncMac.Data = TTDB.Hlpr.TrnvTkmOyncMac(TurnuvaID, TakimID, OyuncuID).OrderByDescending(x => x.Skl).ThenBy(y => y.Trh);
			sw.Stop();
			System.Console.WriteLine(string.Format("TrnvTkmOyncMacPage ms:{0}, tick:{1}", sw.ElapsedMilliseconds, sw.ElapsedTicks));
		}
	}
}
