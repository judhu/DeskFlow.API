# DeskFlow API

API REST desenvolvida em ASP.NET Core 10 para gerenciamento de chamados de Help Desk.

## Sobre o projeto

O DeskFlow API foi desenvolvido como projeto avaliativo do curso de Back-End .NET.

A aplicação permite o gerenciamento de categorias e chamados, incluindo abertura, acompanhamento, encerramento e registro de interações.

O projeto foi desenvolvido com foco na organização em camadas, separação de responsabilidades, persistência de dados com Entity Framework Core e documentação dos endpoints com Swagger/OpenAPI.

## Tecnologias utilizadas

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server
- Swagger / OpenAPI
- Git
- GitHub

## Arquitetura

O projeto utiliza uma organização em camadas, separando responsabilidades entre:

- Controllers
- Services
- Repositories
- Models / Entities
- Data
- Middlewares

Essa organização facilita a manutenção, evolução e separação das responsabilidades da aplicação.

## Funcionalidades

### Categorias

- Criar categoria
- Listar categorias
- Consultar categoria por ID
- Atualizar categoria
- Excluir categoria
- Validação de relacionamento com chamados

### Chamados

- Criar chamado
- Listar chamados
- Consultar chamado por ID
- Atualizar chamado
- Excluir chamado
- Filtrar por status
- Filtrar por prioridade
- Filtrar por categoria
- Iniciar chamado
- Encerrar chamado
- Registrar interações
- Consultar detalhes do chamado com categoria e interações

## Ciclo de vida do chamado

O chamado possui os seguintes estados:

- Aberto
- EmAndamento
- Fechado

O fluxo principal do chamado é:

**Aberto → EmAndamento → Fechado**

Para iniciar um chamado, ele deve estar no status `Aberto`.

Para encerrar um chamado, ele deve estar no status `EmAndamento` e é necessário informar uma solução.

Chamados fechados não permitem o registro de novas interações.

## Principais endpoints

### Categorias

```text
GET     /api/categorias
GET     /api/categorias/{id}
POST    /api/categorias
PUT     /api/categorias/{id}
DELETE  /api/categorias/{id}

Chamados
GET     /api/chamados
GET     /api/chamados/{id}
POST    /api/chamados
PUT     /api/chamados/{id}
DELETE  /api/chamados/{id}

Ciclo de vida
POST    /api/chamados/{id}/iniciar
POST    /api/chamados/{id}/encerrar

Interações
POST    /api/chamados/{id}/interacoes

Filtros de chamados
A listagem de chamados permite utilizar filtros por:
- Status
- Prioridade
- Categoria
Exemplos:
GET /api/chamados?status=1
GET /api/chamados?prioridade=2
GET /api/chamados?categoriaId=1

Os filtros também podem ser combinados:
GET /api/chamados?status=2&prioridade=3&categoriaId=1

Relacionamentos
Os principais relacionamentos da aplicação são:
Categoria
   │
   └── Chamados
          │
          └── Interacoes

Um chamado pertence a uma categoria e pode possuir várias interações.
O relacionamento entre categorias e chamados também garante a integridade referencial dos dados.
Validações
A aplicação possui validações para garantir a consistência das informações recebidas pela API.

Entre elas:

- Título obrigatório
- Descrição obrigatória
- Nome do solicitante obrigatório
- Limite de caracteres para os campos de texto
- Categoria deve existir para criação ou atualização de chamado
- Solução obrigatória para encerramento do chamado
- Autor e mensagem obrigatórios para registro de interação
- Chamados fechados não permitem novas interações

Tratamento de exceções

A aplicação possui um middleware global para tratamento de exceções.
O middleware centraliza o tratamento de erros inesperados e retorna respostas padronizadas para a API.
Isso evita a repetição de tratamentos de exceção nos controllers e facilita a manutenção da aplicação.

Banco de dados

O projeto utiliza:
- SQL Server
- Entity Framework Core
- Migrations

As migrations utilizadas no projeto incluem:

InitialCreate
AtualizarCamposChamado

O arquivo script.sql contém o script SQL gerado a partir das migrations do projeto.

Como executar o projeto

Pré-requisitos

- .NET 10 SDK
- SQL Server
- Visual Studio Code ou Visual Studio

1. Clonar o repositório
git clone https://github.com/judhu/DeskFlow.API.git

2. Acessar a pasta do projeto
cd DeskFlow.API

3. Restaurar as dependências
dotnet restore

4. Configurar a conexão com o banco
Configure a connection string do SQL Server no arquivo:
appsettings.json

5. Aplicar as migrations
dotnet ef database update

6. Executar a aplicação
dotnet run

Após iniciar a aplicação, os endpoints podem ser testados e consultados por meio do Swagger.

Swagger / OpenAPI

A aplicação utiliza Swagger para documentação e testes dos endpoints da API.
Após executar o projeto, acesse a URL apresentada no terminal pela aplicação para abrir a documentação do Swagger.

O Swagger permite visualizar os endpoints, parâmetros, modelos de dados e realizar testes diretamente pela interface.

Estrutura do projeto

DeskFlow.API/
│
├── Controllers/
│   ├── CategoriasController.cs
│   └── ChamadosController.cs
│
├── Data/
│   └── DeskFlowContext.cs
│
├── Middlewares/
│   └── ExceptionHandlingMiddleware.cs
│
├── Migrations/
│
├── Models/
│   └── Entities/
│       ├── Categoria.cs
│       ├── Chamado.cs
│       ├── Interacao.cs
│       ├── Prioridade.cs
│       └── StatusChamado.cs
│
├── Repositories/
│   ├── CategoriaRepository.cs
│   └── ICategoriaRepository.cs
│
├── Services/
│   ├── CategoriaService.cs
│   ├── ICategoriaService.cs
│   ├── ChamadoService.cs
│   └── IChamadoService.cs
│
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
├── DeskFlow.API.csproj
└── script.sql

Versionamento

O projeto utiliza Git para controle de versão e GitHub para hospedagem do código-fonte.
Os commits foram organizados de acordo com a evolução das funcionalidades do projeto.

Repositório:
https://github.com/judhu/DeskFlow.API

Testes

Os endpoints da API foram testados utilizando o Swagger / OpenAPI.

Foram realizados testes envolvendo:

- CRUD de categorias
- CRUD de chamados
- Filtros de chamados
- Início de chamados
- Encerramento de chamados
- Registro de interações
- Consulta de detalhes com relacionamentos
- Validações
- Restrição de interações em chamados fechados

Autoria
Juliane Alves de Almeida

Projeto desenvolvido durante o curso de Back-End .NET.

Projeto acadêmico
Este projeto foi desenvolvido para fins educacionais e acadêmicos, como parte da formação em Back-End .NET.
