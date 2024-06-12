using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Net;
using System.Xml;
using Newtonsoft.Json;
using System.Text;
using TradingBell.WebCat.Helpers;
using System.Net.Security;


namespace TradingBell.WebCat.CommonServices
{
    public class ActiveCampaignService
    {
        public DataSet responseData=null;
        public string AC_Url = System.Configuration.ConfigurationManager.AppSettings["AC_Url"].ToString();
        public string AC_Api_key = System.Configuration.ConfigurationManager.AppSettings["AC_Api_Key"].ToString();
        public int AC_List_ID = Convert.ToInt32( System.Configuration.ConfigurationManager.AppSettings["AC_List_Id"].ToString());
        public int AC_List_From_ID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AC_List_From_id"].ToString());
        ErrorHandler objErrorHandler = new ErrorHandler();
        public struct Contact_Info
        {
            public int id;
            public string email;
            public string first_name;
            public string last_name;
            public int status;
            public int ListId;
            public int list_FormId;
        }
        public DataSet SetContact_Subscribe_active(int id, int Status)
        {
            Contact_Info coninfo = new Contact_Info();
            DataSet RtnDs = null;
            coninfo = GetContact_IdInfo(id, AC_List_ID);
            if (coninfo.id > 0)
            {
                coninfo.status = Status;
                coninfo.ListId = AC_List_ID;
                coninfo.list_FormId = 0;
                RtnDs = SetContact_Edit(coninfo);
            }                       
            return RtnDs;

        }

        public DataSet SetContact_MailChimp_Subscribe(string mail_id)
        {
            DataSet RtnDs = null;
            try
            {
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());   
            }
            return RtnDs;
        }

        public DataSet SetContact_Subscribe(string mail_id, string first_name, string last_name, int Status, bool isConformationReq)
        {   
            
            DataSet RtnDs = null;
            try
            {
                Contact_Info coninfo = new Contact_Info();
             
                coninfo = GetContact_IdInfo(mail_id, AC_List_ID);
                if (coninfo.id > 0)
                {
                    //objErrorHandler.CreateLog("inside active cap edit subscriber");  
                    coninfo.first_name = first_name;
                    coninfo.last_name = last_name;
                    coninfo.status = Status;
                    coninfo.ListId = AC_List_ID;
                    coninfo.list_FormId = 0;
                    RtnDs = SetContact_Edit(coninfo);
                }
                else
                {
                    coninfo.id = 0;
                    coninfo.email = mail_id;
                    coninfo.first_name = first_name;
                    coninfo.last_name = last_name;
                    coninfo.status = Status;
                    coninfo.ListId = AC_List_ID;
                    if (isConformationReq == true) //through get deal
                        coninfo.list_FormId = AC_List_From_ID; // for subscribe conformation
                    else
                        coninfo.list_FormId = 0;
                   // objErrorHandler.CreateLog("inside active cap add subscriber");  
                    RtnDs = SetContact_Add(coninfo);
                }
            }
            catch (Exception ex)
            {

                objErrorHandler.CreateLog(ex.ToString());   
            }
            return RtnDs;
        }
        public string GetContact_Exists(string mail_id)
        {
            Contact_Info coninfo = new Contact_Info();
            string Rtnstr = "";
            coninfo = GetContact_IdInfo(mail_id, AC_List_ID);
            if (coninfo.id > 0)
            {               
                Rtnstr=mail_id + " is already subscribed to Wagner Updates";             
            }
            return Rtnstr;
        }
        //public DataSet SetContact_Subscribe(string Oldmail_id, string Newmail_id, string first_name, string last_name, int Status, bool isConformationReq)
        //{
        //    Contact_Info coninfo = new Contact_Info();
        //    DataSet RtnDs = null;
        //    coninfo = GetContact_IdInfo(Oldmail_id, AC_List_ID);
        //    if (coninfo.id > 0)
        //    {
        //        coninfo.email = Newmail_id;
        //        coninfo.first_name = first_name;
        //        coninfo.last_name = last_name;
        //        coninfo.status = Status;
        //        coninfo.ListId = AC_List_ID;
        //        coninfo.list_FormId = AC_List_From_ID;
        //        RtnDs = SetContact_Edit(coninfo);
        //    }
        //    else
        //    {
        //        coninfo.id = 0;
        //        coninfo.email = Newmail_id;
        //        coninfo.first_name = first_name;
        //        coninfo.last_name = last_name;
        //        coninfo.status = Status;
        //        coninfo.ListId = AC_List_ID;
        //        if (isConformationReq == true) //through get deal
        //            coninfo.list_FormId = AC_List_From_ID; // for subscribe conformation
        //        else
        //            coninfo.list_FormId = 0;

        //        RtnDs = SetContact_Add(coninfo);
        //    }
        //    return RtnDs;
        //}
        public DataSet SetContact_Add(Contact_Info contactinfo)
        {


            string Param="&email=" +contactinfo.email.ToString()+"&first_name=" + contactinfo.first_name +"&last_name="+ contactinfo.last_name;
            Param += "&p[" + contactinfo.ListId.ToString() + "]=" + contactinfo.ListId.ToString();
            if (contactinfo.status!=0 )
                Param += "&status[" + contactinfo.ListId.ToString() + "]=" + contactinfo.status.ToString();
            
            if (contactinfo.list_FormId>0)
                Param += "&form=" + contactinfo.list_FormId.ToString();
           // objErrorHandler.CreateLog(Param);   
            return Exec_Request("contact_add", Param, "xml");
        }


        public DataSet SetContact_Edit(Contact_Info contactinfo)
        {
            string Param = "id=" + contactinfo.id.ToString() +"&email=" + contactinfo.email.ToString() + "&first_name=" + contactinfo.first_name + "&last_name=" + contactinfo.last_name;
            Param += "&p[" + contactinfo.ListId.ToString() + "]=" + contactinfo.ListId.ToString() + "&status[" + contactinfo.ListId.ToString() + "]=" + contactinfo.status.ToString();
            return Exec_Request("contact_edit", Param,"xml");
            
        }
        public Contact_Info GetContact_IdInfo(int Id,int list_id)
        {
            Contact_Info coninfo=new Contact_Info();
            string Param = "ids=" + Id.ToString() + "&ilters[listid]=" + list_id.ToString();
            Exec_Request("contact_list", Param,"Xml");
            coninfo = CopyDataSetToContact_info(responseData, coninfo);            
            return coninfo;
           

        }
   
        public Contact_Info GetContact_IdInfo(string email_id, int list_id)
        {
            Contact_Info coninfo = new Contact_Info();
            string Param = "filters[email]=" + email_id + "&filters[listid]=" + list_id.ToString();
            Exec_Request("contact_list", Param, "Xml");
            coninfo = CopyDataSetToContact_info(responseData, coninfo);
            return coninfo;


        }
        public Contact_Info CopyDataSetToContact_info(DataSet ds, Contact_Info CI)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt16(ds.Tables[0].Rows[0]["result_code"].ToString()) > 0 && ds.Tables["row"] != null && ds.Tables["row"].Rows.Count > 0)
                {
                    CI.id = Convert.ToInt32(ds.Tables["row"].Rows[0]["id"].ToString());
                    CI.email = ds.Tables["row"].Rows[0]["email"].ToString();
                    CI.first_name = ds.Tables["row"].Rows[0]["first_name"].ToString();
                    CI.last_name = ds.Tables["row"].Rows[0]["last_name"].ToString();
                    CI.status = Convert.ToInt32(ds.Tables["row"].Rows[0]["status"].ToString());
                    CI.ListId = Convert.ToInt32(ds.Tables["row"].Rows[0]["listid"].ToString());
                }
            }
            return CI;

        }
        public DataSet  Exec_Request( string action ,string Params,string output)
        {



            string resultStr = "";

            try
            {
               

                string strURL = AC_Url.ToString() + "api_key=" + AC_Api_key + "&api_action=" + action + "&api_output=" + output;
               // objErrorHandler.CreateLog(strURL);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strURL);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(Params);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = requestBytes.Length;
                //req.Credentials = new NetworkCredential("anonymous", "janeDoe@contoso.com");
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.Default);
                if (sr.ToString() != "")
                    resultStr = sr.ReadToEnd();

                if (resultStr != "")
                {
                    if (output == "json")
                    {
                        XmlDocument xd = new XmlDocument();
                        resultStr = "{ \"rootNode\": {" + resultStr.Trim().TrimStart('{').TrimEnd('}') + "} }";
                        xd = (XmlDocument)JsonConvert.DeserializeXmlNode(resultStr);
                        responseData = new DataSet();
                        responseData.ReadXml(new XmlNodeReader(xd));
                    }
                    else
                    {                        
                            responseData = new DataSet();
                            StringReader theReader = new StringReader(resultStr);
                            responseData.ReadXml(theReader);                        
                    }
                }
                else
                    responseData = null;
            }
            catch (Exception ex)
            {
                responseData = null;
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }

            return responseData;
        }
    }
}
