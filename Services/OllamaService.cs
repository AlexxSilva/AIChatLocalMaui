using System.Text;
using System.Text.Json;


namespace AIChatLocal.Services
{
    public class OllamaService
    {
        private readonly HttpClient _httpClient;

        private readonly List<Dictionary<string, string>> _conversaAtual = new()
        {
            new() { ["role"] = "system", ["content"] = "Você é um assistente útil." }
        };


        public OllamaService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://192.168.0.40:11434") //IP onde está rodando o Ollama
            };
        }

       
        public async Task<string> EnviarMensagemAsync(string mensagem)
        {
            try
            {
                _conversaAtual.Add(new() { ["role"] = "user", ["content"] = mensagem });

                var request = new
                {
                    model = "llama3.1", // Nome do modelo rodando no Ollama
                    messages = _conversaAtual,
                    stream = false,
                    max_tokens = 100,
                    temperature = 0.7
                };

                var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/chat", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Erro HTTP {(int)response.StatusCode}: {response.ReasonPhrase}. Corpo: {errorBody}");
                }

                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                var resposta = "";
                while (!reader.EndOfStream)
                {
                    var linha = await reader.ReadLineAsync();
                    if (linha?.StartsWith("{") == true)
                    {
                        var json = JsonDocument.Parse(linha);
                        if (json.RootElement.TryGetProperty("message", out var msg))
                        {
                            if (msg.TryGetProperty("content", out var r))
                            {
                                resposta += r.GetString();
                            }
                        }
                    }
                }

                _conversaAtual.Add(new() { ["role"] = "assistant", ["content"] = resposta });

                return resposta;
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception("Erro na comunicação HTTP: " + httpEx.Message, httpEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado: " + ex.Message, ex);
            }

        }
    }
}
