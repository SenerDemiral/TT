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

			var trnObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
			//var TT = Db.SQL<TTDB.TurnuvaTakim>("SELECT tt FROM TurnuvaTakim tt WHERE tt.Turnuva = ?", trnObj);
			var TT = Db.SQL<TTDB.TurnuvaTakim>("SELECT tt FROM TurnuvaTakim tt WHERE tt.Turnuva.ObjectId = ?", TurnuvaID);

			TrnTkmsElementJson te;
			foreach(var tt in TT) {
				te = this.TrnTkms.Add();
				te.TakimAd = string.Format("{0} ·{1}", tt.Takim.Ad, tt.Takim.GetObjectID());
				te.ID = tt.GetObjectID();
				te.MF = false;

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
			foreach(var pet in TrnTkms) {
				var aaa = pet.ChangeLog;
				if(pet.MF) {
					if(!string.IsNullOrEmpty(pet.ID)) {
						var trnObj = (TTDB.TurnuvaTakim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(pet.ID));
						if(pet.DF) {
							trnObj.Delete();

							deleteVar = true;
						}
						else {
							var tkmID = pet.TakimAd.Substring(pet.TakimAd.IndexOf('·') + 1);
							trnObj.Takim = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(tkmID));
							//trnObj.Ad = pet.TakimAd;
						}
					}
					else {
						var t = new TTDB.TurnuvaTakim();
						pet.ID = t.GetObjectID();
						t.Turnuva = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
						var tkmID = pet.TakimAd.Substring(pet.TakimAd.IndexOf('·')+1);
						t.Takim = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(tkmID));
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

			string takimAd = "";
			for(int i = 0; i < TrnTkms.Count; i++)
				if(TrnTkms[i].ID == CurRowID) {
					takimAd = TrnTkms[i].TakimAd;
					break;
				}

			//string s = TrnTkms[a].TakimAd;
			//var tkmID = s.Substring(s.IndexOf('·') + 1);

			oyuncular.htid = "TrnTkmOyn" + CurRowID;
			oyuncular.TurnuvaID = TurnuvaID;
			//oyuncular.TakimID = TTDB.Hlpr.GetIdFromText(TrnTkms[a].TakimAd);
			var result = TTDB.Hlpr.GetIdsFromText(takimAd);
			oyuncular.TakimID = result.Item2;
			//oyuncular.Heading = TrnTkms[a].TakimAd + " Oyuncularý";
			oyuncular.Heading = result.Item1 + " Oyuncularý";
			oyuncular.Data = null;

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
