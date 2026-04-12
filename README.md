# 📌 CP2 — Persistência com EF Core, Mapeamento e Camada de Infraestrutura

## 🎯 Objetivo

1. **Persistir o modelo COMPLETO do MER** usando **Entity Framework Core** (.NET 9/10), respeitando o **Clean Architecture**:
  - `DbContext` na camada **Infrastructure**.
  - **Mapeamento** das entidades (Fluent API).
  - **Relacionamentos** fiéis ao CP1: cardinalidade, opcionalidade, chaves primárias/estrangeiras e índices quando fizer sentido (ex.: unicidade, FKs).

2. **Migrations** versionadas no repositório:
  - Pelo menos **uma migration inicial** que materialize o esquema completo (no máximo duas migrations, se houver justificativa explícita no README).
  - Banco de dados à escolha do grupo, desde que a execução seja **reproduzível** (ex.: **SQLite** para simplicidade; **SQL Server**, **PostgreSQL**, **Oracle** ou **MySQL** com instruções claras no README).

3. **Padrão de acesso a dados**:
  - Interfaces de repositório na camada **Application**
  - Implementações na **Infrastructure**.
  - **Injeção de dependência** registrada no **Program.cs** do projeto **API**.

4. **Configuração segura**:
  - Connection string em `appsettings` (e `User Secrets` / variáveis de ambiente para dados sensíveis em desenvolvimento).
  - **Não** commitar senhas ou segredos reais.

---

## 👥 Forma de Trabalho

- O trabalho deverá ser realizado **em grupo** com até **3 integrantes**.
- Cada grupo deverá entregar **um único repositório** no GitHub.
- Somente **um integrante** deverá entregar o link no portal do aluno.

---

## 🧭 Escopo (o que fazer)

1. Adicionar os pacotes NuGet necessários ao **EF Core** e ao **provider** do banco escolhido (e `Microsoft.EntityFrameworkCore.Design` no projeto correto para gerar migrations).
2. Criar o **`DbContext`** (ex.: `ApplicationDbContext`) incluindo **todas** as entidades modeladas no CP1.
3. Implementar o mapeamento (**Fluent API** com `IEntityTypeConfiguration<T>` e/ou **Data Annotations**):
  - **Mínimo obrigatório:** mapear explicitamente todas as entidades que participam de relacionamentos **N:N** ou com **opcionalidade** que não seja óbvia só pelas propriedades.
4. Gerar **migration(es)** e garantir que o banco seja criado/atualizado com sucesso (ex.: `dotnet ef database update`).
5. Implementar **repositório genérico** *ou* **repositórios por agregado** — **uma** estratégia, aplicada de forma consistente.
6. Registrar serviços no container de DI (**AddDbContext**, repositórios/UoW).
7. **Validação do cenário** — **uma** das opções (definir em conjunto com o professor):
  - **(A)** Apenas camada de dados + README com comandos e evidência (print da ferramenta do banco ou do esquema gerado); **ou**
  - **(B)** Incluir **um endpoint mínimo** (ex.: `GET` de health + **um** `GET` que demonstre leitura no banco, com **seed** opcional de dados de exemplo).

---

## 🧱 Restrições (o que NÃO fazer)

- ❌ Nada de regras de negócio complexas na **Infrastructure** (foco em persistência, mapeamento e acesso a dados).
- ❌ Não commitar **credenciais reais** nem connection strings com segredos.
- ✅ Foco em **EF Core**, **migrations**, **mapeamento** e **organização** em camadas.

---

## 🗂️ Entregáveis (no GitHub público)

- Solução atualizada com **Infrastructure** contendo `DbContext`, implementações de repositório e pasta de **Migrations**.
- **`README.md`** na raiz **atualizado** com:
  - Nome e RM dos integrantes do grupo.
  - Domínio escolhido (pode resumir o do CP1).
  - **Qual SGBD** foi usado
- **`/docs/`** (recomendado): diagrama ou print do **esquema físico** no banco (ou atualização do MER se o modelo evoluiu, com breve justificativa).
- A entrega no portal continua sendo **somente o link do Git** (não enviar ZIP do código).

---

## 🏅 Avaliação (até 10 pontos)

| Critério | Pontos |
|----------|--------|
| **Mapeamento EF** — Fluent API/anotações, tipos, nullability, relacionamentos fiéis ao MER | até **3,0** |
| **Migrations e banco** — migration aplicável sem erros, esquema coerente | até **2,5** |
| **Clean Architecture** — DbContext e implementações na Infrastructure; contratos claros; DI correto na API | até **2,5** |
| **Repositórios* — interfaces bem definidas, uso consistente, sem acoplamento indevido | até **2,0** |

---

## 🌟 Propósito

> “Faça o teu melhor, na condição que você tem, enquanto você não tem condições melhores, para fazer melhor ainda”  
> — Mario Sergio Cortella

---

## 📎 Relação com o CP1

| CP1 | CP2 |
|-----|-----|
| MER + entidades em C# | Esquema físico + EF Core + migrations |
| Sem banco de dados | Banco configurado e reproduzível |
| Sem persistência | Repositórios/UoW + DI |

---