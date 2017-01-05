using Starcounter;

namespace TTclient
{
	partial class OyuncuPage : Json
	{
		protected override void OnData()
		{
			base.OnData();

			Oyuncular = Db.SQL<TTDB.Oyuncu>("SELECT tt FROM Oyuncu tt");
		}
	}
}
