using System;
using Starcounter;

namespace TTadmin
{
   class Program
   {
		static void Main()
		{
			Console.WriteLine("TTadmin");
			Application.Current.Use(new HtmlFromJsonProvider());
			Application.Current.Use(new PartialToStandaloneHtmlProvider());

			//TTDB.InitDB initDB = new TTDB.InitDB();
			//initDB.Deneme();
			//initDB.Init();

			Handle.GET("/TTadmin", () => {
				return Db.Scope(() => {
					MasterPage master;

					if(Session.Current != null) {
						master = (MasterPage)Session.Current.Data;
					}
					else {
						master = new MasterPage();
						//master.Data = null; // Trn.OnData yi tetiklemek icin
						master.Session = new Session(SessionOptions.PatchVersioning);

						//master.RecentTurnuvalar = new Trn();
					}
					//TTDB.Mac.deneme("dilara");
					return master;
				});
			});

				/*
				Handle.GET("/TTadmin/deneme", () => {
					return Db.Scope(() => {
						var trnTkm = Db.SQL<TTDB.TurnuvaTakim>("SELECT tt FROM TurnuvaTakim tt");
						//var json = new TrnTkm() {
						//    Data = trnTkm
						//};
						var json = new TrnTkm();
						json.TrnTkms = trnTkm;


						//json.Takimlar.Add(new TrnTkmTakimlar() { ID = "deneme" });

						//select * from TTDB.Turnuva t where t.ObjectNo = 1344
						var trn = Db.SQL<TTDB.Turnuva>("SELECT t FROM Turnuva t WHERE t.ObjectNo = ?", 1344).First;


						new TTDB.TurnuvaTakim() {
							Turnuva = (TTDB.Turnuva)DbHelper.FromID(1344)
						};
						Transaction.Current.Commit();

						if(Session.Current == null) {
							Session.Current = new Session(SessionOptions.PatchVersioning);
						}

						json.Session = Session.Current;
						return json;
					});
				});

				Handle.GET("/TTadmin/aaaaa", () => new DatagridPage());


				Handle.GET("/TTadminyyy/www", () => {
					return Db.Scope(() => {
						Trn master;

						if(Session.Current != null) {
							master = (Trn)Session.Current.Data;
						}
						else {
							master = new Trn();
							master.Data = null; // Trn.OnData yi tetiklemek icin
							master.Session = new Session(SessionOptions.PatchVersioning);
						}
						//TTDB.Mac.deneme("dilara");
						return master;
					});
				});

				Handle.GET("/TTadmin/yy", () => {
					return Db.Scope(() => {
						TrnGrid master;

						if(Session.Current != null) {
							master = (TrnGrid)Session.Current.Data;
						}
						else {
							master = new TrnGrid();
							master.Data = null;  // TrnGrid.OnData yi tetiklemek icin
							master.Session = new Session(SessionOptions.PatchVersioning);
						}

						return master;
					});
				});

				Handle.GET("/TTadmin/xx", () => {
					return Db.Scope(() => {
						DatagridPage master;

						if(Session.Current != null) {
							master = (DatagridPage)Session.Current.Data;
						}
						else {
							master = new DatagridPage();
							master.Data = null;
							master.Session = new Session(SessionOptions.PatchVersioning);
						}

						//((OyuncularJson)master.RecentOyuncular).RefreshData();
						//master.FocusedOyuncu = null;

						//((TurnuvalarJson)master.RecentTurnuvalar).RefreshData();

						return master;
					});
				});

				Handle.GET("/TTadmin/dumy", () => {
					MasterPage master;

					if(Session.Current != null) {
						master = (MasterPage)Session.Current.Data;
					}
					else {
						master = new MasterPage() {
							Html = "/TTadmin/MasterPage.html"
						};
						master.Session = new Session(SessionOptions.PatchVersioning);
					}

					//((OyuncularJson)master.RecentOyuncular).RefreshData();
					//master.FocusedOyuncu = null;

					//((TurnuvalarJson)master.RecentTurnuvalar).RefreshData();

					return master;
				});*/
			}
	}
}