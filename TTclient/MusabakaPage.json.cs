using Starcounter;

namespace TTclient
{
	partial class MusabakaPage : Json
	{
		protected override void OnData()
		{
			base.OnData();

			//Musabakalar

			Musabakalar = Db.SQL<TTDB.Musabaka>("SELECT tt FROM Musabaka tt WHERE tt.Turnuva.ObjectId = ? ORDER BY tt.Trh", TurnuvaID);

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
		[MusabakaPage_json.Musabakalar]
		partial class TurnuvaPageElementJson : Json
		{
			protected override void OnData()
			{
				base.OnData();
				//ID = Data.GetObjectID();
			}
			
			void Handle(Input.Toggle inp)
			{
				Opened = !Opened;

				if(Opened) {
					var mac = new MacPage();
					mac.MusabakaID = ID;
					mac.HomeTakimAd = HomeTakimAd;
					mac.GuestTakimAd = GuestTakimAd;
					mac.Data = null;
					RecentMac = mac;
				}
			}
			/*

			void Handle(Input.MusabakaToggle inp)
			{
				MusabakaOpened = !MusabakaOpened;

				if(MusabakaOpened) {
					var musabaka = new MusabakaPage();
					musabaka.TurnuvaID = ID;
					musabaka.Data = null;
					RecentMusabaka = musabaka;
				}

			}
		   */
		}

	}
}
