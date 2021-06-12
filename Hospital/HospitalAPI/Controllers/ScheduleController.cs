using HospitalAPI.DTOs;
using HospitalAPI.Helpers.Enums;
using HospitalAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalAPI.Controllers
{

    public class ScheduleController : BaseApiController
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateSchedule([FromQuery] GenerateSheduleQuery query )
        {
            if(query.Profession == Profession.NURSE)
               await _scheduleService.GenerateScheduleForNursesAsync(query.Month);
            if (query.Profession == Profession.DOCTOR)
               await _scheduleService.GenerateScheduleForDoctorsAsync(query.Month);

            return NoContent();
        }

        [HttpDelete]
        public ActionResult DeleteSchedules([FromQuery] int month)
        {
            _scheduleService.DeleteSchedulesForMonth(month);
            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<ScheduleDTO>> GetSchedulesForUser([FromQuery] string login)
        {
            var schedules = await _scheduleService.GetSchedulesForUserAsync(login);

            return schedules;
        }
    }
}
