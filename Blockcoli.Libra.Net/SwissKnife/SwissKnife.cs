using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blockcoli.Libra.Net.SwissKnife
{
    public class SwissKnifeHelper
    {
        private readonly string contentRootPath;
        private readonly JsonSerializerSettings settings;

        public SwissKnifeHelper(string ContentRootPath)
        {
            contentRootPath = ContentRootPath;
            settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
        }

        public SwissAccount GenerateTestEd25519Keypair()
        {
            var jsonResponse =this.exec("generate-test-ed25519-keypair");
            SiwssKinfeResponse<SwissAccount> rawTxn = JsonConvert.DeserializeObject<SiwssKinfeResponse<SwissAccount>>(jsonResponse);

            return rawTxn.data;
        }

        public RawTxn GenerateRawTxn(Transaction transaction)
        {
            var json = JsonConvert.SerializeObject(transaction, settings);
            var jsonResponse = this.exec($"generate-raw-txn ", json);
            SiwssKinfeResponse<RawTxn> rawTxn = JsonConvert.DeserializeObject<SiwssKinfeResponse<RawTxn>>(jsonResponse);

            return rawTxn.data;
        }

        public RawTxn SignTransaction(RawTxn rawTxn)
        {
            var json = JsonConvert.SerializeObject(rawTxn, settings);
            var jsonResponse = this.exec($"sign-transaction-using-ed25519", json);
            return JsonConvert.DeserializeObject<SiwssKinfeResponse<RawTxn>>(jsonResponse).data;
        }

        public RawTxn GenerateSignedTxn(RawTxn rawTxn)
        {
            var json = JsonConvert.SerializeObject(rawTxn, settings);
            var jsonResponse = this.exec($"generate-signed-txn", json);
            return JsonConvert.DeserializeObject<SiwssKinfeResponse<RawTxn>>(jsonResponse).data;
        }

        public string exec(string args, string input = null)
        {
            System.Diagnostics.Process si = new System.Diagnostics.Process();
            si.StartInfo.WorkingDirectory = contentRootPath + "/wwwroot";
            si.StartInfo.UseShellExecute = false;
            si.StartInfo.FileName = "cmd.exe";
            si.StartInfo.Arguments = $"/c swiss-knife {args}";
            si.StartInfo.CreateNoWindow = true;
            si.StartInfo.RedirectStandardInput = true;
            si.StartInfo.RedirectStandardOutput = true;
            si.StartInfo.RedirectStandardError = true;
            si.Start();
            if (!string.IsNullOrEmpty(input))
            {
                System.IO.StreamWriter sw = si.StandardInput;
                sw.WriteLine(input);
                sw.Close();
            }
            string output = si.StandardOutput.ReadToEnd();
            si.Close();

            return output;
        }

    }
}
