using System;
using System.Collections.Generic;
using ParkingAppApi.Domain.Interfaces;
using ParkingAppApi.Domain.Models;
using ParkingAppApi.ServiceInterface;
using ParkingAppApi.Domain.Enums;
using System.Linq;

namespace ParkingAppApi.Domain.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAbsenceServiceGateway _absenceServiceGateway;

        public EmployeeService(IEmployeeRepository employeeRepo, IAbsenceServiceGateway absenceServiceGateway)
        {
            _employeeRepository = employeeRepo;
            _absenceServiceGateway = absenceServiceGateway;
        }

        public IList<Employee> GetEmployees()
        {
            var employeeList = _employeeRepository.List();

            foreach (var employee in employeeList)
            {
                employee.AbsenceList = _absenceServiceGateway.GetAbsenceList(employee.Id);
            }

            return employeeList;
        }

        public Employee GetOne(int id)
        {
            Employee employee = _employeeRepository.GetOne(id);

            return employee;
        }

        public IList<AbsenceView> GetAvailableParkingBays(SearchDateCriteria searchDateCriteria)
        {
            var validAbsences = new List<AbsenceView>();
            var parkingBayOwners = GetEmployees();
            var searchPeriodEnd = DateTime.Today.AddDays((int)searchDateCriteria);

            foreach (var owner in parkingBayOwners)
            {
                foreach (var absence in owner.AbsenceList)
                {
                    if (absence.IsValidFor(DateTime.Today, searchPeriodEnd))
                    {
                        var absenceView = BuildAbsenceView(owner, absence);
                        validAbsences.Add(absenceView);
                    }
                }
            }

            return validAbsences;
        }

        //TODO: Remove when intergrating into HR system
        public void LogAbsence(Absence absence, int id)
        {
            Employee dbEmployee = _employeeRepository.GetOne(id);
            dbEmployee.AbsenceList.Add(absence);
            _employeeRepository.Update(dbEmployee);
        }

        private AbsenceView BuildAbsenceView(Employee employee, Absence absence)
        {
            AbsenceView absenceView = new AbsenceView();

            if (absence != null && employee != null)
            {
                absenceView.EmployeeId = employee.Id;
                absenceView.ParkingBaynumber = employee.ParkingBay;
                absenceView.Absence = absence;
                absenceView.BayOwner = employee.FirstName + " " + employee.LastName;
            }

            return absenceView;
        }

        public void ReserveAbsence(EmployeeView employeeView, Absence absence, int id)
        {
            Employee employee = _employeeRepository.GetOne(id);

            IList<Absence> list = employee.AbsenceList;
            if (employee != null && list != null && list.Count > 0)
            {
                Absence newAbsence = list.FirstOrDefault(c => c.StartDate == absence.StartDate && c.EndDate == absence.EndDate);

                if (newAbsence.ReservedBy != null)
                {
                    return;
                }
                else
                {
                    newAbsence.ReservedBy = employeeView;

                    _employeeRepository.Update(employee);
                }
            }
        }

        public void CancelReservation(EmployeeView employeeView, Absence absence, int id)
        {
            Employee employee = _employeeRepository.GetOne(id);

            if (absence.ReservedBy != null && employeeView.Id == absence.ReservedBy.Id)
            {
                Absence employeeAbsence = employee.AbsenceList.FirstOrDefault(c => c.StartDate == absence.StartDate && c.EndDate == absence.EndDate && c.ReservedBy.Id == employeeView.Id);

                employeeAbsence.ReservedBy = null;

                _employeeRepository.Update(employee);
            }
        }
    }

}

