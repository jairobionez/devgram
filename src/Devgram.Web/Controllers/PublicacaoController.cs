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

    [HttpGet("editar-publicacao/{id}")]
    public async Task<ActionResult> EditarPublicacao(Guid id)
    {
        var publicacao = _mapper.Map<PublicacaoResponseModel>(await _publicacaoRepository.GetAsync(id));
        return View(publicacao);
    }

    [HttpPost("editar-publicacao/{id}")]
    public async Task<ActionResult> EditarPublicacao(Guid id, PublicacaoResponseModel publicacao)
    {
        if (ModelState.IsValid)
        {
            if (publicacao.File != null && publicacao.File.Length > 0)
                publicacao.Logo = await UpdateFile(publicacao.File, publicacao.Logo);

            await _publicacaoRepository.UpdateAsync(id, _mapper.Map<Publicacao>(publicacao));
            return RedirectToAction("Index");
        }

        return View(publicacao);
    }

    [HttpGet("remover-publicacao/{id}")]
    public async Task<ActionResult> DeletarPublicacao(Guid id)
    {
        var publicacao = _mapper.Map<PublicacaoResponseModel>(await _publicacaoRepository.GetAsync(id));

        return View(publicacao);
    }

    [HttpDelete("remover-publicacao/{id}")]
    public async Task<JsonResult> ConfirmarDeletarPublicacao(Guid id)
    {
        await _publicacaoRepository.DeleteAsync(id);
        this.AddAlertSuccess("Publicação removida com sucesso!");

        return Json(new { url = "Index" });
    }

    [HttpGet("ler-publicacao/{id}")]
    public async Task<IActionResult> LerPublicacao(Guid id)
    {
        var publicacao = _mapper.Map<PublicacaoResponseModel>(await _publicacaoRepository.GetAsync(id));
        return View(publicacao);
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