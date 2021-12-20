using System;
using GrpcService.Data;

namespace GrpcClient.Data
{
    public static class ReverseMap
    {
        public static EnumCategory GetCategoryEnum(this string category)
            => category switch
            {
                "BE" => EnumCategory.Backend,
                "FE" => EnumCategory.Frontend,
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, "Well, that is not expected.")
            };

        public static EnumEnvType GetEnvEnum(this string envType)
            => envType switch
            {
                "NA" => EnumEnvType.AnyEnv,
                "DEV" => EnumEnvType.Dev,
                "STG" => EnumEnvType.STAGING,
                "PROD" => EnumEnvType.Production,
                _ => throw new ArgumentOutOfRangeException(nameof(envType), envType, "Well, that is not expected.")
            };

        public static EnumDomain GetDomainEnum(this string domain)
            => domain switch
            {
                "ALL" => EnumDomain.DefaultDomain,
                "DOMAIN_2" => EnumDomain.DOMAIN_2,
                "DOMAIN_3" => EnumDomain.DOMAIN_3,
                "DOMAIN_1" => EnumDomain.DOMAIN_1,
                "DOMAIN_4" => EnumDomain.DOMAIN_4,
                _ => throw new ArgumentOutOfRangeException(nameof(domain), domain, "Well, that is not expected.")
            };

        public static EnumDevEnv GetDevEnvEnum(this string envType)
            => envType switch
            {
                "NA" => EnumDevEnv.DEFAULT,
                "DOCKER" => EnumDevEnv.DOCKER,
                "STG" => EnumDevEnv.STAGING,
                _ => throw new ArgumentOutOfRangeException(nameof(envType), envType, "Well, that is not expected.")
            };
    }
}