using System.ComponentModel.DataAnnotations;

namespace Devgram.Data.ViewModels;

public class NovaContaViewModel
{
    [Required(ErrorMessage = "O campo nome nome é necessário")]
    public string Nome { get; set; }
    
    [Required(ErrorMessage = "O campo nome sobrenome é necessário")]
    public string Sobrenome { get; set; }
    
    [Required(ErrorMessage = "O campo e-mail é necessário")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "O campo senha é necessário")]
    public string Senha { get; set; }
    
    [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
    public string ConfirmarSenha { get; set; }
}