# 📌 CP4 — Health Checks, Observabilidade e Testes com xUnit

## 🎯 Objetivo

1. **Tornar a API do CP3 operável em runtime** com **health checks** no ASP.NET Core:
   - Expor **`GET /health`** como único endpoint de health check (processo + dependências).
   - Verificar o **banco** usado no CP2 (série API × SGBD).
   - Resposta JSON interpretável (humano e ferramenta de monitoramento).

2. **Instrumentar observabilidade mínima** com **logs**:
   - **Logs** estruturados (`ILogger`) com correlação (`traceId` / `HttpContext.TraceIdentifier`).

3. **Proteger o domínio com testes automatizados**:
   - Projetos de teste **xUnit** na solução.
   - **Domínio** testado **sem mock** (AAA, `[Fact]` e `[Theory]`).
   - **Application** testada com **mock** das interfaces de repositório (Moq ou equivalente).
   - Execução reproduzível: `dotnet test` verde no README.

Manter **Clean Architecture**: health e logs na composição da **API**; regras de negócio continuam no **Domain** / **Application**; persistência na **Infrastructure**.

---

## 👥 Forma de Trabalho

- O trabalho deverá ser realizado **em grupo** com até **3 integrantes** (mesmo grupo do CP1–CP3).
- Cada grupo deverá entregar **um único repositório** no GitHub (**evolução** do repositório do CP3).
- Somente **um integrante** deverá entregar o link no portal do aluno.

---

## 🧭 Escopo (o que fazer)

### A) Health checks (disponibilidade operacional)

1. Pacote-base: `Microsoft.Extensions.Diagnostics.HealthChecks` (shared framework).
2. Registrar `AddHealthChecks()` na DI da **API**.
3. Implementar **no mínimo** dois checks registrados em `AddHealthChecks()`:
   - Check **`self`** (ou equivalente): processo no ar (`HealthCheckResult.Healthy`).
   - Check do **banco**. Escolher **uma** abordagem e documentar no README:
     - **(A)** `AddDbContextCheck<TContext>` (`Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore`) — preferível, alinhado ao `DbContext` do CP2; **ou**
     - **(B)** check do provider (ex.: `AspNetCore.HealthChecks.Sqlite` / `SqlServer` / `Npgsql` / `MySql`), coerente com o SGBD do grupo;
4. Mapear **apenas** **`GET /health`** com o relatório completo (todos os checks). Não é necessário separar `/alive` nem `/ready`.
5. **Response writer JSON** (não deixar só o texto `Healthy` padrão): status geral, duração, lista de checks (nome, status, duração; detalhe de exceção **só em Development**).
6. Status HTTP alinhados ao runtime: **Healthy → 200**; **Degraded → 200** (ainda serve tráfego, com aviso); **Unhealthy → 503**.
7. *(Recomendado)* Check extra de URL externa (ex.: site da FIAP). Se a URL cair, o grupo deve **entender** o impacto no status agregado de `/health` (um check `Unhealthy` derruba o relatório inteiro).
8. *(Recomendado)* Extensão `AddXxxHealthChecks` para não inchar o `Program.cs`.

**Validação:** API no ar e banco ok → `/health` **200** com os checks nomeados. Simular falha do banco (parar o SGBD ou connection string inválida **em ambiente local**) → `/health` **503**. Anotar isso no README ou em print em `/docs/`.

### B) Observabilidade (logs)

Os health checks dizem *posso receber tráfego agora?*. Os logs dizem *o que aconteceu* e *em qual requisição*.

1. **Logs**
   - Usar `ILogger<T>` (nativo ou Serilog — à escolha).
   - Em **pelo menos um** fluxo de escrita já existente no CP3 (controller ou serviço de aplicação): log de **início** e de **sucesso** (ou de falha de negócio), com **propriedades nomeadas** (não concatenar string solta) e **`traceId`** (`HttpContext.TraceIdentifier`).
   - No `GlobalExceptionHandler` do CP3: logar a exceção em nível **Error** incluindo o mesmo **`traceId`**.
   - *(Recomendado)* Em Development, incluir `traceId` em `ProblemDetails.Extensions`.
   - Em **Production**, a resposta HTTP continua **sem** stack trace (regra do CP3). O detalhe fica no **log**.

2. **Não misturar públicos**
   - `/health` → health check.
   - Swagger **não** precisa listar `/health`.
   - **Não** é necessário (nem será cobrado) `GET /metrics`, Prometheus, Jaeger, Seq nem OpenTelemetry Tracing.

### C) Testes com xUnit (pirâmide — base e meio)

1. Incluir na **mesma solution** no mínimo:
   - `Projeto.Domain.Tests` — referencia **somente Domain**.
   - `Projeto.Application.Tests` — referencia Application (e Domain, indiretamente); **não** sobe a API nem o banco.
2. Pacotes: `xunit`, `Microsoft.NET.Test.Sdk`, runner Visual Studio; **Moq** (ou NSubstitute) no projeto de Application; *(recomendado)* `coverlet.collector`.
3. **Domínio (sem mock)** — mínimo:
   - Uma entidade (ou value object) com **regra de negócio real** do MER (ex.: faixa de nota, unicidade de invariante, data mínima, campo obrigatório).
   - Pelo menos **um** `[Fact]` no caminho feliz (AAA explícito: Arrange / Act / Assert).
   - Pelo menos **um** `[Theory]` + `[InlineData]` no caminho de erro (exceção de domínio).
   - Nomes de método no estilo `MetodoOuCenario_Condicao_ResultadoEsperado`.
4. **Application (com mock)** — mínimo:
   - Um serviço de aplicação que já existe no CP3 (criação/atualização).
   - Mock das **interfaces** de repositório (`IRepository<T>` e/ou repositórios específicos).
   - Cenário: dependência ausente (usuário/conteúdo/categoria inexistente) → lança a exceção já mapeada no CP3 **e** **não** chama `Add`/`Update` (verificar com `Times.Never`).
   - *(Recomendado)* segundo teste: caminho feliz persiste uma vez (`Times.Once`).
5. **O que não é obrigatório neste CP:** teste de controller com `WebApplicationFactory`, teste de EF InMemory, E2E Selenium. Podem fazer como extra, sem substituir os testes de Domain/Application.
6. Comando no README: `dotnet test` (a partir da solution). Todos os testes **passando**.

### D) README e evidências

Atualizar o **`README.md`** da raiz com:

- Nome e RM dos integrantes.
- Domínio e SGBD (herdados).
- Como subir a API (igual CP3) **e** as URLs: Swagger, `/health`.
- Quais checks entram em `/health`.
- Como rodar os testes.
- Tabela de mapeamento de exceções do CP3 **permanece**.

Em **`/docs/`** (recomendado, ajuda na correção):

- Print ou trecho JSON de `/health` **Healthy**.
- Print ou trecho de `/health` **Unhealthy** (banco parado) **ou** evidência com connection string inválida em launchSettings **local** (não commitada com segredo), se o grupo não conseguir “derrubar” o SGBD.
- Trecho de log (console) de um POST/PUT com `traceId`, e/ou de uma exceção tratada pelo handler.
- Saída de `dotnet test` (ou print do Test Explorer).

---

## 🧱 Restrições (o que NÃO fazer)

- ❌ Não remover o que foi entregue no **CP2** e no **CP3** (DbContext, migrations, controllers, DTOs, Swagger, `IRepository<T>`, `GlobalExceptionHandler`).
- ❌ Controllers **continuam sem** `DbContext`.
- ❌ Health check **não** é `GET` de listagem de negócio (`/api/...`) nem `EnsureCreated()` no startup.
- ❌ Não é necessário (nem será cobrado) separar `/alive` e `/ready`.
- ❌ Não commitar **credenciais reais**.
- ❌ Não é necessário (nem será cobrado) `/metrics`, Prometheus, Jaeger, Seq nem OpenTelemetry (Metrics/Tracing).
- ❌ Testes de Domain **não** podem referenciar Infrastructure ou API.
- ❌ Não substituir testes por “compilou” ou só prints manuais da API.
- ✅ Foco em **health check**, **logs** e **testes da pirâmide** (unidade de domínio + unidade de aplicação).

---

## 🗂️ Entregáveis (no GitHub público)

- Solução do CP3 **atualizada** com:
  - Health checks em **`GET /health`** + writer JSON.
  - Logs estruturados com `traceId` (`HttpContext.TraceIdentifier`).
  - Projetos `*.Domain.Tests` e `*.Application.Tests` na `.sln`.
- **`README.md`** na raiz atualizado (execução, rota `/health`, testes).
- **`/docs/`** com evidências (prints/JSON).
- A entrega no portal continua sendo **somente o link do Git** (não enviar ZIP do código).

---

## 🏅 Avaliação (até 10 pontos)

| Critério | Pontos |
|----------|--------|
| **Health checks** — `self` + banco, `GET /health`, JSON, HTTP 200/503 coerentes | até **2,5** |
| **Observabilidade** — logs estruturados + `traceId` no handler e em pelo menos um fluxo de escrita; correlação visível | até **2,5** |
| **Testes de Domain** — xUnit, AAA, `[Fact]` + `[Theory]`, regra de negócio real, sem mock e sem Infrastructure | até **2,5** |
| **Testes de Application** — mock de repositório, cenário de falha sem persistir, `dotnet test` verde, projetos na solution | até **2,5** |

---

## 🌟 Propósito

> “Faça o teu melhor, na condição que você tem, enquanto você não tem condições melhores, para fazer melhor ainda”  
> — Mario Sergio Cortella

---

## 📎 Relação com os CPs anteriores

| CP1 | CP2 | CP3 | CP4 |
|-----|-----|-----|-----|
| MER + entidades em C# | Esquema físico + EF Core + migrations | API REST + Swagger + repositório genérico + ProblemDetails | **Health checks**, **logs** e **testes** sobre o mesmo sistema |
| Sem banco | Banco configurado | Endpoints exercitando o banco | **Health check do banco** em `/health` + logs das rotas |
| Sem persistência | Repositórios | `IRepository<T>` na API | Mocks das **mesmas** interfaces nos testes |
| — | Validação mínima | Swagger + `GlobalExceptionHandler` | Handler **loga** com `traceId`; erros de domínio **cobertos** por teste |

### Checklist rápido (continuidade do CP3)

- [ ] Migrations, `DbContext`, Swagger e `GlobalExceptionHandler` intactos.
- [ ] `GET /health` **200** com a API e o banco ok; JSON com cada check nomeado.
- [ ] `GET /health` **503** com banco inacessível.
- [ ] Log de um POST/PUT com propriedades + `traceId`; exceção logada no handler.
- [ ] `Domain.Tests`: Fact + Theory numa regra do domínio.
- [ ] `Application.Tests`: mock + `Times.Never` no caminho de erro.
- [ ] `dotnet test` passa; README com URLs e comandos.