using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class AudienceSettings
    {
        public string? Secret { get; set; }
        public string? Iss { get; set; }
        public string? Aud { get; set; }
        public string? ClientId { get; set; }
    }
}
