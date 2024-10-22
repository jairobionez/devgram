using Devgram.Data.Entities;
using Devgram.Data.Interfaces;
using Devgram.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Devgram.Data.Infra;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly DevgramDbContext _context;
    private readonly IAspnetUser _aspnetUser;

    public UsuarioRepository(DevgramDbContext context, IAspnetUser aspnetUser)
    {
        _context = context;
        _aspnetUser = aspnetUser;
    }

    public async Task<Guid> CreateAsync(Usuario usuario)
    {
        await _context.AddAsync(usuario);
        await _context.SaveChangesAsync();

        return usuario.Id;
    }

    public async Task<Usuario?> GetAsync(Guid id)
    {
        return await _context.Set<Usuario>().FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IQueryable<Publicacao>> GetPublicacoesAsync()
    {
        var usuarioId =_aspnetUser.GetUserId();
        
        var resultado = _context.Set<Publicacao>()
            .AsNoTracking()
            .Where(p => p.UsuarioId == usuarioId)
            .OrderByDescending(p => p.DataCriacao);
        
        return await Task.FromResult(resultado);
    }

    public async Task<IQueryable<Publicacao>> GetAllPublicacoesAsync(string termo)
    {

        var resultado = _context.Set<Publicacao>()
            .AsNoTracking()
            .Where(p => (string.IsNullOrEmpty(termo) || p.Titulo.ToUpper().Contains(termo.ToUpper())) ||
                        (string.IsNullOrEmpty(termo) || p.Descricao.ToUpper().Contains(termo.ToUpper())))
            .OrderByDescending(p => p.DataCriacao);

        return await Task.FromResult(resultado);
    }

    public async Task<IQueryable<Publicacao>> GetPublicacoesAsync(string termo)
    {
        var usuarioId =_aspnetUser.GetUserId();
        
        var resultado = _context.Set<Publicacao>()
            .AsNoTracking()
            .Where(p => p.UsuarioId == usuarioId &&
                            (string.IsNullOrEmpty(termo) || p.Titulo.ToUpper().Contains(termo.ToUpper())) ||
                            (string.IsNullOrEmpty(termo) || p.Descricao.ToUpper().Contains(termo.ToUpper())))
            .OrderByDescending(p => p.DataCriacao);
        
        return await Task.FromResult(resultado);
    }
    
    public async Task<Publicacao?> GetPublicacaoAsync(Guid publicacaoId)
    {
        var usuarioId =_aspnetUser.GetUserId();
        
        var resultado = await _context.Set<Publicacao>()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId && p.Id == publicacaoId);
        
        return resultado;
    }

    public async Task<PublicacaoComentario?> BuscarComentario(Guid usuarioId, Guid publicacaoId, Guid comentarioId, bool admin = false)
    {
        return await _context.Set<PublicacaoComentario>()
            .FirstOrDefaultAsync(p => (admin || p.UsuarioId == usuarioId) && p.Id == comentarioId);
    }
    
    public async Task<Publicacao> NovoComentarioAsync(Guid publicacaoId, PublicacaoComentario comentario)
    {
        var publicacao = await _context.Set<Publicacao>()
            .Include(p => p.Comentarios.OrderByDescending(p => p.DataCriacao))
                .ThenInclude(p => p.Usuario)
            .Include(p => p.Usuario)
            .FirstOrDefaultAsync(p => p.Id == publicacaoId);

        if (publicacao.Comentarios.Count == 0)
            comentario.VincularUsuario(
                await _context.Set<Usuario>().FirstOrDefaultAsync(p => p.Id == _aspnetUser.GetUserId()));
        
        publicacao.AdicionarComentario(comentario);
        _context.Set<Publicacao>().Update(publicacao);
        await _context.SaveChangesAsync();

        return publicacao;
    }
    
    public async Task<Publicacao> AlterarComentarioAsync(Guid usuarioId, Guid publicacaoId, Guid comentarioId, PublicacaoComentario comentario)
    {
        var publicacao = await _context.Set<Publicacao>()
            .Include(p => p.Comentarios.OrderByDescending(p => p.DataCriacao))
            .ThenInclude(p => p.Usuario)
            .Include(p => p.Usuario)
            .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId && p.Id == publicacaoId);
        
        publicacao.AtualizarComentario(comentarioId, comentario);
        _context.Set<Publicacao>().Update(publicacao);
        await _context.SaveChangesAsync();

        return publicacao;
    }
    
    public async Task<Publicacao> RemoverComentarioAsync(Guid usuarioId, Guid publicacaoId, Guid comentarioId, bool admin = false)
    {
        var publicacao = await _context.Set<Publicacao>()
            .Include(p => p.Usuario)
            .Include(p => p.Comentarios.OrderByDescending(p => p.DataCriacao))
                .ThenInclude(p => p.Usuario)
            .FirstOrDefaultAsync(p => (admin || p.UsuarioId == usuarioId) && p.Id == publicacaoId);
        
        publicacao.RemoverComentario(comentarioId);
        _context.Set<Publicacao>().Update(publicacao);
        await _context.SaveChangesAsync();

        return publicacao;
    }
}