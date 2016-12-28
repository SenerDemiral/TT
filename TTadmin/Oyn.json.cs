using Starcounter;
using Starcounter.Templates;

namespace TTadmin
{
    partial class Oyn : Json
    {
		bool reading = false;

		public void RefreshOyuncu()
		{
			reading = true;
			Oyns.Clear();

			var oyns = Db.SQL<TTDB.Oyuncu>("SELECT tt FROM Oyuncu tt");
			
			OynsElementJson oej;
			foreach(var rec in oyns) {
				oej = this.Oyns.Add();

				oej.ID = rec.GetObjectID();
				oej.Ad = rec.Ad;
				oej.Sex = rec.Sex;
				oej.Tel = rec.Tel;
				oej.eMail = rec.eMail;
				oej.DgmYil = rec.DgmYil;

				oej.MF = false;
			}		
			reading = false;
		}

		protected override void OnData()
		{
			base.OnData();

			RefreshOyuncu();
		}

		void Handle(Input.Insert action)
		{
			reading = true;
			var p = Oyns.Add();
			p.Ad = "";
			reading = false;
		}

		void Handle(Input.Refresh action)
		{
			RefreshOyuncu();
		}

		void Handle(Input.Save action)
		{
			reading = true;
			bool deleteVar = false;

			TTDB.Oyuncu rec;
			foreach(var oyn in Oyns) {
				if(oyn.MF) {
					if(!string.IsNullOrEmpty(oyn.ID)) {
						rec = (TTDB.Oyuncu)DbHelper.FromID(DbHelper.Base64DecodeObjectID(oyn.ID));
						if(oyn.DF) {
							rec.Delete();

							deleteVar = true;
						}
						else {
							rec.Ad = oyn.Ad;
						}
					}
					else {
						rec = new TTDB.Oyuncu();
						
						oyn.ID = rec.GetObjectID();
						rec.Ad = oyn.Ad;
					}
				}
			}
			Transaction.Commit();

			if(deleteVar)
				RefreshOyuncu();
			else {
				for(int i = 0; i < Oyns.Count; i++)
					Oyns[i].MF = false;
			}
			reading = false;
		}

		[Oyn_json.Oyns]
		partial class OynsElementJson : Json
		{
			protected override void OnData()
			{
				base.OnData();
			}

			protected override void HasChanged(TValue property)
			{
				var parent = (Oyn)this.Parent.Parent;
				if(!parent.reading && MF == false && property.PropertyName != "MF")
					MF = true;
			}

			void Handle(Input.Ad inp)
			{
				var a = inp.OldValue;
				var b = inp.Value;
				var c = inp.Template;
				//MF = true;
			}
		}

	}
}
