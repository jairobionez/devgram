using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devgram.Infra.Entities
{
    public class PublicacaoAnexo : EntityBase
    {
        public string Url { get; private set; }
        public Guid PublicacaoId { get; private set; }
        public virtual Publicacao Publicacao { get; private set; }
    }
}
