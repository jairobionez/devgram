using Devgram.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devgram.Data.Interfaces
{
    public interface IPublicacaoRepository
    {
        Task<IQueryable<Publicacao>> GetAsync();
        Task<Publicacao?> GetAsync(Guid id);
        Task<Guid> InsertAsync(Publicacao publicacao);
        Task InsertAsync(List<Publicacao> publicacoes);
        Task<Publicacao> InsertCommentAsync(Guid publicacaoId, PublicacaoComentario comentario);
        Task UpdateAsync(Guid id, Publicacao publicacao);
        Task DeleteAsync(Guid id);

    }
}
