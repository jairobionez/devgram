namespace Devgram.Data.Entities
{
    public class PublicacaoAnexo : EntityBase
    {
        public string Url { get; private set; }
        public Guid PublicacaoId { get; private set; }
        public virtual Publicacao Publicacao { get; private set; }
    }
}
