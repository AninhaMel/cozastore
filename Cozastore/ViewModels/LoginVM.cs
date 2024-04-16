using System.ComponentModel.DataAnnotations;
namespace Cozastore.ViewModels;

public class LoginVM
{
    [Display(Name = "Email ou Nome do Usuário")]
    [Required(ErrorMessage = "Por favor, informe seu email ou seu nome de usuário")]
    public string Email {get; set;}
    [DataType(DataType.Password)]
    [Display(Name = "Senha de acesso")]
    [Required (ErrorMessage = "Por favor informe sua senha")]
    public string Senha {get; set;}
    [Display(Name = "Manter conectado?")]
    public bool Lembrar { get; set; } = false;

    public string UrlRetorno { get; set; }
}