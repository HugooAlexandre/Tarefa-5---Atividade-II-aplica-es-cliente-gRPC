using Grpc.Net.Client;
using VotingSystem.Voting; // Namespace gerado pelo voting.proto

// URL fornecido no enunciado V2.0
const string ENDPOINT = "https://ken01.utad.pt:9091";

Console.Title = "Cliente Autoridade de Votação (AV)";
Console.WriteLine($"--- Cliente da Autoridade de Votação (AV) ---");

// CONFIGURAÇÃO SSL
var httpHandler = new HttpClientHandler();
httpHandler.ServerCertificateCustomValidationCallback = 
    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

using var channel = GrpcChannel.ForAddress(ENDPOINT, new GrpcChannelOptions { HttpHandler = httpHandler });
var client = new VotingService.VotingServiceClient(channel);

while (true)
{
    Console.WriteLine("\n=== MENU DE VOTAÇÃO ===");
    Console.WriteLine("1 - Listar Candidatos");
    Console.WriteLine("2 - Votar");
    Console.WriteLine("3 - Consultar Resultados");
    Console.WriteLine("0 - Sair");
    Console.Write("Opção: ");
    
    var option = Console.ReadLine();

    try
    {
        switch (option)
        {
            case "1":
                await ListarCandidatos(client);
                break;
            case "2":
                await Votar(client);
                break;
            case "3":
                await VerResultados(client);
                break;
            case "0":
                Console.WriteLine("A sair...");
                return;
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nErro de comunicação: {ex.Message}");
        Console.ResetColor();
    }
}

static async Task ListarCandidatos(VotingService.VotingServiceClient client)
{
    var reply = await client.GetCandidatesAsync(new GetCandidatesRequest());
    Console.WriteLine("\n--- Lista de Candidatos ---");
    Console.WriteLine($"{"ID",-5} | {"Nome",-20}");
    Console.WriteLine(new string('-', 30));
    
    foreach (var cand in reply.Candidates)
    {
        Console.WriteLine($"{cand.Id,-5} | {cand.Name,-20}");
    }
}

static async Task Votar(VotingService.VotingServiceClient client)
{
    Console.Write("\nIntroduza a Credencial de Voto: ");
    var cred = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(cred)) return;
    
    Console.Write("Introduza o ID do Candidato: ");
    if (int.TryParse(Console.ReadLine(), out int candId))
    {
        var reply = await client.VoteAsync(new VoteRequest 
        { 
            VotingCredential = cred, 
            CandidateId = candId 
        });

        if (reply.Success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[VOTO ACEITE] {reply.Message}");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[VOTO RECUSADO] {reply.Message}");
        }
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine("ID inválido.");
    }
}

static async Task VerResultados(VotingService.VotingServiceClient client)
{
    var reply = await client.GetResultsAsync(new GetResultsRequest());
    Console.WriteLine("\n--- Resultados Eleitorais Atuais ---");
    Console.WriteLine($"{"Candidato",-20} | {"Votos",-5}");
    Console.WriteLine(new string('-', 30));
    
    foreach (var res in reply.Results)
    {
        Console.WriteLine($"{res.Name,-20} | {res.Votes,-5}");
    }
}