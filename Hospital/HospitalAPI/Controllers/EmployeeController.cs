﻿using HospitalAPI.DTOs;
using HospitalAPI.DTOs.Pagination;
using HospitalAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HospitalAPI.Controllers
{
    [Authorize]
    public class EmployeeController : BaseApiController
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("basicDetails")]
        public async Task<ActionResult<PagedList<BasicEmployeeDetailsDTO>>> GetBasicDetails([FromQuery] EmployeesPaginationQuery query)
        {
            var pagedList = await _employeeService.GetPaginatedDetails<BasicEmployeeDetailsDTO>(query, false);

            return Ok(pagedList);
        }

        [HttpGet("allDetails")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<ActionResult<PagedList<EmployeeDetailsDTO>>> GetAllDetails([FromQuery] EmployeesPaginationQuery query)
        {
            var pagedList = await _employeeService.GetPaginatedDetails<EmployeeDetailsDTO>(query, true);

            return Ok(pagedList);
        }
    }
}