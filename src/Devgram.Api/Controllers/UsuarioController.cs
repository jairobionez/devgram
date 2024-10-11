using AutoMapper;
using Devgram.Data.Entities;
using Devgram.Data.Infra;
using Devgram.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Devgram.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/usuario")]
public class UsuarioController: Controller
{
    private readonly PublicacaoRepository _publicacaoRepository;
    private readonly UsuarioRepository _usuarioRepository;
    private readonly IMapper _mapper;
    private readonly Notifiable _notifiable;

    public UsuarioController(PublicacaoRepository publicacaoRepository, UsuarioRepository usuarioRepository, IMapper mapper, Notifiable notifiable)
    {
        _publicacaoRepository = publicacaoRepository;
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
        _notifiable = notifiable;
    }

    [HttpGet("{usuarioId}/publicacao")]
    [ProducesResponseType(typeof(IEnumerable<PublicacaoResponseModel>), 200)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> GetAllPublicacoesAsync([FromQuery] PublicacaoFiltroModel model)
    {
        try
        {
            var resultado =
                _mapper.Map<List<PublicacaoResponseModel>>(await _usuarioRepository.GetPublicacoesAsync(model.Termo));
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpGet("{usuarioId}/publicacao/{publicacaoId}")]
    [ProducesResponseType(typeof(IEnumerable<PublicacaoResponseModel>), 200)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> GetPublicacaosAsync(Guid usuarioId, Guid publicacaoId)
    {
        try
        {
            var resultado =
                _mapper.Map<List<PublicacaoResponseModel>>(await _usuarioRepository.GetPublicacaoAsync(publicacaoId));
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost("{usuarioId}/publicacao")]
    [ProducesResponseType(typeof(IEnumerable<PublicacaoResponseModel>), 200)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> NovaPublicacaoAsync(Guid usuarioId, [FromForm] PublicacaoModel model)
    {
        try
        {
            if (model.File != null && model.File.Length > 0)
                model.Logo = await UpdateFile(model.File, model.Logo);
            
            var resultado = await _publicacaoRepository.InsertAsync(_mapper.Map<Publicacao>(model));
            
            if (_notifiable.HasNotification)
                return Unauthorized(_notifiable.GetNotifications);
            
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPut("{usuarioId}/publicacao/{publicacaoId}")]
    [ProducesResponseType(typeof(IEnumerable<PublicacaoResponseModel>), 200)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> AtualizarPublicacaoAsync(Guid usuarioId, Guid publicacaoId, [FromForm] PublicacaoModel model)
    {
        try
        {
            if (model.File != null && model.File.Length > 0)
                model.Logo = await UpdateFile(model.File, model.Logo);
            
            await _publicacaoRepository.UpdateAsync(publicacaoId,_mapper.Map<Publicacao>(model));
            
            if (_notifiable.HasNotification)
                return Unauthorized(_notifiable.GetNotifications);
            
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpDelete("{usuarioId}/publicacao/{publicacaoId}")]
    [ProducesResponseType(typeof(IEnumerable<PublicacaoResponseModel>), 200)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult> RemoverPublicacaoAsync(Guid usuarioId, Guid publicacaoId)
    {
        try
        {
            await _publicacaoRepository.DeleteAsync(publicacaoId);

            if (_notifiable.HasNotification)
                return Unauthorized(_notifiable.GetNotifications);
            
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
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