using HospitalAPI.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HospitalAPI.Entities
{
    public class Employee : User
    {
        public Profession Profession { get; set; }
        public Specialization Specialization { get; set; }
        public string RtPPNumber { get; set; }

        public static List<string> GetNames()
        {
            List<string> columns = new List<string>()
            {
                nameof(Employee.FirstName).ToUpper(),
                nameof(Employee.LastName).ToUpper(),
                nameof(Employee.Login).ToUpper(),
                nameof(Employee.PersonalId).ToUpper(),
                nameof(Employee.Profession).ToUpper(),
                nameof(Employee.Specialization).ToUpper(),
                nameof(Employee.RtPPNumber).ToUpper()
            };

            return columns;
        }

        public static Dictionary<string, Expression<Func<Employee, object>>> GetDictionaryOfProperties()
        {
            var columnSelector = new Dictionary<string, Expression<Func<Employee, object>>>
            {
                { nameof(Employee.FirstName).ToUpper(), e => e.FirstName },
                { nameof(Employee.LastName).ToUpper(), e => e.LastName },
                { nameof(Employee.Login).ToUpper(), e => e.Login},
                { nameof(Employee.PersonalId).ToUpper(), e => e.PersonalId },
                { nameof(Employee.Profession).ToUpper(), e => e.Profession },
                { nameof(Employee.Specialization).ToUpper(), e => e.Specialization },
                { nameof(Employee.RtPPNumber).ToUpper(), e => e.RtPPNumber }
            };

            return columnSelector;
        }
    }
}
