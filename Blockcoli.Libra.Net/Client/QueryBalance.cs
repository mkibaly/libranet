using System;
using System.Collections.Generic;
using System.Text;

namespace Blockcoli.Libra.Net.Client
{

    // JsonRpcRoot myDeserializedClass = JsonConvert.DeserializeObject<JsonRpcRoot>(myJsonResponse); 
    public class Balance
    {
        public ulong amount { get; set; }
        public string currency { get; set; }

    }

    public class ParentVasp
    {
        public string base_url { get; set; }
        public string compliance_key { get; set; }
        public string expiration_time { get; set; }
        public string human_name { get; set; }
        public int num_children { get; set; }

    }

    public class Role
    {
        public ParentVasp parent_vasp { get; set; }

    }

    public class QueryBalance
    {
        public string authentication_key { get; set; }
        public List<Balance> balances { get; set; }
        public bool delegated_key_rotation_capability { get; set; }
        public bool delegated_withdrawal_capability { get; set; }
        public bool is_frozen { get; set; }
        public string received_events_key { get; set; }
        public Role role { get; set; }
        public string sent_events_key { get; set; }
        public ulong sequence_number { get; set; }

    }

    public class JsonRpcRoot
    {
        public int id { get; set; }
        public string jsonrpc { get; set; }
        public QueryBalance result { get; set; }

    }


}
