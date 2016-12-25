using System;
using Starcounter;
using Starcounter.Templates;

namespace TTadmin
{
   partial class TrnTkm : Json
    {
		bool reading = false;

		public void RefreshTurnuvaTakim()
		{
			reading = true;
			
			TrnTkms.Clear();

			//var TT = Db.SQL<TTDB.TurnuvaTakim>("SELECT tt FROM TurnuvaTakim tt WHERE tt.Turnuva = ?", trnObj);
			var TT = Db.SQL<TTDB.TurnuvaTakim>("SELECT tt FROM TurnuvaTakim tt WHERE tt.Turnuva.ObjectId = ?", TurnuvaID);

			TrnTkmsElementJson te;
			foreach(var tt in TT) {
				te = this.TrnTkms.Add();
				te.TakimAd = string.Format("{0} ·{1}", tt.Takim.Ad, tt.Takim.GetObjectID());
				te.ID = tt.GetObjectID();
				te.MF = false;

				var ozet = tt.Ozet;
				te.MusabakaWin = ozet.MusabakaWin;
				te.MusabakaLost = ozet.MusabakaLost;
				te.MusabakaTie = ozet.MusabakaTie;
				te.MusabakaOynadigi = ozet.MusabakaOynadigi;
				te.Puan = ozet.Puan;

				//var tID = te.TakimAd.Substring(te.TakimAd.IndexOf('·')+1);
			}

			LookupTakim.Clear();
			var takimlar = Db.SQL<TTDB.Takim>("SELECT t FROM Takim t");
			foreach(var takim in takimlar) {
				string s = "'" + takim.Ad + " ·" + takim.GetObjectID() + "'";
				LookupTakim.Add(new Json(s));
			}

			reading = false;
		}

		protected override void OnData()
		{
			base.OnData();

			var trn = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));

			htid = "TrnTkm" + TurnuvaID;
			Heading = trn.Ad + " Takýmlarý";

			RefreshTurnuvaTakim();
		}

		void Handle(Input.Insert action)
		{
			reading = true;
			var p = this.TrnTkms.Add();
			//p.TakimAd = "deneme  ·W4";
			reading = false;
		}

		void Handle(Input.Refresh action)
		{
			RefreshTurnuvaTakim();
		}

		void Handle(Input.Save action)
		{
			reading = true;
			bool deleteVar = false;
			foreach(var tkm in TrnTkms) {
				if(tkm.MF) {
					if(!string.IsNullOrEmpty(tkm.ID)) {
						var trnObj = (TTDB.TurnuvaTakim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(tkm.ID));
						if(tkm.DF) {
							trnObj.Delete();
							deleteVar = true;
						}
						else { // Update
							trnObj.Takim = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TTDB.Hlpr.GetIdFromText(tkm.TakimAd)));
						}
					}
					else { // Insert
						var newTT = new TTDB.TurnuvaTakim();
						tkm.ID = newTT.GetObjectID();
						newTT.Turnuva = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
						newTT.Takim = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TTDB.Hlpr.GetIdFromText(tkm.TakimAd)));
					}
				}
			}
			Transaction.Commit();

			if(deleteVar)
				RefreshTurnuvaTakim();
			else {
				for(int i = 0; i < TrnTkms.Count; i++)
					TrnTkms[i].MF = false;
			}
			reading = false;
		}

		void Handle(Input.GetOyuncular action)
		{
			if(string.IsNullOrEmpty(CurRowID))
				return;

			var oyuncular = new TrnTkmOyn();
			oyuncular.TurnuvaID = TurnuvaID;

			var trnTkm = (TTDB.TurnuvaTakim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(CurRowID));
			oyuncular.TakimID = trnTkm.Takim.GetObjectID();
			/*
			string takimAd = "";
			for(int i = 0; i < TrnTkms.Count; i++)
				if(TrnTkms[i].ID == CurRowID) {
					takimAd = TrnTkms[i].TakimAd;
					break;
				}

			oyuncular.htid = "TrnTkmOyn" + CurRowID;
			var result = TTDB.Hlpr.GetIdsFromText(takimAd);
			oyuncular.TakimID = result.Item2;
			oyuncular.Heading = result.Item1 + " Oyuncularý"; */

			oyuncular.Data = null;	// Trigger TrnTkmOyn.OnData
			RecentOyuncular = oyuncular;
		}

		[TrnTkm_json.TrnTkms]
		partial class TrnTkmsElementJson : Json
		{
			protected override void OnData()
			{
				base.OnData();

				//ID = Data.GetObjectID();
			}

			protected override void HasChanged(TValue property)
			{
				var parent = (TrnTkm)this.Parent.Parent;
				if(!parent.reading && MF == false && property.PropertyName != "MF")
					MF = true;
			}

		}
	}
}
