using System;
using Starcounter;
using Starcounter.Internal;

namespace TTClient2
{
	class Program
	{
		static void Main()
		{
			Console.WriteLine("TTclient2");

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
					<link rel=""import"" href=""/sys/iron-icons/maps-icons.html"">
					
					<!--
					<link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-alpha.6/css/bootstrap.min.css"" integrity=""sha384-rwoIResjU2yc3z8GV/NPeZWAv56rSmLldC3R/AZzGRnGxQQKnKkoFVhFQhNUwEyJ"" crossorigin=""anonymous"">
					<script src=""https://code.jquery.com/jquery-3.1.1.slim.min.js"" integrity=""sha384-A7FZj7v+d/sdmMqp/nOQwliLvUsJfDHW+k9Omg/a/EheAdgtzNs3hpfag6Ed950n"" crossorigin=""anonymous""></script>
					<script src=""https://cdnjs.cloudflare.com/ajax/libs/tether/1.4.0/js/tether.min.js"" integrity=""sha384-DztdAPBWPRXSA/3eYEEUWrWCy7G5KFbe8fFjk5JAIxUYHKkDx6Qin1DkWx51bBrb"" crossorigin=""anonymous""></script>
					<script src=""https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-alpha.6/js/bootstrap.min.js"" integrity=""sha384-vBWWzlZJ8ea9aCX4pEW3rVHjgjt7zpkNpZk+02D9phzyeVkE+jo0ieGizqPLForn"" crossorigin=""anonymous""></script>
					-->
			   		<link rel=""stylesheet"" href=""/sys/Stylesheet1.css"">
					<style>
						body {{
							margin: 20px;
						}}
					</style>
				</head>
				<body>
					<template is=""dom-bind"" id=""puppet-root"">
						<template is=""imported-template"" content$=""{{{{model.Html}}}}"" model=""{{{{model}}}}""></template>
					</template>
					<puppet-client ref=""puppet-root"" remote-url=""{1}"" use-web-socket=""true""></puppet-client>
					<starcounter-debug-aid></starcounter-debug-aid>
				</body>
				</html>";

			Application.Current.Use(new HtmlFromJsonProvider());
			Application.Current.Use(new PartialToStandaloneHtmlProvider(html));

			/*
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "MusabakaTrnHomeTkmIdx").First == null)
				Db.SQL("CREATE INDEX MusabakaHomeTkmIdx ON Musabaka(Turnuva, HomeTakim)");
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "MusabakaTrnGuestTkmIdx").First == null)
				Db.SQL("CREATE INDEX MusabakaGuestTkmIdx ON Musabaka(Turnuva, GuestTakim)");
			*/
			/*
			Db.SQL("DROP INDEX MacSonucMacIdx ON MacSonuc");
			Db.SQL("DROP INDEX MacMsbkIdx ON Mac");
			Db.SQL("DROP INDEX MusabakaTrnIdx ON Musabaka");
			Db.SQL("DROP INDEX MusabakaTrnHomeTkmIdx ON Musabaka");
			Db.SQL("DROP INDEX MusabakaTrnGuestTkmIdx ON Musabaka");
			Db.SQL("DROP INDEX TurnuvaTakimTkmIdx ON TurnuvaTakim");
			Db.SQL("DROP INDEX TakimOyuncuTrnIdx ON TakimOyuncu");
			Db.SQL("DROP INDEX TakimOyuncuTkmIdx ON TakimOyuncu");
			Db.SQL("DROP INDEX TakimOyuncuOynIdx ON TakimOyuncu");

			Db.SQL("DROP INDEX MacSonuc_MacIdx ON MacSonuc");
			Db.SQL("DROP INDEX Mac_MusbakaIdx ON Mac");
			Db.SQL("DROP INDEX Musabaka_TurnuvaIdx ON Musabaka");
			Db.SQL("DROP INDEX Musabaka_HomeTakimIdx ON Musabaka");
			Db.SQL("DROP INDEX Musabaka_GuestTakimIdx ON Musabaka");
			Db.SQL("DROP INDEX Musabaka_TrhIdx ON Musabaka");
			Db.SQL("DROP INDEX TurnuvaTakim_TakimIdx ON TurnuvaTakim");
			Db.SQL("DROP INDEX TakimOyuncu_TurnuvaIdx ON TakimOyuncu");
			Db.SQL("DROP INDEX TakimOyuncu_TakimIdx ON TakimOyuncu");
			Db.SQL("DROP INDEX TakimOyuncu_OyuncuIdx ON TakimOyuncu");
			*/

			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "ttIndex_MacSonuc_Mac").First == null)
				Db.SQL("CREATE INDEX ttIndex_MacSonuc_Mac ON MacSonuc(Mac)");
			
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "ttIndex_Mac_Musabaka").First == null)
				Db.SQL("CREATE INDEX ttIndex_Mac_Musabaka ON Mac(Musabaka)");
			
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "ttIndex_Mac_HomeOyuncu").First == null)
				Db.SQL("CREATE INDEX ttIndex_Mac_HomeOyuncu ON Mac(HomeOyuncu)");
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "ttIndex_Mac_HomeOyuncu2").First == null)
				Db.SQL("CREATE INDEX ttIndex_Mac_HomeOyuncu2 ON Mac(HomeOyuncu2)");
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "ttIndex_Mac_GuestOyuncu").First == null)
				Db.SQL("CREATE INDEX ttIndex_Mac_GuestOyuncu ON Mac(GuestOyuncu)");
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "ttIndex_Mac_GuestOyuncu2").First == null)
				Db.SQL("CREATE INDEX ttIndex_Mac_GuestOyuncu2 ON Mac(GuestOyuncu2)");
			
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "ttIndex_Musabaka_Turnuva").First == null)
				Db.SQL("CREATE INDEX ttIndex_Musabaka_Turnuva ON Musabaka(Turnuva)");
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "ttIndex_Musabaka_HomeTakim").First == null)
				Db.SQL("CREATE INDEX ttIndex_Musabaka_HomeTakim ON Musabaka(HomeTakim)");
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "ttIndex_Musabaka_GuestTakim").First == null)
				Db.SQL("CREATE INDEX ttIndex_Musabaka_GuestTakim ON Musabaka(GuestTakim)");
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "ttIndex_Musabaka_Trh").First == null)
				Db.SQL("CREATE INDEX ttIndex_Musabaka_Trh ON Musabaka(Trh)");

			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "ttIndex_TurnuvaTakim_Turnuva").First == null)
				Db.SQL("CREATE INDEX ttIndex_TurnuvaTakim_Turnuva ON TurnuvaTakim(Turnuva)");
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "ttIndex_TurnuvaTakim_Takim").First == null)
				Db.SQL("CREATE INDEX ttIndex_TurnuvaTakim_Takim ON TurnuvaTakim(Takim)");

			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "ttIndex_TakimOyuncu_Turnuva").First == null)
				Db.SQL("CREATE INDEX ttIndex_TakimOyuncu_Turnuva ON TakimOyuncu(Turnuva)");
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "ttIndex_TakimOyuncu_Takim").First == null)
				Db.SQL("CREATE INDEX ttIndex_TakimOyuncu_Takim ON TakimOyuncu(Takim)");
			if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "ttIndex_TakimOyuncu_Oyuncu").First == null)
				Db.SQL("CREATE INDEX ttIndex_TakimOyuncu_Oyuncu ON TakimOyuncu(Oyuncu)");

			/*
			Handle.GET("/TTclient2", () =>
			{
				MasterPage master;
				if(Session.Current != null)
				{
					master = (MasterPage)Session.Current.Data;
				}
				else
				{
					master = new MasterPage();
					var sf = Session.Flags.PatchVersioning;
					master.Session = new Session(sf);
				}
				return master;

				
				return Db.Scope(() => {
					MasterPage master;

					if(Session.Current != null) {
						master = (MasterPage)Session.Current.Data;
					}
					else {
						master = new MasterPage();
						//master.Data = null; // Trn.OnData yi tetiklemek icin
						//master.Session = new Session(SessionOptions.PatchVersioning);
						var sf = Session.Flags.PatchVersioning;
						master.Session = new Session(sf);
						//master.RecentTurnuvalar = new Trn();
					}
					//TTDB.Mac.deneme("dilara");
					return master;
				});
			});
			*/
			Handle.GET("/", () => {
				return Self.GET("/ttClient2/master");
			});
			
			Handle.GET("/ttClient2/master", (Request req) => {
				return Db.Scope(() => {
					//MasterPage master;

					if(Session.Current != null) {
						return (MasterPage)Session.Current.Data;
					}
					else {
						var master = new MasterPage();
						var cv = Starcounter.Internal.CurrentVersion.Version;
						master.Session = new Session(SessionOptions.PatchVersioning);
						//master.Session = new Session(Session.Flags.PatchVersioning);
						master.CurrentPage = new NavPage();
						// deneme
						return master;
					}
				});
			});
			/*
			Handle.GET("/TTclient2/masterX", () =>
            {
				return Db.Scope(() =>
				{
					Session session = Session.Current;
					if(session != null && session.Data != null)
						return session.Data;

					var master = new MasterPage();

					if(session == null)
					{
						session = new Session(SessionOptions.PatchVersioning);
					}

					var nav = new NavPage();
					master.CurrentPage = nav;

					master.Session = session;
					return master;
				});
            });
			*/
			Handle.GET("/ttClient2", () => Self.GET("/TTclient2/master"));

			//Handle.GET("/ttClient2/partial/Trnv", () => new TrnvPage());
            //Handle.GET("/ttClient2/Trnv", () => WrapPage<TrnvPage>("/ttClient2/partial/Trnv"));

			
			//Handle.GET("/ttClient2/partial/TrnvTkm/{?}", (string param) => new TrnvTkmPage());

			Handle.GET("/ttClient2/Bilgi/RankCalc", () =>
			{
				var master = (MasterPage)Self.GET("/TTclient2/master");
				var nav = master.CurrentPage as NavPage;
				nav.CurrentPage = new Bilgi.RankCalc();
				nav.CurrentPage.Data = null;
                
				return master;
			});

			Handle.GET("/ttClient2/Trnv", () =>
			{
				var master = (MasterPage)Self.GET("/TTclient2/master");
				var nav = master.CurrentPage as NavPage;
				nav.CurrentPage = new TrnvPage();
				nav.CurrentPage.Data = null;
                
				return master;
			});
			Handle.GET("/ttClient2/TrnvTkm/{?}", (string trnvID) =>
			{
				var master = (MasterPage)Self.GET("/ttClient2/master");
				
				var nav = master.CurrentPage as NavPage;
				/*
				nav.CurrentPage = new TrnvTkmPage();
				(nav.CurrentPage as TrnvTkmPage).TurnuvaID = trnvID;
				nav.CurrentPage.Data = null;
				*/
				
				if (!(nav.CurrentPage is TrnvTkmPage)) {
					nav.CurrentPage = new TrnvTkmPage();
					(nav.CurrentPage as TrnvTkmPage).TurnuvaID = trnvID;
					nav.CurrentPage.Data = null;
                }
                return master;
			});

			Handle.GET("/ttClient2/TrnvTkmMsbk/{?}/{?}", (string trnvID, string tkmID) =>
			{
				var master = (MasterPage)Self.GET("/TTclient2/master");
				var nav = master.CurrentPage as NavPage;
				nav.CurrentPage = new TrnvTkmMsbkPage();
				(nav.CurrentPage as TrnvTkmMsbkPage).TurnuvaID = trnvID;
				(nav.CurrentPage as TrnvTkmMsbkPage).TakimID = tkmID;
				nav.CurrentPage.Data = null;
                
				return master;
			});

			Handle.GET("/ttClient2/TrnvMsbk/{?}", (string trnvID) =>
			{
				var master = (MasterPage)Self.GET("/TTclient2/master");
				var nav = master.CurrentPage as NavPage;
				nav.CurrentPage = new TrnvMsbkPage();
				(nav.CurrentPage as TrnvMsbkPage).TurnuvaID = trnvID;
				nav.CurrentPage.Data = null;
                
				return master;
			});

			Handle.GET("/ttClient2/MsbkMac/{?}", (string msbkID) =>
			{
				var master = (MasterPage)Self.GET("/TTclient2/master");
				var nav = master.CurrentPage as NavPage;
				nav.CurrentPage = new MsbkMacPage();
				(nav.CurrentPage as MsbkMacPage).MusabakaID = msbkID;
				nav.CurrentPage.Data = null;
                
				return master;
			});
			Handle.GET("/ttClient2/TrnvTkmOync/{?}/{?}", (string trnvID, string tkmID) =>
			{
				var master = (MasterPage)Self.GET("/ttClient2/master");
				var nav = master.CurrentPage as NavPage;
				nav.CurrentPage = new TrnvTkmOyncPage();
				(nav.CurrentPage as TrnvTkmOyncPage).TurnuvaID = trnvID;
				(nav.CurrentPage as TrnvTkmOyncPage).TakimID = tkmID;
				nav.CurrentPage.Data = null;
                
				return master;
			});
			Handle.GET("/ttClient2/TrnvTkmOyncMac/{?}/{?}/{?}", (string trnvID, string tkmID, string oyncID) =>
			{
				var master = (MasterPage)Self.GET("/ttClient2/master");
				var nav = master.CurrentPage as NavPage;
				nav.CurrentPage = new TrnvTkmOyncMacPage();
				(nav.CurrentPage as TrnvTkmOyncMacPage).TurnuvaID = trnvID;
				(nav.CurrentPage as TrnvTkmOyncMacPage).TakimID = tkmID;
				(nav.CurrentPage as TrnvTkmOyncMacPage).OyuncuID = oyncID;
				nav.CurrentPage.Data = null;
                
				return master;
			});
			Handle.GET("/ttClient2/OyncRank", () =>
			{
				var master = (MasterPage)Self.GET("/TTclient2/master");
				var nav = master.CurrentPage as NavPage;
				nav.CurrentPage = new OyncRankPage();
				nav.CurrentPage.Data = null;
                
				return master;
			});
			Handle.GET("/ttClient2/OyncMac/{?}", (string oyncID) =>
			{
				var master = (MasterPage)Self.GET("/TTclient2/master");
				var nav = master.CurrentPage as NavPage;
				nav.CurrentPage = new OyncMacPage();
				(nav.CurrentPage as OyncMacPage).OyuncuID = oyncID;
				nav.CurrentPage.Data = null;
                
				return master;
			});


			/*
			Handle.GET("/ttClient2/TrnvTkm/{?}", (string param) =>
			{
				var master = WrapPage<TrnvTkmPage>("/ttClient2/partial/TrnvTkm/"+param) as MasterPage;
                var nav = master.CurrentPage as NavPage;
                var page = nav.CurrentPage as TrnvTkmPage;
				page.TrnvID = param;
				page.Data = null;
                //page.YourFavoriteFood = "You've got some tasty " + param;
                return master;
			});
			*/


			Handle.GET("/ttClient2/partial/TrnvTkmMsbk", () => new TrnvTkmMsbkPage());
            Handle.GET("/ttClient2/TrnvTkmMsbk", () => WrapPage<TrnvTkmMsbkPage>("/ttClient2/partial/TrnvTkmMsbk"));

			Handle.GET("/ttClient2/partial/TrnvTkmOync", () => new TrnvTkmOyncPage());
            Handle.GET("/ttClient2/TrnvTkmOync", () => WrapPage<TrnvTkmOyncPage>("/ttClient2/partial/TrnvTkmOync"));

			Handle.GET("/ttClient2/partial/TrnvTkmOyncMac", () => new TrnvTkmOyncMacPage());
            Handle.GET("/ttClient2/TrnvTkmOyncMac", () => WrapPage<TrnvTkmOyncMacPage>("/ttClient2/partial/TrnvTkmOyncMac"));

			Handle.GET("/ttClient2/partial/TkmMap", () => new TkmMapPage());
            Handle.GET("/ttClient2/TkmMap", () => WrapPage<TkmMapPage>("/ttClient2/partial/TkmMap"));

			Handle.GET("/ttClient2/partial/TrnvMsbk", () => new TrnvMsbkPage());
            Handle.GET("/ttClient2/TrnvMsbk", () => WrapPage<TrnvMsbkPage>("/ttClient2/partial/TrnvMsbk"));

			Handle.GET("/ttClient2/partial/TrnvOync", () => new TrnvOyncPage());
            Handle.GET("/ttClient2/TrnvOync", () => WrapPage<TrnvOyncPage>("/ttClient2/partial/TrnvOync"));




			Handle.GET("/ttClient2/partial/text", () => new TextPage());
			Handle.GET("/ttClient2/text", () => WrapPage<TextPage>("/ttClient2/partial/text"));

			Handle.GET("/ttClient2/partial/button", () => new ButtonPage());
            Handle.GET("/ttClient2/button", () => WrapPage<ButtonPage>("/ttClient2/partial/button"));
		
		} // EndOfMain
		
		private static Json WrapPage<T>(string partialPath) where T : Json
		{
			var master = (MasterPage) Self.GET("/ttClient2/master");
			var nav = master.CurrentPage as NavPage;

			if (nav.CurrentPage != null && nav.CurrentPage.GetType().Equals(typeof(T)))
			{
				return master;
			}

			nav.CurrentPage = Self.GET(partialPath);

			if (nav.CurrentPage.Data == null)
			{
				nav.CurrentPage.Data = null; //trick to invoke OnData in partial
			}

			return master;
		}
	}
}