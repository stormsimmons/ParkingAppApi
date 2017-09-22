using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingAppApi.Domain.Repositories;
using System.Configuration;
using ParkingAppApi.Domain.Models;
using System.Collections.Generic;
using System;

namespace ParkingAppApi.Integration.Test
{
    [TestClass]
    public class EmployeeRepositoryTest
    {
        private EmployeeRepository _employeeRepository;
        Employee _employee = new Employee
        {
            Id = 500,
            UserName= "TestName.TestLastName",
            FirstName = "TestName",
            LastName = "TestLastName",
            Email = "cool@cool.com",
            AbsenceList = new List<Absence>() { new Absence() { StartDate = new DateTime(2017, 7, 22), EndDate = new DateTime(2017, 7, 26), ReservedBy = null } },
            ParkingBay = new ParkingBay() { BayNumber = 22 }
        };

        [TestInitialize]
        public void Initialize()
        {
            _employeeRepository = new EmployeeRepository(ConfigurationManager.ConnectionStrings["MongoDatabaseConnection"].ConnectionString);
            _employeeRepository.Insert(_employee);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _employeeRepository.Remove(_employee);
        }

        [TestMethod, TestCategory("Integration")]
        public void List_Should_Return_ListOfEmployees()
        {
            //act
            IList<Employee> result = _employeeRepository.List();

            //assert
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod, TestCategory("Intergration")]
        public void List_OrderByBayId_Descending()
        {
            //arrange
            IList<Employee> employeeList = _employeeRepository.List();
            Employee employee = new Employee();
            Employee employee1 = employeeList[1];
            Employee employee2 = employeeList[2];
            Employee employee3 = employeeList[3];
            //assert
            Assert.AreEqual(employee1.ParkingBay.BayNumber, 50);
            Assert.AreEqual(employee2.ParkingBay.BayNumber, 51);
            Assert.AreEqual(employee3.ParkingBay.BayNumber, 52);
        }

        [TestMethod, TestCategory("Integration")]
        public void GetOne_Should_Return_OneEmployee()
        {
            //act
            Employee testEmployee = _employeeRepository.GetOne(_employee.Id);

            //assert
            Assert.IsTrue(testEmployee != null);
        }

        [TestMethod, TestCategory("Integration")]
        public void Update_UpdatesEmployee_AddsNewAbsence()
        {
            //arrange
            Absence absence = new Absence
            {
                EndDate = new DateTime(2017, 7, 22),
                StartDate = new DateTime(2017, 7, 21)
            };

            //act
            _employeeRepository.Update(_employee);
            _employee.AbsenceList.Add(absence);

            //assert
            Assert.IsTrue(_employee.AbsenceList.Contains(absence));
        }

        [TestMethod, TestCategory("Integration")]
        public void Remove_RemovesEmployee()
        {
            //arrange
            Employee testEmployee = _employeeRepository.GetOne(_employee.Id);

            //act
            _employeeRepository.Remove(testEmployee);

            //assert
            Assert.IsTrue(_employeeRepository.GetOne(_employee.Id) == null);
        }
    }
}
