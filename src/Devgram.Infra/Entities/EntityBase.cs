using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Devgram.Infra.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; protected set; }
        public DateTime? DataCriacao { get; protected set; }
        public DateTime? DataAtualizacao { get; protected set; }
    }
}