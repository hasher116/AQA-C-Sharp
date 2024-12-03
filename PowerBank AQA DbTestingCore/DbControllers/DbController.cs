using PowerBank_AQA_DbTestingCore.DbClient;
using System.Collections.Concurrent;

namespace PowerBank_AQA_DbTestingCore.DbControllers
{
    public class DbController
    {
        private readonly Lazy<ConcurrentDictionary<string, IDbClient>> _connections = new Lazy<ConcurrentDictionary<string, IDbClient>>();

        public ConcurrentDictionary<string, IDbClient> Connections => _connections.Value;
    }
}
