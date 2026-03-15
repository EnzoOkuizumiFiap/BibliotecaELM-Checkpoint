# 📚 BibliotecaELM - Checkpoint 01

Projeto WebAPI em .NET 10 seguindo princípios de Clean Architecture.

Este projeto foi desenvolvido como parte do **Checkpoint 01 (CP1)**, focado exclusivamente na elaboração do **Modelo Entidade-Relacionamento (MER)** e na **modelagem das Entidades de Domínio utilizando C#** (sem implementação de banco de dados, CRUD, controllers ou endpoints).

---

## 🏢 O Domínio Escolhido

O domínio escolhido para este projeto é o de uma **Biblioteca**, que abrange não apenas o serviço clássico de **empréstimos de acervo**, mas também a **gestão de compras e aquisição de livros pelos usuários**.

---

## 📦 Entidades Modeladas

As seguintes entidades principais foram identificadas e modeladas no código seguindo a camada de **Domínio (`BibliotecaELM.Domain/Entities`)**:

- **Usuario**: Representa os leitores/clientes da biblioteca.
- **Endereco**: Representa a localização de residência do usuário.
- **Livro**: Representa as obras literárias e físicas da biblioteca.
- **Autor**: Representa os escritores responsáveis pelas obras.
- **Emprestimo**: Representa o ato transacional (histórico) onde o usuário leva o livro temporariamente com prazos definidos.
- **Compra**: Representa a transação comercial onde o usuário adquire livros em definitivo.

Todas as entidades herdam de uma **classe abstrata `BaseEntity`**, estabelecendo a padronização de **identificadores únicos (`Id` como tipo `GUID`)**.

---

## 🔗 Resumo dos Relacionamentos

Baseado na modelagem construída no código, estabelecemos as seguintes **cardinalidades e opcionalidades**:

- **Usuario (1) ↔ (1) Endereco**  
  Relacionamento **1:1 obrigatório**. Todo usuário precisa possuir um endereço associado.

- **Usuario (1) ↔ (N) Emprestimo**  
  Relacionamento **1:N**. Um único usuário pode ter o histórico de vários empréstimos.  
  Todo empréstimo requer obrigatoriamente um **Usuário vinculado**.

- **Usuario (1) ↔ (N) Compra**  
  Relacionamento **1:N**. Um usuário pode efetuar inúmeras compras.  
  Cada compra pertence restritamente a um **usuário obrigatório**.

- **Livro (1) ↔ (N) Emprestimo**  
  Relacionamento **1:N**. Um mesmo livro (obra) pode aparecer em inúmeros registros de empréstimos ao longo do tempo.

- **Autor (1) ↔ (N) Livro**  
  Relacionamento **1:N**. Um autor compôs **um ou múltiplos livros** cadastrados no acervo da biblioteca.

- **Compra (1) ↔ (N) Livro**  
  Relacionamento modelado como **listas de agregação** (onde uma **Compra engloba uma coleção de Livros**).

---

## 🗺️ Modelo Entidade-Relacionamento (MER)

O diagrama de **Entidade-Relacionamento** correspondente a essa modelagem de domínio encontra-se no arquivo abaixo:

*(Caso a imagem não seja renderizada, verifique os arquivos em `/docs/mer.png` ou `/docs/mer.pdf`)*

---

## 👥 Integrantes da Equipe (Autores e Créditos)

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