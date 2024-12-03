using System;

namespace PowerBank_AQA_ApiTestingCore.Models.Provider
{
    public interface IHttpProvider
    {
        Task<HttpResponseMessage> SendRequestAsync(RequestInfo request);
    }
}
