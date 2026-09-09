# 📌 BibliotecaELM — Checkpoint 04

## 🎯 Sobre o Projeto (Domínio Escolhido)

Este projeto é uma API REST em .NET desenvolvida seguindo os princípios de **Clean Architecture**, abordando o domínio de uma **Biblioteca**. O sistema gerencia o serviço clássico de empréstimos (locação de acervo físico) e transações de compras/aquisição de livros em definitivo pelos usuários.

O projeto foi evoluído ao longo dos CPs:

| CP | Entrega |
|----|---------|
| CP1 | MER + entidades em C# |
| CP2 | Esquema físico + EF Core + Migrations (Oracle) |
| CP3 | API REST + Swagger + `IRepository<T>` + `GlobalExceptionHandler` |
| CP4 | Health Checks + Logs estruturados com `traceId` + Testes xUnit/Moq |

---

## 👥 Integrantes da Equipe

<table>
<tr>
<th>Nome</th>
<th>RM</th>
<th>Turma</th>
<th>GitHub</th>
<th>LinkedIn</th>
</tr>

<tr>
<td>Enzo Okuizumi</td>
<td>561432</td>
<td>2TDSPG</td>
<td><a href="https://github.com/EnzoOkuizumiFiap">EnzoOkuizumiFiap</a></td>
<td><a href="https://www.linkedin.com/in/enzo-okuizumi-b60292256/">Enzo Okuizumi</a></td>
</tr>

<tr>
<td>Lucas Barros Gouveia</td>
<td>566422</td>
<td>2TDSPG</td>
<td><a href="https://github.com/LuzBGouveia">LuzBGouveia</a></td>
<td><a href="https://www.linkedin.com/in/lucas-barros-gouveia-09b147355/">Lucas Barros Gouveia</a></td>
</tr>

<tr>
<td>Milton Marcelino</td>
<td>564836</td>
<td>2TDSPG</td>
<td><a href="https://github.com/MiltonMarcelino">MiltonMarcelino</a></td>
<td><a href="http://linkedin.com/in/milton-marcelino-250298142">Milton Marcelino</a></td>
</tr>

</table>

---

## 🧭 Escopo e Entregas do Checkpoint 04 (CP4)

### 1. Health Checks e Operacionalidade em Runtime
* **Endpoint `/health`**: Implementado na API do ASP.NET Core para verificação contínua de integridade do serviço.
* **Verificação de Banco de Dados**: Configurado via `AddDbContextCheck<BibliotecaElmContext>()` no `HealthCheckExtensions`, garantindo o status de conexão com o banco Oracle antes de responder `Healthy`.
* **Resposta Formatada (JSON)**: Resposta customizada que retorna um payload estruturado detalhando o status global e o resultado individual dos componentes verificados (`self`, `database`), duração e exceção condicionada ao ambiente `Development`.

### 2. Observabilidade e Logging com TraceId / Correlation ID
* **Injeção de `ILogger<T>` nos Controllers e Services**: Controllers capturam nativamente `HttpContext.TraceIdentifier` e registram início e término das requisições com propriedades estruturadas. Os serviços de aplicação utilizam `ILogger<T>` para registrar logs de repositório e avisos de validação de negócio, mantendo a camada Application desacoplada de HTTP.
* **Logs Estruturados**: Eventos cruciais da aplicação (criação de recursos, atualizações, exclusões e validações falhas) gravam dados estruturados usando parâmetros nomeados (`NomeLivro`, `LivroId`, `AutorId`, `UsuarioId`, `TraceId`).
* **Correlation ID / TraceId**: Todas as mensagens de log nos controllers e respostas de exceção correlacionam requisições utilizando o `TraceId` extraído de `HttpContext.TraceIdentifier`, garantindo rastreabilidade fim a fim.
* **Filtro de Erros (`GlobalExceptionHandler`)**: Atualizado para incluir o `TraceId` no payload no formato `ProblemDetails` (`RFC 7807`) em ambiente `Development` e registrar a exceção no log antes do envio ao cliente.

### 3. Testes Unitários Automatizados (xUnit + Moq)
A solução possui projetos dedicados para validação automatizada sem dependências externas reais:

* **Domain Tests (`BibliotecaELM.Domain.Tests`) — 38 testes**:
  * **Testes de Entidades (`LivroTests`, `UsuarioTests`, `CompraTests`, `EmprestimoTests`, `EnderecoTests`)**: Utilizam os atributos `[Fact]` para validação de instanciação válida e `[Theory]` / `[InlineData]` para cenários de borda.
  * **Isolamento de Domínio**: Validação rigorosa das regras e invariantes de negócio sem mocks e sem dependências de Infrastructure/API.
* **Application Tests (`BibliotecaELM.Application.Tests`) — 21 testes**:
  * **Testes de Serviços (`LivroServiceTests`, `AutorServiceTests`, `CompraServiceTests`, `UsuarioServiceTests`, `EnderecoServiceTests`, `LivroAppServiceTests`)**: Utilizam **Moq** para simular as interfaces de repositório (`ILivroRepository`, `IAutorRepository`, `IUsuarioRepository`, `IEnderecoRepository`, `ICompraRepository`).
  * **Verificação de Regras de Fluxo**: Testam se a aplicação lança exceções para entradas inválidas ou dependências ausentes e asseguram que o repositório **nunca** seja chamado em cenários de falha (`Times.Never`), e chamado **exatamente uma vez** (`Times.Once`) em cenários de sucesso.

---

## 🧱 Arquitetura e Estrutura do Projeto

O projeto segue os princípios de Clean Architecture, organizado nas seguintes camadas:

1. **Domain (`BibliotecaELM.Domain`)**: Entidades de domínio (Autor, Livro, Usuario, Endereco, Compra, Emprestimo), exceções de negócio (`BusinessRuleValidationException`, `ResourceNotFoundException`, `DomainException`) e classe base `BaseEntity`. Livre de dependências externas.
2. **Application (`BibliotecaELM.Application`)**: DTOs (`Request` e `Response`), interfaces de repositórios e serviços, e a camada de implementação (`LivroService`, `AutorService`, `UsuarioService`, `EnderecoService`, etc.) contendo validações e logs.
3. **Infrastructure (`BibliotecaELM.Infrastructure`)**: Implementação do repositório genérico `Repository<T>`, repositórios concretos, contexto de persistência `BibliotecaElmContext` (Oracle) e as Migrations do EF Core.
4. **API (`BibliotecaELM.API`)**: Ponto de entrada da aplicação com Controllers, endpoints de Health Check (`/health`), pipeline de middlewares, `GlobalExceptionHandler` e documentação Swagger.
5. **Tests (`BibliotecaELM.Domain.Tests` e `BibliotecaELM.Application.Tests`)**: Suítes de testes unitários com xUnit e Moq.

```
BibliotecaELM/
├── BibliotecaELM.Domain/            # Entidades, Exceções de domínio, BaseEntity
├── BibliotecaELM.Application/       # DTOs, Interfaces de repositório/serviço, Serviços de aplicação
├── BibliotecaELM.Infrastructure/    # Repository<T>, repositórios concretos, DbContext (Oracle), Migrations
├── BibliotecaELM.API/               # Controllers, GlobalExceptionHandler, Swagger, Health Checks
├── BibliotecaELM.Domain.Tests/      # 38 testes xUnit do domínio (sem mock)
└── BibliotecaELM.Application.Tests/ # 21 testes xUnit da Application (com Moq)
```

**SGBD:** Oracle (via `Oracle.EntityFrameworkCore`)

---

## 🚀 Como Executar a API

### Pré-requisitos
- .NET 10 SDK
- Banco de dados Oracle acessível
- Connection string configurada em `BibliotecaELM.API/appsettings.json` (ou via User Secrets)

### Executar

```bash
# A partir do diretório raiz da solution
cd BibliotecaELM/BibliotecaELM.API
dotnet run
```

### URLs após subir

| Recurso | URL |
|---------|-----|
| **Swagger UI** | `http://localhost:<port>/` (raiz — configurado como `RoutePrefix = ""`) |
| **Health Check** | `http://localhost:<port>/health` |

---

## 🏥 Health Checks — `GET /health`

A rota `/health` verifica dois checks registrados via extensão `HealthCheckExtensions`:

| Check | Descrição |
|-------|-----------|
| `self` | Processo da API no ar (`HealthCheckResult.Healthy`) |
| `database` | Conectividade com o banco Oracle via `AddDbContextCheck<BibliotecaElmContext>` |

### Resposta Healthy (HTTP 200)

```json
{
  "status": "Healthy",
  "duration": "00:00:00.0123456",
  "checks": [
    { "name": "self",     "status": "Healthy", "duration": "00:00:00.0001234" },
    { "name": "database", "status": "Healthy", "duration": "00:00:00.0112345" }
  ]
}
```

### Resposta Unhealthy (HTTP 503)

Para simular falha de banco, altere a connection string para um servidor inválido em `appsettings.json` localmente:

```json
"BibliotecaElmOracle": "User Id=invalid;Password=invalid;Data Source=//localhost:1521/INVALID"
```

```json
{
  "status": "Unhealthy",
  "duration": "00:00:05.0012345",
  "checks": [
    { "name": "self",     "status": "Healthy",   "duration": "00:00:00.0001000" },
    { "name": "database", "status": "Unhealthy",  "duration": "00:00:05.0011345" }
  ]
}
```

> **Nota:** O campo `exception` só aparece quando a API está em `ASPNETCORE_ENVIRONMENT=Development`.

---

## 🗂️ Repositório Genérico (`IRepository<T>`)

- **Contrato:** `BibliotecaELM.Application/Services/Interfaces/IRepository.cs`
  - Operações: `GetAll()`, `GetById(Guid)`, `Add(T)`, `Update(T)`, `Delete(T)`, `ExistsById(Guid)`
  - Restrição: `where T : BaseEntity`
- **Implementação:** `BibliotecaELM.Infrastructure/Repositories/Repository.cs`
  - Usa `DbContext.Set<T>()` + `AsNoTracking()` nas leituras
- **Registro na DI:** `services.AddScoped(typeof(IRepository<>), typeof(Repository<>))`
- Todos os repositórios específicos herdam ou usam `IRepository<T>` (ex.: `ILivroRepository : IRepository<Livro>`)

---

## ⚠️ Mapeamento de Exceções → Status HTTP

| Exceção | Status HTTP | Título |
|---------|-------------|--------|
| `BusinessRuleValidationException` | **400** Bad Request | Regra de Negócio Violada |
| `ArgumentException` | **400** Bad Request | Requisição Inválida |
| `InvalidOperationException` | **400** Bad Request | Operação Inválida |
| `ResourceNotFoundException` | **404** Not Found | Recurso não encontrado |
| `KeyNotFoundException` | **404** Not Found | Recurso não encontrado |
| Qualquer outra | **500** Internal Server Error | Erro interno do servidor |

Todas as respostas de erro seguem o padrão **RFC 7807** (`application/problem+json`).  
Em **Development**, o campo `traceId` é exposto no body do `ProblemDetails`.  
Em **Production**, o `traceId` fica apenas nos logs (sem vazar detalhes internos).

---

## 📊 Observabilidade — Logs Estruturados com TraceId

Seguindo o padrão de arquitetura e observabilidade do projeto de referência (**Recommenda**):

1. **Controllers (Camada API)**: Cada fluxo de escrita captura nativamente `HttpContext.TraceIdentifier` e registra o ciclo de vida da requisição HTTP com parâmetros nomeados:
   - **`LivroController`**: Log de início e conclusão em `Create`, `Update` e `Delete`.
   - **`AutorController`**: Log de início e conclusão em `Create`, `Update` e `Delete`.
   - **`CompraController`**: Log de início e conclusão em `Create`, `Update` e `Delete`.
   - **`UsuarioController`**: Log de início e conclusão em `Create`, `Update` e `Delete`.
   - **`EmprestimoController`**: Log de início e conclusão em `Create`, `Update` e `Delete`.
   - **`EnderecoController`**: Log de início e conclusão em `Create`, `Update` e `Delete`.

2. **GlobalExceptionHandler**: Captura centralizada de exceções em nível `Error`, correlacionando via `Activity.Current?.Id ?? HttpContext.TraceIdentifier`:
   ```
   fail: BibliotecaELM.Exceptions.GlobalExceptionHandler[0]
         Exceção não tratada: Já existe um autor cadastrado com este nome. : TraceId 0HN9ABCD:00000001
   ```

3. **Application Services**: Recebem `ILogger<T>` para registrar eventos de domínio e avisos de validação de negócio sem poluição de dependências HTTP (`IHttpContextAccessor`), mantendo a camada Application 100% pura.

Exemplo de log emitido pelo `LivroController`:
```
info: BibliotecaELM.Controllers.LivroController[0]
      Iniciando criação de livro : Clean Architecture TraceId 0HN9ABCD:00000001
info: BibliotecaELM.Controllers.LivroController[0]
      Finalizando criação de livro : Clean Architecture (e7d23a10-...) TraceId 0HN9ABCD:00000001
```

---

## 🧪 Como Executar os Testes Automatizados

```bash
# A partir do diretório da solution (BibliotecaELM/)
dotnet test
```

### Saída esperada

```
Test Run Successful.
Total tests: 59
     Passed: 59
  Total time: ~2 Seconds
```

| Projeto | Testes | Cobertura | Tipo |
|---------|--------|-----------|------|
| `BibliotecaELM.Domain.Tests` | 38 testes | `Livro`, `Usuario`, `Compra`, `Emprestimo`, `Endereco` | Sem mock — regras reais de domínio (`[Fact]` e `[Theory]`) |
| `BibliotecaELM.Application.Tests` | 21 testes | `LivroService`, `AutorService`, `CompraService`, `UsuarioService`, `EnderecoService`, `LivroAppService` | Mocks via Moq — validação de `Times.Never` em falha e `Times.Once` em sucesso |

---

## 📎 Entregáveis por CP

| CP | Entregável |
|----|-----------|
| CP2 | Migrations + DbContext + Oracle |
| CP3 | Controllers + DTOs + Swagger + `IRepository<T>` + `GlobalExceptionHandler` + `ProblemDetails` |
| CP4 | `GET /health` (JSON 200/503) + Logs com `traceId` + `Domain.Tests` (38) + `Application.Tests` (21) |