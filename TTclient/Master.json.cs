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
				Turnuvalar.Data = null;
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
