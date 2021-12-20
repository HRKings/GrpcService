using System;
using GrpcService.Services;

namespace GrpcService.Data
{
    public static class Utils
    {
        public static string GetRepresetation(this EnumCategory category)
            => category switch
            {
                EnumCategory.Backend => "BE",
                EnumCategory.Frontend => "FE",
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, "Well, that is not expected.")
            };
        
        public static string GetRepresetation(this EnumDomain domain)
            => domain switch
            {
                EnumDomain.DefaultDomain => "ALL",
                EnumDomain.DOMAIN_2 => "DOMAIN_2",
                EnumDomain.DOMAIN_3 => "DOMAIN_3",
                EnumDomain.DOMAIN_1 => "DOMAIN_1",
                EnumDomain.DOMAIN_4 => "DOMAIN_4",
                _ => throw new ArgumentOutOfRangeException(nameof(domain), domain, "Well, that is not expected.")
            };
        
        public static string GetRepresetation(this EnumEnvType envType)
            => envType switch
            {
                EnumEnvType.AnyEnv => "NA",
                EnumEnvType.Dev => "DEV",
                EnumEnvType.STAGING => "STG",
                EnumEnvType.Production => "PROD",
                _ => throw new ArgumentOutOfRangeException(nameof(envType), envType, "Well, that is not expected.")
            };
        
        public static string GetRepresetation(this EnumDevEnv envType)
            => envType switch
            {
                EnumDevEnv.DEFAULT => "NA",
                EnumDevEnv.DOCKER => "DOCKER",
                EnumDevEnv.STAGING => "STG",
                _ => throw new ArgumentOutOfRangeException(nameof(envType), envType, "Well, that is not expected.")
            };

        public static string BuildKeyString(ConfigKeyRequest requestConfigKey)
            => $"{requestConfigKey.Category.GetRepresetation()}/{requestConfigKey.Domain.GetRepresetation()}";
        
        public static string BuildKeyString(ConfigService.ConfigKeyValuePair requestConfigKey)
            => $"{requestConfigKey.Category.GetRepresetation()}/{requestConfigKey.Domain.GetRepresetation()}";

        public static string BuildKeyEnvString(string keyString, ConfigKeyRequest requestConfigKey)
        {
            keyString = $"{keyString}/{requestConfigKey.EnvType.GetRepresetation()}";

            if (requestConfigKey.EnvNumber != 0)
                keyString = requestConfigKey.EnvType is EnumEnvType.Dev
                    ? $"{keyString}/{((EnumDevEnv) requestConfigKey.EnvNumber).GetRepresetation()}"
                    : $"{keyString}/{requestConfigKey.EnvNumber}";

            return keyString;
        }
        
        public static string BuildKeyEnvString(string keyString, ConfigService.ConfigKeyValuePair requestConfigKey)
        {
            keyString = $"{keyString}/{requestConfigKey.EnvType.GetRepresetation()}";

            if (requestConfigKey.EnvNumber != 0)
                keyString = requestConfigKey.EnvType is EnumEnvType.Dev
                    ? $"{keyString}/{((EnumDevEnv) requestConfigKey.EnvNumber).GetRepresetation()}"
                    : $"{keyString}/{requestConfigKey.EnvNumber}";

            return keyString;
        }
    }
}