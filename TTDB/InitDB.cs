using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starcounter;

namespace TTDB
{
    public class InitDB
    {
        public void Init()
        {
            Db.Transact(() => {
                Db.SlowSQL("DELETE FROM Turnuva");
                Db.SlowSQL("DELETE FROM Takim");
                Db.SlowSQL("DELETE FROM Oyuncu");
                Db.SlowSQL("DELETE FROM TurnuvaTakim");
                Db.SlowSQL("DELETE FROM TakimOyuncu");
                Db.SlowSQL("DELETE FROM TurnuvaMusabaka");
                Db.SlowSQL("DELETE FROM Mac");
                Db.SlowSQL("DELETE FROM MacSonuc");

                Oyuncu oyuncu1 = new Oyuncu() { Ad = "Şener", DgmYil = 1960, Sex = "E", Tel = "533-271-9797" };
                Oyuncu oyuncu2 = new Oyuncu() { Ad = "Hakan", DgmYil = 1965, Sex = "E" };
                Oyuncu oyuncu3 = new Oyuncu() { Ad = "Şevket", DgmYil = 1970, Sex = "E" };
                Oyuncu oyuncu4 = new Oyuncu() { Ad = "Erhan", DgmYil = 1970, Sex = "E" };
                Oyuncu oyuncu5 = new Oyuncu() { Ad = "Mehmet", DgmYil = 1980, Sex = "E" };
                Oyuncu oyuncu6 = new Oyuncu() { Ad = "Ali", DgmYil = 1980, Sex = "E" };
                Oyuncu oyuncu7 = new Oyuncu() { Ad = "Veli", DgmYil = 1980, Sex = "E" };
                Oyuncu oyuncu8 = new Oyuncu() { Ad = "Nihat", DgmYil = 1980, Sex = "E" };
                Oyuncu oyuncu9 = new Oyuncu() { Ad = "Tunç", DgmYil = 1980, Sex = "E" };

                Takim takim1 = new Takim() { Ad = "Promil" };
                Takim takim2 = new Takim() { Ad = "Ponpin" };
                Takim takim3 = new Takim() { Ad = "Dragon" };
                Takim takim4 = new Takim() { Ad = "mEAT United" };

                Turnuva turnuva1 = new Turnuva() { Ad = "Bodrum MT 1.Lig 2016", Trh = DateTime.Now };
                Turnuva turnuva2 = new Turnuva() { Ad = "Bodrum MT 2.Lig 2016", Trh = DateTime.Now };

                TurnuvaTakim turnuvaTakim11 = new TurnuvaTakim() { Turnuva = turnuva1, Takim = takim1 };
                TurnuvaTakim turnuvaTakim12 = new TurnuvaTakim() { Turnuva = turnuva1, Takim = takim2 };
                TurnuvaTakim turnuvaTakim13 = new TurnuvaTakim() { Turnuva = turnuva1, Takim = takim3 };
                TurnuvaTakim turnuvaTakim24 = new TurnuvaTakim() { Turnuva = turnuva2, Takim = takim4 };

                TakimOyuncu takimOyuncu111 = new TakimOyuncu() { TurnuvaTakim = turnuvaTakim11, Oyuncu = oyuncu1 };
                TakimOyuncu takimOyuncu112 = new TakimOyuncu() { TurnuvaTakim = turnuvaTakim11, Oyuncu = oyuncu2 };
                TakimOyuncu takimOyuncu113 = new TakimOyuncu() { TurnuvaTakim = turnuvaTakim11, Oyuncu = oyuncu3 };
                TakimOyuncu takimOyuncu114 = new TakimOyuncu() { TurnuvaTakim = turnuvaTakim11, Oyuncu = oyuncu4 };

                TakimOyuncu takimOyuncu125 = new TakimOyuncu() { TurnuvaTakim = turnuvaTakim12, Oyuncu = oyuncu5 };
                TakimOyuncu takimOyuncu126 = new TakimOyuncu() { TurnuvaTakim = turnuvaTakim12, Oyuncu = oyuncu6 };
                TakimOyuncu takimOyuncu127 = new TakimOyuncu() { TurnuvaTakim = turnuvaTakim12, Oyuncu = oyuncu7 };

                TakimOyuncu takimOyuncu137 = new TakimOyuncu() { TurnuvaTakim = turnuvaTakim13, Oyuncu = oyuncu8 };
                TakimOyuncu takimOyuncu138 = new TakimOyuncu() { TurnuvaTakim = turnuvaTakim13, Oyuncu = oyuncu9 };

                TakimOyuncu takimOyuncu31 = new TakimOyuncu() { TurnuvaTakim = turnuvaTakim24, Oyuncu = oyuncu1 };
                TakimOyuncu takimOyuncu32 = new TakimOyuncu() { TurnuvaTakim = turnuvaTakim24, Oyuncu = oyuncu2 };

                TurnuvaMusabaka turnuvaMusabaka11 = new TurnuvaMusabaka() { Turnuva = turnuva1, HomeTurnuvaTakim = turnuvaTakim11, GuestTurnuvaTakim = turnuvaTakim12, Trh = DateTime.Now };
                Mac mac111 = new Mac() { TurnuvaMusabaka = turnuvaMusabaka11, Sira = 1, HomeTakimOyuncu = takimOyuncu111, GuestTakimOyuncu = takimOyuncu125, Skl = "S" };
                new MacSonuc() { Mac = mac111, SetNo = 1, HomeSayi = 11, GuestSayi = 7 };
                new MacSonuc() { Mac = mac111, SetNo = 2, HomeSayi = 9, GuestSayi = 11 };
                new MacSonuc() { Mac = mac111, SetNo = 3, HomeSayi = 15, GuestSayi = 13 };
                Mac mac112 = new Mac() { TurnuvaMusabaka = turnuvaMusabaka11, Sira = 2, HomeTakimOyuncu = takimOyuncu112, GuestTakimOyuncu = takimOyuncu126, Skl = "S" };
                new MacSonuc() { Mac = mac112, SetNo = 1, HomeSayi = 6, GuestSayi = 11 };
                new MacSonuc() { Mac = mac112, SetNo = 2, HomeSayi = 9, GuestSayi = 11 };
                Mac mac113 = new Mac() { TurnuvaMusabaka = turnuvaMusabaka11, Sira = 3, HomeTakimOyuncu = takimOyuncu113, GuestTakimOyuncu = takimOyuncu127, Skl = "S" };
                new MacSonuc() { Mac = mac113, SetNo = 1, HomeSayi = 11, GuestSayi = 3 };
                new MacSonuc() { Mac = mac113, SetNo = 2, HomeSayi = 11, GuestSayi = 5 };

                TurnuvaMusabaka turnuvaMusabaka12 = new TurnuvaMusabaka() { Turnuva = turnuva1, HomeTurnuvaTakim = turnuvaTakim13, GuestTurnuvaTakim = turnuvaTakim11, Trh = DateTime.Now };
                Mac mac121 = new Mac() { TurnuvaMusabaka = turnuvaMusabaka12, Sira = 1, HomeTakimOyuncu = takimOyuncu137, GuestTakimOyuncu = takimOyuncu111, Skl = "S" };
                new MacSonuc() { Mac = mac121, SetNo = 1, HomeSayi = 11, GuestSayi = 5 };
                new MacSonuc() { Mac = mac121, SetNo = 2, HomeSayi = 11, GuestSayi = 6 };
                Mac mac122 = new Mac() { TurnuvaMusabaka = turnuvaMusabaka12, Sira = 2, HomeTakimOyuncu = takimOyuncu138, GuestTakimOyuncu = takimOyuncu112, Skl = "S" };
                new MacSonuc() { Mac = mac122, SetNo = 1, HomeSayi = 8, GuestSayi = 11 };
                new MacSonuc() { Mac = mac122, SetNo = 2, HomeSayi = 9, GuestSayi = 11 };


                Mac m = Db.SQL<Mac>("select m from TTDB.Mac m where m = ?", mac111).First;
                //Console.WriteLine(m.MacOzet.Puanlar);
                //Console.WriteLine(m.MacOzet.Setler);
                //Console.WriteLine(m.MacOzet.Sayilar);
            });
        }
    }
}
