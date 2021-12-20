using GrpcClient;
using GrpcService.Data;

namespace GrpcClient.Data
{
    public class RequestConfigService
    {
        public static ConfigKeyRequest BuildConfigRequest(EnumCategory category, EnumDomain domain)
            => new()
            {
                Category = category,
                Domain = domain
            };
        
        public static ConfigKeyRequest BuildConfigRequest(EnumCategory category, EnumDomain domain, string key)
            => new()
            {
                Category = category,
                Key = key,
                Domain = domain
            };
        
        public static ConfigKeyRequest BuildConfigRequest(EnumCategory category, EnumDomain domain, string key, EnumEnvType env)
            => new()
            {
                Category = category,
                Domain = domain,
                Key = key,
                EnvType = env
            };

        public static ConfigKeyRequest BuildConfigRequest(EnumCategory category, EnumDomain domain, string key, EnumEnvType env,
            int envNumber)
            => new()
            {
                Category = category,
                Domain = domain,
                Key = key,
                EnvType = env,
                EnvNumber = envNumber
            };

        public static ConfigKeyRequest BuildConfigRequest(EnumCategory category, EnumDomain domain, string key, EnumEnvType env,
            EnumDevEnv devEnv)
            => BuildConfigRequest(category, domain, key, env, (int)devEnv);
        
        public static ConfigKeyRequest BuildConfigRequest(string key)
        {
            var keyParts = key.Split('/');

            var response = new ConfigKeyRequest
            {
                Category = keyParts[0].GetCategoryEnum(),
                Domain = keyParts[1].GetDomainEnum()
            };

            if (keyParts.Length >= 3)
                response.Key = keyParts[2];
            
            if(keyParts.Length >= 4)
                response.EnvType = keyParts[3].GetEnvEnum();

            if (keyParts.Length == 5)
                response.EnvNumber = response.EnvType is EnumEnvType.Dev
                    ? (int) keyParts[4].GetDevEnvEnum()
                    : int.Parse(keyParts[4]);

            return response;
        }
        
        public static ConfigKeyRequest BuildDomainConfigRequest(string key)
        {
            var keyParts = key.Split('/');

            var response = new ConfigKeyRequest
            {
                Category = keyParts[0].GetCategoryEnum(),
                Domain = keyParts[1].GetDomainEnum()
            };
            
            if(keyParts.Length >= 3)
                response.EnvType = keyParts[2].GetEnvEnum();

            if (keyParts.Length == 4)
                response.EnvNumber = response.EnvType is EnumEnvType.Dev
                    ? (int) keyParts[3].GetDevEnvEnum()
                    : int.Parse(keyParts[3]);

            return response;
        }
    }
}