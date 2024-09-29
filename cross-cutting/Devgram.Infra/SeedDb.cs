using Devgram.Infra.Entities;
using Devgram.Infra.Repositories;
using Devgram.ViewModel.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Devgram.Infra;

public static class SeedDb
{
    public static void DbInitializer(this IServiceCollection services)
    {
        IServiceProvider provider = services.BuildServiceProvider();

        using (var serviceScope = provider?.GetService<IServiceScopeFactory>()?.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<DbContext>();

            context.Database.Migrate();

            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var passwordHash = serviceScope.ServiceProvider.GetRequiredService<IPasswordHasher<IdentityUser>>();
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var usuarioRepository = serviceScope.ServiceProvider.GetRequiredService<UsuarioRepository>();
            var publicacaoRepository = serviceScope.ServiceProvider.GetRequiredService<PublicacaoRepository>();

            var usuarioDb = userManager.FindByEmailAsync("jairo@devgram.com.br").Result;

            if (usuarioDb is null)
            {
                var identityUser = new IdentityUser("jairo@devgram.com.br");
                identityUser.PasswordHash = passwordHash.HashPassword(identityUser, "Teste@123");
                identityUser.Email = "jairo@devgram.com.br";

                var identityResult = userManager.CreateAsync(identityUser).Result;

                if (identityResult.Succeeded)
                {
                    CreateRoles(roleManager).Wait();
                    userManager.AddToRoleAsync(identityUser, nameof(PerfilUsuarioEnum.USER)).Wait();

                    var usuario = new Usuario(Guid.Parse(identityUser.Id), "Jairo", "Bionez", "jairo@devgram.com.br");
                    usuarioRepository.CreateAsync(usuario).Wait();

                    var hash = passwordHash.HashPassword(identityUser, "Teste@123");
                    identityUser.SecurityStamp = Guid.NewGuid().ToString();
                    identityUser.PasswordHash = hash;
                    userManager.UpdateAsync(identityUser).Wait();
                }

                var publicacoes = new List<Publicacao>()
                {
                    new Publicacao(
                        "A Revolução da Inteligência Artificial no Desenvolvimento de Software",
                        "A inteligência artificial (IA) está transformando a maneira como desenvolvemos software. Desde a automação de tarefas repetitivas até a criação de soluções personalizadas, a IA está acelerando o ciclo de desenvolvimento e melhorando a qualidade dos produtos finais.",
                        Guid.Parse(identityUser.Id),
                        "https://via.placeholder.com/1340x480?text=Revolução+da+inteligência+artificial"),
                    new Publicacao(
                        "Dicas Essenciais para Aumentar a Segurança em Aplicações Web",
                        "A segurança de aplicações web é uma das principais preocupações para desenvolvedores e empresas. Confira nossas cinco dicas essenciais para proteger suas aplicações contra as ameaças mais comuns, como ataques de injeção SQL, XSS e CSRF.",
                        Guid.Parse(identityUser.Id),
                        "https://via.placeholder.com/1340x480?text=Segurança+web"),
                    new Publicacao(
                        "Como a Nuvem Está Redefinindo a Infraestrutura de TI",
                        "O uso de soluções baseadas em nuvem está revolucionando a infraestrutura de TI, proporcionando escalabilidade, flexibilidade e economia de custos. Empresas de todos os portes estão migrando para a nuvem em busca de maior eficiência operacional.",
                        Guid.Parse(identityUser.Id),
                        "https://via.placeholder.com/1340x480?text=Soluções+nuvem"),
                    new Publicacao(
                        "Angular 18: As Novidades da Mais Recente Versão",
                        "A versão 18 do Angular trouxe uma série de melhorias de performance e novas funcionalidades que facilitam o desenvolvimento de aplicações web dinâmicas. Entre as principais novidades, destacam-se a otimização no carregamento de componentes e o suporte aprimorado para SSR.",
                        Guid.Parse(identityUser.Id),
                        "https://via.placeholder.com/1340x480?text=Angular+18+novidades"),
                };

                publicacaoRepository.InsertAsync(publicacoes).Wait();
            }

            var adminDb = userManager.FindByEmailAsync("admin@devgram.com.br").Result;

            if (adminDb is null)
            {
                var identityUser = new IdentityUser("admin@devgram.com.br");
                identityUser.PasswordHash = passwordHash.HashPassword(identityUser, "Teste@123");
                identityUser.Email = "admin@devgram.com.br";

                var identityResult = userManager.CreateAsync(identityUser).Result;

                if (identityResult.Succeeded)
                {
                    CreateRoles(roleManager).Wait();
                    userManager.AddToRoleAsync(identityUser, nameof(PerfilUsuarioEnum.ADMIN)).Wait();

                    var usuario = new Usuario(Guid.Parse(identityUser.Id), "Admin", "Geral", "admin@devgram.com.br");
                    usuarioRepository.CreateAsync(usuario).Wait();

                    var hash = passwordHash.HashPassword(identityUser, "Teste@123");
                    identityUser.SecurityStamp = Guid.NewGuid().ToString();
                    identityUser.PasswordHash = hash;
                    userManager.UpdateAsync(identityUser).Wait();
                }
            }
        }
    }

    private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
    {
        string[] rolesNames =
        {
            nameof(PerfilUsuarioEnum.ADMIN),
            nameof(PerfilUsuarioEnum.USER),
        };

        foreach (var namesRole in rolesNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(namesRole);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(namesRole));
            }
        }
    }
}