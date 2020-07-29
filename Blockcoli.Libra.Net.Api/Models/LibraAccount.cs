using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blockcoli.Libra.Net.Api.Models
{
    public class LibraAccount
    {
        public string AuthKey { get; set; }
        public string Address { get; set; }
        public string Mnemonic { get; set; }
        public ulong Balance { get; set; }
        public ulong Sequence { get; set; }

    }
}
