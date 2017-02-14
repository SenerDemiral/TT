using System;
using Starcounter;

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
					<title>{0}</title>
					<script src=""/sys/webcomponentsjs/webcomponents.min.js""></script>
					<link rel=""import"" href=""/sys/polymer/polymer.html"">
					<link rel=""import"" href=""/sys/starcounter.html"">
					<link rel=""import"" href=""/sys/starcounter-include/starcounter-include.html"">
					<link rel=""import"" href=""/sys/starcounter-debug-aid/src/starcounter-debug-aid.html"">
					<link rel=""import"" href=""/sys/bootstrap.html"">
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
					<puppet-client ref=""puppet-root"" remote-url=""{1}""></puppet-client>
					<starcounter-debug-aid></starcounter-debug-aid>
				</body>
				</html>";

			Application.Current.Use(new HtmlFromJsonProvider());
			Application.Current.Use(new PartialToStandaloneHtmlProvider(html));
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
			Handle.GET("/TTclient2/master", () =>
            {
                Session session = Session.Current;
                if (session != null && session.Data != null)
                    return session.Data;

                var master = new MasterPage();

                if (session == null)
                {
                    session = new Session(Session.Flags.PatchVersioning);
                }

                var nav = new NavPage();
                master.CurrentPage = nav;

                master.Session = session;
                return master;
            });

			Handle.GET("/ttClient2", () => Self.GET("/TTclient2/master"));

			Handle.GET("/ttClient2/partial/Trnv", () => new TrnvPage());
            Handle.GET("/ttClient2/Trnv", () => WrapPage<TrnvPage>("/ttClient2/partial/Trnv"));

			
			Handle.GET("/ttClient2/partial/TrnvTkm/{?}", (string param) => new TrnvTkmPage());

			Handle.GET("/ttClient2/TrnvTkm/{?}", (string param) =>
			{
				var master = (MasterPage)Self.GET("/TTclient2/master");
				var nav = master.CurrentPage as NavPage;
				nav.CurrentPage = new TrnvTkmPage();
				(nav.CurrentPage as TrnvTkmPage).TurnuvaID = param;
				nav.CurrentPage.Data = null;
				/*
				if (!(master.CurrentPage is TrnvTkmPage)) {
					master.CurrentPage = new TrnvTkmPage();
					(master.CurrentPage as TrnvTkmPage).TrnvID = param;
					master.CurrentPage.Data = null;
                }*/
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