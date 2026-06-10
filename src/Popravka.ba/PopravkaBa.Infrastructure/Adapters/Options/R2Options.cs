using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Infrastructure.Adapters.Options
{
    public class R2Options
    {
        public string AccountID { get; set; } = default!;
       public string AccessKey { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
        public string PublicBucket  { get; set; } = default!;
        public string PrivateBucket { get; set; } = default!;
        public string PublicBaseURL { get; set; } = default!;
    }
}
