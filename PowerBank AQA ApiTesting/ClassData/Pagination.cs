using Newtonsoft.Json;
using System;

namespace PowerBank_AQA_ApiTesting.ClassData
{
    public class Pagination
    {
        [JsonProperty("currentPage", Required = Required.Always)]
        public int CurrentPage { get; set; }

        [JsonProperty("lastPage", Required = Required.Always)]
        public int LastPage { get; set; }

        [JsonProperty("perPage", Required = Required.Always)]
        public int PerPage { get; set; }

        [JsonProperty("total", Required = Required.Always)]
        public int Total { get; set; }
    }
}
