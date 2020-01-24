using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.DTO
{
    public class Produto : Default
    {
        public Produto()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MinLength(1)]
        [MaxLength(255)]
        public string Nome { get; set; }

        [MinLength(1)]
        public decimal Valor { get; set; }

        public override void Validate()
        {
            if (Nome.Length < 1)
            {
                AddError("O campo Nome não foi informado.");
            }

            if (Valor <= 0)
            {
                AddError("O campo Valor não pode ser menor ou igual a zero.");
            }
        }
    }
}
