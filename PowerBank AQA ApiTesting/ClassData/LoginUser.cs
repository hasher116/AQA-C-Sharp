using System;

namespace PowerBank_AQA_ApiTesting.ClassData
{
    public record LoginUser
    {
        public string MobilePhone { get; set; }
        public string PasswordEncode { get; set; }
        public string PassportNumber { get; internal set; }
    }
}
