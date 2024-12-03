using System;

namespace PowerBank_AQA_TestingCore.Models
{
    public class BinDirectory : DefaultDirectory
    {
        public override string Get()
        {
            return Environment.CurrentDirectory;
        }
    }
}
