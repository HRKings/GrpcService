using System;
using Google.Protobuf.Collections;
using Grpc.Net.Client;
using GrpcClient;
using GrpcClient.Data;
using GrpcService.Data;

using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new ConfigServer.ConfigServerClient(channel);

var configRequest = new ConfigRequest();

configRequest.ConfigKeys.AddRange(new []
{
    RequestConfigService.BuildConfigRequest("BE/ALL"),
    RequestConfigService.BuildConfigRequest(EnumCategory.Backend, EnumDomain.DefaultDomain),
    RequestConfigService.BuildConfigRequest("BE/ALL/Key1"),
    RequestConfigService.BuildConfigRequest(EnumCategory.Backend, EnumDomain.DefaultDomain, "Key1"),
    RequestConfigService.BuildConfigRequest("BE/ALL/Key1/DEV"),
    RequestConfigService.BuildConfigRequest(EnumCategory.Frontend, EnumDomain.DefaultDomain, "Key1", EnumEnvType.Dev),
    RequestConfigService.BuildConfigRequest("FE/ALL/Key1/STG/8"),
    RequestConfigService.BuildConfigRequest(EnumCategory.Backend, EnumDomain.DefaultDomain, "Key1", EnumEnvType.Dev, EnumDevEnv.DOCKER),
    RequestConfigService.BuildConfigRequest(EnumCategory.Backend, EnumDomain.DefaultDomain, "Key1", EnumEnvType.Dev, EnumDevEnv.STAGING),
});

var reply = await client.RequestConfigAsync(configRequest);

Console.WriteLine("The returned configs were:");
foreach (var (key, value) in reply.Configs)
    Console.WriteLine($"{key} => {value}");

var anyDomainRequest = RequestConfigService.BuildDomainConfigRequest("BE/DOMAIN_1");
reply = await client.RequestDomainConfigsAsync(anyDomainRequest);

Console.WriteLine("The returned domain configs were:");
foreach (var (key, value) in reply.Configs)
    Console.WriteLine($"{key} => {value}");
    
anyDomainRequest = RequestConfigService.BuildDomainConfigRequest("BE/DOMAIN_1/DEV");
reply = await client.RequestDomainConfigsAsync(anyDomainRequest);

Console.WriteLine("The returned domain configs were:");
foreach (var (key, value) in reply.Configs)
    Console.WriteLine($"{key} => {value}");
    
anyDomainRequest = RequestConfigService.BuildDomainConfigRequest("BE/DOMAIN_1/DEV/DOCKER");
reply = await client.RequestDomainConfigsAsync(anyDomainRequest);

Console.WriteLine("The returned domain configs were:");
foreach (var (key, value) in reply.Configs)
    Console.WriteLine($"{key} => {value}");