using Devgram.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devgram.Data.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Guid> CreateAsync(Usuario usuario);
        Task<Usuario?> GetAsync(Guid id);
        Task<IQueryable<Publicacao>> GetPublicacoesAsync();
        Task<IQueryable<Publicacao>> GetAllPublicacoesAsync(string termo);
        Task<IQueryable<Publicacao>> GetPublicacoesAsync(string termo);
        Task<Publicacao?> GetPublicacaoAsync(Guid publicacaoId);
        Task<PublicacaoComentario?> BuscarComentario(Guid usuarioId, Guid comentarioId);
        Task<Publicacao> NovoComentarioAsync(Guid usuarioId, Guid publicacaoId, PublicacaoComentario comentario);
        Task<Publicacao> AlterarComentarioAsync(Guid usuarioId, Guid publicacaoId, Guid comentarioId, PublicacaoComentario comentario);
        Task<Publicacao> RemoverComentarioAsync(Guid usuarioId, Guid publicacaoId, Guid comentarioId);
    }
}
