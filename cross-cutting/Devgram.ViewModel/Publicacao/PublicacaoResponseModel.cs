using Microsoft.AspNetCore.Http;

namespace Devgram.ViewModel;

public class PublicacaoResponseModel
{
    public Guid Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public string? Logo { get; set; }
    public Guid UsuarioId { get; set; }
    public DateTime? DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    public IFormFile File { get; set; }

    public virtual ICollection<PublicacaoComentarioResponseModel>? Comentarios { get;  set; }
}

public class PublicacaoComentarioResponseModel
{
    public Guid Id { get; set; }
    public string? Descricao { get; set; }
    public bool Editado { get; set; }
    public Guid PublicacaoId { get; set; }
    public Guid? UsuarioId { get; set; }
    public UsuarioResponseModel Usuario { get; set; }
    public DateTime? DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    
    public virtual ICollection<PublicacaoComentarioResponseModel>? Respostas { get; set; }
}