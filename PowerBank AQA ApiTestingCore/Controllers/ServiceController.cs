using PowerBank_AQA_ApiTestingCore.Models;
using System.Collections.Concurrent;

namespace PowerBank_AQA_ApiTestingCore.Controllers
{
    public class ServiceController
    {
        private readonly Lazy<ConcurrentDictionary<string, ResponseInfo>> _services = new Lazy<ConcurrentDictionary<string, ResponseInfo>>();

        public ConcurrentDictionary<string, ResponseInfo> Services => _services.Value;
    }
}
