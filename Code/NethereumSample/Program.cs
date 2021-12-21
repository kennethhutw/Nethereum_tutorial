using System;
using Nethereum.Web3;
using System.Threading.Tasks;
using System.Text;
using Nethereum.Web3.Accounts;
using NBitcoin;
using Nethereum.HdWallet;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using System.Numerics;

using Nethereum.ABI.FunctionEncoding.Attributes;


namespace NethereumSample
{
   
    class Program
    { 
        static string _bytecode = "608060405234801561001057600080fd5b50610273806100206000396000f3fe608060405234801561001057600080fd5b506004361061004c5760003560e01c806301243fce146100515780633eb76b9c1461006f5780635b57014c1461009d578063aec2ccae146100bb575b600080fd5b610059610117565b6040518082815260200191505060405180910390f35b61009b6004803603602081101561008557600080fd5b810190808035906020019092919050505061011d565b005b6100a5610218565b6040518082815260200191505060405180910390f35b6100fd600480360360208110156100d157600080fd5b81019080803573ffffffffffffffffffffffffffffffffffffffff16906020019092919050505061021e565b604051808215151515815260200191505060405180910390f35b60005481565b600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060009054906101000a900460ff16158015610182575060018114806101815750600281145b5b61018b57600080fd5b60018114156101aa5760008081548092919060010191905055506101bd565b6001600081548092919060010191905055505b6001600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060006101000a81548160ff02191690831515021790555050565b60015481565b60026020528060005260406000206000915054906101000a900460ff168156fea265627a7a7231582050ad3372b8c7cb32051f666b2eaac62eb9ed41c353e2db4a79a95d235ab39e8964736f6c63430005110032";
        static string _abi ="[{'constant':true,'inputs':[],'name':'candidate1','outputs':[{'internalType':'uint256','name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'candidate2','outputs':[{'internalType':'uint256','name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'internalType':'uint256','name':'candidate','type':'uint256'}],'name':'castVote','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'internalType':'address','name':'','type':'address'}],'name':'voted','outputs':[{'internalType':'bool','name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'}]";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SignMsg();
            //  NonceSrv().Wait();
            //  EstimateGasFee().Wait();
            //  CallSmartContractMethod().Wait();
            //  DeploySmartContract().Wait();
            //  generateHDWallet();
            //  TransferEther().Wait();
            //  TestUnit().Wait();
            //  ImportFromKeyStore();
            //  ImportKey();
            //  GetAccountBalance().Wait();
            //  Key();
        }

        static void SignMsg(){
            var message = "This is the message";
            var privatekey = "fbde582d0deb10b30d0c1be8752650a9f4fc12c705610cb754b7ae3b0a2d4aa7";
            var providedaddress ="0x43c020cA68FfaA09AFc1674A78047C1b40CBb60E";
            var signer = new Nethereum.Signer.MessageSigner();
            var signature = signer.HashAndSign(message, privatekey);

            Console.WriteLine("Signature : " +signature );

            var address = signer.HashAndEcRecover(message, signature);
            Console.WriteLine("Address : " + address);
            Console.WriteLine(address == providedaddress);
        }

        static async Task NonceSrv(){

             Account _account = new Account("fbde582d0deb10b30d0c1be8752650a9f4fc12c705610cb754b7ae3b0a2d4aa7", 5777);
            // var account = new Account(privatekey, Nethereum.Signer.Chain.MainNet);
            var _web3 = new Web3(_account,"http://127.0.0.1:7545");
       
            _web3.TransactionManager.UseLegacyAsDefault = true;

            _account.NonceService = new Nethereum.RPC.NonceServices.InMemoryNonceService(_account.Address, _web3.Client);
            var  currentNonce =await _web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(_account.Address, Nethereum.RPC.Eth.DTOs.BlockParameter.CreatePending());
            Console.WriteLine("Current Nonce: " + currentNonce);
        }

        [Function("castVote")]
        public class VotingFunc: FunctionMessage
        {
            [Parameter("uint256","candidate",1)]
            public BigInteger candidate{get; set;}
        }

        static async Task EstimateGasFee(){
            var _contractAddress = "0x556aE816E31AdaCf5F64bE5C36c7565bD84B1237";
              Account _account = new Account("fbde582d0deb10b30d0c1be8752650a9f4fc12c705610cb754b7ae3b0a2d4aa7", 5777);
            // Account account = new Account(privatekey, Nethereum.Signer.Chain.MainNet);
            var _web3 = new Web3(_account,"http://127.0.0.1:7545");
       
            _web3.TransactionManager.UseLegacyAsDefault = true;

            var votingHandler = _web3.Eth.GetContractTransactionHandler<VotingFunc>();
            var _candidate = new VotingFunc(){
                candidate =2
            };

            var _estimateGas = await votingHandler.EstimateGasAsync(_contractAddress, _candidate);

            Console.WriteLine("Transaction gas estimate : " + _estimateGas.Value);
        }

        static async Task CallSmartContractMethod(){
            var _contractAddress = "0x556aE816E31AdaCf5F64bE5C36c7565bD84B1237";
            Account _account = new Account("f4787bddd02d87d671697809885a3b36f902f4545a77556840b6b52a7dac8c5a", 5777);
             // Account account = new Account(privatekey, Nethereum.Signer.Chain.MainNet);
            var web3 = new Web3(_account, "http://127.0.0.1:7545");
              web3.TransactionManager.UseLegacyAsDefault = true;

            Contract voteContract = web3.Eth.GetContract(_abi,_contractAddress);

            var candidate1 = await voteContract.GetFunction("candidate1").CallAsync<BigInteger>();
            Console.WriteLine("candidate1 has "+candidate1+" vote");
            var candidate2 = await voteContract.GetFunction("candidate2").CallAsync<BigInteger>();
            Console.WriteLine("candidate2 has "+candidate2+" vote");

            var gas = new Nethereum.Hex.HexTypes.HexBigInteger(400000);
            var value = new Nethereum.Hex.HexTypes.HexBigInteger(0);

            var castVoteResult = await voteContract.GetFunction("castVote").SendTransactionAsync(_account.Address,gas,value,1);

            candidate1 = await voteContract.GetFunction("candidate1").CallAsync<BigInteger>();
            Console.WriteLine("candidate1 has "+candidate1+" vote");
            candidate2 = await voteContract.GetFunction("candidate2").CallAsync<BigInteger>();
            Console.WriteLine("candidate2 has "+candidate2+" vote");
        }

        static async Task DeploySmartContract(){
            Account _account = new Account("fbde582d0deb10b30d0c1be8752650a9f4fc12c705610cb754b7ae3b0a2d4aa7", 5777);
              // Account account = new Account(privatekey, Nethereum.Signer.Chain.MainNet);
            var web3 = new Web3(_account, "http://127.0.0.1:7545");
            web3.TransactionManager.UseLegacyAsDefault = true;

            var gasPrice = new Nethereum.Hex.HexTypes.HexBigInteger(8000000);
            var txHash = await web3.Eth.DeployContract.SendRequestAsync(_abi,_bytecode,_account.Address, gasPrice);
            Console.WriteLine("TxHash : "+ txHash);

        }

        static void generateHDWallet(){
            var mnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
            Console.WriteLine("The 12 seed words are : "+ mnemonic.ToString());

            var password="";
            var wallet = new Wallet(mnemonic.ToString(), password);
            // var account = wallet.GetAccount(0);
            // Console.WriteLine("Address at Index 0 is : " + account.Address + " with private key : "+ account.PrivateKey );
            for(int i = 0 ; i <10; i++){
                  var account = wallet.GetAccount(i);
            Console.WriteLine("Address at Index "+ i +" is : " + account.Address + " with private key : "+ account.PrivateKey );
            }
        }

        static async Task TestUnit(){
            var web3 = new Web3("https://mainnet.infura.io/v3/7238211010344719ad14a89db874158c");

            var balance =await web3.Eth.GetBalance.SendRequestAsync("0x84295d5e054d8cff5a22428b195F5A1615bD644F");

            Console.WriteLine("Balance of Ethereum Foundation's account : "+ balance.Value);

            var balanceInEther = Web3.Convert.FromWei( balance.Value);
            Console.WriteLine("Balance of Ethereum Foundation's account in Ethere : "+ balanceInEther);

            var balanceInWei = Web3.Convert.ToWei(balanceInEther);
            Console.WriteLine("Balance of Ethereum Foundation's account in Wei : "+ balanceInWei);
        }

        static async Task TransferEther(){
            var privateKey = "fbde582d0deb10b30d0c1be8752650a9f4fc12c705610cb754b7ae3b0a2d4aa7";

            var account = new Account(privateKey, 5777);
            // Account account = new Account(privatekey, Nethereum.Signer.Chain.MainNet);

            var web3 = new Web3(account,"http://127.0.0.1:7545");
            web3.TransactionManager.UseLegacyAsDefault = true;
            var balance = await web3.Eth.GetBalance.SendRequestAsync(account.Address);
            Console.WriteLine($"Balance in wei : {balance.Value}");

            var tx = await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync("0x86Ac1893ff1Ef07Ec709C6b81516bc25eB3FF7aE",10);

            balance = await web3.Eth.GetBalance.SendRequestAsync(account.Address);
            Console.WriteLine($"Balance in wei : {balance.Value}");

        }

        static void ImportFromKeyStore(){
            var jsonContent = @"{""address"":""0d4b18a9f46c8e235038fe4670013df2227d4311"",""crypto"":{""cipher"":""aes-128-ctr"",""ciphertext"":""bc38faaad7b1c830ee87b211ab0b264dfffd23c2e58cde2d89df5c2ba5d7e85d"",""cipherparams"":{""iv"":""6d4c286ad6dc9f9dcef72e7875bad7dd""},""kdf"":""scrypt"",""kdfparams"":{""dklen"":32,""n"":262144,""p"":1,""r"":8,""salt"":""ff4f071683b6a819d3b1541a53e2560d91cf5e69a59573109ea5adfa9887ea37""},""mac"":""bd11200f160c7548dc853b1b997a8158c677507e71ef04435586183081af998d""},""id"":""03080d1e-dcf1-4e6a-b708-82e410d569f0"",""version"":3}";
            var password ="";
            var account = Account.LoadFromKeyStore(jsonContent,password);
            Console.WriteLine($"Address : {account.Address}");

        }

        static void ImportKey(){
            var privateKey ="0x2c12239cba46542f92bf6045f6c3b3c697a1d76a39535a95ae4a7bd3d7225705";
            var account = new Account(privateKey);
            Console.WriteLine($"Address : {account.Address}");
        }
       
        static async Task GetAccountBalance(){
            var web3 = new Web3("http://127.0.0.1:7545");
            var balance = await web3.Eth.GetBalance.SendRequestAsync("0x86Ac1893ff1Ef07Ec709C6b81516bc25eB3FF7aE");
            Console.WriteLine($"Balance in wei : {balance.Value}");
        }

        static void Key(){
            var ecKey = Nethereum.Signer.EthECKey.GenerateKey();

            var privateKey = ecKey.GetPrivateKey();
            var publickeyByte = ecKey.GetPubKey();
            var pbulickey = ByteArrayToString(publickeyByte);
            var address = ecKey.GetPublicAddress();
            Console.WriteLine($"Private Key : {privateKey}");
            Console.WriteLine($"Public Key : {pbulickey}");
            Console.WriteLine($"Address : {address}");
        }

        static string ByteArrayToString(byte[] ba){
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach(byte b in ba)
                hex.AppendFormat("{0:x2}",b);
            return hex.ToString();
        }
    }
}
