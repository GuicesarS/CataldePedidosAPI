# 📦 CataldePedidosAPI - Sistema de Gerenciamento de Pedidos

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-10.0-239120?logo=c-sharp)](https://docs.microsoft.com/dotnet/csharp/)
[![Tests](https://img.shields.io/badge/Tests-35%20passados-brightgreen)]()
[![Coverage](https://img.shields.io/badge/Coverage-Multicamada-success)]()

Projeto desenvolvido como **teste técnico para Catalde**, aplicando **Clean Architecture**, **DDD**, **Repository Pattern**, **Value Objects** e boas práticas. Inclui autenticação JWT, CRUD de pedidos e ocorrências, logging e cobertura completa de testes automatizados.

---

## ⚙️ Funcionalidades

* 🔐 **JWT com validação de token, claims e expiração** (admin / 123456) – implementada de forma prática para testes
* 📦 CRUD completo de pedidos e ocorrências
* ✅ Regras de negócio:

  * Impede duplicidade de ocorrências em menos de 10 minutos
  * Marca automaticamente a ocorrência finalizadora
  * Atualiza status do pedido (`IndEntregue`) conforme resultado
* 🧠 Value Object para `NumeroPedido`
* 📄 Swagger com suporte a autenticação
* 📈 Logging com Serilog (console, arquivo e SQL Server)
* 🌱 Seed de dados inicial e migrations configuradas
* 🧪 Testes unitários e de integração com xUnit, FluentAssertions e Moq

---

## 🛠️ Endpoints da API

### Autenticação

* **POST `/api/login`** – Gera token JWT para testes

**Request:**

```json
{
  "usuario": "admin",
  "senha": "123456"
}
```

**Response:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

---

### Pedidos

* **GET `/api/Pedidos`** – Lista todos os pedidos

**Response 200 OK:**

```json
[
  {
    "idPedido": 1,
    "numeroPedido": 1001,
    "horarioPedido": "2025-10-02T18:39:47.452Z",
    "indEntregue": true,
    "ocorrencias": [
      {
        "idOcorrencia": 1,
        "tipoOcorrencia": "EntregueComSucesso",
        "horaOcorrencia": "2025-10-02T18:39:47.452Z",
        "indFinalizadora": true
      }
    ]
  }
]
```

* **GET `/api/Pedidos/{id}`** – Retorna um pedido pelo ID

**Response 200 OK:**

```json
{
  "idPedido": 1,
  "numeroPedido": 1001,
  "horarioPedido": "2025-10-02T18:39:47.456Z",
  "indEntregue": true,
  "ocorrencias": [
    {
      "idOcorrencia": 1,
      "tipoOcorrencia": "EntregueComSucesso",
      "horaOcorrencia": "2025-10-02T18:39:47.456Z",
      "indFinalizadora": true
    }
  ]
}
```

**Response 404 Not Found:**

```json
{
  "type": "string",
  "title": "Pedido não encontrado",
  "status": 404,
  "detail": "O pedido com o ID informado não existe."
}
```

* **POST `/api/Pedidos`** – Cria um novo pedido

**Request:**

```json
{
  "numeroPedido": 1003
}
```

**Response 201 Created:**

```json
{
  "idPedido": 3,
  "numeroPedido": 1003,
  "horarioPedido": "2025-10-02T18:39:47.454Z",
  "indEntregue": false,
  "ocorrencias": []
}
```

**Response 400 Bad Request:**

```json
{
  "type": "string",
  "title": "Dados inválidos",
  "status": 400,
  "detail": "O número do pedido deve ser maior que zero."
}
```

---

### Ocorrências

* **POST `/api/Pedidos/{id}/ocorrencia`** – Adiciona uma ocorrência a um pedido

**Request:**

```json
{
  "pedidoId": 1,
  "tipoOcorrencia": "EmRotaDeEntrega"
}
```

**Response 204 No Content** – Ocorrência adicionada com sucesso

**Response 404 Not Found** – Pedido não encontrado

* **DELETE `/api/Ocorrencias/{id}`** – Remove uma ocorrência existente

**Response 204 No Content** – Ocorrência removida com sucesso

**Response 404 Not Found** – Ocorrência ou pedido não encontrado

💡 Todos os endpoints estão testáveis diretamente no Swagger (`/swagger`). O token JWT de teste é suficiente para validar todos os endpoints protegidos.

---

## 🧪 Testes

* Total: **35 testes automatizados**
* Cobertura: Domain, Application, API, Validation
* Testes de integração com autenticação real

```bash
dotnet test
# Resultado esperado: 35 passados, 0 falhas
```

---

## 🚀 Execução (Passo a Passo Seguro)

1. **Clone o repositório**

```bash
git clone https://github.com/GuicesarS/CataldePedidosAPI
cd CataldePedidosAPI
```

2. **Verifique se você tem o .NET 8 SDK instalado**

```bash
dotnet --version
# Deve retornar 8.x.x
```

3. **Verifique o SQL Server e ajuste a connection string**

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=PedidosDb;User Id=sa;Password=SuaSenha;TrustServerCertificate=True;"
}
```

4. **Restaure os pacotes NuGet**

```bash
dotnet restore
```

5. **Crie o banco de dados e aplique migrations**

```bash
dotnet ef database update \
  --project src/backend/Catalde.Pedidos.Infrastructure \
  --startup-project src/backend/Catalde.Pedidos.Api
```

6. **Execute a API**

```bash
dotnet run --project src/backend/Catalde.Pedidos.Api
```

> Caso a porta padrão (`5001`) esteja ocupada, use outra:

```bash
dotnet run --project src/backend/Catalde.Pedidos.Api --urls "https://localhost:5005"
```

7. **Acesse o Swagger**

* Navegue para: `https://localhost:5001/swagger` (ou a porta que você escolheu)
* Teste todos os endpoints da API

---

## 👤 Credenciais de Teste

| Campo   | Valor    |
| ------- | -------- |
| Usuário | `admin`  |
| Senha   | `123456` |

---

## 🧱 Estrutura do Projeto

```
CataldePedidosAPI/
├── src/backend/
│   ├── Catalde.Pedidos.Api/
│   ├── Catalde.Pedidos.Application/
│   ├── Catalde.Pedidos.Domain/
│   └── Catalde.Pedidos.Infrastructure/
├── tests/
│   ├── Catalde.Pedidos.UnitTests/
│   └── Catalde.Pedidos.IntegrationTests/
```

---

## 📌 Status

✅ Projeto finalizado e pronto para avaliação técnica.

---

