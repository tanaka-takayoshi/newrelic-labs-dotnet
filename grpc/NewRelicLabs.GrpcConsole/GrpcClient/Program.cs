using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcClient;
using NewRelic.Api.Agent;

//Grpcサービスの起動待機。本当はヘルスチェックなどで対応するのがよい。
await Task.Delay(TimeSpan.FromSeconds(10));
while(true)
{ 
    await Run();
    await Task.Delay(TimeSpan.FromMinutes(1));
}

[Transaction]
async Task Run()
{
    // The port number must match the port of the gRPC server.
    var httpHandler = new HttpClientHandler();
    // Return `true` to allow certificates that are untrusted/invalid
    httpHandler.ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

    using var channel = GrpcChannel.ForAddress("http://grpcservice", new GrpcChannelOptions { HttpHandler = httpHandler });
    var client = new Greeter.GreeterClient(channel);
    var reply = await client.SayHelloAsync(
                      new HelloRequest { Name = "GreeterClient" });
    Console.WriteLine("Greeting: " + reply.Message);
}
