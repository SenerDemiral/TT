using System;
using Starcounter;
//using System.Collections;
using System.Collections.Generic;

namespace TTDB
{
    public class OyuncuOzet
    {
        public int oMac;
        public int aMac;
        public int aSet;
        public int vSet;
        public int aSayi;
        public int vSayi;

        public OyuncuOzet()
        {
            oMac = 0;
            aMac = 0;
            aSet = 0;
            vSet = 0;
            aSayi = 0;
            vSayi = 0;
        }
    }

    [Database]
    public class Oyuncu
    {
        public string Ad;
        public string Sex;
        public string Tel;
        public Int16 DgmYil;
        public OyuncuOzet Ozet {
            get {
                OyuncuOzet ozet = new OyuncuOzet();

                QueryResultRows<TakimOyuncu> TO = Db.SQL<TakimOyuncu>("select m from TakimOyuncu m where m.Oyuncu = ?", this);
                foreach (var to in TO) {
                    QueryResultRows<Mac> hMac = Db.SQL<Mac>("select m from Mac m where m.HomeTakimOyuncu = ?", to);
                    foreach (var m in hMac) {
                        //Console.WriteLine(string.Format("    Mac<{2}-{3}> Set<{4}-{5}> Sayi<{6}-{7}> {0}/{1}", m.GuestOyuncuAd, m.GuestTakimAd, m.Ozet.HomeMac, m.Ozet.GuestMac, m.Ozet.HomeSet, m.Ozet.GuestSet, m.Ozet.HomeSayi, m.Ozet.GuestSayi));
                        ozet.oMac++;
                        ozet.aMac += m.Ozet.HomeMac;
                        ozet.aSet += m.Ozet.HomeSet;
                        ozet.vSet += m.Ozet.GuestSet;
                        ozet.aSayi += m.Ozet.HomeSayi;
                        ozet.vSayi += m.Ozet.GuestSayi;
                    }
                    QueryResultRows<Mac> gMac = Db.SQL<Mac>("select m from Mac m where m.GuestTakimOyuncu = ?", to);
                    foreach (var m in gMac) {
                        //Console.WriteLine(string.Format("    Mac<{3}-{2}> Set<{5}-{4}> Sayi<{7}-{6}> {0}/{1}", m.HomeOyuncuAd, m.HomeTakimAd, m.Ozet.HomeMac, m.Ozet.GuestMac, m.Ozet.HomeSet, m.Ozet.GuestSet, m.Ozet.HomeSayi, m.Ozet.GuestSayi));
                        ozet.oMac++;
                        ozet.aMac += m.Ozet.GuestMac;
                        ozet.aSet += m.Ozet.GuestSet;
                        ozet.vSet += m.Ozet.HomeSet;
                        ozet.aSayi += m.Ozet.GuestSayi;
                        ozet.vSayi += m.Ozet.HomeSayi;
                    }
                }

                return ozet;
            }
        }
    }

    [Database]
    public class Takim
    {
        public string Ad;
        public string Tel;
        public Int16 KurYil;
    }

    [Database]
    public class Turnuva
    {
        public string Ad;
        public DateTime Trh;
    }

    public class TurnuvaTakimOzet
    {
        public Int16 Puan;
        public Int16 MusabakaWin;
        public Int16 MusabakaLost;
        public Int16 MusabakaTie;

        public TurnuvaTakimOzet()
        {
            Puan = 0;
            MusabakaWin = 0;
            MusabakaLost = 0;
            MusabakaTie = 0;
        }
    }

    [Database]
    public class TurnuvaTakim
    {
        public Turnuva Turnuva;
        public Takim Takim;
        public string TakimAd => Takim.Ad;
        public string TurnuvaAd => Turnuva.Ad;

        public TurnuvaTakimOzet Ozet {
            get {
                TurnuvaTakimOzet ozet = new TurnuvaTakimOzet();
                // Evinde oynadiklari
                QueryResultRows<TurnuvaMusabaka> hta = Db.SQL<TurnuvaMusabaka>("SELECT ta FROM TTDB.TurnuvaMusabaka ta WHERE ta.HomeTurnuvaTakim = ?", this);
                foreach (var ta in hta) {
                    ozet.Puan += ta.Ozet.HomePuan;
                    if (ta.Ozet.HomePuan > ta.Ozet.GuestPuan)
                        ozet.MusabakaWin++;
                    else if (ta.Ozet.HomePuan < ta.Ozet.GuestPuan)
                        ozet.MusabakaLost++;
                    else
                        ozet.MusabakaTie++;
                }
                // Misafir oynadiklari
                QueryResultRows<TurnuvaMusabaka> gta = Db.SQL<TurnuvaMusabaka>("SELECT ta FROM TTDB.TurnuvaMusabaka ta WHERE ta.GuestTurnuvaTakim = ?", this);
                foreach (var ta in gta) {
                    ozet.Puan += ta.Ozet.GuestPuan;
                    if (ta.Ozet.GuestPuan > ta.Ozet.HomePuan)
                        ozet.MusabakaWin++;
                    else if (ta.Ozet.GuestPuan < ta.Ozet.HomePuan)
                        ozet.MusabakaLost++;
                    else
                        ozet.MusabakaTie++;
                }
                return ozet;
            }
        }
    }

    [Database]
    public class TakimOyuncu
    {
        public TurnuvaTakim TurnuvaTakim;
        public Oyuncu Oyuncu;
        public string OyuncuAd => Oyuncu.Ad;
        public string TakimAd => TurnuvaTakim.Takim.Ad;
        public string TurnuvaAd => TurnuvaTakim.Turnuva.Ad;
    }

    [Database]
    public class TurnuvaMusabaka
    {
        public Turnuva Turnuva;
        public TurnuvaTakim HomeTurnuvaTakim;
        public TurnuvaTakim GuestTurnuvaTakim;
        public DateTime Trh;
        public string Yeri;
        public string TurnuvaAd => Turnuva.Ad;
        public string HomeTakimAd => HomeTurnuvaTakim.Takim.Ad;
        public string GuestTakimAd => GuestTurnuvaTakim.Takim.Ad;

        public string Tarih {
            get { return string.Format("{0:dd.MM.yy}", Trh); }
        }
        public Ozet Ozet {
            get {
                Ozet ozet = new Ozet();
                QueryResultRows<Mac> ms = Db.SQL<Mac>("SELECT ms FROM TTDB.Mac ms WHERE ms.TurnuvaMusabaka = ?", this);
                foreach (var m in ms) {
                    ozet.HomePuan += m.Ozet.HomePuan;
                    ozet.HomeMac += m.Ozet.HomeMac;
                    ozet.HomeSet += m.Ozet.HomeSet;
                    ozet.HomeSayi += m.Ozet.HomeSayi;

                    ozet.GuestPuan += m.Ozet.GuestPuan;
                    ozet.GuestMac += m.Ozet.GuestMac;
                    ozet.GuestSet += m.Ozet.GuestSet;
                    ozet.GuestSayi += m.Ozet.GuestSayi;
                }

                return ozet;
            }
        }
    }

    public class Ozet {
        public Int16 HomePuan;
        public Int16 GuestPuan;
        public Int16 HomeMac;
        public Int16 HomeSet;
        public Int16 GuestMac;
        public Int16 GuestSet;

        public Int16 HomeSayi;
        public Int16 GuestSayi;
        public string Puanlar;   // H:G
        public string Setler;   // H:G
        public string Sayilar;  // H:G, H:G,...

        public Ozet()
        {
            HomePuan = 0;
            HomeMac = 0;
            HomeSet = 0;
            GuestPuan = 0;
            GuestMac = 0;
            GuestSet = 0;

            Puanlar = "";
            Setler = "";
            Sayilar = "";
        }
    }

    [Database]
    public partial class Mac
    {
        public TurnuvaMusabaka TurnuvaMusabaka;
        public string Skl;  // Single/Double/MixDouble
        public Int16 Sira;
        public TakimOyuncu HomeTakimOyuncu;
        public TakimOyuncu HomeTakimOyuncu2;
        public TakimOyuncu GuestTakimOyuncu;
        public TakimOyuncu GuestTakimOyuncu2;
        public string HomeOyuncuAd {
            get {
                string info = HomeTakimOyuncu.Oyuncu.Ad;
                if (HomeTakimOyuncu2 != null)
                    info += " + " + HomeTakimOyuncu2.Oyuncu.Ad;
                return info;
            }
        }
        public string HomeTakimAd => TurnuvaMusabaka.HomeTakimAd;

        public string GuestOyuncuAd {
            get {
                string info = GuestTakimOyuncu.Oyuncu.Ad;
                if (GuestTakimOyuncu2 != null)
                    info += " + " + GuestTakimOyuncu2.Oyuncu.Ad;
                return info;
            }
        }
        public string GuestTakimAd => TurnuvaMusabaka.GuestTakimAd;
        public string TurnuvaAd => TurnuvaMusabaka.TurnuvaAd;

        public Ozet Ozet {
            get {
                Ozet ozet = new Ozet();
                string sayilar = "";
                Int16 hSet = 0;
                Int16 gSet = 0;

                QueryResultRows<MacSonuc> ms = Db.SQL<MacSonuc>("SELECT ms FROM TTDB.MacSonuc ms WHERE ms.Mac = ?", this);
                foreach (var m in ms) {
                    ozet.HomeSayi += m.HomeSayi;
                    ozet.GuestSayi += m.GuestSayi;

                    sayilar += string.Format("{0}-{1} ", m.HomeSayi.ToString().PadLeft(2, '0'), m.GuestSayi.ToString().PadLeft(2, '0'));
                    if (m.HomeSayi > m.GuestSayi)
                        hSet++;
                    else
                        gSet++;
                }

                if (hSet > gSet) {
                    ozet.HomeMac = 1;
                    ozet.HomePuan = 2;
                } else if (hSet < gSet) {
                    ozet.GuestMac = 1;
                    ozet.GuestPuan = 2;
                }
                ozet.HomeSet = hSet;
                ozet.GuestSet = gSet;

                ozet.Puanlar = string.Format("{0}-{1}", ozet.HomePuan, ozet.GuestPuan);
                ozet.Setler = string.Format("{0}-{1} ", hSet, gSet);
                ozet.Sayilar = sayilar.TrimEnd();

                return ozet;
            }
        }
    }


    [Database]
    public class MacSonuc
    {
        public Mac Mac;
        public Int16 SetNo;
        public Int16 HomeSayi;
        public Int16 GuestSayi;
        public string HomeOyuncuAd => Mac.HomeOyuncuAd;
        public string GuestOyuncuAd => Mac.GuestOyuncuAd;
        public string TurnuvaAd => Mac.TurnuvaAd;

    }

    public partial class Mac
    {/*
        static public Ozet GetOzet(Mac macObj)
        {
            Ozet ozet = new Ozet();
            string sayilar = "";
            int hSet = 0;
            int gSet = 0;
            int tSet = 0;

            QueryResultRows<MacSonuc> ms = Db.SQL<MacSonuc>("SELECT ms FROM TTDB.MacSonuc ms WHERE ms.Mac = ?", macObj);
            foreach (var m in ms) {
                ozet.HomeSayi += m.HomeSayi;
                ozet.GuestSayi += m.GuestSayi;

                sayilar += string.Format("{0}:{1} ", m.HomeSayi, m.GuestSayi);
                if (m.HomeSayi > m.GuestSayi)
                    hSet++;
                else
                    gSet++;
            }
            tSet = hSet + gSet; // Toplam Set sayisi

            if (hSet > gSet) {
                ozet.HomePuan = 2;
                ozet.HomeSetWin = (Int16)hSet;
                ozet.HomeSetLost = (Int16)(tSet - hSet);
            } else if (hSet < gSet) {
                ozet.GuestPuan = 2;
                ozet.GuestSetWin = (Int16)gSet;
                ozet.GuestSetLost = (Int16)(tSet - gSet);
            }

            ozet.Puanlar = string.Format("{0}-{1}", ozet.HomePuan, ozet.GuestPuan);
            ozet.Setler = string.Format("{0}:{1} ", hSet, gSet);
            ozet.Sayilar = sayilar;

            return ozet;
        }*/
    }

    public class TurnuvaOyuncuOzet
    {
        public string OyuncuAd;
        public string TakimAd;
        public Int16 Puan;
        public Int16 MacO;
        public Int16 MacG;
        public Int16 MacM;
        public Int16 SetA;
        public Int16 SetV;
        public Int16 SayiA;
        public Int16 SayiV;

        public TurnuvaOyuncuOzet()
        {
            OyuncuAd = "";
            TakimAd = "";
            Puan = 0;
            MacO = 0;
            MacG = 0;
            MacM = 0;
            SetA = 0;
            SetV = 0;
            SayiA = 0;
            SayiV = 0;
        }
    }
    /*
    public class TurnuvaOyuncularOzet
    {
        IObjectView turnuva;

        public TurnuvaOyuncularOzet(string turnuvaID)
        {
            turnuva = DbHelper.FromID(DbHelper.Base64DecodeObjectID(turnuvaID));
        }

        //public IEnumerable<TurnuvaOyuncuOzet> GetEnumerator()
        public IEnumerator<TurnuvaOyuncuOzet> GetEnumerator()
        {
            //List<TurnuvaOyuncuOzet> ltoo = new List<TurnuvaOyuncuOzet>();
            QueryResultRows<TakimOyuncu> to = Db.SQL<TakimOyuncu>("select m from TakimOyuncu m where m.TurnuvaTakim.Turnuva = ?", turnuva);

            foreach (var t in to) {
                TurnuvaOyuncuOzet too = new TurnuvaOyuncuOzet();

                int oMac = 0,   // Oynadigi
                    aMac = 0,   // Aldigi
                    aSet = 0,   // Aldigi
                    vSet = 0,   // Verdigi
                    aSay = 0,
                    vSay = 0;

                //Console.WriteLine(string.Format("    {0}/{1}", t.OyuncuAd, t.TakimAd));
                too.OyuncuAd = t.OyuncuAd;
                too.TakimAd = t.TakimAd;

                // Home olarak oynadiklari
                QueryResultRows<Mac> hMac = Db.SQL<Mac>("select m from Mac m where m.HomeTakimOyuncu = ?", t);

                foreach (var m in hMac) {
                    oMac++;
                    aMac += m.Ozet.HomeMac;
                    aSet += m.Ozet.HomeSet;
                    vSet += m.Ozet.GuestSet;
                    aSay += m.Ozet.HomeSayi;
                    vSay += m.Ozet.GuestSayi;
                }
                // Guest olarak oynadiklar
                QueryResultRows<Mac> gMac = Db.SQL<Mac>("select m from Mac m where m.GuestTakimOyuncu = ?", t);

                foreach (var m in gMac) {
                    oMac++;
                    aMac += m.Ozet.GuestMac;
                    aSet += m.Ozet.GuestSet;
                    vSet += m.Ozet.HomeSet;
                    aSay += m.Ozet.GuestSayi;
                    vSay += m.Ozet.HomeSayi;
                }

                too.MacO = (Int16)oMac;
                too.MacG = (Int16)aMac;
                too.MacM = (Int16)(oMac - aMac);
                too.SetA = (Int16)aSet;
                too.SetV = (Int16)vSet;
                too.SayiA = (Int16)aSay;
                too.SayiV = (Int16)vSay;

                //ltoo.Add(too);
                yield return too;
            }
            //return ltoo;
        }
    }
    */
    public static class Hlpr
    {
        public static void sener()
        {

            foreach (var value in TurnuvaOyuncularOzet("VC")) {
                Console.Write(value);
                Console.Write(" ");
            }
            Console.WriteLine();
        }

        public static IEnumerable<TurnuvaOyuncuOzet> TurnuvaOyuncularOzet(string turnuvaID)
        {
            var turnuva = DbHelper.FromID(DbHelper.Base64DecodeObjectID(turnuvaID));

            QueryResultRows<TakimOyuncu> to = Db.SQL<TakimOyuncu>("select m from TakimOyuncu m where m.TurnuvaTakim.Turnuva = ?", turnuva);

            foreach (var t in to) {
                TurnuvaOyuncuOzet too = new TurnuvaOyuncuOzet();

                int oMac = 0,   // Oynadigi
                    aMac = 0,   // Aldigi
                    aSet = 0,   // Aldigi
                    vSet = 0,   // Verdigi
                    aSay = 0,
                    vSay = 0;

                //Console.WriteLine(string.Format("    {0}/{1}", t.OyuncuAd, t.TakimAd));
                too.OyuncuAd = t.OyuncuAd;
                too.TakimAd = t.TakimAd;

                // Home olarak oynadiklari
                QueryResultRows<Mac> hMac = Db.SQL<Mac>("select m from Mac m where m.HomeTakimOyuncu = ?", t);

                foreach (var m in hMac) {
                    oMac++;
                    aMac += m.Ozet.HomeMac;
                    aSet += m.Ozet.HomeSet;
                    vSet += m.Ozet.GuestSet;
                    aSay += m.Ozet.HomeSayi;
                    vSay += m.Ozet.GuestSayi;
                }
                // Guest olarak oynadiklar
                QueryResultRows<Mac> gMac = Db.SQL<Mac>("select m from Mac m where m.GuestTakimOyuncu = ?", t);

                foreach (var m in gMac) {
                    oMac++;
                    aMac += m.Ozet.GuestMac;
                    aSet += m.Ozet.GuestSet;
                    vSet += m.Ozet.HomeSet;
                    aSay += m.Ozet.GuestSayi;
                    vSay += m.Ozet.HomeSayi;
                }

                too.Puan = (Int16)(aMac * 2 + (oMac - aMac));
                too.MacO = (Int16)oMac;
                too.MacG = (Int16)aMac;
                too.MacM = (Int16)(oMac - aMac);
                too.SetA = (Int16)aSet;
                too.SetV = (Int16)vSet;
                too.SayiA = (Int16)aSay;
                too.SayiV = (Int16)vSay;

                //ltoo.Add(too);
                yield return too;
            }
        }
    }
}