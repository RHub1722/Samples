using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using RTDemoProject.Entities.POCOs;
using RTDemoProject.Shared.DTOs;

namespace RTDemoProject.AutoMaper
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();

            CreateMap<SalesOrderDetail, SalesOrderDetailDto>();
            CreateMap<SalesOrderDetailDto, SalesOrderDetail>();
            CreateMap<Contact, ContactDto>();
            CreateMap<ContactDto, Contact>();
        }
    }
}