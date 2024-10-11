using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Devgram.Auth.Extensions;
using Devgram.Data.Entities;
using Devgram.Data.Enums;
using Devgram.Data.Infra;
using Devgram.Data.ViewModels;
using Devgram.Web.Extensions;
using Microsoft.Extensions.Options;
using Devgram.Data.Interfaces;

namespace Devgram.Web.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AppSettings _appSettings;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher<IdentityUser> _passwordHasher;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthController(ILogger<AuthController> logger, UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager,
        IOptions<AppSettings> appSettings, IUsuarioRepository usuarioRepository, IHttpContextAccessor httpContextAccessor, IPasswordHasher<IdentityUser> passwordHasher)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _usuarioRepository = usuarioRepository;
        _httpContextAccessor = httpContextAccessor;
        _passwordHasher = passwordHasher;
        _appSettings = appSettings.Value;
    }
    
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        if(User.Identity.IsAuthenticated) 
            return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginAsync(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await
                _signInManager.PasswordSignInAsync(model.Email, model.Senha, false, false);

            if (result.Succeeded)
            {
                var token = await GerarJwt(model.Email, string.Empty);
                Response.Cookies.Append("Token", token.AccessToken, new CookieOptions
                {
                    HttpOnly = true, 
                    Secure = true,   
                    SameSite = SameSiteMode.Strict, 
                });
                
                this.AddAlertSuccess("Bem-vindo(a) ao Devgram!!");
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Email ou senha incorretos.");
        }

        return View(model);
    }

    [HttpGet("nova-conta")]
    public IActionResult NovaConta()
    {
        if(User.Identity.IsAuthenticated) 
            return RedirectToAction("Index", "Home");
        
        return View();
    }

    [HttpPost("nova-conta")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NovaContaAsync(NovaContaViewModel model)
    {
        if (ModelState.IsValid)
        {
            var identityUser = new IdentityUser(model.Email);
            identityUser.PasswordHash = _passwordHasher.HashPassword(identityUser, model.Senha);
            identityUser.Email = model.Email;
            
            var result = await _userManager.CreateAsync(identityUser);

            if (result.Succeeded)
            {
                await CreateRoles();
                await _userManager.AddToRoleAsync(identityUser, nameof(PerfilUsuarioEnum.USER));
                
                var usuario = new Usuario(Guid.Parse(identityUser.Id), model.Nome, model.Sobrenome, model.Email);
                await _usuarioRepository.CreateAsync(usuario);
                
                this.AddAlertSuccess("Conta criada com sucesso!");
                
                return RedirectToAction("Login", "Auth");
            }

            var erros = result.Errors.Select(x => x.Description).ToArray();
            ModelState.AddModelError(string.Empty, string.Join('\n', erros));
        }

        return View(model);
    }

    [HttpGet("logout")]
    public IActionResult LogoutAsync()
    {
        Response.Cookies.Delete("Token");
        return RedirectToAction("Index", "Home");
    }
    
    private async Task CreateRoles()
    {
        string[] rolesNames =
        {
            nameof(PerfilUsuarioEnum.ADMIN),
            nameof(PerfilUsuarioEnum.USER),
        };

        foreach (var namesRole in rolesNames)
        {
            var roleExist = await _roleManager.RoleExistsAsync(namesRole);
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole(namesRole));
            }
        }
    }

    private async Task<UsuarioLoginResponseModel> GerarJwt(string email, string nomeUsuario)
    {
        var identityUser = await _userManager.FindByEmailAsync(email);
        if (string.IsNullOrEmpty(nomeUsuario))
        {
            var usuarioDb = await _usuarioRepository.GetAsync(Guid.Parse(identityUser.Id)); 
            nomeUsuario = $"{usuarioDb.Nome} {usuarioDb.Sobrenome}";
        }

        var claims = await _userManager.GetClaimsAsync(identityUser);

        var identityClaims = await ObterClaimsUsuario(claims, identityUser, nomeUsuario);
        var encodedToken = CodificarToken(identityClaims);

        return await ObterRespostaToken(encodedToken, identityUser, nomeUsuario, claims);
    }

    private async Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims, IdentityUser identityUser,
        string usuarioNome)
    {
        var userRoles = await _userManager.GetRolesAsync(identityUser);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, identityUser.Id.ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, identityUser.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Name, usuarioNome));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(),
            ClaimValueTypes.Integer64));
        claims.Add(new Claim(JwtRegisteredClaimNames.Exp,
            ToUnixEpochDate(DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras)).ToString(),
            ClaimValueTypes.Integer64));

        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim("role", userRole));
        }

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        return identityClaims;
    }

    private string CodificarToken(ClaimsIdentity identityClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _appSettings.Emissor,
            Audience = _appSettings.ValidoEm,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        return tokenHandler.WriteToken(token);
    }

    private async Task<UsuarioLoginResponseModel> ObterRespostaToken(string encodedToken,
        IdentityUser identityUser, string nomeUsuario, IEnumerable<Claim> claims)
    {
        return new UsuarioLoginResponseModel
        {
            Authenticated = true,
            AccessToken = encodedToken,
            ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalHours,
            UsuarioToken = new UsuarioToken
            {
                Id = identityUser.Id,
                Email = identityUser.Email,
                Nome = nomeUsuario,
                Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
            }
        };
    }

    private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
            .TotalSeconds);
}