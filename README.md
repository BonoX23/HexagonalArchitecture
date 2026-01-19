<p align="center">
  <img src="https://img.shields.io/badge/.NET-6.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 6.0"/>
  <img src="https://img.shields.io/badge/C%23-10.0-239120?style=for-the-badge&logo=c-sharp&logoColor=white" alt="C# 10"/>
  <img src="https://img.shields.io/badge/SQL%20Server-2019-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white" alt="SQL Server"/>
  <img src="https://img.shields.io/badge/Entity%20Framework-6.0-512BD4?style=for-the-badge&logo=nuget&logoColor=white" alt="EF Core"/>
  <img src="https://img.shields.io/badge/MediatR-CQRS-FF6B6B?style=for-the-badge" alt="MediatR"/>
</p>

<h1 align="center">Hotel Booking Service</h1>

<p align="center">
  <strong>Sistema de Reservas de Hotel</strong><br/>
  Demonstração de Arquitetura Hexagonal (Ports & Adapters) com CQRS
</p>

<p align="center">
  <a href="#-sobre-o-projeto">Sobre</a> •
  <a href="#-funcionalidades">Funcionalidades</a> •
  <a href="#%EF%B8%8F-arquitetura">Arquitetura</a> •
  <a href="#-padrões-de-projeto">Padrões</a> •
  <a href="#-tecnologias">Tecnologias</a> •
  <a href="#-como-executar">Como Executar</a> •
  <a href="#-endpoints">Endpoints</a>
</p>

---

## 📋 Sobre o Projeto

Este projeto é uma **API REST** para gerenciamento de reservas de hotel, desenvolvida com foco em demonstrar conhecimentos avançados em **Arquitetura de Software** e **boas práticas de desenvolvimento**.

O sistema foi construído seguindo os princípios da **Arquitetura Hexagonal (Ports & Adapters)** combinada com o padrão **CQRS (Command Query Responsibility Segregation)**, garantindo:

- **Separação clara de responsabilidades** entre camadas
- **Independência de frameworks** e tecnologias externas
- **Alta testabilidade** com isolamento de dependências
- **Escalabilidade** e facilidade de manutenção
- **Domain-Driven Design (DDD)** aplicado ao núcleo do negócio

---

## ✨ Funcionalidades

### Gestão de Hóspedes (Guests)
- Cadastro de hóspedes com validação de documentos
- Consulta de hóspedes por ID
- Validação de e-mail e documentos de identificação

### Gestão de Quartos (Rooms)
- Cadastro de quartos com preços em múltiplas moedas
- Controle de disponibilidade e manutenção
- Suporte a diferentes níveis/andares

### Gestão de Reservas (Bookings)
- Criação de reservas com validação de disponibilidade
- Consulta de reservas por ID
- **Máquina de Estados** para controle do ciclo de vida da reserva:
  ```
  Created → Paid → Finished
     ↓        ↓
  Canceled ← Refunded
  ```

### Processamento de Pagamentos
- Integração com provedores de pagamento (MercadoPago)
- Suporte a múltiplos métodos de pagamento
- Padrão Factory para seleção de processadores

---

## 🏗️ Arquitetura

O projeto implementa a **Arquitetura Hexagonal (Ports & Adapters)**, também conhecida como "Clean Architecture", organizando o código em camadas bem definidas:

```
┌─────────────────────────────────────────────────────────────────┐
│                        CONSUMERS (API)                          │
│                     Controllers REST API                        │
├─────────────────────────────────────────────────────────────────┤
│                         ADAPTERS                                │
│            Implementações de Repositórios (EF Core)             │
│                  Adaptadores de Pagamento                       │
├─────────────────────────────────────────────────────────────────┤
│                     APPLICATION (CORE)                          │
│              Commands & Queries (CQRS/MediatR)                  │
│                   Application Services                          │
│                         DTOs                                    │
├─────────────────────────────────────────────────────────────────┤
│                       DOMAIN (CORE)                             │
│                 Entidades & Agregados                           │
│                     Value Objects                               │
│                   Regras de Negócio                             │
│               Ports (Interfaces/Contratos)                      │
└─────────────────────────────────────────────────────────────────┘
```

### Estrutura de Pastas

```
HexagonalArchitecture/
│
├── BookingService/
│   │
│   ├── Core/                          # Núcleo da aplicação
│   │   ├── Domain/                    # Camada de Domínio
│   │   │   ├── Booking/
│   │   │   │   └── Ports/             # Interfaces (Contratos)
│   │   │   ├── Guest/
│   │   │   │   ├── Entities/          # Entidades de domínio
│   │   │   │   ├── Enums/             # Enumeradores
│   │   │   │   ├── Exceptions/        # Exceções de domínio
│   │   │   │   ├── Ports/             # Interfaces (Contratos)
│   │   │   │   └── ValueObjects/      # Objetos de Valor
│   │   │   └── Room/
│   │   │       ├── Entities/
│   │   │       ├── Ports/
│   │   │       └── ValueObjects/
│   │   │
│   │   └── Application/               # Camada de Aplicação
│   │       ├── Booking/
│   │       │   ├── Commands/          # CQRS - Comandos
│   │       │   ├── Queries/           # CQRS - Consultas
│   │       │   ├── Dtos/
│   │       │   └── Responses/
│   │       ├── Guest/
│   │       ├── Room/
│   │       └── Payment/
│   │
│   ├── Adapters/                      # Adaptadores externos
│   │   └── Data/                      # Persistência (EF Core)
│   │       ├── Migrations/
│   │       ├── GuestRepository.cs
│   │       ├── RoomRepository.cs
│   │       ├── BookingRepository.cs
│   │       └── HotelDbContext.cs
│   │
│   ├── Consumers/                     # Pontos de entrada
│   │   └── API/                       # REST API
│   │       └── Controllers/
│   │
│   └── Tests/                         # Testes automatizados
│       ├── Domain/
│       ├── Application/
│       └── Adapters/
│
└── PaymentService/                    # Serviço de Pagamento
    └── Core/Application/
        └── MercadoPago/               # Adapter MercadoPago
```

### Fluxo de uma Requisição

```
┌──────────┐    ┌────────────┐    ┌─────────────┐    ┌────────────┐    ┌──────────┐
│ HTTP     │───▶│ Controller │───▶│   MediatR   │───▶│  Handler   │───▶│  Domain  │
│ Request  │    │   (API)    │    │  (Command/  │    │ (Use Case) │    │ (Entity) │
└──────────┘    └────────────┘    │   Query)    │    └────────────┘    └──────────┘
                                  └─────────────┘           │
                                                            ▼
┌──────────┐    ┌────────────┐    ┌─────────────┐    ┌────────────┐
│ HTTP     │◀───│ Controller │◀───│   Response  │◀───│ Repository │
│ Response │    │   (API)    │    │    (DTO)    │    │  (Adapter) │
└──────────┘    └────────────┘    └─────────────┘    └────────────┘
```

---

## 🎯 Padrões de Projeto

### Arquiteturais

| Padrão | Aplicação |
|--------|-----------|
| **Hexagonal Architecture** | Organização em camadas com Ports & Adapters |
| **CQRS** | Separação entre Commands (escrita) e Queries (leitura) |
| **Clean Architecture** | Dependências apontando para o centro (Domain) |
| **Domain-Driven Design** | Modelagem rica do domínio com entidades e value objects |

### Design Patterns

| Padrão | Aplicação |
|--------|-----------|
| **Repository Pattern** | Abstração do acesso a dados via interfaces |
| **Factory Pattern** | Criação de processadores de pagamento |
| **State Machine** | Controle de transições de estado da reserva |
| **Mediator Pattern** | Desacoplamento via MediatR |
| **DTO Pattern** | Transferência de dados entre camadas |
| **Value Object** | Encapsulamento de conceitos (Price, PersonId) |

### Princípios SOLID Aplicados

- **S**ingle Responsibility: Cada classe tem uma única responsabilidade
- **O**pen/Closed: Extensível via novas implementações de interfaces
- **L**iskov Substitution: Interfaces permitem substituição de implementações
- **I**nterface Segregation: Interfaces específicas por contexto
- **D**ependency Inversion: Dependência de abstrações, não de implementações

---

## 🚀 Tecnologias

### Backend
- **.NET 6.0** - Framework principal
- **C# 10** - Linguagem de programação
- **ASP.NET Core Web API** - Framework para APIs REST

### Persistência
- **Entity Framework Core 6.0** - ORM
- **SQL Server** - Banco de dados relacional
- **Code First Migrations** - Versionamento do schema

### Padrões & Bibliotecas
- **MediatR 10.0** - Implementação do Mediator Pattern para CQRS
- **Swagger/OpenAPI** - Documentação interativa da API

### Testes
- **NUnit** - Framework de testes
- **Moq** - Biblioteca de mocking
- **AutoFixture** - Geração automática de dados de teste

---

## 📦 Pré-requisitos

Antes de executar o projeto, certifique-se de ter instalado:

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB, Express ou superior)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

---

## ⚙️ Como Executar

### 1. Clone o Repositório

```bash
git clone https://github.com/seu-usuario/HexagonalArchitecture.git
cd HexagonalArchitecture
```

### 2. Configure a Connection String

Edite o arquivo `BookingService/Consumers/API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HotelManagement;Trusted_Connection=True;"
  }
}
```

### 3. Instale as Ferramentas do Entity Framework

```bash
dotnet tool install --global dotnet-ef --version 6.0.28
```

### 4. Crie o Banco de Dados

```bash
dotnet ef database update \
  --project BookingService/Adapters/Data/Data.csproj \
  --startup-project BookingService/Consumers/API/API.csproj
```

**Ou via Visual Studio (Package Manager Console):**
```powershell
Update-Database -Project Data
```

### 5. Execute a Aplicação

```bash
cd BookingService/Consumers/API
dotnet run
```

A API estará disponível em:
- **HTTPS:** https://localhost:7245
- **HTTP:** http://localhost:5122
- **Swagger:** https://localhost:7245/swagger

---

## 🔌 Endpoints

### Hóspedes (Guests)

#### Criar Hóspede
```http
POST /Guests
Content-Type: application/json

{
  "name": "João",
  "surname": "Silva",
  "email": "joao.silva@email.com",
  "idNumber": "123456789",
  "idTypeCode": 1
}
```

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `name` | string | Nome do hóspede |
| `surname` | string | Sobrenome |
| `email` | string | E-mail válido |
| `idNumber` | string | Número do documento |
| `idTypeCode` | int | 1 = Passaporte, 2 = CNH |

#### Buscar Hóspede
```http
GET /Guests?guestId=1
```

---

### Quartos (Rooms)

#### Criar Quarto
```http
POST /Room
Content-Type: application/json

{
  "name": "Suíte Master",
  "level": 5,
  "inMaintenance": false,
  "price": 450.00,
  "currency": 0
}
```

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `name` | string | Nome/identificação do quarto |
| `level` | int | Andar do quarto |
| `inMaintenance` | bool | Em manutenção? |
| `price` | decimal | Valor da diária |
| `currency` | int | 0 = Dólar, 1 = Euro, 2 = Bitcoin |

---

### Reservas (Bookings)

#### Criar Reserva
```http
POST /Booking
Content-Type: application/json

{
  "start": "2024-03-01T14:00:00",
  "end": "2024-03-05T12:00:00",
  "roomId": 1,
  "guestId": 1
}
```

#### Buscar Reserva
```http
GET /Booking?id=1
```

#### Processar Pagamento
```http
POST /Booking/1/Pay
Content-Type: application/json

{
  "paymentIntention": "full_payment",
  "selectedPaymentProvider": 4,
  "selectedPaymentMethod": 2
}
```

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `paymentIntention` | string | Intenção do pagamento |
| `selectedPaymentProvider` | int | 1 = PayPal, 2 = Stripe, 3 = PagSeguro, 4 = MercadoPago |
| `selectedPaymentMethod` | int | 1 = Débito, 2 = Crédito, 3 = Transferência |

---

## 🧪 Executando os Testes

```bash
# Todos os testes
dotnet test

# Testes de Domínio
dotnet test BookingService/Tests/Domain/DomainTests/DomainTests.csproj

# Testes de Aplicação
dotnet test BookingService/Tests/Application/ApplicationTests/ApplicationTests.csproj
```

---

## 📊 Diagrama de Entidades

```
┌─────────────────┐       ┌─────────────────┐       ┌─────────────────┐
│     GUEST       │       │    BOOKING      │       │      ROOM       │
├─────────────────┤       ├─────────────────┤       ├─────────────────┤
│ Id              │       │ Id              │       │ Id              │
│ Name            │◀──────│ GuestId (FK)    │       │ Name            │
│ Surname         │       │ RoomId (FK)     │──────▶│ Level           │
│ Email           │       │ PlacedAt        │       │ InMaintenance   │
│ DocumentId (VO) │       │ Start           │       │ Price (VO)      │
│  - IdNumber     │       │ End             │       │  - Value        │
│  - DocumentType │       │ Status          │       │  - Currency     │
└─────────────────┘       └─────────────────┘       └─────────────────┘
```

---

## 🔄 Máquina de Estados - Booking

```
                    ┌─────────┐
                    │ CREATED │
                    └────┬────┘
                         │ Pay()
                    ┌────▼────┐
          Cancel()  │  PAID   │
       ┌────────────┴────┬────┘
       │                 │ Finish()
  ┌────▼─────┐     ┌─────▼─────┐
  │ CANCELED │     │ FINISHED  │
  └────▲─────┘     └─────┬─────┘
       │                 │ Refund()
       │           ┌─────▼─────┐
       └───────────│ REFUNDED  │
         Reopen()  └───────────┘
```

---

## 👨‍💻 Autor

<table>
  <tr>
    <td align="center">
      <strong>Ricardo Bono da Silva</strong>
      <br/>
      Desenvolvedor .NET
    </td>
  </tr>
</table>

Desenvolvido com dedicação para demonstrar conhecimentos em:
- Arquitetura de Software
- Padrões de Projeto
- Desenvolvimento .NET
- Boas práticas de código

---

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

```
MIT License - Copyright (c) 2024 Ricardo Bono da Silva
```

---

<p align="center">
  <strong>⭐ Se este projeto foi útil, considere dar uma estrela!</strong>
</p>
