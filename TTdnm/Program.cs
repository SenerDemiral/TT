using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Starcounter;
using TTDB;

namespace TTdnm
{
	class Program
	{
		static void Main()
		{
			//Db.SQL("CREATE INDEX MacSonucMacIdx ON MacSonuc(Mac)");
			//Db.SQL("CREATE INDEX MacMsbkIdx ON Mac(Musabaka)");
			//Db.SQL("CREATE INDEX MacHomeOyuncuIdx ON Mac(HomeOyuncu)");
			//Db.SQL("CREATE INDEX MacHomeOyuncu2Idx ON Mac(HomeOyuncu2)");
			//Db.SQL("CREATE INDEX MacGuestOyuncuIdx ON Mac(GuestOyuncu)");
			//Db.SQL("CREATE INDEX MacGuestOyuncu2Idx ON Mac(GuestOyuncu2)");

			//Db.SQL("CREATE INDEX MusabakaTrnIdx ON Musabaka(Turnuva)");
			//Db.SQL("CREATE INDEX MusabakaTrnHomeTkmIdx ON Musabaka(Turnuva, HomeTakim)");
			//Db.SQL("CREATE INDEX MusabakaTrnGuestTkmIdx ON Musabaka(Turnuva, GuestTakim)");
			//Db.SQL("CREATE INDEX TurnuvaTakimTkmIdx ON TurnuvaTakim(Takim)");
			//Db.SQL("CREATE INDEX TakimOyuncuTrnIdx ON TakimOyuncu(Turnuva)");
			//Db.SQL("CREATE INDEX TakimOyuncuTkmIdx ON TakimOyuncu(Takim)");
			//Db.SQL("CREATE INDEX TakimOyuncuOynIdx ON TakimOyuncu(Oyuncu)");

			Handle.GET("/Write2File", () => {
				TTDB.RatingChart.Write2File(2005);	// 1.Lig
				return "Log.txt written";
			});

			Handle.GET("/CalcRankBaz", () => {
				// Bir kere yapilir herseyi initialize eder.
				// RankBaz hesaplandiktan sonra Rank = RankBaz + 1900 olarak update eder.
				TTDB.RatingChart.RankCalcultionBaz(DateTime.Parse("2016-10-31"), DateTime.Parse("2017-01-14"));

				return "CalcRankBaz";
			});

			Handle.GET("/CalcRank", () => {
				// Bir periyod icin yalniz bir kere yapilmali, aksi halde mukerrer olur!!!
				// Her seferinde iki tarih arasindaki (Ptesi-Cuma) Single maclara gore NOPX hesaplar ve Rank'i update eder.
				// Yeni giren oyuncu icin BazRank manuel girilir ve Rank = BazRank yapilir.

				/*
				// Burasi BazRank hesaplamasinda kullanildi
				TTDB.RatingChart.RankCalcultion(DateTime.Parse("2016-10-31"), 1);
				TTDB.RatingChart.RankCalcultion(DateTime.Parse("2016-11-07"), 2);
				TTDB.RatingChart.RankCalcultion(DateTime.Parse("2016-11-14"), 3);
				TTDB.RatingChart.RankCalcultion(DateTime.Parse("2016-11-21"), 4);
				TTDB.RatingChart.RankCalcultion(DateTime.Parse("2016-11-28"), 5);
				TTDB.RatingChart.RankCalcultion(DateTime.Parse("2016-12-05"), 6);
				TTDB.RatingChart.RankCalcultion(DateTime.Parse("2016-12-12"), 7);
				TTDB.RatingChart.RankCalcultion(DateTime.Parse("2016-12-19"), 8);
				TTDB.RatingChart.RankCalcultion(DateTime.Parse("2016-12-26"), 9);
				TTDB.RatingChart.RankCalcultion(DateTime.Parse("2017-01-02"), 10);
				TTDB.RatingChart.RankCalcultion(DateTime.Parse("2017-01-09"), 11);
				*/
				// Baz hesaplandi, sadece bir kere olacak
				// Asil Rank bundan sonra baslayacak hepsini tekrar yap, boylece Hata/Eksik tamamlanabilir!!!
				TTDB.RatingChart.InitRank();
				ulong turnuvaNO = 2005;
				TTDB.RatingChart.RankCalcultion(turnuvaNO, DateTime.Parse("2016-10-31"), 1);
				TTDB.RatingChart.RankCalcultion(turnuvaNO, DateTime.Parse("2016-11-07"), 2);
				TTDB.RatingChart.RankCalcultion(turnuvaNO, DateTime.Parse("2016-11-14"), 3);
				TTDB.RatingChart.RankCalcultion(turnuvaNO, DateTime.Parse("2016-11-21"), 4);
				TTDB.RatingChart.RankCalcultion(turnuvaNO, DateTime.Parse("2016-11-28"), 5);
				TTDB.RatingChart.RankCalcultion(turnuvaNO, DateTime.Parse("2016-12-05"), 6);
				TTDB.RatingChart.RankCalcultion(turnuvaNO, DateTime.Parse("2016-12-12"), 7);
				TTDB.RatingChart.RankCalcultion(turnuvaNO, DateTime.Parse("2016-12-19"), 8);
				TTDB.RatingChart.RankCalcultion(turnuvaNO, DateTime.Parse("2016-12-26"), 9);
				TTDB.RatingChart.RankCalcultion(turnuvaNO, DateTime.Parse("2017-01-02"), 10);
				TTDB.RatingChart.RankCalcultion(turnuvaNO, DateTime.Parse("2017-01-09"), 11);

				TTDB.RatingChart.RankCalcultion(turnuvaNO, DateTime.Parse("2017-01-16"), 12);
				TTDB.RatingChart.RankCalcultion(turnuvaNO, DateTime.Parse("2017-01-23"), 13);
				TTDB.RatingChart.RankCalcultion(turnuvaNO, DateTime.Parse("2017-01-30"), 14);
				TTDB.RatingChart.RankCalcultion(turnuvaNO, DateTime.Parse("2017-02-06"), 15);
				TTDB.RatingChart.RankCalcultion(turnuvaNO, DateTime.Parse("2017-02-13"), 16);
				TTDB.RatingChart.RankCalcultion(turnuvaNO, DateTime.Parse("2017-02-20"), 17);
				
				return "CalcRank";
			});

			Handle.GET("/CalcRankMsbk", () => {
				TTDB.RatingChart.InitRank();
				ulong turnuvaNO = 2005;
				TTDB.RatingChart.RankCalcultionMusabaka(turnuvaNO);
				
				return "CalcRankMsbk";
			});

			Handle.GET("/MsbkCache", () => {
			StringBuilder sb = new StringBuilder();
			Stopwatch watch = new Stopwatch();

			watch.Start();

			var cT = new Trnv();							 
			cT.Msbks = new List<Msbk>();
			for(int i = 0; i < 1000; i++) {
				foreach(var M in Db.SQL<Musabaka>("SELECT m FROM Musabaka m")) {
					var cM = new Msbk();

					cM.HmTkmNo = M.HomeTakim.GetObjectNo();
					cM.GsTkmNo = M.HomeTakim.GetObjectNo();
					cM.HmTkmAd = M.HomeTakim.Ad;
					cM.GsTkmAd = M.GuestTakim.Ad;

					cT.Msbks.Add(cM);

					foreach(var mac in Db.SQL<Mac>("SELECT mac FROM Mac mac WHERE mac.Musabaka = ?", M)) {
						}
					}
				}
				
				watch.Stop();
				sb.AppendLine("MsbkCacheCreate took: " + watch.Elapsed);

				watch.Start();
				for(int i = 0; i < 1; i++)
					foreach(var m in cT.Msbks) {
						var msbk = m;
					}
				watch.Stop();
				sb.AppendLine(string.Format("MsbkCache iterate took: {0} {1}", cT.Msbks.Count, watch.Elapsed));

				return sb.ToString();

			});
			
			Handle.GET("/DropIndex", () => {
				Db.SQL("DROP INDEX ChildFKParent ON Child");
			
				return "Index dropped";
			});
			

			Handle.GET("/CreateIndex", () => {
				if(Db.SQL("SELECT i FROM Starcounter.Metadata.\"Index\" i WHERE Name = ?", "ChildFKParent").First == null) {
					Db.SQL(@"CREATE INDEX ChildFKParent ON Child (Parent ASC)");
				}
				return "Index created";
			});
			
			Handle.GET("/Insert", () => {
				Db.Transact(() => {
					for(int i = 0; i < 100; i++) {
						Parent p = new Parent() {
							Name = "Parent #" + i
						};

						for(int j = 0; j < 100; j++) {
							new Child() {
								Name = "Child #" + j + " of " + p.Name,
								Parent = p
							};
						}
					}
				});

				return "Done";
			});


			Handle.GET("/Delete", () => {
				Db.Transact(() => {
					Db.SlowSQL("DELETE FROM TTDB.Child");
					Db.SlowSQL("DELETE FROM TTDB.Parent");
				});

				return "Done";
			});

			Handle.GET("/DelSimplifiedTables", () => { 
				StringBuilder sb = new StringBuilder();
				Stopwatch watch = new Stopwatch();
			
				watch.Start();

				var tbl = Db.SQL<Starcounter.Metadata.Table>("select t from Starcounter.Metadata.Table t where t.FullName LIKE ?", "Simplified%");
				foreach(var t in tbl)
				{
					sb.Append(t.FullName).AppendLine();
					Db.SQL("DROP TABLE " + t.FullName);

				}
				watch.Stop();
				sb.Append("Simplified Tables deleted: " + watch.Elapsed).Append(Environment.NewLine);

				return sb.ToString();
			});
			
			Handle.GET("/testFormID", () => { 
				StringBuilder sb = new StringBuilder();
				Stopwatch watch = new Stopwatch();
			
				watch.Start();

				for(int i = 0; i < 1000000; i++) {
					var oyncObj = DbHelper.FromID(2508);
				}

				watch.Stop();
				sb.Append("1M Object read took: " + watch.Elapsed).Append(Environment.NewLine);

				return sb.ToString();
			});

			Handle.GET("/Run/{?}", (int count) => {
				StringBuilder sb = new StringBuilder();
				Stopwatch watch = new Stopwatch();

				watch.Start();

				for(int i = 0; i < count; i++) {
					QueryResultRows<Parent> parents = Db.SQL<Parent>("SELECT p FROM TTDB.Parent p");

					foreach(Parent p in parents) {
						List<Child> children = Db.SQL<Child>("SELECT c FROM TTDB.Child c WHERE c.Parent = ?", p).ToList();
					}
				}

				watch.Stop();
				sb.Append("Compare by reference took: " + watch.Elapsed).Append(Environment.NewLine);

				watch.Reset();
				watch.Start();

				for(int i = 0; i < count; i++) {
					QueryResultRows<Parent> parents = Db.SQL<Parent>("SELECT p FROM Parent p");

					foreach(Parent p in parents) {
						List<Child> children = Db.SQL<Child>("SELECT c FROM Child c WHERE c.Parent.ObjectNo = ?", p.GetObjectNo()).ToList();
					}
				}

				watch.Stop();
				sb.Append("Compare by object no took: " + watch.Elapsed);

				return sb.ToString();
			});
		}
	}
}