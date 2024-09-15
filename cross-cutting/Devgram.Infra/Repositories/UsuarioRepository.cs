using Devgram.Infra.Entities;

namespace Devgram.Infra.Repositories;

public class UsuarioRepository
{
    private readonly DbContext _context;

    public UsuarioRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateAsync(Usuario usuario)
    {
        await _context.AddAsync(usuario);
        await _context.SaveChangesAsync();

        return usuario.Id;
    }
}