using System;
using Starcounter;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TTclient
{
	class Program
	{
		static void Main()
		{
			int ConnCount = 0;
			Console.WriteLine("ttClient");

			var html = @"<!DOCTYPE html>
				<html>
				<head>
					<meta charset=""utf-8"">
					<title>{0}</title>
					<script src=""/sys/webcomponentsjs/webcomponents.min.js""></script>
					<link rel=""import"" href=""/sys/polymer/polymer.html"">
					<link rel=""import"" href=""/sys/starcounter.html"">
					<link rel=""import"" href=""/sys/starcounter-include/starcounter-include.html"">
					<link rel=""import"" href=""/sys/starcounter-debug-aid/src/starcounter-debug-aid.html"">
					<link rel=""import"" href=""/sys/bootstrap.html"">
					<link rel=""import"" href=""/TTclient/simple-overlay.html"">
					<link rel=""import"" href=""/sys/iron-collapse/iron-collapse.html"">
		 			<link rel=""import"" href=""/sys/app-layout/app-header/app-header.html"">
					<link rel=""import"" href=""/sys/google-map/google-map.html"">
			   		<link rel=""stylesheet"" href=""/TTclient/Master.css"">
				  
					<style>
						body {{
							margin: 0px;
						}}
					</style>

					<style is=""custom-style"">
						:root {{
							--paper-dialog {{
								margin: 0;
								font-size: 12px;
							}}

							--paper-dialog-scrollable {{
									padding-left: 5px;
									padding-right: 5px;
								}}

							h2 {{
									margin-top: 5px;
									padding-left: 5px;
									margin-bottom: 5px;
								}}
						}}

						body {{
								margin: 0;
								font-family: 'Roboto', 'Noto', sans-serif;
								background-color: #eee;
						}}

						app-toolbar {{
							background-color: #4285f4;
							color: #fff;
						}}

						paper-icon-button + [main-title] {{
							margin-left: 24px;
						}}

						paper-progress {{
							display: block;
							width: 100%;
							--paper-progress-active-color: rgba(255, 255, 255, 0.5);
							--paper-progress-container-color: transparent;
						}}

						app-header {{
							position: fixed;
							top: 0;
							left: 0;
							width: 100%;
							background-color: #4285f4;
							color: #fff;
						}}

						app-drawer {{
							--app-drawer-scrim-background: rgba(0, 0, 100, 0.8);
							--app-drawer-content-container: {{
								background-color: #B0BEC5;
							}}
						}}

						section {{
								padding-top: 24px;
						}}

						[main-title] {{
							text-align: center;
							font-size: 20px;
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
			
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "MacSonucMacIdx").First == null)
				Db.SQL("CREATE INDEX MacSonucMacIdx ON MacSonuc(Mac)");
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "MacMsbkIdx").First == null)
				Db.SQL("CREATE INDEX MacMsbkIdx ON Mac(Musabaka)");
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "MusabakaTrnIdx").First == null)
				Db.SQL("CREATE INDEX MusabakaTrnIdx ON Musabaka(Turnuva)");
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "MusabakaTrnHomeTkmIdx").First == null)
				Db.SQL("CREATE INDEX MusabakaTrnHomeTkmIdx ON Musabaka(Turnuva, HomeTakim)");
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "MusabakaTrnGuestTkmIdx").First == null)
				Db.SQL("CREATE INDEX MusabakaTrnGuestTkmIdx ON Musabaka(Turnuva, GuestTakim)");
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "TurnuvaTakimTkmIdx").First == null)
				Db.SQL("CREATE INDEX TurnuvaTakimTkmIdx ON TurnuvaTakim(Takim)");
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "TakimOyuncuTrnIdx").First == null)
				Db.SQL("CREATE INDEX TakimOyuncuTrnIdx ON TakimOyuncu(Turnuva)");
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "TakimOyuncuTkmIdx").First == null)
				Db.SQL("CREATE INDEX TakimOyuncuTkmIdx ON TakimOyuncu(Takim)");
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "TakimOyuncuOynIdx").First == null)
				Db.SQL("CREATE INDEX TakimOyuncuOynIdx ON TakimOyuncu(Oyuncu)");
			
			Handle.GET("/", (Request req) => {
				return Self.GET("/TTclient");
			});

			Handle.GET("/TTclient", (Request req) => {
				Console.WriteLine(ConnCount++);
				Console.WriteLine(DateTime.Now.ToString());
				Console.WriteLine(req.ClientIpAddress.ToString());
				return Db.Scope(() => {
					Master master;

					if(Session.Current != null) {
						master = (Master)Session.Current.Data;
					}
					else {
						master = new Master();
						//master.Session = new Session(SessionOptions.PatchVersioning);
						var sf = Session.Flags.PatchVersioning;
						master.Session = new Session(sf);
					}

					//TTDB.Mac.deneme("dilara");

					var sw = Stopwatch.StartNew();
					var turnuvaSay = Db.SQL<Int64>("SELECT COUNT(t) FROM Turnuva t").First;
					master.TurnuvalarHeader = $"Turnuvalar {turnuvaSay}";
					var oyuncuSay = Db.SQL<Int64>("SELECT COUNT(t) FROM Oyuncu t").First;
					master.OyuncularHeader = $"Oyuncular {oyuncuSay}";
					var takimSay = Db.SQL<Int64>("SELECT COUNT(t) FROM Takim t").First;
					master.TakimlarHeader = $"Takimlar {takimSay}";
					//var macSay = Db.SQL<Int64>("SELECT COUNT(t) FROM Mac t").First;
					//var macSonucSay = Db.SQL<Int64>("SELECT COUNT(t) FROM MacSonuc t").First;
					Console.WriteLine(string.Format("Toplamlar ms:{0}, tick:{1}", sw.ElapsedMilliseconds, sw.ElapsedTicks));

					return master;
				});
			});

			Handle.GET("/TTclient/abc", () => {
				Master master;

				if(Session.Current != null) {
					master = (Master)Session.Current.Data;
					master.Deneme = "SENER";
				}
				else {
					master = new Master();
				}

				return new MacPage();
			});

			Handle.GET("/TTclient/TurnuvaOyuncuMaclar/{?}", (string param1) => {
				var aaa = param1;
				TurnuvaTakimPage ttp = new TurnuvaTakimPage();
				return ttp;
			});

			Handle.GET("/sener", (Request req) => {
				StringBuilder sb = new StringBuilder();

				//var sener = TTDB.Hlpr.OyuncuMaclari("nM");
				var sener = TTDB.Hlpr.OyuncuMaclari("nM").OrderByDescending(x => x.Skl).ThenByDescending(y => y.Trh);
				foreach(var sen in sener) {
					sb.AppendFormat("{5} {0}-{1}-{2}-{3,-30}-{4}", sen.Skl, sen.Sira, sen.GM, sen.RakipAd, sen.RakipTakimAd, sen.Tarih);
					sb.AppendLine();
				}
				Console.WriteLine(req.Body);
				return sb.ToString();
			});
		}
	} 
}