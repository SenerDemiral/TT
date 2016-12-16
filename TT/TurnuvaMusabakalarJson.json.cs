using Starcounter;

namespace TT
{
    partial class TurnuvaMusabakalarJson : Json, IBound<TTDB.TurnuvaMusabaka>
    {
        public void RefreshData(string turnuvaID)
        {
            var turnuva = DbHelper.FromID(DbHelper.Base64DecodeObjectID(turnuvaID));
            Musabakalar = Db.SQL("SELECT o FROM TurnuvaMusabaka o where o.Turnuva = ?", turnuva);
        //    Musabakalar = Db.SQL("SELECT o FROM TurnuvaMusabaka o where o.Turnuva = ?", DbHelper.Base64DecodeObjectID(turnuvaID));
        }

        [TurnuvaMusabakalarJson_json.Musabakalar]
        partial class TurnuvaMusabakalarItemPage : Json
        {

            protected override void OnData()
            {
                base.OnData();
                ID = Data.GetObjectID();
                Opened = "";

                Url = string.Format("/tt/turnuvalar/{0}/musabakalar", Data.GetObjectNo());

                //Tarih = string.Format("  [{0:dd.MM.yy}]", (DbHelper.FromID(Data.GetObjectNo()) as TTDB.Turnuva).Trh);
                //MusabakaAd = string.Format("{0} <{1}-{2}> {3}", HomeTakimAd, Ozet.HomePuan, Ozet.GuestPuan, GuestTakimAd);
                //MusabakaInfo = string.Format("Mac<{0}-{1}> Set<{2}-{3}> Sayi<{4}-{5}>", Ozet.HomeMac, Ozet.GuestMac, Ozet.HomeSet, Ozet.GuestSet, Ozet.HomeSayi, Ozet.GuestSayi);
            }

            void Handle(Input.Toggle inp)
            {
                if (Opened == "true")
                    Opened = "";
                else {
                    RecentMaclar = new MusabakaMaclarJson() {
                        Html = "/TT/MusabakaMaclarJson.html",
                         
                    };

                    ((MusabakaMaclarJson)RecentMaclar).RefreshData(this.ID);
                    Opened = "true";
                }
            }
        }
    }
}
