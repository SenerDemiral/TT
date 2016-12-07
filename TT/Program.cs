using System;
using Starcounter;
using TTDB;

namespace TT
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("MyDB");
            Application.Current.Use(new HtmlFromJsonProvider());
            //Application.Current.Use(new PartialToStandaloneHtmlProvider());

            //InitDB initDB = new InitDB();
            //initDB.Init();

            QueryResultRows<Turnuva> turnuva = Db.SQL<Turnuva>("select m from Turnuva m");
            foreach (var tr in turnuva) {
                Console.WriteLine(tr.Ad);
                QueryResultRows<TurnuvaMusabaka> ta = Db.SQL<TurnuvaMusabaka>("select m from TurnuvaMusabaka m where m.Turnuva = ?", tr);
                Console.WriteLine("  Musabaka Ozeti");
                foreach (var t in ta) {
                    Console.WriteLine(string.Format("{0,20} <{1}-{2}> {3,-20} Mac<{4}-{5}> Set<{6}-{7}> Sayi<{8}-{9}>", t.HomeTakimAd, t.Ozet.HomePuan, t.Ozet.GuestPuan, t.GuestTakimAd, t.Ozet.HomeMac, t.Ozet.GuestMac, t.Ozet.HomeSet, t.Ozet.GuestSet, t.Ozet.HomeSayi, t.Ozet.GuestSayi));

                    QueryResultRows<Mac> mm = Db.SQL<Mac>("select m from Mac m where m.TurnuvaMusabaka = ?", t);
                    foreach (var m in mm) {
                        Console.WriteLine(string.Format("{0,20} <{1}> {2,-20} Mac<{3}-{4}> Set<{5}-{6}> Sayi<{7}>", m.HomeOyuncuAd, m.Ozet.Puanlar, m.GuestOyuncuAd, m.Ozet.HomeMac, m.Ozet.GuestMac, m.Ozet.HomeSet, m.Ozet.GuestSet, m.Ozet.Sayilar));
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();

                Console.WriteLine("  Takim Sonuclari");
                QueryResultRows<TurnuvaTakim> tt = Db.SQL<TurnuvaTakim>("select m from TurnuvaTakim m where m.Turnuva = ?", tr);
                foreach (var t in tt) {
                    Console.WriteLine(string.Format("{0,20}   P:{1,-3} G:{2,-3} M:{3,-3} B:{4,-3}", t.TakimAd, t.Ozet.Puan, t.Ozet.MusabakaWin, t.Ozet.MusabakaLost, t.Ozet.MusabakaTie));
                }
                Console.WriteLine();

                Console.WriteLine("  Oyuncu Sonuclari");
                QueryResultRows<TakimOyuncu> tako = Db.SQL<TakimOyuncu>("select m from TakimOyuncu m where m.TurnuvaTakim.Turnuva = ?", tr);

                foreach (var t in tako) {
                    int oMac = 0,   // Oynadigi
                        aMac = 0,   // Aldigi
                        aSet = 0,   // Aldigi
                        vSet = 0,   // Verdigi
                        aSay = 0,
                        vSay = 0;

                    Console.WriteLine(string.Format("    {0}/{1}", t.OyuncuAd, t.TakimAd));
                    QueryResultRows<Mac> hMac = Db.SQL<Mac>("select m from Mac m where m.HomeTakimOyuncu = ?", t);
                    foreach (var m in hMac) {
                        Console.WriteLine(string.Format("    Mac<{2}-{3}> Set<{4}-{5}> Sayi<{6}-{7}> {0}/{1}", m.GuestOyuncuAd, m.GuestTakimAd, m.Ozet.HomeMac, m.Ozet.GuestMac, m.Ozet.HomeSet, m.Ozet.GuestSet, m.Ozet.HomeSayi, m.Ozet.GuestSayi));
                        oMac++;
                        aMac += m.Ozet.HomeMac;
                        aSet += m.Ozet.HomeSet;
                        vSet += m.Ozet.GuestSet;
                        aSay += m.Ozet.HomeSayi;
                        vSay += m.Ozet.GuestSayi;
                    }
                    QueryResultRows<Mac> gMac = Db.SQL<Mac>("select m from Mac m where m.GuestTakimOyuncu = ?", t);
                    foreach (var m in gMac) {
                        Console.WriteLine(string.Format("    Mac<{3}-{2}> Set<{5}-{4}> Sayi<{7}-{6}> {0}/{1}", m.HomeOyuncuAd, m.HomeTakimAd, m.Ozet.HomeMac, m.Ozet.GuestMac, m.Ozet.HomeSet, m.Ozet.GuestSet, m.Ozet.HomeSayi, m.Ozet.GuestSayi));
                        oMac++;
                        aMac += m.Ozet.GuestMac;
                        aSet += m.Ozet.GuestSet;
                        vSet += m.Ozet.HomeSet;
                        aSay += m.Ozet.GuestSayi;
                        vSay += m.Ozet.HomeSayi;
                    }
                    Console.WriteLine(string.Format("    Toplam Mac<O{0}:G{1}:M{2}> Set<{3}-{4}> Sayi<{5}-{6}>", oMac, aMac, oMac-aMac, aSet, vSet, aSay, vSay));
                    Console.WriteLine();
                }
            }

            //TTDB.TurnuvaOyuncularOzet too = new TTDB.TurnuvaOyuncularOzet("VA");
            foreach (var o in TTDB.Hlpr.TurnuvaOyuncularOzet("VA")) {
                Console.WriteLine(string.Format("{0}-{1}  Mac<{2}-{3}-{4}> Set<{5}-{6}> Sayi<{7}-{8}>", o.OyuncuAd, o.TakimAd, o.MacO, o.MacG, o.MacM, o.SetA, o.SetV, o.SayiA, o.SayiV ));
            };

            Handle.GET("/TT", () => {
                MasterJson master;

                if (Session.Current != null) {
                    master = (MasterJson)Session.Current.Data;
                } else {
                    master = new MasterJson() {
                        Html = "/TT/MasterJson.html"
                    };
                    master.Session = new Session(SessionOptions.PatchVersioning);

                    master.RecentOyuncular = new OyuncularJson() {
                        Html = "/TT/OyuncularJson.html"
                    };

                    master.RecentTurnuvalar = new TurnuvalarJson() {
                        Html = "/TT/TurnuvalarJson.html"
                    };
                }

                //((OyuncularJson)master.RecentOyuncular).RefreshData();
                master.FocusedOyuncu = null;

                //((TurnuvalarJson)master.RecentTurnuvalar).RefreshData();

                return master;
            });
        }
    }
}