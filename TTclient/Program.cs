using System;
using Starcounter;

namespace TTclient
{
	class Program
	{
		static void Main()
		{
			Console.WriteLine("ttClient");
			Application.Current.Use(new HtmlFromJsonProvider());
			//Application.Current.Use(new PartialToStandaloneHtmlProvider());

			//TTDB.InitDB initDB = new TTDB.InitDB();
			//initDB.Deneme();

			Handle.GET("/TTclient", () => {
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
					
					var turnuvaSay = Db.SQL<Int64>("SELECT COUNT(t) FROM TTDB.Turnuva t").First;
					master.TurnuvalarHeader = $"Turnuvalar {turnuvaSay}";
					var oyuncuSay = Db.SQL<Int64>("SELECT COUNT(t) FROM Oyuncu t").First;
					master.OyuncularHeader = $"Oyuncular {oyuncuSay}";
					var takimSay = Db.SQL<Int64>("SELECT COUNT(t) FROM Takim t").First;
					master.TakimlarHeader = $"Takimlar {takimSay}";
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
				return "";
			});
		}
	}
}