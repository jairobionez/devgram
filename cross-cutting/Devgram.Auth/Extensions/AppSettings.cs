using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Devgram.Auth.Extensions
{
    public class AppSettings
    {
        public string? Secret { get; set; }
        public double ExpiracaoHoras { get; set; }
        public string Emissor { get; set; }
        public string ValidoEm { get; set; }
    }
}