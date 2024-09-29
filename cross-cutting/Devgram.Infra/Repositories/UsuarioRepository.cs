using Devgram.Infra.Entities;
using Devgram.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Devgram.Infra.Repositories;

public class UsuarioRepository
{
    private readonly DbContext _context;
    private readonly IAspnetUser _aspnetUser;

    public UsuarioRepository(DbContext context, IAspnetUser aspnetUser)
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
            .Where(p => p.UsuarioId == usuarioId);
        
        return await Task.FromResult(resultado);
    }
    
    public async Task<IQueryable<Publicacao>> GetPublicacoesAsync(string termo)
    {
        var usuarioId =_aspnetUser.GetUserId();
        
        var resultado = _context.Set<Publicacao>()
            .AsNoTracking()
            .Where(p => p.UsuarioId == usuarioId &&
                            (string.IsNullOrEmpty(termo) || p.Titulo.ToUpper().Contains(termo.ToUpper())) ||
                            (string.IsNullOrEmpty(termo) || p.Descricao.ToUpper().Contains(termo.ToUpper())));
        
        return await Task.FromResult(resultado);
    }
}