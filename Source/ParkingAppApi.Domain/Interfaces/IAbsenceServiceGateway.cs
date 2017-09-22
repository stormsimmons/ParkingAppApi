using ParkingAppApi.Domain.Models;
using System.Collections.Generic;

namespace ParkingAppApi.ServiceInterface
{
    public interface IAbsenceServiceGateway
    {
        IList<Absence> GetAbsenceList(int employeeId);
    }
}
