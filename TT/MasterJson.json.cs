using Starcounter;

namespace TT
{
    partial class MasterJson : Json
    {
        void Handle(Input.OyuncularToggle inp)
        {
            if (OyuncularOpened == "true")
                OyuncularOpened = "";
            else {
                ((OyuncularJson)RecentOyuncular).RefreshData();
                OyuncularOpened = "true";
            }
        }

        void Handle(Input.TurnuvalarToggle inp)
        {
            if (TurnuvalarOpened == "true")
                TurnuvalarOpened = "";
            else {
                ((TurnuvalarJson)RecentTurnuvalar).RefreshData();
                TurnuvalarOpened = "true";
            }
        }

        void Handle(Input.TakimlarToggle inp)
        {
            if (TakimlarOpened == "true")
                TakimlarOpened = "";
            else {
                //((TurnuvalarJson)RecentTurnuvalar).RefreshData();
                TakimlarOpened = "true";
            }
        }

        void Handle(Input.TurnuvaEkleToggle inp)
        {
            ((PaperDialog)RecentTurnuvaEkle).Opened = true;
        }
    }
}
