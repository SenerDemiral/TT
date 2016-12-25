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
				
			}
		}

	}
}
