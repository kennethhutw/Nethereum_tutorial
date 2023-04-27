using System;
using System.Threading;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.StandardNonFungibleTokenERC721;
using Nethereum.StandardNonFungibleTokenERC721.ContractDefinition;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System.Numerics;


namespace NethereumERC721
{

  
    class Program
    {
        static   void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // deployERC721().Wait();
            // MintingERC721().Wait();
            // BalanceAndOwnerERC721().Wait();
             BalanceAndOwner2ERC721().Wait();
             TransferFrom2ERC721().Wait();
             BalanceAndOwner2ERC721().Wait();
      
        }

        [Function("safeTransferFrom")]
       public class SafeTransferFromFucntion : FunctionMessage{
         [Parameter("address","from",1)]
            public string From{get; set;}

             [Parameter("address","to",2)]
            public string To{get; set;}
          
            [Parameter("uint256","tokenId",3)]
            public BigInteger TokenId{get; set;}

       }

         static async Task TransferFrom2ERC721(){
            var privatekey = "";

            var account = new Account(privatekey);

            var web3 = new Web3(account, "HTTP://127.0.0.1:8545");
  
            var safeTransferFromMessage = new SafeTransferFromFucntion(){
                From="0x5375E7270d796dCD6c4048809539D3CC04476c32",
                To="0x796838555b8C3730377b008aA0cC7a164d1A9276",
                TokenId=1
            };

            var contractHandler = web3.Eth.GetContractHandler("0xF1dd6A4F045dDAf300269aA556d74Ef8EbFb5B1B");

            var result  =  await contractHandler.SendRequestAsync(safeTransferFromMessage);

            Console.WriteLine("result : " + result);
          
       } 
    
           static async Task TransferFromERC721(){
            var privatekey = "";

            var account = new Account(privatekey);

            var web3 = new Web3(account, "HTTP://127.0.0.1:8545");

            var tokenService = new ERC721Service(web3,"0xF1dd6A4F045dDAf300269aA556d74Ef8EbFb5B1B");

            var result = await tokenService.TransferFromRequestAndWaitForReceiptAsync("0x796838555b8C3730377b008aA0cC7a164d1A9276","0x5375E7270d796dCD6c4048809539D3CC04476c32", 1, new CancellationTokenSource());
        
            Console.WriteLine("result : " + result);
            
       } 
      
         static async Task BalanceAndOwner2ERC721(){
            var privatekey = "";

            var account = new Account(privatekey);

            var web3 = new Web3(account, "HTTP://127.0.0.1:8545");
  
            var ownerOfMessage = new OwnerOfFucntion(){TokenId=1};
            var queryHandler = web3.Eth.GetContractQueryHandler<OwnerOfFucntion>();

            var owner  =  await queryHandler.QueryAsync<string>("0xF1dd6A4F045dDAf300269aA556d74Ef8EbFb5B1B", ownerOfMessage).ConfigureAwait(false);

            var balanceOfMessage = new BalanceOfFucntion(){Owner = owner};

            var queryHanlder2 = web3.Eth.GetContractQueryHandler<BalanceOfFucntion>();

            var balance = await queryHanlder2.QueryAsync<BigInteger>("0xF1dd6A4F045dDAf300269aA556d74Ef8EbFb5B1B", balanceOfMessage);

        // 0xF1dd6A4F045dDAf300269aA556d74Ef8EbFb5B1B
            Console.WriteLine("owner : " + owner);
             Console.WriteLine("balance : " + balance);
       } 
    
        
       [Function("ownerOf","address")]
       public class OwnerOfFucntion : FunctionMessage{
          
            [Parameter("uint256","tokenId",1)]
            public BigInteger TokenId{get; set;}

       }

       
       [Function("balanceOf", "uint256")]
       public class BalanceOfFucntion : FunctionMessage{
            [Parameter("address","owner",1)]
            public string Owner{get; set;}

       }


         static async Task BalanceAndOwnerERC721(){
            var privatekey = "";

            var account = new Account(privatekey);

            var web3 = new Web3(account, "HTTP://127.0.0.1:8545");

            var tokenService = new ERC721Service(web3,"0xF1dd6A4F045dDAf300269aA556d74Ef8EbFb5B1B");

            var owner = await tokenService.OwnerOfQueryAsync(1);
            var balance = await tokenService.BalanceOfQueryAsync(owner);
        // 0xF1dd6A4F045dDAf300269aA556d74Ef8EbFb5B1B
            Console.WriteLine("owner : " + owner);
             Console.WriteLine("balance : " + balance);

       } 
    
      
       static async Task deployERC721(){
        var privatekey = "";

        var account = new Account(privatekey);

        var web3 = new Web3(account, "HTTP://127.0.0.1:8545");

        var deploymentContract = new ERC721Deployment();

        var token = await ERC721Service.DeployContractAndWaitForReceiptAsync(web3, deploymentContract);

        Console.WriteLine("Contract Address : " + token.ContractAddress);
       } 
    
       [Function("safeMint")]
       public class SafeMintFucntion : FunctionMessage{
            [Parameter("address","to",1)]
            public string To{get; set;}

            [Parameter("string","uri",2)]
            public string Uri{get; set;}

       }
       static async Task MintingERC721(){
            var privatekey = "";

            var account = new Account(privatekey);

            var web3 = new Web3(account, "HTTP://127.0.0.1:8545");

            var safeMintFun = new SafeMintFucntion(){
                To = account.Address,
                Uri ="ipfs://QmdYeDpkVZedk1mkGodjNmF35UNxwafhFLVvsHrWgJoz6A/beanz_metadata/1"
            };

        // 0xF1dd6A4F045dDAf300269aA556d74Ef8EbFb5B1B

            var contractHandler = web3.Eth.GetContractHandler("0xF1dd6A4F045dDAf300269aA556d74Ef8EbFb5B1B");
            var result = await contractHandler.SendRequestAsync(safeMintFun);


            Console.WriteLine("result : " + result);
       } 
    

    }
}
