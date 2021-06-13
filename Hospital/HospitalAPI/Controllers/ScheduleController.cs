using HospitalAPI.DTOs;
using HospitalAPI.Helpers.Enums;
using HospitalAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalAPI.Controllers
{
    [Authorize]
    public class ScheduleController : BaseApiController
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpPost]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<ActionResult> CreateSchedule([FromQuery] GenerateSheduleQuery query )
        {
            if(query.Profession == Profession.NURSE)
               await _scheduleService.GenerateScheduleForNursesAsync(query.Month);
            if (query.Profession == Profession.DOCTOR)
               await _scheduleService.GenerateScheduleForDoctorsAsync(query.Month);

            return NoContent();
        }

        [HttpDelete("All")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public ActionResult DeleteSchedules([FromQuery] int month)
        {
            _scheduleService.DeleteSchedulesForMonth(month);
            return Ok();
        }

        [HttpGet]
        public IEnumerable<ScheduleDTO> GetSchedulesForUser([FromQuery] string login)
        {
            var schedules = _scheduleService.GetSchedulesForUserAsync(login);

            return schedules.Result;
        }

        [HttpGet("GetAvailableDays")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IEnumerable<DateTime>> GetAvailableDays([FromQuery] int scheduleId, [FromQuery] string employeeLogin, [FromQuery] int month)
        {
            IEnumerable<DateTime> days = new List<DateTime>();

            if(scheduleId != 0 && string.IsNullOrEmpty(employeeLogin))
            {
                days = await _scheduleService.GetAvailableDaysForChange(scheduleId);
            }
            else if(!string.IsNullOrEmpty(employeeLogin) && month > 0 && month < 13)
            {
                days = await _scheduleService.GetAvailableDaysForNew(employeeLogin, month);
            }

            return days;
        }

        [HttpPut("New")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<ActionResult> CreateNewSchedule([FromQuery] DateTime date, [FromQuery] string employeeLogin)
        {
            if (date.Year != DateTime.Now.Year || string.IsNullOrEmpty(employeeLogin))
                return BadRequest("Invalid data from query");

            if (await _scheduleService.CreateNewSchedule(date, employeeLogin))
                return Ok();

            return BadRequest("Something went wrong");
        }

        [HttpDelete]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<ActionResult> DeleteSchedule([FromQuery] int scheduleId)
        {
            if (await _scheduleService.DeleteSchedule(scheduleId))
                return NoContent();

            return BadRequest("Something went wrong");
        }

        [HttpPut("Update")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<ActionResult> Update([FromQuery] int scheduleId, [FromQuery] DateTime date)
        {
            if(date.Year != DateTime.Now.Year)
                return BadRequest("Invalid data from query");

            if (await _scheduleService.UpdateSchedule(scheduleId, date))
                return Ok();

            return BadRequest("Something went wrong");
        }


    }
}
