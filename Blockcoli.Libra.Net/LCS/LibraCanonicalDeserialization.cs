///===================================================================================================
///
/// Source https://github.com/1950Labs/2019_POC_Libra
///
///===================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blockcoli.Libra.Net.LCS
{
    public class LibraCanonicalDeserialization
    {
        public byte[] ToByte(ulong source)
        {
            return BitConverter.GetBytes(source);
        }

        public byte[] ToByte(AddressLCS source)
        {
            return source.Value.HexStringToByteArray();
        }

        public byte[] ToByte(uint source)
        {
            return BitConverter.GetBytes(source);
        }

        public byte[] ToByte(string source)
        {
            var data = Encoding.UTF8.GetBytes(source);
            var len = ToByte((uint)data.Length);
            return len.Concat(data).ToArray();
        }

        public byte[] ToByte(byte[] source)
        {
            var len = ToByte((uint)source.Length);
            var data = source;
            return len.Concat(data).ToArray();
        }

        public byte[] ToByte(List<byte[]> source)
        {
            List<byte> retArr = new List<byte>();
            var len = ToByte((uint)source.Count);
            retArr = retArr.Concat(len).ToList();
            foreach (var item in source)
            {
                var localLen = ToByte((uint)item.Length);
                retArr = retArr.Concat(localLen).ToList();
                retArr = retArr.Concat(item).ToList();
            }
            return retArr.ToArray();
        }

        public byte[] ToByte(bool source)
        {
            return BitConverter.GetBytes(source);
        }

        public byte[] ToByte(TransactionPayloadLCS source)
        {
            var result = ToByte((uint)source.PayloadType);
            switch (source.PayloadType)
            {
                case TransactionPayloadType.Program:
                    var pro = ToByte(source.Program);
                    result = result.Concat(pro).ToArray();
                    break;
                case TransactionPayloadType.WriteSet:
                    var writeSet = ToByte(source.WriteSet);
                    result = result.Concat(writeSet).ToArray();
                    break;
                case TransactionPayloadType.Script:
                    var sc = ToByte(source.Script);
                    result = result.Concat(sc).ToArray();
                    break;
            }
            return result;
        }

        public byte[] ToByte(ProgramLCS source)
        {
            var result = ToByte(source.Code);
            var argLen = ToByte((uint)source.TransactionArguments.Count);
            result = result.Concat(argLen).ToArray();
            foreach (var arg in source.TransactionArguments)
            {
                var argData = ToByte(arg);
                result = result.Concat(argData).ToArray();
            }

            var module = ToByte(source.Modules);
            result = result.Concat(module).ToArray();

            return result;
        }

        public byte[] ToByte(AccessPathLCS source)
        {
            var result = ToByte(source.Address);
            var path = ToByte(source.Path);
            result = result.Concat(path).ToArray();
            return result;
        }

        public byte[] ToByte(WriteOpLCS source)
        {
            switch (source.WriteOpType)
            {
                case WriteOpType.Value:
                    var type = ToByte((uint)source.WriteOpType);
                    var data = ToByte(source.Value);
                    return type.Concat(data).ToArray();
                default:
                    return ToByte((uint)source.WriteOpType);
            }
        }

        public byte[] ToByte(WriteSetLCS source)
        {
            var result = ToByte((uint)source.WriteSet.Count);
            foreach (var writeSet in source.WriteSet)
            {
                var key = ToByte(writeSet.Key);
                result = result.Concat(key).ToArray();
                var ops = ToByte(writeSet.Value);
                result = result.Concat(ops).ToArray();
            }

            return result;
        }

        public byte[] ToByte(RawTransactionLCS source)
        {
            var result = ToByte(source.Sender);
            
            var sequence = ToByte(source.SequenceNumber);
            result = result.Concat(sequence).ToArray();
            
            var payload = ToByte(source.TransactionPayload);
            result = result.Concat(payload).ToArray();

            var max = ToByte(source.MaxGasAmount);
            result = result.Concat(max).ToArray();
            
            var gas = ToByte(source.GasUnitPrice);
            result = result.Concat(gas).ToArray();
            
            var gasCurr = ToByte(source.GasCurrencyCode);
            result = result.Concat(gasCurr).ToArray();
            
            var expire = ToByte(source.ExpirationTime);
            result = result.Concat(expire).ToArray();
            
            return result;
        }

        public byte[] ToByte(ScriptLCS source)
        {
            var result = ToByte(source.CoinTag);
            result = result.Concat(ToByte(source.RecipientAddress)).ToArray();
            result = result.Concat(ToByte(source.Amount)).ToArray();

            //var result = ToByte(source.Code);
            //var argLen = ToByte((uint)source.TransactionArguments.Count);
            //result = result.Concat(argLen).ToArray();
            //foreach (var arg in source.TransactionArguments)
            //{
            //    var argData = ToByte(arg);
            //    result = result.Concat(argData).ToArray();
            //}

            return result;
        }

        public byte[] ToByte(ModuleLCS source)
        {
            throw new NotImplementedException();
        }

        public byte[] ToByte(TransactionArgumentLCS source)
        {
            var len = ToByte((uint)source.ArgType);
            byte[] data;
            switch (source.ArgType)
            {
                case Types.TransactionArgument.Types.ArgType.Address:
                    data = ToByte(source.Address);
                    return len.Concat(data).ToArray();
                case Types.TransactionArgument.Types.ArgType.Bytearray:
                    data = ToByte(source.ByteArray);
                    return len.Concat(data).ToArray();
                case Types.TransactionArgument.Types.ArgType.String:
                    data = ToByte(source.String);
                    return len.Concat(data).ToArray();
                case Types.TransactionArgument.Types.ArgType.U64:
                    data = BitConverter.GetBytes(source.U64);
                    return len.Concat(data).ToArray();
            }

            throw new InvalidOperationException();
        }

        public byte[] ToByte(List<TransactionArgumentLCS> source)
        {
            throw new NotImplementedException();
        }
    }
}