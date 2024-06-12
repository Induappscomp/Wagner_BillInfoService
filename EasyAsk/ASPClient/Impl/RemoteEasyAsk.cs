//namespace TradingBell.WebCat.EasyAsk.Impl OLD
//{
//    using TradingBell.WebCat.EasyAsk;
//    using System;
//    using System.Web;

//    public class RemoteEasyAsk : IRemoteEasyAsk
//    {
//        private static readonly string ADVISOR_URI = "EasyAsk/apps/Advisor.jsp";
//        private static readonly string HTTP_PROTOCOL = "http";
//        private static readonly string HTTPS_PROTOCOL = "https";
//        private int m_nPort = -1;
//        private IOptions m_options = null;
//        private string m_sHostName = "";
//        private string m_sProtocol = HTTP_PROTOCOL;
//        private static readonly string ROOT_URI = ADVISOR_URI;

//        public RemoteEasyAsk(string sHostName, int nPort, string dictionary)
//        {
//            this.m_sHostName = sHostName;
//            this.m_nPort = nPort;
//            this.m_options = new Options(dictionary);
//        }

//        private string addNonNullVal(string val)
//        {
//            return ((val != null) ? val : "");
//        }

//        private string addParam(string name, string val)
//        {
//            return (((val != null) && (0 < val.Length)) ? ("&" + name + "=" + val) : "");
//        }

//        private string addTrueParam(string name, bool val)
//        {
//            return (val ? string.Concat(new object[] { "&", name, "=", val }) : "");
//        }

//        private string formBaseURL()
//        {
//            return string.Concat(new object[] { this.m_sProtocol, "://", this.m_sHostName, ":", this.m_nPort, "/", ROOT_URI, "?disp=xml&oneshot=1" });
//        }

//        private string formURL()
//        {
//            return string.Concat(new object[] { this.formBaseURL(), "&dct=", this.m_options.getDictionary(), "&indexed=1&ResultsPerPage=", this.m_options.getResultsPerPage(), this.addParam("defsortcols", this.m_options.getSortOrder()), this.addTrueParam("subcategories", this.m_options.getSubCategories()), this.addTrueParam("rootprods", this.m_options.getToplevelProducts()), this.addTrueParam("navigatehierarchy", this.m_options.getNavigateHierarchy()), this.addTrueParam("returnskus", this.m_options.getReturnSKUs()), this.addParam("defarrangeby", this.m_options.getGrouping()), this.addNonNullVal(this.m_options.getCallOutParam()) });
//        }

//        public IOptions getOptions()
//        {
//            return this.m_options;
//        }

//        public void setOptions(IOptions options)
//        {
//            this.m_options = options;
//        }

//        public INavigateResults userAttributeClick(string path, string attr)
//        {
//            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_AttributeSelected&AttribSel=" + attr;
//            RemoteResults results = new RemoteResults();
//            results.load(url);
//            return results;
//        }
//        public INavigateResults userAttributeClick_Brand(string path, string attr)
//        {
//            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_AttributeSelected&AttribSel=" + HttpUtility.UrlEncode(attr);
//            RemoteResults results = new RemoteResults();
//            results.load(url);
//            return results;
//        }

//        public INavigateResults userBreadCrumbClick(string path)
//        {
//            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_BreadcrumbSelect";
//            RemoteResults results = new RemoteResults();
//            results.load(url);
//            return results;
//        }

//        public INavigateResults userBreadCrumbClick1(String path)
//        {
//            String url = formURL() + "&RequestAction=advisor&CatPath=" + path + "&RequestData=CA_BreadcrumbSelect";
//            RemoteResults res = new RemoteResults();
//            res.load(url);
//            return res;
//        }

//        public INavigateResults userCategoryClick(string path, string cat)
//        {
//            string str = (((path != null) && (0 < path.Length)) ? (path + "/") : "") + cat;
//            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(str) + "&RequestData=CA_CategoryExpand";
//            RemoteResults results = new RemoteResults();
//            results.load(url);
//            return results;
//        }
//        public INavigateResults userCategoryClick(string path)
//        {
//            // string str = (((path != null) && (0 < path.Length)) ? (path + "") : "") + cat;
//            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_CategoryExpand";
//            RemoteResults results = new RemoteResults();
//            results.load(url);
//            return results;
//        }
//        public INavigateResults userGoToPage(string path, string pageNumber)
//        {
//            string url = this.formURL() + "&RequestAction=navbar&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=page" + pageNumber;
//            RemoteResults results = new RemoteResults();
//            results.load(url);
//            return results;
//        }

//        public INavigateResults userPageOp(string path, string curPage, string pageOp)
//        {
//            string url = this.formURL() + "&RequestAction=navbar&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=" + pageOp;
//            if ((curPage != null) && (0 < curPage.Length))
//            {
//                url = url + "&currentpage=" + curPage;
//            }
//            RemoteResults results = new RemoteResults();
//            results.load(url);
//            return results;
//        }

//        public INavigateResults userSearch(string path, string question)
//        {
//            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_Search&q=" + HttpUtility.UrlEncode(question);
//            RemoteResults results = new RemoteResults();
//            results.load(url);
//            return results;
//        }
//    }
//} OLD



namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;
    using System.Web;
    using System.Net;

    using System.Xml;
    using System.Data;  
    using Newtonsoft.Json;
    using System.Runtime.Serialization;
    using TradingBell.WebCat.Helpers;

    //using TradingBell.WebCat.Helpers;
    using System.Text;
    using System.Diagnostics;
    using System.IO;
    public class RemoteEasyAsk : IRemoteEasyAsk
    {
        private static readonly string ADVISOR_URI = "EasyAsk/apps/Advisor.jsp";
        private static readonly string HTTP_PROTOCOL = "http";
        private static readonly string HTTPS_PROTOCOL = "https";
        private int m_nPort = -1;
        private IOptions m_options = null;
        private string m_sHostName = "";
        private string m_sProtocol = HTTP_PROTOCOL;
        private static readonly string ROOT_URI = ADVISOR_URI;

        public RemoteEasyAsk(string sHostName, int nPort, string dictionary)
        {
            this.m_sHostName = sHostName;
            this.m_nPort = nPort;
            this.m_options = new Options(dictionary);
        }

        private string addNonNullVal(string val)
        {
            return ((val != null) ? val : "");
        }

        private string addParam(string name, string val)
        {
            return (((val != null) && (0 < val.Length)) ? ("&" + name + "=" + val) : "");
        }

        private string addTrueParam(string name, bool val)
        {
            return (val ? string.Concat(new object[] { "&", name, "=", val }) : "");
        }

        private string formBaseURL()
        {
            return string.Concat(new object[] { this.m_sProtocol, "://", this.m_sHostName, ":", this.m_nPort, "/", ROOT_URI, "?disp=json&oneshot=1" });
        }

        private string formURL()
        {
            return string.Concat(new object[] { this.formBaseURL(), "&dct=", this.m_options.getDictionary(), "&indexed=1&ResultsPerPage=", this.m_options.getResultsPerPage(), this.addParam("defsortcols", this.m_options.getSortOrder()), this.addTrueParam("subcategories", this.m_options.getSubCategories()), this.addTrueParam("rootprods", this.m_options.getToplevelProducts()), this.addTrueParam("navigatehierarchy", this.m_options.getNavigateHierarchy()), this.addTrueParam("returnskus", this.m_options.getReturnSKUs()), this.addParam("defarrangeby", this.m_options.getGrouping()), this.addNonNullVal(this.m_options.getCallOutParam()) });
        }

        public IOptions getOptions()
        {
            return this.m_options;
        }

        public void setOptions(IOptions options)
        {
            this.m_options = options;
        }

        public INavigateResults userAttributeClick(string path, string attr)
        {
            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_AttributeSelected&AttribSel=" + HttpUtility.UrlEncode(attr);
            //RemoteResults results = new RemoteResults();
            //results.load(url);
            //return results;
            return (urlPostjson(url));
        }
        public INavigateResults userAttributeClick_Brand(string path, string attr)
        {
            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_AttributeSelected&AttribSel=" + HttpUtility.UrlEncode(attr);
            //RemoteResults results = new RemoteResults();
            //results.load(url);
            //return results;
            return (urlPostjson(url));
        }

        public INavigateResults userBreadCrumbClick(string path)
        {
            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_BreadcrumbSelect";
            //RemoteResults results = new RemoteResults();
            //results.load(url);
            //return results;
           // if (path=="AllProducts////WESAUSTRALASIA")
            //if (HttpContext.Current.Session["JSON"]=="JSON")
            //ErrorHandler objErrorhandler = new ErrorHandler();
            //objErrorhandler.CreateLog(url);

                return (urlPostjson(url));
            //else
            //    return (urlPost(url));

            //return (urlPost(url));
        }
        
        //public INavigateResults userBreadCrumbClick1(String path)
        //{
        //    String url = formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_BreadcrumbSelect";
        //    //RemoteResults res = new RemoteResults();
        //    //res.load(url);
        //    //return res;
        //    return (urlPost(url));
        //}

        public INavigateResults userCategoryClick(string path, string cat)
        {
            string str = (((path != null) && (0 < path.Length)) ? (path + "/") : "") + cat;
            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(str) + "&RequestData=CA_CategoryExpand";
            //RemoteResults results = new RemoteResults();
            //results.load(url);
            //return results;
            return (urlPostjson(url));
        }
        public INavigateResults userCategoryClick(string path)
        {
            // string str = (((path != null) && (0 < path.Length)) ? (path + "") : "") + cat;
            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_CategoryExpand";
            //RemoteResults results = new RemoteResults();
            //results.load(url);
            //return results;
            return (urlPostjson(url));
        }
        public INavigateResults userGoToPage(string path, string pageNumber)
        {
            string url = this.formURL() + "&RequestAction=navbar&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=page" + pageNumber;
            //RemoteResults results = new RemoteResults();
            //results.load(url);
            //return results;
            return (urlPostjson(url));
        }

        public INavigateResults userPageOp(string path, string curPage, string pageOp)
        {
            string url = this.formURL() + "&RequestAction=navbar&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=" + pageOp;
            if ((curPage != null) && (0 < curPage.Length))
            {
                url = url + "&currentpage=" + curPage;
            }
            //RemoteResults results = new RemoteResults();
            //results.load(url);
            //return results;
            return (urlPostjson(url));
        }

        public INavigateResults userSearch(string path, string question)
        {
           // string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_Search&q=" + HttpUtility.UrlEncode(question);
            string url = this.formURL() + "&RequestAction=advisor&CatPath=" + HttpUtility.UrlEncode(path) + "&RequestData=CA_Search&q=" + HttpUtility.UrlEncode(question);
            //ErrorHandler objErrorhandler = new ErrorHandler();
            //objErrorhandler.CreateLog(url);
            //RemoteResults results = new RemoteResults();
            //results.load(url);
            //return results;
            return (urlPostjson(url));

          }

        public INavigateResults urlPost(String url)
        {

            RemoteResults res = new RemoteResults();

            DataSet dsAdvisor = new DataSet();

            url = url.Replace("disp=json", "disp=xml");
            WebRequest req = WebRequest.Create(url);

            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            WebResponse resp = req.GetResponse();
            res.load(resp.GetResponseStream());

            //-----------------------------------
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.Method = "POST";
            //request.ContentType = "application/json; charset=utf-8";
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(data.GetType());
            //MemoryStream ms = new MemoryStream();
            //ser.WriteObject(ms, data);
            //String json = Encoding.UTF8.GetString(ms.ToArray());
            //StreamWriter writer = new StreamWriter(request.GetRequestStream());
            //writer.Write(json);
            //writer.Close();
            //-----------------------------------

            //var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //httpWebRequest.ContentType = "application/json; charset=utf-8";
            //httpWebRequest.Method = "POST";

            //using (var streamWriter = new System.IO.StreamWriter(httpWebRequest.GetRequestStream()))
            //{
            //    string json = "{\"x\":\"true\"}";

            //    streamWriter.Write(json);
            //    streamWriter.Flush();
            //}

            //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //string restr = "";
            //using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            //{
            //    restr = streamReader.ReadToEnd();
            //    restr = "{ \"rootNode\": {" + restr.Trim().TrimStart('{').TrimEnd('}') + "} }";

            //}
            //string xml = JsonConvert.DeserializeXNode(restr).ToString();
            //res.load(new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml)));
            //-----------------------------------
            //System.Net.WebClient myWebClient = new System.Net.WebClient();


            //byte[] data = myWebClient.DownloadData(HttpUtility.UrlDecode(url));

            //string resultStr = System.Text.Encoding.UTF8.GetString(data);

            //if (resultStr != "")
            //{

            //    XmlDocument xd = new XmlDocument();
            //    resultStr = "{ \"rootNode\": {" + resultStr.Trim().TrimStart('{').TrimEnd('}') + "} }";
            //    xd = (XmlDocument)JsonConvert.DeserializeXmlNode(resultStr);

            //    dsAdvisor.ReadXml(new XmlNodeReader(xd));
            //}

            //res.SetDBAdvisor(dsAdvisor);
            //string xml = JsonConvert.DeserializeXNode(resultStr).ToString();
            //res.load(new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml)));

            //-----------------------------------
            return res;
            //}
            //catch(Exception ex)
            //{
            //    ErrorHandler objErrorHandler = new ErrorHandler();
            //    objErrorHandler.CreateLog(ex.ToString());
            //    RemoteResults res = new RemoteResults();
            //    return res;
            //}

        }
        public INavigateResults urlPostjson(String url)
        {

            Stopwatch swurlpostjson = new Stopwatch();
            swurlpostjson.Start();
            ErrorHandler objErrorhandler = new ErrorHandler();

            RemoteResults res = new RemoteResults();
            DataSet dsAdvisor = new DataSet();

            //WebRequest req = WebRequest.Create(url);

            //req.ContentType = "application/x-www-form-urlencoded";
            //req.Method = "POST";
            //WebResponse resp = req.GetResponse();
            //res.load(resp.GetResponseStream());

            //-----------------------------------
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.Method = "POST";
            //request.ContentType = "application/json; charset=utf-8";
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(data.GetType());
            //MemoryStream ms = new MemoryStream();
            //ser.WriteObject(ms, data);
            //String json = Encoding.UTF8.GetString(ms.ToArray());
            //StreamWriter writer = new StreamWriter(request.GetRequestStream());
            //writer.Write(json);
            //writer.Close();
            //-----------------------------------

            Stopwatch swhttpWebRequest = new Stopwatch();
            swhttpWebRequest.Start();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";

            swhttpWebRequest.Stop();
            //objErrorhandler.CreateLog("swhttpWebRequest Total time " + swhttpWebRequest.Elapsed.TotalSeconds.ToString());
            //ErrorHandler objerr = new ErrorHandler();
            //objerr.CreateLog(url);
            //using (var streamWriter = new System.IO.StreamWriter(httpWebRequest.GetRequestStream()))
            //{
            //    string json = "{\"x\":\"true\"}";

            //    streamWriter.Write(json);
            //    streamWriter.Flush();
            //}

            //current start
            //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //string restr = "";
            //using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            //{
            //    XmlDocument xd = new XmlDocument();
            //    restr = streamReader.ReadToEnd();
            //    restr = "{ \"rootNode\": {" + restr.Trim().TrimStart('{').TrimEnd('}') + "} }";
            //    xd = (XmlDocument)JsonConvert.DeserializeXmlNode(restr);
            //    dsAdvisor.ReadXml(new XmlNodeReader(xd));
            //}
            //current end


            Stopwatch swhttpResponse = new Stopwatch();
            swhttpResponse.Start();
           
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string restr = "";
            string restr1 = "";
            StringBuilder sb = new StringBuilder();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                XmlDocument xd = new XmlDocument();


                swhttpResponse.Stop();
                //objErrorhandler.CreateLog("swhttpResponse Total time " + swhttpResponse.Elapsed.TotalSeconds.ToString());


                Stopwatch stopwatch = new Stopwatch();
                Stopwatch st1 = new Stopwatch();
                stopwatch.Start();

                //while ((restr = streamReader.ReadLine()) != null)
                //{
                //    sb.Append(restr);
                //}
                //restr = sb.ToString();

               // restr = streamReader.ReadLine();



                if ((restr = streamReader.ReadLine()) != null)
                {
                }

                //using (StreamReader subds = new System.IO.StreamReader(httpResponse.GetResponseStream()))
                //{
                //    using (JsonReader reader = new JsonTextReader(subds))
                //    {
                //        JsonSerializer serializer = new JsonSerializer();
                //        dsAdvisor = (DataSet)serializer.Deserialize(subds, typeof(DataSet));
                //        reader.Close();
                //    }
                //    subds.Dispose();
                //}
           
            

             //   while ((restr = streamReader.ReadLine()) != null)
             //   {
                   // if (restr == string.Empty)
                   //     break;
                 //   sb.Append(restr);
              //  }
              //  restr = sb.ToString();
              
             
                stopwatch.Stop();
               // objErrorhandler.CreateLog("restr Total time " + stopwatch.Elapsed.TotalSeconds.ToString());

                restr = "{ \"rootNode\": {" + restr.Trim().TrimStart('{').TrimEnd('}') + "} }";
                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(restr);
                dsAdvisor.ReadXml(new XmlNodeReader(xd));


                //st1.Start();
                //restr1 = sb.Append(streamReader.ReadToEnd()).ToString();
                //st1.Stop();
                //objErrorhandler.CreateLog("restr1 Total time " + st1.Elapsed.TotalSeconds.ToString());


                //sb.Append(streamReader.ReadToEnd());
                //restr = "{ \"rootNode\": {" + sb.ToString().Trim().TrimStart('{').TrimEnd('}') + "} }";
                //xd = (XmlDocument)JsonConvert.DeserializeXmlNode(restr);
                //dsAdvisor.ReadXml(new XmlNodeReader(xd));




              

               // st1.Start();
              // restr1 = streamReader.ReadToEnd();
              // st1.Stop();
              // objErrorhandler.CreateLog("restr1 Total time " + st1.Elapsed.TotalSeconds.ToString());

             
            }

            //using (StreamReader sr = new StreamReader(bs))
            //{
            //    string s;
            //    while ((s = sr.ReadLine()) != null)
            //    {
            //        //we're just testing read speeds    
            //    }
            //}

           // HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();

           // WebHeaderCollection header = response.Headers;

           //// var encoding = ASCIIEncoding.ASCII;
           // using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
           // {
           //     XmlDocument xd = new XmlDocument();
           //     string responseText = reader.ReadToEnd();
           //     responseText = "{ \"rootNode\": {" + responseText.Trim().TrimStart('{').TrimEnd('}') + "} }";
           //     xd = (XmlDocument)JsonConvert.DeserializeXmlNode(responseText);
           //     dsAdvisor.ReadXml(new XmlNodeReader(xd));
           // }


            Stopwatch swloopdsadvisor = new Stopwatch();
            swloopdsadvisor.Start();
            if (dsAdvisor != null && dsAdvisor.Tables.Count > 0 && dsAdvisor.Tables["items"] != null && dsAdvisor.Tables["items"].Rows.Count > 0 && dsAdvisor.Tables["datadescription"] != null && dsAdvisor.Tables["datadescription"].Rows.Count > 0)
            {
                foreach (DataRow dr in dsAdvisor.Tables["datadescription"].Rows)
                {
                    if (dsAdvisor.Tables["items"].Columns[dr["tagname"].ToString()] == null)
                    {
                        DataColumn dc = new DataColumn(dr["tagname"].ToString(), typeof(string));
                        dc.DefaultValue = "";
                        dsAdvisor.Tables["items"].Columns.Add(dc);
                    }
                }

            }
            swloopdsadvisor.Stop();
            //objErrorhandler.CreateLog("swloopdsadvisor Total time " + swloopdsadvisor.Elapsed.TotalSeconds.ToString());


            Stopwatch SetDBAdvisor = new Stopwatch();
            SetDBAdvisor.Start();

            res.SetDBAdvisor(dsAdvisor);

            SetDBAdvisor.Stop();
            //objErrorhandler.CreateLog("SetDBAdvisor Total time " + swloopdsadvisor.Elapsed.TotalSeconds.ToString());
            //-----------------------------------------------------


            //System.Net.WebClient myWebClient = new System.Net.WebClient();

            //url = HttpUtility.UrlDecode(url);
            //var resultStr = myWebClient.DownloadString(url);


            //if (resultStr != "")
            //{

            //    XmlDocument xd = new XmlDocument();
            //    resultStr = "{ \"rootNode\": {" + resultStr.Trim().TrimStart('{').TrimEnd('}') + "} }";
            //    xd = (XmlDocument)JsonConvert.DeserializeXmlNode(resultStr);
            //    //System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer();

            //    dsAdvisor.ReadXml(new XmlNodeReader(xd));

            //}
            //if (dsAdvisor != null && dsAdvisor.Tables.Count > 0 && dsAdvisor.Tables["items"] != null && dsAdvisor.Tables["items"].Rows.Count > 0 && dsAdvisor.Tables["datadescription"] != null && dsAdvisor.Tables["datadescription"].Rows.Count > 0)
            //{
            //    foreach (DataRow dr in dsAdvisor.Tables["datadescription"].Rows)
            //    {
            //        if (dsAdvisor.Tables["items"].Columns[dr["tagname"].ToString()] == null)
            //        {
            //            DataColumn dc = new DataColumn(dr["tagname"].ToString(), typeof(string));
            //            dc.DefaultValue = "";
            //            dsAdvisor.Tables["items"].Columns.Add(dc);
            //        }
            //    }

            //}
            //res.SetDBAdvisor(dsAdvisor);

            swurlpostjson.Stop();
            //objErrorhandler.CreateLog("urlPostjson Total time " + swurlpostjson.Elapsed.TotalSeconds.ToString());



            return res;
        }
    }
}

