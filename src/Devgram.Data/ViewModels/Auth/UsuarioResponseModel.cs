namespace Devgram.Data.ViewModels;

public class UsuarioResponseModel
{
    public Guid Id { get; set; }
    public string? Nome { get; private set; }
    public string? Sobrenome { get; private set; }
    public string? Email { get; private set; }
    public string NomeCompleto { get; private set; }
}