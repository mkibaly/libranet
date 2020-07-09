using System;
using System.Collections.Generic;
using System.Text;

namespace Blockcoli.Libra.Net.SwissKnife
{
    public class SwissKnife
    {

//        public GenerateRawTxnResponse generate_raw_txn( GenerateRawTxnRequest g) {
//    var script = match g.script_params {
//                Preburn MoveScriptParams { coin_tag, amount
//    } => {
//            var coin_tag = helpers::coin_tag_parser(&coin_tag);
//    transaction_builder::encode_preburn_script(coin_tag, amount)
//}
//MoveScriptParams::PeerToPeerTransfer {
//            coin_tag,
//            recipient_address,
//            amount,
//            metadata_hex_encoded,
//            metadata_signature_hex_encoded,
//        } => {
//            var coin_tag = helpers::coin_tag_parser(&coin_tag);
//var recipient_address = helpers::account_address_parser(&recipient_address);
//transaction_builder::encode_transfer_with_metadata_script(
//    coin_tag,
//    recipient_address,
//    amount,
//    helpers::hex_decode(&metadata_hex_encoded),
//                helpers::hex_decode(&metadata_signature_hex_encoded),
//            )
//        }
//    };
//    var raw_txn =  new RawTransaction(
//        helpers::account_address_parser(&g.txn_params.sender_address),
//        g.txn_params.sequence_number,
//        TransactionPayload::Script(script),
//        g.txn_params.max_gas_amount,
//        g.txn_params.gas_unit_price,
//        g.txn_params.gas_currency_code,
//        std::time::Duration::new(g.txn_params.expiration_timestamp, 0),
//    );
//    return new GenerateRawTxnResponse {
//        raw_txn: hex.encode(
//            lcs.to_bytes(&raw_txn)
//                .map_err(|err| {
//    helpers::exit_with_error(format!(
//        "lcs serialization failure of raw_txn : {}",
//        err
//    ))
//                })
//                .unwrap(),
//        ),
//    }
//}

    }
}
