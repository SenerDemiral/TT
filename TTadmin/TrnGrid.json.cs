using System;
using Starcounter;
using Starcounter.Templates;

namespace TTadmin
{
	partial class TrnGrid : Json
	{
		bool reading = false;

		public void RefreshTurnuva()
		{
			reading = true;
			Trns.Clear();

			var trns = Db.SQL<TTDB.Turnuva>("SELECT tt FROM Turnuva tt");

			TrnGridTrnsElementJson te;
			foreach (var trn in trns) {
				te = this.Trns.Add();
				te.Ad = trn.Ad;
				te.Tarih = string.Format("{0:dd.MM.yy}", trn.Trh);
				te.ID = trn.GetObjectID();
				te.MF = false;
			}
			reading = false;
		}

		protected override void OnData()
		{
			base.OnData();

			RefreshTurnuva();
		}

		protected override void HasChanged(TValue property) // Insert edince calisiyor
		{
		}
	
		void Handle(Input.CurRowID action)
		{
			//var aaa = this.GetObjectID();
			//if (action.Value == "Vu")
			//    Sec.Add(new Json(@"'four ·4'"));
		}

		void Handle(Input.Insert action)
		{
			reading = true;
			var p = Trns.Add();
			p.Ad = "";
			p.Tarih = string.Format("{0:dd.MM.yy}", DateTime.Today);
			reading = false;
		}

		void Handle(Input.Refresh action)
		{
			RefreshTurnuva();
		}

		void Handle(Input.Save action)
		{
			reading = true;
			bool deleteVar = false;
			foreach (var pet in Trns) {
				var aaa = pet.ChangeLog;
				if (pet.MF) {
					if (!string.IsNullOrEmpty(pet.ID)) {
						var trnObj = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(pet.ID));
						if (pet.DF) {
							trnObj.Delete();

							deleteVar = true;
						}
						else {
							trnObj.Ad = pet.Ad;
							trnObj.Trh = DateTime.ParseExact(pet.Tarih, "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture);
						}
					}
					else {
						var t = new TTDB.Turnuva();
						t.Ad = pet.Ad;
						if (string.IsNullOrEmpty(pet.Tarih))
							t.Trh = DateTime.Now;
					}
				}
			}
			Transaction.Commit();

			if (deleteVar)
				RefreshTurnuva();
			else {
				for (int i = 0; i < Trns.Count; i++)
					Trns[i].MF = false;
			}
			reading = false;
			//var trn = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(Pets[0].ID));
			// var trn = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64ForUrlDecode(Pets[0].ID));
			//trn.Ad = Pets[0].Name;
			//Transaction.Commit();
		}

      //[DatagridPage_json.Trns]
		[TrnGrid_json.Trns]
      partial class TrnGridTrnsElementJson : Json
      {
         protected override void OnData()
         {
				base.OnData();
         }

         protected override void HasChanged(TValue property)
         {
				var parent = (TrnGrid)this.Root;

				//if (!parent.Fetching && property.PropertyName != "Degisti")
				if (!parent.reading && MF == false && property.PropertyName != "MF")
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
