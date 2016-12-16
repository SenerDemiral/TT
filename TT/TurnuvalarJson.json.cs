using Starcounter;

namespace TT
{
    partial class TurnuvalarJson : Json, IBound<TTDB.Turnuva>
    {
        public void RefreshData()
        {
            Turnuvalar = Db.SQL("SELECT o FROM Turnuva o");
        }

        [TurnuvalarJson_json.Turnuvalar]
        partial class TurnuvalarItemPage : Json
        {
            protected override void OnData()
            {
                base.OnData();
                ID = Data.GetObjectID();
                Opened = "";
                Tarih = string.Format("  [{0:dd.MM.yy}]", (DbHelper.FromID(Data.GetObjectNo()) as TTDB.Turnuva).Trh);

                Url = string.Format("/tt/turnuvalar/{0}", Data.GetObjectNo());
            }

            void Handle(Input.Toggle inp)
            {
                var a = inp.OldValue;
                var b = inp.Value;

                if (Opened == "true")
                    Opened = "";
                else
                    Opened = "true";

            }

            void Handle(Input.MusabakalarToggle inp)
            {
                var a = inp.OldValue;
                var b = inp.Value;

                if (MusabakalarOpened == "true")
                    MusabakalarOpened = "";
                else {
                    RecentMusabakalar = new TurnuvaMusabakalarJson() {
                        Html = "/TT/TurnuvaMusabakalarJson.html",
                        TurnuvaInfo = this.Ad
                    };

                    ((TurnuvaMusabakalarJson)RecentMusabakalar).RefreshData(this.ID);
                    MusabakalarOpened = "true";
                }
            }

            void Handle(Input.TakimlarToggle inp)
            {
                if (TakimlarOpened == "true")
                    TakimlarOpened = "";
                else {
                    RecentTakimlar = new TurnuvaTakimlarJson() {
                        Html = "/TT/TurnuvaTakimlarJson.html",
                        TurnuvaInfo = Ad
                    };

                    ((TurnuvaTakimlarJson)RecentTakimlar).RefreshData(this.ID);
                    TakimlarOpened = "true";
                }
            }

            void Handle(Input.OyuncularToggle inp)
            {
                if (OyuncularOpened == "true")
                    OyuncularOpened = "";
                else {
                    RecentOyuncular = new TurnuvaOyuncularOzetJson() {
                        Html = "/TT/TurnuvaOyuncularOzetJson.html",
                        TurnuvaInfo = Ad
                    };

                    ((TurnuvaOyuncularOzetJson)RecentOyuncular).RefreshData(this.ID);
                    OyuncularOpened = "true";
                }
            }
        }
    }
}
