using System;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.RPC.Eth.DTOs;

using Nethereum.StandardTokenEIP20;
using Nethereum.StandardTokenEIP20.ContractDefinition;
using Nethereum.StandardTokenEIP20.Events.DTO;

using System.Linq;

namespace NethereumERC20
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
             TrasferFromDemo().Wait();
            // TransferToken().Wait();
            // DisplayTokenInfo().Wait();
            // deployERC20().Wait();
        }

      static async Task TrasferFromDemo(){
             var _TokenAddress = "0x4e3c5117ed03c6e3a47f4414506f2f4723ce79e1";
            var _account1Address = "0x4438083Ef903ACD146D3f705d790f9188d5B11Ac";
            var _account1 = new Account("fbde582d0deb10b30d0c1be8752650a9f4fc12c705610cb754b7ae3b0a2d4aa7");
            var _web3 = new Web3(_account1,"http://127.0.0.1:7545");

            var _tokenService = new StandardTokenService(_web3, _TokenAddress);

            var ownerBalance = await _tokenService.BalanceOfQueryAsync(_account1Address);
            Console.WriteLine("Account1's Balance : " +ownerBalance );

            var _account2Address = "0x43c020cA68FfaA09AFc1674A78047C1b40CBb60E";
            var _account3Address="0x86Ac1893ff1Ef07Ec709C6b81516bc25eB3FF7aE";

            var approveTransactionReceipt = await _tokenService.ApproveRequestAndWaitForReceiptAsync(_account3Address,5000);

            var allowanceAmount = await _tokenService.AllowanceQueryAsync(_account1Address, _account3Address);

            Console.WriteLine("allowanceAmount : "+ allowanceAmount);


            var _account3 = new Account("f4787bddd02d87d671697809885a3b36f902f4545a77556840b6b52a7dac8c5a");
             _web3 = new Web3(_account3,"http://127.0.0.1:7545");

             var _tokenServiceFrom = new StandardTokenService(_web3, _TokenAddress);

             var TransferFromReceipt = await _tokenServiceFrom.TransferFromRequestAndWaitForReceiptAsync(_account1Address,_account2Address,2000);
            Console.WriteLine("================ TransferFrom =====================");
            ownerBalance = await _tokenServiceFrom.BalanceOfQueryAsync(_account1Address);
            Console.WriteLine("Account1's Balance : " +ownerBalance );
             allowanceAmount = await _tokenServiceFrom.AllowanceQueryAsync(_account1Address, _account3Address);

            Console.WriteLine("allowanceAmount : "+ allowanceAmount);   
      }

        static async Task TransferToken(){
            var _TokenAddress = "0x4e3c5117ed03c6e3a47f4414506f2f4723ce79e1";
            var _owner = "0x4438083Ef903ACD146D3f705d790f9188d5B11Ac";
            var _account = new Account("fbde582d0deb10b30d0c1be8752650a9f4fc12c705610cb754b7ae3b0a2d4aa7");
            var _web3 = new Web3(_account,"http://127.0.0.1:7545");

            var _tokenService = new StandardTokenService(_web3, _TokenAddress);

            var ownerBalance = await _tokenService.BalanceOfQueryAsync(_owner);
            Console.WriteLine("Balance : " +ownerBalance );

            var _account2 = "0x43c020cA68FfaA09AFc1674A78047C1b40CBb60E";

            var transferReceipt = await _tokenService.TransferRequestAndWaitForReceiptAsync(_account2,1500);
            ownerBalance = await _tokenService.BalanceOfQueryAsync(_owner);
            Console.WriteLine("Balance : " +ownerBalance );

            var account2Balance = await _tokenService.BalanceOfQueryAsync(_account2);
            Console.WriteLine("Account2's Balance : " +account2Balance );

            var _transferEvent = _tokenService.GetTransferEvent();

            var _allTransferFilter = await _transferEvent.CreateFilterAsync(new BlockParameter(transferReceipt.BlockNumber));

            var eventLogsAll = await _transferEvent.GetAllChanges(_allTransferFilter);

            var transferLog = eventLogsAll.First();

            Console.WriteLine("TxID : "+ transferLog.Log.TransactionHash);
            Console.WriteLine("BlockNumber : "+ transferLog.Log.BlockNumber.Value);
            Console.WriteLine("To : "+ transferLog.Event.To.ToLower());
            Console.WriteLine("Value : "+ transferLog.Event.Value);


        }

        static async Task DisplayTokenInfo(){
            var _TokenAddress = "0x4e3c5117ed03c6e3a47f4414506f2f4723ce79e1";
            var _account = new Account("fbde582d0deb10b30d0c1be8752650a9f4fc12c705610cb754b7ae3b0a2d4aa7");
            var _web3 = new Web3(_account,"http://127.0.0.1:7545");

            var _tokenService = new StandardTokenService(_web3, _TokenAddress); 

            var _totalSupply = await _tokenService.TotalSupplyQueryAsync();
            Console.WriteLine("TotalSupply : " +_totalSupply);

            var _tokenName= await _tokenService.NameQueryAsync();
            Console.WriteLine("TokenName : " +_tokenName);

            var _tokenSymbol = await _tokenService.SymbolQueryAsync();
            Console.WriteLine("TokenSymbol : " +_tokenSymbol);

        }
        static async Task deployERC20(){
            var _account = new Account("fbde582d0deb10b30d0c1be8752650a9f4fc12c705610cb754b7ae3b0a2d4aa7");
            var _web3 = new Web3(_account,"http://127.0.0.1:7545");
            ulong totalSupply = 2000000;
            var deploymentContract = new EIP20Deployment(){
                InitialAmount=totalSupply,
                TokenName="TestToken",
                TokenSymbol="TT"
            };

            var tokenService = await StandardTokenService.DeployContractAndWaitForReceiptAsync(_web3, deploymentContract);

            Console.WriteLine("Contract Address: "+ tokenService.ContractAddress);
        }
    }
}
