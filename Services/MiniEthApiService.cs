
using Nethereum.Hex.HexConvertors.Extensions;
using metamask_mini_api.Shared;
using Nethereum.Signer;
using Nethereum.RLP;

namespace metamask_mini_api.Services;
public class MinimalEthApi
{
    public SimpleBlockChain BlockChain { get; set; }
    public MinimalEthApi(SimpleBlockChain blockChain)
    {
        BlockChain = blockChain;
    }
    public const string ChainId = "0x3039";//12345
    public JsonRpcResponse ProcessRequest(JsonRpcRequest request)
    {
        var parameters = GetParametersAsString(request.parameters);
        var firstParameter = parameters.First();
        object? result = request.method switch
        {
            "eth_chainId" => ChainId,
            "eth_blockNumber" => GetBlockNumber(),
            "eth_getBalance" => GetBalance(firstParameter),
            "net_version" => ChainId,
            "eth_getBlockByNumber" => GetBlockByNumber(firstParameter),
            "eth_gasPrice" => GetGasPrice(),
            "eth_estimateGas" => EstimateGas(),
            "eth_getCode" => GetCode(),
            "eth_getTransactionCount" => GetTransactionCount(),
            "eth_sendRawTransaction" => SendRawTransaction(firstParameter),
            "eth_getTransactionReceipt" => GetTransactionReceipt(firstParameter),
            "eth_getBlockByHash" => GetBlockByHash(firstParameter),
            _ => throw new Shared.JsonRpcErrorException(new JsonRpcError(ErrorType.NotImplemented, request.method))
        };
        return new JsonRpcResponse(request.id, result, null);
    }

    private Shared.SimpleBlockReceipt GetBlockByHash(string blockHash)
    {
        if (!BlockChain.Blocks.ContainsKey(blockHash))
        {
            throw new Shared.JsonRpcErrorException(
                new JsonRpcError(
                    ErrorType.InvalidRequest,
                    "Cannot find block by hash : " + blockHash)
            );
        }
        var block = BlockChain.Blocks[blockHash];
        return new SimpleBlockReceipt(block.Transactions.Keys.ToArray(), block.Hash, block.ParentHash, block.Timestamp, block.BlockNumber, "0x4db7a1c01d8a8072");
    }

    private Shared.SimpleTransactionReceipt? GetTransactionReceipt(string transactionHash)
    {
        if (!BlockChain.TransactionPool.ContainsKey(transactionHash))
        {
            foreach (var block in BlockChain.Blocks)
            {
                if (block.Value.Transactions.ContainsKey(transactionHash))
                {
                    var index = Array.IndexOf(block.Value.Transactions.Keys.ToArray(), transactionHash);
                    var transaction = BlockChain.Transactions[transactionHash];
                    var errorMessage = block.Value.Transactions[transactionHash];
                    var status = string.IsNullOrEmpty(errorMessage) ? "0x1" : "0x0";
                    return new SimpleTransactionReceipt(
                        block.Value.Hash,
                        block.Value.BlockNumber,
                        transaction.From,
                        "0x0",
                        status,
                        transaction.To,
                        transactionHash,
                        index.ToHexString());
                }
            }
        }
        return null;
    }

    private string SendRawTransaction(string transactionEncoded)
    {
        byte[] rlpEncoded = transactionEncoded.HexToByteArray();
        var rlpDecoded = Nethereum.RLP.RLP.Decode(rlpEncoded);
        var transaction = new Nethereum.Signer.LegacyTransactionChainId(rlpEncoded);
        var transactionHash = BlockChain.AddTransaction(
            transaction.GetSenderAddress().ToLower(),
            transaction.ReceiveAddress.ToHexString().ToLower(),
            transaction.Value.ToBigIntegerFromRLPDecoded(),
            transactionEncoded);
        return transactionHash;
    }

    private string GetTransactionCount()
    {
        return "0x0";
    }

    private string GetCode()
    {
        return "0x0";
    }

    private string EstimateGas()
    {
        return "0x5208";
    }

    private string GetGasPrice()
    {
        return "0x0";
    }

    private SimpleBlock? GetBlockByNumber(string blockNumber)
    {
        var result = BlockChain.Blocks.Values.FirstOrDefault(x => x.BlockNumber == blockNumber);
        return result;
    }

    private string GetBalance(string address)
    {
        var result = "0x0";
        address = address.ToLower();
        if (LastBlock.Balances.ContainsKey(address))
        {
            return LastBlock.Balances[address].ToHexString();
        }
        return result;
    }

    private string GetBlockNumber()
    {
        return LastBlock.BlockNumber;
    }

    public SimpleBlock LastBlock => BlockChain.Blocks.Last().Value;

    public IEnumerable<string> GetParametersAsString(object[]? parameters)
    {
        if (parameters != null && parameters.Any())
        {
            return parameters.Select(x => $"{x}");
        }
        return new string[] { string.Empty };
    }
}