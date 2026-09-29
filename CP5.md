# 📌 CP5 — Versionamento de API, Paginação e Rate Limit
## 🎯 Objetivo
1. **Evoluir o contrato HTTP** de um recurso já entregue no CP3, sem quebrar quem ainda consome o contrato antigo:
   - Conviver **duas versões** do mesmo recurso.
   - A versão antiga fica **deprecada** (aviso, não corte).
   - A versão nova muda o formato da **listagem**.
2. **Paginar a listagem na versão nova**:
   - O `GET` de lista deixa de devolver o banco inteiro.
   - A página é cortada **no banco** (`Skip` / `Take` no `IQueryable`), com teto de `pageSize`.
   - A resposta é um **envelope** com totais, não só o array.
3. **Limitar a taxa** de pelo menos um endpoint já existente:
   - Política **fixed window** no pipeline do ASP.NET Core.
   - Estouro responde **429** com **`Retry-After`**.
   - **`GET /health`** (CP4) permanece de fora do teto.
Manter **Clean Architecture**: versionamento, paginação HTTP e rate limit na composição da **API** e no contrato de **Application** / **Infrastructure**. Regra de negócio continua no **Domain**. Versionar o JSON **não** duplica o serviço de aplicação.
---
## 👥 Forma de Trabalho
- O trabalho deverá ser realizado **em grupo** com até **3 integrantes** (mesmo grupo do CP1–CP4).
- Cada grupo deverá entregar **um único repositório** no GitHub (**evolução** do repositório do CP4).
- Somente **um integrante** deverá entregar o link no portal do aluno.
---
## 🧭 Escopo (o que fazer)
Escolham **um** recurso que já tenha `GET` de listagem no CP3 (o que mais cresce no domínio de vocês: pedidos, produtos, avaliações, matrículas, etc.). Esse recurso é o único que precisa ganhar versão e página. Os demais endpoints do CP3 **continuam no ar**.
### A) Versionamento (dois contratos, o mesmo recurso)
1. Pacotes na **API**:
   - `Asp.Versioning.Mvc`
   - `Asp.Versioning.Mvc.ApiExplorer`
2. Registrar `AddApiVersioning()` + `AddMvc()` + `AddApiExplorer()` na DI da **API**, com:
   - **`DefaultApiVersion = 2.0`**
   - **`AssumeDefaultVersionWhenUnspecified = true`** (sem versão na requisição, cai na **2.0**)
   - **`ReportApiVersions = true`** (a resposta envia `api-supported-versions` e `api-deprecated-versions`)
   - **`ApiVersionReader.Combine`** com, no mínimo, estes dois leitores:
     - query string **`api-version`**
     - header **`X-Api-Version`**
3. **v1 (deprecada)** — o `GET` de listagem que já existia:
   - `[ApiVersion("1.0", Deprecated = true)]`
   - o `GET` de lista devolve o **contrato antigo** (array / lista, como no CP3)
   - a action de lista mapeada para a **1.0** (`[MapToApiVersion("1.0")]` ou equivalente)
4. **v2 (atual)** — o mesmo recurso, contrato novo:
   - `[ApiVersion("2.0")]`
   - o `GET` de lista devolve o **envelope paginado** da seção B
5. Os dois contratos chamam o **mesmo** serviço de aplicação. Não copiar regra de domínio para “uma service da v1 e outra da v2”.
6. **Swagger** (Development): um documento por versão (`GroupNameFormat` no estilo `'v'VVVV`, ou equivalente). A descrição do documento da **v1** deixa explícito que a versão está **deprecada**. A UI permite trocar entre os grupos.
7. Os outros recursos do CP3 **seguem chamáveis**. Se sumirem do Swagger ao ligar o ApiExplorer, marquem `[ApiVersionNeutral]` ou declarem uma versão neles e expliquem no README. Sumir endpoint antigo não conta como versionamento.
**Como o cliente escolhe a versão** (os três têm de funcionar):
```http
GET /api/{recurso}?api-version=1.0
```
```http
GET /api/{recurso}
X-Api-Version: 1.0
```
```http
GET /api/{recurso}
```
O terceiro (sem versão) cai na **2.0**.
**Validação:** v1 devolve a lista antiga; v2 devolve o envelope; os headers `api-supported-versions` e `api-deprecated-versions` aparecem na resposta. Anotar no README as URLs exatas do recurso escolhido.
*(Recomendado)* Leitor de segmento de URL (`v{version:apiVersion}`), para `GET /api/v1/{recurso}` e `GET /api/v2/{recurso}`. Não substitui query e header: os dois continuam obrigatórios.
*(Recomendado)* `GET` por id, `POST`, `PUT` e `DELETE` também mapeados na versão em que o cliente vai usá-los. Com default **2.0**, um `POST` que só existe na 1.0 **quebra** se o cliente não mandar versão. O fluxo de escrita do CP3 tem de continuar funcionando — na 2.0, na 1.0 com versão explícita, ou nos dois. Documentem no README qual chamada a correção deve usar.
### B) Paginação (só na listagem v2)
A v1 **não** pagina: ela preserva o contrato antigo. Paginar o array da v1 seria breaking change silencioso — é exatamente o que o versionamento evita.
1. Query string da listagem **v2**:
   | Parâmetro | Padrão | Regra |
   |-----------|--------|--------|
   | `page` | `1` | inteiro ≥ 1 |
   | `pageSize` | `20` | inteiro de **1 a 100** |
2. `page < 1` ou `pageSize` fora de **1–100** → **400**. O corpo deixa a regra explícita (mensagem dizendo o que falhou). Preferir **Problem Details** (`application/problem+json`), no mesmo espírito do CP3. `BadRequest` com mensagem clara também é aceito.
3. Página além do total → **200** com `items` vazio. Não é erro.
4. Corpo **200** da v2 (nomes destes campos):
```json
{
  "page": 1,
  "pageSize": 20,
  "totalItems": 137,
  "totalPages": 7,
  "items": []
}
```
   `totalPages` = teto de `totalItems / pageSize`. Com `pageSize <= 0` isso não chega a ser calculado: a requisição já foi **400**.
5. Onde cada camada entra:
   - **Controller:** lê `page` e `pageSize`.
   - **Application:** valida o intervalo (tipo dedicado, método do serviço, ou os dois) e pede a página ao repositório. O DTO do envelope fica em **Application**, não na entidade.
   - **Infrastructure:** `GetPaged` no `IRepository<T>` (ou no repositório específico desse recurso) executa `Count` + `OrderBy` + `Skip` + `Take` no **`IQueryable`**, e só então materializa (`ToList`).
6. **`OrderBy` é obrigatório** (por exemplo `CreatedAt` ou o campo estável que o grupo já usa). Sem ordenação, página 1 e página 2 não são reproduzíveis.
7. `GetById` **não** pagina. Paginar em memória (`GetAll()` e depois `Skip` na lista) **não** atende este CP.
*(Recomendado)* `hasPrevious` e `hasNext` no envelope.
*(Recomendado)* Um `[Theory]` + `[InlineData]` no projeto `*.Application.Tests` cobrindo a regra de `page` / `pageSize` inválido, e um `[Fact]` no intervalo válido. Sem subir API nem banco. Os testes do CP4 continuam na solution e **verdes**.
**Validação:** com dados suficientes (semeiem localmente se preciso; `pageSize=2` basta):
- página 1 e página 2 **não se sobrepõem**;
- `totalPages` fecha com o total;
- `page=0` e `pageSize=9999` respondem **400**;
- `page` enorme responde **200** com `items: []`.
### C) Rate limit (teto de frequência)
Paginação limita o **tamanho** de cada resposta. Rate limit limita **quantas** respostas o mesmo cliente pede na janela.
1. Middleware **nativo** (`Microsoft.AspNetCore.RateLimiting`). Não é obrigatório pacote de terceiros.
2. `AddRateLimiter` com:
   - `RejectionStatusCode = 429`
   - uma política **fixed window** nomeada (`AddFixedWindowLimiter` ou `FixedWindowRateLimiter` por partição)
   - `OnRejected` (ou equivalente) que devolve:
     - header **`Retry-After`** (segundos até tentar de novo)
     - corpo **JSON** (Problem Details ou objeto com `status: 429`)
3. `UseRateLimiter()` **depois** de `UseExceptionHandler()` e **antes** de `MapControllers()`.
4. A política entra em **pelo menos um** endpoint de escrita já existente (`POST` ou `PUT`), via `[EnableRateLimiting("...")]` (controller ou action).
5. Números **documentados no README** (limite e janela). O teto precisa ser estourável na correção sem script: sugestão **10 requisições por minuto** nesse `POST`/`PUT`. Não coloquem `PermitLimit = 1` na listagem inteira — o Swagger e o teste da paginação ficam inutilizáveis.
6. **`GET /health` não entra no teto.** Se a política for global, `DisableRateLimiting` nesse endpoint. Depois de estourar o `POST`, `/health` continua **200**.
*(Recomendado)* Partição por IP (`PartitionedRateLimiter` + `RemoteIpAddress`). Sem autenticação, IP é a chave didática. Usuário autenticado fica para quando houver auth.
*(Recomendado)* Headers `X-RateLimit-Limit`, `X-RateLimit-Remaining` e `X-RateLimit-Reset` também nas respostas que **passam**.
*(Recomendado)* Política mais branda na listagem `GET` v2, separada da política do `POST`.
**Validação:** disparar o `POST`/`PUT` limitado até receber **429**, com `Retry-After` e corpo JSON. Guardar o trecho em `/docs/`. Em seguida, `GET /health` ainda **200**.
### D) README e evidências
Atualizar o **`README.md`** da raiz com:
- Nome e RM dos integrantes.
- Domínio e SGBD (herdados).
- Como subir a API (igual CP4) e as URLs: Swagger, `/health`, **listagem v1**, **listagem v2**.
- Como informar a versão (query, header, omissão → 2.0).
- Parâmetros de paginação, padrões e o teto de `pageSize`.
- Qual endpoint tem rate limit, o limite, a janela e o que acontece no 429.
- Como rodar os testes (`dotnet test`).
- Tabela de mapeamento de exceções do CP3 **permanece**.
Em **`/docs/`**:
- JSON do `GET` **v1** (lista) e do `GET` **v2** (envelope), no mesmo recurso.
- Trecho dos headers `api-supported-versions` e `api-deprecated-versions`.
- Print ou trecho do Swagger com os dois grupos e a v1 marcada como deprecada.
- **400** de `page` ou `pageSize` inválido.
- Página 1 e página 2 (ou `totalItems` / `totalPages` coerentes com `pageSize` pequeno).
- **429** com `Retry-After` no endpoint limitado.
- `GET /health` **200** depois desse estouro (prova de que o probe não divide o teto).
- Saída de `dotnet test` (os testes do CP4 e, se fizerem, o da paginação).
---
## 🧱 Restrições (o que NÃO fazer)
- ❌ Não remover o que foi entregue no **CP2**, **CP3** e **CP4** (DbContext, migrations, controllers, DTOs, Swagger, `IRepository<T>`, `GlobalExceptionHandler`, `GET /health`, logs com `traceId`, projetos `*.Domain.Tests` e `*.Application.Tests`).
- ❌ Controllers **continuam sem** `DbContext`.
- ❌ Não versionar a API inteira. **Um** recurso basta. Os outros continuam funcionando.
- ❌ Não duplicar regra de negócio por versão. O serviço de aplicação é compartilhado.
- ❌ Não paginar a v1. A lista antiga permanece lista.
- ❌ Não paginar em memória depois de `ToList()` / `GetAll()`.
- ❌ Não tratar página fora do intervalo como 404. É **200** com `items` vazio.
- ❌ Não aplicar o rate limit em `/health`.
- ❌ Não commitar **credenciais reais**.
- ❌ Não é necessário (nem será cobrado) autenticação, API key, `/metrics`, Prometheus, Jaeger, Seq nem OpenTelemetry.
- ❌ Não substituir este CP por “o Swagger abriu” sem os dois contratos, o 400 da página e o 429.
- ✅ Foco em **convivência de contratos**, **página cortada no banco** e **teto de taxa** com `/health` intacto.
---
## 🗂️ Entregáveis (no GitHub público)
- Solução do CP4 **atualizada** com:
  - Versões **1.0 (deprecada)** e **2.0** de um recurso, leitores por query e header, Swagger por versão.
  - Listagem **v2** paginada (`page`, `pageSize`, envelope, `GetPaged` no repositório).
  - Rate limit **fixed window** com **429** + **`Retry-After`**, e `/health` de fora.
- **`README.md`** na raiz atualizado (URLs das duas versões, paginação, política de taxa, testes).
- **`/docs/`** com as evidências da seção D.
- A entrega no portal continua sendo **somente o link do Git** (não enviar ZIP do código).
---
## 🏅 Avaliação (até 10 pontos)
| Critério | Pontos |
|----------|--------|
| **Versionamento** — v1 deprecada (lista) e v2 (envelope), query `api-version` + header `X-Api-Version`, omissão cai na 2.0, headers de versão, Swagger com os dois grupos | até **3,5** |
| **Paginação** — `page` / `pageSize` com padrão e teto, 400 fora da faixa, 200 com página vazia, envelope com totais, `Skip`/`Take` no `IQueryable` com `OrderBy` | até **4,0** |
| **Rate limit** — fixed window em um POST ou PUT, 429 + `Retry-After` + corpo JSON, `/health` segue 200 | até **2,5** |
---
## 🌟 Propósito
> “Faça o teu melhor, na condição que você tem, enquanto você não tem condições melhores, para fazer melhor ainda”  
> — Mario Sergio Cortella
---
## 📎 Relação com os CPs anteriores
| CP1 | CP2 | CP3 | CP4 | CP5 |
|-----|-----|-----|-----|-----|
| MER + entidades em C# | Esquema físico + EF Core + migrations | API REST + Swagger + repositório genérico + ProblemDetails | Health checks, logs e testes | **Versão**, **página** e **taxa** sobre o mesmo sistema |
| Sem banco | Banco configurado | `GET` devolve a lista inteira | `/health` observa o processo e o banco | A listagem **v2** devolve **uma página**; a **v1** fica como estava |
| Sem persistência | Repositórios | `IRepository<T>.GetAll()` | Mocks das mesmas interfaces | `GetPaged` no mesmo repositório, cortando no SQL |
| — | Validação mínima | Swagger + `GlobalExceptionHandler` | Handler loga com `traceId` | Swagger **por versão**; 400 da página e 429 do teto convivem com o handler |
### O que a correção executa
1. `GET` do recurso **sem** versão → **200**, envelope da **v2** (não o array da v1).
2. `GET` com `?api-version=1.0` → **200**, **lista** (contrato antigo).
3. `GET` com header `X-Api-Version: 1.0` → **200**, a mesma lista.
4. Resposta com `api-supported-versions` e `api-deprecated-versions`.
5. Swagger em Development lista **v1.0** e **v2.0**; a v1 aparece como deprecada.
6. `page=0` e `pageSize=9999` na v2 → **400** com mensagem da regra.
7. `page=1` e `page=2` com `pageSize` pequeno → itens distintos; `totalPages` coerente com `totalItems`.
