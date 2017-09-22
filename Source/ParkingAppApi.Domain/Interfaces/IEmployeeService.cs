using ParkingAppApi.Domain.Enums;
using ParkingAppApi.Domain.Models;
using System.Collections.Generic;

namespace ParkingAppApi.Domain.Interfaces
{
    public interface IEmployeeService
    {
        IList<Employee> GetEmployees();
        void LogAbsence(Absence abs, int id);
        Employee GetOne(int id);
        IList<AbsenceView> GetAvailableParkingBays(SearchDateCriteria searchDateCriteria);
		void ReserveAbsence(EmployeeView employeeView, Absence absence, int id);
        void CancelReservation(EmployeeView employeeView, Absence absence, int id);
    }
}
