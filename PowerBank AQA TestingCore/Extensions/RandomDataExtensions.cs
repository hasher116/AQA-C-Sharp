using Bogus;

namespace PowerBank_AQA_TestingCore.Extensions
{
    public static class RandomDataExtensions
    {
        private const string NUMBERS = "123456789";
        private const string SPECIALCHARACTERS = "!@#$%";
        private const string RUSLETTERS = "абвгдежзийклмнопрстуфхцчшщъыьэюя";

        public static string CreateRandomString(this string baseStr, int count = 5)
        {
            return new Faker().Random.String2(count);
        }

        public static string CreateRandomStringNumbers(this string baseStr, int count = 5)
        {
            return new Faker().Random.String2(count, NUMBERS);
        }

        public static string CreateRandomStringRusLetters(this string baseStr, int count = 5)
        {
            return new Faker().Random.String2(count, RUSLETTERS);
        }

        public static string CreateRandomStringSpecialCharacters(this string baseStr, int count = 1)
        {
            return new Faker().Random.String2(count, SPECIALCHARACTERS);
        }
    }
}
