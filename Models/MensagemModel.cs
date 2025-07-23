using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChatLocal.Models
{
    public class MensagemModel
    {
        public string? Texto { get; set; }
        public bool EhUsuario { get; set; }
        public string Remetente => EhUsuario ? "Você" : "Assistente";
    }
}
