# DeskFlow API

API REST desenvolvida em ASP.NET Core 10 para gerenciamento de chamados de Help Desk.

## 1. Sobre o projeto

O **DeskFlow API** é um projeto acadêmico desenvolvido no curso de Back-End .NET, com o objetivo de aplicar conceitos de construção de APIs REST, organização em camadas, injeção de dependência, persistência de dados e implementação de regras de negócio.

A aplicação permite gerenciar categorias de atendimento e chamados de suporte técnico, desde a abertura até o encerramento, incluindo acompanhamento do status, filtros de consulta e registro de interações.

O projeto utiliza Entity Framework Core para comunicação com o banco de dados SQL Server e Swagger para documentação e testes dos endpoints.

## 2. Objetivos do projeto

- Desenvolver uma API REST utilizando ASP.NET Core.
- Implementar operações CRUD para categorias.
- Gerenciar o ciclo de vida dos chamados.
- Aplicar regras de negócio para transições de status.
- Utilizar Entity Framework Core para persistência de dados.
- Organizar o código em camadas com responsabilidades definidas.
- Implementar injeção de dependência.
- Centralizar o tratamento de exceções.
- Documentar e testar os endpoints com Swagger.
- Utilizar Git e GitHub para versionamento do código.

## 3. Tecnologias utilizadas

- **C#** — linguagem de programação.
- **.NET 10** — plataforma de desenvolvimento.
- **ASP.NET Core Web API** — construção dos endpoints REST.
- **Entity Framework Core 10** — mapeamento objeto-relacional e acesso ao banco.
- **SQL Server** — banco de dados relacional.
- **Swagger / OpenAPI** — documentação e testes da API.
- **Git e GitHub** — controle de versão e hospedagem do código.

## 4. Funcionalidades

### 4.1. Gerenciamento de categorias

- Cadastrar novas categorias.
- Listar categorias cadastradas.
- Consultar uma categoria por identificador.
- Atualizar informações de uma categoria.
- Excluir categorias, respeitando as restrições de relacionamento com chamados.

### 4.2. Gerenciamento de chamados

- Abrir novos chamados de suporte.
- Listar chamados cadastrados.
- Consultar os detalhes de um chamado.
- Filtrar chamados por status, prioridade e categoria.
- Iniciar o atendimento de um chamado.
- Encerrar chamados informando a solução aplicada.
- Registrar interações durante o atendimento.
- Consultar informações relacionadas à categoria e às interações do chamado.

### 4.3. Regras de negócio

O sistema controla o ciclo de vida dos chamados por meio dos seguintes estados:

1. **Aberto:** estado inicial atribuído à criação do chamado.
2. **EmAndamento:** estado definido quando o atendimento é iniciado.
3. **Fechado:** estado atribuído após o encerramento com a solução informada.

As transições de status são validadas pelo serviço de chamados. O encerramento exige que o chamado esteja em andamento e que a solução não esteja vazia. Chamados fechados não aceitam novas interações.

O sistema também verifica a existência de registros relacionados antes de executar determinadas operações.

## 5. Arquitetura e organização do projeto

A aplicação segue uma organização em camadas para separar responsabilidades e facilitar a manutenção.

- **Controllers:** recebem as requisições HTTP, acionam os serviços ou o contexto de dados utilizado pelo endpoint e retornam as respostas.
- **Services:** concentram as regras de negócio, incluindo as transições de status dos chamados.
- **Repositories:** implementam a abstração de acesso a dados utilizada pelas funcionalidades que adotam esse padrão.
- **Data:** contém o contexto do Entity Framework Core e a configuração de acesso ao banco.
- **Models:** contém as entidades e os modelos utilizados pela aplicação.
- **Middlewares:** contém o tratamento global de exceções.

A injeção de dependência é configurada no `Program.cs`, permitindo registrar os serviços e demais componentes necessários à execução da API.

## 6. Endpoints da API

A API utiliza o prefixo `/api` para os recursos.

### 6.1. Categorias

| Método HTTP | Endpoint | Descrição |
|---|---|---|
| GET | `/api/categorias` | Lista as categorias |
| GET | `/api/categorias/{id}` | Consulta uma categoria pelo ID |
| POST | `/api/categorias` | Cadastra uma categoria |
| PUT | `/api/categorias/{id}` | Atualiza uma categoria |
| DELETE | `/api/categorias/{id}` | Exclui uma categoria |

### 6.2. Chamados

| Método HTTP | Endpoint | Descrição |
|---|---|---|
| GET | `/api/chamados` | Lista chamados e permite aplicar filtros |
| GET | `/api/chamados/{id}` | Consulta um chamado pelo ID |
| POST | `/api/chamados` | Abre um novo chamado |
| POST | `/api/chamados/{id}/iniciar` | Inicia o atendimento |
| POST | `/api/chamados/{id}/encerrar` | Encerra o atendimento informando a solução |
| POST | `/api/chamados/{id}/interacoes` | Registra uma interação |

### 6.3. Filtros de consulta

O endpoint `GET /api/chamados` aceita os seguintes parâmetros opcionais:

- `status`
- `prioridade`
- `categoriaId`

Exemplo de consulta:

`GET /api/chamados?status=2&prioridade=2&categoriaId=6`

Os valores numéricos de `status` e `prioridade` devem corresponder aos valores definidos nos enums do projeto.

### 6.4. Exemplos de requisições

**Cadastrar uma categoria**

```http
POST /api/categorias
Content-Type: application/json
```

O corpo da requisição deve conter os campos definidos no modelo de categoria da aplicação.

**Abrir um chamado**

```http
POST /api/chamados
Content-Type: application/json
```

O corpo deve conter os dados exigidos pelo modelo de chamado, incluindo a categoria relacionada.

**Iniciar um chamado**

```http
POST /api/chamados/1/iniciar
```

**Encerrar um chamado**

```http
POST /api/chamados/1/encerrar
Content-Type: application/json
```

O corpo deve informar a solução no formato esperado pelo endpoint.

**Registrar uma interação**

```http
POST /api/chamados/1/interacoes
Content-Type: application/json
```

O corpo deve conter os dados exigidos pelo modelo de interação.

Os exemplos com identificador `1` são ilustrativos; é necessário utilizar um chamado existente no banco de dados.

## 7. Banco de dados e persistência

O projeto utiliza SQL Server para armazenar as informações e Entity Framework Core para mapear as entidades e executar operações de persistência.

O contexto `DeskFlowContext` centraliza o acesso aos conjuntos de entidades e a configuração utilizada pelo EF Core.

### Migrations

As migrations registradas no projeto são:

- `InitialCreate`
- `AtualizarCamposChamado`
- `LimitesTextoChamado`

Elas registram a evolução do esquema do banco de dados ao longo do desenvolvimento.

A migration `LimitesTextoChamado` contempla os limites de tamanho definidos para campos de texto do chamado, conforme a configuração do projeto.

## 8. Validação e tratamento de erros

A aplicação possui um middleware global para tratamento de exceções, registrado no pipeline HTTP.

O middleware transforma exceções conhecidas em respostas HTTP apropriadas, incluindo:

- **404 Not Found:** registro não encontrado.
- **400 Bad Request:** operação inválida ou dados que não atendem às regras verificadas.
- **500 Internal Server Error:** erro interno não tratado por uma categoria específica.

Os endpoints também realizam verificações necessárias para operações como abertura de chamados, encerramento e registro de interações.

## 9. Documentação com Swagger

O Swagger fornece uma interface interativa para consultar os endpoints, visualizar os métodos HTTP e executar requisições de teste.

Em ambiente de desenvolvimento, após iniciar a aplicação, acesse a URL indicada no terminal e acrescente `/swagger`.

A disponibilidade da interface depende da configuração do ambiente e do pipeline da aplicação.

## 10. Como executar o projeto

### Pré-requisitos

- .NET SDK 10.
- SQL Server configurado.
- Visual Studio Code ou IDE compatível.
- Git instalado.

### 10.1. Clonar o repositório

```bash
git clone https://github.com/judhu/DeskFlow.API.git
```

### 10.2. Acessar o projeto

Entre na pasta que contém o arquivo `.csproj` da API.

### 10.3. Configurar a conexão com o banco

Configure a connection string `DefaultConnection` no arquivo `appsettings.json`, utilizando os dados do seu ambiente SQL Server.

Não publique senhas, tokens ou credenciais reais no repositório.

### 10.4. Restaurar as dependências

```bash
dotnet restore
```

### 10.5. Aplicar as migrations

```bash
dotnet ef database update
```

Caso a ferramenta de linha de comando do Entity Framework Core não esteja instalada, configure a ferramenta compatível com a versão utilizada no projeto.

### 10.6. Executar a API

```bash
dotnet run
```

A aplicação exibirá no terminal os endereços em que está disponível.

## 11. Testes realizados

Durante o desenvolvimento, foram testados endpoints pelo Swagger, incluindo:

- Cadastro e consulta de categorias.
- Atualização de categorias.
- Verificação das restrições de exclusão de categorias relacionadas a chamados.
- Abertura e consulta de chamados.
- Consulta de detalhes e interações.
- Filtros por status, prioridade e categoria.
- Início e encerramento de chamados.
- Validação das regras de negócio para operações inválidas.
- Registro de interações e bloqueio de interações em chamados fechados.

Esses testes permitiram verificar o funcionamento dos principais fluxos implementados.

## 12. Aprendizados

O desenvolvimento do DeskFlow API permitiu praticar conceitos fundamentais do desenvolvimento Back-End com .NET, como:

- Construção de APIs REST.
- Organização do código em camadas.
- Injeção de dependência.
- Persistência com Entity Framework Core.
- Versionamento de banco de dados por migrations.
- Implementação de regras de negócio.
- Tratamento global de exceções.
- Testes de endpoints com Swagger.
- Versionamento de código com Git e GitHub.

## 13. Objetivo acadêmico

O DeskFlow API foi desenvolvido como parte do projeto avaliativo do curso de Back-End .NET, com foco na aplicação prática dos conceitos estudados e na construção de uma API organizada, funcional e documentada.

---

**DeskFlow API** — API REST para gerenciamento de chamados de Help Desk.
