using System.Collections.Generic;
using System.Linq;

namespace Domain.DTO
{
    public abstract class Default
    {
        private List<string> _mensagemValidacao { get; set; }

        protected List<string> MensagemValidacao
        {
            get { return _mensagemValidacao ?? (_mensagemValidacao = new List<string>()); }
        }

        protected void ClearValidateMensages()
        {
            MensagemValidacao.Clear();
        }

        protected void AddError(string mensagem)
        {
            MensagemValidacao.Add(mensagem);
        }

        public abstract void Validate();

        protected bool isValid
        {
            get { return MensagemValidacao.Any(); }
        }
    }
}
