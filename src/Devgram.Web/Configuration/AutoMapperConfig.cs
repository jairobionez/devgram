using AutoMapper;
using Devgram.Infra.Entities;
using Devgram.ViewModel;

namespace Devgram.Web.Configuration;

public class AutoMapperConfig: Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Usuario, UsuarioResponseModel>();
        
        CreateMap<Publicacao, PublicacaoResponseModel>().ReverseMap();
        CreateMap<PublicacaoModel, Publicacao>();
        CreateMap<PublicacaoComentario, PublicacaoComentarioResponseModel>();
        CreateMap<PublicacaoComentarioModel, PublicacaoComentario>();
    }
}