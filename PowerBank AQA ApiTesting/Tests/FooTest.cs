using Allure.Commons;
using Microsoft.Extensions.Configuration;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using PowerBank_AQA_ApiTesting.ClassData;
using PowerBank_AQA_ApiTestingCore.Models;
using PowerBank_AQA_DbTestingCore.DbClient;
using PowerBank_AQA_DbTestingCore.DbControllers;
using PowerBank_AQA_TestingCore.Configuration;
using PowerBank_AQA_ApiTesting.AllureSteps;
using PowerBank_AQA_ApiTesting.Hooks;

namespace PowerBank_AQA_ApiTesting.Tests
{
    public class Tests
    {
        [TestFixture]
        [AllureNUnit]
        [AllureSuite("Foo tests")]
        [AllureFeature("Foo tests")]
        public class Test
        {
            private WebService service;
            private IConfiguration configuration;
            private DbController dbController;
            private DbClient dbClient;
            private string baseUrl;
            private readonly string endPoint = "api/v1/user/login/mobile_phone";

            //[OneTimeSetUp]
            //public void CleanupResultDirectory()
            //{
            //    AllureExtensions.WrapSetUpTearDownParams(() => { AllureLifecycle.Instance.CleanupResultDirectory(); },
            //        "Clear Allure Results Directory");
            //}

            [SetUp]
            public void Setup()
            {
                service = new WebService();
                configuration = Configuration.GetConfiguration();
                dbController = new DbController();
                dbClient = new DbClient(configuration);
                dbClient.Create();
                dbController.Connections.TryAdd("1", dbClient);
                baseUrl = configuration["BaseUrl"].ToString() + endPoint;
            }

            [AllureName("Simple test with steps")]
            [Test, Ignore("Foo test")]
            public void CheckApiTest()
            {
                var user = new LoginUser
                {
                    MobilePhone = "79990234561",
                    PasswordEncode = "aqwQQport@43",

                };

                //var headers = RequestCreator.CreateHeaders(HeaderTypes.JSON);
                //var request = RequestCreator.CreateRequest(user, headers, baseUrl, HttpMethod.Post, int.Parse(configuration["timeOut"]));
                //var response = service.SendMessageAsync(request).Result;
                //var accessToken = response.ResponseBody.GetJsonValueByKey(".accessToken");
                //response.StatusCode.Should().Be(HttpStatusCode.OK, "Статус код отличается от ОК");

                var headers = ApiAllureSteps.CreateHeaders();
                var request = ApiAllureSteps.CreateRequest(user, headers, baseUrl, HttpMethod.Post, int.Parse(configuration["timeOut"]));
                var response = service.SendMessage(request);
                var accessToken = response.ResponseBody.GetValueByKee(".accessToken");
                response.StatusCode.CheckStatusCode(200);
            }

            [Test, Ignore("Don't need now")]
            public void CheckDbTest()
            {
                var table = dbClient.SelectQuery("SELECT * FROM public.\"UserProfiles\"");
                var row = dbClient.SelectOneRow("SELECT * FROM public.\"UserProfiles\"LIMIT 1");
                var str = dbClient.SelectOneCellAsString("SELECT \"Email\" FROM public.\"UserProfiles\" LIMIT 1");
                //dbClient.NonQuery("");
            }

            
            [Test, Ignore("Юзать по необходимости")]
            public void RecreateDb()
            {
                ApiMethods apiMethods = new ApiMethods();
                apiMethods.RecreatingDatabaseCard();
                apiMethods.RecreatingDatabaseCredit();
                apiMethods.RecreatingDatabaseUsers();
                apiMethods.RecreatingDatabaseDeposit();
                apiMethods.RecreatingDatabaseTransactionHistory();
            }

            [TearDown]
            public void TearDown()
            {
                service.Dispose();
                dbClient.Dispose();
                dbController.Connections.Clear();
            }
        }
    }
}
