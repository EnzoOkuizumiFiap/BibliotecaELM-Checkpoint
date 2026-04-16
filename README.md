# 📌 BibliotecaELM - Checkpoint 02

**Integrantes do Grupo:**
- [Seu Nome Aqui] - RM: [Seu RM Aqui]
- [Nome do Integrante 2 (Se houver)] - RM: [RM Aqui]
- [Nome do Integrante 3 (Se houver)] - RM: [RM Aqui]

## 🎯 Sobre o Projeto (Domínio Escolhido)
Este projeto é uma API em .NET desenvolvida seguindo os princípios de **Clean Architecture**, abordando o domínio de uma **Biblioteca**. O sistema gerencia o serviço clássico de empréstimos (locação de acervo físico) e transações de compras/aquisição de livros em definitivo pelos usuários.

Este projeto iniciou-se no **Checkpoint 01 (CP1)** focado na modelagem do Domínio (MER) e agora evoluiu no **Checkpoint 02 (CP2)** com a inclusão de acesso a dados (**Entity Framework Core**), camada de **Infrastructure**, Mapeamento Fluent API e criação do banco por meio de **Migrations**.

---

## 🧱 Arquitetura e Estrutura do Projeto (Checkpoin 02)

Esta entrega foi refatorada para comportar persistência e a lógica em camadas separadas:
1. **API (`BibliotecaELM.API`)**: Controladores e injeção de dependência (`Program.cs`).
2. **Application (`BibliotecaELM.Application`)**: Serviços (interfaces de repositório) e DTOs.
3. **Domain (`BibliotecaELM.Domain`)**: Entidades de domínio originárias do CP1 e classe base `BaseEntity`.
4. **Infrastructure (`BibliotecaELM.Infrastructure`)**: 
   - `DbContext` (`BibliotecaElmContext`) persistindo o contexto com **Oracle**.
   - Mapeamentos das entidades usando **Fluent API** (ex. `IEntityTypeConfiguration<T>`).
   - Implementações correspondentes aos Repositórios Genéricos / Agregados.
   - Migrations do Entity Framework.

### 💾 Persistência e Banco de Dados
Para o escopo do **CP2**, utilizamos:
- **SGBD**: Banco de Dados **Oracle** (`Oracle.EntityFrameworkCore`).
- **ORM Configurado**: Entity Framework Core 9/10.

#### Como Executar e aplicar as Migrations:
1. Certifique-se de configurar a *Connection String* do Oracle (`BibliotecaElmOracle`) em `appsettings.Development.json` no projeto da API. *(Nota: as credenciais reais de banco não foram comitadas por segurança).*
2. Pelo terminal (na pasta principal ou do projeto web), aplique a migration no banco:
   ```bash
   dotnet ef database update --project BibliotecaELM.Infrastructure --startup-project BibliotecaELM.API
   ```

---

## 📚 Entidades Modeladas
Todas as entidades listadas abaixo (originadas no CP1) implementam a classe abstrata `BaseEntity` utilizando o identificador único padrão (`Id` do tipo `Guid`).

- **Usuario**: Representa os leitores/clientes da biblioteca.
- **Endereco**: Representa a localização de residência do usuário.
- **Livro**: Representa as obras literárias e físicas da biblioteca.
- **Autor**: Representa os escritores responsáveis pelas obras.
- **Emprestimo**: Representa o ato transacional (histórico) onde o usuário leva o livro temporariamente com prazos definidos.
- **Compra**: Representa a transação comercial onde o usuário adquire livros em definitivo.

---

## 🔗 Resumo dos Relacionamentos

Baseado na modelagem e devidamente mapeados com EF Core no CP2:

- **Usuario (1) ↔ (1) Endereco**
  - Relacionamento 1:1 obrigatório no modelo atual. Cada usuário possui exatamente um endereço e cada endereço pertence a exatamente um usuário (FK `UsuarioId` com índice único em `BD_Addresses`).
- **Usuario (1) ↔ (N) Emprestimo**
  - Relacionamento 1:N obrigatório. Um usuário pode ter vários empréstimos e todo empréstimo exige um usuário vinculado (`UsuarioId` obrigatório em `BD_Loans`).
- **Usuario (1) ↔ (N) Compra**
  - Relacionamento 1:N obrigatório. Um usuário pode efetuar inúmeras compras e toda compra exige um usuário vinculado (`UsuarioId` obrigatório em `BD_Purchases`).
- **Livro (N) ↔ (N) Emprestimo**
  - Relacionamento N:N implementado por tabela de junção `BD_LoanBooks`.
- **Autor (1) ↔ (N) Livro**
  - Relacionamento 1:N obrigatório. Um autor possui vários livros e todo livro exige um autor (`AutorId` obrigatório em `BD_Books`).
- **Compra (N) ↔ (N) Livro**
  - Relacionamento N:N implementado por tabela de junção `BD_PurchaseBooks`.