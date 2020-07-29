using System.Threading.Tasks;
using Blockcoli.Libra.Net.Wallet;
using Types;
using static AdmissionControl.AdmissionControl;
using System.Linq;
using System.Net.Http;
using System.Net;
using System;
using Grpc.Core;
using AdmissionControl;
using System.Collections.Generic;
using Blockcoli.Libra.Net.Common;
using Blockcoli.Libra.Net.Crypto;
using Blockcoli.Libra.Net.LCS;
using Blockcoli.Libra.Net.JsonRPC;
using Newtonsoft.Json;

namespace Blockcoli.Libra.Net.Client
{
    public class LibraClient
    {
        public LibraNetwork Network { get; private set; }
        public string EndPointUrl { get; private set; }
        public string Host { get; private set; }
        public int Port { get; private set; }
        public string FaucetServerHost { get; private set; }
        //AdmissionControlClient acClient;
        JsonRpcClient rpcClient;
        public ChannelCredentials ChannelCredentials { get; private set; }

        public LibraClient(LibraNetwork network)
        {
            this.Network = network;

            switch (network)
            {
                case LibraNetwork.Testnet:
                    Host = Constant.ServerHosts.TestnetAdmissionControl;
                    Port = 80;
                    ChannelCredentials = ChannelCredentials.Insecure;
                    FaucetServerHost = Constant.ServerHosts.TestnetFaucet;
                    EndPointUrl = Constant.ServerHosts.EndPointUrl;
                    break;
            }

            Channel channel = new Channel(Host, Port, ChannelCredentials);
            //acClient = new AdmissionControlClient(channel);

            rpcClient = new JsonRpcClient(EndPointUrl);
        }

        public async Task<ulong> MintWithFaucetService(string address, ulong amount)
        {
            var httpClient = new HttpClient();
            //var url = $"http://{Constant.ServerHosts.TestnetFaucet}?amount={amount}&address={address}";            
            //var response = await httpClient.PostAsync(url, null);
            // if (response.StatusCode != HttpStatusCode.OK) throw new Exception($"Failed to query faucet service. Code: {response.StatusCode}");
            // var sequenceNumber = await response.Content.ReadAsStringAsync();                        
            // return ulong.Parse(sequenceNumber);

            //var url = $"https://api-test.libexplorer.com/api?module=faucet&action=getfaucet&amount={amount}&address={address}";

            var auth_key = address;
            var curency = "LBR";
            var url = $"http://faucet.testnet.libra.org/?amount={amount}&auth_key={auth_key}&currency_code={curency}";
            // account address: d50e29c8844a7265363d6c4958584dd9
            // auth key: 9b75402062ff056c9e2ac0761b0b1ea2d50e29c8844a7265363d6c4958584dd9



            var response = await httpClient.PostAsync(url, null);
            if (response.StatusCode != HttpStatusCode.OK) throw new Exception($"Failed to query faucet service. Code: {response.StatusCode}");
            var json = await response.Content.ReadAsStringAsync();
            //var faucetResult = System.Text.Json.JsonSerializer.Deserialize<FaucetResult>(json);
            //if (json != "1") throw new Exception($"Failed to query faucet service. Message: {json}");
            return ulong.Parse(json);
        }

        public async Task<string> TransferCoins(Account sender, string receiverAddress, ulong amount, string currency, ulong gasUnitPrice = 0, ulong maxGasAmount = 1000000)
        {
            try
            {
                //var accountState = await QueryBalance(sender.Address);

                //var payloadLCS = new PayloadLCS();
                //payloadLCS.Code = Convert.FromBase64String(Constant.ProgamBase64Codes.PeerToPeerTxn);
                //payloadLCS.TransactionArguments = new List<TransactionArgumentLCS>();
                //payloadLCS.TransactionArguments.Add(new TransactionArgumentLCS
                //{
                //    ArgType = TransactionArgument.Types.ArgType.String,
                //    String = currency
                //});
                //payloadLCS.TransactionArguments.Add(new TransactionArgumentLCS
                //{
                //    ArgType = Types.TransactionArgument.Types.ArgType.Address,
                //    Address = new AddressLCS
                //    {
                //        Value = receiverAddress
                //    }
                //});
                //payloadLCS.TransactionArguments.Add(new TransactionArgumentLCS
                //{
                //    ArgType = Types.TransactionArgument.Types.ArgType.U64,
                //    U64 = amount
                //});

                var transaction = new RawTransactionLCS
                {
                    SequenceNumber = 1,//accountState.SequenceNumber,
                    MaxGasAmount = maxGasAmount,
                    GasUnitPrice = gasUnitPrice,
                    GasCurrencyCode = currency,
                    Sender = new AddressLCS { Value = sender.Address },
                    ExpirationTime = (ulong)Math.Floor((decimal)DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000) + 100,
                    TransactionPayload = TransactionPayloadLCS.FromScript(new ScriptLCS()
                    {
                        CoinTag = currency,
                        RecipientAddress = receiverAddress,
                        Amount = amount
                    }),
                };

                var transactionLCS = LCSCore.LCSDeserialization(transaction);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(transaction);
                Console.WriteLine(json);
                Console.WriteLine(transactionLCS.ToHexString());



                var digestSHA3 = new SHA3_256();
                var saltDigest = digestSHA3.ComputeVariable(Constant.HashSaltValues.RawTransactionHashSalt.ToBytes());
                var saltDigestAndTransaction = saltDigest.Concat(transactionLCS).ToArray();
                var hash = digestSHA3.ComputeVariable(saltDigestAndTransaction);
                var senderSignature = sender.KeyPair.Sign(hash);

                var publicKeyLen = BitConverter.GetBytes((uint)sender.PublicKey.Length);
                var signatureLen = BitConverter.GetBytes((uint)senderSignature.Length);
                var txnBytes = transactionLCS.Concat(publicKeyLen).ToArray();
                txnBytes = txnBytes.Concat(sender.PublicKey).ToArray();
                txnBytes = txnBytes.Concat(signatureLen).ToArray();
                txnBytes = txnBytes.Concat(senderSignature).ToArray();
                Console.WriteLine("sender.PublicKey:", sender.PublicKey.ToHexString());
                Console.WriteLine("senderSignature:", senderSignature.ToHexString());

                var request = new SubmitTransactionRequest
                {
                    Transaction = new SignedTransaction
                    {
                        TxnBytes = txnBytes.ToByteString()
                    }
                };

                //var response = await acClient.SubmitTransactionAsync(request);

                Console.WriteLine("Trns hex:");
                Console.WriteLine(txnBytes.ToHexString());

                var response = await rpcClient.CallAsync("submit", txnBytes.ToHexString());

                Console.WriteLine("response:");
                Console.WriteLine(response);
                //return response.AcStatus.Code == AdmissionControlStatusCode.Accepted;
                //todo: check response code
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AccountTransactions> GetAccountTransaction(string address, ulong sequence, bool include_events = false)
        {
            var jsonResponse = await rpcClient.CallAsync("get_account_transaction", address, sequence, include_events);
            AccountTransactionsRoot jsonRpc = JsonConvert.DeserializeObject<AccountTransactionsRoot>(jsonResponse);

            return jsonRpc.result;
        }

        public async Task<QueryBalance> QueryBalanceAsync(string address)
        {
            var request = new UpdateToLatestLedgerRequest();
            var accountStateRequest = new GetAccountStateRequest { Address = address.ToByteString() };

            var requestItem = new RequestItem { GetAccountStateRequest = accountStateRequest };
            request.RequestedItems.Add(requestItem);

            var balanceJsonResponse = await rpcClient.CallAsync("get_account", address);
            JsonRpcRoot jsonRpc = JsonConvert.DeserializeObject<JsonRpcRoot>(balanceJsonResponse);

            return jsonRpc.result;
        }

        //public async Task<List<Wallet.AccountState>> QueryBalances(params string[] addresses)
        //{
        //    var request = new UpdateToLatestLedgerRequest();
        //    foreach (var address in addresses)
        //    {
        //        var accountStateRequest = new GetAccountStateRequest{ Address = address.ToByteString() };

        //        var requestItem = new RequestItem { GetAccountStateRequest = accountStateRequest };
        //        request.RequestedItems.Add(requestItem);
        //    }

        //    var accountStates = new List<Wallet.AccountState>();
        //    var response = await acClient.UpdateToLatestLedgerAsync(request);                                    
        //    foreach (var item in response.ResponseItems)
        //    {
        //        var accountState = DecodeAccountStateBlob(item.GetAccountStateResponse.AccountStateWithProof.Blob);                            
        //        accountStates.Add(accountState);                                                                                  
        //    }

        //    return accountStates;
        //}

        private static Wallet.AccountState DecodeAccountStateBlob(AccountStateBlob blob)
        {
            var cursor = new CursorBuffer(blob.Blob.ToByteArray());
            var blobLen = cursor.Read32();

            var state = new Dictionary<string, byte[]>();
            for (int i = 0; i < blobLen; i++)
            {
                var keyLen = cursor.Read32();
                var keyBuffer = new byte[keyLen];
                for (int j = 0; j < keyLen; j++)
                {
                    keyBuffer[j] = cursor.Read8();
                }

                var valueLen = cursor.Read32();
                var valueBuffer = new byte[valueLen];
                for (int k = 0; k < valueLen; k++)
                {
                    valueBuffer[k] = cursor.Read8();
                }

                state[keyBuffer.ToHexString()] = valueBuffer;
            }

            return Wallet.AccountState.FromBytes(state[Constant.PathValues.AccountStatePath]);
        }
    }
}