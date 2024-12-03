using System;

namespace PowerBank_AQA_DbTestingCore.DbClient
{
    public interface IDbClient : IDisposable
    {
        public bool Create();
    }
}
