using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Blockcoli.Libra.Net.SwissKnife
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Transaction
    {
        public TxnParams txn_params { get; set; }
        public ScriptParams script_params { get; set; }
    }

    public class ScriptParams
    {
        public PeerToPeerTransfer peer_to_peer_transfer { get; set; }
    }

    public class TxnParams
    {
        public string sender_address { get; set; }
        public ulong sequence_number { get; set; }
        public ulong max_gas_amount { get; set; }
        public ulong gas_unit_price { get; set; }
        public string gas_currency_code { get; set; }
        public string chain_id { get; set; }
        public ulong expiration_timestamp_secs { get; set; }
    }

    public class PeerToPeerTransfer
    {
        public string coin_tag { get; set; }
        public string recipient_address { get; set; }
        public ulong amount { get; set; }
        public string metadata_hex_encoded { get; set; }
        public string metadata_signature_hex_encoded { get; set; }
    }

    public class SiwssKinfeResponse<T> where T : class
    {
        public string error_message { get; set; }
        public T data { get; set; }
    }

    public class RawTxn
    {
        public string raw_txn { get; set; }
        public string script { get; set; }
        public string signed_txn { get; set; }
        public string txn_hash { get; set; }
        public string signature { get; set; }


        public string public_key { get; set; }
        public string private_key { get; set; }
    }

    public class SwissAccount
    {
        public string libra_account_address { get; set; }
        public string libra_auth_key { get; set; }
        public string private_key { get; set; }
        public string public_key { get; set; }

    }
}
