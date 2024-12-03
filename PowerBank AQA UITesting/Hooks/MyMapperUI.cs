using AutoMapper;
using PowerBank_AQA_UITesting.Dto;
using System.Data;

namespace PowerBank_AQA_UITesting.Hooks
{
    public static class MyMapperUI
    {
        public static MapperConfiguration LoansInformation = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DataRow, LoanDetailedInformation>()
          .ForMember(dest => dest.Name, orig => orig.MapFrom(row => row["Name"]))
            .ForMember(dest => dest.IsGuarantee, orig => orig.MapFrom(row => row["IsGuarantee"]))
                        .ForMember(dest => dest.InterestRate, orig => orig.MapFrom(row => row["InterestRate"]))

                        .ForMember(dest => dest.IsRevocable, orig => orig.MapFrom(row => row["IsRevocable"]))

                        .ForMember(dest => dest.AmountMin, orig => orig.MapFrom(row => row["AmountMin"]))
                            .ForMember(dest => dest.MaxDurationMonths, orig => orig.MapFrom(row => row["MaxDurationMonths"]))

                        .ForMember(dest => dest.MinDurationMonths, orig => orig.MapFrom(row => row["MinDurationMonths"]))
                        .ForMember(dest => dest.AmountMax, orig => orig.MapFrom(row => row["AmountMax"]));

        });

    }
}
