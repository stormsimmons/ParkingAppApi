using ServiceStack;
using System;

namespace ParkingAppApi.ServiceModel.Absence
{
	[Route("/employee/absence", Verbs="Post")]
	public class LogAbsence
	{
		public int Id { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}
