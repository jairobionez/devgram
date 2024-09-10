namespace Devgram.Infra.Entities
{
    public class Usuario : EntityBase
    {
        protected Usuario()
        {
            
        }

        public Usuario(string? nome, string? sobrenome, string? email, string? cpf, string? cidade, string? cep, string? logradouro, string? uf, string? numero)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            Email = email;
            Cpf = cpf;
            Cidade = cidade;
            Cep = cep;
            Logradouro = logradouro;
            Uf = uf;
            Numero = numero;
        }

        public string? Nome { get; private set; }
        public string? Sobrenome { get; private set; }
        public string? Email { get; private set; }
        public string? Cpf { get; private set; }
        public string? Cidade { get; private set; }
        public string? Cep { get; private set; }
        public string? Logradouro { get; private set; }
        public string? Uf { get; private set; }
        public string? Numero { get; private set; }

        public virtual ICollection<Publicacao> Publicacoes { get; private set; }
        public virtual ICollection<Comentario> Comentarios { get; private set; }

        public void Atualizar(Usuario usuario)
        {
            Nome = usuario.Nome;
            Sobrenome = usuario.Sobrenome;
            Cpf = usuario.Cpf;
            Cidade = usuario.Cidade;
            Cep = usuario.Cep;
            Logradouro = usuario.Logradouro;
            Uf = usuario.Uf;
            Numero = usuario.Numero;
        }
    }
}