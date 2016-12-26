using Starcounter;
using System.Linq;

namespace TTclient
{
	partial class TurnuvaTakimPage : Json
	{

		protected override void OnData()
		{
			base.OnData();

			Takimlar.Data = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TTDB.TurnuvaTakim o WHERE o.Turnuva.ObjectId = ?", TurnuvaID).OrderByDescending(x => x.Ozet.Puan);
		}
	}
}
