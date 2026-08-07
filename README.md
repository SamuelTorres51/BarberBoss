# BarberBoss

API REST para gestão financeira de barbearias. O BarberBoss permite que equipes de barbearia registrem e acompanhem faturamentos de serviços — barbeiro, cliente, valor, forma de pagamento e status — com autenticação segura e relatórios mensais em Excel para administradores.

---

## Objetivo do Projeto

O BarberBoss é um projeto com foco em demonstrar na prática a aplicação de tecnologias, padrões de design e abordagens de arquitetura de software. O domínio de gestão financeira de barbearias serve como contexto realista para exercitar conceitos como Arquitetura Limpa, DDD, casos de uso, repositórios, autenticação JWT, validações, mapeamento de objetos e persistência com EF Core.

Não se trata de um produto inovador destinado ao uso em produção, e sim de uma referência técnica que mostra como estruturar um backend .NET de forma organizada, testável e alinhada a boas práticas de mercado.

Principais capacidades:

- **Usuários** — cadastro, login, perfil, alteração de senha e exclusão de conta
- **Faturamentos** — CRUD completo de registros de serviços (barbeiro, cliente, serviço, valor, data, pagamento e observações)
- **Relatórios** — exportação mensal em `.xlsx` (restrita a administradores)
- **Autenticação** — JWT com controle de papéis (`TeamMember` e `Administrator`)

---

## Arquitetura

O projeto segue **Arquitetura Limpa (Clean Architecture)**, com dependências apontando sempre para o centro (Domínio). A organização em camadas separa responsabilidades e facilita testes, manutenção e evolução do sistema.

```
┌─────────────────────────────────────────────────────────┐
│                    BarberBoss.API                       │
│         Controllers · JWT · Swagger · Health          │
└─────────────────────────┬───────────────────────────────┘
                          │
┌─────────────────────────▼───────────────────────────────┐
│               BarberBoss.Application                    │
│      Use Cases · FluentValidation · AutoMapper          │
└─────────────────────────┬───────────────────────────────┘
                          │
┌─────────────────────────▼───────────────────────────────┐
│                  BarberBoss.Domain                      │
│   Entidades · Enums · Interfaces de Repositório         │
└─────────────────────────▲───────────────────────────────┘
                          │
┌─────────────────────────┴───────────────────────────────┐
│              BarberBoss.Infrastructure                  │
│     EF Core · MySQL · Repositórios · JWT · BCrypt       │
└─────────────────────────────────────────────────────────┘

  BarberBoss.Communication ── DTOs de entrada e saída da API
  BarberBoss.Exception     ── Exceções customizadas e mensagens
```

### Camadas

| Camada | Projeto | Responsabilidade |
|--------|---------|------------------|
| **Apresentação** | `BarberBoss.API` | Endpoints REST, autenticação JWT, Swagger, health check |
| **Aplicação** | `BarberBoss.Application` | Casos de uso, validações e mapeamento entre DTOs e entidades |
| **Domínio** | `BarberBoss.Domain` | Entidades, enums e contratos (interfaces) — sem dependências externas |
| **Infraestrutura** | `BarberBoss.Infrastructure` | Persistência (EF Core + MySQL), repositórios, criptografia e tokens |
| **Comunicação** | `BarberBoss.Communication` | Request/Response JSON da API |
| **Exceções** | `BarberBoss.Exception` | Tipos de erro e mensagens centralizadas (`.resx`) |

### Padrões adotados

**Domain-Driven Design (DDD)** — conceitos táticos aplicados de forma pragmática:

- **Entidades** — `User` e `Billing` como núcleo do domínio
- **Enums de domínio** — `PaymentMethod` (Cartão, Dinheiro, Pix, Outro), `Status` (Pago, Cancelado), `Roles`
- **Repositórios** — interfaces no Domínio, implementações na Infraestrutura
- **Separação CQRS nos repositórios** — contratos segregados (`ReadOnly`, `WriteOnly`, `UpdateOnly`)
- **Unit of Work** — transações coordenadas via `IUnityOfWork`
- **Serviços de domínio (interfaces)** — abstrações para usuário logado, criptografia de senha e geração de tokens
- **Casos de uso** — cada operação de negócio isolada em uma classe (`RegisterBillingUseCase`, `DoLoginUseCase`, etc.)

**Outros padrões:**

- **Inversão de dependência** — o Domínio define contratos; a Infraestrutura os implementa
- **Composition Root** — injeção de dependência centralizada em `Program.cs`, `AddApplication()` e `AddInfrastructure()`
- **Validação na camada de Aplicação** — FluentValidation com mensagens localizadas
- **Controllers enxutos** — delegam toda a lógica aos casos de uso

---

## Estrutura do Repositório

```
BarberBoss/
├── src/
│   ├── BarberBoss.API/              # Camada de apresentação
│   ├── BarberBoss.Application/      # Casos de uso e validações
│   ├── BarberBoss.Communication/    # DTOs (Requests / Responses)
│   ├── BarberBoss.Domain/           # Entidades e interfaces
│   ├── BarberBoss.Exception/        # Exceções e mensagens de erro
│   └── BarberBoss.Infrastructure/   # Persistência, segurança e serviços
├── tests/
│   └── UseCases.Test/               # Testes unitários e de integração
├── Dockerfile
└── BarberBoss.slnx
```

---

## Tecnologias

| Categoria | Tecnologia |
|-----------|------------|
| **Runtime** | .NET 10 |
| **Framework Web** | ASP.NET Core |
| **ORM** | Entity Framework Core 10 |
| **Banco de dados** | MySQL |
| **Autenticação** | JWT Bearer |
| **Criptografia** | BCrypt |
| **Validação** | FluentValidation |
| **Mapeamento** | AutoMapper |
| **Relatórios** | ClosedXML (Excel) |
| **Documentação da API** | Swagger / OpenAPI |
| **Testes** | xUnit, FluentAssertions, EF Core InMemory |
| **Containerização** | Docker |

---

## Como Executar

### Pré-requisitos

- [.NET SDK 10.0]
- MySQL 

### Configuração

Crie o arquivo `src/BarberBoss.API/appsettings.Development.json` (não versionado por segurança):

```json
{
  "ConnectionStrings": {
    "Connection": "Server=localhost;Database=BarberBoss;User=root;Password=sua_senha;"
  },
  "Settings": {
    "Jwt": {
      "SigningKey": "sua-chave-secreta-com-pelo-menos-32-caracteres",
      "ExpiresMinutes": 60
    }
  }
}
```

### Build e execução

```powershell
cd src/BarberBoss.API
dotnet restore
dotnet run
```

- Swagger (ambiente Development): `/swagger`

As migrations do EF Core são aplicadas automaticamente na inicialização.

### Testes

```powershell
cd tests/UseCases.Test
dotnet test
```

---

### Muitos são os planos no coração do homem, mas o que prevalece é o propósito do senhor. Provérbios 19:21