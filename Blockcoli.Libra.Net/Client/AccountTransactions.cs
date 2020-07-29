using System;
using System.Collections.Generic;
using System.Text;

namespace Blockcoli.Libra.Net.Client
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    //public class Script
    //{
    //    public int amount { get; set; }
    //    public string auth_key_prefix { get; set; }
    //    public string metadata { get; set; }
    //    public string metadata_signature { get; set; }
    //    public string receiver { get; set; }
    //    public string type { get; set; }

    //}

    //public class Transaction
    //{
    //    public int expiration_timestamp_secs { get; set; }
    //    public int gas_unit_price { get; set; }
    //    public int max_gas_amount { get; set; }
    //    public string public_key { get; set; }
    //    public Script script { get; set; }
    //    public string script_hash { get; set; }
    //    public string sender { get; set; }
    //    public int sequence_number { get; set; }
    //    public string signature { get; set; }
    //    public string signature_scheme { get; set; }
    //    public string type { get; set; }

    //}

    //public class AccountTransactions
    //{
    //    public List<object> events { get; set; }
    //    public int gas_used { get; set; }
    //    public Transaction transaction { get; set; }
    //    public int version { get; set; }
    //    public int vm_status { get; set; }

    //}

    //public class AccountTransactionsRoot
    //{
    //    public int id { get; set; }
    //    public string jsonrpc { get; set; }
    //    public AccountTransactions result { get; set; }

    //}






    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Script
    {
        public ulong amount { get; set; }
        public string currency { get; set; }
        public string metadata { get; set; }
        public string metadata_signature { get; set; }
        public string receiver { get; set; }
        public string type { get; set; }

    }

    public class Transaction
    {
        public int chain_id { get; set; }
        public int expiration_timestamp_secs { get; set; }
        public string gas_currency { get; set; }
        public int gas_unit_price { get; set; }
        public int max_gas_amount { get; set; }
        public string public_key { get; set; }
        public Script script { get; set; }
        public string script_hash { get; set; }
        public string sender { get; set; }
        public ulong sequence_number { get; set; }
        public string signature { get; set; }
        public string signature_scheme { get; set; }
        public string type { get; set; }

    }

    //public class MoveAbort
    //{
    //    public int abort_code { get; set; }
    //    public string location { get; set; }

    //}

    //public class VmStatus
    //{
    //    public MoveAbort move_abort { get; set; }

    //}

    public class AccountTransactions
    {
        public List<object> events { get; set; }
        public ulong gas_used { get; set; }
        public string hash { get; set; }
        public Transaction transaction { get; set; }
        public ulong version { get; set; }
        // public VmStatus vm_status { get; set; }

    }

    public class AccountTransactionsRoot
    {
        public int id { get; set; }
        public string jsonrpc { get; set; }
        public int libra_chain_id { get; set; }
        public long libra_ledger_timestampusec { get; set; }
        public int libra_ledger_version { get; set; }
        public AccountTransactions result { get; set; }

    }


}
