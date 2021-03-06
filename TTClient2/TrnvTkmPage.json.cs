using System;
using System.Linq;
using Starcounter;

namespace TTClient2
{
	partial class TrnvTkmPage : Json
	{
		
		[TrnvTkmPage_json]
		protected override void OnData()
		{
			base.OnData();
							

			var trnvObj = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));
			TurnuvaInfo = trnvObj.Ad;
			//TrnvTkm = Db.SQL<TTDB.TurnuvaTakim>("SELECT tt FROM TurnuvaTakim tt WHERE tt.Turnuva = ? ORDER BY tt.TakimAd", trnvObj);
			//TrnvTkm.Data = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TTDB.TurnuvaTakim o WHERE o.Turnuva = ?", trnvObj).OrderByDescending(x => x.Ozet.TrnPuan).ThenByDescending(x => x.Ozet.PuanAV);
			//TrnvTkm.Data = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TTDB.TurnuvaTakim o WHERE o.Turnuva = ?", trnvObj).OrderByDescending(x => x.Ozet.PuanAV);
			
			
			var sw = System.Diagnostics.Stopwatch.StartNew();
			TrnvTkm.Data = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TTDB.TurnuvaTakim o WHERE o.Turnuva = ?", trnvObj).OrderByDescending(x => x.Ozet.TrnPuan).ThenByDescending(x => x.Ozet.PuanAV);;
			/*
			foreach(var r in recs) {
				//TrnvTkmPageElementJson ttp = new TrnvTkmPageElementJson();
				//ttp.PuanA = r.Ozet.PuanA;
			}	   */
			//TrnvTkm.Data = recs;
			sw.Stop();
			Console.WriteLine(string.Format("TrnvTkmPage ms:{0}, tick:{1}", sw.ElapsedMilliseconds, sw.ElapsedTicks));
		}

		[TrnvTkmPage_json.TrnvTkm]
		partial class TrnvTkmPageElementJson : Json 
		{	
			protected override void OnData()
			{
				base.OnData();

				var trnvTkmObj = (TTDB.TurnuvaTakim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(this.ID));
				
				var ozt = trnvTkmObj.Ozet;
				PuanA = ozt.PuanA;
				PuanV = ozt.PuanV;
				PuanAV = PuanA - PuanV;
				MsbkO = ozt.MsbkO;
				MsbkA = ozt.MsbkA;
				MsbkB = ozt.MsbkB;
				MsbkV = ozt.MsbkV;

				TrnPuan = ozt.TrnPuan;

				//var parent = (TrnvTkmPage)this.Parent.Parent;
				//var turnuvaID = parent.TurnuvaID;
				//TrnvTkmMsbkUrl = $"/ttClient2/TrnvTkmMsbk/{turnuvaID}/{TakimID}";
				//TrnvTkmOyncUrl = $"/ttClient2/TrnvTkmOync/{turnuvaID}/{TakimID}";
			} 	
		}

	}
}
