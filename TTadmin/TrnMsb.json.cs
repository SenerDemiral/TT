using System;
using Starcounter;
using Starcounter.Templates;

namespace TTadmin
{
   partial class TrnMsb : Json
	{
		bool reading = false;

		public void RefreshTurnuvaMusabaka()
		{
			reading = true;

			TrnMsbs.Clear();

			var trnObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
			var TM = Db.SQL<TTDB.TurnuvaMusabaka>("SELECT tt FROM TurnuvaMusabaka tt WHERE tt.Turnuva = ?", trnObj);

			TrnMsbsElementJson te;
			foreach(var tt in TM) {
				te = this.TrnMsbs.Add();
				te.ID = tt.GetObjectID();
				te.HomeTakimAd = string.Format("{0} ·{1}", tt.HomeTakimAd, tt.HomeTakim.GetObjectID());
				te.GuestTakimAd = string.Format("{0} ·{1}", tt.GuestTakimAd, tt.GuestTakim.GetObjectID());
				te.Tarih = string.Format("{0:dd.MM.yy}", tt.Trh);

				te.MF = false;

				//var tID = te.TakimAd.Substring(te.TakimAd.IndexOf('·')+1);
			}

			LookupTakim.Clear();
			var takimlar = Db.SQL<TTDB.TurnuvaTakim>("SELECT t FROM TurnuvaTakim t");
			foreach(var takim in takimlar) {
				string s = "'" + takim.Takim.Ad + " ·" + takim.Takim.GetObjectID() + "'";
				LookupTakim.Add(new Json(s));
			}

			reading = false;
		}

		protected override void OnData()
		{
			base.OnData();

			RefreshTurnuvaMusabaka();
		}

		void Handle(Input.Insert action)
		{
			reading = true;
			var p = this.TrnMsbs.Add();
			//p.TakimAd = "deneme  ·W4";
			p.Tarih = string.Format("{0:dd.MM.yy}", DateTime.Today);
			reading = false;
		}

		void Handle(Input.Refresh action)
		{
			RefreshTurnuvaMusabaka();
		}

		void Handle(Input.Save action)
		{
			reading = true;
			bool deleteVar = false;
			foreach(var pet in TrnMsbs) {
				if(pet.MF) {
					if(!string.IsNullOrEmpty(pet.ID)) {
						var trnObj = (TTDB.TurnuvaMusabaka)DbHelper.FromID(DbHelper.Base64DecodeObjectID(pet.ID));
						if(pet.DF) {
							trnObj.Delete();

							deleteVar = true;
						}
						else {
							var homeTkmID = pet.HomeTakimAd.Substring(pet.HomeTakimAd.IndexOf('·') + 1);
							trnObj.HomeTakim = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(homeTkmID));
							var guestTkmID = pet.GuestTakimAd.Substring(pet.GuestTakimAd.IndexOf('·') + 1);
							trnObj.GuestTakim = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(guestTkmID));
							trnObj.Trh = DateTime.ParseExact(pet.Tarih, "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture);
							//trnObj.Ad = pet.TakimAd;
						}
					}
					else {
						var t = new TTDB.TurnuvaMusabaka();
						pet.ID = t.GetObjectID();
						t.Turnuva = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
						var homeTkmID = pet.HomeTakimAd.Substring(pet.HomeTakimAd.IndexOf('·') + 1);
						t.HomeTakim = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(homeTkmID));
						var guestTkmID = pet.GuestTakimAd.Substring(pet.GuestTakimAd.IndexOf('·') + 1);
						t.GuestTakim = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(guestTkmID));
						t.Trh = DateTime.ParseExact(pet.Tarih, "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture);
					}
				}
			}
			Transaction.Commit();

			if(deleteVar)
				RefreshTurnuvaMusabaka();
			else {
				for(int i = 0; i < TrnMsbs.Count; i++)
					TrnMsbs[i].MF = false;
			}
			reading = false;
		}
		/*
		void Handle(Input.GetOyuncular action)
		{
			var oyuncular = new TrnTkmOyn();

			oyuncular.ID = "TrnTkmOyn" + CurRowID;
			oyuncular.TurnuvaTakimID = CurRowID;

			oyuncular.Data = null;

			int a = 0;
			for(int i = 0; i < TrnTkms.Count; i++)
				if(TrnTkms[i].ID == CurRowID) {
					a = i;
					break;
				}

			oyuncular.Heading = TrnTkms[a].TakimAd + " Oyuncularý " + CurRowID;
			//Trns[a].RecentTrnTkms = takimlar;
			RecentOyuncular = oyuncular;
		}
		*/
		[TrnMsb_json.TrnMsbs]
		partial class TrnMsbsElementJson : Json
		{
			protected override void OnData()
			{
				base.OnData();

				//ID = Data.GetObjectID();
			}

			protected override void HasChanged(TValue property)
			{
				var parent = (TrnMsb)this.Parent.Parent;
				if(!parent.reading && MF == false && property.PropertyName != "MF")
					MF = true;
			}

		}
	}

}
