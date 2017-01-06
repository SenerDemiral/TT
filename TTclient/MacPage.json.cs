using Starcounter;

namespace TTclient
{
	partial class MacPage : Json
	{
		protected override void OnData()
		{
			base.OnData();

			//Musabakalar

			var msbObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(MusabakaID));
			Maclar = Db.SQL<TTDB.Mac>("SELECT tt FROM Mac tt WHERE tt.Musabaka = ?", msbObj);

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
