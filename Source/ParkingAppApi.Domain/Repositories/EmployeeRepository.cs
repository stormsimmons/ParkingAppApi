using System.Collections.Generic;
using System.Linq;
using ParkingAppApi.Domain.Interfaces;
using ParkingAppApi.Domain.Models;
using ParkingAppApi.Domain.ModelDto;
using MongoDB.Driver;
using System;
using AutoMapper;

namespace ParkingAppApi.Domain.Repositories
{
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        public EmployeeRepository(string connectionString) : base(connectionString)
        {
        }

        public Employee GetOne(int id)
        {
            var collection = database.GetCollection<EmployeeModelDto>("Employee");
            var filter = Builders<EmployeeModelDto>.Filter.Eq("Id", id);
            var employeeDto = collection.Find(filter).FirstOrDefault();

            if (employeeDto == null)
            {
                return null;
            }
            var employee = MapEmployeeModelDtoToEmployee(employeeDto);

            return employee;
        }

        public void Insert(Employee employee)
        {
            var collection = database.GetCollection<EmployeeModelDto>("Employee");

            var employeeDto = MapEmployeeToEmployeeModelDto(employee);

            collection.InsertOne(employeeDto);
        }

        public void Update(Employee employee)
        {
            if (employee != null)
            {
                EmployeeModelDto employeeDto = MapEmployeeToEmployeeModelDto(employee);

                var collection = database.GetCollection<EmployeeModelDto>("Employee");
                var filter = Builders<EmployeeModelDto>.Filter.Eq("Id", employeeDto.Id);
                var update = Builders<EmployeeModelDto>.Update.Set("AbsenceList", employeeDto.AbsenceList);
                collection.UpdateOne(filter, update);
            }
        }

        public IList<Employee> List()
        {
            var collection = database.GetCollection<EmployeeModelDto>("Employee");
            var filter = Builders<EmployeeModelDto>.Filter.Empty;
            var employeeDtoList = collection.Find(filter).ToList();

            var employeeList = new List<Employee>();

            foreach (var employeeDto in employeeDtoList)
            {
                var employee = MapEmployeeModelDtoToEmployee(employeeDto);

                employeeList.Add(employee);
            }

            return employeeList.OrderBy(c => c.ParkingBay.BayNumber).ToList();
        }

        public void Remove(Employee employee)
        {
            var collection = database.GetCollection<EmployeeModelDto>("Employee");
            var filter = Builders<EmployeeModelDto>.Filter.Eq("Id", employee.Id);
            collection.DeleteOne(filter);
        }

        private Employee MapEmployeeModelDtoToEmployee(EmployeeModelDto employeeDto)
        {
            var employee = new Employee()
            {
                Id = employeeDto.Id,
                UserName=employeeDto.UserName,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Email = employeeDto.Email,
                AbsenceList = employeeDto.AbsenceList == null ? null : employeeDto.AbsenceList.Select(x => new Absence()
                {
                    StartDate = new DateTime(x.StartDate),
                    EndDate = new DateTime(x.EndDate),
                    ReservedBy = x.ReservedBy == null ? null : new EmployeeView() { Id = x.ReservedBy.Id, FirstName = x.ReservedBy.FirstName, LastName = x.ReservedBy.LastName }
                }).ToList(),
                ParkingBay =  new ParkingBay() { BayNumber = employeeDto.ParkingBay.BayNumber }
            };

            return employee;
        }

        private EmployeeModelDto MapEmployeeToEmployeeModelDto(Employee employee)
        {
            var employeeDto = new EmployeeModelDto()
            {
                Id = employee.Id,
                UserName = employee.UserName,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                AbsenceList = employee.AbsenceList == null ? null : employee.AbsenceList.Select(x => new AbsenceModelDto()
                {
                    StartDate = x.StartDate.Ticks,
                    EndDate = x.EndDate.Ticks,
                    ReservedBy = x.ReservedBy == null ? null : new EmployeeViewModelDto() { Id = x.ReservedBy.Id, FirstName = x.ReservedBy.FirstName, LastName = x.ReservedBy.LastName }
                }).ToList(),
                ParkingBay =  new ParkingBayModelDto() { BayNumber = employee.ParkingBay.BayNumber }
            };

            return employeeDto;
        }
    }
}
