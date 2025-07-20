using AIChatLocal.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AIChatLocal.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly OllamaService _ollamaService;

        [ObservableProperty]
        private string mensagem;

        [ObservableProperty]
        private string resposta;

        [ObservableProperty]
        private bool executando;

        public IRelayCommand EnviarCommand { get; }

        public MainViewModel()
        {
            _ollamaService = new OllamaService();
            EnviarCommand = new AsyncRelayCommand(EnviarMensagem, () => !Executando);
        }

        private async Task EnviarMensagem()
        {
            if (string.IsNullOrWhiteSpace(Mensagem))
            {
                Resposta = "Digite uma pergunta válida.";
                return;
            }

            Executando = true;
            EnviarCommand.NotifyCanExecuteChanged();
            Resposta = "Carregando...";

            try
            {
                var resultado = await _ollamaService.EnviarMensagemAsync(Mensagem);
                Resposta = resultado;
            }
            catch (Exception ex)
            {
                Resposta = $"Erro: {ex.Message}";
            }
            finally
            {
                Executando = false;
                EnviarCommand.NotifyCanExecuteChanged();
            }
        }
    }
}
