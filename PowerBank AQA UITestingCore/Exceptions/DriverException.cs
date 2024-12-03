namespace PowerBank_AQA_UITestingCore.Exeptions
{
    public class DriverException : Exception
    {
        public DriverException(string? message) : base(message)
        {
        }

        public DriverException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
