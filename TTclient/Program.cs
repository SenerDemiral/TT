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

			Handle.GET("/TTclient", () => {
				return Db.Scope(() => {
					Master master;

					if(Session.Current != null) {
						master = (Master)Session.Current.Data;
					}
					else {
						master = new Master();
						master.Data = null; // Master.OnData yi tetiklemek icin
						master.Session = new Session(SessionOptions.PatchVersioning);
					}
					//TTDB.Mac.deneme("dilara");
					return master;
				});
			});
			/*
			Handle.GET("/TTclient/TurnuvaOyuncuMaclar/{?}", (int OyuncuID, Response response) => {
				return null;
			});	 */
		}
	}
}