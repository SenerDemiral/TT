using Starcounter;
using Starcounter.Templates;

namespace TTclient
{
	partial class TurnuvaPage : Json
	{
		protected override void OnData()
		{
			base.OnData();

			Turnuvalar = Db.SQL<TTDB.Turnuva>("SELECT tt FROM Turnuva tt");
			
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
		
		[TurnuvaPage_json.Turnuvalar]
		partial class TurnuvaPageElementJson : Json
		{
			protected override void OnData()
			{
				base.OnData();
				//ID = Data.GetObjectID();	//Table'dan zate geliyor
			}
			
			void Handle(Input.Toggle inp)
			{
				Opened = !Opened;
			}

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

		}  
	}
}
