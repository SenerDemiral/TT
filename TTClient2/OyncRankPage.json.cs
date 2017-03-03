using System.Linq;
using Starcounter;

namespace TTClient2
{
	partial class OyncRankPage : Json
	{
		int idx = 0;

		[OyncRankPage_json]
		protected override void OnData()
		{
			base.OnData();

			//TrnvTkmOync.Data = TTDB.Hlpr.TurnuvaTakimOyuncularOzet(TurnuvaID, TakimID).OrderByDescending(x => (x.MacGS - x.MacMS) + (x.MacGD - x.MacMD));
			OyncRank = Db.SQL<TTDB.Oyuncu>("SELECT o FROM Oyuncu o WHERE o.NopxTxt > ? ORDER BY o.Rank DESC, o.BazRank DESC", "");
		}

		void Handle(Input.SortAd inp)
		{
			// Asagidaki TrnvTkmOyncPageElementJson.OnData yi tetikliyor Idx degisiyor YAPMA
			//OyncRank = Db.SQL<TTDB.Oyuncu>("SELECT o FROM Oyuncu o WHERE o.NopxTxt > ? ORDER BY o.Ad", "");
			OyncRank.Data = OyncRank.OrderBy(x => x.Ad).ToArray<TrnvTkmOyncPageElementJson>();

		}

		[OyncRankPage_json.OyncRank]
		partial class TrnvTkmOyncPageElementJson : Json
		{
			protected override void OnData()
			{
				base.OnData();

				//var parent = (TrnvTkmOyncPage)this.Parent.Parent;
				Idx = ++((OyncRankPage)this.Parent.Parent).idx;
			}
		}
}
}
