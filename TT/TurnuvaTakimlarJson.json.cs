using Starcounter;
using System.Linq;

namespace TT
{
    partial class TurnuvaTakimlarJson : Json, IBound<TTDB.TurnuvaTakim>
    {
        public void RefreshData(string turnuvaID)
        {
             
            var turnuva = DbHelper.FromID(DbHelper.Base64DecodeObjectID(turnuvaID));
            //Takimlar = Db.SQL("SELECT o FROM TurnuvaTakim o where o.Turnuva = ?", turnuva);
            //var taks = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TurnuvaTakim o where o.Turnuva = ?", turnuva).ToArray();
            Takimlar.Data = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TurnuvaTakim o WHERE o.Turnuva = ?", turnuva).OrderByDescending(x => x.Ozet.Puan);

            //TurnuvaTakimlarJson page = new TurnuvaTakimlarJson();
            //page.Takimlar.Data = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TurnuvaTakim o WHERE o.Turnuva = ?", turnuva).OrderByDescending(x => x.Ozet.Puan);
            //var abc = Takimlar.OrderBy(x => x.Ozet.Puan);
            /*
            var sort = from s in taks
                       orderby  s.Ozet.Puan descending
                       select s;

            var abc = Takimlar.OrderBy(x => x.Ozet.Puan);

            foreach (var item in sort) {
                //item.Ozet.Puan;   //deneme github  
               
            }*/
        }

    }
}
