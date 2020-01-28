using System.ComponentModel.DataAnnotations;

namespace Domain.RDTO
{
    public class RequestProduto
    {
        public RequestProduto()
        {

        }

        public RequestProduto(string id, string nome, string valor)
        {
            Id = id;
            Nome = nome;
            Valor = valor;
        }

        public string Id { get; set; }

        [MinLength(1)]
        [MaxLength(255)]
        public string Nome { get; set; }

        [MinLength(1)]
        public string Valor { get; set; }
    }
}
