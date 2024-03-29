﻿using System;
using System.Text;
using Nethereum.Hex.HexConvertors.Extensions;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.KeyStore.Model;


namespace Keystore
{
   
    class Program
    { 
       
        static void Main(string[] args)
        {
            var keyStoreService = new Nethereum.KeyStore.KeyStoreScryptService();
            // lower cost than default N == 262144 as this is using wasm, the lower the easier to compute but also easier to crack 
            var scryptParams = new ScryptParams {Dklen = 32, N = 32, R = 1, P = 8};
            var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            Console.WriteLine("Private key:" + ecKey.GetPrivateKey());
            var password = "testPassword";
            // encrypting using our custome scrypt params
            var keyStore = keyStoreService.EncryptAndGenerateKeyStore(password, ecKey.GetPrivateKeyAsBytes(),
                ecKey.GetPublicAddress(), scryptParams);
            var json = keyStoreService.SerializeKeyStoreToJson(keyStore);
            Console.WriteLine(json);
            //decrypting our key
            var key = keyStoreService.DecryptKeyStoreFromJson(password, json);
            Console.WriteLine("Private key decrypted:" + key.ToHex(true));
        }
    }
      
}
