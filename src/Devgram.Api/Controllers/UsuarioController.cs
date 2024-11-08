using AutoMapper;
using Devgram.Data.Entities;
using Devgram.Data.Enums;
using Devgram.Data.Infra;
using Devgram.Data.Interfaces;
using Devgram.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Devgram.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/usuario")]
public class UsuarioController : Controller
{
    private readonly IPublicacaoRepository _publicacaoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMapper _mapper;
    private readonly INotifiable _notifiable;
    private readonly IAspnetUser _aspnetUser;

    public UsuarioController(IPublicacaoRepository publicacaoRepository, IUsuarioRepository usuarioRepository,
        IMapper mapper, INotifiable notifiable, IAspnetUser aspnetUser)
    {
        _publicacaoRepository = publicacaoRepository;
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
        _notifiable = notifiable;
        _aspnetUser = aspnetUser;
    }
    
    /// <summary>
    /// Responsável por listar todas as publicações de um usuário
    /// </summary>
    /// <response code="200">Lista de publicações de um usuário</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="500">Erro interno</response>
    [HttpGet("{usuarioId}/publicacao")]
    [ProducesResponseType(typeof(IEnumerable<PublicacaoResponseModel>), 200)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> GetAllPublicacoesAsync([FromQuery] PublicacaoFiltroModel model)
    {
        var resultado =
            _mapper.Map<List<PublicacaoResponseModel>>(await _usuarioRepository.GetPublicacoesAsync(model.Termo));
        return Ok(resultado);
    }

    
    /// <summary>
    /// Responsável por buscar uma publicação específica do usuário
    /// </summary>
    /// <response code="200">Publicação do usuário</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="500">Erro interno</response>
    [HttpGet("{usuarioId}/publicacao/{publicacaoId}")]
    [ProducesResponseType(typeof(IEnumerable<PublicacaoResponseModel>), 200)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> GetPublicacaosAsync(Guid usuarioId, Guid publicacaoId)
    {
        var resultado =
            _mapper.Map<List<PublicacaoResponseModel>>(await _usuarioRepository.GetPublicacaoAsync(publicacaoId));
        return Ok(resultado);
    }
    
    /// <summary>
    /// Responsável por inserir uma publicação específica para usuário
    /// </summary>
    /// <response code="200">Identificador do usuário</response>
    /// <response code="400">Conteúdo inválido</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="500">Erro interno</response>
    [HttpPost("{usuarioId}/publicacao")]
    [ProducesResponseType(typeof(IEnumerable<PublicacaoResponseModel>), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> NovaPublicacaoAsync(Guid usuarioId, [FromForm] PublicacaoModel model)
    {
        if (model.File != null && model.File.Length > 0)
            model.Logo = await UpdateFile(model.File, model.Logo);

        var resultado = await _publicacaoRepository.InsertAsync(_mapper.Map<Publicacao>(model));

        if (_notifiable.HasNotification)
            return Unauthorized(_notifiable.GetNotifications);

        return Ok(resultado);
    }

    /// <summary>
    /// Responsável por alterar uma publicação específica para usuário
    /// </summary>
    /// <response code="200"></response>
    /// <response code="400">Conteúdo inválido</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="500">Erro interno</response>
    [HttpPut("{usuarioId}/publicacao/{publicacaoId}")]
    [ProducesResponseType(typeof(IEnumerable<PublicacaoResponseModel>), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> AtualizarPublicacaoAsync(Guid usuarioId, Guid publicacaoId,
        [FromForm] PublicacaoModel model)
    {
        if (model.File != null && model.File.Length > 0)
            model.Logo = await UpdateFile(model.File, model.Logo);

        await _publicacaoRepository.UpdateAsync(publicacaoId, _mapper.Map<Publicacao>(model));

        if (_notifiable.HasNotification)
            return Unauthorized(_notifiable.GetNotifications);

        return Ok();
    }

    /// <summary>
    /// Responsável por remover uma publicação específica para usuário
    /// </summary>
    /// <response code="200"></response>
    /// <response code="400">Conteúdo inválido</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="500">Erro interno</response>
    [HttpDelete("{usuarioId}/publicacao/{publicacaoId}")]
    [ProducesResponseType(typeof(IEnumerable<PublicacaoResponseModel>), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> RemoverPublicacaoAsync(Guid usuarioId, Guid publicacaoId)
    {
        await _publicacaoRepository.DeleteAsync(publicacaoId);

        if (_notifiable.HasNotification)
            return Unauthorized(_notifiable.GetNotifications);

        return Ok();
    }
    
       
    /// <summary>
    /// Responsável por adicionar um comentário em determinada publicação
    /// </summary>
    /// <response code="200">Publicação no qual o comentário foi inserido</response>
    /// <response code="400">Conteúdo inválido</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="500">Erro interno</response>
    [HttpPost("{usuarioId}/publicacao/{publicacaoId}/comentario")]
    [ProducesResponseType(typeof(Guid), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> NovoComentario(Guid usuarioId, Guid publicacaoId, [FromBody] PublicacaoComentarioModel model)
    {
        var resultado = await _usuarioRepository.NovoComentarioAsync(publicacaoId, _mapper.Map<PublicacaoComentario>(model));
        
        return Ok(resultado);
    }
    
    /// <summary>
    /// Responsável por alterar um comentário em determinada publicação
    /// </summary>
    /// <response code="200">Publicação no qual o comentário foi inserido</response>
    /// <response code="400">Conteúdo inválido</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="500">Erro interno</response>
    [HttpPut("{usuarioId}/publicacao/{publicacaoId}/comentario/{comentarioId}")]
    [ProducesResponseType(typeof(Guid), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> AlterarComentario(Guid usuarioId, Guid publicacaoId, Guid comentarioId, [FromBody] PublicacaoComentarioModel model)
    {
        var usuarioLogadoId = _aspnetUser.GetUserId();
        
        if (usuarioId != usuarioLogadoId)
        {
            _notifiable.AddNotification("Falha ao remover comentário, privilégios insuficientes.");
            return Unauthorized(_notifiable.GetNotifications);
        }
        
        var resultado = await _usuarioRepository.AlterarComentarioAsync(
            usuarioId, publicacaoId, comentarioId, _mapper.Map<PublicacaoComentario>(model));
        
        return Ok(resultado);
    }
    
    /// <summary>
    /// Responsável por remover um comentário em determinada publicação
    /// </summary>
    /// <response code="200"></response>
    /// <response code="400">Conteúdo inválido</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="500">Erro interno</response>
    [HttpDelete("{usuarioId}/publicacao/{publicacaoId}/comentario/{comentarioId}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> RemoverComentario(Guid usuarioId, Guid publicacaoId, Guid comentarioId)
    {
        var usuarioLogadoId = _aspnetUser.GetUserId();

        if (!_aspnetUser.Admin() && usuarioId != usuarioLogadoId)
        {
            _notifiable.AddNotification("Falha ao remover comentário, privilégios insuficientes.");
            return Unauthorized(_notifiable.GetNotifications);
        }
        
        await _usuarioRepository.RemoverComentarioAsync(usuarioId, publicacaoId, comentarioId, _aspnetUser.Admin());
        return NoContent();
    }

    private async Task<string> UpdateFile(IFormFile file, string? existingFile = null)
    {
        string uploadsFolder = Path.Combine("../../src/Devgram.Web/wwwroot/publicacoes");
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