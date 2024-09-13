namespace Devgram.Infra.Entities
{
    public class PublicacaoComentario : EntityBase
    {
        protected PublicacaoComentario()
        {
            
        }

        public PublicacaoComentario(string? descricao, bool editado, Guid publicacaoId)
        {
            Descricao = descricao;
            Editado = editado;
            PublicacaoId = publicacaoId;
        }

        public string? Descricao { get; private set; }
        public bool Editado { get; private set; }
        public Guid PublicacaoId { get; private set; }
        public virtual Publicacao? Publicacao { get; private set; }

        public Guid? ComentarioPaiId { get; private set; }
        public virtual PublicacaoComentario? ComentarioPai { get; private set; }

        public Guid? UsuarioId { get; private set; }
        public virtual Usuario? Usuario { get; private set; }

        public virtual ICollection<PublicacaoComentario>? Respostas { get; private set; }

        public void Atualizar(PublicacaoComentario publicacaoComentario)
        {
            Descricao = publicacaoComentario.Descricao;
            Editado = true;
        }
    }
}
