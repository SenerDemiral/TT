using Starcounter;

namespace TTClient2
{
	partial class TrnvPage : Json
	{
		[TrnvPage_json]
		protected override void OnData()
		{
			base.OnData();
			Trnv = Db.SQL<TTDB.Turnuva>("SELECT tt FROM Turnuva tt ORDER BY tt.Ad");
		}

		[TrnvPage_json.Trnv]
		partial class TrnvPageElementJson : Json 
		{
			protected override void OnData()
			{
				base.OnData();

				TrnvTkmUrl = $"/ttClient2/TrnvTkm/{this.ID}";
				TrnvMsbkUrl = $"/ttClient2/TrnvMsbk/{this.ID}";
				TrnvOyncUrl = $"/ttClient2/TrnvOync/{this.ID}";
			}
		}
	}
}
