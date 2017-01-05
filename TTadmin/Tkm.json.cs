using Starcounter;

namespace TTadmin
{
    partial class Tkm : Json
    {
		bool reading = false;

		public void RefreshTakim()
		{
			reading = true;

			Tkms = Db.SQL<TTDB.Takim>("SELECT tt FROM Takim tt");

			reading = false;
		}

		protected override void OnData()
		{
			base.OnData();

			RefreshTakim();
		}

		void Handle(Input.Refresh action)
		{
			RefreshTakim();
		}

		void Handle(Input.Save action)
		{
			reading = true;
			bool deleteVar = false;
			foreach(var pet in Tkms) {
				if(pet.MF) {
					if(!string.IsNullOrEmpty(pet.ID)) {
						var tkmObj = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(pet.ID));
						if(pet.DF) {
							tkmObj.Delete();

							deleteVar = true;
						}
						else {
							tkmObj.Ad = pet.Ad;
							tkmObj.Tel = pet.Tel;
							tkmObj.Adres = pet.Adres;
							tkmObj.KurYil = (short)pet.KurYil;
							tkmObj.Lat = pet.Lat;
							tkmObj.Lon = pet.Lon;
						}
					}
					else {
						var t = new TTDB.Takim();
						pet.ID = t.GetObjectID();
						t.Ad = pet.Ad;
					}
				}
			}
			Transaction.Commit();

			if(deleteVar)
				RefreshTakim();
			else {
				for(int i = 0; i < Tkms.Count; i++)
					Tkms[i].MF = false;
			}
			reading = false;
		}

	}
}
