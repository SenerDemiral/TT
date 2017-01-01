using System;
using Starcounter;

namespace TTrest
{
	class Program
	{
		static void Main()
		{
			Console.WriteLine("TTrest");

			Handle.GET("/TTrest", () => {
				var r = new Sonuc();
				r.ID = "deneme";
				r.Hata = "Hata Yok!";
				return r;
			});
			

			#region Musabaka

			Handle.POST("/TTrest/musabaka", (Musabaka m) => {
				return Db.Transact(() => {
					var rec = new TTDB.Musabaka();

					if(!string.IsNullOrWhiteSpace(m.TurnuvaID))
						rec.Turnuva = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(m.TurnuvaID));
					else
						rec.Turnuva = Db.SQL<TTDB.Turnuva>("SELECT t FROM Turnuva t WHERE t.Ad = ?", m.TurnuvaAd).First;
					
					if(!string.IsNullOrWhiteSpace(m.HomeTakimID))
						rec.HomeTakim = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(m.HomeTakimID));
					else
						rec.HomeTakim = Db.SQL<TTDB.Takim>("SELECT t FROM Takim t WHERE t.Ad = ?", m.HomeTakimAd).First;
					
					if(!string.IsNullOrWhiteSpace(m.HomeTakimID))
						rec.GuestTakim = (TTDB.Takim)DbHelper.FromID(DbHelper.Base64DecodeObjectID(m.GuestTakimID));
					else
						rec.GuestTakim = Db.SQL<TTDB.Takim>("SELECT t FROM Takim t WHERE t.Ad = ?", m.GuestTakimAd).First;

					if(!string.IsNullOrWhiteSpace(m.Tarih))
						rec.Trh = DateTime.ParseExact(m.Tarih, "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture);
					else
						rec.Trh = DateTime.Today;

					return $"Musabaka Eklendi ID:[{rec.GetObjectID()}]";
				});
			});

			Handle.DELETE("/TTrest/musabaka/{?}", (string ID) => {
				if(string.IsNullOrWhiteSpace(ID))
					return "HATA! ID not given";

				return Db.Transact(() => {
					var rec = (TTDB.Musabaka)DbHelper.FromID(DbHelper.Base64DecodeObjectID(ID));
					if(rec != null) {
						rec.Delete();
						return $"ID:[ID] Deleted";
					}
					return $"HATA! ID:[{ID}] Musabaka bulunamadı";
				});
			});

			#endregion

			#region Oyuncu

			Handle.POST("/TTrest/oyuncu", (Oyuncu o) => {
				if(string.IsNullOrWhiteSpace(o.Ad))
					return "HATA! Ad belirtilmemiş";

				return Db.Transact(() => {
					var rec = new TTDB.Oyuncu();
					rec.Ad = o.Ad;
					rec.Sex = string.IsNullOrWhiteSpace(o.Sex) ? "E" : o.Sex;
					if(o.DgmYil > 0)
						rec.DgmYil = (short)o.DgmYil;
					if(!string.IsNullOrWhiteSpace(o.eMail))
						rec.eMail = o.eMail;
					if(!string.IsNullOrWhiteSpace(o.Tel))
						rec.Tel = o.Tel;

					return $"ID:[{rec.GetObjectID()}]";
				});
			});

			Handle.PUT("/TTrest/oyuncu/{?}", (string ID, Oyuncu o) => {
				if(string.IsNullOrWhiteSpace(ID))
					return "HATA! ID belirtilmemiş";

				return Db.Transact(() => {
					var rec = (TTDB.Oyuncu)DbHelper.FromID(DbHelper.Base64DecodeObjectID(ID));

					if(!string.IsNullOrWhiteSpace(o.Ad))
						rec.Ad = o.Ad;
					if(!string.IsNullOrWhiteSpace(o.Sex))
						rec.Sex = o.Sex;
					if(!string.IsNullOrWhiteSpace(o.eMail))
						rec.eMail = o.eMail;
					if(!string.IsNullOrWhiteSpace(o.Tel))
						rec.Tel = o.Tel;
					if(o.DgmYil > 0)
						rec.DgmYil = (short)o.DgmYil;

					return $"ID:[{rec.GetObjectID()}]";
				});
			});

			Handle.DELETE("/TTrest/oyuncu/{?}", (string ID) => {
				if(string.IsNullOrEmpty(ID))
					return "HATA! ID belirtilmemiş";

				return Db.Transact(() => {
					var rec = (TTDB.Oyuncu)DbHelper.FromID(DbHelper.Base64DecodeObjectID(ID));
					if(rec != null) {
						rec.Delete();
						return $"ID:[ID] Deleted";
					}
					return $"HATA! ID:[{ID}] Oyuncu bulunamadı";
				});
			});
			#endregion
		}
	}
}