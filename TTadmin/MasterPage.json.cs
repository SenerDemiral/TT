using Starcounter;

namespace TTadmin
{
    partial class MasterPage : Json
    {
		void Handle(Input.TurnuvalarToggle inp)
		{
			TurnuvalarOpened = !TurnuvalarOpened;
			if(TurnuvalarOpened) {
				RecentTurnuvalar = new Trn();
				((Trn)RecentTurnuvalar).RefreshTurnuva();
				//RecentOyuncular.Data = null;
				//turnuvalar.Data = null;
				//RecentTurnuvalar = turnuvalar;
				//((Trn)RecentTurnuvalar).RefreshTurnuva();
				//TurnuvalarOpened = "true";
			}
		}

		void Handle(Input.OyuncularToggle inp)
		{
			OyuncularOpened = !OyuncularOpened;
			if(OyuncularOpened) {
				var oyuncular = new Oyn();
				oyuncular.Data = null;
				RecentOyuncular = oyuncular;

			}
		}

	}
}
