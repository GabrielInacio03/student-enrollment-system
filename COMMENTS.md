# COMMENTS.md

## 🏗️ Decisão da arquitetura utilizada

### Frontend (Vue 3 + Composition API)

Optei por Vue 3 com Composition API para melhor modularização e controle da lógica reativa. A estrutura foi organizada em:

- **Components**: elementos reutilizáveis da interface
- **Views**: páginas principais
- **Services**: abstração de chamadas à API
- **Store (Pinia)**: gerenciamento de estado
- **Router**: navegação entre rotas

Essa arquitetura facilita escalabilidade, testes e manutenção.

### Backend (.NET 8 + Repository Pattern)

Implementei o backend com ASP.NET Core 8, utilizando o **Repository Pattern** para separar a lógica de acesso a dados da lógica de negócio. A estrutura inclui:

- **Controllers**: endpoints da API
- **DTOs**: transporte de dados entre camadas
- **Models**: representação das entidades
- **Repositories**: interfaces e implementações para acesso a dados
- **AppDbContext**: contexto do Entity Framework
- **Migrations**: controle de versão do banco de dados

Essa abordagem melhora a testabilidade, reutilização e desacoplamento entre camadas.

---

## 📦 Bibliotecas de terceiros utilizadas

### Frontend

- `axios` – requisições HTTP
- `bootstrap` – estilização responsiva
- `pinia` – gerenciamento de estado
- `vue-router` – navegação entre páginas
- `vite` – bundler moderno e rápido

### Backend

- `Entity Framework Core` – ORM para acesso ao banco
- `Swashbuckle` – geração de documentação Swagger
- `AutoMapper` – mapeamento entre DTOs e entidades

---

## ⏳ O que eu melhoraria se tivesse mais tempo

### Frontend

- Implementaria testes unitários com `Vitest`
- Melhoraria acessibilidade e responsividade
- Revisaria a arquitetura do projeto

### Backend

- Implementaria autenticação com JWT
- Criaria testes automatizados com `xUnit`
- Adicionaria logs estruturados com `Serilog`
- Melhoraria tratamento de erros com middlewares personalizados

---

## ❌ Requisitos obrigatórios que não foram entregues

- [ ] Autenticação de usuários (não especificado no escopo original)
- [ ] Testes automatizados (por limitação de tempo)
- [ ] Upload de arquivos (caso fosse parte do desafio)

---
