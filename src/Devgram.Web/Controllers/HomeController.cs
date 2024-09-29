using System.Diagnostics;
using AutoMapper;
using Devgram.Infra.Repositories;
using Devgram.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Devgram.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace Devgram.Web.Controllers;

[Route("home")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PublicacaoRepository _publicacaoRepository;
    private IMapper _mapper;

    public HomeController(ILogger<HomeController> logger, PublicacaoRepository publicacaoRepository, IMapper mapper)
    {
        _logger = logger;
        _publicacaoRepository = publicacaoRepository;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var publicacoes = _mapper.Map<List<PublicacaoResponseModel>>(await _publicacaoRepository.GetAsync()); 
        return View(publicacoes);
    }
}
