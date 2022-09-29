# metamask-mini-api
This project is a demo of how to implement the minimum required ethereum rpc apis to be able to accept metamask transactions.

Methods implemented
  - eth_chainId
  - eth_blockNumber
  - eth_getBalance
  - eth_getBlockByNumber
  - eth_gasPrice
  - eth_estimateGas
  - eth_getCode
  - eth_getTransactionCount
  - eth_sendRawTransaction
  - eth_getTransactionReceipt
  - eth_getBlockByHash

I found that the ethereum apis are well documented on infura's website (ex : [getBlockByHash](https://docs.infura.io/infura/networks/ethereum/json-rpc-methods/eth_getblockbyhash))


# DEMO : 
Configure metamask
url : http://localhost:5035/
chainid: 12345

Add these two wallets (prefunded in genesis block) : 
public : 0x7bdCc0809d6954049E15c0D2De49E6d608c6c64F
private : 3afc555a9243069a7e7891afe00fe3b5ae6637fd3e3b23b8fc79d4969828789b

public : 0xc90a1E4F11248B7657fC845cAE769E00463C81b8
private : 88bd3ffa4871b96f2d2e353fee8bfa5ff1f79531eb3d4a9acf0c0b943f8b156f