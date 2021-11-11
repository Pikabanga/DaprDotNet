using Dapr.Client;
using DaprDotNet.Service.Models;

public static class Program
{
    public static async Task Main(string[] args)
   {
        Console.WriteLine("Hello, World!");

        var cts = new CancellationTokenSource();

        Console.CancelKeyPress += (s, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        var cancellationToken = cts.Token;

        var endpoint = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT");

        using var client = new DaprClientBuilder()
            .Build();

        Console.WriteLine("Invoking grpc deposit");
        var deposit = new DaprDotnet.Service.Grpc.Transaction() { Id = "17", Amount = 99 };
        var account = await client.InvokeMethodGrpcAsync<DaprDotnet.Service.Grpc.Transaction, DaprDotnet.Service.Grpc.Account>("daprdotnet.service", "deposit", deposit, cancellationToken);
        Console.WriteLine("Returned: id:{0} | Balance:{1}", account.Id, account.Balance);
        Console.WriteLine("Completed grpc deposit");

        Console.WriteLine("Invoking grpc withdraw");
        var withdraw = new DaprDotnet.Service.Grpc.Transaction() { Id = "17", Amount = 10, };
        await client.InvokeMethodGrpcAsync("grpcsample", "withdraw", withdraw, cancellationToken);
        Console.WriteLine("Completed grpc withdraw");

        Console.WriteLine("Invoking grpc balance");
        var request = new DaprDotnet.Service.Grpc.GetAccountRequest() { Id = "17", };
        account = await client.InvokeMethodGrpcAsync<DaprDotnet.Service.Grpc.GetAccountRequest, DaprDotnet.Service.Grpc.Account>("daprdotnet.service", "getaccount", request, cancellationToken);
        Console.WriteLine($"Received grpc balance {account.Balance}");
    }
}