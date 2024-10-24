using AutoMapper;
using Devgram.Data.Entities;
using Devgram.Data.ViewModels;

namespace Devgram.Api.Configuration;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Usuario, UsuarioResponseModel>()
            .ForMember(p => p.NomeCompleto, opts => opts.MapFrom(p => p.ToString()));
        CreateMap<Publicacao, PublicacaoResponseModel>()
            .ForMember(p => p.TempoMedioLeitura, opt => opt.MapFrom(x => x.CalcularTempoMedioLeitura()))
            .ReverseMap();
        CreateMap<PublicacaoModel, Publicacao>();
        CreateMap<PublicacaoComentario, PublicacaoComentarioResponseModel>()
            .ForMember(p => p.UltimaAlteracao, opt => opt.MapFrom(p => p.CalcularTempoDesdeEdicao()));;
        CreateMap<PublicacaoComentarioModel, PublicacaoComentario>();
    }
}