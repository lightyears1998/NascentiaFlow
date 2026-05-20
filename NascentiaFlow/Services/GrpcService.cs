using DynamicData.Tests;
using Grpc.Net.Client;

namespace NascentiaFlow.Services;

public class GrpcService
{
    public void Connect()
    { 
        using var channel = GrpcChannel.ForAddress("http://localhost:5000");
    }
}