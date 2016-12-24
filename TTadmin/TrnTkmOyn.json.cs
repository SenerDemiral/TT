using Starcounter;
using Starcounter.Templates;

namespace TTadmin
{
   partial class TrnTkmOyn : Json
	{
		bool reading = false;

		public void RefreshTurnuvaTakimOyuncu()
		{
			reading = true;

			TrnTkmOyns.Clear();

			var trnObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
			var tkmObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(TakimID));
			var ttRows = Db.SQL<TTDB.TakimOyuncu>("SELECT tt FROM TakimOyuncu tt WHERE tt.Turnuva.ObjectId = ? AND tt.Takim.ObjectId = ?", TurnuvaID, TakimID);

			TrnTkmOynsElementJson te;
			foreach(var row in ttRows) {
				te = this.TrnTkmOyns.Add();
				te.OyuncuAd = string.Format("{0} ·{1}", row.Oyuncu.Ad, row.Oyuncu.GetObjectID());
				te.ID = row.GetObjectID();
				te.MF = false;

				//var tID = te.TakimAd.Substring(te.TakimAd.IndexOf('·')+1);
			}

			LookupOyuncu.Clear();
			var oRows = Db.SQL<TTDB.Oyuncu>("SELECT t FROM Oyuncu t");
			foreach(var r in oRows) {
				string s = "'" + r.Ad + " ·" + r.GetObjectID() + "'";
				LookupOyuncu.Add(new Json(s));
			}

			reading = false;
		}

		protected override void OnData()
		{
			base.OnData();

			var tkm = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TakimID));

			htid = "TrnTkmOyn" + TakimID;
			Heading = tkm.Ad + " Oyuncularý";

			RefreshTurnuvaTakimOyuncu();
		}

		void Handle(Input.Insert action)
		{
			reading = true;
			var p = this.TrnTkmOyns.Add();
			//p.TakimAd = "deneme  ·W4";
			reading = false;
		}

		void Handle(Input.Refresh action)
		{
			RefreshTurnuvaTakimOyuncu();
		}

		void Handle(Input.Save action)
		{
			reading = true;
			bool deleteVar = false;
			foreach(var pet in TrnTkmOyns) {
				if(pet.MF) {
					if(!string.IsNullOrEmpty(pet.ID)) {
						var curTO = (TTDB.TakimOyuncu)DbHelper.FromID(DbHelper.Base64DecodeObjectID(pet.ID));
						if(pet.DF) {
							curTO.Delete();
							deleteVar = true;
						}
						else {
							curTO.Oyuncu = (TTDB.Oyuncu)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TTDB.Hlpr.GetIdFromText(pet.OyuncuAd)));
						}
					}
					else {
						var newTO = new TTDB.TakimOyuncu();
						pet.ID = newTO.GetObjectID();
						newTO.Turnuva = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
						newTO.Takim = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TakimID));
						newTO.Oyuncu = (TTDB.Oyuncu)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TTDB.Hlpr.GetIdFromText(pet.OyuncuAd)));
					}
				}
			}
			Transaction.Commit();

			if(deleteVar)
				RefreshTurnuvaTakimOyuncu();
			else {
				for(int i = 0; i < TrnTkmOyns.Count; i++)
					TrnTkmOyns[i].MF = false;
			}
			reading = false;
		}

		[TrnTkmOyn_json.TrnTkmOyns]
		partial class TrnTkmOynsElementJson : Json
		{
			protected override void OnData()
			{
				base.OnData();

				//ID = Data.GetObjectID();
			}

			protected override void HasChanged(TValue property)
			{
				var parent = (TrnTkmOyn)this.Parent.Parent;
				if(!parent.reading && MF == false && property.PropertyName != "MF")
					MF = true;
			}

		}
	}
}
