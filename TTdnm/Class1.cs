using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTdnm
{
	class Class1
	{
	}

	public class Trnv {
		public ulong TrnvNo;
		public List<Msbk> Msbks;

	}

	public class Msbk
	{
		public ulong HmTkmNo;
		public ulong GsTkmNo;
		public string HmTkmAd;
		public string GsTkmAd;
		public DateTime Trh;
		public int HomePuan;
		public int GuestPuan;
	}


}
