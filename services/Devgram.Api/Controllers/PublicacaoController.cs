using Devgram.Infra.Repositories;
using Devgram.ViewModel;
using Microsoft.AspNetCore.Mvc;


namespace Devgram.Api.Controllers;

[ApiController]
[Route("api/publicacao")]
public class PublicacaoController : Controller
{
    private readonly PublicacaoRepository _publicacaoRepository;

    public PublicacaoController(PublicacaoRepository publicacaoRepository)
    {
        _publicacaoRepository = publicacaoRepository;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PublicacaoResponseModel>), 200)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> GetAllPublicacoesAsync()
    {
        try
        {
            return Ok(await _publicacaoRepository.GetAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}