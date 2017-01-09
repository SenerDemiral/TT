using Starcounter;
using System.Linq;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace TTclient
{
	partial class TurnuvaTakimPage : Json
	{

		protected override void OnData()
		{
			base.OnData();
			var trnObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(TurnuvaID));

			var sw = Stopwatch.StartNew();
			Takimlar.Data = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TTDB.TurnuvaTakim o WHERE o.Turnuva = ?", trnObj).OrderByDescending(x => x.Ozet.Puan);
			Console.WriteLine(string.Format("TurnuvaTakimPage.OnData-LinqSort ms:{0}, tick:{1}", sw.ElapsedMilliseconds, sw.ElapsedTicks));
			
			/*
			var sw = Stopwatch.StartNew();
			Dictionary<ulong, int> lst = new Dictionary<ulong, int>();
			var aaa = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TurnuvaTakim o WHERE o.Turnuva = ?", trnObj);	// Ozet'e dokunmuyor
			foreach(var a in aaa)
			{
				lst.Add(a.PK, a.Ozet.Puan);
			}
			var list = lst.Keys.ToList();
			list.Sort();
			Takimlar.Clear();
			TakimlarElementJson te;
			foreach(var pk in list)
			{
				var tkm = (TTDB.TurnuvaTakim)DbHelper.FromID(pk);
				te = Takimlar.Add();
				te.ID = tkm.GetObjectID();
				te.TakimID = tkm.TakimID;
				te.TakimAd = tkm.TakimAd;
			}
			Console.WriteLine(string.Format("TurnuvaTakimPage.OnData-List.Sort ms:{0}, tick:{1}", sw.ElapsedMilliseconds, sw.ElapsedTicks));
			var xcv = 1;
			*/
			//Takimlar.Data = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TurnuvaTakim o WHERE o.Turnuva = ?", trnObj);
			//foreach(var tkm in Takimlar) {
			//var ozt = tkm.Ozet.Puan;
			//}


			//var abc = Db.SQL<Int64>("SELECT SUM(Ozet.Puan) FROM TurnuvaTakim o WHERE o.Turnuva = ?", trnObj);

			//Takimlar.Data = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TTDB.TurnuvaTakim o WHERE o.Turnuva = ?", trnObj).OrderByDescending(x => x.Ozet.Puan);
			//Takimlar.Data = Taks.OrderByDescending(x => x.Ozet.Puan); 
			//Console.WriteLine(string.Format("TurnuvaTakimPage.OnData.Sort ms:{0}, tick:{1}", sw.ElapsedMilliseconds, sw.ElapsedTicks));
			/*
			//sw = Stopwatch.StartNew();
			//Takimlar.Data = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TTDB.TurnuvaTakim o WHERE o.Turnuva = ? ORDER BY o.Puan", trnObj);
			//Console.WriteLine(string.Format("TurnuvaTakimPage.OnData ms:{0}, tick:{1}", sw.ElapsedMilliseconds, sw.ElapsedTicks));
			*/

			//var sw = Stopwatch.StartNew();
			//Takimlar = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TTDB.TurnuvaTakim o WHERE o.Turnuva = ?", trnObj);
			//Console.WriteLine(string.Format("TurnuvaTakimPage.OnData ms:{0}, tick:{1}", sw.ElapsedMilliseconds, sw.ElapsedTicks));
			/*			
			
			var sw = Stopwatch.StartNew();
			var tkms = Db.SQL<TTDB.TurnuvaTakim>("SELECT o FROM TTDB.TurnuvaTakim o WHERE o.Turnuva = ?", trnObj);
			Takimlar.Clear();
						TakimlarElementJson te;
						foreach(var trn in tkms) {
							te = Takimlar.Add();
							te.ID = trn.GetObjectID();
							te.TakimID = trn.TakimID;
							te.TakimAd = trn.TakimAd;

							//var sw2 = Stopwatch.StartNew();
							te.Ozet = new TakimlarElementJson.OzetJson();
							var ozt = trn.Ozet;
							te.Ozet.Puan = ozt.Puan;
							te.Ozet.MusabakaWin = ozt.MusabakaWin;
							te.Ozet.MusabakaLost = ozt.MusabakaLost;
							te.Ozet.MusabakaTie = ozt.MusabakaTie; 
							//Console.WriteLine(string.Format("TurnuvaTakimPage.Ozet ms:{0}, tick:{1} mac#:{2}", sw2.ElapsedMilliseconds, sw2.ElapsedTicks, ozt.MacCnt));
						}

			  */

			/*
			TTDB.TT[] tts = new TTDB.TT[11];
			int i = 0;
			foreach(var trn in tkms) {
				TTDB.TT tt = new TTDB.TT();
				tt.PK = trn.PK;
				tt.TakimAd = trn.TakimAd;
				tt.Ozet = new TTDB.TurnuvaTakimOzet();
				tt.Ozet.Puan = trn.Ozet.Puan;
				tts[i] = tt;
				i++;
			}
			var xxx = tts.OrderByDescending(x => x.Ozet.Puan);
			TakimlarElementJson te;
			foreach(var trn in xxx) {
				te = Takimlar.Add();
				//te.ID = trn.GetObjectID();
				te.TakimAd = trn.TakimAd;

				//var sw2 = Stopwatch.StartNew();
				te.Ozet = new TakimlarElementJson.OzetJson();
				var ozt = trn.Ozet;
				te.Ozet.Puan = ozt.Puan;
				//Console.WriteLine(string.Format("TurnuvaTakimPage.Ozet ms:{0}, tick:{1} mac#:{2}", sw2.ElapsedMilliseconds, sw2.ElapsedTicks, ozt.MacCnt));
			}

			Console.WriteLine(string.Format("TurnuvaTakimPage.OnData ms:{0}, tick:{1}", sw.ElapsedMilliseconds, sw.ElapsedTicks));
			*/
		}

		void Handle(Input.TakimMusabakaOpened inp) {
			CurTakimMusabakalari.Data = null;
		}

		void Handle(Input.TakimMapOpened inp) {
			TakimMap = null;
		}

		[TurnuvaTakimPage_json.Takimlar]
		partial class TakimlarElementJson : Json 
		{
			
			void Handle(Input.TakimClick inp)
			{
				var parent = (TurnuvaTakimPage)this.Parent.Parent;
				parent.CurRowID = this.TakimID;
				parent.CurRowTakimAd = this.TakimAd;

				var trnObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(parent.TurnuvaID));
				var tkmObj = DbHelper.FromID(DbHelper.Base64DecodeObjectID(TakimID));
				
				var sw = Stopwatch.StartNew();
				parent.CurTakimMusabakalari.Data =
					Db.SQL<TTDB.Musabaka>("SELECT mm FROM Musabaka mm WHERE mm.Turnuva = ? AND (mm.HomeTakim = ? OR mm.GuestTakim = ?)",
						trnObj, tkmObj, tkmObj)
					.OrderBy(x => x.Trh);
				Console.WriteLine(string.Format("TurnuvaTakimPage.CurTakimMusabakalari ms:{0}, tick:{1}", sw.ElapsedMilliseconds, sw.ElapsedTicks));

				sw = Stopwatch.StartNew();
				parent.CurTakimMusabakalari.Data =
					Db.SQL<TTDB.Musabaka>("SELECT mm FROM Musabaka mm WHERE mm.Turnuva = ? AND (mm.HomeTakim = ? OR mm.GuestTakim = ?) ORDER BY mm.Trh",
						trnObj, tkmObj, tkmObj);

				Console.WriteLine(string.Format("TurnuvaTakimPage.CurTakimMusabakalari ms:{0}, tick:{1}", sw.ElapsedMilliseconds, sw.ElapsedTicks));

				sw = Stopwatch.StartNew();
				parent.CurTakimMusabakalari.Data =
					Db.SQL<TTDB.Musabaka>("SELECT mm FROM Musabaka mm WHERE mm.Turnuva = ? AND (mm.HomeTakim = ? OR mm.GuestTakim = ?)",
						trnObj, tkmObj, tkmObj);

				Console.WriteLine(string.Format("TurnuvaTakimPage.CurTakimMusabakalari ms:{0}, tick:{1}", sw.ElapsedMilliseconds, sw.ElapsedTicks));

				foreach(var msbk in parent.CurTakimMusabakalari)
				{
					var msbkObj = (TTDB.Musabaka)DbHelper.FromID(DbHelper.Base64DecodeObjectID(msbk.ID));
					
					msbk.IsHome = false;
					if(msbkObj.HomeTakim.GetObjectNo() == tkmObj.GetObjectNo())
						msbk.IsHome = true;

					msbk.IsWinner = false;
					msbk.Sonuc = "X";
					if(msbk.IsHome) {
						if(msbkObj.Ozet.HomePuan > msbkObj.Ozet.GuestPuan)
							msbk.Sonuc = "G";
						if(msbkObj.Ozet.HomePuan < msbkObj.Ozet.GuestPuan)
							msbk.Sonuc = "M";
						if(msbkObj.Ozet.HomePuan == msbkObj.Ozet.GuestPuan && msbkObj.Ozet.HomePuan != 0)
							msbk.Sonuc = "B";
					}
					else {
						if(msbkObj.Ozet.HomePuan > msbkObj.Ozet.GuestPuan)
							msbk.Sonuc = "M";
						if(msbkObj.Ozet.HomePuan < msbkObj.Ozet.GuestPuan)
							msbk.Sonuc = "G";
						if(msbkObj.Ozet.HomePuan == msbkObj.Ozet.GuestPuan && msbkObj.Ozet.HomePuan != 0)
							msbk.Sonuc = "B";

						if(msbkObj.Ozet.GuestPuan > msbkObj.Ozet.HomePuan)
							msbk.IsWinner = true;
					}
				}
				parent.TakimMusabakaOpened = true;
			}
			
			void Handle(Input.TakimMapClick inp) {
				var tkmObj = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(TakimID));
				
				var parent = (TurnuvaTakimPage)this.Parent.Parent;
				parent.TakimLat = tkmObj.Lat;
				parent.TakimLon = tkmObj.Lon;
				parent.TakimMapOpened = true;

				parent.TakimMap = new TakimMap() { TakimLat = tkmObj.Lat, TakimLon = tkmObj.Lon }; 
			
			} 	
		}
	}
}
