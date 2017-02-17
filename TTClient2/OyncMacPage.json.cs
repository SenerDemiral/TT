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

			//TrnvTkmOyncMac.Data
			//TrnvTkmOyncMac.Data = TTDB.Hlpr.TrnvTkmOyncMac(TurnuvaID, TakimID, OyuncuID).OrderByDescending(x => x.Skl).ThenBy(y => y.Trh);
			OyncMac.Data = TTDB.Hlpr.OyuncuMaclari(OyuncuID).OrderByDescending(x => x.Trh);
		}
	}
}
