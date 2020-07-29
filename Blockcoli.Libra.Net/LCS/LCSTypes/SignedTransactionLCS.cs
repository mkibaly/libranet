///===================================================================================================
///
/// Source https://github.com/1950Labs/2019_POC_Libra
///
///===================================================================================================

using System;

namespace Blockcoli.Libra.Net.LCS
{
    public class SignedTransactionLCS
    {
        public RawTransactionLCS RawTransaction { get; set; }
        public Authenticator Authenticator { get; set; }

        public override string ToString()
        {
            string retStr = "{" + string.Format("transaction: {0},{1}", RawTransaction.ToString(), Environment.NewLine);
            retStr += string.Format("Authenticator: {0},{1}", Authenticator.ToString(), Environment.NewLine)+ "}";
            return retStr;
        }
    }

    public class Authenticator
    {
        public string PublicKey { get; set; }
        public string Signature { get; set; }
    }
}
