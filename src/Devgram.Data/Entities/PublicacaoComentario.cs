namespace Devgram.Data.Entities
{
    public class PublicacaoComentario : EntityBase
    {
        protected PublicacaoComentario()
        {
            DataCriacao = DateTime.Now;
        }

        public PublicacaoComentario(string? descricao)
        {
            Descricao = descricao;
            DataCriacao = DateTime.Now;
        }
        
        public PublicacaoComentario(string? descricao, bool editado, Guid publicacaoId)
        {
            Descricao = descricao;
            Editado = editado;
            PublicacaoId = publicacaoId;
            DataCriacao = DateTime.Now;
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

        public string CalcularTempoDesdeEdicao()
        {
            DateTime dataUtilizada = DataAtualizacao.HasValue ? DataAtualizacao.Value : DataCriacao.Value;
            
            TimeSpan diferencaTempo = DateTime.Now - dataUtilizada;

            if (diferencaTempo.TotalDays >= 1)
            {
                int dias = (int)diferencaTempo.TotalDays;
                return dias == 1 ? "1 dia atrás" : $"{dias} dias atrás";
            }
            else if (diferencaTempo.TotalHours >= 1)
            {
                int horas = (int)diferencaTempo.TotalHours;
                return horas == 1 ? "1 hora atrás" : $"{horas} horas atrás";
            }
            else
            {
                return "A alguns minutos";
            }
        }
        
        public void Atualizar(PublicacaoComentario publicacaoComentario)
        {
            Descricao = publicacaoComentario.Descricao;
            DataAtualizacao = DateTime.Now;
            Editado = true;
        }

        public void VincularUsuario(Usuario usuario)
        {
            Usuario = usuario;
        }
    }
}
