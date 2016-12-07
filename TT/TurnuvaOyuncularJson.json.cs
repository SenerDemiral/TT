using Starcounter;
using System.Linq;

namespace TT
{
    partial class TurnuvaOyuncularJson : Json
    {
        public void RefreshData(string turnuvaID)
        {
            var turnuva = DbHelper.FromID(DbHelper.Base64DecodeObjectID(turnuvaID));
            Oyuncular.Data = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TTDB.TurnuvaTakim o WHERE o.Turnuva = ?", turnuva).OrderByDescending(x => x.Ozet.Puan);
        }
    }
}
