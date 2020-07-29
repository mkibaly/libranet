using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blockcoli.Libra.Net.Api.Models;
using Blockcoli.Libra.Net.Client;
using Blockcoli.Libra.Net.Wallet;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Blockcoli.Libra.Net.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        string serverHost;
        LibraWallet wallet;
        LibraClient client;
        public AccountController()
        {
            LibraNetwork network = LibraNetwork.Testnet;
            this.wallet = new LibraWallet();
            this.client = new LibraClient(network);
            switch (network)
            {
                case LibraNetwork.Testnet:
                    this.serverHost = Constant.ServerHosts.TestnetAdmissionControl;
                    break;
            }
        }

        // http://localhost:58162/Account/Create
        [HttpGet("Create")]
        public LibraAccount Create()
        {
            var acc = wallet.NewAccount();
            var account = new LibraAccount
            {
                Address = acc.Address,
                AuthKey = acc.AuthKey,
                Mnemonic = wallet.Mnemonic.ToString()
            };

            return account;
        }


        // http://localhost:58162/Account/Recover?mnemonic=purchase%20creek%20exact%20artefact%20muffin%20life%20forum%20abuse%20country%20cave%20gas%20social%20sauce%20gesture%20town%20have%20guess%20drop%20cloud%20hope%20parent%20shy%20nerve%20voice
        [HttpPost("Recover")]
        public async Task<LibraAccount> RecoverAsync(WordsDto words)
        {
            //var words = Mnemonic.Split(';');

            wallet = new LibraWallet(words.words);
            var acc = wallet.NewAccount();

            var account = new LibraAccount
            {
                Address = acc.Address,
                AuthKey = acc.AuthKey,
                Mnemonic = wallet.Mnemonic.ToString()
            };

            try
            {
                var state = await client.QueryBalanceAsync(acc.Address);
                account.Balance = state?.balances?.FirstOrDefault()?.amount ?? ulong.MinValue;
                account.Sequence = state?.sequence_number ?? ulong.MinValue;
            }
            catch { }

            return account;
        }


        // http://localhost:58162/Account/mint?amount=100000&AuthKey=25b5ef85f5595b7fe85c5fe4ba93d169f52c9086ed3b8af9414f947148f953af
        [HttpPost("Mint")]
        public async Task<ulong> MintAsync(MintDto dto)
        {
            string address = dto.AuthKey;
            try
            {
                var resultAmount = await client.MintWithFaucetService(address, dto.amount);

                return resultAmount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: - {ex.Message}");
            }
            return 0;
        }

        // http://localhost:58162/Account/QueryBalance?address=f52c9086ed3b8af9414f947148f953af
        [HttpGet("QueryBalance")]
        public async Task<ActionResult> QueryBalance(string address)
        {
            try
            {
                var state = await client.QueryBalanceAsync(address);

                var balance = state?.balances?.FirstOrDefault()?.amount ?? ulong.MinValue;


                return Json(balance);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }


        }

        // http://localhost:58162/Account/QuerySequence?address=f52c9086ed3b8af9414f947148f953af
        [HttpGet("QuerySequence")]
        public async Task<ActionResult> QuerySequence(string address)
        {
            try
            {
                var state = await client.QueryBalanceAsync(address);

                return Json(state?.sequence_number);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        // http://localhost:58162/Account/Transactions?address=be0d38de83c1f385fb913ec09281167c&sequence_number=19
        [HttpPost("Transactions")]
        public async Task<ActionResult> Transactions(TransactionInputDto dto)
        {
            try
            {
                var trx = await client.GetAccountTransaction(dto.address, dto.sequence_number);
                var transaction = new Models.Transaction()
                {
                    Id = trx.version,
                    Amount = trx.transaction.script.amount,
                    FromAccount = dto.address,
                    ToAccount = trx.transaction.script.receiver,
                    Fee = trx.gas_used,
                    Sequence = trx.transaction.sequence_number
                };

                return Json(transaction);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        // http://localhost:58162/Account/submit?currency=LBR&receiverAddress=f52c9086ed3b8af9414f947148f953af&amount=10&mnemonic=purchase%20creek%20exact%20artefact%20muffin%20life%20forum%20abuse%20country%20cave%20gas%20social%20sauce%20gesture%20town%20have%20guess%20drop%20cloud%20hope%20parent%20shy%20nerve%20voice
        [HttpPost("Submit")]
        public async Task<ActionResult> Submit(SubmitDto dto)
        {
            try
            {
                wallet = new LibraWallet(dto.Mnemonic);
                var acc = wallet.NewAccount();


                var trx = await client.TransferCoins(acc, dto.receiverAddress, dto.amount, dto.currency);

                return Content(trx, "Application/json");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }


    }


    public class WordsDto
    {
        public string words { get; set; }
    }
    public class MintDto
    {
        public string AuthKey { get; set; }
        public ulong amount { get; set; }
    }
    public class SubmitDto
    {
        public string Mnemonic { get; set; }
        public string receiverAddress { get; set; }
        public ulong amount { get; set; }
        public string currency { get; set; } = "LBR";

    }
    public class TransactionInputDto
    {
        public string address { get; set; }
        public ulong sequence_number { get; set; } = 0;
    }

}
