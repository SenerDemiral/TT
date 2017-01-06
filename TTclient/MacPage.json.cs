using Starcounter;

namespace TTclient
{
	partial class MacPage : Json
	{
		protected override void OnData()
		{
			base.OnData();

			//Musabakalar

			Maclar = Db.SQL<TTDB.Mac>("SELECT tt FROM Mac tt WHERE tt.Musabaka.ObjectId = ? ORDER BY tt.Skl DESC, tt.Sira DESC", MusabakaID);

			/*
			Turnuvalar.Clear();
			TurnuvaPageElementJson te;
			foreach(var trn in trns) {
				te = Turnuvalar.Add();
				te.ID = trn.GetObjectID();
				te.Ad = trn.Ad;
				te.Tarih = string.Format("{0:dd.MM.yy}", trn.Trh);
			}*/
		}
	}
}
