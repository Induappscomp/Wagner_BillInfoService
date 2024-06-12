using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Braintree;
using System.Configuration; 
namespace TradingBell.WebCat.CommonServices
{
  
    class BraintreeService
    {
        public string Environment { get; set; }
        public string MerchantId { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        private IBraintreeGateway BraintreeGateway { get; set; }
        public IBraintreeGateway CreateGateway()
        {
            Environment = System.Environment.GetEnvironmentVariable("BraintreeEnvironment");
            MerchantId = System.Environment.GetEnvironmentVariable("BraintreeMerchantId");
            PublicKey = System.Environment.GetEnvironmentVariable("BraintreePublicKey");
            PrivateKey = System.Environment.GetEnvironmentVariable("BraintreePrivateKey");

            if (MerchantId == null || PublicKey == null || PrivateKey == null)
            {
                Environment = GetConfigurationSetting("BraintreeEnvironment");
                MerchantId = GetConfigurationSetting("BraintreeMerchantId");
                PublicKey = GetConfigurationSetting("BraintreePublicKey");
                PrivateKey = GetConfigurationSetting("BraintreePrivateKey");
            }

            return new BraintreeGateway(Environment, MerchantId, PublicKey, PrivateKey);
        }

        public string GetConfigurationSetting(string setting)
        {
            return ConfigurationManager.AppSettings[setting];
        }

        public IBraintreeGateway GetGateway()
        {
            if (BraintreeGateway == null)
            {
                BraintreeGateway = CreateGateway();
            }

            return BraintreeGateway;
        }

        public string CreateTransaction()
        {
            var gateway = GetGateway();
            decimal amount;

            try
            {
                amount = Convert.ToDecimal(100);
            }
            catch (FormatException e)
            {
                //TempData["Flash"] = "Error: 81503: Amount is an invalid format.";
                //return RedirectToAction("New");
            }
          //  Result<PaymentMethodNonce> result1 = gateway.PaymentMethodNonce.Create("A_PAYMENT_METHOD_TOKEN");
            String nonce = "tokencc_bh_vn5gsz_9nhcbw_534tdr_yvrtsz_qhz";
           
            var request = new TransactionRequest
            {
                Amount = 100,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            if (result.IsSuccess())
            {
                Transaction transaction = result.Target;
                return transaction.Id;
            }
            else if (result.Transaction != null)
            {
                return result.Transaction.Id;
            }
            else
            {

                return "";
            }

        }


    }
}