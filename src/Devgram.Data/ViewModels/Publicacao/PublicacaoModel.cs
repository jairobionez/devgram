using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Devgram.Data.ViewModels;

public class PublicacaoModel
{
    [Required(ErrorMessage = "Campo obrigátorio")]
    [MaxLength(100)]
    public string? Titulo { get; set; }
    
    [Required(ErrorMessage = "Campo obrigátorio")]
    [MaxLength(500)]
    public string? Descricao { get; set; }
    
    public Guid UsuarioId { get; set; }
    public string? Logo { get; set; }
    public IFormFile? File { get; set; }
}

public class PublicacaoComentarioModel
{
    public Guid Id { get; set; }
    public string? Descricao { get; set; }
    public bool Editado { get; set; }
    public Guid PublicacaoId { get; set; }
    public Guid? UsuarioId { get; set; }
    public Guid? ComentarioPaiId { get; private set; }
}