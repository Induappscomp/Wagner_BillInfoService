using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using WagnerBillInfoService.Models;

using Newtonsoft.Json;
using System.IO;
using System.Data;

using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.CommonServices;
using System.Text;
using TradingBell.WebCat.TemplateRender;
using RestSharp;

namespace WagnerBillInfoService.Controllers
{
    public class WagnerBillInfoController : ApiController
    {
        public Exception smException;
        public StreamWriter sw;
        HelperDB objHelperDB = new HelperDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperServices objHelperServices = new HelperServices();
        OrderServices objOrderServices = new OrderServices();

        PaymentServices objPaymentServices = new PaymentServices();
        PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();

        OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();

        UserServices objUserServices = new UserServices();
        public int PaymentID = 0;

        [HttpPost]
        public HttpResponseMessage GetResponseData(string transid,string ShipOrderID)
        {
            //CreatePayLog("Wagner Ship Order ID" + ShipOrderID);
            string retstring = string.Empty;
            int OrderID = 0;
            if (!string.IsNullOrEmpty(ShipOrderID))
            {
                OrderID = Convert.ToInt32(ShipOrderID);
            }
            oPayInfo = objPaymentServices.GetPayment(OrderID);
            PaymentID = oPayInfo.PaymentID;
            //CreatePayLog("Payment ID" + PaymentID);
            string mertransid = string.Empty;
            mertransid = transid;
            int transcount = 0;
            if (!string.IsNullOrEmpty(mertransid))
            {
                transcount = objPaymentServices.GetTransactionCount(mertransid);
            }
            IRestResponse response = null;
            //string tillTransget = "https://test-gateway.tillpayments.com/api/v3/status/JgJVM3cHT5mCQk8yJQX5NdvWRrRtGH/getByMerchantTransactionId/";
            //string tillusername = "WesAlliance_API_Dev";
            //string tillpassword = "*G)dU$1DP$(wOsb&r)XtYRh/+dHtd";

            ////Till Live Credentials
            string tillTransget = "https://gateway.tillpayments.com/api/v3/status/HywkXO7mayo4DlpMVpAHUmgOyvWmTu/getByMerchantTransactionId/";
            //string tillTransget = "https://gateway.tillpayments.com/api/v3/transaction/HywkXO7mayo4DlpMVpAHUmgOyvWmTu/debit";
            string tillusername = "WagnerElectronics_API_Live";
            string tillpassword = "BLzMbglq#rSa2GX@=Yi+8EW/tY_.l";


            TillPaymentGateway.tillcallback resval = new TillPaymentGateway.tillcallback();
            var client = new RestClient(tillTransget + mertransid + "");
            var request = new RestRequest(Method.GET);
            CreatePayLog("URL " + client);
            var data = "{}";
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            request.AddHeader("accept", "application/json");
            string encodeval = GenerateBasicAuthorizationParameter(tillusername, tillpassword);
            request.AddHeader("Authorization", encodeval);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "", ParameterType.RequestBody);

            response = client.Execute(request);
            TillPaymentGateway.TillResponseCode retdata = new TillPaymentGateway.TillResponseCode();
            //TillPaymentGateway.TillPaidResponse jObject = new TillPaymentGateway.TillPaidResponse();
            if (response != null)
            {
                //response.Request.AddParameter("application/json", , ParameterType.RequestBody);
                CreatePayLog(response.Content);
                TillPaymentGateway.TillPaidResponse jObject = JsonConvert.DeserializeObject<TillPaymentGateway.TillPaidResponse>(response.Content);
                retdata.rescode = response.StatusDescription;
                //retstring = JsonConvert.DeserializeObject(response.Content).ToString();
                string ordstatus = objOrderServices.GetOrderStatus(OrderID);
                //string cardnumber = jObject.returnData.firstSixDigits + "xxxxxx" + jObject.returnData.lastFourDigits;
                //CreatePayLog("Order Status" + ordstatus);
                //CreatePayLog("ExpiryDate" + jObject.returnData.expiryMonth + "" + jObject.returnData.expiryYear);

                DataTable dtcheckresponse = objOrderServices.Get_Check_Request_Response(mertransid);
                if (dtcheckresponse == null || dtcheckresponse.Rows.Count == 0)
                {
                    if (jObject.success != false)
                    {
                        if (ordstatus == OrderServices.OrderStatus.OPEN.ToString() && jObject.transactionStatus.ToLower() != "error")
                        {
                            objErrorHandler.CreatePayLog(response.Content);
                            objErrorHandler.CreatePayLog(response.ResponseStatus.ToString());

                            //objOrderServices.UpdatePaymentOrderStatus(OrderID, PaymentID, false);
                            SecurePayService objSecurePayService = new SecurePayService();

                            objSecurePayService.GetPaymentRequest_braintree(OrderID, PaymentID, jObject.returnData.type,
                            jObject.returnData.cardHolder, jObject.returnData.firstSixDigits + "xxx" + jObject.returnData.lastFourDigits, "", jObject.returnData.expiryMonth + "/" + jObject.returnData.expiryYear, "Yes", "000/" + mertransid + "", "01", jObject.uuid, jObject.transactionStatus, jObject.transactionStatus, "TI");
                            objErrorHandler.CreatePayLog("After update"+ OrderID);
                            //objOrderServices.UpdatePaymentOrderStatus_Express(OrderID, PaymentID, false);
                            //objOrderServices.UpdatePAYMENTSELECTION(OrderID, "TI");
                            //TBWTemplateEngine tbwtEngine = new TBWTemplateEngine();
                            //tbwtEngine.SendMail_AfterPaymentSP(OrderID, (int)OrderServices.OrderStatus.Payment_Successful, false, jObject.returnData.type);
                            //tbwtEngine.SendMail_Review(OrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);

                        }
                        else if (jObject.transactionStatus.ToLower() == "error" && (ordstatus == OrderServices.OrderStatus.Proforma_Payment_Required.ToString() || ordstatus == OrderServices.OrderStatus.OPEN.ToString()))
                        {
                            if (transcount == 0)
                            {
                                CreatePayLog("Service Update Proforma Payment");

                                SecurePayService objSecurePayService = new SecurePayService();
                                objSecurePayService.GetPaymentRequest_braintree(OrderID, PaymentID, jObject.returnData.type,
                                jObject.returnData.cardHolder, jObject.returnData.firstSixDigits + "xxx" + jObject.returnData.lastFourDigits, "", jObject.returnData.expiryMonth + "/" + jObject.returnData.expiryYear, "False", "ERROR/" + mertransid + "", "02", jObject.uuid, jObject.transactionStatus, jObject.transactionStatus, "TI");
                                // objOrderServices.updateorderstatus_99_1(OrderID);
                            }
                        }

                    }
                    else
                    {
                        SecurePayService objSecurePayService = new SecurePayService();
                        objSecurePayService.GetPaymentRequest_braintree(OrderID, PaymentID, "", "", "", "", "", "No", "000", "01", "", "8001", "Transaction not found",   "TI");
                    }
                }
            }

            //else
            //{
            string json = response.StatusDescription;// something

            return new HttpResponseMessage()
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
            //return  response.StatusDescription;//response.StatusCode;// response.ResponseStatus.ToString();
            //return new HttpResponseMessage(HttpStatusCode.Accepted);
            //}
        }

        public string GenerateBasicAuthorizationParameter(string username, string password)
        {
            string unencodedUsernameAndPassword = string.Format("{0}:{1}", username, password);
            byte[] unencodedBytes = ASCIIEncoding.ASCII.GetBytes(unencodedUsernameAndPassword);
            string base64UsernameAndPassword = System.Convert.ToBase64String(unencodedBytes);

            return string.Format("Basic {0}", base64UsernameAndPassword);
        }
        public void CreatePayLog(string strvalue)
        {
            try
            {
                Thread thlog = new Thread(new ParameterizedThreadStart(CreatePayLogThread));
                thlog.Start(strvalue);
            }
            catch (Exception ex)
            {
                smException = ex;
            }
        }

        public void CreatePayLogThread(object strvalue)
        {
            try
            {
                AppDomain sPath;
                sPath = AppDomain.CurrentDomain;

                string FName = "PayLog/PayRecord" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
                string Path = sPath.BaseDirectory + FName;
                Path = Path.Replace("\\", "/");
                if (File.Exists(Path) == false)
                {
                    FileStream fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.Close();
                }
                bool rst = WriteErrorLog(Path, (string)strvalue);
            }
            catch (Exception ex)
            {
                smException = ex;
            }
        }
        public bool WriteErrorLog(string strPathName, string str)
        {
            string strException = string.Empty;
            bool bReturn = false;
            try
            {
                sw = new StreamWriter(strPathName, true);
                sw.WriteLine("Method        : " + str.ToString());
                sw.WriteLine("Date        : " + DateTime.Now.TimeOfDay);
                sw.WriteLine("Time        : " + DateTime.Now.ToShortDateString());

                sw.WriteLine("^^-------------------------------------------------------------------^^");
                sw.Flush();
                sw.Close();
                bReturn = true;
            }
            catch (Exception ex)
            {
                smException = ex;
                bReturn = false;
            }
            return bReturn;
        }

    }
}
