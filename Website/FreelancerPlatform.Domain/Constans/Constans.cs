using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Domain.Constans
{
	public static class Constans
	{
		public static List<Provice> Address = new List<Provice>()
		{
			new Provice(){Id = 1, Name = "Cần Thơ"},
			new Provice(){Id = 2, Name = "TP.HCM"},
			new Provice(){Id = 3, Name = "Hà Nội"},
			new Provice(){Id = 4, Name = "Đà Nẵng"},

		};

        public static List<BankInfo> Bank = new List<BankInfo>()
        {
            new BankInfo(){Id = "VCB", Name = "Vietcombank"},
            new BankInfo(){Id = "SCB", Name = "Sacombank"},
            new BankInfo(){Id = "TCB", Name = "Techcombank"},
            new BankInfo(){Id = "AGB", Name = "Agribank"},
            new BankInfo(){Id = "MBB", Name = "Mbbank"},

        };

    }
}
