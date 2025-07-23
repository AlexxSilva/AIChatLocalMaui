# Chat MAUI com IA Local (Ollama 3.1)

Este projeto é um aplicativo mobile feito com .NET MAUI que simula um chat com uma Inteligência Artificial local, utilizando o modelo `llama3.1` rodando via **Ollama API** em rede local.

## Funcionalidades

- Interface em estilo de chat, com bolhas de mensagem para o usuário e a IA.
- Integração com servidor local Ollama (localhost ou IP em rede).
- Exibição de histórico da conversa entre usuário e IA.
- Interface moderna e responsiva.
- Baseado em MVVM e comandos assíncronos.

## Tecnologias

- .NET MAUI
- MVVM (CommunityToolkit.Mvvm)
- Ollama API (`/api/chat`)
- Converters personalizados
- HttpClient com JSON

## Como usar

1. Certifique-se de que o Ollama está rodando localmente com o modelo `llama3.1`.
2. Altere o IP no `OllamaService.cs` se necessário.
3. Rode o app e inicie sua conversa com a IA.

## Captura de Tela

![Exemplo do chat com IA](screenshot.png)
