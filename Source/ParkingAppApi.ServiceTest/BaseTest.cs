using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingAppApi.ServiceTest
{
	public class BaseTest
	{
		public ServiceStackHost AppHost;

		public BaseTest()
		{
			AppHost = new AppHost();
			AppHost.Init();
		}
	}
}
