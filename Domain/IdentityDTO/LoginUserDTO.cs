using System.ComponentModel.DataAnnotations;

namespace Domain.DTO
{
    public class LoginUserDTO
    {
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }
        public string Username { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 5)]
        public string Password { get; set; }
    }
}
