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

    [HttpGet("{usuarioId}/publicacao")]
    [ProducesResponseType(typeof(IEnumerable<PublicacaoResponseModel>), 200)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> GetAllPublicacoesAsync([FromQuery] PublicacaoFiltroModel model)
    {
        var resultado =
            _mapper.Map<List<PublicacaoResponseModel>>(await _usuarioRepository.GetPublicacoesAsync(model.Termo));
        return Ok(resultado);
    }

    [HttpGet("{usuarioId}/publicacao/{publicacaoId}")]
    [ProducesResponseType(typeof(IEnumerable<PublicacaoResponseModel>), 200)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> GetPublicacaosAsync(Guid usuarioId, Guid publicacaoId)
    {
        var resultado =
            _mapper.Map<List<PublicacaoResponseModel>>(await _usuarioRepository.GetPublicacaoAsync(publicacaoId));
        return Ok(resultado);
    }

    [HttpPost("{usuarioId}/publicacao")]
    [ProducesResponseType(typeof(IEnumerable<PublicacaoResponseModel>), 200)]
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

    [HttpPut("{usuarioId}/publicacao/{publicacaoId}")]
    [ProducesResponseType(typeof(IEnumerable<PublicacaoResponseModel>), 200)]
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

    [HttpDelete("{usuarioId}/publicacao/{publicacaoId}")]
    [ProducesResponseType(typeof(IEnumerable<PublicacaoResponseModel>), 200)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> RemoverPublicacaoAsync(Guid usuarioId, Guid publicacaoId)
    {
        await _publicacaoRepository.DeleteAsync(publicacaoId);

        if (_notifiable.HasNotification)
            return Unauthorized(_notifiable.GetNotifications);

        return Ok();
    }
    
       
    [HttpPost("{usuarioId}/publicacao/{publicacaoId}/comentario")]
    [ProducesResponseType(typeof(Guid), 200)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> NovoComentario(Guid usuarioId, Guid publicacaoId, [FromBody] PublicacaoComentarioModel model)
    {
        var usuarioLogadoId = _aspnetUser.GetUserId();
        
        if (usuarioId != usuarioLogadoId)
        {
            _notifiable.AddNotification("Falha ao remover comentário, privilégios insuficientes.");
            return BadRequest(_notifiable.GetNotifications);
        }
        
        var resultado = await _usuarioRepository.NovoComentarioAsync(
            usuarioId, publicacaoId, _mapper.Map<PublicacaoComentario>(model));
        
        return Ok(resultado);
    }
    
    [HttpPut("{usuarioId}/publicacao/{publicacaoId}/comentario/{comentarioId}")]
    [ProducesResponseType(typeof(Guid), 200)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> AlterarComentario(Guid usuarioId, Guid publicacaoId, Guid comentarioId, [FromBody] PublicacaoComentarioModel model)
    {
        var usuarioLogadoId = _aspnetUser.GetUserId();
        
        if (usuarioId != usuarioLogadoId)
        {
            _notifiable.AddNotification("Falha ao remover comentário, privilégios insuficientes.");
            return BadRequest(_notifiable.GetNotifications);
        }
        
        var resultado = await _usuarioRepository.AlterarComentarioAsync(
            usuarioId, publicacaoId, comentarioId, _mapper.Map<PublicacaoComentario>(model));
        
        return Ok(resultado);
    }
    
    [HttpDelete("{usuarioId}/publicacao/{publicacaoId}/comentario/{comentarioId}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> RemoverComentario(Guid usuarioId, Guid publicacaoId, Guid comentarioId)
    {
        var perfilUsuario = _aspnetUser.GetUserRole();
        var usuarioLogadoId = _aspnetUser.GetUserId();

        if (perfilUsuario != nameof(PerfilUsuarioEnum.ADMIN) && usuarioId != usuarioLogadoId)
        {
            _notifiable.AddNotification("Falha ao remover comentário, privilégios insuficientes.");
            return BadRequest(_notifiable.GetNotifications);
        }
        
        await _usuarioRepository.RemoverComentarioAsync(usuarioId, publicacaoId, comentarioId);
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