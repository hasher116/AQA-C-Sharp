using PowerBank_AQA_ApiTesting.ClassData;
using System.Globalization;
using AutoMapper;
using System.Data;

namespace PowerBank_AQA_ApiTesting.Hooks
{
    public static class MyMapper
    {
        public static MapperConfiguration Balance = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DataRow, DbHelperBalance>()

        .ForMember(dest => dest.CurrentBalance, orig => orig.MapFrom(row => row["CurrentBalance"]));
        });

        public static MapperConfiguration NewUser = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DataRow, RegistrationNewUser>()

        .ForMember(dest => dest.Id, orig => orig.MapFrom(row => row["Id"]));
        });

        public static MapperConfiguration Transactions = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DataRow, DbHelperTransactions>()
          .ForMember(dest => dest.AccountNumber, orig => orig.MapFrom(row => row["AccountNumber"]))
            .ForMember(dest => dest.Amount, orig => orig.MapFrom(row => row["Amount"]));
        });
    }
}
