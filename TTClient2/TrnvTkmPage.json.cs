using Starcounter;

namespace TTClient2
{
	partial class TrnvTkmPage : Json
	{
		
		[TrnvTkmPage_json]
		protected override void OnData()
		{
			base.OnData();
			
			var trnvObj = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
			TurnuvaInfo = trnvObj.Ad;
			TrnvTkm = Db.SQL<TTDB.TurnuvaTakim>("SELECT tt FROM TurnuvaTakim tt WHERE tt.Turnuva = ?", trnvObj);
		}

		[TrnvTkmPage_json.TrnvTkm]
		partial class TrnvTkmPageElementJson : Json 
		{
			protected override void OnData()
			{
				base.OnData();

				var parent = (TrnvTkmPage)this.Parent.Parent;
				var turnuvaID = parent.TurnuvaID;
				TrnvTkmMsbkUrl = $"/ttClient2/TrnvTkmMsbk/{turnuvaID}/{TakimID}";
				TrnvTkmOyncUrl = $"/ttClient2/TrnvTkmOync/{turnuvaID}/{TakimID}";
			}
		}

	}
}
