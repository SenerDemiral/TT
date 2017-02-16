using Starcounter;

namespace TTClient2
{
	partial class MsbkMacPage : Json
	{
		[MsbkMacPage_json]
		protected override void OnData()
		{
			base.OnData();
			
			var msbkObj = (TTDB.Musabaka)DbHelper.FromID(DbHelper.Base64DecodeObjectID(MusabakaID));
			HomeTakimAd = msbkObj.HomeTakimAd;
			GuestTakimAd = msbkObj.GuestTakimAd;
			TurnuvaAd = msbkObj.Turnuva.Ad;
			var ozt = msbkObj.Ozet;
			//MusabakaInfo = $"{msbkObj.Tarih} : {msbkObj.HomeTakimAd} {ozt.HomePuan}-{ozt.GuestPuan} {msbkObj.GuestTakimAd}";
			MusabakaInfo = $"{msbkObj.Tarih} : {msbkObj.HomeTakimAd} - {msbkObj.GuestTakimAd}";
			HomeTakimPuan = ozt.HomePuan;
			GuestTakimPuan = ozt.GuestPuan;
			if(HomeTakimPuan == GuestTakimPuan)
			{
				HTGBM = "B";
				GTGBM = "B";
			}
			else if(HomeTakimPuan > GuestTakimPuan)
			{
				HTGBM = "G";
				GTGBM = "M";
			}
			else {
				HTGBM = "M";
				GTGBM = "G";
			}

			MsbkMac = Db.SQL<TTDB.Mac>("SELECT tt FROM Mac tt WHERE tt.Musabaka = ? ORDER BY tt.Skl DESC, tt.Sira DESC", msbkObj);
		}

		[MsbkMacPage_json.MsbkMac]
		partial class MsbkMacPageElementJson : Json
		{
			protected override void OnData()
			{
				base.OnData();

				var macObj = (TTDB.Mac)DbHelper.FromID(DbHelper.Base64DecodeObjectID(ID));
				var ozt = macObj.Ozet;

				HomeSet = ozt.HomeSet;
				GuestSet = ozt.GuestSet;

				if(HomeSet > GuestSet) {
					HGM = "G";
					GGM = "M";
				}
				else {
					HGM = "M";
					GGM = "G";
				}

			}
		}
	}
}
