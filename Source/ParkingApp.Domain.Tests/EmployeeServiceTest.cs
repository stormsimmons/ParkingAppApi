using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ParkingAppApi.Domain.Interfaces;
using ParkingAppApi.Domain.Services;
using System.Collections.Generic;
using ParkingAppApi.Domain.Models;
using ParkingAppApi.Domain.Enums;
using System;
using ParkingAppApi.ServiceInterface;
using ParkingAppApi.Domain.Gateways;

namespace ParkingApp.Domain.Tests
{
    [TestClass]
    public class EmployeeServiceTest
    {
        private EmployeeService _employeeService;
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private Mock<IAbsenceServiceGateway> _mockAbsenceServiceGateway;

        [TestInitialize]
        public void Initialize()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _mockAbsenceServiceGateway = new Mock<IAbsenceServiceGateway>();

            //GetEmployeeSetup
            _mockEmployeeRepository.Setup(x => x.List()).Returns(new List<Employee>() { new Employee() });

            //GetOneSetup- employee with a reservation
            _mockEmployeeRepository.Setup(x => x.GetOne(200))
                .Returns(new Employee
                {
                    Id = 200,
                    FirstName = "Jeff",
                    LastName = "Johnson",
                    Email = "cool@cool",
                    AbsenceList = new List<Absence>
                    {
                            new Absence
                            {
                                EndDate = new DateTime(2017, 7, 22),
                                StartDate = new DateTime(2017, 7, 23),
                                ReservedBy= new EmployeeView
                                {
                                      Id = 201,
                                      FirstName = "Storm",
                                      LastName = "Simmons"
                                }
                            }
                    },
                    ParkingBay = new ParkingBay()
                });

            //GetOneSetup- employee without a reservation
            _mockEmployeeRepository.Setup(x => x.GetOne(205))
                .Returns(new Employee
                {
                    Id = 205,
                    FirstName = "Jeff",
                    LastName = "Johnson",
                    Email = "cool@cool",
                    AbsenceList = new List<Absence>
                    {
                            new Absence
                            {
                                EndDate = new DateTime(2017, 7, 22),
                                StartDate = new DateTime(2017, 7, 23),
                                ReservedBy= null
                            }
                    },
                    ParkingBay = new ParkingBay()
                });

            _employeeService = new EmployeeService(_mockEmployeeRepository.Object, _mockAbsenceServiceGateway.Object);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void GetEmployees_HealthyRepository_ReturnRepositoryList()
        {
            //arrange
            var newList = new List<Employee>();
            _mockEmployeeRepository.Setup(mock => mock.List()).Returns(newList);

            //act
            IList<Employee> returnedList = _employeeService.GetEmployees();

            //assert
            Assert.AreEqual(newList, returnedList);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void GetAbsenceList_ReturnEmployeeAbsences()
        {
            //arrange 
            AbsenceServiceGateway absenceServiceGateway = new AbsenceServiceGateway(_mockEmployeeRepository.Object);

            //act
            IList<Absence> returnedAbsence = absenceServiceGateway.GetAbsenceList(200);

            //assert
            Assert.IsTrue(returnedAbsence.Count > 0);

        }

        [TestMethod, TestCategory("UnitTest")]
        public void LogAbsence_MakesCallToRepo_Update()
        {
            //arrange
            Absence absence = new Absence
            {
                EndDate = new DateTime(2017, 7, 11),
                StartDate = new DateTime(2017, 7, 19)
            };

            //act
            _employeeService.LogAbsence(absence, 200);

            //assert
            _mockEmployeeRepository.Verify(x => x.Update(It.IsAny<Employee>()), Times.Once());
        }

        [TestMethod, TestCategory("UnitTest")]
        public void ReserveAbsence_ValidAbsence_MakesCallToRepoUpdate()
        {
            //arrange
            Absence absence = new Absence
            {
                EndDate = new DateTime(2017, 7, 22),
                StartDate = new DateTime(2017, 7, 23)
            };
            EmployeeView employeeView = new EmployeeView
            {
                Id = 202,
                FirstName = "Daniel",
                LastName = "Coetzee"
            };

          

            //act
            _employeeService.ReserveAbsence(employeeView, absence, 205);

            //assert
            _mockEmployeeRepository.Verify(x => x.Update(It.IsAny<Employee>()), Times.Once);
        }


        [TestMethod, TestCategory("UnitTest")]
        public void ReserveAbsence_ValidAbsenceAlreadyReserved_DoesNotMakesCallToRepoUpdate()
        {
            //arrange
            Absence absence = new Absence
            {
                EndDate = new DateTime(2017, 7, 22),
                StartDate = new DateTime(2017, 7, 23)
            };
            EmployeeView employeeView = new EmployeeView
            {
                Id = 202,
                FirstName = "Daniel",
                LastName = "Coetzee"
            };


            //act
            _employeeService.ReserveAbsence(employeeView, absence, 200);

            //assert
            _mockEmployeeRepository.Verify(x => x.Update(It.IsAny<Employee>()), Times.Never);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void GetOne_Returns_OneEmployee()
        {
            //act
            Employee employee = _employeeService.GetOne(200);

            //assert
            Assert.IsTrue(employee != null);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void GetEmployees_Returns_ListOfEmployees()
        {
            //act
            IList<Employee> employeeList = _employeeService.GetEmployees();

            //assert
            Assert.IsTrue(employeeList.Count > 0);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void GetAbsenceList_Returns_ListOfAbsence()
        {
            //arrange
            List<Absence> absenceList = new List<Absence>();
            //act
            _mockAbsenceServiceGateway.Setup(x => x.GetAbsenceList(200)).Returns(absenceList);

            //assert
            Assert.IsTrue(absenceList != null);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void GetAvailableParkingBays_EmptyEmployeeRepository_ReturnsEmptyAbsenceViewList()
        {
            // arrange
            _mockAbsenceServiceGateway.Setup(x => x.GetAbsenceList(It.IsAny<int>())).Returns(new List<Absence>());

            //act
            var absenceViews = _employeeService.GetAvailableParkingBays(SearchDateCriteria.Today);

            //assert
            Assert.AreEqual(0, absenceViews.Count);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void GetAvailableParkingBays_EmployeeWithValidAbsence_ReturnsAbsenceViewList()
        {
            // arrange
            var absenceMock = new Mock<Absence>();
            absenceMock.Setup(x => x.IsValidFor(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);

            _mockAbsenceServiceGateway.Setup(x => x.GetAbsenceList(It.IsAny<int>())).Returns(new List<Absence> { absenceMock.Object });

            //act
            var absenceViews = _employeeService.GetAvailableParkingBays(SearchDateCriteria.Today);

            //assert
            Assert.AreEqual(1, absenceViews.Count);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void GetAvailableParkingBays_EmployeeWithValidAbsences_ReturnsAbsenceViewList()
        {
            // arrange
            var absenceMock1 = new Mock<Absence>();
            var absenceMock2 = new Mock<Absence>();
            absenceMock1.Setup(x => x.IsValidFor(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);
            absenceMock2.Setup(x => x.IsValidFor(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);

            _mockAbsenceServiceGateway.Setup(x => x.GetAbsenceList(It.IsAny<int>())).Returns(new List<Absence> { absenceMock1.Object, absenceMock2.Object });

            //act
            var absenceViews = _employeeService.GetAvailableParkingBays(SearchDateCriteria.Today);

            //assert
            Assert.AreEqual(2, absenceViews.Count);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void GetAvailableParkingBays_EmployeeWithInValidAbsence_ReturnsEmptyAbsenceViewList()
        {
            // arrange
            var absenceMock = new Mock<Absence>();
            absenceMock.Setup(x => x.IsValidFor(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(false);

            _mockAbsenceServiceGateway.Setup(x => x.GetAbsenceList(It.IsAny<int>())).Returns(new List<Absence> { absenceMock.Object });

            //act
            var absenceViews = _employeeService.GetAvailableParkingBays(SearchDateCriteria.Today);

            //assert
            Assert.AreEqual(0, absenceViews.Count);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void CancelReservation_EmployeeWithReserverationRequest_SetsParkingBayToAvailable()
        {
            //arrange

            Absence mockAbsence = new Absence
            {
                EndDate = new DateTime(2017, 7, 22),
                StartDate = new DateTime(2017, 7, 23),
            };
            EmployeeView employeeView = new EmployeeView
            {
                Id = 201,
                FirstName = "Storm",
                LastName = "Simmons"
            };
            mockAbsence.ReservedBy = employeeView;

            //act
            _employeeService.CancelReservation(employeeView, mockAbsence, 200);

            //assert
            _mockEmployeeRepository.Verify(x => x.Update(It.IsAny<Employee>()), Times.Once());
        }

        [TestMethod, TestCategory("UnitTest")]
        public void CancelReservation_ReservedByDifferentEmployee_UnableToCancelReservationOfAnotherEmployee()
        {
            //arrange
            Absence mockAbsence = new Absence
            {
                EndDate = new DateTime(2017, 7, 22),
                StartDate = new DateTime(2017, 7, 23),
                ReservedBy = new EmployeeView
                {
                    Id = 999,
                    FirstName = "Jannie",
                    LastName = "Van Der Wall"
                }
            };

            EmployeeView employeeView = new EmployeeView
            {
                Id = 201,
                FirstName = "Storm",
                LastName = "Simmons"
            };

            //act
            _employeeService.CancelReservation(employeeView, mockAbsence, 200);

            //assert
            _mockEmployeeRepository.Verify(x => x.Update(It.IsAny<Employee>()), Times.Never);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void CancelReservation_AvailableParkingBay_UnableToBeCancelledByAnyone()
        {
            //arrange
            Absence mockAbsence = new Absence
            {
                EndDate = new DateTime(2017, 7, 22),
                StartDate = new DateTime(2017, 7, 23),
                ReservedBy = null
            };

            EmployeeView employeeView = new EmployeeView
            {
                Id = 201,
                FirstName = "Storm",
                LastName = "Simmons"
            };

            //act
            _employeeService.CancelReservation(employeeView, mockAbsence, 200);

            //assert
            _mockEmployeeRepository.Verify(x => x.Update(It.IsAny<Employee>()), Times.Never);
        }
    }
}
