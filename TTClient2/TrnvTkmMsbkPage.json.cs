using Starcounter;

namespace TTClient2
{
	partial class TrnvTkmMsbkPage : Json
	{
		[TrnvTkmMsbkPage_json]
		protected override void OnData()
		{
			base.OnData();
			
			var trnvObj = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
			TurnuvaInfo = trnvObj.Ad;
			var tkmObj = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TakimID));
			TakimInfo = tkmObj.Ad;
			
			TrnvTkmMsbk = Db.SQL<TTDB.Musabaka>("SELECT tt FROM Musabaka tt WHERE tt.Turnuva = ? AND (tt.HomeTakim = ? OR tt.GuestTakim = ?) ORDER BY tt.Trh", trnvObj, tkmObj, tkmObj);
		}

		[TrnvTkmMsbkPage_json.TrnvTkmMsbk]
		partial class TrnvTkmMsbkPageElementJson : Json 
		{
			protected override void OnData()
			{
				base.OnData();
				var TakimID = ((TrnvTkmMsbkPage)Parent.Parent).TakimID;

				var msbkObj = (TTDB.Musabaka)DbHelper.FromID(DbHelper.Base64DecodeObjectID(this.ID));
				var ozt = msbkObj.Ozet;

				if((ozt.HomePuan + ozt.GuestPuan) != 0)
				{
					HomePuan = $"{ozt.HomePuan}";
					GuestPuan = $"{ozt.GuestPuan}";
					//HomePuan = $"{(ozt.HomePuan == 0 ? "" : ozt.HomePuan.ToString())}";
					//GuestPuan = $"{(ozt.GuestPuan == 0 ? "" : ozt.GuestPuan.ToString())}";

					if(ozt.HomePuan == ozt.GuestPuan)
						GBM = "B";
					else
					{
						if(TakimID == msbkObj.HomeTakimID)
						{
							if(ozt.HomePuan > ozt.GuestPuan)
								GBM = "G";
							else
								GBM = "M";
						}
						else
						{
							if(ozt.HomePuan < ozt.GuestPuan)
								GBM = "G";
							else
								GBM = "M";
						}
					}
				}
			}
		}


	}
}
