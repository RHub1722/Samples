using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RTDemoProject.Service;
using RTDemoProject.Shared.DTOs;

namespace RTDemoProject.Controllers
{
    [RoutePrefix("api/Employee")]
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Array of best employees. For test send "?year=2017&month=4"   
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [Route("EmployOfTheMonth")]
        [HttpGet]
        [ResponseType(typeof(List<EmployOfTheMonthDto>))]
        public IHttpActionResult GetEmployOfTheMonth([FromUri]int year, [FromUri]int month)
        {
            List<EmployOfTheMonthDto> employOfTheMonthDtos = _employeeService.EmployOfTheMonth(new DateTime(year, month,1));
            if (employOfTheMonthDtos == null || !employOfTheMonthDtos.Any())
                return NotFound();
            return Ok(employOfTheMonthDtos);
        }

        /// <summary>
        /// Set chief for employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="chiefId"></param>
        /// <returns></returns>
        [Route("ChangeChief")]
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult ChangeChief([FromUri] int employeeId, [FromBody] int chiefId)
        {
            _employeeService.ChangeChief(employeeId,chiefId);
            return Ok();
        }

        /// <summary>
        /// Array of employee contacts
        /// </summary>
        /// <param name="id">Employee id</param>
        /// <returns></returns>
        [Route("Contacts")]
        [HttpGet]
        [ResponseType(typeof(List<ContactDto>))]
        public IHttpActionResult GetAllEmployeeContacts([FromUri]int id)
        {
            var allEmployeeContacts = _employeeService.GetAllEmployeeContacts(id);
            if (!allEmployeeContacts.Any())
                return NotFound();//or StatusCode(HttpStatusCode.NotFound);
            return Ok(allEmployeeContacts);
        }

        /// <summary>
        /// All Employees in Site
        /// </summary>
        /// <param name="name">Site name</param>
        /// <returns></returns>
        [Route("EmployeeBySite")]
        [HttpGet]
        [ResponseType(typeof(List<EmployeeDto>))]
        public IHttpActionResult GetEmployeeBySite([FromUri] string[] name)
        {
            var employeeBySite = _employeeService.GetEmployeeBySite(name);
            if (!employeeBySite.Any())
                return NotFound();
            return Ok(employeeBySite);
        }
    }
}
