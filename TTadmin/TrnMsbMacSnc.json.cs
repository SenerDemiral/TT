using Starcounter;
using Starcounter.Templates;

namespace TTadmin
{
	partial class TrnMsbMacSnc : Json
	{
		bool reading = false;

		public void RefreshMacSonuc()
		{
			reading = true;

			TrnMsbMacSncs.Clear();

			var macObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(MacID));
			//var TT = Db.SQL<TTDB.TurnuvaTakim>("SELECT tt FROM TurnuvaTakim tt WHERE tt.Turnuva = ?", trnObj);
			var TT = Db.SQL<TTDB.MacSonuc>("SELECT tt FROM MacSonuc tt WHERE tt.Mac.ObjectId = ? ORDER BY tt.SetNo", MacID);

			TrnMsbMacSncsElementJson te;
			foreach(var tt in TT) {
				te = this.TrnMsbMacSncs.Add();
				te.ID = tt.GetObjectID();
				te.SetNo = tt.SetNo;
				te.HomeSayi = tt.HomeSayi;
				te.GuestSayi = tt.GuestSayi;
				te.MF = false;

				//var tID = te.TakimAd.Substring(te.TakimAd.IndexOf('�')+1);
			}
			reading = false;
		}

		protected override void OnData()
		{
			base.OnData();

			var mac = (TTDB.Mac)DbHelper.FromID(DbHelper.Base64DecodeObjectID(MacID));
			var home = mac.HomeOyuncuInfo;
			var guest = mac.GuestOyuncuInfo;

			htid = "TrnMsbMacSnc" + CurRowID;
			Heading = $"{mac.Musabaka.HomeTakim.Ad} - {mac.Musabaka.GuestTakim.Ad} {mac.Skl}/{mac.Sira} Sonu�lar�";
			HomeOyuncuAd = $"{(mac.HomeOyuncu == null ? "" : mac.HomeOyuncu.Ad)}{(mac.HomeOyuncu2 == null ? "" : TTDB.Constants.sepDblOyn + mac.HomeOyuncu2.Ad)}";
			GuestOyuncuAd = $"{(mac.GuestOyuncu == null ? "" : mac.GuestOyuncu.Ad)}{(mac.GuestOyuncu2 == null ? "" : TTDB.Constants.sepDblOyn + mac.GuestOyuncu2.Ad)}";

			RefreshMacSonuc();
		}

		void Handle(Input.Insert action)
		{
			reading = true;
			var p = this.TrnMsbMacSncs.Add();
			//p.TakimAd = "deneme  �W4";
			reading = false;
		}

		void Handle(Input.Refresh action)
		{
			RefreshMacSonuc();
		}

		void Handle(Input.Save action)
		{
			reading = true;
			bool deleteVar = false;
			foreach(var pet in TrnMsbMacSncs) {
				if(pet.MF) {
					if(!string.IsNullOrEmpty(pet.ID)) {
						var trnObj = (TTDB.MacSonuc)DbHelper.FromID(DbHelper.Base64DecodeObjectID(pet.ID));
						if(pet.DF) {
							trnObj.Delete();

							deleteVar = true;
						}
						else {
							trnObj.SetNo = (short)pet.SetNo;
							trnObj.HomeSayi = (short)pet.HomeSayi;
							trnObj.GuestSayi = (short)pet.GuestSayi;
						}
					}
					else {
						var t = new TTDB.MacSonuc();
						pet.ID = t.GetObjectID();
						t.Mac = (TTDB.Mac)DbHelper.FromID(DbHelper.Base64DecodeObjectID(MacID));
						t.SetNo = (short)pet.SetNo;
						t.HomeSayi = (short)pet.HomeSayi;
						t.GuestSayi = (short)pet.GuestSayi;
					}
				}
			}
			Transaction.Commit();

			if(deleteVar)
				RefreshMacSonuc();
			else {
				for(int i = 0; i < TrnMsbMacSncs.Count; i++)
					TrnMsbMacSncs[i].MF = false;
			}
			reading = false;
		}

		[TrnMsbMacSnc_json.TrnMsbMacSncs]
		partial class TrnMsbMacSncsElementJson : Json
		{
			protected override void OnData()
			{
				base.OnData();

				//ID = Data.GetObjectID();
			}

			protected override void HasChanged(TValue property)
			{
				var parent = (TrnMsbMacSnc)this.Parent.Parent;
				if(!parent.reading && MF == false && property.PropertyName != "MF")
					MF = true;
			}

		}
	}
}
