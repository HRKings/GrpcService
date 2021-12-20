using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Google.Protobuf.Collections;
using Grpc.Core;
using GrpcService.Data;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace GrpcService.Services
{
    public class ConfigService : ConfigServer.ConfigServerBase
    {
        private const string QUERY_WITH_FALLBACK = @"
SELECT key as Key,
        value as Value,
        category as Category,
        domain as Domain,
        env as EnvType,
        env_number as EnvNumber
    FROM config_service
WHERE category = @requestedCategory AND deletion_time IS NULL
    AND key = @requestedKey
    AND (domain = @requestedDomain OR env = 0)
    AND (env = @requestedEnv OR env = 0)
    AND (env_number = @requestedEnvNumber OR env_number = 0)
ORDER BY domain DESC, env_number DESC, env DESC
LIMIT 1";
        
        private const string DOMAIN_QUERY_WITH_FALLBACK = @"
SELECT key as Key,
        value as Value,
        category as Category,
        domain as Domain,
        env as EnvType,
        env_number as EnvNumber
    FROM config_service
WHERE category = @requestedCategory  AND deletion_time IS NULL
    AND (domain = @requestedDomain OR env = 0)
    AND (env = @requestedEnv OR env = 0)
    AND (env_number = @requestedEnvNumber OR env_number = 0)
ORDER BY domain ASC, env_number ASC, env ASC";
        
        private readonly ILogger<ConfigService> _logger;
        private readonly NpgsqlConnection _sqlConnection;

        public ConfigService(ILogger<ConfigService> logger)
        {
            _logger = logger;
            _sqlConnection = new NpgsqlConnection("Server=127.0.0.1;Port=5432;Database=postgres;User Id=postgres;Password=password;");
        }

        public record ConfigKeyValuePair(string Key, string Value, EnumCategory Category, EnumDomain Domain, EnumEnvType EnvType, int EnvNumber);

        public override Task<ConfigResponse> RequestDomainConfigs(ConfigKeyRequest request, ServerCallContext context)
        {
            var response = new ConfigResponse();
            
            var keyString = Utils.BuildKeyString(request);

            if (request.EnvType is not EnumEnvType.AnyEnv)
                keyString = Utils.BuildKeyEnvString(keyString, request);

            _logger.Log(LogLevel.Information, $"The requested config keys was: {keyString}");
            
            var keyValuePairs = _sqlConnection.Query<ConfigKeyValuePair>(DOMAIN_QUERY_WITH_FALLBACK, new
            {
                requestedCategory = (int)request.Category,
                requestedDomain = request.Domain,
                requestedEnv = (int)request.EnvType,
                requestedEnvNumber = request.EnvNumber
            }).ToList();

            if (keyValuePairs.Count == 0)
                return null;

            foreach (var keyValuePair in keyValuePairs)
            {
                if(keyValuePair is null)
                    continue;

                if (response.Configs.ContainsKey(keyValuePair.Key))
                {
                    response.Configs[keyValuePair.Key] = keyValuePair.Value;
                    continue;
                }

                response.Configs.Add(keyValuePair.Key, keyValuePair.Value);
            }
            
            return Task.FromResult(response);
        }

        public override Task<ConfigResponse> RequestConfig(ConfigRequest request, ServerCallContext context)
        {
            var response = new ConfigResponse();
            _logger.Log(LogLevel.Debug, "The requested config keys was:");
            foreach (var requestConfigKey in request.ConfigKeys)
            {
                var keyString = Utils.BuildKeyString(requestConfigKey);

                if (requestConfigKey.EnvType is not EnumEnvType.AnyEnv)
                    keyString = Utils.BuildKeyEnvString(keyString, requestConfigKey);

                if(response.Configs.ContainsKey(keyString))
                    continue;

                _logger.Log(LogLevel.Information, keyString);
                
                var keyValuePair = _sqlConnection.QueryFirstOrDefault<ConfigKeyValuePair>(QUERY_WITH_FALLBACK, new
                {
                    requestedCategory = (int)requestConfigKey.Category,
                    requestedKey = requestConfigKey.Key,
                    requestedDomain = requestConfigKey.Domain,
                    requestedEnv = (int)requestConfigKey.EnvType,
                    requestedEnvNumber = requestConfigKey.EnvNumber
                });
                
                if(keyValuePair is null)
                    continue;
                
                keyString = Utils.BuildKeyString(keyValuePair);

                if (requestConfigKey.EnvType is not EnumEnvType.AnyEnv)
                    keyString = Utils.BuildKeyEnvString(keyString, keyValuePair);
                
                if(response.Configs.ContainsKey(keyString))
                    continue;

                response.Configs.Add(keyString, keyValuePair.Value);
            }
            
            return Task.FromResult(response);
        }
    }
}