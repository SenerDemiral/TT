using System.Linq;
using Starcounter;

namespace TTClient2
{
	partial class OyncMacPage : Json
	{
		[OyncMacPage_json]
		protected override void OnData()
		{
			base.OnData();

			var oyncObj = (TTDB.Oyuncu)DbHelper.FromID(DbHelper.Base64DecodeObjectID(OyuncuID));
			OyuncuInfo = oyncObj.Ad;

			//TrnvTkmOyncMac.Data = TTDB.Hlpr.TrnvTkmOyncMac(TurnuvaID, TakimID, OyuncuID).OrderByDescending(x => x.Skl).ThenBy(y => y.Trh);
			var sw = System.Diagnostics.Stopwatch.StartNew();
			OyncMac.Data = TTDB.Hlpr.OyuncuMaclari(OyuncuID).OrderByDescending(x => x.Skl).ThenBy(y => y.Trh); ; // OrderByDescending(x => x.Trh);
			sw.Stop();
			System.Console.WriteLine(string.Format("OyncMacPage ms:{0}, tick:{1}", sw.ElapsedMilliseconds, sw.ElapsedTicks));
		}
	}
}
