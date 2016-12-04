using System;
using Starcounter;

namespace TT
{
    partial class OyuncularJson : Json //, IBound<TTDB.Oyuncu>
    {
       
        public void RefreshData()
        {
            Oyuncular = Db.SQL("SELECT o FROM Oyuncu o");
        }

        [OyuncularJson_json.Oyuncular]
        partial class OyuncularItemPage : Json
        {
            
            protected override void OnData()
            {
                base.OnData();
                //var aa = this.Data.GetObjectID
                var ccc = Data.GetObjectID();

                Url = string.Format("/tt/oyuncular/{0}", Data.GetObjectNo());
            }
        }

    }
}
