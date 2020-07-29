using System;
using System.Collections.Generic;
using System.Linq;

namespace Blockcoli.Libra.Net.LCS
{
    public class PayloadLCS
    {
        public string CoinTag { get; set; }
        public string RecipientAddress { get; set; }
        public ulong Amount { get; set; }

        public byte[] Code { get; set; }
        public List<TransactionArgumentLCS> TransactionArguments { get; set; }
        public List<byte[]> Modules { get; set; }
    }
}
