<div align="center">
  <img src="https://raw.githubusercontent.com/thejaobiell/MottuFlowJava/refs/heads/main/MottuFlow/src/main/resources/static/images/logo.png" alt="MottuFlow" width="200"/>
  <br/><br/>
  <img src="https://img.shields.io/badge/Java-21-orange.svg" alt="Java"/>
  <img src="https://img.shields.io/badge/Spring%20Boot-3.x-brightgreen.svg" alt="Spring Boot"/>
  <img src="https://img.shields.io/badge/MySQL-8.0-blue.svg" alt="MySQL"/>
  <img src="https://img.shields.io/badge/React%20Native-0.74-blue.svg" alt="React Native"/>
  <img src="https://img.shields.io/badge/Expo-53-black.svg" alt="Expo"/>
</div>


**MottuFlow** é uma solução completa para gerenciamento de frotas de motocicletas, desenvolvida com arquitetura híbrida que combina **API REST** moderna com interface web intuitiva e aplicativo mobile. O sistema utiliza **visão computacional** e **ArUco Tags** para identificação automática de veículos, oferecendo controle total sobre funcionários, pátios, motos, câmeras e localização de ativos.

## 🎯 Visão Geral

O MottuFlow foi desenvolvido integrando as disciplinas de **Java Advanced**, **Mobile Application Development** e **Database**. A solução oferece:

- **📱 Arquitetura Híbrida**: API REST + Interface web + App Mobile
- **🔒 Segurança Robusta**: Autenticação JWT e Spring Security
- **📊 Gestão Completa**: Controle de funcionários, frotas, pátios e status em tempo real

### 🎥 Demonstração

[![Ver demonstração no YouTube](https://img.shields.io/badge/YouTube-Ver%20Demonstração-red?style=for-the-badge&logo=youtube)](<https://youtu.be/j_LRC3WB7pA>)

### 🔗 Repositórios do Projeto

| Componente | Repositório | Descrição |
|------------|-------------|-----------|
| **🖥️ API REST + Web** | [MottuFlowJava](https://github.com/thejaobiell/MottuFlowJava) | Backend Spring Boot + Interface Thymeleaf |
| **📱 App Mobile** | [challenge-mottuflow](https://github.com/FIAP-MOBILE/challenge-mottuflow) | Aplicativo React Native + Expo |

---

## ✨ Funcionalidades

### 🏢 Módulos Principais

| Módulo | Descrição | Funcionalidades |
|--------|-----------|-----------------|
| **🏪 Pátios** | Gerenciamento de locais | Monitoramento |
| **🏍️ Motos** | Controle de frota | Registro, status, localização, manutenção |
| **🏷️ ArUco Tags** | Identificação visual | Cadastro e rastreamento |
| **📍 Status & Localização** || Posição, disponibilidade, alertas |

### 🚀 Recursos Avançados

- ✅ **Interface Web Responsiva** - Thymeleaf + Bootstrap
- ✅ **App Mobile Nativo** - React Native + Expo
- ✅ **Autenticação Segura** - JWT + Spring Security
- ✅ **Migração de Dados** - Flyway para versionamento de BD
- ✅ **Validação de Dados** - Bean Validation integrado

## 🛠️ Tecnologias

### Backend (API REST + Web Interface)
- **Java 21** - LTS com recursos modernos
- **Spring Boot** - Framework principal
- **Spring Data JPA** - Persistência de dados
- **Spring Security** - Autenticação e autorização
- **Spring Web** - API REST
- **Thymeleaf** - Engine de templates
- **MySQL 8.0** - Banco de dados principal
- **Flyway** - Controle de versão do schema

### Mobile Application
- **React Native** - Framework mobile multiplataforma
- **Expo** - Plataforma de desenvolvimento
- **TypeScript** - Linguagens de programação

## 🚀 Como Executar o Projeto

### 📋 Pré-requisitos

- **Java 21+** ([OpenJDK](https://openjdk.org/install/) ou [Oracle JDK](https://www.oracle.com/java/technologies/downloads/))
- **MySQL 8.0+** ([Download](https://dev.mysql.com/downloads/mysql/))
- **Node.js 18+** ([Download](https://nodejs.org/))
- **Expo SDK 53** ([Download](https://expo.dev/go))

## 🔧 Configuração e Execução da API

### 1. 🗄️ Configuração do Banco de Dados MySQL

#### Instalação MySQL

##### **Linux**
```bash
sudo apt update
sudo apt install mysql-server mysql-client
sudo mysql_secure_installation
```

##### **Windows**
1. Baixe o [MySQL Installer](https://dev.mysql.com/downloads/installer/)
2. Durante a instalação, configure a senha do usuário `root`
3. Inicie o MySQL

#### Criação do Usuário e Banco
```sql
-- Execute no MySQL como root
CREATE USER 'mottu_user'@'%' IDENTIFIED BY 'user123';
GRANT ALL PRIVILEGES ON mottuflow.* TO 'mottu_user'@'%';
FLUSH PRIVILEGES;
```

### 2. 📥 Clonagem e Configuração da API

```bash
# Clone o repositório da API
git clone https://github.com/thejaobiell/MottuFlowJava.git
cd MottuFlowJava/MottuFlow
```

### 3. ⚙️ Configuração da API

Edite o arquivo `src/main/resources/application.properties`:

```properties
spring.application.name=MottuFlow

spring.datasource.url=jdbc:mysql://localhost:3306/mottuflow?createDatabaseIfNotExist=true
spring.datasource.username=<SEU USUARIO>
spring.datasource.password=<SUA SENHA>

spring.jpa.hibernate.ddl-auto=update
spring.jpa.show-sql=true
spring.jpa.database-platform=org.hibernate.dialect.MySQL8Dialect

spring.flyway.enabled=true
spring.flyway.locations=classpath:db/migration
spring.flyway.repair=true
spring.flyway.repair-on-migrate=true

logging.level.root=WARN
logging.level.org.springframework=WARN
logging.level.org.hibernate=WARN
logging.level.com.mysql.cj=WARN
logging.level.org.hibernate.orm.deprecation=ERROR
spring.jpa.open-in-view=false

logging.level.com.sprint.MottuFlow=WARN

spring.main.allow-bean-definition-overriding=true

server.address=0.0.0.0
server.port=8080
```

### 4. 🚀 Executando a API

#### Via Terminal (Linux/macOS/WSL)
```bash
./mvnw spring-boot:run
```

#### Via Windows PowerShell
```powershell
.\mvnw.cmd spring-boot:run
```

#### Via IDEs

<details>
<summary><b>🚀 IntelliJ IDEA (Recomendado)</b></summary>

1. **File** → **Open**
2. Selecione a pasta `MottuFlow` (contém `pom.xml`)
3. Aguarde o IntelliJ importar as dependências Maven
4. Execute `MottuFlowApplication.java` → **Run**

</details>

<details>
<summary><b>🌙 Eclipse IDE</b></summary>

1. **File** → **Import** → **Maven** → **Existing Maven Projects**
2. **Browse** → Selecione pasta `MottuFlow`
3. Marque o `pom.xml` → **Finish**
4. **Run As** → **Spring Boot App**

</details>

<details>
<summary><b>💻 VS Code</b></summary>

1. Instale as extensões: **Java Extension Pack**, **Spring Boot Extension Pack**
2. Abra a pasta `MottuFlow`
3. **Ctrl+Shift+P** → "Spring Boot: Run"

</details>

### ✅ Verificação da API

Após a execução, você verá:

```
 ██████╗ ███╗   ██╗██╗     ██╗███╗   ██╗███████╗██╗
██╔═══██╗████╗  ██║██║     ██║████╗  ██║██╔════╝██║
██║   ██║██╔██╗ ██║██║     ██║██╔██╗ ██║█████╗  ██║
██║   ██║██║╚██╗██║██║     ██║██║╚██╗██║██╔══╝  ╚═╝
╚██████╔╝██║ ╚████║███████╗██║██║ ╚████║███████╗██╗
 ╚═════╝ ╚═╝  ╚═══╝╚══════╝╚═╝╚═╝  ╚═══╝╚══════╝╚═╝

Clique aqui para acessar o Thymeleaf:   http://localhost:8080
Clique aqui para acessar o Swagger UI:   http://localhost:8080/swagger-ui/index.html
```

## 📱 Configuração e Execução do App Mobile

### 1. 📥 Clonagem do Repositório Mobile

```bash
git clone https://github.com/FIAP-MOBILE/challenge-mottuflow.git
git checkout sprint3
cd challenge-mottuflow
```

### 2. 📦 Instalação das Dependências

```bash
# Instale as dependências
npm install
```

### 3. ⚙️ Conectando a API ao aplicativo

#### 📍 Descobrindo o IP da sua máquina

Antes de configurar a API, você precisa descobrir o endereço IP da sua máquina:

**🐧 No Linux:**
```bash
hostname -I | awk '{print $1}'
```

**🪟 No Windows:**
```cmd
ipconfig | findstr "IPv4" | findstr "192.168\|10\.\|172\."
```
Ou simplesmente:
```cmd
ipconfig
```
E procure pelo "Endereço IPv4" da sua conexão ativa.


#### 🔧 Configurando a API

Edite o arquivo `types/Api.ts` e substitua `<IP DA SUA MÁQUINA AQUI>` pelo IP obtido:

```typescript
const api = axios.create({
	baseURL: "http://<IP DA SUA MÁQUINA AQUI>:8080/api",
	headers: { "Content-Type": "application/json" },
});
```

#### 📝 Dicas importantes:

- **IP local vs externo**: Use o IP da rede local (geralmente começa com 192.168.x.x, 10.x.x.x ou 172.16-31.x.x)
- **Firewall**: Certifique-se de que a porta 8080 não está bloqueada pelo firewall
- **Mesma rede**: O dispositivo que vai acessar a API deve estar na mesma rede Wi-Fi/Ethernet

### 4. 🚀 Executando o App

```bash
# Execute o projeto
npx expo start
```

ou

```bash
npm start
```

### 4. 📱 Testando o App

Após executar `npx expo start`, você pode:

- **📱 No dispositivo físico**: Baixe o app **Expo Go** e escaneie o QR Code
- **📱 Simulador iOS**: Pressione `i` no terminal
- **📱 Emulador Android**: Pressione `a` no terminal

## 🔗 Acessos e Endpoints

### 🖥️ Serviços da API

| Serviço | URL | Descrição |
|---------|-----|-----------|
| **🖥️ Interface Web** | http://localhost:8080 | Dashboard Thymeleaf (Onde os administradores irão usar) |
| **📡 API REST** | http://localhost:8080/api | Endpoints REST (Onde os funcionários irão usar) |

### 👤 Usuários Padrão

| Usuário | Senha | Cargo | Acesso |
|---------|-------|--------|--------|
| `admin@email.com` | `adminmottu` | Administrador | Completo | 
| `joao@email.com` | `joao123` | Mecânico | Limitado | 
| `maria@email.com` | `maria123` | Gerente | Completo | 

> ⚠️ **Recomendação**: Use a conta de Administrador para testes completos

## 🔐 Autenticação JWT

### 🔑 Obtendo Token via Postman

1. **Importe** a coleção [API - MottuFlow.postman\_collection.json](https://github.com/thejaobiell/MottuFlowJava/blob/main/MottuFlow/jsonsAPIREST/API%20-%20MottuFlow.postman_collection.json)
2. No menu **`0 - JWT`**, execute **POST Pegar Token JWT**:
   ```json
   {
     "email": "admin@email.com",
     "senha": "adminmottu"
   }
   ```
3. Copie o `tokenAcesso` retornado
4. Em **Variables**, substitua `jwt` pelo seu token
5. Todos os endpoints estarão autenticados! 🚀

## 📡 Principais Endpoints da API

# 📚 Documentação da API MottuFlow

## 🔧 Configuração Base
- **Base URL:** `http://localhost:8080/api`
- **Autenticação:** Bearer Token (JWT)
- **Content-Type:** `application/json`

## 🔐 Autenticação (JWT)

### Login
```http
POST /login
Content-Type: application/json

{
  "email": "admin@email.com",
  "senha": "adminmottu"
}
```

### Atualizar Token
```http
POST /atualizar-token
Content-Type: application/json

{
  "refreshToken": "seu_refresh_token_aqui"
}
```

### Verificar Token
```http
POST /verificar-jwt
Content-Type: application/json

{
  "tokenAcesso": "seu_token_jwt_aqui"
}
```

## 👥 Funcionários

### Listar Funcionários
```http
GET /funcionario/listar
Authorization: Bearer {jwt_token}
```

### Buscar por ID
```http
GET /funcionario/buscar-por-id/{id}
Authorization: Bearer {jwt_token}
```

### Buscar por CPF
```http
GET /funcionario/buscar-por-cpf/{cpf}
Authorization: Bearer {jwt_token}
```
**Exemplo:** `/funcionario/buscar-por-cpf/000.000.000-00`

### Criar Funcionário
```http
POST /funcionario/cadastrar
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "nome": "Novo Funcionário",
  "cpf": "333.333.333-33",
  "cargo": "MECANICO",
  "telefone": "(33) 33333-3333",
  "email": "novo@email.com",
  "senha": "senha123"
}
```

### Atualizar Funcionário
```http
PUT /funcionario/editar/{id}
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "id": 2,
  "nome": "João Mecânico Atualizado",
  "cpf": "111.111.111-11",
  "cargo": "MECANICO",
  "telefone": "(11) 11111-1111",
  "email": "joao@email.com",
  "senha": "novaSenha123"
}
```

### Alterar Senha
```http
PATCH /funcionario/alterar-senha
Content-Type: application/json

{
  "email": "admin@email.com",
  "senhaAtual": "adminmottu",
  "novaSenha": "mottuadmin"
}
```

### Deletar Funcionário
```http
DELETE /funcionario/deletar/{id}
Authorization: Bearer {jwt_token}
```

## 🏢 Pátios

### Listar Pátios
```http
GET /patios/listar
Authorization: Bearer {jwt_token}
```

### Buscar por ID
```http
GET /patios/buscar-por-id/{id}
Authorization: Bearer {jwt_token}
```

### Criar Pátio
```http
POST /patios/cadastrar
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "nome": "Patio AlfaBeta",
  "endereco": "Rua Principal, 123",
  "capacidadeMaxima": 500
}
```

### Atualizar Pátio
```http
PUT /patios/editar/{id}
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "nome": "Patio Atualizado",
  "endereco": "Rua Nova, 456",
  "capacidadeMaxima": 100
}
```

### Deletar Pátio
```http
DELETE /patios/deletar/{id}
Authorization: Bearer {jwt_token}
```

## 🏍️ Motos

### Listar Motos
```http
GET /motos/listar
Authorization: Bearer {jwt_token}
```

### Listar Motos com ArUco Tags
```http
GET /motos/motos-com-tags
Authorization: Bearer {jwt_token}
```

### Buscar por ID
```http
GET /motos/buscar-por-id/{id}
Authorization: Bearer {jwt_token}
```

### Buscar por Placa
```http
GET /motos/buscar-por-placa/{placa}
Authorization: Bearer {jwt_token}
```

### Buscar por Fabricante
```http
GET /motos/buscar-por-fabricante?fabricante={fabricante}
Authorization: Bearer {jwt_token}
```
**Exemplo:** `/motos/buscar-por-fabricante?fabricante=Yamaha`

### Buscar por Pátio
```http
GET /motos/buscar-por-patio/{idPatio}
Authorization: Bearer {jwt_token}
```

### Criar Moto
```http
POST /motos/cadastrar
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "placa": "ABC-1234",
  "modelo": "Honda CB500",
  "fabricante": "Honda",
  "ano": 2021,
  "idPatio": 2,
  "localizacaoAtual": "Setor A"
}
```

### Atualizar Moto
```http
PUT /motos/editar/{id}
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "placa": "XYZ-5678",
  "modelo": "Yamaha MT-07",
  "fabricante": "Yamaha",
  "ano": 2022,
  "idPatio": 2,
  "localizacaoAtual": "Setor B"
}
```

### Deletar Moto
```http
DELETE /motos/deletar/{id}
Authorization: Bearer {jwt_token}
```

## 📹 Câmeras

### Listar Câmeras
```http
GET /cameras/listar
Authorization: Bearer {jwt_token}
```

### Buscar por ID
```http
GET /cameras/buscar-por-id/{id}
Authorization: Bearer {jwt_token}
```

### Buscar por Status Operacional
```http
GET /cameras/buscar-por-status/{status}
Authorization: Bearer {jwt_token}
```
**Exemplo:** `/cameras/buscar-por-status/Operacional`

### Criar Câmera
```http
POST /cameras/cadastrar
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "statusOperacional": "ONLINE",
  "localizacaoFisica": "Entrada do Patio",
  "idPatio": 2
}
```

### Atualizar Câmera
```http
PUT /cameras/editar/{id}
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "statusOperacional": "INATIVA",
  "localizacaoFisica": "Saida do Patio",
  "idPatio": 3
}
```

### Deletar Câmera
```http
DELETE /cameras/deletar/{id}
Authorization: Bearer {jwt_token}
```

## 🏷️ ArUco Tags

### Listar ArUco Tags
```http
GET /aruco-tags/listar
Authorization: Bearer {jwt_token}
```

### Buscar por ID
```http
GET /aruco-tags/buscar-por-id/{id}
Authorization: Bearer {jwt_token}
```

### Buscar por Status
```http
GET /aruco-tags/buscar-por-status/{status}
Authorization: Bearer {jwt_token}
```
**Exemplo:** `/aruco-tags/buscar-por-status/ativo`

### Buscar por Código
```http
GET /aruco-tags/buscar-por-codigo/{codigo}
Authorization: Bearer {jwt_token}
```
**Exemplo:** `/aruco-tags/buscar-por-codigo/TAG004`

### Criar ArUco Tag
```http
POST /aruco-tags/cadastrar
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "codigo": "TAG12345",
  "idMoto": 4,
  "status": "ATIVO"
}
```

### Atualizar ArUco Tag
```http
PUT /aruco-tags/editar/{id}
Authorization: Bearer {jwt_token}
Content-Type: application/json
```

### Deletar ArUco Tag
```http
DELETE /aruco-tags/deletar/{id}
Authorization: Bearer {jwt_token}
```

## 📊 Status

### Listar Status
```http
GET /status/listar
Authorization: Bearer {jwt_token}
```

### Buscar por ID
```http
GET /status/buscar-por-id/{id}
Authorization: Bearer {jwt_token}
```

### Buscar por Tipo de Status
```http
GET /status/buscar-por-tipo?tipoStatus={tipo}
Authorization: Bearer {jwt_token}
```
**Exemplo:** `/status/buscar-por-tipo?tipoStatus=BAIXA_BOLETIM_OCORRENCIA`

### Buscar por Descrição
```http
GET /status/buscar-por-descricao?descricao={descricao}
Authorization: Bearer {jwt_token}
```
**Exemplo:** `/status/buscar-por-descricao?descricao=Perda por BO`

### Buscar por Período
```http
GET /status/buscar-por-periodo?inicio={dataInicio}&fim={dataFim}
Authorization: Bearer {jwt_token}
```
**Exemplo:** `/status/buscar-por-periodo?inicio=2025-09-28T00:00:00&fim=2025-09-28T23:59:59`

### Criar Status
```http
POST /status/cadastrar
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "idMoto": 4,
  "tipoStatus": "DISPONIVEL",
  "descricao": "Moto disponível para uso",
  "idFuncionario": 3
}
```

### Atualizar Status
```http
PUT /status/editar/{id}
Authorization: Bearer {jwt_token}
Content-Type: application/json
```

### Deletar Status
```http
DELETE /status/deletar/{id}
Authorization: Bearer {jwt_token}
```

## 📍 Localidades

### Listar Localidades
```http
GET /localidades/listar
Authorization: Bearer {jwt_token}
```

### Buscar por ID
```http
GET /localidades/buscar-por-id/{id}
Authorization: Bearer {jwt_token}
```

### Buscar por Pátio
```http
GET /localidades/buscar-por-patio/{idPatio}
Authorization: Bearer {jwt_token}
```

### Buscar por Ponto de Referência
```http
GET /localidades/buscar-por-ponto-referencia/{pontoReferencia}
Authorization: Bearer {jwt_token}
```
**Exemplo:** `/localidades/buscar-por-ponto-referencia/Vaga`

### Buscar por Período
```http
GET /localidades/buscar-por-periodo?dataInicio={dataInicio}&dataFim={dataFim}
Authorization: Bearer {jwt_token}
```
**Exemplo:** `/localidades/buscar-por-periodo?dataInicio=2025-09-06T08:00:00.000Z&dataFim=2025-09-08T08:20:00.000Z`

### Criar Localidade
```http
POST /localidades/cadastrar
Authorization: Bearer {jwt_token}
Content-Type: application/json
```

### Atualizar Localidade
```http
PUT /localidades/editar/{id}
Authorization: Bearer {jwt_token}
Content-Type: application/json
```

### Deletar Localidade
```http
DELETE /localidades/deletar/{id}
Authorization: Bearer {jwt_token}
```

## 📝 Notas Importantes

### Tipos de Status Disponíveis
- `DISPONIVEL`
- `RESERVADO`
- `BAIXA_BOLETIM_OCORRENCIA`

### Status Operacional de Câmeras
- `ONLINE`
- `INATIVA`
- `Operacional`

### Cargos de Funcionários
- `MECANICO`
- `OPERADOR`
- `ADMIN`

### Status de ArUco Tags
- `ATIVO`
- `INATIVO`

### Formato de Datas
Utilize o formato ISO 8601: `YYYY-MM-DDTHH:mm:ss` ou `YYYY-MM-DDTHH:mm:ss.sssZ`

### Variáveis de Ambiente
- `{{baseUrl}}`: http://localhost:8080/api
- `{{jwt}}`: Token JWT obtido no login

## 🔄 Integração API + Mobile

O aplicativo mobile consome a API REST através de:

1. **🔐 Autenticação JWT**: Login seguro via API
2. **📊 Sincronização de Dados**: Tempo real com backend
3. **📱 Interface Nativa**: Experiência otimizada para mobile
4. **🌐 Conectividade**: Comunicação direta com endpoints REST

O app mobile oferece:
- ✅ Cadastro de motos, ArUco Tags e localização
- ✅ Visualização de motos em tempo real
- ✅ Interface intuitiva e responsiva
- ✅ Integração completa com a API backend

## 🛡️ Segurança

- **🔐 JWT Authentication**: Tokens seguros para autenticação
- **🔒 Spring Security**: Configuração robusta de segurança
- **👤 Controle de Acesso**: Diferentes níveis de permissão
- **🛡️ Validação de Dados**: Proteção contra injeção e ataques

## ⚡ Troubleshooting

<details>
<summary><b>❌ Erro de Conexão com MySQL</b></summary>

```bash
# Verifique se o MySQL está rodando
sudo systemctl status mysql

# Reinicie o MySQL se necessário
sudo systemctl restart mysql

# Teste a conexão
mysql -u mottu_user -p mottuflow
```

</details>

<details>
<summary><b>❌ Erro de Porta em Uso</b></summary>

```bash
# Verifique qual processo está usando a porta 8080
sudo lsof -i :8080

# Mate o processo se necessário
sudo kill -9 <PID>
```

</details>

<details>
<summary><b>❌ Problemas com Maven</b></summary>

```bash
# Limpe e reinstale dependências
./mvnw clean install

# Force update das dependências
./mvnw clean install -U
```

</details>

<details>
<summary><b>❌ Problemas no App Mobile</b></summary>

```bash
# Limpe o cache do npm
npm cache clean --force

# Reinstale dependências
rm -rf node_modules package-lock.json
npm install

# Limpe cache do Expo
npx expo install --fix
```

</details>

## 👥 Equipe de Desenvolvimento

<table>
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
