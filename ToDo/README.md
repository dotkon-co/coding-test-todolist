# Projeto


## Dependencia para execução

* Docker

Na pasta src, executar:
```
docker-compose up -d
```

O composer vai gerar a execução do banco e da aplicação (atentar apenas que o container da aplicação pode parar pelas configurações iniciais do banco ainda não estarem prontas, só rodar o container do app caso aconteça)


Dockerfile -> Arquivo dockerfile para build da imagem

DockerCompose -> Composer com start do banco e aplicação

Migration -> Será executada e migration o start da aplicação


## Considerações
Como no próprio requisito era a aplicação estar rodando, me concentrei até o momento na entrega em exeução para avaliação. Mas vou dar continuidade com mais calma (até por estudo de teste de integração que não é algo de vivência do meu dia dia)

* **Mediator**: Comecei com mediator, mas removi pela simplicidade e concentração das validações, requests e responses na **Domain**

* **Testes Unitários**: Criei o escopo mas ainda não iniciei

* **Testes de Integração**: Vou criar testes de integração também

* **Release**: Vou provisionar recursos na Azure e esteiras CI/CD