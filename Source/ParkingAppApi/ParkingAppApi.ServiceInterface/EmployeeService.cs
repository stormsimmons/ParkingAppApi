using System.Collections.Generic;
using ServiceStack;
using ParkingAppApi.Domain.Interfaces;
using ParkingAppApi.ServiceModel.Employee;
using ParkingAppApi.Domain.Models;
using AutoMapper;
using ParkingAppApi.ServiceModel.Absence;
using ServiceStack.Logging;
using System;
using ParkingAppApi.Domain.Enums;

namespace ParkingAppApi.ServiceInterface
{
    public class EmployeeService : Service
    {
        private readonly IEmployeeService _employeeService;
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public EmployeeService(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
		}

		

        public  IList<EmployeeDto> Get(EmployeeRequest request)
        {
            try
            {
                IList<EmployeeDto> list =  Mapper.Map<List<EmployeeDto>>(_employeeService.GetEmployees());
                 return list;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        public EmployeeDto Get(EmployeeRequestOne request)
        {
            try
            {
                EmployeeDto response = Mapper.Map<EmployeeDto>(_employeeService.GetOne(request.Id));
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        public IList<AbsenceViewDto> Get(AbsenceViewRequest request)
        {
            try
            { 
                IList<AbsenceViewDto> absenceList = Mapper.Map<List<AbsenceViewDto>>(_employeeService.GetAvailableParkingBays((SearchDateCriteria)request.searchCriteria));
				
					return absenceList;

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        public void Post(LogAbsence request)
        {
            try
            {
                LogAbsenceDto absenceDto = new LogAbsenceDto { StartDate = request.StartDate, EndDate = request.EndDate };
                Absence response = Mapper.Map<Absence>(absenceDto);

                _employeeService.LogAbsence(response, request.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        public void Post(ReserveParkngBayRequest request)
        {
		
			try
            {
       
                AbsenceViewDto absenceViewDto = new AbsenceViewDto
                {
                    Absence = request.absenceViewDto.Absence,
                    BayOwner = request.absenceViewDto.BayOwner,
                    EmployeeId = request.absenceViewDto.EmployeeId,
                    ParkingBaynumber = request.absenceViewDto.ParkingBaynumber
                };

                AbsenceView absenceView = Mapper.Map<AbsenceView>(absenceViewDto);
                EmployeeView employeeView = Mapper.Map<EmployeeView>(request.employeeViewDto);

                _employeeService.ReserveAbsence(employeeView, absenceView.Absence, absenceView.EmployeeId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        public void Post(CancelReservedParkingBayRequest request)
        {
            try
            {
				
				AbsenceViewDto absenceViewDto = new AbsenceViewDto
                {
                    Absence = request.absenceViewDto.Absence,
                    BayOwner = request.absenceViewDto.BayOwner,
                    EmployeeId = request.absenceViewDto.EmployeeId,
                    ParkingBaynumber = request.absenceViewDto.ParkingBaynumber
                };

                AbsenceView absenceView = Mapper.Map<AbsenceView>(absenceViewDto);
                EmployeeView employeeView = Mapper.Map<EmployeeView>(request.employeeViewDto);

                _employeeService.CancelReservation(employeeView, absenceView.Absence, absenceView.EmployeeId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }
    }
}
