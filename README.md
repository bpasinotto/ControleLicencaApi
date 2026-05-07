# API de Controle de Licença - ERP

API REST para gerenciar licenças de clientes em seu sistema ERP.

## 📋 Estrutura do Banco de Dados

A API utiliza a seguinte tabela:

```sql
CREATE TABLE SistemaControle (
    ClienteId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    NomeCliente VARCHAR(100) NOT NULL,
    Bloqueado BIT NOT NULL DEFAULT 0 -- 0 = Liberado, 1 = Bloqueado
);
```

## 🚀 Configuração

### 1. Criar o Banco de Dados

Execute o script SQL acima em seu SQL Server.

### 2. Configurar a String de Conexão

Abra `appsettings.json` e atualize a conexão:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=ControleLicenca;User Id=usuario;Password=senha;"
}
```

### 3. Executar Migrations (opcional)

Se desejar usar migrations do EF Core:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 📡 Endpoints da API

### 1. Listar Todos os Clientes

**GET** `/api/licencas`

```bash
curl -X GET "https://localhost:7001/api/licencas"
```

**Resposta:**
```json
[
    {
        "clienteId": "550e8400-e29b-41d4-a716-446655440000",
        "nomeCliente": "Cliente Teste 01",
        "bloqueado": false
    }
]
```

### 2. Obter Cliente por ID

**GET** `/api/licencas/{id}`

```bash
curl -X GET "https://localhost:7001/api/licencas/550e8400-e29b-41d4-a716-446655440000"
```

**Resposta:**
```json
{
    "clienteId": "550e8400-e29b-41d4-a716-446655440000",
    "nomeCliente": "Cliente Teste 01",
    "bloqueado": false
}
```

### 3. Verificar Status da Licença

**GET** `/api/licencas/{id}/status`

```bash
curl -X GET "https://localhost:7001/api/licencas/550e8400-e29b-41d4-a716-446655440000/status"
```

**Resposta:**
```json
{
    "clienteId": "550e8400-e29b-41d4-a716-446655440000",
    "nomeCliente": "Cliente Teste 01",
    "bloqueado": false,
    "status": "Liberado"
}
```

### 4. Criar Novo Cliente

**POST** `/api/licencas`

```bash
curl -X POST "https://localhost:7001/api/licencas" \
  -H "Content-Type: application/json" \
  -d '{"nomeCliente":"Novo Cliente","bloqueado":false}'
```

**Request Body:**
```json
{
    "nomeCliente": "Novo Cliente",
    "bloqueado": false
}
```

**Resposta (201 Created):**
```json
{
    "clienteId": "550e8400-e29b-41d4-a716-446655440001",
    "nomeCliente": "Novo Cliente",
    "bloqueado": false
}
```

### 5. Atualizar Cliente

**PUT** `/api/licencas/{id}`

```bash
curl -X PUT "https://localhost:7001/api/licencas/550e8400-e29b-41d4-a716-446655440000" \
  -H "Content-Type: application/json" \
  -d '{"nomeCliente":"Cliente Atualizado","bloqueado":false}'
```

**Request Body:**
```json
{
    "nomeCliente": "Cliente Atualizado",
    "bloqueado": false
}
```

**Resposta:**
```json
{
    "clienteId": "550e8400-e29b-41d4-a716-446655440000",
    "nomeCliente": "Cliente Atualizado",
    "bloqueado": false
}
```

### 6. Bloquear Cliente

**POST** `/api/licencas/{id}/bloquear`

```bash
curl -X POST "https://localhost:7001/api/licencas/550e8400-e29b-41d4-a716-446655440000/bloquear"
```

**Resposta:**
```json
{
    "mensagem": "Cliente bloqueado com sucesso",
    "cliente": {
        "clienteId": "550e8400-e29b-41d4-a716-446655440000",
        "nomeCliente": "Cliente Teste 01",
        "bloqueado": true
    }
}
```

### 7. Desbloquear Cliente

**POST** `/api/licencas/{id}/desbloquear`

```bash
curl -X POST "https://localhost:7001/api/licencas/550e8400-e29b-41d4-a716-446655440000/desbloquear"
```

**Resposta:**
```json
{
    "mensagem": "Cliente desbloqueado com sucesso",
    "cliente": {
        "clienteId": "550e8400-e29b-41d4-a716-446655440000",
        "nomeCliente": "Cliente Teste 01",
        "bloqueado": false
    }
}
```

### 8. Deletar Cliente

**DELETE** `/api/licencas/{id}`

```bash
curl -X DELETE "https://localhost:7001/api/licencas/550e8400-e29b-41d4-a716-446655440000"
```

**Resposta:**
```json
{
    "mensagem": "Cliente deletado com sucesso"
}
```

## 📁 Estrutura do Projeto

```
ControleLicenca.Api/
├── Controllers/
│   └── LicencasController.cs      # Endpoints da API
├── Models/
│   └── SistemaControle.cs         # Entidade do cliente
├── Data/
│   └── SeuDbContext.cs            # DbContext
├── Program.cs                      # Configuração da aplicação
├── appsettings.json                # Configurações
└── ControleLicenca.Api.csproj     # Arquivo de projeto
```

## 🔧 Tecnologias Utilizadas

- **.NET 10**
- **Entity Framework Core 10.0.1**
- **SQL Server**
- **ASP.NET Core Web API**

## 💾 Instalando Pacotes

```bash
dotnet add package Microsoft.EntityFrameworkCore --version 10.0.1
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 10.0.1
```

## 🚀 Executando a Aplicação

```bash
dotnet run
```

A API estará disponível em: `https://localhost:7001`

Para visualizar a documentação Swagger/OpenAPI: `https://localhost:7001/openapi/v1.json`

## ✅ Testes

Você pode testar os endpoints usando:
- **Postman**
- **Thunder Client** (extensão VS Code)
- **curl** (linha de comando)
- **Visual Studio** - Built-in HTTP Client

## 📝 Notas Importantes

- Todos os IDs de cliente são `GUID` (Globally Unique Identifier)
- `Bloqueado = false` significa licença **Liberada**
- `Bloqueado = true` significa licença **Bloqueada**
- A API retorna mensagens de erro descritivas em português
- Validações incluem verificação de cliente inexistente e estados inválidos

## 🔐 Considerações de Segurança

Para produção, considere adicionar:
- Autenticação e autorização (JWT, OAuth)
- Rate limiting
- HTTPS obrigatório
- Logging e monitoramento
- Validação de entrada mais robusta

---

**Desenvolvido com ❤️ para seu ERP**
