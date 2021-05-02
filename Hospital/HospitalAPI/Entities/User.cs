using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace HospitalAPI.Entities
{
    public abstract class User
    {
        private string _firstName;
        private string _lastName;
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName 
        {
            get { return _firstName; }
            set { _firstName = value.First().ToString().ToUpper() + value.Substring(1).ToLower(); }
        }
        public string LastName 
        {
            get { return _lastName; }
            set { _lastName = value.ToUpper(); } 
        }
        public string PersonalId { get; set; }

    }
}
