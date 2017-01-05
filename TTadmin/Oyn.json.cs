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

			Oyns = Db.SQL<TTDB.Oyuncu>("SELECT tt FROM Oyuncu tt");

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
