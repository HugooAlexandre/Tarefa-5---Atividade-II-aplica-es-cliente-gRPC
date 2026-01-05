using Grpc.Net.Client;
using VotingSystem; // Namespace gerado pelo voter.proto

// URL fornecido no enunciado V2.0
const string ENDPOINT = "https://ken01.utad.pt:9091";

Console.Title = "Cliente Autoridade de Registo (AR)";
Console.WriteLine($"--- Cliente da Autoridade de Registo (AR) ---");
Console.WriteLine($"A conectar a {ENDPOINT}...");

// CONFIGURAÇÃO SSL: Permitir certificados não confiáveis (equivalente ao -insecure)
var httpHandler = new HttpClientHandler();
httpHandler.ServerCertificateCustomValidationCallback = 
    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

using var channel = GrpcChannel.ForAddress(ENDPOINT, new GrpcChannelOptions { HttpHandler = httpHandler });
var client = new VoterRegistrationService.VoterRegistrationServiceClient(channel);

while (true)
{
    Console.WriteLine("\n------------------------------------------------");
    Console.Write("Introduza o N.º de Cartão de Cidadão (ou 'sair'): ");
    var ccNumber = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(ccNumber) || ccNumber.ToLower() == "sair") break;

    try
    {
        Console.WriteLine("A contactar servidor...");
        
        // Chamada gRPC
        var reply = await client.IssueVotingCredentialAsync(
            new VoterRequest { CitizenCardNumber = ccNumber });

        if (reply.IsEligible)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[SUCESSO] Credencial recebida: {reply.VotingCredential}");
            Console.WriteLine("--> COPIE ESTA CREDENCIAL PARA VOTAR <--");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n[AVISO] {reply.VotingCredential} (Inelegível)");
        }
    }
    catch (Grpc.Core.RpcException rpcEx)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nErro gRPC: {rpcEx.Status.Detail ?? rpcEx.Message}");
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nErro: {ex.Message}");
    }
    finally
    {
        Console.ResetColor();
    }
}