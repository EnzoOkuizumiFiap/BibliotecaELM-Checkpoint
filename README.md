# 📌 BibliotecaELM - Checkpoint 04

## 🎯 Sobre o Projeto (Domínio Escolhido)
Este projeto é uma API em .NET desenvolvida seguindo os princípios de **Clean Architecture**, abordando o domínio de uma **Biblioteca**. O sistema gerencia o serviço clássico de empréstimos (locação de acervo físico) e transações de compras/aquisição de livros em definitivo pelos usuários.

O projeto foi totalmente evoluído para o **Checkpoint 04 (CP4)**, tornando a aplicação operável em runtime com **Health Checks** (usando ASP.NET Core), adicionando observabilidade via **Logs Estruturados com TraceId**, e garantindo a resiliência do domínio e da aplicação através de **Testes Automatizados com xUnit e Moq**.

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
* **Resposta Formatada (JSON)**: Resposta customizada que retorna um payload estruturado detalhando o status global e o resultado individual dos componentes verificados.

### 2. Observabilidade e Logging com TraceId / Correlation ID
* **Injeção de `ILogger<T>`**: Injetado nos serviços de aplicação (`LivroAppService`, etc.) e na camada de API.
* **Logs Estruturados**: Eventos cruciais da aplicação (como criação de recursos, atualizações e validações falhas) gravam dados estruturados usando parâmetros nomeados (`NomeLivro`, `LivroId`, `AutorId`).
* **Correlation ID / TraceId**: Todas as mensagens de log e respostas de exceção correlacionam requisições utilizando o `TraceId` extraído de `HttpContext.TraceIdentifier` (ou via header `X-Trace-Id`), garantindo rastreabilidade fim a fim.
* **Filtro de Erros (`GlobalExceptionHandler`)**: Atualizado para incluir o `TraceId` no payload no formato `ProblemDetails` (`RFC 7807`) e registrar a exceção no log antes do envio ao cliente.

### 3. Testes Unitários Automatizados (xUnit + Moq)
A solução possui projetos dedicados para validação automatizada sem dependências externas reais:

* **Domain Tests (`BibliotecaELM.Domain.Tests`)**:
  * **Testes de Entidade (`LivroTests`)**: Utilizam os atributos `[Fact]` para validação de instanciação válida e `[Theory]` / `[InlineData]` para cenários de borda.
  * **Isolamento de Domínio**: Validação rigorosa das exceções customizadas de domínio (`BusinessRuleValidationException`) disparadas pelas regras de negócio, sem a utilização de Mocks.
* **Application Tests (`BibliotecaELM.Application.Tests`)**:
  * **Testes de Serviços (`LivroAppServiceTests`)**: Utilizam **Moq** (`Mock<ILivroRepository>`) para simular o comportamento de persistência e verificar a orquestração do fluxo.
  * **Verificação de Regras de Fluxo**: Testam se a aplicação lança exceções para entradas inválidas (ex: `AutorId` nulo) e asseguram que o repositório **nunca** seja chamado em cenários de falha (`Times.Never`), e chamado **exatamente uma vez** (`Times.Once`) em cenários de sucesso.

---

## 🧱 Arquitetura e Estrutura do Projeto

O projeto segue os princípios de Clean Architecture, organizado nas seguintes camadas:

1. **Domain (`BibliotecaELM.Domain`)**: Entidades de domínio (Autor, Livro, Usuario, Endereco, Compra, Emprestimo), exceções de negócio (`BusinessRuleValidationException`) e classe base `BaseEntity`. Livre de dependências externas.
2. **Application (`BibliotecaELM.Application`)**: DTOs (`Request` e `Response`), interfaces de repositórios e serviços, e a camada de implementação (`LivroAppService`, etc.) contendo validações e logs com `TraceId`.
3. **Infrastructure (`BibliotecaELM.Infrastructure`)**: Implementação do repositório genérico `Repository<T>`, repositórios concretos, contexto de persistência `BibliotecaElmContext` (Oracle) e as Migrations do EF Core.
4. **API (`BibliotecaELM.API`)**: Ponto de entrada da aplicação com Controllers, endpoints de Health Check (`/health`), pipeline de middlewares, `GlobalExceptionHandler` e documentação Swagger.
5. **Tests (`BibliotecaELM.Domain.Tests` e `BibliotecaELM.Application.Tests`)**: Suítes de testes unitários com xUnit e Moq.

---

## 🧪 Como Executar os Testes Automatizados

A partir do diretório raiz do projeto, execute o comando abaixo no terminal para rodar todas as suítes de testes da solução:

```bash
dotnet test