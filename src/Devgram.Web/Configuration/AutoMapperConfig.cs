using AutoMapper;
using Devgram.Data.Entities;
using Devgram.Data.ViewModels;

namespace Devgram.Web.Configuration;

public class AutoMapperConfig: Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Usuario, UsuarioResponseModel>()
            .ForMember(p => p.NomeCompleto, opts => opts.MapFrom(p => p.NomeCompelto()));
        
        CreateMap<Publicacao, PublicacaoResponseModel>().ReverseMap();
        CreateMap<PublicacaoModel, Publicacao>();
        CreateMap<PublicacaoComentario, PublicacaoComentarioResponseModel>();
        CreateMap<PublicacaoComentarioModel, PublicacaoComentario>();
    }
}