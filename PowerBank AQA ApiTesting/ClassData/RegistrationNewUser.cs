using System;

namespace PowerBank_AQA_ApiTesting.ClassData
{
    public class RegistrationNewUser
    {
        public string Id { get; set; }
        public string MobilePhone { get; set; }
        public string Password { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PassportNumber { get; set; }
        public bool CountryOfResidence { get; set; }
    }
}
