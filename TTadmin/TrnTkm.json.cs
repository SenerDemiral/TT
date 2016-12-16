using Starcounter;

namespace TTadmin
{
    partial class TrnTkm : Json
    {
        public void TrnTkmRefresh(string trnID)
        {
            var trnObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(trnID));
            Takimlar = Db.SQL<TrnTkm>("SELECT tt FROM TurnuvaTakim tt WHERE tt.Turnuva = ?", trnObj);
        }
    }

    [TrnTkm_json.Takimlar]
    partial class TrnTkmTakimlar: Json
    {
        protected override void OnData()
        {
            base.OnData();

            ID = Data.GetObjectID();
        }
    }
}
