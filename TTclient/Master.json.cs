using Starcounter;

namespace TTclient
{
	partial class Master : Json
	{
		void Handle(Input.TurnuvalarToggle inp)
		{
			TurnuvalarOpened = !TurnuvalarOpened;
			if(TurnuvalarOpened) {
				Turnuvalar = new TurnuvaPage();
				//(Turnuvalar as TurnuvaPage).RefreshTurnuva();

				Turnuvalar.Data = null;
				
				//var trnPge = new TurnuvaPage();
				//trnPge.Turnuvalar = Db.SQL<TTDB.Turnuva>("SELECT tt FROM Turnuva tt ORDER BY tt.Ad");
				//Turnuvalar = trnPge;
			}
		}

		void Handle(Input.OyuncularToggle inp)
		{
			OyuncularOpened = !OyuncularOpened;
			if(OyuncularOpened) {
				Oyuncular = new OyuncuPage();
				Oyuncular.Data = null;
			}
		}

		void Handle(Input.TakimlarToggle inp)
		{
			TakimlarOpened = !TakimlarOpened;
			if(TakimlarOpened) {
				Takimlar = new TakimPage();
				Takimlar.Data = null;
			}
		}

	}
}
