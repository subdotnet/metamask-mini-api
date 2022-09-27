using System.Numerics;

namespace metamask_mini_api.Shared;
public record SimpleBlock(string BlockNumber, string ParentHash, string Timestamp, Dictionary<string, string> Transactions, Dictionary<string, BigInteger> Balances, string Hash);
public record SimpleTransaction(string Timestamp, string From, string To, BigInteger Amount, string Data, string Hash);
public record SimpleBlockReceipt(string[] transactions, string hash, string parentHash, string timestamp, string number, string nonce);
public record SimpleTransactionReceipt(
string blockHash, 
string blockNumber,
string from,
string gasUsed,
string status,
string to,
string transactionHash,
string transactionIndex
);