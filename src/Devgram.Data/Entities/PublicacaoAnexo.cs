namespace Devgram.Data.Entities
{
    public class PublicacaoAnexo : EntityBase
    {
        protected PublicacaoAnexo()
        {
            
        }

        public PublicacaoAnexo(string url, Guid publicacaoId)
        {
            Url = url;
            PublicacaoId = publicacaoId;
        }

        public string Url { get; private set; }
        public Guid PublicacaoId { get; private set; }
        public virtual Publicacao Publicacao { get; private set; }
    }
}
