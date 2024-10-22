using AutoMapper;
using Devgram.Data.Entities;
using Devgram.Data.Infra;
using Devgram.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Devgram.Web.Extensions;
using Devgram.Data.Interfaces;
using Devgram.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Devgram.Web.Controllers;

[Route("publicacao")]
public class PublicacaoController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPublicacaoRepository _publicacaoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IAspnetUser _aspnetUser;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly INotifiable _notifiable;

    public PublicacaoController(
        ILogger<HomeController> logger,
        IPublicacaoRepository publicacaoRepository,
        IUsuarioRepository usuarioRepository,
        IMapper mapper,
        IWebHostEnvironment webHostEnvironment,
        IAspnetUser aspnetUser,
        INotifiable notifiable)
    {
        _logger = logger;
        _publicacaoRepository = publicacaoRepository;
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
        _aspnetUser = aspnetUser;
        _notifiable = notifiable;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var role = _aspnetUser.GetUserRole();

        if (role == nameof(PerfilUsuarioEnum.ADMIN))
            return View(_mapper.Map<List<PublicacaoResponseModel>>(
                 await _usuarioRepository.GetAllPublicacoesAsync(string.Empty)));

        return View(_mapper.Map<List<PublicacaoResponseModel>>(
             await _usuarioRepository.GetPublicacoesAsync()));
    }

    [HttpPost("filtrar")]
    public async Task<ActionResult> FiltrarPublicacoes(PublicacaoFiltroModel model)
    {
        var role = _aspnetUser.GetUserRole();

        if (role == nameof(PerfilUsuarioEnum.ADMIN))
            return PartialView("_ListaPublicacoes", _mapper.Map<List<PublicacaoResponseModel>>(
            await _usuarioRepository.GetAllPublicacoesAsync(model.Termo)));

        return PartialView("_ListaPublicacoes", _mapper.Map<List<PublicacaoResponseModel>>(
            await _usuarioRepository.GetPublicacoesAsync(model.Termo)));
    }

    [HttpGet("nova-publicacao")]
    public async Task<ActionResult> NovaPublicacao()
    {
        return View();
    }

    [HttpPost("nova-publicacao")]
    public async Task<ActionResult> NovaPublicacao(PublicacaoModel publicacao)
    {
        if (ModelState.IsValid)
        {
            if (publicacao.File != null && publicacao.File.Length > 0)
                publicacao.Logo = await UpdateFile(publicacao.File);

            await _publicacaoRepository.InsertAsync(_mapper.Map<Publicacao>(publicacao));
            return RedirectToAction("Index");
        }

        return View(publicacao);
    }

    [HttpGet("{publicacaoId}/editar-publicacao")]
    public async Task<ActionResult> EditarPublicacao(Guid publicacaoId)
    {
        var publicacao = _mapper.Map<PublicacaoResponseModel>(await _publicacaoRepository.GetAsync(publicacaoId));
        return View(publicacao);
    }

    [HttpPost("{publicacaoId}/editar-publicacao")]
    public async Task<ActionResult> EditarPublicacao(Guid publicacaoId, PublicacaoResponseModel publicacao)
    {
        if (ModelState.IsValid)
        {
            if (publicacao.File != null && publicacao.File.Length > 0)
                publicacao.Logo = await UpdateFile(publicacao.File, publicacao.Logo);

            await _publicacaoRepository.UpdateAsync(publicacaoId, _mapper.Map<Publicacao>(publicacao));
            return RedirectToAction("Index");
        }

        return View(publicacao);
    }

    [HttpGet("{publicacaoId}/remover-publicacao")]
    public async Task<ActionResult> DeletarPublicacao(Guid publicacaoId)
    {
        var publicacao = _mapper.Map<PublicacaoResponseModel>(await _publicacaoRepository.GetAsync(publicacaoId));

        return View(publicacao);
    }

    [HttpDelete("{publicacaoId}/remover-publicacao")]
    public async Task<JsonResult> ConfirmarDeletarPublicacao(Guid publicacaoId)
    {
        await _publicacaoRepository.DeleteAsync(publicacaoId);
        this.AddAlertSuccess("Publicação removida com sucesso!");

        return Json(new { url = "Index" });
    }

    [HttpGet("{publicacaoId}/ler-publicacao")]
    public async Task<IActionResult> LerPublicacao(Guid publicacaoId)
    {
        var publicacao = _mapper.Map<PublicacaoResponseModel>(await _publicacaoRepository.GetAsync(publicacaoId));
        return View(publicacao);
    }

    [HttpPost("{publicacaoId}/comentar")]
    public async Task<IActionResult> ComentarPublicacao(Guid publicacaoId, [FromBody] PublicacaoComentarioModel model)
    {
        var publicacao = await _usuarioRepository.NovoComentarioAsync(publicacaoId, _mapper.Map<PublicacaoComentario>(model));
        
        this.AddAlertSuccess("Comentário adicionado com sucesso!");
        return PartialView("_ListaComentarios", _mapper.Map<PublicacaoResponseModel>(publicacao));
    }
    
    [HttpGet("{publicacaoId}/alterar-comentario/{id}")]
    public async Task<IActionResult> AlterarComentario(Guid publicacaoId, Guid id)
    {
        var usuarioId = _aspnetUser.GetUserId();
        var comentario = _mapper.Map<PublicacaoComentarioResponseModel>(
            await _usuarioRepository.BuscarComentario(usuarioId!.Value, publicacaoId, id));
        
        return View(comentario);
    }
    
    [HttpPost("{publicacaoId}/alterar-comentario/{id}")]
    public async Task<IActionResult> ConfirmarAlterarComentario(Guid id, Guid publicacaoId, [FromBody] PublicacaoComentarioModel model)
    {
        var usuarioId = _aspnetUser.GetUserId();
        var publicacao = await _usuarioRepository.AlterarComentarioAsync(usuarioId!.Value, publicacaoId, id, 
            _mapper.Map<PublicacaoComentario>(model));
        
        this.AddAlertSuccess("Comentário alterado com sucesso!");
        return PartialView("_ListaComentarios", _mapper.Map<PublicacaoResponseModel>(publicacao));
    }
    
    [HttpGet("{publicacaoId}/remover-comentario/{id}")]
    public async Task<ActionResult> DeletarComentario(Guid id, Guid publicacaoId)
    {
        var usuarioId = _aspnetUser.GetUserId();
        
        var comentario = _mapper.Map<PublicacaoComentarioResponseModel>(
            await _usuarioRepository.BuscarComentario(usuarioId!.Value, publicacaoId, id, _aspnetUser.Admin()));

        return View(comentario);
    }
    
    [HttpPost("{publicacaoId}/remover-comentario/{id}")]
    public async Task<IActionResult> ConfirmarDeletarComentario(Guid id, Guid publicacaoId)
    {
        var usuarioId = _aspnetUser.GetUserId();
        var publicacao = await _usuarioRepository.RemoverComentarioAsync(usuarioId!.Value, publicacaoId, id, _aspnetUser.Admin());
        this.AddAlertSuccess("Comentário removido com sucesso!");

        return PartialView("_ListaComentarios", _mapper.Map<PublicacaoResponseModel>(publicacao));
    }

    private async Task<string> UpdateFile(IFormFile file, string? existingFile = null)
    {
        string uploadsFolder = Path.Combine("wwwroot/publicacoes");
        string uniqueFileName = string.Empty;
        if (existingFile != null)
        {
            var fileName = existingFile.Split('/').Last();
            uniqueFileName = $"{fileName.Split('.')[0]}{Path.GetExtension(file.FileName)}";
        }
        else
            uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return $"/publicacoes/{uniqueFileName}";
    }
}