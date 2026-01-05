Estrutura do Projeto

RegistoClient/ - Aplicação de consola para interação com a AR (Emissão de credenciais).
VotacaoClient/ - Aplicação de consola para interação com a AV (Lista de candidatos, Voto e Resultados).
Protos/ - Contém os ficheiros de contrato gRPC (`voter.proto` e `voting.proto`).

## Pré-requisitos

.NET SDK(https://dotnet.microsoft.com/download) (Versão 6.0, 7.0 ou 8.0).
Acesso à Internet (para comunicar com o servidor remoto).

## Configuração Inicial

Certifique-se de que os pacotes NuGet necessários estão restaurados antes de executar:

bash

dotnet restore RegistoClient
dotnet restore VotacaoClient



## Como Executar

O fluxo recomendado de teste é: Obter Credencial -> Votar -> Ver Resultados.

### Cliente de Registo (AR)
Utilize esta aplicação para validar um Cartão de Cidadão e obter uma credencial de voto anónima.

bash

cd RegistoClient

dotnet run

Input: Insira um número de CC .
Output: Copie a credencial gerada.

### 2. Cliente de Votação (AV)
Utilize esta aplicação para exercer o direito de voto e consultar o apuramento.

bash

cd VotacaoClient

dotnet run

Opção 1: Liste os candidatos para saber os seus IDs.
Opção 2: Vote inserindo a credencial obtida no passo anterior e o ID do candidato.
Opção 3: Consulte a tabela de resultados em tempo real.

## Endpoint e Conexão

Servidor: ken01.utad.pt
Porta: 9091
Protocolo: http 

