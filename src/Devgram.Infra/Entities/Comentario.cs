using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devgram.Infra.Entities
{
    public class Comentario : EntityBase
    {
        protected Comentario()
        {
            
        }

        public Comentario(string? descricao, bool editado, Guid publicacaoId)
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
        public virtual Comentario? ComentarioPai { get; private set; }

        public Guid? UsuarioId { get; private set; }
        public virtual Usuario? Usuario { get; private set; }

        public virtual ICollection<Comentario>? Respostas { get; private set; }

        public void Atualizar(Comentario comentario)
        {
            Descricao = comentario.Descricao;
            Editado = true;
        }
    }
}
