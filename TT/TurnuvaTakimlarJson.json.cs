using Starcounter;
using System.Linq;

namespace TT
{
    partial class TurnuvaTakimlarJson : Json, IBound<TTDB.TurnuvaTakim>
    {
        public void RefreshData(string turnuvaID)
        {
            var turnuva = DbHelper.FromID(DbHelper.Base64DecodeObjectID(turnuvaID));
            Takimlar = Db.SQL("SELECT o FROM TurnuvaTakim o where o.Turnuva = ?", turnuva);
            var taks = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TurnuvaTakim o where o.Turnuva = ?", turnuva).ToArray();

            var sort = from s in taks
                       orderby  s.Ozet.Puan descending
                       select s;

            //Takimlar = sort;  ??????
            foreach (var item in sort) {
                //item.Ozet.Puan;
               
            }
        }

    }
}
