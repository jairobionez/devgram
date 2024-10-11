
# Feedback do Instrutor

#### 11/10/24 - Revisão Inicial - Eduardo Pires

## Pontos Positivos:

- Uso adequado de ferramentas como AutoMapper.
- Controle eficiente de usuários com autorização e registro de permissões por Roles.
- Demonstrou sólido conhecimento em Identity e JWT.
- Mostrou entendimento do ecossistema de desenvolvimento .NET
- Bom uso da TagHelper para Alerts
- Uso adequado de extension methods para configurações
- Uso interessante de notificações com a classe Notifiable


## Pontos Negativos:

- Uso desnecessário de camadas e complexidades para o tipo de projeto.
- Achei confusa a distribuição de camadas e organização das responsabilidades
    - Não existe necessidade de uma camada ViewModels
    - As entidades não deveriam estar dentro da camada de infra
    - Uma vez que conceitos de DDD são implementados eles precisam seguir um determinado padrão de implementação sem violar premissas importantes.
- Uso indevido ou falta de uso de padrões importantes
    - Não utilizou interfaces para classes injetadas (ex repositórios)
    - Uso desnecesário de try-catch em controllers
    - Não se implementa regra de negócios em repositório "PossuiPermissao"
    - Uso desnecessário da classe Startup nos projetos Web
    - Refez algo que já vem pronto ao não utilizar as views e mecanismos do Identity
    - Uso desnecessário de NewtonsoftJson sendo que o System.Text.Json é mais eficiente.
- Não deu suporte ao SQLite

## Sugestões:

- Para um projeto dessa complexidade, seria mais viável criar apenas uma camada de aplicação para compartilhar o controle das chamadas entre MVC e API, dispensando as demais camadas.
- Reserve a implementação de estruturas mais complexas para projetos críticos que ainda virão; o Módulo 1 não abrange tudo isso.
- Recomendo rever conceitos importantes de padronização no projeto todo conforme problemas apontados.

## Problemas:

- Não consegui executar a aplicação de imediato na máquina. É necessário que o Seed esteja configurado corretamente, com uma connection string apontando para o SQLite.

  **P.S.** As migrations precisam ser geradas com uma conexão apontando para o SQLite; caso contrário, a aplicação não roda.
