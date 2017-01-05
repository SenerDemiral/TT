using Starcounter;

namespace TTclient
{
	partial class TakimPage : Json
	{
		protected override void OnData()
		{
			base.OnData();

			Takimlar = Db.SQL<TTDB.Takim>("SELECT tt FROM Takim tt");
		}
	}
}
