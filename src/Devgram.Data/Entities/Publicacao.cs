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
        
        public void AtualizarComentario(Guid comentarioId, PublicacaoComentario publicacaoComentario)
        {
            if(Comentarios == null)
                Comentarios = new List<PublicacaoComentario>();
            
            Comentarios.Where(p => p.Id == comentarioId)
                .ToList().ForEach(p => p.Atualizar(publicacaoComentario));
        }
        
        public void RemoverComentario(Guid comentarioId)
        {
            if(Comentarios == null)
                Comentarios = new List<PublicacaoComentario>();
            
            var comentario = Comentarios.First(p => p.Id == comentarioId);

            Comentarios.Remove(comentario);
        }

        public void NovaPublicacao()
        {
            DataCriacao = DateTime.Now;
        }
        
        public string CalcularTempoMedioLeitura()
        {
            int palavrasPorMinuto = 200;
            int numeroDePalavras = ContarPalavras(Descricao);
            double tempoMinutos = (double)numeroDePalavras / palavrasPorMinuto;

            int minutos = (int)tempoMinutos;

            if (minutos < 1)
                return "Tempo de leitura menos de 1 minuto";
;            
            return $"Tempo médiop de leitura: {minutos} minuto(s)";
        }

        private int ContarPalavras(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return 0;

            string[] palavras = texto.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            return palavras.Length;
        }
    }
}
