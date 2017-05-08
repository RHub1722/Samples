using System;
using System.Collections.Generic;
using RTDemoProject.Shared.DTOs;

namespace RTDemoProject.Service
{
    public interface IEmployeeService
    {
        void ChangeChief(int employeeId, int chiefId);
        List<EmployOfTheMonthDto> EmployOfTheMonth(DateTime month);
        List<ContactDto> GetAllEmployeeContacts(int employeeId);
        List<EmployeeDto> GetEmployeeBySite(params string[] name);
    }
}