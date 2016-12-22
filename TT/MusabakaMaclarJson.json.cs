using Starcounter;

namespace TT
{
    partial class MusabakaMaclarJson : Json, IBound<TTDB.Mac>
    {
        public void RefreshData(string musabakaID)
        {
            var musabaka = DbHelper.FromID(DbHelper.Base64DecodeObjectID(musabakaID));
            Maclar = Db.SQL("SELECT o FROM Mac o where o.Musabaka = ? ORDER BY o.Sira DESC", musabaka);
      //    Musabakalar = Db.SQL("SELECT o FROM TurnuvaMusabaka o where o.Turnuva = ?", DbHelper.Base64DecodeObjectID(turnuvaID));
        }

    }
}
