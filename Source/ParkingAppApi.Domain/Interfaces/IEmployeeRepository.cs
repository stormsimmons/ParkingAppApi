using ParkingAppApi.Domain.Models;
using System.Collections.Generic;

namespace ParkingAppApi.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        IList<Employee> List();
        Employee GetOne(int id);
        void Insert(Employee employee);
        void Update(Employee employee);
        void Remove(Employee employee);
    }
}
