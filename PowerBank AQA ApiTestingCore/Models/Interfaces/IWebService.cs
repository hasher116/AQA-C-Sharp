using System;

namespace PowerBank_AQA_ApiTestingCore.Models.Interfaces
{
    public interface IWebService
    {
        Task<ResponseInfo> SendMessageAsync(RequestInfo requestInfo);
    }
}
