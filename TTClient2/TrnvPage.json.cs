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

				/*
				"TrnvTkmUrl": "",
				"TrnvMsbkUrl": "",
				"TrnvOyncUrl": "",
				TrnvTkmUrl = $"/ttClient2/TrnvTkm/{this.ID}";
				TrnvMsbkUrl = $"/ttClient2/TrnvMsbk/{this.ID}";
				TrnvOyncUrl = $"/ttClient2/TrnvOync/{this.ID}";
				*/
			}

			void Handle(Input.DenemeClick inp) {
				Transaction.Commit();
			}

			void Handle(Input.Ad inp) {
				//<input type="text" value="{{item.Ad$::change}}" />
				//<input type="text" value="{{item.Ad$::input}}" />
				//DenemeClick = 1111;
				//Ad = "2222";
				//inp.Value = "DENEME";
			}
		}
	}
}
