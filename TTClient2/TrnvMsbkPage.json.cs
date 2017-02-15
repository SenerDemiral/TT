using Starcounter;

namespace TTClient2
{
	partial class TrnvMsbkPage : Json
	{
		[TrnvMsbkPage_json]
		protected override void OnData()
		{
			base.OnData();
			
			var trnvObj = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
			TurnuvaInfo = trnvObj.Ad;

			TrnvMsbk = Db.SQL<TTDB.Musabaka>("SELECT tt FROM Musabaka tt WHERE tt.Turnuva = ? ORDER BY tt.Trh", trnvObj);
		}

		[TrnvMsbkPage_json.TrnvMsbk]
		partial class TrnvMsbkPageElementJson : Json 
		{
			protected override void OnData()
			{
				base.OnData();

				var msbkObj = (TTDB.Musabaka)DbHelper.FromID(DbHelper.Base64DecodeObjectID(this.ID));
				var ozt = msbkObj.Ozet;

				HomePuan = $"{(ozt.HomePuan == 0 ? "" : ozt.HomePuan.ToString())}";
				GuestPuan = $"{(ozt.GuestPuan == 0 ? "" : ozt.GuestPuan.ToString())}";

				if((ozt.HomePuan + ozt.GuestPuan) > 0)
				{
					if(ozt.HomePuan == ozt.GuestPuan)
					{
						HTGBM = "B";
						GTGBM = "B";
					}
					else if(ozt.HomePuan > ozt.GuestPuan)
					{
						HTGBM = "G";
						GTGBM = "M";
					}
					else
					{
						HTGBM = "M";
						GTGBM = "G";
					}
				}
			}
		}

	}
}
