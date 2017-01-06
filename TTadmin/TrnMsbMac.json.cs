using Starcounter;
using Starcounter.Templates;

namespace TTadmin
{
	partial class TrnMsbMac : Json
	{
		bool reading = false;

		public void RefreshTurnuvaMusabakaMac()
		{
			reading = true;
			
			TrnMsbMacs.Clear();

			var trnObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
			var msbObj = (TTDB.Musabaka)DbHelper.FromID(DbHelper.Base64DecodeObjectID(MusabakaID));
			var ttRows = Db.SQL<TTDB.Mac>("SELECT tt FROM Mac tt WHERE tt.Turnuva = ? AND tt.Musabaka = ? ORDER BY tt.Skl DESC, tt.Sira DESC", trnObj, msbObj);

			//var ttRows = Db.SQL<TTDB.Mac>("SELECT tt FROM Mac tt WHERE tt.Turnuva.ObjectId = ? AND tt.Musabaka.ObjectId = ? ORDER BY tt.Skl DESC, tt.Sira DESC", TurnuvaID, MusabakaID);
			//TrnMsbMacs = Db.SQL<TTDB.Mac>("SELECT tt FROM Mac tt WHERE tt.Musabaka.ObjectId = ? ORDER BY tt.Skl DESC, tt.Sira DESC", MusabakaID);
			
			TrnMsbMacsElementJson te;
			foreach(var row in ttRows) {
				te = this.TrnMsbMacs.Add();
				te.ID = row.GetObjectID();
				te.Skl = row.Skl;
				te.Sira = row.Sira;
				if(row.HomeOyuncu != null)
					te.HomeOyuncuAd = string.Format("{0} ·{1}", row.HomeOyuncu.Ad, row.HomeOyuncu.GetObjectID());
				if (row.HomeOyuncu2 != null)
					te.HomeOyuncuAd2 = string.Format("{0} ·{1}", row.HomeOyuncu2.Ad, row.HomeOyuncu2.GetObjectID());
				if(row.GuestOyuncu != null)
					te.GuestOyuncuAd = string.Format("{0} ·{1}", row.GuestOyuncu.Ad, row.GuestOyuncu.GetObjectID());
				if(row.GuestOyuncu2 != null)
					te.GuestOyuncuAd2 = string.Format("{0} ·{1}", row.GuestOyuncu2.Ad, row.GuestOyuncu2.GetObjectID());
				te.MF = false;
				var ozet = row.Ozet;
				te.Ozet.Puanlar = ozet.Puanlar;
				te.Ozet.Setler = ozet.Setler;
				te.Ozet.Sayilar = ozet.Sayilar;
			}
			

			var musabaka = Db.SQL<TTDB.Musabaka>("SELECT m FROM Musabaka m where m.ObjectId = ?", MusabakaID).First;
			LookupHomeOyuncu.Clear();
			//var hRows = Db.SQL<TTDB.TakimOyuncu>("SELECT t FROM TakimOyuncu t WHERE t.Turnuva.ObjectId = ? AND t.Takim.ObjectId = ?", TurnuvaID, musabaka.HomeTakim.GetObjectID());
			var hRows = Db.SQL<TTDB.TakimOyuncu>("SELECT t FROM TakimOyuncu t WHERE t.Turnuva.ObjectId = ? AND t.Takim = ?", TurnuvaID, msbObj.HomeTakim);
			foreach(var r in hRows) {
				string s = "'" + r.Oyuncu.Ad + " ·" + r.Oyuncu.GetObjectID() + "'";
				LookupHomeOyuncu.Add(new Json(s));
			}
			LookupGuestOyuncu.Clear();
			var gRows = Db.SQL<TTDB.TakimOyuncu>("SELECT t FROM TakimOyuncu t WHERE t.Turnuva.ObjectId = ? AND t.Takim.ObjectId = ?", TurnuvaID, musabaka.GuestTakim.GetObjectID());
			foreach(var r in gRows) {
				string s = "'" + r.Oyuncu.Ad + " ·" + r.Oyuncu.GetObjectID() + "'";
				LookupGuestOyuncu.Add(new Json(s));
			}

			reading = false;
		}

		protected override void OnData()
		{
			base.OnData();

			var musabaka = (TTDB.Musabaka)DbHelper.FromID(DbHelper.Base64DecodeObjectID(MusabakaID));
			var trn = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));

			htid = "TrnMsbMac" + MusabakaID;
			Heading = $"{musabaka.HomeTakim.Ad} - {musabaka.GuestTakim.Ad} Maçlarý";
			HomeTakimAd = TTDB.Hlpr.GetFirstName(musabaka.HomeTakim.Ad);
			GuestTakimAd = TTDB.Hlpr.GetFirstName(musabaka.GuestTakim.Ad);

			RefreshTurnuvaMusabakaMac();
		}

		void Handle(Input.Insert action)
		{
			reading = true;
			var p = this.TrnMsbMacs.Add();
			//p.TakimAd = "deneme  ·W4";
			reading = false;
		}

		void Handle(Input.Create action)
		{
			reading = true;
			TTDB.Hlpr.CreateTrnMsbMac(MusabakaID);
			RefreshTurnuvaMusabakaMac(); 
			reading = false;
		}

		void Handle(Input.Refresh action)
		{
			RefreshTurnuvaMusabakaMac();
		}

		void Handle(Input.Save action)
		{
			reading = true;
			bool deleteVar = false;
			string oynID = "";
			var oyuncuBos = Db.SQL<TTDB.Oyuncu>("SELECT o FROM Oyuncu o WHERE Ad = ?", "*").First;
			foreach(var pet in TrnMsbMacs) {
				if(pet.MF) {
					if(!string.IsNullOrEmpty(pet.ID)) {
						var trnObj = (TTDB.Mac)DbHelper.FromID(DbHelper.Base64DecodeObjectID(pet.ID));
						if(pet.DF) {
							trnObj.Delete();

							deleteVar = true;
						}
						else {
							trnObj.Skl = pet.Skl;
							trnObj.Sira = (short)pet.Sira;
							if(pet.HomeOyuncuAd.IndexOf('·') >= 0) {
								oynID = pet.HomeOyuncuAd.Substring(pet.HomeOyuncuAd.IndexOf('·') + 1);
								trnObj.HomeOyuncu = (TTDB.Oyuncu)DbHelper.FromID(DbHelper.Base64DecodeObjectID(oynID));
							}

							if(pet.GuestOyuncuAd.IndexOf('·') >= 0) {
								oynID = pet.GuestOyuncuAd.Substring(pet.GuestOyuncuAd.IndexOf('·') + 1);
								trnObj.GuestOyuncu = (TTDB.Oyuncu)DbHelper.FromID(DbHelper.Base64DecodeObjectID(oynID));
							}

							if(pet.HomeOyuncuAd2.IndexOf('·') >= 0) {
								oynID = pet.HomeOyuncuAd2.Substring(pet.HomeOyuncuAd2.IndexOf('·') + 1);
								trnObj.HomeOyuncu2 = (TTDB.Oyuncu)DbHelper.FromID(DbHelper.Base64DecodeObjectID(oynID));
							}
							if(pet.GuestOyuncuAd2.IndexOf('·') >= 0) {
								oynID = pet.GuestOyuncuAd2.Substring(pet.GuestOyuncuAd2.IndexOf('·') + 1);
								trnObj.GuestOyuncu2 = (TTDB.Oyuncu)DbHelper.FromID(DbHelper.Base64DecodeObjectID(oynID));
							}
						}
					}
					else {
						var t = new TTDB.Mac();
						pet.ID = t.GetObjectID();
						t.Skl = pet.Skl;
						t.Sira = (short)pet.Sira;
						t.Turnuva = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
						t.Musabaka = (TTDB.Musabaka)DbHelper.FromID(DbHelper.Base64DecodeObjectID(MusabakaID));

						if(pet.HomeOyuncuAd.IndexOf('·') >= 0) {
							oynID = pet.HomeOyuncuAd.Substring(pet.HomeOyuncuAd.IndexOf('·') + 1);
							t.HomeOyuncu = (TTDB.Oyuncu)DbHelper.FromID(DbHelper.Base64DecodeObjectID(oynID));
						}
						else
							t.HomeOyuncu = oyuncuBos;

						if(pet.GuestOyuncuAd.IndexOf('·') >= 0) {
							oynID = pet.GuestOyuncuAd.Substring(pet.GuestOyuncuAd.IndexOf('·') + 1);
							t.GuestOyuncu = (TTDB.Oyuncu)DbHelper.FromID(DbHelper.Base64DecodeObjectID(oynID));
						}
						else
							t.GuestOyuncu = oyuncuBos;

						if(pet.HomeOyuncuAd2.IndexOf('·') >= 0) {
							oynID = pet.HomeOyuncuAd2.Substring(pet.HomeOyuncuAd2.IndexOf('·') + 1);
							t.HomeOyuncu2 = (TTDB.Oyuncu)DbHelper.FromID(DbHelper.Base64DecodeObjectID(oynID));
						}
						
						if(pet.GuestOyuncuAd2.IndexOf('·') >= 0) {
							oynID = pet.GuestOyuncuAd2.Substring(pet.GuestOyuncuAd2.IndexOf('·') + 1);
							t.GuestOyuncu2 = (TTDB.Oyuncu)DbHelper.FromID(DbHelper.Base64DecodeObjectID(oynID));
						}
					}
				}
			}
			Transaction.Commit();

			if(deleteVar)
				RefreshTurnuvaMusabakaMac();
			else {
				for(int i = 0; i < TrnMsbMacs.Count; i++)
					TrnMsbMacs[i].MF = false;
			}
			reading = false;
		}

		void Handle(Input.GetMacSonuclar action)
		{
			if(string.IsNullOrEmpty(CurRowID))
				return;

			var macSonuclar = new TrnMsbMacSnc();
			macSonuclar.MacID = CurRowID;

			macSonuclar.Data = null;
			RecentMacSonuclar = macSonuclar;
		}

		[TrnMsbMac_json.TrnMsbMacs]
		partial class TrnMsbMacsElementJson : Json
		{
			protected override void OnData()
			{
				base.OnData();

				//ID = Data.GetObjectID();
			}

			protected override void HasChanged(TValue property)
			{
				var parent = (TrnMsbMac)this.Parent.Parent;
				if(!parent.reading && MF == false && property.PropertyName != "MF")
					MF = true;
			}

		}
	}
}
