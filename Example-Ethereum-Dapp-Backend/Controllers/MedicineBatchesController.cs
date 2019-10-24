using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace Example_Ethereum_Dapp_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineBatchesController : ControllerBase
    {
        private readonly Web3 web3;
        private readonly Contract contract;
        private readonly string ethereumAccount;
        public MedicineBatchesController(IOptions<EthereumSettings> options)
        {
            var privateKey = options.Value.EthereumPrivateKey;
            var account = new Account(privateKey);
            web3 = new Web3(account, "https://ropsten.infura.io/v3/ad8ea364154b464eb6c7ff37f66ffc94");

            var abi = options.Value.Abi;
            var contractAddress = options.Value.ContractAddress;
            if (String.IsNullOrEmpty(abi) || String.IsNullOrEmpty(contractAddress))
            {
                throw new ArgumentNullException();
            }
            contract = web3.Eth.GetContract(abi, contractAddress);

            ethereumAccount = options.Value.EthereumAccount;
        }

        // GET api/values
        [HttpGet("name")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await contract.GetFunction("name").CallAsync<string>(
                    ethereumAccount,
                    new HexBigInteger(500000),
                    new HexBigInteger(0));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateMedicineBatchCommand command)
        {
            try
            {
                var result = await contract.GetFunction("updateMedicineBatchInformations").SendTransactionAndWaitForReceiptAsync(
                    ethereumAccount,
                    new HexBigInteger(1000000),
                    new HexBigInteger(Web3.Convert.ToWei(50, UnitConversion.EthUnit.Gwei)),
                    new HexBigInteger(0),
                    functionInput: new object[] {
                        command.Name,
                        command.RegistrationCode,
                        command.BatchNumber,
                        command.Quantity,
                        command.ManufacturingDate,
                        command.ExpiryDate,
                        command.DosageForm
                    });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

    }
}
