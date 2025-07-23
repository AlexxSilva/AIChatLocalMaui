using AIChatLocal.Models;
using AIChatLocal.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
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
        private bool executando;

        public ObservableCollection<MensagemModel> Conversa { get; } = new();

        public IRelayCommand EnviarCommand { get; }

        public MainViewModel()
        {
            _ollamaService = new OllamaService();
            EnviarCommand = new AsyncRelayCommand(EnviarMensagemAsync, () => !Executando);
        }

        private async Task EnviarMensagemAsync()
        {
            if (string.IsNullOrWhiteSpace(Mensagem))
                return;

            var mensagemUsuario = Mensagem;

            Conversa.Add(new MensagemModel
            {
                Texto = mensagemUsuario,
                EhUsuario = true
            });

            Mensagem = string.Empty;
            Executando = true;
            EnviarCommand.NotifyCanExecuteChanged();

            try
            {
                var resposta = await _ollamaService.EnviarMensagemAsync(mensagemUsuario);

                Conversa.Add(new MensagemModel
                {
                    Texto = resposta,
                    EhUsuario = false
                });
            }
            catch (Exception ex)
            {
                Conversa.Add(new MensagemModel
                {
                    Texto = $"Erro: {ex.Message}",
                    EhUsuario = false
                });
            }
            finally
            {
                Executando = false;
                EnviarCommand.NotifyCanExecuteChanged();
            }
        }
    }
}
