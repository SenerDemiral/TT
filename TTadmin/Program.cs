using System;
using Starcounter;

namespace TTadmin
{
   class Program
   {
		static void Main()
		{
			Console.WriteLine("TTadmin");

			var html = @"<!DOCTYPE html>
				<html>
				<head>
					<meta charset=""utf-8"">
				    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
					<title>{0}</title>
					<script src=""/sys/webcomponentsjs/webcomponents.min.js""></script>
					<link rel=""import"" href=""/sys/polymer/polymer.html"">
					<link rel=""import"" href=""/sys/starcounter.html"">
					<link rel=""import"" href=""/sys/starcounter-include/starcounter-include.html"">
					<link rel=""import"" href=""/sys/starcounter-debug-aid/src/starcounter-debug-aid.html"">
					<link rel=""import"" href=""/sys/bootstrap.html"">
				    <link rel=""import"" href=""/sys/iron-collapse/iron-collapse.html"">
					<link rel=""import"" href=""/TTadmin/simple-overlay.html"">
					<link rel=""import"" href=""/sys/can-highlighted-row-id/can-highlighted-row-id.html"">
					<link rel=""import"" href=""/sys/hot-table/hot-table.html"">
					<link rel=""import"" href=""/sys/paper-styles/color.html"">
			   		<link rel=""import"" href=""/sys/paper-card/paper-card.html"">
				  	<link rel=""import"" href=""/sys/iron-flex-layout/iron-flex-layout-classes.html"">
					<link rel=""stylesheet"" href=""/TTadmin/style.css"">
						
					<style>
						body {{
							margin: 0px;
						}}
					</style>
				</head>
				<body>
					<template is=""dom-bind"" id=""puppet-root"">
						<template is=""imported-template"" content$=""{{{{model.Html}}}}"" model=""{{{{model}}}}""></template>
					</template>
					<puppet-client ref=""puppet-root"" remote-url=""{1}""></puppet-client>
					<starcounter-debug-aid></starcounter-debug-aid>
				</body>
				</html>";

			Application.Current.Use(new HtmlFromJsonProvider());
			Application.Current.Use(new PartialToStandaloneHtmlProvider(html));

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
						//master.Session = new Session(SessionOptions.PatchVersioning);
						master.Session = new Session(SessionOptions.PatchVersioning);
						//master.Session = new Session(Session.Flags.PatchVersioning);
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