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
			var TT = Db.SQL<TTDB.TurnuvaTakim>("SELECT tt FROM TurnuvaTakim tt WHERE tt.Turnuva = ?", trnObj);

			TrnTkmsElementJson te;
			foreach(var tt in TT) {
				te = this.TrnTkms.Add();
				te.TakimAd = string.Format("{0} ·{1}", tt.TakimAd, tt.Takim.GetObjectID());
				te.ID = tt.GetObjectID();
				te.MF = false;

				//var tID = te.TakimAd.Substring(te.TakimAd.IndexOf('·')+1);

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
