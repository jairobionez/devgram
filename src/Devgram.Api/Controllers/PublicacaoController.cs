using Devgram.Data.Infra;
using Devgram.Data.Interfaces;
using Devgram.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace Devgram.Api.Controllers;

[ApiController]
[Route("api/publicacao")]
public class PublicacaoController : Controller
{
    private readonly IPublicacaoRepository _publicacaoRepository;

    public PublicacaoController(IPublicacaoRepository publicacaoRepository)
    {
        _publicacaoRepository = publicacaoRepository;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PublicacaoResponseModel>), 200)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> GetAllPublicacoesAsync()
    {
        return Ok(await _publicacaoRepository.GetAsync());
    }
}