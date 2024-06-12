using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace WagnerBillInfoService.Models
{
    public class TillPaymentGateway
    {
        public class TillResponseCode
        {
            public string rescode { get; set; }
        }
        public class tillcallback
        {
            public string callback { get; set; }
        }
        public class TillPaidResponse
        {
            public string transactionStatus { get; set; }
            public bool success { get; set; }
            public string uuid { get; set; }
            public string merchantTransactionId { get; set; }
            public string purchaseId { get; set; }
            public string transactionType { get; set; }
            public string paymentMethod { get; set; }
            public string amount { get; set; }
            public string currency { get; set; }
            public TillCustomer customer { get; set; }
            public TillReturnData returnData { get; set; }
        }
        public class TillCustomer
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string emailVerified { get; set; }
            public string ipAddress { get; set; }
        }
        public class TillReturnData
        {
            public string _TYPE { get; set; }
            public string type { get; set; }
            public string cardHolder { get; set; }
            public int expiryMonth { get; set; }
            public int expiryYear { get; set; }
            public string firstSixDigits { get; set; }
            public string lastFourDigits { get; set; }
            public string fingerprint { get; set; }
            public string threeDSecure { get; set; }
            public string eci { get; set; }
            public string binBrand { get; set; }
            public string binBank { get; set; }
            public string binType { get; set; }
            public string binLevel { get; set; }
            public string binCountry { get; set; }
        }

        public class TillBodyData
        {
            public string merchantTransactionId { get; set; }
            //public string referenceUuid { get; set; }
            public string amount { get; set; }
            public string currency { get; set; }
        }
    }
}