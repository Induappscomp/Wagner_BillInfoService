using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json;


namespace TradingBell.WebCat.CommonServices
{
    public class MailChimpManagerServices
    {
        public string MailChimp_Url = System.Configuration.ConfigurationManager.AppSettings["MailChimp_Url"].ToString();
        public string MailChimp_Api_Key = System.Configuration.ConfigurationManager.AppSettings["MailChimp_Api_Key"].ToString();
        public string MailChimp_ListId = System.Configuration.ConfigurationManager.AppSettings["MailChimp_ListId"].ToString();
        public string CreateMailChimpMember(string dataCenter, string subscriberEmail, string firstname, string lastname, string addr1,string city,string state,string zip, string country)
        {
            string StrRtnVal = "";
            string url = "";
            //StrRtnVal = AddOrUpdateListMember(dataCenter, apiKey, listId, subscriberEmail, firstname, lastname);
            url = MailChimp_Url + "{1}/members/{2}";
           StrRtnVal = AddOrUpdateListMember(dataCenter, MailChimp_Api_Key, MailChimp_ListId, subscriberEmail, firstname, lastname, url, addr1, city, state, zip, country);

           // StrRtnVal = AddOrUpdateListMember(dataCenter, MailChimp_Api_Key, MailChimp_ListId, subscriberEmail, firstname, lastname, url, city, state, zip, country);


            return StrRtnVal;
        }



        public string ResubscribeMailChimpMember(string dataCenter, string subscriberEmail, string firstname, string lastname, string addr1, string city, string state, string zip, string country)
        {
            string StrRtnVal = "";
            string url = "";
            //StrRtnVal = AddOrUpdateListMember(dataCenter, apiKey, listId, subscriberEmail, firstname, lastname);
            url = MailChimp_Url + "{1}/members/{2}";
            StrRtnVal = ResubscribeListMember(dataCenter, MailChimp_Api_Key, MailChimp_ListId, subscriberEmail, firstname, lastname, url, addr1, city, state, zip, country);
            return StrRtnVal;
        }

        public string CreateMailChimpMemberDeal(string dataCenter, string subscriberEmail, string firstname, string lastname, string addr1, string city, string state, string zip, string country)
        {
            string StrRtnVal = "";
            string url = "";
            url = MailChimp_Url + "{1}/members/{2}";
            StrRtnVal = AddOrUpdateListMemberDeal(dataCenter, MailChimp_Api_Key, MailChimp_ListId, subscriberEmail, firstname, lastname, url, addr1, city, state, zip, country);
            return StrRtnVal;
        }

        public string UnSubscribeChimpMember(string dataCenter, string subscriberEmail)
        {
            string StrRtnVal = "";
            string url = "";
            url = MailChimp_Url + "{1}/members/{2}";
            StrRtnVal = UnSubscribeListMember(dataCenter, MailChimp_Api_Key, MailChimp_ListId, subscriberEmail, url);
            return StrRtnVal;
        }

        public string GetCheckList(string subscriberEmail)
        {
              string StrRtnVal = "";
            string url = "";
            url = MailChimp_Url + "{1}/members/{2}";
            StrRtnVal = CheckList(subscriberEmail, MailChimp_ListId, url, MailChimp_Api_Key);
            return StrRtnVal;
        }

        private static string UnSubscribeListMember(string dataCenter, string apiKey, string listId, string subscriberEmail, string url)
        {
            var sampleListMember = JsonConvert.SerializeObject(
                    new
                    {
                        email_address = subscriberEmail,
                        status = "unsubscribed"
                       // status = "DELETE"
                    });

            var hashedEmailAddress = string.IsNullOrEmpty(subscriberEmail) ? "" : CalculateMD5Hash(subscriberEmail.ToLower());
            //  var uri = string.Format("https://{0}.api.mailchimp.com/3.0/lists/{1}/members/{2}", dataCenter, listId, hashedEmailAddress);
            //var uri = string.Format("https://us12.api.mailchimp.com/3.0/lists/{1}/members/{2}", dataCenter, listId, hashedEmailAddress);

            var uri = string.Format(url, dataCenter, listId, hashedEmailAddress);
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("Accept", "application/json");
                    webClient.Headers.Add("Authorization", "apikey " + apiKey);
                    
                   // return webClient.UploadString(uri, "DELETE", sampleListMember);
                    return webClient.UploadString(uri, "PUT", sampleListMember);
                }
            }
            catch (WebException we)
            {
                using (var sr = new StreamReader(we.Response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private static string ResubscribeListMember(string dataCenter, string apiKey, string listId, string subscriberEmail, string firstname, string lastname, string url, string addr1, string city, string state, string zip, string country)
        {
            var sampleListMember = JsonConvert.SerializeObject(
               new
               {
                   email_address = subscriberEmail,
                   merge_fields =
                   new
                   {
                       FNAME = firstname,
                       LNAME = lastname,
                       ADDRESS = new
                       {
                           addr1 = addr1,
                           city = city,
                           state = state,
                           zip = zip,
                           country = country
                       },

                   },

                   status = "subscribed"
               });

            var hashedEmailAddress = string.IsNullOrEmpty(subscriberEmail) ? "" : CalculateMD5Hash(subscriberEmail.ToLower());
            //  var uri = string.Format("https://{0}.api.mailchimp.com/3.0/lists/{1}/members/{2}", dataCenter, listId, hashedEmailAddress);
            //var uri = string.Format("https://us12.api.mailchimp.com/3.0/lists/{1}/members/{2}", dataCenter, listId, hashedEmailAddress);

            var uri = string.Format(url, dataCenter, listId, hashedEmailAddress);
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("Accept", "application/json");
                    webClient.Headers.Add("Authorization", "apikey " + apiKey);

                    return webClient.UploadString(uri, "PUT", sampleListMember);
                }
            }
            catch (WebException we)
            {
                using (var sr = new StreamReader(we.Response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
        }
         private static string AddOrUpdateListMember(string dataCenter, string apiKey, string listId, string subscriberEmail, string firstname, string lastname, string url, string addr1, string city, string state, string zip, string country)
        
        {
            var sampleListMember = JsonConvert.SerializeObject(
                new
                {
                    email_address = subscriberEmail,
                    merge_fields =
                    new
                    {
                        FNAME = firstname,
                        LNAME = lastname,
                        ADDRESS = new
                        {
                            addr1 = addr1,
                            city = city,
                            state = state,
                            zip = zip,
                            country = country

                            //addr1 = "",
                            //city = city,
                            //state = state,
                            //zip = zip,
                            //country = country
                        },
                   
                    },
               
                   status_if_new = "subscribed"
                });

            var hashedEmailAddress = string.IsNullOrEmpty(subscriberEmail) ? "" : CalculateMD5Hash(subscriberEmail.ToLower());
          //  var uri = string.Format("https://{0}.api.mailchimp.com/3.0/lists/{1}/members/{2}", dataCenter, listId, hashedEmailAddress);
            //var uri = string.Format("https://us12.api.mailchimp.com/3.0/lists/{1}/members/{2}", dataCenter, listId, hashedEmailAddress);
           
            var uri = string.Format(url, dataCenter, listId, hashedEmailAddress);
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("Accept", "application/json");
                    webClient.Headers.Add("Authorization", "apikey " + apiKey);

                    return webClient.UploadString(uri, "PUT", sampleListMember);
                }
            }
            catch (WebException we)
            {
                using (var sr = new StreamReader(we.Response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private static string AddOrUpdateListMemberDeal(string dataCenter, string apiKey, string listId, string subscriberEmail, string firstname, string lastname, string url, string addr1, string city, string state, string zip, string country)
        {
            var sampleListMember = JsonConvert.SerializeObject(
               new
               {
                   email_address = subscriberEmail,
                   merge_fields =
                   new
                   {
                       FNAME = firstname,
                       LNAME = lastname
                   },

                   status_if_new = "subscribed"
               });

            var hashedEmailAddress = string.IsNullOrEmpty(subscriberEmail) ? "" : CalculateMD5Hash(subscriberEmail.ToLower());
            //  var uri = string.Format("https://{0}.api.mailchimp.com/3.0/lists/{1}/members/{2}", dataCenter, listId, hashedEmailAddress);
            //var uri = string.Format("https://us12.api.mailchimp.com/3.0/lists/{1}/members/{2}", dataCenter, listId, hashedEmailAddress);

            var uri = string.Format(url, dataCenter, listId, hashedEmailAddress);
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("Accept", "application/json");
                    webClient.Headers.Add("Authorization", "apikey " + apiKey);

                    return webClient.UploadString(uri, "PUT", sampleListMember);
                }
            }
            catch (WebException we)
            {
                using (var sr = new StreamReader(we.Response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
        }


        public  string CheckList(string subscriberEmail, string listId, string url, string apiKey)
        {

            var sampleListMember = JsonConvert.SerializeObject(
      new
      {
          email_address = subscriberEmail
      });




            var hashedEmailAddress = string.IsNullOrEmpty(subscriberEmail) ? "" : CalculateMD5Hash(subscriberEmail.ToLower());
            var uri = string.Format(url, "", listId, hashedEmailAddress);
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("Accept", "application/json");
                    webClient.Headers.Add("Authorization", "apikey " + apiKey);

                    return webClient.UploadString(uri, "PUT", sampleListMember);
                }
            }
            catch (WebException we)
            {
                using (var sr = new StreamReader(we.Response.GetResponseStream()))
                {
                    return "";
                }
            }
        }

        private static string CalculateMD5Hash(string input)
        {
            // Step 1, calculate MD5 hash from input.
            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // Step 2, convert byte array to hex string.
            var sb = new StringBuilder();
            foreach (var @byte in hash)
            {
                sb.Append(@byte.ToString("X2"));
            }
            return sb.ToString();
        }


        
    }
}
