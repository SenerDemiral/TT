
using System;
using Starcounter;

namespace TableTennisDB
{
    [Database]
    public class Turnuva
    {
        public string Ad;
        public DateTime Trh;
    }

    [Database]
    public class Takim
    {
        public Turnuva Turnuva;
        public string Ad;
    }

    [Database]
    public class Oyuncu
    {
        public Takim Takim;
        public string Ad;
        public string Sex;
        public string Tel;
        public Int16 DgmYil;
    }

    [Database]
    public class Mac
    {
        public Turnuva Turnuva;
        public Takim HomeTakim;
        public Takim GuestTakim;
        public DateTime Trh;
        public string Yeri;
    }

    [Database]
    public class Game
    {
        public Mac Mac;
        public Int16 Sira;
        public Oyuncu HomeOyuncu;
        public Oyuncu GuestOyuncu;
    }

    [Database]
    public class Sonuc
    {
        public Game Game;
        public Int16 SetNo;
        public Int16 HomeSayi;
        public Int16 GuestSayi;
    }
}