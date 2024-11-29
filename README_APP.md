# Projeto com Docker Compose - RabbitMQ e SQLServer

Este projeto utiliza Docker Compose para configurar e executar um ambiente com RabbitMQ. Após configurar o contêiner, você poderá iniciar a API e o projeto web. **O SQL Server é necessário, mas não está incluído na configuração do Docker Compose**. 

## Pré-requisitos
Antes de começar, certifique-se de ter os seguintes requisitos instalados no seu ambiente:

- Docker Desktop (com suporte ao Docker Compose integrado).
- Ferramentas para executar e testar a API e o projeto web (como .NET SDK e um editor de texto como Visual Studio Code ou Visual Studio).
- **SQL Server em execução**: O projeto precisa de um banco de dados SQL Server configurado e rodando (não incluído no Docker Compose).

## Configuração e Execução

### 1. Configuração do Docker Compose
O arquivo `docker-compose.json` está configurado para executar o serviço RabbitMQ, com o console de gerenciamento disponível na porta 15672.

#### RabbitMQ:
- Serviço RabbitMQ com o console de gerenciamento disponível na porta 15672.
- Credenciais padrão:
  - Usuário: guest.
  - Senha: guest.

### 2. Como subir o contêiner RabbitMQ
Para iniciar o serviço RabbitMQ:

1. Abra o terminal no diretório que contém o arquivo `docker-compose.json`.
2. Execute o comando abaixo para criar e rodar o contêiner:

   ```bash
   docker compose -f docker-compose.json up -d

### Configuração do Connection String
No arquivo appsettings.json do projeto, adicione a configuração da connection string para o SQL Server:

json
Copiar código
{
  "ConnectionStrings": {
    "default": "Server=SUA STRING CONNECTION",
    "identity": "Server=SUA STRING CONNECTION"
  }
}

### Iniciar a API
Após o contêiner RabbitMQ estar ativo, você pode iniciar a API do seu projeto:

Navegue até o diretório da API UMBIT.ToDo.API.

### Iniciar o Projeto Web
Por fim, inicie o projeto web UMBIT.ToDo.Web:

###Cadastro de Usuário Administrador
O sistema exige o cadastro de um usuário administrador, que será responsável por coordenar as tarefas. O administrador tem acesso a todas as tarefas dos outros usuários. Caso queira testar com outro usuário, crie um novo usuário na tela de login. No sistema, existe apenas um usuário administrador.
