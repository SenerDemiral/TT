using Starcounter;
using System.Linq;

namespace TT
{
    partial class TurnuvaOyuncularOzetJson : Json
    {
        public void RefreshData(string turnuvaID)
        {
            //var turnuva = DbHelper.FromID(DbHelper.Base64DecodeObjectID(turnuvaID));
            //Oyuncular.Data = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TTDB.TurnuvaTakim o WHERE o.Turnuva = ?", turnuva).OrderByDescending(x => x.Ozet.Puan);

            //TurnuvaOyuncularOzetJson json = new TurnuvaOyuncularOzetJson();

            //var ccc = TTDB.Hlpr.TurnuvaOyuncularOzet(turnuvaID).OrderByDescending(x => (x.MacG * 2) + x.MacM).ThenBy(y => y.MacM * 2).OrderByDescending(y => y.SetA * 2);
            var ccc = TTDB.Hlpr.TurnuvaOyuncularOzet(turnuvaID).OrderByDescending(x => x.Puan).ThenBy(y => y.MacM).OrderByDescending(y => y.SetA * 2);
            foreach (var o in ccc) {
                OyuncularElementJson item = new OyuncularElementJson();
                item.OyuncuAd = o.OyuncuAd;
                item.TakimAd = o.TakimAd;
                item.Puan = o.Puan;
                item.MacO = o.MacO;
                item.MacG = o.MacG;
                item.MacM = o.MacM;
                item.SetA = o.SetA;
                item.SetV = o.SetV;
                item.SayiA = o.SayiA;
                item.SayiV = o.SayiV;

                Oyuncular.Add(item);
            }
            /*
            TTDB.TurnuvaOyuncularOzet aaa = new TTDB.TurnuvaOyuncularOzet("VA");
            foreach (var o in aaa) {
                //Console.WriteLine(string.Format("{0}-{1}  Mac<{2}-{3}-{4}> Set<{5}-{6}> Sayi<{7}-{8}>", o.OyuncuAd, o.TakimAd, o.MacO, o.MacG, o.MacM, o.SetA, o.SetV, o.SayiA, o.SayiV));

                OyuncularElementJson item = new OyuncularElementJson();
                item.OyuncuAd = o.OyuncuAd;
                item.TakimAd = o.TakimAd;
                item.MacO = o.MacO;
                item.MacG = o.MacG;
                item.MacM = o.MacM;
                item.SetA = o.SetA;
                item.SetV = o.SetV;
                item.SayiA = o.SayiA;
                item.SayiV = o.SayiV;

                Oyuncular.Add(item);
            };*/
        }
    }
}
