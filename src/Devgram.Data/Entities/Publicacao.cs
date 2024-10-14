namespace Devgram.Data.Entities
{
    public class Publicacao : EntityBase
    {
        protected Publicacao()
        {

        }

        public Publicacao(string titulo, string? descricao, Guid usuarioId, string logo)
        {
            Titulo = titulo;
            Descricao = descricao;
            UsuarioId = usuarioId;
            Logo = logo;
            DataCriacao = DateTime.Now;
        }
        
        public Publicacao(string titulo, string? descricao, Guid usuarioId, string logo, List<PublicacaoComentario> comentarios)
        {
            Titulo = titulo;
            Descricao = descricao;
            UsuarioId = usuarioId;
            Logo = logo;
            DataCriacao = DateTime.Now;
            Comentarios = comentarios;
        }

        public string? Titulo { get; private set; }
        public string? Descricao { get; private set; }
        public string? Logo { get; private set; }
        public virtual ICollection<PublicacaoAnexo>? Anexos { get; private set; }
        public virtual ICollection<PublicacaoComentario>? Comentarios { get; private set; }

        public Guid UsuarioId { get; private set; }
        public virtual Usuario Usuario { get; private set; }

        public void Atualizar(Publicacao publicacao)
        {
            Titulo = publicacao.Titulo;
            Descricao = publicacao.Descricao;
            Logo = publicacao.Logo;
            Comentarios = publicacao.Comentarios;
            DataAtualizacao = DateTime.Now;
        }

        public void AdicionarComentario(PublicacaoComentario publicacaoComentario)
        {
            if(Comentarios == null)
                Comentarios = new List<PublicacaoComentario>();
            
            Comentarios.Add(publicacaoComentario);
        }

        public void NovaPublicacao()
        {
            DataCriacao = DateTime.Now;
        }
    }
}
