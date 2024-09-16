namespace Devgram.Infra.Entities
{
    public class Publicacao : EntityBase
    {
        protected Publicacao()
        {

        }

        public Publicacao(string? descricao, Guid usuarioId, string titulo)
        {
            Titulo = titulo;
            Descricao = descricao;
            UsuarioId = usuarioId;
        }

        public string? Titulo { get; private set; }
        public string? Descricao { get; private set; }
        public virtual ICollection<PublicacaoAnexo>? Anexos { get; private set; }
        public virtual ICollection<PublicacaoComentario>? Comentarios { get; private set; }

        public Guid UsuarioId { get; private set; }
        public virtual Usuario Usuario { get; private set; }

        public void Atualizar(Publicacao publicacao)
        {
            Titulo = publicacao.Titulo;
            Descricao = publicacao.Descricao;
            Anexos = publicacao.Anexos;
            Comentarios = publicacao.Comentarios;
            DataAtualizacao = DateTime.Now;
        }

        public void NovaPublicacao()
        {
            DataCriacao = DateTime.Now;
        }
    }
}
