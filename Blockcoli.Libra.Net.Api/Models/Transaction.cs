using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blockcoli.Libra.Net.Api.Models
{
    public class Transaction
    {
        public ulong Id { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public ulong Amount { get; set; }
        public ulong Fee { get; set; }
        public ulong Sequence { get; set; }
    }
}
