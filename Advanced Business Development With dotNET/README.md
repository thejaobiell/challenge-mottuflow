<div align="center">
  <img src="https://github.com/thejaobiell/MottuFlowJava/blob/main/MottuFlow/src/main/resources/static/images/logo.png?raw=true" alt="MottuFlow Logo" width="200"/>
  
  <h1><i><b>MottuFlow</b></i> - Sprint 4</h1>
  <p><b>Disciplina:</b> Advanced Business Development with .NET</p>
  <p><b>Professor Orientador:</b> Leonardo Gasparini Romão</p>
  <p>API RESTful desenvolvida em <b>.NET 8</b> para o gerenciamento inteligente de frotas de motocicletas da empresa <b>Mottu</b>.</p>
</div>

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-ASP.NET_Core-green.svg)](https://learn.microsoft.com/aspnet/core)
[![Entity Framework](https://img.shields.io/badge/ORM-Entity%20Framework%20Core-purple.svg)](https://learn.microsoft.com/ef/)
[![ML.NET](https://img.shields.io/badge/ML.NET-Previsão%20de%20Manutenção-orange.svg)](https://dotnet.microsoft.com/apps/machinelearning-ai/ml-dotnet)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)
[![FIAP](https://img.shields.io/badge/FIAP-2TDSB-red.svg)](https://www.fiap.com.br/)

---

## Visão Geral

O **MottuFlow** é uma **API RESTful** desenvolvida para otimizar o **gerenciamento inteligente de frotas de motocicletas** da empresa **Mottu**.  
A solução oferece controle completo sobre **funcionários, pátios, motos, câmeras, ArUco Tags, registros de status e localidades**, proporcionando **eficiência operacional**, **automação de processos** e **monitoramento centralizado** com boas práticas REST e integração com **Oracle**.

A aplicação implementa **autenticação JWT**, **HATEOAS**, **Health Checks**, **versionamento de API**, **Machine Learning com ML.NET** para previsão de manutenção preventiva, e testes automatizados com **xUnit** e **WebApplicationFactory**.

---

## Arquitetura do Sistema

O sistema segue arquitetura em camadas (**Controller → Service → Repository → Data → Model**), garantindo modularidade e manutenibilidade.

### 1. Container Diagram

```mermaid
graph TB
    subgraph MottuFlow["Software System: MottuFlow"]
        api["API ASP.NET Core Web API"]
        service["Services (Lógica de Negócio)"]
        repo["Repositories (Acesso a Dados)"]
        db[("Database (Oracle / InMemory)")]

        swagger["Swagger UI (OpenAPI)"]
        health["Health Checks"]
        ml["ML.NET Engine (Previsão de Manutenção)"]
    end

    user["Front-End Web/Mobile"]
    idp["Provedor de Identidade JWT"]

    user --> api
    api --> service
    service --> repo
    repo --> db
    api --> swagger
    api --> health
    service --> ml
    api --> idp
```

---

### 2. Component Diagram

```mermaid
graph LR
    ctrl["MotoController – Endpoints REST"]
    svc["MotoService – Regras de Negócio"]
    repo["MotoRepository – Acesso a Dados"]
    mapper["MotoMapper – DTO ⇄ Entidade"]
    validator["MotoValidator – Validação de Dados"]
    mlengine["ML.NET Engine – Previsão de Manutenção"]
    db[(Banco de Dados Oracle / InMemory)]

    ctrl --> svc
    svc --> repo
    svc --> mapper
    svc --> validator
    svc --> mlengine
    repo --> db
```

---

## Funcionalidades Principais

- CRUD completo para Funcionário, Pátio, Moto, etc.  
- Autenticação JWT e proteção de endpoints  
- HATEOAS em todas as respostas  
- Versionamento de API (v1, v2)  
- Health Check (`/api/health/ping`)  
- Swagger/OpenAPI documentado com anotações  
- **Integração ML.NET para previsão de manutenção preventiva**
- Testes automatizados com xUnit

---

## Machine Learning - Previsão de Manutenção

O **MottuFlow** utiliza **ML.NET** para analisar dados de telemetria das motocicletas e prever a necessidade de manutenção preventiva, ajudando a reduzir custos e aumentar a disponibilidade da frota.

### 📊 Dados de Entrada para o Modelo

O modelo de ML analisa os seguintes parâmetros em tempo real:

| Parâmetro | Descrição | Unidade |
|-----------|-----------|---------|
| **Vibração** | Nível de vibração do motor | Valor normalizado (0.0 - 1.0) |
| **Temperatura do Motor** | Temperatura de operação | Graus Celsius (°C) |
| **KM Rodados** | Quilometragem total | Quilômetros |
| **Idade do Óleo** | Dias desde a última troca de óleo | Dias |

### 🔍 Exemplos de Dados

#### Exemplo 1: Moto em Bom Estado
```json
{
  "Vibracao": 0.2,
  "TemperaturaMotor": 85.5,
  "KMRodados": 1500,
  "IdadeOleoDias": 30
}
```
**Resultado esperado:** Manutenção não necessária (baixa vibração, temperatura normal, baixa quilometragem, óleo recente)

---

#### Exemplo 2: Moto Requerendo Manutenção
```json
{
  "Vibracao": 0.6,
  "TemperaturaMotor": 98.5,
  "KMRodados": 8000,
  "IdadeOleoDias": 120
}
```
**Resultado esperado:** Manutenção preventiva recomendada (vibração elevada, temperatura alta, alta quilometragem, óleo vencido)

---

### 🎯 Endpoint de Previsão

**Método:** `POST`  
**URL:** `/api/v1/ml/predicao`

**Corpo da requisição:**
```json
{
  "vibracao": 0.6,
  "temperaturaMotor": 98.5,
  "kmRodados": 8000,
  "idadeOleoDias": 120
}
```

**Resposta de sucesso (200 OK):**
```json
{
  "success": true,
  "message": "Previsão de manutenção realizada com sucesso",
  "data": {
    "requerManutencao": true,
    "confianca": 0.87,
    "recomendacoes": [
      "Troca de óleo urgente (120 dias desde última troca)",
      "Verificar sistema de arrefecimento (temperatura elevada)",
      "Inspeção de componentes mecânicos (vibração anormal detectada)"
    ],
    "prioridade": "Alta",
    "dataPrevisao": "2025-10-30T14:30:00Z"
  },
  "statusCode": 200,
  "timestampUtc": "2025-10-30T14:30:00.123Z"
}
```

---

### 🧠 Algoritmo de Machine Learning

O modelo utiliza **classificação binária** com os seguintes algoritmos do ML.NET:

- **FastTree** (Gradient Boosting Decision Trees)
- **LightGBM** (Light Gradient Boosting Machine)
- **SDCA** (Stochastic Dual Coordinate Ascent)

O modelo é treinado com dados históricos de manutenções e telemetria para identificar padrões que indicam necessidade de intervenção preventiva.

---

## Tecnologias Utilizadas

| Tecnologia | Descrição |
|-------------|------------|
| **.NET 8 / ASP.NET Core** | Framework principal da API |
| **Entity Framework Core** | ORM para Oracle e InMemory |
| **ML.NET** | Machine Learning para previsão de manutenção |
| **Swagger / Swashbuckle** | Documentação interativa da API |
| **JWT Bearer** | Autenticação e segurança |
| **xUnit** | Testes de unidade e integração |
| **HATEOAS** | Navegação via links semânticos |
| **Oracle / InMemory** | Suporte a múltiplos bancos de dados |

---

## Pré-requisitos

Antes de executar o projeto, certifique-se de ter instalado:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Oracle Database](https://www.oracle.com/database/technologies/oracle-database-software-downloads.html) (ou Docker com [oracle-free](https://hub.docker.com/r/gvenzl/oracle-free))
- [Oracle SQL Developer para VSCode](https://marketplace.visualstudio.com/items?itemName=Oracle.sql-developer) para executar scripts SQL

---

## Execução Local

### 1️⃣ Clonar o repositório

```bash
git clone https://github.com/leomotalima/MottuFlow-Sprint4.git
cd MottuFlow-Sprint4
```

---

### 2️⃣ Configurar as credenciais do banco de dados

Crie um arquivo `.env` na raiz do projeto e configure as credenciais do Oracle:

```env
ORACLE_USER_ID=<Seu Username Oracle>
ORACLE_PASSWORD=<Sua Senha Oracle>
ORACLE_DATA_SOURCE=host:porta/nome_do_serviço
ConnectionStrings__OracleConnection=User Id=${ORACLE_USER_ID};Password=${ORACLE_PASSWORD};Data Source=${ORACLE_DATA_SOURCE}
```

> **⚠️ IMPORTANTE:** Altere os valores de `ORACLE_USER_ID`, `ORACLE_PASSWORD` e `ORACLE_DATA_SOURCE` conforme seu ambiente Oracle local.

---

### 3️⃣ Instalar ferramentas e dependências

Execute os seguintes comandos no terminal:

```bash
# Instalar Entity Framework CLI globalmente
dotnet tool install --global dotnet-ef

# Restaurar pacotes NuGet
dotnet restore

# Compilar o projeto
dotnet build

# Aplicar migrations no banco de dados
dotnet ef database update
```

---

### 4️⃣ Popular o banco de dados com dados iniciais

Após aplicar as migrations, execute o script SQL para inserir os dados iniciais:

**Opção 1: Usando Oracle SQL Developer para VSCode**

1. Instale a extensão [Oracle SQL Developer](https://marketplace.visualstudio.com/items?itemName=Oracle.sql-developer) no VSCode

2. Configure uma conexão com seu banco Oracle:
   - Abra o painel lateral do Oracle SQL Developer no VSCode
   - Clique em "Create Connection"
   - Preencha os dados de conexão (user, password, host, port, service)

3. Abra o arquivo `Scripts/inserts.sql` no VSCode

4. Execute o script:
   - Clique com botão direito no editor → "Execute SQL"
   - Ou use o atalho `Ctrl+Enter` (Linux/Windows) / `Cmd+Enter` (Mac)

**Opção 2: Usando Oracle SQL Developer Desktop**

1. Abra o Oracle SQL Developer
2. Conecte-se ao banco de dados
3. Abra o arquivo `Scripts/inserts.sql`
4. Execute o script clicando no botão "Run Script" (F5)

> **💡 O que esse script faz:**
> - Insere um usuário administrador padrão (`admin@mottu.com`)
> - Cria um pátio de exemplo
> - Adiciona uma câmera, moto, ArUco tag e registros de localidade e status
> - Utiliza `EXECUTE IMMEDIATE` com concatenação dinâmica do schema do usuário atual

---

### 5️⃣ Executar a aplicação

Volte para a raiz do projeto (se estiver na pasta Scripts):
```bash
cd ..
```

Execute a aplicação:
```bash
dotnet run
```

A API estará disponível em: **[http://localhost:5224/swagger/index.html](http://localhost:5224/swagger/index.html)**

---

### 6️⃣ Utilizar os endpoints versionados

Todos os endpoints da API utilizam versionamento na URL. Exemplo:
```http
GET /api/v1/funcionarios
POST /api/v1/auth/login
POST /api/v1/ml/predicao
```

> **💡 Observação:** A versão padrão é `v1`. Caso nenhuma versão seja especificada na URL, a API assumirá automaticamente a versão 1.0.

---

### 7️⃣ Autenticar na API

Antes de utilizar os endpoints protegidos, faça login no endpoint de autenticação:

**Endpoint:**
```http
POST /api/v1/auth/login
```

**Corpo da requisição:**
```json
{
  "username": "admin@mottu.com",
  "password": "adminmottu"
}
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhZG1pbkBtb3R0dS5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhZG9yIiwianRpIjoiNmM3MDE4ZDEtNmU4MC00YTAyLTk4MjgtMTg2Yzg0ZmI4ODUzIiwiZXhwIjoxNzYxODM1NDkwLCJpc3MiOiJNb3R0dUZsb3ciLCJhdWQiOiJNb3R0dUZsb3dBcGlVc2VycyJ9.1kQV4c9rf2TzEaHHnn1PJ5F_0xjtTCEzgdkHjj6SIS0",
  "role": "Administrador",
  "expiresIn": "2h"
}
```

> **💡 Recomendação:** Utilize o Swagger UI para testar os endpoints. Clique no botão **"Authorize"** e cole o **token JWT** retornado (copie apenas o valor do campo `token` sem as aspas).

---

## Estrutura do Projeto

```
MottuFlow-Sprint4/
├── Controllers/           # Endpoints da API
├── Data/                 # DbContext e configurações EF
├── DTOs/                 # Data Transfer Objects
├── Hateoas/              # Implementação HATEOAS
├── Models/               # Entidades do domínio
├── Repositories/         # Acesso a dados
├── Services/             # Lógica de negócio
├── ML/                   # Modelos e serviços de Machine Learning
├── Swagger/              # Configurações Swagger
├── Scripts/              # Scripts SQL (inserts.sql)
├── MottuFlow.Tests/      # Testes automatizados
├── Program.cs            # Ponto de entrada da aplicação
├── .env                  # Variáveis de ambiente (criar manualmente)
└── README.md
```

---

## Documentação da API

### Health Check
```http
GET /api/health/ping
```
**Resposta:**
```json
{
  "success": true,
  "message": "API rodando com sucesso 🚀",
  "data": {
    "status": "Healthy",
    "version": "1.0.0",
    "uptime": "00:00:00",
    "environment": "Development",
    "host": "<nome do host>",
    "timestampUtc": "2025-10-30T12:50:01.5172445Z"
  },
  "statusCode": 200,
  "timestampUtc": "2025-10-30T12:50:01.5173065Z"
}
```

---

### Funcionários

| Método | Endpoint | Descrição |
|--------|-----------|-----------|
| `GET` | `/api/v1/funcionarios` | Lista todos os funcionários |
| `GET` | `/api/v1/funcionarios/{id}` | Retorna um funcionário específico |
| `POST` | `/api/v1/funcionarios` | Cria um novo funcionário |
| `PUT` | `/api/v1/funcionarios/{id}` | Atualiza dados de um funcionário |
| `DELETE` | `/api/v1/funcionarios/{id}` | Remove um funcionário |

**Exemplo de resposta com HATEOAS:**
```json
{
  "success": true,
  "message": "Funcionários listados com sucesso.",
  "data": {
    "meta": {
      "totalItems": 1,
      "page": 1,
      "pageSize": 10,
      "totalPages": 1
    },
    "funcionarios": [
      {
        "nome": "Admin",
        "cpf": "000.000.000-00",
        "cargo": "Administrador",
        "telefone": "(00)00000-0000",
        "email": "admin@mottu.com",
        "dataCadastro": "2025-10-29T12:48:25",
        "id": 1,
        "links": []
      }
    ]
  },
  "statusCode": 200,
  "timestampUtc": "2025-10-30T12:48:25.2077607Z"
}
```

---

### Pátios

| Método | Endpoint | Descrição |
|--------|-----------|-----------|
| `GET` | `/api/v1/patios` | Lista todos os pátios |
| `GET` | `/api/v1/patios/{id}` | Retorna um pátio específico |
| `POST` | `/api/v1/patios` | Cria um novo pátio |
| `PUT` | `/api/v1/patios/{id}` | Atualiza dados de um pátio |
| `DELETE` | `/api/v1/patios/{id}` | Remove um pátio |

---

### Motos

| Método | Endpoint | Descrição |
|--------|-----------|-----------|
| `GET` | `/api/v1/motos` | Lista todas as motos |
| `GET` | `/api/v1/motos/{id}` | Retorna uma moto específica |
| `POST` | `/api/v1/motos` | Cria uma nova moto |
| `PUT` | `/api/v1/motos/{id}` | Atualiza dados de uma moto |
| `DELETE` | `/api/v1/motos/{id}` | Remove uma moto |

---

### Câmeras

| Método | Endpoint | Descrição |
|--------|-----------|-----------|
| `GET` | `/api/v1/cameras` | Lista todas as câmeras |
| `GET` | `/api/v1/cameras/{id}` | Retorna uma câmera específica |
| `POST` | `/api/v1/cameras` | Cria uma nova câmera |
| `PUT` | `/api/v1/cameras/{id}` | Atualiza dados de uma câmera |
| `DELETE` | `/api/v1/cameras/{id}` | Remove uma câmera |

---

### ArUco Tags

| Método | Endpoint | Descrição |
|--------|-----------|-----------|
| `GET` | `/api/v1/arucotags` | Lista todas as ArUco tags |
| `GET` | `/api/v1/arucotags/{id}` | Retorna uma ArUco tag específica |
| `POST` | `/api/v1/arucotags` | Cria uma nova ArUco tag |
| `PUT` | `/api/v1/arucotags/{id}` | Atualiza dados de uma ArUco tag |
| `DELETE` | `/api/v1/arucotags/{id}` | Remove uma ArUco tag |

---

### Localidades

| Método | Endpoint | Descrição |
|--------|-----------|-----------|
| `GET` | `/api/v1/localidades` | Lista todas as localidades |
| `GET` | `/api/v1/localidades/{id}` | Retorna uma localidade específica |
| `POST` | `/api/v1/localidades` | Cria uma nova localidade |
| `PUT` | `/api/v1/localidades/{id}` | Atualiza dados de uma localidade |
| `DELETE` | `/api/v1/localidades/{id}` | Remove uma localidade |

---

### Registros de Status

| Método | Endpoint | Descrição |
|--------|-----------|-----------|
| `GET` | `/api/v1/registro-status` | Lista todos os registros de status |
| `GET` | `/api/v1/registro-status/{id}` | Retorna um registro específico |
| `POST` | `/api/v1/registro-status` | Cria um novo registro de status |
| `PUT` | `/api/v1/registro-status/{id}` | Atualiza um registro de status |
| `DELETE` | `/api/v1/registro-status/{id}` | Remove um registro de status |

---

## Testes Automatizados

![Tests](https://img.shields.io/badge/Testes%20de%20Integração-100%25%20Aprovados-brightgreen.svg)
![Build](https://img.shields.io/badge/Build-Sucesso-blue.svg)

- Banco InMemory criado e inicializado corretamente  
- Endpoints retornando status HTTP esperados (200, 201, 404 etc.)  
- Separação entre ambientes Oracle (produção) e InMemory (testes)  
- Testes de autenticação JWT validados com sucesso  
- Testes de previsão ML.NET validados

**Executando os testes manualmente:**
```bash
dotnet clean
dotnet build
dotnet test
```
> Todos os testes rodam com banco **InMemory**, sem necessidade do Oracle local.

---

## Equipe de Desenvolvimento

<table align="center">
<tr>
<td align="center">
<a href="https://github.com/thejaobiell">
<img src="https://github.com/thejaobiell.png" width="100px;" alt="João Gabriel"/><br>
<sub><b>João Gabriel Boaventura</b></sub><br>
<sub>RM554874 • 2TDSB2025</sub><br>
</a>
</td>
<td align="center">
<a href="https://github.com/leomotalima">
<img src="https://github.com/leomotalima.png" width="100px;" alt="Léo Mota"/><br>
<sub><b>Léo Mota Lima</b></sub><br>
<sub>RM557851 • 2TDSB2025</sub><br>
</a>
</td>
<td align="center">
<a href="https://github.com/LucasLDC">
<img src="https://github.com/LucasLDC.png" width="100px;" alt="Lucas Leal"/><br>
<sub><b>Lucas Leal das Chagas</b></sub><br>
<sub>RM551124 • 2TDSB2025</sub><br>
</a>
</td>
</tr>
</table>

---

## Licença

Distribuído sob a licença **MIT**.  
Consulte [LICENSE](https://choosealicense.com/licenses/mit/).
