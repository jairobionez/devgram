using AutoMapper;
using Devgram.Data.Infra;
using Devgram.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
