# 📌 BibliotecaELM - Checkpoint 04

## 🎯 Sobre o Projeto (Domínio Escolhido)
Este projeto é uma API em .NET desenvolvida seguindo os princípios de **Clean Architecture**, abordando o domínio de uma **Biblioteca**. O sistema gerencia o serviço clássico de empréstimos (locação de acervo físico) e transações de compras/aquisição de livros em definitivo pelos usuários.

O projeto foi totalmente evoluído para o **Checkpoint 04 (CP4)**, tornando ela operável em runtime com health checks (usando ASP .NET Core), adicionando logs com TraceId e adicionando proteção ao Domain usando testes automatizados com xUnit e testando Application com Moq.

## Integrantes da Equipe

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

## 🧭 Escopo do Checkpoint 03 (Entregas e Melhorias)

### 1. Repositório Genérico (`IRepository<T>` & `Repository<T>`)
* **Interface `IRepository<T>`** (`BibliotecaELM.Application/Services/Interfaces/IRepository.cs`): Define o contrato de CRUD genérico para as entidades que herdam de `BaseEntity`.
  * Operações: `GetAll`, `GetById`, `Add`, `Update`, `Delete` e `ExistsById`.
* **Classe `Repository<T>`** (`BibliotecaELM.Infrastructure/Repositories/Repository.cs`): Implementa o contrato genérico utilizando o Entity Framework Core com o contexto `BibliotecaElmContext`. Aplica `.AsNoTracking()` em consultas de listagem para otimização de performance.
* **Injeção de Dependência**: Registrado no container com escopo scoped:
  ```csharp
  builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
  ```

### 2. Camada de Serviços de Aplicação (Application Services)
* **Arquitetura de Serviços**: Criamos a camada de `Services` dividida em `Interfaces/` e `Implementations/` para orquestração, regras de negócio e mapeamento DTO:
  * Interfaces em `BibliotecaELM.Application/Services/Interfaces/`: `IAutorService`, `ILivroService`, `IUsuarioService`, `IEnderecoService`, `ICompraService`, `IEmprestimoService`.
  * Implementações em `BibliotecaELM.Application/Services/Implementations/`: `AutorService`, `LivroService`, `UsuarioService`, `EnderecoService`, `CompraService`, `EmprestimoService`.
* **Desacoplamento de DTOs**: Os controllers agora chamam apenas os serviços que mapeiam e tratam DTOs de entrada/saída, enquanto os repositórios focam estritamente no tráfego de entidades de domínio.

### 3. Tratamento Global de Erros (`GlobalExceptionHandler`)
* **Classe `GlobalExceptionHandler`** (`BibliotecaELM.API/Exceptions/GlobalExceptionHandler.cs`): Implementa `IExceptionHandler` do ASP.NET Core para interceptar exceções não tratadas em toda a aplicação.
* **Respostas Padronizadas (RFC 7807)**: Retorna um objeto JSON no formato `ProblemDetails` (`application/problem+json`) ocultando detalhes confidenciais em ambientes não de desenvolvimento.
* **Mapeamento de Exceções**:
  
  | Exceção | Código HTTP | Descrição / Título |
  | :--- | :--- | :--- |
  | `BusinessRuleValidationException` / `ArgumentException` | `400 Bad Request` | Requisição Inválida / Regra de Negócio |
  | `ResourceNotFoundException` | `404 Not Found` | Recurso não encontrado |
  | Outras Exceções não tratadas | `500 InternalServerError` | Erro interno do servidor |

* **Limpeza dos Controllers**: Os blocos `try/catch` foram removidos de todos os endpoints das Controllers, deixando o pipeline limpo e transferindo a responsabilidade de erro para o Handler.

### 4. Swagger Completo e Comentários XML
* **Swagger/OpenAPI** (`Swashbuckle.AspNetCore`): Configurado com metadados personalizados (Título, Versão, Descrição da API).
* **Comentários XML**: Habilitados no build do projeto API e expostos na interface gráfica do Swagger. Todos os endpoints de todas as controllers possuem documentação XML enriquecida.
* **Anotações de Resposta**: Uso das diretivas `[ProducesResponseType]` em todas as rotas para expor de antemão os status codes mapeados (sucessos e erros comuns).
* **Acesso na Raiz**: O Swagger UI serve diretamente na raiz `/` (ex. `http://localhost:<porta>/`), melhorando a acessibilidade para testes.

---

## 🧱 Arquitetura e Estrutura do Projeto

O projeto segue os princípios de Clean Architecture, organizado nas seguintes camadas:
1. **Domain (`BibliotecaELM.Domain`)**: Entidades de domínio (Autor, Livro, Usuario, Endereco, Compra, Emprestimo) e classe base `BaseEntity`. Livre de dependências externas.
2. **Application (`BibliotecaELM.Application`)**: DTOs (`Request` e `Response`) e a camada de Serviços de Aplicação (`Interfaces/` e `Implementations/`), que orquestram a lógica da aplicação, mapeamentos e validações de fluxo.
3. **Infrastructure (`BibliotecaELM.Infrastructure`)**: Implementação do repositório genérico `Repository<T>`, repositórios específicos focados em banco de dados, contexto de persistência `BibliotecaElmContext` (Oracle) e as Migrations do EF Core.
4. **API (`BibliotecaELM.API`)**: Ponto de entrada da aplicação, contendo os Controllers, a configuração da injeção de dependência (`Program.cs`) e o tratamento global de erros (`GlobalExceptionHandler`).

### 💾 Persistência e Banco de Dados
* **SGBD**: Banco de Dados **Oracle** (`Oracle.EntityFrameworkCore`).
* **ORM Configurado**: Entity Framework Core 10.

#### Como Executar e aplicar as Migrations:
> Execute os comandos abaixo a partir da raiz do repositorio.

1. Inicialize o User Secrets no projeto da API (uma vez por maquina):
   ```bash
   dotnet user-secrets init --project BibliotecaELM/BibliotecaELM.API
   ```
2. Configure a connection string do Oracle no User Secrets (sem commitar senha no repositorio):
   ```bash
   dotnet user-secrets set "ConnectionStrings:BibliotecaElmOracle" "Data Source=oracle.fiap.com.br:1521/orcl;User ID=SEU_USUARIO;Password=SUA_SENHA;" --project BibliotecaELM/BibliotecaELM.API
   ```
3. Pelo terminal, aplique a migration no banco:
   ```bash
   dotnet ef database update --project BibliotecaELM/BibliotecaELM.Infrastructure --startup-project BibliotecaELM/BibliotecaELM.API
   ```

### 🧾 Estratégia de Migrations
Para manter o histórico enxuto e aderente, o projeto adota uma migration consolidada para o esquema final do MER no checkpoint.

**Se precisar recriar do zero em ambiente local:**
```bash
dotnet ef migrations remove --project BibliotecaELM/BibliotecaELM.Infrastructure --startup-project BibliotecaELM/BibliotecaELM.API
# repetir enquanto houver migrations pendentes
dotnet ef migrations add InitialCp3 --project BibliotecaELM/BibliotecaELM.Infrastructure --startup-project BibliotecaELM/BibliotecaELM.API
dotnet ef database update --project BibliotecaELM/BibliotecaELM.Infrastructure --startup-project BibliotecaELM/BibliotecaELM.API
```

---

## 📚 Entidades Modeladas
Todas as entidades implementam a classe abstrata `BaseEntity` utilizando o identificador único padrão (`Id` do tipo `Guid`).

* **Usuario**: Representa os leitores/clientes da biblioteca.
* **Endereco**: Representa a localização de residência do usuário.
* **Livro**: Representa as obras literárias e físicas da biblioteca.
* **Autor**: Representa os escritores responsáveis pelas obras.
* **Emprestimo**: Representa o ato transacional onde o usuário leva o livro temporariamente com prazos definidos.
* **Compra**: Representa a transação comercial onde o usuário adquire livros em definitivo.

---

## 🔗 Resumo dos Relacionamentos
Mapeados declarativamente com EF Core:

* **Usuario (1) ↔ (1) Endereco**
  * Relacionamento 1:1 com endereço opcional no usuário. O usuário pode existir sem endereço, mas cada endereço pertence a exatamente um usuário (FK `UsuarioId` com índice único em `BD_Addresses`).
* **Usuario (1) ↔ (N) Emprestimo**
  * Relacionamento 1:N obrigatório. Um usuário pode ter vários empréstimos e todo empréstimo exige um usuário vinculado (`UsuarioId` obrigatório em `BD_Loans`).
* **Usuario (1) ↔ (N) Compra**
  * Relacionamento 1:N obrigatório. Um usuário pode efetuar inúmeras compras e toda compra exige um usuário vinculado (`UsuarioId` obrigatório em `BD_Purchases`).
  * Regra de negócio: usuário sem endereço não pode realizar compra.
* **Livro (N) ↔ (N) Emprestimo**
  * Relacionamento N:N implementado por tabela de junção `BD_LoanBooks`.
* **Autor (1) ↔ (N) Livro**
  * Relacionamento 1:N obrigatório. Um autor possui vários livros e todo livro exige um autor (`AutorId` obrigatório em `BD_Books`).
* **Compra (N) ↔ (N) Livro**
  * Relacionamento N:N implementado por tabela de junção `BD_PurchaseBooks`.

## ✅ Regras de Negócio Implementadas
* Cadastro de usuário permite endereço opcional.
* No update de usuário, quando `endereco` vier `null`, o endereço atual é mantido sem alteração.
* Compras são bloqueadas para usuários sem endereço cadastrado.
* Compras não aceitam `dataCompra` futura.
* Empréstimos não aceitam `dataEmprestimo` ou `dataDevolucao` futuras.
* Autor possui validação de faixa para `nascimento` (1500 a 2026).

---

## 🗂️ Evidências de Banco e Migrations

### MER
![MER](docs/mer.png)

### Tabela de histórico de migrations
![Tabela _EFMigrationsHistory](docs/Tabela%20_EFMigrationHistory.png)

## Estrutura das tabelas principais

### Tabela BD_User
![Tabela BD_Users](docs/Tabela%20BD_Users.png)

### Tabela BD_Address
![Tabela BD_Address](docs/Tabela%20BD_Address.png)

### Tabela Purchases
![Tabela BD_Purchases](docs/Tabela%20BD_Purchases.png)

### Tabela BD_Loans
![Tabela BD_Loans](docs/Tabela%20BD_Loans.png)

### Tabela BD_Books
![Tabela BD_Books](docs/Tabela%20BD_Books.png)

### Tabela BD_Authors
![Tabela BD_Authors](docs/Tabela%20BD_Authors.png)

### Tabela BD_PurchasesBooks
![Tabela BD_PurchasesBooks](docs/Tabela%20BD_PurchasesBooks.png)

### Tabela BD_LoanBooks
![Tabela BD_LoanBooks](docs/Tabela%20BD_LoanBooks.png)

## 🖼️ Evidências de Testes (Insomnia)

### Usuario

**POST**

![Post Usuario](docs/Post%20Usuario.png)

**GET**

![Get Usuario](docs/Get%20Usuario.png)

**GET By Id**

![Get Usuario By Id](docs/Get%20Usuario%20By%20Id.png)

**PUT**

![Update Usuario](docs/Update%20Usuario.png)

**DELETE**

![Delete Usuario](docs/Delete%20Usuario.png)

### Endereco

**POST**

![Post Endereco](docs/Post%20Endereco.png)

**GET**

![Get Endereco](docs/Get%20Endereco.png)

**GET By Id**

![Get Endereco By Id](docs/Get%20Endereco%20By%20Id.png)

**PUT**

![Update Endereco](docs/Update%20Endereco.png)

**DELETE**

![Delete Endereco](docs/Delete%20Endereco.png)

### Compra

**POST (válido)**

![Post Compra](docs/Post%20Compra.png)

**POST (bloqueio sem endereço)**

![Post Compra Sem Endereco](docs/Post%20Compra%20Sem%20Endereco.png)

**GET**

![Get Compra](docs/Get%20Compra.png)

**GET By Id**

![Get Compra By Id](docs/Get%20Compra%20By%20Id.png)

**PUT**

![Update Compra](docs/Update%20Compra.png)

**DELETE**

![Delete Compra](docs/Delete%20Compra.png)

### Emprestimo

**POST**

![Post Emprestimo](docs/Post%20Emprestimo.png)

**GET**

![Get Emprestimo](docs/Get%20Emprestimo.png)

**GET By Id**

![Get Emprestimo By Id](docs/Get%20Emprestimo%20By%20Id.png)

**PUT**

![Update Emprestimo](docs/Update%20Emprestimo.png)

**DELETE**

![Delete Emprestimo](docs/Delete%20Emprestimo.png)

### Livro

**POST**

![Post Livro](docs/Post%20Livro.png)

**GET**

![Get Livro](docs/Get%20Livro.png)

**GET By Id**

![Get Livro By Id](docs/Get%20Livro%20By%20Id.png)

**PUT**

![Update Livro](docs/Update%20Livro.png)

**DELETE**

![Delete Livro](docs/Delete%20Livro.png)

### Autor

**POST**

![Post Autor](docs/Post%20Autor.png)

**GET**

![Get Autor](docs/Get%20Autor.png)

**GET By Id**

![Get Autor By Id](docs/Get%20Autor%20By%20Id.png)

**PUT**

![Update Autor](docs/Update%20Autor.png)

**DELETE**

![Delete Autor](docs/Delete%20Autor.png)

