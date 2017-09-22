using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ParkingAppApi.Domain.Interfaces;
using System.Collections.Generic;
using ParkingAppApi.Domain.Models;
using ParkingAppApi.ServiceModel.Employee;
using ServiceStack.Testing;
using ParkingAppApi.ServiceModel.Absence;
using ParkingAppApi.Domain.Enums;
using ParkingAppApi.ServiceModel.Enum;
using System;

namespace ParkingAppApi.ServiceTest
{
	[TestClass]
	public class EmployeeServiceTest : BaseTest
	{
		private ServiceInterface.EmployeeService _employeeService;
		private Mock<IEmployeeService> _employeeServiceMock;

		[TestInitialize]
		public void Initialize()
		{
			_employeeServiceMock = new Mock<IEmployeeService>();
			AppHost.Container.Register(_employeeServiceMock.Object);
			_employeeService = AppHost.Container.Resolve<ServiceInterface.EmployeeService>();
			_employeeService.Request = new MockHttpRequest();
		}

		[TestCleanup]
		public void Cleanup()
		{
			AppHost.Dispose();
		}

		[TestMethod, TestCategory("UnitTest")]
		public void Service_GetEmployees_Returns_Employees()
		{
			//arrange
			_employeeServiceMock.Setup(x => x.GetEmployees())
				.Returns(new List<Employee>
				{
					new Employee()
					{
						Id = 1,
						FirstName = "Jeff",
						LastName = "Johnson",
						Email = "Storm@cool",
						AbsenceList = new List<Absence>(),
						ParkingBay = new ParkingBay()
					}
				});

			//act
			var response = _employeeService.Get(new EmployeeRequest());

			//assert
			Assert.AreEqual(1, response.Count);
		}

        [TestMethod, TestCategory("UnitTest")]
        [ExpectedException(typeof(Exception))]
        public void GetEmployees_When_ServiceFails_Throws_Exception()
        {
            _employeeServiceMock.Setup(x => x.GetEmployees())
            .Callback(() =>
            {
                throw new Exception();
            });
            _employeeService.Get(new EmployeeRequest());
        }
        [TestMethod, TestCategory("UnitTest")]
		public void Service_GetOneEmployees_Returns_Employees()
		{
			//arrange
			_employeeServiceMock.Setup(x => x.GetOne(1))
				.Returns(
					new Employee()
					{
						Id = 1,
						FirstName = "Jeff",
						LastName = "Johnson",
						Email = "Storm@cool",
						AbsenceList = new List<Absence>(),
						ParkingBay = new ParkingBay()
					}
				);
			//act
			var response = _employeeService.Get(new EmployeeRequestOne() {Id =1 });
			//assert
			Assert.AreEqual(1, response.Id);
		}
        [TestMethod, TestCategory("UnitTest")]
        [ExpectedException(typeof(Exception))]
        public void GetOneEmployees_When_ServiceFails_Throws_Exception()
        {
            _employeeServiceMock.Setup(x => x.GetOne(It.IsAny<int>()))
            .Callback(() =>
            {
                throw new Exception();
            });
            _employeeService.Get(new EmployeeRequestOne() { Id = 1});
        }
        [TestMethod, TestCategory("UnitTest")]
		public void Service_AddAbsence_IsCalled_Once()
		{
			//arrange
			//act
			_employeeService.Post(new LogAbsence());
			//assert
			_employeeServiceMock.Verify(x => x.LogAbsence(It.IsAny<Absence>(), It.IsAny<int>()), Times.Once);
		}
        [TestMethod,TestCategory("UnitTest")]
        [ExpectedException(typeof(Exception))]
        public void PostLogAbsence_When_ServiceFails_Throws_Exception()
        {
            _employeeServiceMock.Setup(x => x.LogAbsence(It.IsAny<Absence>(), It.IsAny<int>()))
            .Callback(() =>
            {
                throw new Exception();
            });
            _employeeService.Post(new LogAbsence());
        }

        [TestMethod, TestCategory("UnitTest")]
		public void PostReserveParkingBay_ValidAbsence_ReserveAbsencecIsCalledOnce()
		{
			//arrange
			ReserveParkngBayRequest request = new ReserveParkngBayRequest
			{
				absenceViewDto = new AbsenceViewDto(),
				employeeViewDto = new EmployeeViewDto()
			};
			//act
			_employeeService.Post(request);
			//assert
			_employeeServiceMock.Verify(x => x.ReserveAbsence(It.IsAny<EmployeeView>(), It.IsAny<Absence>(), It.IsAny<int>()), Times.Once);
		}

        [TestMethod, TestCategory("UnitTest")]
        [ExpectedException(typeof(Exception))]
        public void PostReserveParkingBay_When_ServiceFails_Throws_Exception()
        {
            _employeeServiceMock.Setup(x => x.ReserveAbsence(It.IsAny<EmployeeView>(), It.IsAny<Absence>(), It.IsAny<int>()))
            .Callback(() =>
            {
                throw new Exception();
            });
            _employeeService.Post(new ReserveParkngBayRequest() { absenceViewDto=new AbsenceViewDto(), employeeViewDto=new EmployeeViewDto()});
        }
        [TestMethod, TestCategory("UnitTest")]
        public void Service_GetAvailableParkingBays_ReturnsListOfAbsenceViewsWhenCalled()
        {
            //arrange
            AbsenceViewRequest request = new AbsenceViewRequest
            {
                searchCriteria = new SearchDateCriteriaDto()
            };
            //act
            _employeeService.Get(request);
            //assert
            _employeeServiceMock.Verify(x => x.GetAvailableParkingBays(It.IsAny<SearchDateCriteria>()), Times.Once);
        }

        [TestMethod,TestCategory("UnitTest")]
        public void Service_GetAvailableParkingBays_ReturnsAbsenceViews()
        {
            //arrange
            
            _employeeServiceMock.Setup(x => x.GetAvailableParkingBays(It.IsAny<SearchDateCriteria>()))
                .Returns(new List<AbsenceView>
                {
                    new AbsenceView()
                    {
                       EmployeeId  =1,
                       ParkingBaynumber=new ParkingBay(), 
                       Absence = new Absence(),
                       BayOwner = "Jeffy" + "Johnson"
                    }
                });

            //act
            var response = _employeeService.Get(new AbsenceViewRequest());

            //assert
            Assert.AreEqual(1, response.Count);
        }

        [TestMethod,TestCategory("UnitTest")]
        [ExpectedException(typeof(Exception))]
        public void GetAvailableParkingBays_When_ServiceFails_Throws_Exception()
        {
            _employeeServiceMock.Setup(x => x.GetAvailableParkingBays(It.IsAny<SearchDateCriteria>()))
            .Callback(() =>
            {
                throw new Exception();
            });
            _employeeService.Get(new AbsenceViewRequest());
        }

        [TestMethod, TestCategory("UnitTest")]
        public void CancelReservedParkngBayRequest_ValidAbsence_CancelReservedBayIsCalledOnce()
        {
            //arrange
            CancelReservedParkingBayRequest request = new CancelReservedParkingBayRequest
            {
                absenceViewDto = new AbsenceViewDto(),
                employeeViewDto = new EmployeeViewDto()
            };

            //act
            _employeeService.Post(request);

            //assert
            _employeeServiceMock.Verify(x => x.CancelReservation(It.IsAny<EmployeeView>(), It.IsAny<Absence>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod, TestCategory("UnitTest")]
        [ExpectedException(typeof(Exception))]
        public void CancelReservedParkngBayRequest_When_ServiceFails_Throws_Exception()
        {
            _employeeServiceMock.Setup(x => x.CancelReservation(It.IsAny<EmployeeView>(), It.IsAny<Absence>(), It.IsAny<int>()))
            .Callback(() =>
            {
                throw new Exception();
            });
            _employeeService.Post(new CancelReservedParkingBayRequest() { absenceViewDto = new AbsenceViewDto(), employeeViewDto = new EmployeeViewDto() });
        }
    }
}