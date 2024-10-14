using Devgram.Data.Entities;
using Devgram.Data.Enums;
using Devgram.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Devgram.Data.Infra;
public class PublicacaoRepository : IPublicacaoRepository
{
    private readonly DevgramDbContext _context;
    private readonly IAspnetUser _aspnetUser;
    private readonly INotifiable _notifiable;

    public PublicacaoRepository(DevgramDbContext context, IAspnetUser aspnetUser, INotifiable notifiable)
    {
        _context = context;
        _aspnetUser = aspnetUser;
        _notifiable = notifiable;
    }

    public async Task<IQueryable<Publicacao>> GetAsync()
    {
        var resultado = _context.Set<Publicacao>()
                                                    .AsNoTracking();
        return await Task.FromResult(resultado);
    }
    
    public async Task<Publicacao?> GetAsync(Guid id)
    {
        return await _context.Set<Publicacao>()
            .Include(p => p.Comentarios)!
                .ThenInclude(p => p.Usuario)
            .Include(p => p.Comentarios)
                .ThenInclude(p => p.Respostas)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Guid> InsertAsync(Publicacao publicacao)
    {
        await _context.Set<Publicacao>().AddAsync(publicacao);
        await _context.SaveChangesAsync();
        
        return publicacao.Id;
    }
    
    public async Task InsertAsync(List<Publicacao> publicacoes)
    {
        await _context.Set<Publicacao>().AddRangeAsync(publicacoes);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Publicacao> InsertCommentAsync(Guid publicacaoId, PublicacaoComentario comentario)
    {
        var publicacao = await _context.Set<Publicacao>()
                .Include(p => p.Comentarios)
                    .ThenInclude(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Id == publicacaoId);

        publicacao?.AdicionarComentario(comentario);
        
        _context.Set<Publicacao>().Update(publicacao);
        await _context.SaveChangesAsync();
        
        return publicacao!;
    }

    public async Task UpdateAsync(Guid id, Publicacao publicacao)
    {
        var entity = await GetAsync(id);
        
        entity.Atualizar(publicacao);
        
        _context.Set<Publicacao>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var publicacao = await GetAsync(id);

        _context.Set<Publicacao>().Remove(publicacao);
        await _context.SaveChangesAsync();
    }
}