# TaskFlowEngine 

Este é um projeto para o gerenciamento e processamento de tarefas em background. A arquitetura foca em escalabilidade, resiliência e desacoplamento entre a API e o processamento de dados.

## Tecnologias e Arquitetura
* **Runtime:** .NET 8
* **Banco de Dados:** MongoDB (NoSQL) para persistência de estado das tarefas.
* **Mensageria:** RabbitMQ para orquestração de filas e comunicação entre serviços.
* **Containerização:** Docker e Docker-Compose para ambiente agnóstico.

## Requisitos Implementados 
- **Recebimento de Tarefas:** API pronta para criar jobs com dados dinâmicos em JSON.
- **Processamento em Background:** Worker especializado que consome a fila de forma assíncrona.
- **Sistema de Retry:** Lógica de re-tentativa até 3 vezes para falhas durante o processamento.
- **Gestão de Status:** Transição de estados: `Pendente` -> `EmProcessamento` -> `Concluido` (ou `Erro`).
- **Controle de Concorrência:** Configuração de `BasicQos` no RabbitMQ para suportar múltiplos workers simultâneos.
- **Dockerização:** Todo o ambiente sobe com um único comando.

## Como Executar o Projeto
Certifique-se de ter o **Docker** instalado.

1. Na raiz do projeto, execute: `docker-compose up --build`
2. Acesse o **Swagger** em: `http://localhost:5000/swagger`
3. Acesse o **RabbitMQ** em:
`​http://localhost:15672`

`​Usuário padrão: guest`
`​Senha padrão: guest`
