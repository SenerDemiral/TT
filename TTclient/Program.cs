﻿using System;
using Starcounter;
using System.Diagnostics;

namespace TTclient
{
	class Program
	{
		static void Main()
		{
			int ConnCount = 0;
			Console.WriteLine("ttClient");
			Application.Current.Use(new HtmlFromJsonProvider());
			//Application.Current.Use(new PartialToStandaloneHtmlProvider());

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
						//master.Data = null; // Master.OnData yi tetiklemek icin
						master.Session = new Session(SessionOptions.PatchVersioning);
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
				Console.WriteLine(req.Body);
				return Self.GET("/TTclient");
			});
		}
	} 
}