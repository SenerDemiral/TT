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