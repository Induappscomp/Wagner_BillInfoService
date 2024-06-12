using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using System.Text;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.CommonServices ;
using System.Configuration;
using System.Globalization;
using System.Threading; 
//using System.Diagnostics;

namespace TradingBell.WebCat.EasyAsk
{

    /*********************************** J TECH CODE ***********************************/
    public class EasyAsk_WAGNER
    {

        /*********************************** DECLARATION ***********************************/
        #region "Common Declaration"         
        HelperServices objhelper = new HelperServices();
        HelperDB objhelperDb = new HelperDB();
        Security objsecurity = new Security();
         
        EasyAskServices objEAservice = new EasyAskServices();
        CategoryServices objCategoryServices = new CategoryServices();
        //ConnectionDB objConnectionDB = new ConnectionDB();
        ErrorHandler objErrorhandler = new ErrorHandler();
        //Stopwatch stopwatch = new Stopwatch(); 

        string StrSql;
        const String COOKIE_NAME = "EasyAsk-eCommerce-Demo";
        String m_rpp = "10";
        String m_sort = string.Empty;
        String m_grp = string.Empty;
        String tmpstr = string.Empty;
        int j;
        public string EasyAsk_URL = System.Configuration.ConfigurationManager.AppSettings["EasyAsk_URL"].ToString();
        public int EasyAsk_Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EasyAsk_Port"]);
        public string EasyAsk_WesCatBrandDictionary = System.Configuration.ConfigurationManager.AppSettings["EasyAsk_WesCatBrandDictionary"].ToString();
        public string EasyAsk_WebCatDictionary = System.Configuration.ConfigurationManager.AppSettings["EasyAsk_WebCatDictionary"].ToString();
        public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
        public string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();
        public string Dum_Price_Code = System.Configuration.ConfigurationManager.AppSettings["DUM_Price_Code"].ToString();
        string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        public string strxml = HttpContext.Current.Server.MapPath("xml");

        IRemoteEasyAsk getRemote()
        {
            IRemoteEasyAsk ea =Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
            IOptions opts = ea.getOptions();
            opts.setResultsPerPage(m_rpp); // ea_rpp.Value);   // use current settings
            opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
            opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
            opts.setToplevelProducts(true);                   
            return ea;
        }

        /*********************************** DECLARATION ***********************************/
        #endregion

        

        # region   "you have select & BreadCrumb"

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CREATE BREAD CRUMB DETAILS FOR SELECTED PRODUCTS  ***/
        /********************************************************************************/
        public void CreateYouHaveSelectAndBreadCrumb(bool xml)
        {

            try
            {

                CreateYouHaveSelectAndBreadCrumb("", xml);
            }
            catch

            { }
           
        }
        public void CreateYouHaveSelectAndBreadCrumb(string EAPATH,bool xml)
        {

            string[] StrValues = null;
            string EA = string.Empty;
            string temp = string.Empty;
            string breadcrumb = string.Empty;
            string[] tmpsplit;
            string CatId = string.Empty;
            string byp="0";
            DataSet Ds=new DataSet();
            if (xml == true)
            {
                HttpContext.Current.Session["BreadCrumbDS_URL"] = null;
          
            }
            else
            {

                if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                {
                    HttpContext.Current.Session["BreadCrumbDS_URL"] = HttpContext.Current.Session["BreadCrumbDS"];
               
                }
            }
            HttpContext.Current.Session["BreadCrumbDS"] = null;
            DataSet YHSAndBC = new DataSet();
            string TempPath = string.Empty;
            string tsb = string.Empty;
            string tsm = string.Empty;
            string tempstr = string.Empty;
            string scrtext = string.Empty;
            string familyid = string.Empty;
            string Productid = string.Empty;
            bool isFamily = false;
            bool isProduct = false;
            YHSAndBC.Tables.Add("YHSAndBC");
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("EAPath", typeof(string));
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("RemoveEAPath", typeof(string));
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("ActualValue", typeof(string));
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("ItemType", typeof(string));
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("ItemValue", typeof(string));
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("Url", typeof(string));
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("RemoveUrl", typeof(string));
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("TempPath", typeof(string));
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("FamilyName", typeof(string));
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("ProductCode", typeof(string));

            if (EAPATH == string.Empty)
            {
                if (HttpContext.Current.Session["EA"] != null)
                {
                    EA = HttpContext.Current.Session["EA"].ToString();

                }
            }
            else
            {

                EA = EAPATH;
            }
            EA = EA.Replace("AllProducts////UserSearch", "AllProducts////WESAUSTRALASIA////UserSearch");
            if (EA != string.Empty)
            {
                StrValues = EA.Split(new string[] { "////" }, StringSplitOptions.None);
                if (StrValues.Length > 0)
                {
                    for (int i = 2; i < StrValues.Length; i++)
                    {
                        
                        

                        temp = string.Empty ;                            
                        for (int j = 0; j <= i; j++)
                        {
                            if (j == 0)
                            {
                                temp = temp + StrValues[j];
                            }
                            else
                            {
                                temp = temp + "////" + StrValues[j];
                            }
                        }
                        DataRow row = YHSAndBC.Tables["YHSAndBC"].NewRow();
                        row["EAPath"] = temp;
                        row["RemoveEAPath"] = temp.Replace("////" + StrValues[i].ToString(), "");
                        row["ActualValue"] = StrValues[i].ToString();
                        string strvalupp = string.Empty;
                        strvalupp = StrValues[i].ToUpper();

                        if (strvalupp.Contains("ATTRIBSEL"))
                        {
                            tmpsplit = StrValues[i].Split('=');
                            if (tmpsplit.Length >=2)
                            {
                                row["ItemType"] = tmpsplit[1].Trim();
                                TempPath =TempPath + "/"+tmpsplit[1].Trim();
                                if (tmpsplit[2].Contains(":"))
                                {
                                    tmpsplit = tmpsplit[2].Split(':');
                                    row["ItemValue"] = tmpsplit[1].Trim().Replace("'","");

                                }
                                else
                                {
                                    row["ItemValue"] = tmpsplit[2].Trim().Replace("'", "");
                                }

                            }                          
                        }
                        else if (strvalupp.Contains("SEARCH"))
                        {
                            if (strvalupp.Contains("FAMILY"))
                            {
                                tmpsplit = StrValues[i].Split('=');
                                if (tmpsplit.Length >= 2)
                                {
                                        row["ItemType"] = "Family";
                                        TempPath = TempPath + "/" + "Family";
                                        row["ItemValue"] = tmpsplit[2].ToString();
                                        //DataSet tmpds=GetDataSet("Select FAMILY_NAME from TB_FAMILY WHERE FAMILY_ID='" + tmpsplit[2].ToString() + "'");
                                        //if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                                         //   row["FamilyName"] = tmpds.Tables[0].Rows[0]["FAMILY_NAME"];
                                        if (tmpsplit[2].ToString() != "")
                                            row["FamilyName"] = objhelperDb.GetGenericDataDB(tmpsplit[2].ToString(), "GET_FAMILY_NAME", HelperDB.ReturnType.RTString);
                                        
                                    
                                }
                            }
                            else if (strvalupp.Contains("PROD"))
                            {
                                tmpsplit = StrValues[i].Split('=');
                                if (tmpsplit.Length >= 2)
                                {
                                    row["ItemType"] = "Product";
                                    TempPath = TempPath + "/" + "Product";
                                    row["ItemValue"] = tmpsplit[2].ToString();
                                    //DataSet tmpds = GetDataSet("Select isnull(STRING_VALUE,'') as STRING_VALUE from TB_PROD_SPECS Where ATTRIBUTE_ID=1 And  PRODUCT_ID=" + tmpsplit[2].ToString() + "");
                                    //if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                                    //    row["ProductCode"] = tmpds.Tables[0].Rows[0]["STRING_VALUE"];
                                    if (tmpsplit[2].ToString() != string.Empty)
                                        row["ProductCode"] = objhelperDb.GetGenericDataDB(tmpsplit[2].ToString(), "GET_PRODUCT_CODE", HelperDB.ReturnType.RTString);

                                }
                            }
                            else
                            {
                                tmpsplit = StrValues[i].Split('=');
                                if (tmpsplit.Length >= 1)
                                {
                                    row["ItemType"] = tmpsplit[0].Trim();
                                    TempPath = TempPath + "/" + tmpsplit[0].Trim();
                                    row["ItemValue"] = tmpsplit[1].Trim().Replace("'", "");
                                }
                            }
                        }
                        else
                        {                           
                                row["ItemType"] = "Category";
                                TempPath = TempPath + "/" + "Category";
                                row["ItemValue"] = StrValues[i].Trim().Replace("'", "");                         
                        }

                        row["TempPath"] = TempPath;


                        YHSAndBC.Tables["YHSAndBC"].Rows.Add(row);                        
                    }

                }
                isFamily = false;
                isProduct = false;
                if (YHSAndBC.Tables["YHSAndBC"].Rows.Count > 0)
                {
                    if (YHSAndBC.Tables["YHSAndBC"].Rows[0]["ItemType"].ToString().ToLower().Contains("search"))
                    {
                        for (int i = 0; i <= YHSAndBC.Tables["YHSAndBC"].Rows.Count - 1; i++)
                        {
                            using (DataTable td = YHSAndBC.Tables["YHSAndBC"])
                            {
                                string tditemtype = string.Empty;
                                tditemtype = td.Rows[i]["ItemType"].ToString().ToLower();

                                string tditemval = string.Empty;
                                tditemval = td.Rows[i]["Itemvalue"].ToString();
                                if (td.Rows[i]["TempPath"].ToString().ToLower().Contains("search") && i==0)
                                {
                                    scrtext = tditemval;
                                    tempstr = "?srctext=" + HttpUtility.UrlEncode(tditemval);
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "ps.aspx" + tempstr;
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = "home.aspx?";
                                }

                                else if (tditemtype.Contains("search"))
                                {
                                    if (tditemtype == "brand")
                                        tsb = HttpUtility.UrlEncode(tditemval);
                                    if (tditemtype == "model")
                                        tsm = HttpUtility.UrlEncode(tditemval);
                                    if (tditemtype == "family")
                                    {
                                        familyid = HttpUtility.UrlEncode(tditemval);
                                        isFamily = true;
                                    }
                                    //Modified by:Indu
                                    //if (td.Rows[i]["ItemType"].ToString().ToLower() == "product" )

                                    if (tditemtype == "product" && objhelper.CDEC(td.Rows[i]["Itemvalue"]) > 0)
                                    {

                                        Productid = HttpUtility.UrlEncode(tditemval);
                                        isProduct = true;
                                    }
                                    if ((isProduct))
                                    {
                                        if (tditemtype == "product")
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "pd.aspx" + tempstr + "&fid=" + familyid + "&pid=" + Productid + "&searchstr=" + scrtext;
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"];  //"ct.aspx" + tempstr;
                                        }
                                    }
                                    else if ((isFamily))
                                    {
                                        if (tditemtype == "family")
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "fl.aspx" + tempstr + "&fid=" + HttpUtility.UrlEncode(tditemval) + "&searchstr=" + scrtext;
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"];  //"ct.aspx" + tempstr;
                                        }
                                        else
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "fl.aspx" + tempstr + "&fid=" + familyid + "&tsb=" + tsb + "&tsm=" + tsm + "&type=" + tditemval + "&value=" + HttpUtility.UrlEncode(tditemval) + "&bname=" + tsb + "&searchstr=" + scrtext;
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"];  //"ct.aspx" + tempstr;
                                        }
                                    }

                                    else
                                    {
                                        YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "ps.aspx" + tempstr + "&tsb=" + tsb + "&tsm=" + tsm + "&type=" + tditemval + "&value=" + HttpUtility.UrlEncode(tditemval) + "&bname=" + tsb + "&searchstr=" + scrtext;
                                        YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"];  //"ct.aspx" + tempstr;
                                    }
                                }                                
                            }
                        }
                    }
                    else
                    {
                        isFamily =false;
                        isProduct = false;
                        for (int i = 0; i <= YHSAndBC.Tables["YHSAndBC"].Rows.Count - 1; i++)
                        {
                            using (DataTable td = YHSAndBC.Tables["YHSAndBC"])
                            {
                                string tditemtype = string.Empty;
                                tditemtype = td.Rows[i]["ItemType"].ToString().ToLower();
                                string tdItemvalue = string.Empty;
                                tdItemvalue = td.Rows[i]["Itemvalue"].ToString();
                                string tdTempPath = string.Empty;
                                tdTempPath = td.Rows[i]["TempPath"].ToString().ToLower();
                                if (tditemtype == "family")
                                {
                                    familyid = HttpUtility.UrlEncode(tdItemvalue);
                                    isFamily = true;
                                }
                                //Modified by Indu
                                //if (td.Rows[i]["ItemType"].ToString().ToLower() == "product")
                                if (tditemtype == "product" && objhelper.CDEC(td.Rows[i]["Itemvalue"]) > 0)
                                {
                                    Productid = HttpUtility.UrlEncode(tdItemvalue);
                                    isProduct = true;
                                }
                                if (tdTempPath == "/category")
                                {

                                    //Ds = GetDataSet("Select Category_ID,isnull(CUSTOM_NUM_FIELD3,0) as CUSTOM_NUM_FIELD3 from tb_Category where Category_Name='" + td.Rows[i]["ItemValue"].ToString() + "'");
                                    Ds = (DataSet)objhelperDb.GetGenericDataDB(tdItemvalue, "GET_CATEGORY_ID_PARENT", HelperDB.ReturnType.RTDataSet);

                                    if (Ds != null && Ds.Tables[0].Rows.Count > 0)
                                    {
                                        CatId = Ds.Tables[0].Rows[0][0].ToString();
                                        byp = ((int)float.Parse(Ds.Tables[0].Rows[0]["CUSTOM_NUM_FIELD3"].ToString())).ToString();
                                    }
                                    tempstr = "?id=0&cid=" + CatId + "&byp=" + byp;
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "ct.aspx" + tempstr;
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = "Home.aspx?";
                                }

                                else if (tdTempPath == "/category/category")
                                {
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "pl.aspx" + tempstr + "&type=" + tdItemvalue + "&value=" + HttpUtility.UrlEncode(tdItemvalue) + "&bname=" + "&searchstr=";
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"];  //"ct.aspx" + tempstr;
                                }
                                else if (tdTempPath == "/category/brand")
                                {
                                    tsb = tdItemvalue;
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "ct.aspx" + tempstr + "&bypcat=1&tsb=" + HttpUtility.UrlEncode(tsb);
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"ct.aspx" + tempstr;
                                }
                                else if (tdTempPath == "/category/brand/model")
                                {
                                    tsm = tdItemvalue;
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "bb.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsm);
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"];//"ct.aspx" + tempstr + "&bypcat=1&tsb=" + HttpUtility.UrlEncode(tsb);
                                }
                                else if (tdTempPath == "/category/usersearch")
                                {
                                    scrtext = tdItemvalue;
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "ps.aspx" + tempstr + "&tsb=" + tsb + "&tsm=" + tsm + "&type=&value=&bname=" + tsb + "&searchstr=" + scrtext;
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"];  //"ct.aspx" + tempstr;

                                }
                                else if (tdTempPath.StartsWith("/category/usersearch"))
                                {
                                    if ((isProduct))
                                    {

                                        if (tditemtype == "product" && objhelper.CDEC(td.Rows[i]["Itemvalue"]) > 0)
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "pd.aspx" + tempstr + "&fid=" + familyid + "&pid=" + Productid;
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bb.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                        }
                                    }
                                    else if ((isFamily))
                                    {
                                        if (tditemtype == "family")
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "fl.aspx" + tempstr + "&fid=" + familyid;
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bb.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                        }
                                        else
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "fl.aspx" + tempstr + "&fid=" + familyid + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsm) + "&type=" + tdItemvalue + "&value=" + HttpUtility.UrlEncode(tdItemvalue) + "&bname=" + HttpUtility.UrlEncode(tsb) + "&searchstr=";
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bb.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                        }
                                    }
                                    else
                                    {
                                        YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "ps.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsm) + "&type=" + tdItemvalue + "&value=" + HttpUtility.UrlEncode(tdItemvalue) + "&bname=" + HttpUtility.UrlEncode(tsb) + "&searchstr=";
                                        YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bb.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                    }

                                }
                                else if (tdTempPath.StartsWith("/category/brand/model"))
                                {   if ((isProduct))
                                    {
                                    //Modified by:Indu
                                        //if (td.Rows[i]["ItemType"].ToString().ToLower() == "product")
                                        if (tditemtype == "product" && objhelper.CDEC(td.Rows[i]["Itemvalue"]) > 0)
                                    {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "pd.aspx" + tempstr + "&fid=" + familyid +"&pid=" +Productid;
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bb.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                        }                                       
                                    }
                                    else if ((isFamily))
                                    {
                                        if (tditemtype == "family")
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "fl.aspx" + tempstr + "&fid=" + familyid;
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bb.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                        }
                                        else
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "fl.aspx" + tempstr + "&fid=" + familyid + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsm) + "&type=" + td.Rows[i]["ItemType"].ToString() + "&value=" + HttpUtility.UrlEncode(tdItemvalue) + "&bname=" + HttpUtility.UrlEncode(tsb) + "&searchstr=";
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bb.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                        }
                                    }                                     
                                    else
                                    {
                                        YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "bb.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsm) + "&type=" + td.Rows[i]["ItemType"].ToString() + "&value=" + HttpUtility.UrlEncode(tdItemvalue) + "&bname=" + HttpUtility.UrlEncode(tsb) + "&searchstr=";
                                        YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bb.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                    }
                                }
                                else if (tdTempPath.StartsWith("/category/category") || tdTempPath.StartsWith("/category/family")
                                    //modified by indu
                                    || tdTempPath.StartsWith("/category/brand/family")
                                    )
                                {
                                    if ((isProduct))
                                    {
                                        //if (td.Rows[i]["ItemType"].ToString().ToLower() == "product")
                                        if (tditemtype == "product" && objhelper.CDEC(td.Rows[i]["Itemvalue"]) > 0)
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "pd.aspx" + tempstr + "&fid=" + familyid + "&pid=" + Productid;
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bb.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                        }
                                    }
                                    else if (isFamily == true)
                                    {
                                        if (tditemtype == "family")
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "fl.aspx" + tempstr + "&fid=" + familyid;
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bb.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                        }
                                        else
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "fl.aspx" + tempstr + "&fid=" + familyid + "&type=" + td.Rows[i]["ItemType"].ToString() + "&value=" + HttpUtility.UrlEncode(tdItemvalue) + "&bname=" + HttpUtility.UrlEncode(tsb) + "&searchstr=";
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bb.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                        }
                                    }
                                    
                                    else
                                    {
                                        YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "pl.aspx" + tempstr + "&type=" + td.Rows[i]["ItemType"].ToString() + "&value=" + HttpUtility.UrlEncode(tdItemvalue) + "&bname=" + HttpUtility.UrlEncode(tsb) + "&searchstr=";
                                        YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"ct.aspx" + tempstr;
                                    }
                                }
                                //Added bY iNDU:fOR NEW PROD
                                    //START
                                else if (tdTempPath.StartsWith("/family"))
                                {
                                    if ((isProduct))
                                    {
                                        //if (td.Rows[i]["ItemType"].ToString().ToLower() == "product")
                                        if (tditemtype == "product" && objhelper.CDEC(td.Rows[i]["Itemvalue"]) > 0)
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "pd.aspx?&fid=" + familyid + "&pid=" + Productid;
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = "fl.aspx?" + tempstr + "&fid=" + familyid; 
                                        }
                                    }
                                        
                                    else if ((isFamily))
                                    {
                                        if (tditemtype == "family")
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "fl.aspx?" + tempstr + "&fid=" + familyid;
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] ="Home.aspx?"; //"bb.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                        }
                                        //else
                                        //{
                                        //    YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "family.aspx" + tempstr + "&fid=" + familyid + "&type=" + td.Rows[i]["ItemType"].ToString() + "&value=" + HttpUtility.UrlEncode(td.Rows[i]["Itemvalue"].ToString()) + "&bname=" + HttpUtility.UrlEncode(tsb) + "&searchstr=";
                                        //    YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bybrand.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                        //}
                                    }
                                }
                                //END
                            }
                            
                        }
                    }
                }
              
                HttpContext.Current.Session["BreadCrumbDS"] = YHSAndBC;


          
            }


        }
        //private DataSet GetDataSet(string SQLQuery)
        //{
        //    DataSet ds = new DataSet();
        //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery, Gcon.ConnectionString.ToString().Substring(Gcon.ConnectionString.ToString().IndexOf(';') + 1));
        //    da.Fill(ds, "generictable");
        //    return ds;
        //}


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE BREAD CRUMB DETAILS  ***/
        /********************************************************************************/
        //public string GetBreadCrumb(string templatepath)
        //{
        //    StringTemplateGroup _stg_records = null;
        //    StringTemplateGroup _stg_records1 = null;
        //    StringTemplate _stmpl_records = null;
        //    StringTemplate _stmpl_records1 = null;

        //    string breadcrumb = "";
        //    string stemplatepath = templatepath;
           
        //    bool IsFromps = false;
        //    bool IsFromps_RC = false;
        //    bool IsFromproductlist = false;
        //    bool IsFromproductlist_RC = false;
        //    bool IsFromBrand = false;
        //    bool IsFromBrand_RC = false;
        //    string istsm = string.Empty;
        //    string istsb = string.Empty;
        //    _stg_records = new StringTemplateGroup("Cell", stemplatepath);
        //    _stg_records1 = new StringTemplateGroup("main", stemplatepath);

        //    DataSet ds = new DataSet();
        //    ds = null;
        //    int dscount1 = 0;
        //    int dscount = 0;
        
        //    if (HttpContext.Current.Session["BreadCrumbDS"] != null)
        //    {
        //        ds = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

        //    }
        //    if (ds != null && ds.Tables[0].Rows.Count > 0)
        //    {
        //        dscount = ds.Tables[0].Rows.Count-1;
        //        _stmpl_records1 = _stg_records1.GetInstanceOf("BreadCrumb" + "\\" + "home");
        //        string breadcrumEA = "//// //// ////";
        //        string RemovebreadcrumEA = "//// //// ////";
        //        bool familybrandmodelset = false; 
        //        int i;
        //        for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
        //        {
        //            //Modified by :indu
        //        //foreach (DataRow row in ds.Tables[0].Rows)
        //        //{
                   

        //            DataRow row = ds.Tables[0].Rows[i];
        //            DataRow Revrow;
        //            string Itemvalue = string.Empty;
        //            string newpagename = string.Empty;
        //            string newremovepagename = string.Empty;
        //            if (i == 0)
        //            {
        //                 Revrow = ds.Tables[0].Rows[0];
        //            }
        //            else
        //            {
        //                 Revrow = ds.Tables[0].Rows[i - 1];
        //            }
        //            if (dscount == dscount1)
        //                _stmpl_records = _stg_records.GetInstanceOf("BreadCrumb" + "\\" + "lastcell");
        //            else

        //            _stmpl_records = _stg_records.GetInstanceOf("BreadCrumb" + "\\" + "cell");
        //            string[] PAGENAME = row["Url"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);
        //            if (row["ItemType"].ToString().ToLower() == "category" && i==0)
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString());
        //                Itemvalue = HttpUtility.UrlEncode(row["ItemValue"].ToString());
        //            }

        //            else if (row["ItemType"].ToString().ToLower() == "family")
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["FamilyName"].ToString());
        //                Itemvalue = row["ItemValue"].ToString() + "=" + row["FamilyName"].ToString();
        //                HttpContext.Current.Session["S_FName"] = row["FamilyName"].ToString();
        //            }
        //            else if (row["ItemType"].ToString().ToLower() == "brand" && PAGENAME[0].ToLower().Contains("categorylist.aspx"))
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString());
        //                Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());
        //            }
        //            //else if ((row["ItemType"].ToString().ToLower() == "category") && i==0)
        //            //{
        //            //    _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString());
        //            //    if (HttpContext.Current.Request.QueryString["cid"] != null)
        //            //    {
        //            //        Itemvalue = HttpContext.Current.Request.QueryString["cid"].ToString() + "=" + row["ItemValue"].ToString();
        //            //    }
        //            //    else
        //            //    {
        //            //        Itemvalue =  row["ItemValue"].ToString();
        //            //    }
        //            //}
        //            else if (row["ItemType"].ToString().ToLower() == "product")
        //            {
        //                if (row["ProductCode"].ToString() != "")
        //                {
        //                    _stmpl_records.SetAttribute("TBT_LINKNAME", row["ProductCode"].ToString());

        //                    Itemvalue = row["ItemValue"].ToString() + "=" + row["ProductCode"].ToString();
        //                }
        //                else
        //                {
        //                    _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString());

        //                    Itemvalue = row["ItemType"].ToString() + "=" +HttpUtility.UrlEncode( row["ItemValue"].ToString());
        //                }
        //            }
        //            else if ((row["ItemType"].ToString().ToLower() != "category")
        //                && (row["ItemType"].ToString().ToLower() != "model") 
        //                && (row["ItemType"].ToString().ToLower() != "brand") 
        //                && (IsFromBrand == true) 
        //                && ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("family.aspx"))
        //                || HttpContext.Current.Request.Url.ToString().ToLower().Contains("productdetails.aspx")))
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString());
        //                Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());

        //            }
        //            else if (((row["ItemType"].ToString().ToLower() != "brand") 
        //                && (row["ItemType"].ToString().ToLower() != "model")) 
        //                && ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("categorylist.aspx") == true) 
        //                || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx"))
        //                || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("family.aspx")) 
        //                || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("productdetails.aspx"))))
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString());
        //                Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());

        //            }
        //            //else if ((row["ItemType"].ToString().ToLower() == "model") 
        //            //    && (IsFromBrand == false))
        //            //{
        //            ////else if ((row["ItemType"].ToString().ToLower() == "model") && (((HttpContext.Current.Request.Url.ToString().ToLower().Contains("family.aspx")) && (IsFromBrand == false)) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("product_list.aspx")) || ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("productdetails.aspx")) && (IsFromBrand == false)) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx"))))
        //            ////{
        //            //    _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString());
        //            //    Itemvalue = row["Actualvalue"].ToString().Replace("AttribSelect=", "").Replace("MODEL = ", "MODEL=");
        //            //}
        //            else if ((row["ItemType"].ToString().ToLower() == "model") 
                        
        //                && (IsFromBrand ==false))
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString());
        //                Itemvalue = row["Actualvalue"].ToString().Replace("AttribSelect=", "").Replace("'", "").Replace("MODEL = ", "MODEL=");
        //                Itemvalue = HttpUtility.UrlEncode(Itemvalue);
        //            }
        //            else if ((row["ItemType"].ToString().ToLower() != "category") && (row["ItemType"].ToString().ToLower() != "user search") && (row["ItemType"].ToString().ToLower().Contains("usersearch") == false) && (IsFromproductlist == true || IsFromps == true))
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString());
        //                Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());
        //            }
        //            else if (((row["ItemType"].ToString().ToLower() != "brand")) && ((row["ItemType"].ToString().ToLower() != "model")) && (row["Url"].ToString().Contains("bybrand.aspx") == true))
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString());
        //                Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());

        //            }

        //            else
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString());
        //                Itemvalue = HttpUtility.UrlEncode(row["ItemValue"].ToString());
        //            }
        //            //Itemvalue = Itemvalue.Replace("+", "||");  
        //           // Itemvalue = HttpUtility.UrlEncode(Itemvalue);  
        //            _stmpl_records.SetAttribute("TBT_REMOVEEAPATH", HttpUtility.UrlEncode(objsecurity.StringEnCrypt(row["RemoveEAPath"].ToString())));
        //            _stmpl_records.SetAttribute("TBT_REMOVEURL", row["RemoveUrl"].ToString());
        //            _stmpl_records.SetAttribute("TBT_URL", row["Url"].ToString());
        //            _stmpl_records.SetAttribute("TBT_EAPATH", HttpUtility.UrlEncode(objsecurity.StringEnCrypt(row["EAPath"].ToString())));
        //            _stmpl_records.SetAttribute("TBT_ATTRIBUTE_TYPE", row["ItemType"].ToString());
        //            //modified by :indu
        //            try
        //            {
                       
                    
        //                //TBWTemplateEngine objTempengine = new TBWTemplateEngine("", "", "");
        //                string Itemtype = row["ItemType"].ToString().ToUpper() ;
        //                string RevItemType = Revrow["ItemType"].ToString().ToUpper();
                       
        //                _stmpl_records.SetAttribute("TBT_REM_ATTRIBUTE_TYPE", RevItemType);
                        
        //                if (breadcrumEA == "//// //// ////")
        //                {
        //                    //if ((Itemtype == "family") || (Itemtype == "product") || (Itemtype == "category") || (Itemtype == "model") || (Itemtype == "brand"))
        //                    //{

        //                        RemovebreadcrumEA = breadcrumEA;
        //                        breadcrumEA = breadcrumEA + Itemvalue;
                                
                            
        //                    //}
        //                    //else
        //                    //{
        //                    //    RemovebreadcrumEA = breadcrumEA;
        //                    //    breadcrumEA = breadcrumEA + "////" + Itemtype + "/" + _stmpl_records.GetAttribute("TBT_LINKNAME");
                               
        //                    //}

        //                }
        //                else
        //                {
        //                    //if ((Itemtype == "family") || (Itemtype == "product") || (Itemtype == "category") || (Itemtype == "model") || (Itemtype == "brand"))
        //                    //{
        //                        RemovebreadcrumEA = breadcrumEA;
        //                        breadcrumEA = breadcrumEA + "////" + Itemvalue;
                                
        //                    //}
        //                    //else
        //                    //{
        //                    //    RemovebreadcrumEA = breadcrumEA;
        //                    //    breadcrumEA = breadcrumEA + "////" + Itemtype + "/" + _stmpl_records.GetAttribute("TBT_LINKNAME");
                                
        //                    //}
        //                }
        //                if (PAGENAME[0].ToLower().Contains("product_list.aspx"))
        //                {
        //                    newpagename = PAGENAME[0].ToLower();
        //                    IsFromproductlist = true;
        //                }
        //                else if (PAGENAME[0].ToLower().Contains("productdetails.aspx"))
        //                {
        //                    newpagename = PAGENAME[0].ToLower();
        //                    if (IsFromps == true)
        //                    {
        //                        breadcrumEA = breadcrumEA.Replace("UserSearch1=", "UserSearch=").Replace("User_Search=", "UserSearch=");
        //                        breadcrumEA = breadcrumEA.Replace("UserSearch1=UserSearch1=", "UserSearch=").Replace("User_Search=User_Search=", "UserSearch=");
        //                    }

                            
        //                }
        //                else if (PAGENAME[0].ToLower().Contains("family.aspx"))
        //                {
        //                    newpagename = PAGENAME[0].ToLower();
        //                    if (IsFromps == true)
        //                    {
        //                        breadcrumEA =  breadcrumEA.Replace ("//// //// ////","//// //// ////"+"UserSearch1=");
        //                        breadcrumEA = breadcrumEA.Replace("UserSearch1=UserSearch1=", "UserSearch1=");
        //                    }
        //                    else
        //                    {
        //                        //if ((HttpContext.Current.Request.QueryString["tsm"] != null) && (HttpContext.Current.Request.QueryString["tsm"] != null))
        //                        //{
        //                        //    string _tsm = HttpContext.Current.Request.QueryString["tsm"].Replace("_", " ");
        //                        //    string _tsb = HttpContext.Current.Request.QueryString["tsb"].Replace("_", " ");

        //                        //    if (_tsm != "" && _tsb != "")
        //                        //    {
        //                        //        breadcrumEA = breadcrumEA.Replace(_tsm, "Model=" + _tsm).Replace(_tsb, "Brand=" + _tsb);
        //                        //    }
        //                        //}
        //                        if ((IsFromBrand == true) && (istsb != string.Empty) && (istsm != string.Empty) && (familybrandmodelset==false))
        //                        {

        //                            breadcrumEA = breadcrumEA.Replace(istsm, "Model=" +istsm).Replace(istsb, "Brand=" + istsb);
        //                            familybrandmodelset = true;
        //                        }
                               

        //                    }
        //                }
                      
        //                else if (PAGENAME[0].ToLower().Contains("categorylist.aspx"))
        //                {
        //                    newpagename = PAGENAME[0].ToLower();
        //                    //breadcrumEA = breadcrumEA.Replace("Brand=", "");
        //                    if (Itemtype.ToLower() == "brand")
        //                    {
        //                        istsb = Itemvalue.Replace("Brand=", "");
        //                         IsFromBrand = true;
        //                    }
                        
        //                }
        //                else if (PAGENAME[0].ToLower().Contains("bybrand.aspx"))
        //                {
        //                    newpagename = PAGENAME[0].ToLower();
        //                    IsFromBrand = true;
        //                    if (Itemtype.ToLower() == "model")
        //                    {
        //                        istsm = Itemvalue.Replace("Model=", "");
        //                    }
        //                   breadcrumEA = breadcrumEA.Replace("Model=", "").Replace("Brand=", ""); 
        //                }
        //                else if (PAGENAME[0].ToLower().Contains("ps.aspx"))
        //                {
        //                    newpagename = PAGENAME[0].ToLower();

        //                    IsFromps = true;
        //                    breadcrumEA = breadcrumEA.Replace("User Search=", "").Replace("UserSearch=","").Replace("UserSearch1=","").Replace("UserSearch2=","").Replace("Category=","") ;
        //                  //  breadcrumEA = breadcrumEA.Replace("//// //// ////", "//// //// ////" + "UserSearch=");
        //                }

        //                else
        //                {
        //                    newpagename = PAGENAME[0].ToLower();

        //                }
        //              //  breadcrumEA = breadcrumEA.Replace("+", "||"); 
        //                    //Cons_NewURl(_stmpl_records, breadcrumEA, PAGENAME[0].ToLower(), true, false);                     

        //                if (IsFromBrand == true)
        //                {
        //                    objhelper.Cons_NewURl(_stmpl_records, breadcrumEA, "BC" + "-" + newpagename, false, Itemtype);
        //                }
        //                else 
        //                {
        //                    objhelper.Cons_NewURl(_stmpl_records, breadcrumEA, "BC" + "-" + newpagename, true, Itemtype);
        //                }
        //                    string[] REMOVEPAGENAME = row["RemoveUrl"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);
        //                if (REMOVEPAGENAME[0].ToLower ().Contains("product_list.aspx"))
        //                {

        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();
        //                    IsFromproductlist_RC = true;
        //                }
        //                else if (REMOVEPAGENAME[0].ToLower().Contains("productdetails.aspx"))
        //                {
        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();
        //                    if (IsFromps_RC == true)
        //                    {
        //                        RemovebreadcrumEA = RemovebreadcrumEA.Replace("UserSearch1=", "UserSearch=");
        //                    }
        //                }
        //                else if (REMOVEPAGENAME[0].ToLower().Contains("family.aspx"))
        //                {
        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();
        //                    if (IsFromps == true)
        //                    {
        //                        RemovebreadcrumEA = RemovebreadcrumEA.Replace("//// //// ////", "//// //// ////" + "UserSearch1=");
        //                    }
        //                    else
        //                    {
        //                        //if ((HttpContext.Current.Request.QueryString["tsm"] != null) && (HttpContext.Current.Request.QueryString["tsm"] != null))
        //                        //{
        //                        //    string _tsm = HttpContext.Current.Request.QueryString["tsm"].Replace("_", " ");
        //                        //    string _tsb = HttpContext.Current.Request.QueryString["tsb"].Replace("_", " ");
        //                        //    if (_tsm != "" && _tsb != "")
        //                        //    {
        //                        //        RemovebreadcrumEA = RemovebreadcrumEA.Replace(_tsm, "Model=" + _tsm).Replace(_tsb, "Brand=" + _tsb);
        //                        //    }
        //                        //}
        //                        if ((IsFromBrand == true) && (istsb != string.Empty) && (istsm != string.Empty))
        //                        {
        //                            RemovebreadcrumEA = RemovebreadcrumEA.Replace(istsm, "Model=" + istsm).Replace(istsb, "Brand=" + istsb);
        //                        }

        //                    }
        //                }
        //                else if (REMOVEPAGENAME[0].ToLower().Contains("categorylist.aspx"))
        //                {
        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();
        //                   // RemovebreadcrumEA = RemovebreadcrumEA.Replace("Brand=", "");
        //                    IsFromBrand_RC = true;
        //                }
        //                else if (REMOVEPAGENAME[0].ToLower().Contains("bybrand.aspx"))
        //                {
        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();
        //                    RemovebreadcrumEA = RemovebreadcrumEA.Replace("Model=","");
        //                    IsFromBrand_RC = true;
        //                }
        //                else if (REMOVEPAGENAME[0].ToLower().Contains("ps.aspx"))
        //                {
        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();
        //                    IsFromps_RC = true;
        //                    RemovebreadcrumEA = RemovebreadcrumEA.Replace("User Search=", "").Replace("UserSearch=", "").Replace("UserSearch1=", "").Replace("UserSearch2=", "").Replace("Category=", "");
        //                }

        //                else
        //                {
        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();

        //                }
        //              //  RemovebreadcrumEA = RemovebreadcrumEA.Replace("+", "||"); 
        //            // Cons_NewURl(_stmpl_records, breadcrumEA, REMOVEPAGENAME[0].ToLower(), true, true);
        //                if (IsFromBrand_RC == true)
        //                {
        //                    objhelper.Cons_NewURl(_stmpl_records, RemovebreadcrumEA, "BCRemoveurl" + "-" + newremovepagename.ToLower(), false, RevItemType);
        //                }
        //                else
        //                {
        //                    objhelper.Cons_NewURl(_stmpl_records, RemovebreadcrumEA, "BCRemoveurl" + "-" + newremovepagename.ToLower(), true, RevItemType);
        //                }
                      
        //            }
        //            catch (Exception ex)
        //            {
        //                //objErrorHandler.ErrorMsg = ex;
        //                //objErrorHandler.CreateLog();
        //            }
                  
                    
        //            breadcrumb = breadcrumb + _stmpl_records.ToString();
        //            dscount1++;
        //        }

        //    }
        //    return objhelper.StripWhitespace(_stmpl_records1 + breadcrumb + "</div>");
        //}
        //public string GetBreadCrumb(string templatepath)
        //{
        //    StringTemplateGroup _stg_records = null;
        //    StringTemplateGroup _stg_records1 = null;
        //    StringTemplate _stmpl_records = null;
        //    StringTemplate _stmpl_records1 = null;

        //    string breadcrumb = string.Empty;
        //    string stemplatepath = templatepath;

        //    bool IsFromps = false;
        //    bool IsFromps_RC = false;
        //    bool IsFromproductlist = false;
        //    bool IsFromproductlist_RC = false;
        //    bool IsFromBrand = false;
        //    bool IsFromBrand_RC = false;
        //    string istsm = string.Empty;
        //    string istsb = string.Empty;
        //    _stg_records = new StringTemplateGroup("Cell", stemplatepath);
        //    _stg_records1 = new StringTemplateGroup("main", stemplatepath);

        //    DataSet ds = new DataSet();
        //    ds = null;
        //    int dscount1 = 0;
        //    int dscount = 0;

        //    if (HttpContext.Current.Session["BreadCrumbDS"] != null)
        //    {
        //        ds = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

        //    }
        //    if (ds != null && ds.Tables[0].Rows.Count > 0)
        //    {
        //        dscount = ds.Tables[0].Rows.Count - 1;
        //        _stmpl_records1 = _stg_records1.GetInstanceOf("BreadCrumb" + "\\" + "home");
        //        string breadcrumEA = "//// //// ////";
        //        string RemovebreadcrumEA = "//// //// ////";
        //        bool familybrandmodelset = false;
        //        int i;
        //        for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
        //        {
        //            //Modified by :indu
        //            //foreach (DataRow row in ds.Tables[0].Rows)
        //            //{


        //            DataRow row = ds.Tables[0].Rows[i];
        //            DataRow Revrow;
        //            string Itemvalue = string.Empty;
        //            string newpagename = string.Empty;
        //            string newremovepagename = string.Empty;
        //            if (i == 0)
        //            {
        //                Revrow = ds.Tables[0].Rows[0];
        //            }
        //            else
        //            {
        //                Revrow = ds.Tables[0].Rows[i - 1];
        //            }
        //            if (dscount == dscount1)
        //                _stmpl_records = _stg_records.GetInstanceOf("BreadCrumb" + "\\" + "lastcell");
        //            else

        //                _stmpl_records = _stg_records.GetInstanceOf("BreadCrumb" + "\\" + "cell");
        //            string[] PAGENAME = row["Url"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);

        //            string rItemType = string.Empty;
        //            rItemType = row["ItemType"].ToString().ToLower();
        //            string rItemValue = string.Empty;
        //            rItemValue = row["ItemValue"].ToString();
        //            string httprequrl = string.Empty;
        //            httprequrl = HttpContext.Current.Request.Url.ToString().ToLower();

        //            string pagename = string.Empty;
        //            pagename = PAGENAME[0].ToLower();
        //            if (rItemType == "category" && i == 0)
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", rItemValue.ToUpper());
        //                Itemvalue = HttpUtility.UrlEncode(rItemValue);
        //                HttpContext.Current.Session["PRODUCT_CATEGORY_NAME_SES"] = rItemValue;
        //            }

        //            else if (rItemType == "family")
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["FamilyName"].ToString());
        //                Itemvalue = rItemValue + "=" + row["FamilyName"].ToString();
        //                HttpContext.Current.Session["S_FName"] = row["FamilyName"].ToString();
        //            }
        //            else if (rItemType == "brand" && pagename.Contains("ct.aspx"))
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", rItemValue);
        //                Itemvalue = rItemType + "=" + HttpUtility.UrlEncode(rItemValue);
        //            }
        //            //else if ((row["ItemType"].ToString().ToLower() == "category") && i==0)
        //            //{
        //            //    _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString());
        //            //    if (HttpContext.Current.Request.QueryString["cid"] != null)
        //            //    {
        //            //        Itemvalue = HttpContext.Current.Request.QueryString["cid"].ToString() + "=" + row["ItemValue"].ToString();
        //            //    }
        //            //    else
        //            //    {
        //            //        Itemvalue =  row["ItemValue"].ToString();
        //            //    }
        //            //}
        //            else if (rItemType == "product")
        //            {
        //                if (row["ProductCode"].ToString() != "")
        //                {
        //                    _stmpl_records.SetAttribute("TBT_LINKNAME", row["ProductCode"].ToString());

        //                    Itemvalue = rItemValue + "=" + row["ProductCode"].ToString();
        //                }
        //                else
        //                {
        //                    _stmpl_records.SetAttribute("TBT_LINKNAME", rItemValue);

        //                    Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(rItemValue);
        //                }
        //            }
        //            else if ((rItemType != "category")
        //                && (rItemType != "model")
        //                && (rItemType != "brand")
        //                && ((IsFromBrand))
        //                && ((httprequrl.Contains("fl.aspx"))
        //                || httprequrl.Contains("pd.aspx")))
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", rItemValue);
        //                Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(rItemValue);

        //            }
        //            else if (((rItemType != "brand")
        //                && (rItemType != "model"))
        //                && ((httprequrl.Contains("ct.aspx"))
        //                || (httprequrl.Contains("ps.aspx"))
        //                || (httprequrl.Contains("fl.aspx"))
        //                || (httprequrl.Contains("pd.aspx"))))
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", rItemValue);
        //                Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(rItemValue);

        //            }
        //            //else if ((row["ItemType"].ToString().ToLower() == "model") 
        //            //    && (IsFromBrand == false))
        //            //{
        //            ////else if ((row["ItemType"].ToString().ToLower() == "model") && (((HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")) && (IsFromBrand == false)) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx")) || ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx")) && (IsFromBrand == false)) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx"))))
        //            ////{
        //            //    _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString());
        //            //    Itemvalue = row["Actualvalue"].ToString().Replace("AttribSelect=", "").Replace("MODEL = ", "MODEL=");
        //            //}
        //            else if ((rItemType == "model")

        //                && (!(IsFromBrand)))
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", rItemValue);
        //                Itemvalue = row["Actualvalue"].ToString().ToLower().Replace("attribselect=", "").Replace("'", "").Replace("model = ", "model=");
        //                Itemvalue = HttpUtility.UrlEncode(Itemvalue);
        //            }
        //            else if ((rItemType != "category") && (rItemType != "user search") && (!(rItemType.Contains("usersearch"))) && ((IsFromproductlist) || (IsFromps)))
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", rItemValue);
        //                Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(rItemValue);
        //            }
        //            else if (((rItemType != "brand")) && ((rItemType != "model")) && ((row["Url"].ToString().Contains("bb.aspx"))))
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", rItemValue);
        //                Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(rItemValue);

        //            }

        //            else
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", rItemValue.ToUpper());
        //                Itemvalue = HttpUtility.UrlEncode(rItemValue);
        //            }
        //            //Itemvalue = Itemvalue.Replace("+", "||");  
        //            // Itemvalue = HttpUtility.UrlEncode(Itemvalue);  
        //            _stmpl_records.SetAttribute("TBT_REMOVEEAPATH", HttpUtility.UrlEncode(objsecurity.StringEnCrypt(row["RemoveEAPath"].ToString())));
        //            _stmpl_records.SetAttribute("TBT_REMOVEURL", row["RemoveUrl"].ToString());
        //            _stmpl_records.SetAttribute("TBT_URL", row["Url"].ToString());
        //            _stmpl_records.SetAttribute("TBT_EAPATH", HttpUtility.UrlEncode(objsecurity.StringEnCrypt(row["EAPath"].ToString())));
        //            _stmpl_records.SetAttribute("TBT_ATTRIBUTE_TYPE", row["ItemType"].ToString());

        //            try
        //            {
        //                string linkname = _stmpl_records.GetAttribute("TBT_LINKNAME").ToString() ;
        //                _stmpl_records.SetAttribute("TBT_LINKTITLE", linkname.Replace('"',' '));
        //            }
        //            catch
        //            { }


        //            //modified by :indu
        //            try
        //            {


        //                //TBWTemplateEngine objTempengine = new TBWTemplateEngine("", "", "");
        //                string Itemtype = row["ItemType"].ToString().ToUpper();
        //                string RevItemType = Revrow["ItemType"].ToString().ToUpper();
                    

        //                _stmpl_records.SetAttribute("TBT_REM_ATTRIBUTE_TYPE", RevItemType);

        //                if (breadcrumEA == "//// //// ////")
        //                {
        //                    //if ((Itemtype == "family") || (Itemtype == "product") || (Itemtype == "category") || (Itemtype == "model") || (Itemtype == "brand"))
        //                    //{

        //                    RemovebreadcrumEA = breadcrumEA;
        //                    breadcrumEA = breadcrumEA + Itemvalue;


        //                    //}
        //                    //else
        //                    //{
        //                    //    RemovebreadcrumEA = breadcrumEA;
        //                    //    breadcrumEA = breadcrumEA + "////" + Itemtype + "/" + _stmpl_records.GetAttribute("TBT_LINKNAME");

        //                    //}

        //                }
        //                else
        //                {
        //                    //if ((Itemtype == "family") || (Itemtype == "product") || (Itemtype == "category") || (Itemtype == "model") || (Itemtype == "brand"))
        //                    //{
        //                    RemovebreadcrumEA = breadcrumEA;
        //                    breadcrumEA = breadcrumEA + "////" + Itemvalue;

        //                    //}
        //                    //else
        //                    //{
        //                    //    RemovebreadcrumEA = breadcrumEA;
        //                    //    breadcrumEA = breadcrumEA + "////" + Itemtype + "/" + _stmpl_records.GetAttribute("TBT_LINKNAME");

        //                    //}
        //                }
        //                if (pagename.Contains("pl.aspx"))
        //                {
        //                    newpagename = PAGENAME[0].ToLower();
        //                    IsFromproductlist = true;
        //                }
        //                else if (pagename.Contains("pd.aspx"))
        //                {
        //                    newpagename = PAGENAME[0].ToLower();
        //                    if ((IsFromps))
        //                    {
        //                        breadcrumEA = breadcrumEA.ToUpper().Replace("USERSEARCH1=", "USERSEARCH=").Replace("USER_SEARCH=", "USERSEARCH=");
        //                        breadcrumEA = breadcrumEA.Replace("USERSEARCH1=USERSEARCH1=", "USERSEARCH=").Replace("USER_SEARCH=USER_SEARCH=", "USERSEARCH=");
        //                    }


        //                }
        //                else if (pagename.Contains("fl.aspx"))
        //                {
        //                    newpagename = PAGENAME[0].ToLower();
        //                    if ((IsFromps)&& (!(RemovebreadcrumEA.Contains("USERSEARCH1="))))
        //                    {
        //                        breadcrumEA = breadcrumEA.ToUpper().Replace("//// //// ////", "//// //// ////" + "USERSEARCH1=");
        //                        breadcrumEA = breadcrumEA.ToUpper().Replace("USERSEARCH1=USERSEARCH1=", "USERSEARCH1=");
        //                    }
        //                    else
        //                    {
        //                        //if ((HttpContext.Current.Request.QueryString["tsm"] != null) && (HttpContext.Current.Request.QueryString["tsm"] != null))
        //                        //{
        //                        //    string _tsm = HttpContext.Current.Request.QueryString["tsm"].Replace("_", " ");
        //                        //    string _tsb = HttpContext.Current.Request.QueryString["tsb"].Replace("_", " ");

        //                        //    if (_tsm != "" && _tsb != "")
        //                        //    {
        //                        //        breadcrumEA = breadcrumEA.Replace(_tsm, "Model=" + _tsm).Replace(_tsb, "Brand=" + _tsb);
        //                        //    }
        //                        //}
        //                        if ((rItemType == "brand") && (!(breadcrumEA.ToLower().Contains("brand="))))
        //                        {
        //                            istsb = row["ItemValue"].ToString();
        //                            breadcrumEA = breadcrumEA.ToUpper().Replace(istsb.ToUpper(), "BRAND=" + istsb.ToUpper());
        //                        }
        //                        else if ((IsFromBrand) && (istsb != string.Empty) && (istsm != string.Empty) && (!(familybrandmodelset)))
        //                        {
        //                            if (!(breadcrumEA.ToUpper().Contains("BRAND=")))
        //                            {
        //                                breadcrumEA = breadcrumEA.ToUpper().Replace(istsb.ToUpper(), "BRAND=" + istsb.ToUpper());
        //                            }
        //                            if (!(breadcrumEA.ToUpper().Contains("MODEL=")))
        //                            {
        //                                breadcrumEA = breadcrumEA.ToUpper().Replace(istsm.ToUpper(), "MODEL=" + istsm.ToUpper());
        //                            }

        //                            familybrandmodelset = true;
        //                        }


        //                    }
        //                }

        //                else if (pagename.Contains("ct.aspx"))
        //                {
        //                    newpagename = PAGENAME[0].ToLower();
        //                    //breadcrumEA = breadcrumEA.Replace("Brand=", "");
        //                    if (Itemtype.ToLower() == "brand")
        //                    {
        //                        istsb = Itemvalue.ToLower().Replace("brand=", "");
        //                        IsFromBrand = true;
        //                    }

        //                }
        //                else if (pagename.Contains("bb.aspx"))
        //                {
        //                    newpagename = PAGENAME[0].ToLower();
        //                    IsFromBrand = true;
        //                    if (Itemtype.ToLower() == "model")
        //                    {
        //                        istsm = Itemvalue.ToLower().Replace("model=", "");
        //                    }
        //                    breadcrumEA = breadcrumEA.ToUpper().Replace("MODEL=", "").Replace("BRAND=", "");
        //                }
        //                else if (pagename.Contains("ps.aspx"))
        //                {
        //                    newpagename = PAGENAME[0].ToLower();

        //                    IsFromps = true;
        //                    breadcrumEA = breadcrumEA.ToUpper().Replace("USER SEARCH=", "").Replace("USERSEARCH=", "").Replace("USERSEARCH1=", "").Replace("USERSEARCH2=", "").Replace("CATEGORY=", "");
        //                    //  breadcrumEA = breadcrumEA.Replace("//// //// ////", "//// //// ////" + "UserSearch=");
        //                }

        //                else
        //                {
        //                    newpagename = PAGENAME[0].ToLower();

        //                }
        //                //  breadcrumEA = breadcrumEA.Replace("+", "||"); 
        //                //Cons_NewURl(_stmpl_records, breadcrumEA, PAGENAME[0].ToLower(), true, false);                     
        //                string newurl = string.Empty;
        //                if ((IsFromBrand))
        //                {
        //                newurl=objhelper.Cons_NewURl(_stmpl_records, breadcrumEA, "BC" + "-" + newpagename, false, Itemtype);
        //                }
        //                else
        //                {
        //                 newurl= objhelper.Cons_NewURl(_stmpl_records, breadcrumEA, "BC" + "-" + newpagename, true, Itemtype);
        //                }
        //                if (i == ds.Tables[0].Rows.Count - 1)
        //                {
        //                    string url = HttpContext.Current.Request.RawUrl.ToLower();
        //                    if ((url.ToLower().Contains("pl.aspx")) ||
        //     (url.Contains("fl.aspx")) ||
        //      (url.Contains("pd.aspx")) ||
        //     (url.Contains("ct.aspx")) ||
        //     (url.Contains("bb.aspx")) ||
        //       (url.Contains("ps.aspx")) 
        //              )
        //                    {
        //                        if((!(url.Contains("/ct/")))&& (!(url.Contains("/bb/"))))
        //                        {
        //                            if (newpagename == "")
        //                            {
        //                                newpagename = "ps.aspx";
        //                            }
        //                        HttpContext.Current.Response.RedirectPermanent("/"+newurl + "/" + newpagename.Replace(".aspx","/"));  
        //                        }
        //                    }
        //                }
        //                string[] REMOVEPAGENAME = row["RemoveUrl"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);
        //                string rpagename = string.Empty;
        //                rpagename = REMOVEPAGENAME[0].ToLower();

        //                if (rpagename.Contains("pl.aspx"))
        //                {

        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();
        //                    IsFromproductlist_RC = true;
        //                }
        //                else if (rpagename.Contains("pd.aspx"))
        //                {
        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();
        //                    if ((IsFromps_RC))
        //                    {
        //                        RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("USERSEARCH1=", "USERSEARCH=");
        //                    }
        //                }
        //                else if (rpagename.Contains("fl.aspx"))
        //                {
        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();
        //                    if (((IsFromps))&&(!(RemovebreadcrumEA.Contains("USERSEARCH1="))))
        //                    {
        //                        RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("//// //// ////", "//// //// ////" + "USERSEARCH1=");
        //                    }
        //                    else
        //                    {
        //                        //if ((HttpContext.Current.Request.QueryString["tsm"] != null) && (HttpContext.Current.Request.QueryString["tsm"] != null))
        //                        //{
        //                        //    string _tsm = HttpContext.Current.Request.QueryString["tsm"].Replace("_", " ");
        //                        //    string _tsb = HttpContext.Current.Request.QueryString["tsb"].Replace("_", " ");
        //                        //    if (_tsm != "" && _tsb != "")
        //                        //    {
        //                        //        RemovebreadcrumEA = RemovebreadcrumEA.Replace(_tsm, "Model=" + _tsm).Replace(_tsb, "Brand=" + _tsb);
        //                        //    }
        //                        //}
        //                        if (((IsFromBrand)) && (istsb != string.Empty) && (istsm != string.Empty))
        //                        {
        //                            RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace(istsm.ToUpper(), "MODEL=" + istsm.ToUpper()).Replace(istsb.ToUpper(), "BRAND=" + istsb.ToUpper());
        //                        }

        //                    }
        //                }
        //                else if (rpagename.Contains("ct.aspx"))
        //                {
        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();
        //                    // RemovebreadcrumEA = RemovebreadcrumEA.Replace("Brand=", "");
        //                    IsFromBrand_RC = true;
        //                }
        //                else if (rpagename.Contains("bb.aspx"))
        //                {
        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();
        //                    RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("MODEL=", "");
        //                    IsFromBrand_RC = true;
        //                }
        //                else if (rpagename.Contains("ps.aspx"))
        //                {
        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();
        //                    IsFromps_RC = true;
        //                    RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("USER SEARCH=", "")
        //                        .Replace("USERSEARCH=", "").Replace("USERSEARCH1=", "")
        //                        .Replace("USERSEARCH2=", "").Replace("CATEGORY=", "")
        //                         .Replace("MODEL=MODEL=", "MODEL=").Replace("BRAND=BRAND=", "BRAND="); ;
        //                }

        //                else
        //                {
        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();

        //                }
        //                //added by:indu
        //                RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("USERSEARCH1 = USERSEARCH =", "USERSEARCH =").Replace("USERSEARCH1=USERSEARCH1=", "USERSEARCH1=")
        //                     .Replace("MODEL=MODEL=", "MODEL=").Replace("BRAND=BRAND=", "BRAND="); ; 
        //                //  RemovebreadcrumEA = RemovebreadcrumEA.Replace("+", "||"); 
        //                // Cons_NewURl(_stmpl_records, breadcrumEA, REMOVEPAGENAME[0].ToLower(), true, true);
        //                if ((IsFromBrand_RC))
        //                {
        //                    objhelper.Cons_NewURl(_stmpl_records, RemovebreadcrumEA, "BCRemoveurl" + "-" + newremovepagename, false, RevItemType);
        //                }
        //                else
        //                {
        //                    objhelper.Cons_NewURl(_stmpl_records, RemovebreadcrumEA, "BCRemoveurl" + "-" + newremovepagename, true, RevItemType);
        //                }

        //            }
        //            catch (System.Threading.ThreadAbortException)
        //            {
        //                // ignore it
        //            }
        //            catch (Exception ex)
        //            {
        //                //objErrorHandler.ErrorMsg = ex;
        //                //objErrorHandler.CreateLog();
        //            }


        //            breadcrumb = breadcrumb + _stmpl_records.ToString();
        //            dscount1++;
        //        }

        //    }
        //    return objhelper.StripWhitespace(_stmpl_records1 + breadcrumb + "</div>");
        //}

        //public string GetBreadCrumbMS(string templatepath,Boolean withHome  )
        //{
        //    StringTemplateGroup _stg_records = null;
        //    StringTemplateGroup _stg_records1 = null;
        //    StringTemplateGroup _stg_recordsH = null;
        //    StringTemplate _stmpl_records = null;
        //    StringTemplate _stmpl_records1 = null;
        //    StringTemplate _stmpl_recordsH = null;
        //    string breadcrumb = string.Empty;
        //    string stemplatepath = templatepath;

        //    bool IsFromps = false;
        //    bool IsFromps_RC = false;
        //    bool IsFromproductlist = false;
        //    bool IsFromproductlist_RC = false;
        //    bool IsFromBrand = false;
        //    bool IsFromBrand_RC = false;
        //    string istsm = string.Empty;
        //    string istsb = string.Empty;
        //    _stg_records = new StringTemplateGroup("Cell", stemplatepath);
        //    _stg_records1 = new StringTemplateGroup("main", stemplatepath);
        //    _stg_recordsH = new StringTemplateGroup("home", stemplatepath);

        //    DataSet dsb = new DataSet();
        //    dsb = null;
        //    int dscount1 = 0;
        //    int dscount = 0;
        //    string supName ="";
            

        //    if (HttpContext.Current.Session["BreadCrumbDS"] != null)
        //    {
        //        dsb = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

        //    }
        //    if (dsb != null && dsb.Tables[0].Rows.Count > 0)
        //    {
        //        dscount = dsb.Tables[0].Rows.Count - 1;
        //        _stmpl_records1 = _stg_records1.GetInstanceOf("BreadCrumb" + "\\" + "Main");
        //        _stmpl_recordsH = _stg_recordsH.GetInstanceOf("BreadCrumb" + "\\" + "home");

        //        string breadcrumEA = "//// //// ////";
        //        string RemovebreadcrumEA = "//// //// ////";
        //        bool familybrandmodelset = false;
        //        int i;

        //         supName = dsb.Tables[0].Rows[0]["ItemValue"].ToString();

        //        _stmpl_recordsH.SetAttribute("MICROSITEURL", "/" + objhelper.Cons_NewURl_bybrand("", supName, "mct.aspx", "") + "/mct/");
        //        for (i = 0; i <= dsb.Tables[0].Rows.Count - 1; i++)
        //        {
        //            //Modified by :indu
        //            //foreach (DataRow row in ds.Tables[0].Rows)
        //            //{


        //            DataRow row = dsb.Tables[0].Rows[i];
        //            DataRow Revrow;
        //            string Itemvalue = string.Empty;
        //            string newpagename = string.Empty;
        //            string newremovepagename = string.Empty;
        //            if (i == 0)
        //            {
        //                Revrow = dsb.Tables[0].Rows[0];
        //            }
        //            else
        //            {
        //                Revrow = dsb.Tables[0].Rows[i - 1];
        //            }
        //            if (dscount == dscount1)
        //                _stmpl_records = _stg_records.GetInstanceOf("BreadCrumb" + "\\" + "lastcell");
        //            else

        //                _stmpl_records = _stg_records.GetInstanceOf("BreadCrumb" + "\\" + "cell");



        //            string[] PAGENAME = row["Url"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);
        //            if (row["ItemType"].ToString().ToLower() == "category" && i == 0)
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString().ToLower());
                        
        //                Itemvalue = HttpUtility.UrlEncode(row["ItemValue"].ToString());
        //            }

        //            else if (row["ItemType"].ToString().ToLower() == "family")
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["FamilyName"].ToString().ToLower());
        //                Itemvalue = row["ItemValue"].ToString() + "=" + row["FamilyName"].ToString();
        //                HttpContext.Current.Session["S_FName"] = row["FamilyName"].ToString();
        //            }
        //            else if (row["ItemType"].ToString().ToLower() == "brand" && PAGENAME[0].ToLower().Contains("ct.aspx"))
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString().ToLower());
        //                Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());
        //            }
        //            //else if ((row["ItemType"].ToString().ToLower() == "category") && i==0)
        //            //{
        //            //    _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString());
        //            //    if (HttpContext.Current.Request.QueryString["cid"] != null)
        //            //    {
        //            //        Itemvalue = HttpContext.Current.Request.QueryString["cid"].ToString() + "=" + row["ItemValue"].ToString();
        //            //    }
        //            //    else
        //            //    {
        //            //        Itemvalue =  row["ItemValue"].ToString();
        //            //    }
        //            //}
        //            else if (row["ItemType"].ToString().ToLower() == "product")
        //            {
        //                if (row["ProductCode"].ToString() != "")
        //                {
        //                    _stmpl_records.SetAttribute("TBT_LINKNAME", row["ProductCode"].ToString().ToLower());

        //                    Itemvalue = row["ItemValue"].ToString() + "=" + row["ProductCode"].ToString();
        //                }
        //                else
        //                {
        //                    _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString().ToLower());

        //                    Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());
        //                }
        //            }
        //            else if ((row["ItemType"].ToString().ToLower() != "category")
        //                && (row["ItemType"].ToString().ToLower() != "model")
        //                && (row["ItemType"].ToString().ToLower() != "brand")
        //                && (IsFromBrand == true)
        //                && ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx"))
        //                || HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx")))
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString().ToLower());
        //                Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());

        //            }
        //            else if (((row["ItemType"].ToString().ToLower() != "brand")
        //                && (row["ItemType"].ToString().ToLower() != "model"))
        //                && ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true)
        //                || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx"))
        //                || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx"))
        //                || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx"))))
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString().ToLower());
        //                Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());

        //            }
        //            //else if ((row["ItemType"].ToString().ToLower() == "model") 
        //            //    && (IsFromBrand == false))
        //            //{
        //            ////else if ((row["ItemType"].ToString().ToLower() == "model") && (((HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")) && (IsFromBrand == false)) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx")) || ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx")) && (IsFromBrand == false)) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx"))))
        //            ////{
        //            //    _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString());
        //            //    Itemvalue = row["Actualvalue"].ToString().Replace("AttribSelect=", "").Replace("MODEL = ", "MODEL=");
        //            //}
        //            else if ((row["ItemType"].ToString().ToLower() == "model")

        //                && (IsFromBrand == false))
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString().ToLower());
        //                Itemvalue = row["Actualvalue"].ToString().ToLower().Replace("attribselect=", "").Replace("'", "").Replace("model = ", "model=");
        //                Itemvalue = HttpUtility.UrlEncode(Itemvalue);
        //            }
        //            else if ((row["ItemType"].ToString().ToLower() != "category") && (row["ItemType"].ToString().ToLower() != "user search") && (row["ItemType"].ToString().ToLower().Contains("usersearch") == false) && (IsFromproductlist == true || IsFromps == true))
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString().ToLower());
        //                Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());
        //            }
        //            else if (((row["ItemType"].ToString().ToLower() != "brand")) && ((row["ItemType"].ToString().ToLower() != "model")) && (row["Url"].ToString().Contains("bb.aspx") == true))
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString().ToLower());
        //                Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());

        //            }

        //            else
        //            {
        //                _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString().ToLower());
        //                Itemvalue = HttpUtility.UrlEncode(row["ItemValue"].ToString());
        //            }
        //            //Itemvalue = Itemvalue.Replace("+", "||");  
        //            // Itemvalue = HttpUtility.UrlEncode(Itemvalue);  
        //            _stmpl_records.SetAttribute("TBT_REMOVEEAPATH", HttpUtility.UrlEncode(objsecurity.StringEnCrypt(row["RemoveEAPath"].ToString())));
        //            _stmpl_records.SetAttribute("TBT_REMOVEURL", row["RemoveUrl"].ToString());
        //            _stmpl_records.SetAttribute("TBT_URL", row["Url"].ToString());
        //            _stmpl_records.SetAttribute("TBT_EAPATH", HttpUtility.UrlEncode(objsecurity.StringEnCrypt(row["EAPath"].ToString())));
        //            _stmpl_records.SetAttribute("TBT_ATTRIBUTE_TYPE", row["ItemType"].ToString());
        //            try
        //            {
        //                string linkname = _stmpl_records.GetAttribute("TBT_LINKNAME").ToString();
        //                _stmpl_records.SetAttribute("TBT_LINKTITLE", linkname.Replace('"', ' '));
        //            }
        //            catch
        //            { }
        //            //modified by :indu
        //            try
        //            {


        //                //TBWTemplateEngine objTempengine = new TBWTemplateEngine("", "", "");
        //                string Itemtype = row["ItemType"].ToString().ToUpper();
        //                string RevItemType = Revrow["ItemType"].ToString().ToUpper();

        //                _stmpl_records.SetAttribute("TBT_REM_ATTRIBUTE_TYPE", RevItemType);

        //                if (breadcrumEA == "//// //// ////")
        //                {
        //                    //if ((Itemtype == "family") || (Itemtype == "product") || (Itemtype == "category") || (Itemtype == "model") || (Itemtype == "brand"))
        //                    //{

        //                    RemovebreadcrumEA = breadcrumEA;
        //                    breadcrumEA = breadcrumEA + Itemvalue;


        //                    //}
        //                    //else
        //                    //{
        //                    //    RemovebreadcrumEA = breadcrumEA;
        //                    //    breadcrumEA = breadcrumEA + "////" + Itemtype + "/" + _stmpl_records.GetAttribute("TBT_LINKNAME");

        //                    //}

        //                }
        //                else
        //                {
        //                    //if ((Itemtype == "family") || (Itemtype == "product") || (Itemtype == "category") || (Itemtype == "model") || (Itemtype == "brand"))
        //                    //{
        //                    RemovebreadcrumEA = breadcrumEA;
        //                    breadcrumEA = breadcrumEA + "////" + Itemvalue;

        //                    //}
        //                    //else
        //                    //{
        //                    //    RemovebreadcrumEA = breadcrumEA;
        //                    //    breadcrumEA = breadcrumEA + "////" + Itemtype + "/" + _stmpl_records.GetAttribute("TBT_LINKNAME");

        //                    //}
        //                }
        //                if (PAGENAME[0].ToLower().Contains("pl.aspx"))
        //                {
        //                    newpagename = PAGENAME[0].ToLower();
        //                    IsFromproductlist = true;
        //                }
        //                else if (PAGENAME[0].ToLower().Contains("pd.aspx"))
        //                {
        //                    newpagename = PAGENAME[0].ToLower();
        //                    if (IsFromps == true)
        //                    {
        //                        breadcrumEA = breadcrumEA.ToUpper().Replace("USERSEARCH1=", "USERSEARCH=").Replace("USER_SEARCH=", "USERSEARCH=");
        //                        breadcrumEA = breadcrumEA.Replace("USERSEARCH1=USERSEARCH1=", "USERSEARCH=").Replace("USER_SEARCH=USER_SEARCH=", "USERSEARCH=");
        //                    }


        //                }
        //                else if (PAGENAME[0].ToLower().Contains("fl.aspx"))
        //                {
        //                    newpagename = PAGENAME[0].ToLower();
        //                    if ((IsFromps == true) && (RemovebreadcrumEA.Contains("USERSEARCH1=") == false))
        //                    {
        //                        breadcrumEA = breadcrumEA.ToUpper().Replace("//// //// ////", "//// //// ////" + "USERSEARCH1=");
        //                        breadcrumEA = breadcrumEA.ToUpper().Replace("USERSEARCH1=USERSEARCH1=", "USERSEARCH1=");
        //                    }
        //                    else
        //                    {
        //                        //if ((HttpContext.Current.Request.QueryString["tsm"] != null) && (HttpContext.Current.Request.QueryString["tsm"] != null))
        //                        //{
        //                        //    string _tsm = HttpContext.Current.Request.QueryString["tsm"].Replace("_", " ");
        //                        //    string _tsb = HttpContext.Current.Request.QueryString["tsb"].Replace("_", " ");

        //                        //    if (_tsm != "" && _tsb != "")
        //                        //    {
        //                        //        breadcrumEA = breadcrumEA.Replace(_tsm, "Model=" + _tsm).Replace(_tsb, "Brand=" + _tsb);
        //                        //    }
        //                        //}
        //                        if ((row["ItemType"].ToString().ToLower() == "brand") && (breadcrumEA.ToLower().Contains("brand=") == false))
        //                        {
        //                            istsb = row["ItemValue"].ToString();
        //                            breadcrumEA = breadcrumEA.ToUpper().Replace(istsb.ToUpper(), "BRAND=" + istsb.ToUpper());
        //                        }
        //                        else if ((IsFromBrand == true) && (istsb != string.Empty) && (istsm != string.Empty) && (familybrandmodelset == false))
        //                        {
        //                            if (breadcrumEA.ToUpper().Contains("BRAND=") == false)
        //                            {
        //                                breadcrumEA = breadcrumEA.ToUpper().Replace(istsb.ToUpper(), "BRAND=" + istsb.ToUpper());
        //                            }
        //                            if (breadcrumEA.ToUpper().Contains("MODEL=") == false)
        //                            {
        //                                breadcrumEA = breadcrumEA.ToUpper().Replace(istsm.ToUpper(), "MODEL=" + istsm.ToUpper());
        //                            }

        //                            familybrandmodelset = true;
        //                        }


        //                    }
        //                }

        //                else if (PAGENAME[0].ToLower().Contains("ct.aspx"))
        //                {
        //                    newpagename = PAGENAME[0].ToLower();
        //                    //breadcrumEA = breadcrumEA.Replace("Brand=", "");
        //                    if (Itemtype.ToLower() == "brand")
        //                    {
        //                        istsb = Itemvalue.ToLower().Replace("brand=", "");
        //                        IsFromBrand = true;
        //                    }

        //                }
        //                else if (PAGENAME[0].ToLower().Contains("bb.aspx"))
        //                {
        //                    newpagename = PAGENAME[0].ToLower();
        //                    IsFromBrand = true;
        //                    if (Itemtype.ToLower() == "model")
        //                    {
        //                        istsm = Itemvalue.ToLower().Replace("model=", "");
        //                    }
        //                    breadcrumEA = breadcrumEA.ToUpper().Replace("MODEL=", "").Replace("BRAND=", "");
        //                }
        //                else if (PAGENAME[0].ToLower().Contains("ps.aspx"))
        //                {
        //                    newpagename = PAGENAME[0].ToLower();

        //                    IsFromps = true;
        //                    breadcrumEA = breadcrumEA.ToUpper().Replace("USER SEARCH=", "").Replace("USERSEARCH=", "").Replace("USERSEARCH1=", "").Replace("USERSEARCH2=", "").Replace("CATEGORY=", "");
        //                    //  breadcrumEA = breadcrumEA.Replace("//// //// ////", "//// //// ////" + "UserSearch=");
        //                }

        //                else
        //                {
        //                    newpagename = PAGENAME[0].ToLower();

        //                }
        //                //  breadcrumEA = breadcrumEA.Replace("+", "||"); 
        //                //Cons_NewURl(_stmpl_records, breadcrumEA, PAGENAME[0].ToLower(), true, false);                     
        //                string newurl = string.Empty;
        //                if (IsFromBrand == true)
        //                {
        //                    newurl = objhelper.Cons_NewURl(_stmpl_records, breadcrumEA, "BC" + "-" + "m"+newpagename, false, Itemtype);
        //                }
        //                else
        //                {
        //                    newurl = objhelper.Cons_NewURl(_stmpl_records, breadcrumEA, "BC" + "-" + "m"+newpagename, true, Itemtype);
        //                }
        //                if (i == dsb.Tables[0].Rows.Count - 1)
        //                {
        //                    string url = HttpContext.Current.Request.RawUrl;
        //                    if ((url.ToLower().Contains("pl.aspx") == true) ||
        //     (url.ToLower().Contains("fl.aspx") == true) ||
        //      (url.ToLower().Contains("pd.aspx") == true) ||
        //     (url.ToLower().Contains("ct.aspx") == true) ||
        //     (url.ToLower().Contains("bb.aspx") == true) ||
        //       (url.ToLower().Contains("ps.aspx") == true)
        //              )
        //                    {
        //                        if ((url.ToLower().Contains("/ct/") == false) && (url.ToLower().Contains("/bb/") == false))
        //                        {
        //                            if (newpagename == "")
        //                            {
        //                                newpagename = "ps.aspx";
        //                            }
        //                            HttpContext.Current.Response.RedirectPermanent("/" + newurl + "/" + newpagename.Replace(".aspx", "/"));
        //                        }
        //                    }
        //                }
        //                string[] REMOVEPAGENAME = row["RemoveUrl"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);
        //                if (REMOVEPAGENAME[0].ToLower().Contains("pl.aspx"))
        //                {

        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();
        //                    IsFromproductlist_RC = true;
        //                }
        //                else if (REMOVEPAGENAME[0].ToLower().Contains("pd.aspx"))
        //                {
        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();
        //                    if (IsFromps_RC == true)
        //                    {
        //                        RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("USERSEARCH1=", "USERSEARCH=");
        //                    }
        //                }
        //                else if (REMOVEPAGENAME[0].ToLower().Contains("fl.aspx"))
        //                {
        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();
        //                    if ((IsFromps == true) && (RemovebreadcrumEA.Contains("USERSEARCH1=") == false))
        //                    {
        //                        RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("//// //// ////", "//// //// ////" + "USERSEARCH1=");
        //                    }
        //                    else
        //                    {
        //                        //if ((HttpContext.Current.Request.QueryString["tsm"] != null) && (HttpContext.Current.Request.QueryString["tsm"] != null))
        //                        //{
        //                        //    string _tsm = HttpContext.Current.Request.QueryString["tsm"].Replace("_", " ");
        //                        //    string _tsb = HttpContext.Current.Request.QueryString["tsb"].Replace("_", " ");
        //                        //    if (_tsm != "" && _tsb != "")
        //                        //    {
        //                        //        RemovebreadcrumEA = RemovebreadcrumEA.Replace(_tsm, "Model=" + _tsm).Replace(_tsb, "Brand=" + _tsb);
        //                        //    }
        //                        //}
        //                        if ((IsFromBrand == true) && (istsb != string.Empty) && (istsm != string.Empty))
        //                        {
        //                            RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace(istsm.ToUpper(), "MODEL=" + istsm.ToUpper()).Replace(istsb.ToUpper(), "BRAND=" + istsb.ToUpper());
        //                        }

        //                    }
        //                }
        //                else if (REMOVEPAGENAME[0].ToLower().Contains("ct.aspx"))
        //                {
        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();
        //                    // RemovebreadcrumEA = RemovebreadcrumEA.Replace("Brand=", "");
        //                    IsFromBrand_RC = true;
        //                }
        //                else if (REMOVEPAGENAME[0].ToLower().Contains("bb.aspx"))
        //                {
        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();
        //                    RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("MODEL=", "");
        //                    IsFromBrand_RC = true;
        //                }
        //                else if (REMOVEPAGENAME[0].ToLower().Contains("ps.aspx"))
        //                {
        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();
        //                    IsFromps_RC = true;
        //                    RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("USER SEARCH=", "")
        //                        .Replace("USERSEARCH=", "").Replace("USERSEARCH1=", "")
        //                        .Replace("USERSEARCH2=", "").Replace("CATEGORY=", "")
        //                         .Replace("MODEL=MODEL=", "MODEL=").Replace("BRAND=BRAND=", "BRAND="); ;
        //                }

        //                else
        //                {
        //                    newremovepagename = REMOVEPAGENAME[0].ToLower();

        //                }
        //                //added by:indu
        //                RemovebreadcrumEA = RemovebreadcrumEA.ToUpper().Replace("USERSEARCH1 = USERSEARCH =", "USERSEARCH =").Replace("USERSEARCH1=USERSEARCH1=", "USERSEARCH1=")
        //                     .Replace("MODEL=MODEL=", "MODEL=").Replace("BRAND=BRAND=", "BRAND="); ;
        //                //  RemovebreadcrumEA = RemovebreadcrumEA.Replace("+", "||"); 
        //                // Cons_NewURl(_stmpl_records, breadcrumEA, REMOVEPAGENAME[0].ToLower(), true, true);
        //                if (IsFromBrand_RC == true)
        //                {
        //                    objhelper.Cons_NewURl(_stmpl_records, RemovebreadcrumEA, "BCRemoveurl" + "-" + "m"+newremovepagename.ToLower(), false, RevItemType);
        //                }
        //                else
        //                {
        //                    objhelper.Cons_NewURl(_stmpl_records, RemovebreadcrumEA, "BCRemoveurl" + "-" + "m" + newremovepagename.ToLower(), true, RevItemType);
        //                }

        //            }
        //            catch (System.Threading.ThreadAbortException)
        //            {
        //                // ignore it
        //            }
        //            catch (Exception ex)
        //            {
        //                //objErrorHandler.ErrorMsg = ex;
        //                //objErrorHandler.CreateLog();
        //            }

        //            if (i!=0)  /// remove first category
        //                breadcrumb = breadcrumb + _stmpl_records.ToString();

        //            dscount1++;
        //        }

        //    }
        //    if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mfl.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("mpd.aspx") == true)
        //        _stmpl_records1.SetAttribute("GO_BACK", true);
        //    else
        //        _stmpl_records1.SetAttribute("GO_BACK", false);

        //    string rtn = string.Empty;
        //    if( withHome==true)
        //        rtn = objhelper.StripWhitespace(_stmpl_records1.ToString() + _stmpl_recordsH.ToString() + breadcrumb + "</div>");
        //    else
        //        rtn = objhelper.StripWhitespace(_stmpl_records1.ToString() + breadcrumb + "</div>");
        //    return rtn; 
        //}

        #endregion
        # region   "Main Category_Menu_Click"
        DataSet Menu_Category = new DataSet();
        DataTable Menu_Parent = new DataTable("ParentCategory");
        DataTable Menu_Main_Category = new DataTable("MainCategory");
        DataTable Menu_Sub_Category = new DataTable("SubCategory");
        DataTable Menu_Brand = new DataTable("Brand");


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE CATEGORY,SUB CATEGORY,PRODUCT DETAILS  ***/
        /********************************************************************************/
        public DataTable GetMainMenuClickDetail(string Cid, string ReturnType)
        {
            //Boolean blnGetData = false;
            //if (HttpContext.Current.Session["MainMenuClick"] == null)
            //{
            //    blnGetData = true;
            //}
            //else if (((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["ParentCategory"].Rows[0]["CATEGORY_ID"].ToString().ToUpper() != Cid.ToUpper())
            //{
            //    blnGetData = true;
            //}
            //DataTable tmptbl = new DataTable();

            //string CatName = "";
            //tmptbl = GetCategoryAndBrand("MainCategory").Tables[0].Select("CATEGORY_ID='" + Cid + "'").CopyToDataTable();
            //if (tmptbl != null && tmptbl.Rows.Count > 0)
            //{
            //    CatName = tmptbl.Rows[0]["CATEGORY_NAME"].ToString();
            //}

            //if (blnGetData == true)
            //{

            //    if (CatName != "")
            //    {
            //        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WesCatBrandDictionary);
            //        IOptions opts = ea.getOptions();
            //        opts.setResultsPerPage("1");
            //        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);
            //        opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
            //        opts.setSubCategories(true);
            //        opts.setNavigateHierarchy(false);
            //        opts.setReturnSKUs(false);

            //        INavigateResults res = ea.userBreadCrumbClick("AllProducts////WESAUSTRALASIA////" + CatName);
            //        String path = res.getCatPath();
            //        HttpContext.Current.Session["EA"] = res.getCatPath();
            //        CreateYouHaveSelectAndBreadCrumb();

            //        IList<INavigateCategory> list = res.getDetailedCategories();
            //        //IList<INavigateCategory> li = res.getDetailedAttributeValues(  ();
            //        //For Brand Table.
            //        BuildMainAndSubCategoryTable(list, res, CatName, Cid);

            //        DataRow row2 = Menu_Parent.NewRow();
            //        row2["CATEGORY_ID"] = Cid.ToUpper();
            //        Menu_Parent.Rows.Add(row2);


            //    }
            //}
            //else if (HttpContext.Current.Session["EA"].ToString().ToUpper() == ("AllProducts////WESAUSTRALASIA////" + CatName).ToUpper())
            //{
            //    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////" + CatName;
            //    CreateYouHaveSelectAndBreadCrumb();
            //}
            //if (ReturnType == "MainCategory")
            //    return ((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["MainCategory"];
            //else if (ReturnType == "SubCategory")
            //    return ((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["SubCategory"];
            //else if (ReturnType == "Brand")
            //    return ((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"];
            //else
            //    return null;

            return GetMainMenuClickDetailJson(Cid, ReturnType,true );
             
        }

        public DataTable GetMainMenuClickDetailJson(string Cid, string ReturnType,bool chkExists)
        {

            IRemoteEasyAsk ea = null;
            IOptions opts = null;
            INavigateResults res = null;
            System.IO.FileInfo Fil = null;
            System.IO.FileInfo Fil1 = null;
            try
            {

                Boolean blnGetData = false;
                DataSet Attds = new DataSet();
                string CatName = string.Empty;
                DataTable tmptbl = new DataTable();




                if (HttpContext.Current.Session["MainMenuClick"] == null)
                {
                    blnGetData = true;
                }
                else if (((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["ParentCategory"].Rows[0]["CATEGORY_ID"].ToString().ToUpper() != Cid.ToUpper())
                {
                    blnGetData = true;
                }



                tmptbl = GetCategoryAndBrand("MainCategory").Tables[0].Select("CATEGORY_ID='" + Cid + "'").CopyToDataTable();
                if (tmptbl != null && tmptbl.Rows.Count > 0)
                {
                    CatName = tmptbl.Rows[0]["CATEGORY_NAME"].ToString();
                }

                if ((blnGetData))
                {

                    if (CatName != string.Empty)
                    {





                        Fil = new System.IO.FileInfo(strxml + "\\" + Cid + ".xml");
                        Fil1 = new System.IO.FileInfo(strxml + "\\" + Cid + "_Att.xml");
                        if ((Fil.Exists) && (Fil1.Exists) && (chkExists))
                        {
                           // objErrorhandler.CreateLog_new("Categoryinside xml" + Cid);   
                            Menu_Category.ReadXml(strxml + "\\" + Cid + ".xml");
                            Attds.ReadXml(strxml + "\\" + Cid + "_Att.xml");
                            HttpContext.Current.Session["EA_URL"] = HttpContext.Current.Session["EA"];
                            HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////" + CatName;
                           // CreateYouHaveSelectAndBreadCrumb(false);

                            HttpContext.Current.Session["MainMenuClick"] = Menu_Category;
                            HttpContext.Current.Session["Category_Attributes"] = Attds;


                        }
                        else
                        {
                           // objErrorhandler.CreateLog_new("Categoryinside Easyask" + Cid);   
                             ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WesCatBrandDictionary);
                             opts = ea.getOptions();
                            opts.setResultsPerPage("1");
                            opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);
                            opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
                            opts.setSubCategories(true);
                            opts.setNavigateHierarchy(false);
                            opts.setReturnSKUs(false);



                             res = ea.userBreadCrumbClick("AllProducts////WESAUSTRALASIA////" + CatName);
                            String path = res.getCatPathJson();
                            HttpContext.Current.Session["EA_URL"] = HttpContext.Current.Session["EA"];
                            HttpContext.Current.Session["EA"] = res.getCatPathJson();

                           // CreateYouHaveSelectAndBreadCrumb(false);


                            BuildMainAndSubCategoryTableJson(res, CatName, Cid, res.GetDBAdvisor());

                            DataRow row2 = Menu_Parent.NewRow();
                            row2["CATEGORY_ID"] = Cid.ToUpper();
                            Menu_Parent.Rows.Add(row2);

                            Menu_Category.WriteXml(strxml + "\\" + Cid + ".xml");
                            if (HttpContext.Current.Session["Category_Attributes"] != null)
                            {
                                Attds = (DataSet)HttpContext.Current.Session["Category_Attributes"];
                                Attds.WriteXml(strxml + "\\" + Cid + "_Att.xml");
                            }
                        }
                    }
                }
                else if (HttpContext.Current.Session["EA"].ToString().ToUpper() == ("AllProducts////WESAUSTRALASIA////" + CatName).ToUpper())
                {
                    //objErrorhandler.CreateLog_new("blnGetData" + CatName);   
                    HttpContext.Current.Session["EA_URL"] = HttpContext.Current.Session["EA"];
                    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////" + CatName;
                    CreateYouHaveSelectAndBreadCrumb(false);
                }
                if (ReturnType == "MainCategory")
                    return ((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["MainCategory"];
                else if (ReturnType == "SubCategory")
                    return ((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["SubCategory"];
                else if (ReturnType == "Brand")
                    return ((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"];
                else
                    return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                ea = null;
                opts = null;
                res = null;
                Fil1 = null;
                Fil = null;
            }

        }

        public DataTable GetSubMenuClickDetail(string Cid, string ReturnType)
        {
            //Boolean blnGetData = false;
            //if (HttpContext.Current.Session["MainMenuClick"] == null)
            //{
            //    blnGetData = true;
            //}
            //else if (((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["ParentCategory"].Rows[0]["CATEGORY_ID"].ToString().ToUpper() != Cid.ToUpper())
            //{
            //    blnGetData = true;
            //}
            //DataTable tmptbl = new DataTable();

            //string CatName = "";
            //tmptbl = GetCategoryAndBrand("MainCategory").Tables[0].Select("CATEGORY_ID='" + Cid + "'").CopyToDataTable();
            //if (tmptbl != null && tmptbl.Rows.Count > 0)
            //{
            //    CatName = tmptbl.Rows[0]["CATEGORY_NAME"].ToString();
            //}

            //if (blnGetData == true)
            //{

            //    if (CatName != "")
            //    {
            //        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WesCatBrandDictionary);
            //        IOptions opts = ea.getOptions();
            //        opts.setResultsPerPage("1");
            //        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);
            //        opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
            //        opts.setSubCategories(true);
            //        opts.setNavigateHierarchy(false);
            //        opts.setReturnSKUs(false);

            //        INavigateResults res = ea.userBreadCrumbClick("AllProducts////WESAUSTRALASIA////" + CatName);
            //        String path = res.getCatPath();
            //        HttpContext.Current.Session["EA"] = res.getCatPath();
            //        CreateYouHaveSelectAndBreadCrumb();

            //        IList<INavigateCategory> list = res.getDetailedCategories();
            //        //IList<INavigateCategory> li = res.getDetailedAttributeValues(  ();
            //        //For Brand Table.
            //        BuildMainAndSubCategoryTable(list, res, CatName, Cid);

            //        DataRow row2 = Menu_Parent.NewRow();
            //        row2["CATEGORY_ID"] = Cid.ToUpper();
            //        Menu_Parent.Rows.Add(row2);


            //    }
            //}
            //else if (HttpContext.Current.Session["EA"].ToString().ToUpper() == ("AllProducts////WESAUSTRALASIA////" + CatName).ToUpper())
            //{
            //    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////" + CatName;
            //    CreateYouHaveSelectAndBreadCrumb();
            //}
            //if (ReturnType == "MainCategory")
            //    return ((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["MainCategory"];
            //else if (ReturnType == "SubCategory")
            //    return ((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["SubCategory"];
            //else if (ReturnType == "Brand")
            //    return ((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"];
            //else
            //    return null;

            return GetSubMenuClickDetailJson(Cid, ReturnType, true);

        }

        public DataTable GetSubMenuClickDetailJson(string Cid, string ReturnType, bool chkExists)
        {
            System.IO.FileInfo Fil = null;
            System.IO.FileInfo Fil1 = null;
            try
            {
                Boolean blnGetData = false;
                DataSet Attds = new DataSet();
                string CatName = string.Empty;
                DataTable tmptbl = new DataTable();




                if (HttpContext.Current.Session["MainMenuClick"] == null)
                {
                    blnGetData = true;
                }
                else if (((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["ParentCategory"].Rows[0]["CATEGORY_ID"].ToString().ToUpper() != Cid.ToUpper())
                {
                    blnGetData = true;
                }



                tmptbl = GetCategoryAndBrand("SubCategory").Tables[0].Select("CATEGORY_ID='" + Cid + "'").CopyToDataTable();
                if (tmptbl != null && tmptbl.Rows.Count > 0)
                {
                    CatName = tmptbl.Rows[0]["CATEGORY_NAME"].ToString();
                }

                if (blnGetData == true)
                {

                    if (CatName != "")
                    {





                        Fil = new System.IO.FileInfo(strxml + "\\" + Cid + ".xml");
                        Fil1 = new System.IO.FileInfo(strxml + "\\" + Cid + "_Att.xml");
                        if (Fil.Exists == true && Fil1.Exists == true && chkExists == true)
                        {

                            Menu_Category.ReadXml(strxml + "\\" + Cid + ".xml");
                            Attds.ReadXml(strxml + "\\" + Cid + "_Att.xml");
                            HttpContext.Current.Session["EA_URL"] = HttpContext.Current.Session["EA"];
                            HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////Cellular Accessories////" + CatName;
                            CreateYouHaveSelectAndBreadCrumb(false);

                            HttpContext.Current.Session["MainMenuClick"] = Menu_Category;
                            HttpContext.Current.Session["Category_Attributes"] = Attds;

                        }
                        else
                        {
                            IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WesCatBrandDictionary);
                            IOptions opts = ea.getOptions();
                            opts.setResultsPerPage("1");
                            opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);
                            opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
                            opts.setSubCategories(true);
                            opts.setNavigateHierarchy(false);
                            opts.setReturnSKUs(false);



                            INavigateResults res = ea.userBreadCrumbClick("AllProducts////WESAUSTRALASIA////Cellular Accessories////" + CatName);
                            String path = res.getCatPathJson();
                            HttpContext.Current.Session["EA_URL"] = HttpContext.Current.Session["EA"];
                            HttpContext.Current.Session["EA"] = res.getCatPathJson();
                            CreateYouHaveSelectAndBreadCrumb(false);


                            BuildMainAndSubCategoryTableJson(res, CatName, Cid, res.GetDBAdvisor());

                            DataRow row2 = Menu_Parent.NewRow();
                            row2["CATEGORY_ID"] = Cid.ToUpper();
                            Menu_Parent.Rows.Add(row2);

                            Menu_Category.WriteXml(strxml + "\\" + Cid + ".xml");
                            if (HttpContext.Current.Session["Category_Attributes"] != null)
                            {
                                Attds = (DataSet)HttpContext.Current.Session["Category_Attributes"];
                                Attds.WriteXml(strxml + "\\" + Cid + "_Att.xml");
                            }
                        }
                    }
                }
                else if (HttpContext.Current.Session["EA"].ToString().ToUpper() == ("AllProducts////WESAUSTRALASIA////Cellular Accessories////" + CatName).ToUpper())
                {
                    HttpContext.Current.Session["EA_URL"] = HttpContext.Current.Session["EA"];
                    HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////Cellular Accessories////" + CatName;
                    CreateYouHaveSelectAndBreadCrumb(false);
                }
                if (ReturnType == "MainCategory")
                    return ((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["MainCategory"];
                else if (ReturnType == "SubCategory")
                    return ((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["SubCategory"];
                else if (ReturnType == "Brand")
                    return ((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"];
                else
                    return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                Fil = null;
                Fil1 = null; 

            }
        }

        private void BuildMainAndSubCategoryTable( IList<INavigateCategory> list,INavigateResults res,string CatName,string CatId)
        {
            Menu_Category.Tables.Add(Menu_Parent);
            Menu_Parent.Columns.Add("CATEGORY_ID", typeof(string));
            //For Main_Category Table.
            Menu_Category.Tables.Add(Menu_Main_Category);
            Menu_Main_Category.Columns.Add("CATEGORY_NAME", typeof(string));
            Menu_Main_Category.Columns.Add("CATEGORY_ID", typeof(string));
            Menu_Main_Category.Columns.Add("PARENT_CATEGORY_NAME", typeof(string));
            Menu_Main_Category.Columns.Add("PARENT_CATEGORY_ID", typeof(string));
            Menu_Main_Category.Columns.Add("SHORT_DESC", typeof(string));
            Menu_Main_Category.Columns.Add("IMAGE_FILE", typeof(string));
            Menu_Main_Category.Columns.Add("IMAGE_FILE2", typeof(string));
            Menu_Main_Category.Columns.Add("IMAGE_NAME2", typeof(string));
            Menu_Main_Category.Columns.Add("IMAGE_NAME", typeof(string));
            Menu_Main_Category.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
            Menu_Main_Category.Columns.Add("EA_PATH", typeof(string));
            // Sub_Category
            Menu_Category.Tables.Add(Menu_Sub_Category);
            Menu_Sub_Category.Columns.Add("PARENT_CATEGORY_ID", typeof(string));
            Menu_Sub_Category.Columns.Add("PARENT_CATEGORY_NAME", typeof(string));
            Menu_Sub_Category.Columns.Add("CATEGORY_NAME", typeof(string));
            Menu_Sub_Category.Columns.Add("CATEGORY_ID", typeof(string));
            Menu_Sub_Category.Columns.Add("SHORT_DESC", typeof(string));
            Menu_Sub_Category.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
            Menu_Sub_Category.Columns.Add("EA_PATH", typeof(string));
            //For Brand Table.
            Menu_Category.Tables.Add(Menu_Brand);
            Menu_Brand.Columns.Add("TOSUITE_BRAND", typeof(string));
            Menu_Brand.Columns.Add("EA_PATH", typeof(string));



            try
            {
                foreach (INavigateCategory item in list)
                {
                    DataRow row = Menu_Main_Category.NewRow();
                    row["CATEGORY_NAME"] = item.getName();
                    IList<string> li = item.getIDs();
                    row["CATEGORY_ID"] = li[0].ToString().Substring(2);
                    row["PARENT_CATEGORY_Name"] = CatName;
                    row["PARENT_CATEGORY_ID"] = CatId.ToString();

                    row["SHORT_DESC"] = string.Empty;
                    row["IMAGE_FILE"] = string.Empty;
                    row["IMAGE_FILE2"] = string.Empty;
                    row["CUSTOM_NUM_FIELD3"] = "2";
                    row["EA_PATH"] = HttpContext.Current.Session["EA"].ToString();

                    IList<INavigateCategory> SubCat_List = item.getSubCategories();
                    foreach (INavigateCategory item1 in SubCat_List)
                    {
                        DataRow row1 = Menu_Sub_Category.NewRow();
                        row1["CATEGORY_NAME"] = item1.getName();
                        IList<string> SUB_CATEGORY_ID = item1.getIDs();
                        row1["CATEGORY_ID"] = SUB_CATEGORY_ID[0].ToString().Substring(2);

                        row1["PARENT_CATEGORY_NAME"] = item.getName();
                        row1["PARENT_CATEGORY_ID"] = li[0].ToString().Substring(2);
                        row1["SHORT_DESC"] = string.Empty;
                        row1["CUSTOM_NUM_FIELD3"] = "2";
                        row1["EA_PATH"] = HttpContext.Current.Session["EA"].ToString() + "////" + item.getName();
                        Menu_Sub_Category.Rows.Add(row1);
                    }
                    Menu_Main_Category.Rows.Add(row);
                }
                IList<INavigateAttribute> Brand_list = res.getDetailedAttributeValues("Brand");
                foreach (INavigateAttribute item in Brand_list)
                {
                    DataRow row = Menu_Brand.NewRow();
                    row["TOSUITE_BRAND"] = item.getValue();
                    row["EA_PATH"] = HttpContext.Current.Session["EA"].ToString() + "////AttribSelect=Brand='" + item.getValue().ToString() + "'";
                    Menu_Brand.Rows.Add(row);
                }
                HttpContext.Current.Session["MainMenuClick"] = Menu_Category;
                IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
                HttpContext.Current.Session["Category_Attributes"] = GetCategoryAttribute(Attributes, res, "", "");

                GetMainMenuClickDBCategoryDetails();

            }
            catch (Exception)
            {
            }
        }
        private void BuildMainAndSubCategoryTableJson(INavigateResults res, string CatName, string CatId, DataSet ds)
        {
            Menu_Category = new DataSet();
    

             Menu_Parent = new DataTable("ParentCategory");
             Menu_Main_Category = new DataTable("MainCategory");
             Menu_Sub_Category = new DataTable("SubCategory");
             Menu_Brand = new DataTable("Brand");

            Menu_Category.Tables.Add(Menu_Parent);
            Menu_Parent.Columns.Add("CATEGORY_ID", typeof(string));
            //For Main_Category Table.
            Menu_Category.Tables.Add(Menu_Main_Category);
            Menu_Main_Category.Columns.Add("CATEGORY_NAME", typeof(string));
            Menu_Main_Category.Columns.Add("CATEGORY_ID", typeof(string));
            Menu_Main_Category.Columns.Add("PARENT_CATEGORY_NAME", typeof(string));
            Menu_Main_Category.Columns.Add("PARENT_CATEGORY_ID", typeof(string));
            Menu_Main_Category.Columns.Add("SHORT_DESC", typeof(string));
            Menu_Main_Category.Columns.Add("IMAGE_FILE", typeof(string));
            Menu_Main_Category.Columns.Add("IMAGE_FILE2", typeof(string));
            Menu_Main_Category.Columns.Add("IMAGE_NAME2", typeof(string));
            Menu_Main_Category.Columns.Add("IMAGE_NAME", typeof(string));
            Menu_Main_Category.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
            Menu_Main_Category.Columns.Add("EA_PATH", typeof(string));
            // Sub_Category
            //To check Indu
            Menu_Category.Tables.Add(Menu_Sub_Category);
            Menu_Sub_Category.Columns.Add("PARENT_CATEGORY_ID", typeof(string));
            Menu_Sub_Category.Columns.Add("PARENT_CATEGORY_NAME", typeof(string));
            Menu_Sub_Category.Columns.Add("CATEGORY_NAME", typeof(string));
            Menu_Sub_Category.Columns.Add("CATEGORY_ID", typeof(string));
            Menu_Sub_Category.Columns.Add("SHORT_DESC", typeof(string));
            Menu_Sub_Category.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
            Menu_Sub_Category.Columns.Add("EA_PATH", typeof(string));
            ////For Brand Table.
            //Menu_Category.Tables.Add(Menu_Brand);
            //Menu_Brand.Columns.Add("TOSUITE_BRAND", typeof(string));
            //Menu_Brand.Columns.Add("EA_PATH", typeof(string));

            string catid = string.Empty;
            DataTable maintb = new DataTable();
            string image_string = string.Empty;
            string shortdesc = string.Empty;
            int custnum = 0;
            StrSql = "";
            string CatIds = string.Empty;

            
            //DataSet ds = new DataSet();

            //ds.ReadXml(strxml+"\\ds.xml"); 
            //ds = res.GetDBAdvisor();
            DataTable subdt = null;
            if (CatId.ToUpper().Contains("SPF-"))
            {
                 subdt = objCategoryServices.GetSubCategories(2);
            }
            double cnt = 0;
            int cntchk = 0;
            string sttr = string.Empty;

            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables["CategoryList"] != null && ds.Tables["CategoryList"].Rows.Count > 0)
                {


                    foreach (DataRow Dr in ds.Tables["CategoryList"].Rows)
                    {
                        catid = Dr["ids"].ToString().Split(':')[1].ToString();
                        CatIds = CatIds + "'" + catid + "',";
                    }
                    if (CatIds != "")
                        CatIds = CatIds.Substring(0, CatIds.Length - 1) + "";

                    maintb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, CatIds, "GET_CATEGORY", HelperDB.ReturnType.RTTable);





                    foreach (DataRow item in ds.Tables["CategoryList"].Rows)
                    {
                        catid = item["ids"].ToString().Split(':')[1].ToString();

                        DataRow row = Menu_Main_Category.NewRow();
                        row["CATEGORY_NAME"] = item["name"].ToString();
                        row["CATEGORY_ID"] = catid;
                        row["PARENT_CATEGORY_Name"] = CatName;
                        row["PARENT_CATEGORY_ID"] = CatId.ToString();
                        row["SHORT_DESC"] = string.Empty;
                        row["IMAGE_FILE"] = string.Empty;
                        row["IMAGE_FILE2"] = string.Empty;
                        row["CUSTOM_NUM_FIELD3"] = "2";
                        row["EA_PATH"] = HttpContext.Current.Session["EA"].ToString();


                        //row["URL_RW_PATH"] = objhelper.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + row["CATEGORY_NAME"].ToString(), "ct.aspx", true, "");
                        //sttr = row["CATEGORY_NAME"].ToString();
                        //int indx = sttr.IndexOf(" ", 7);
                        //if (indx >= 7)
                        //    sttr = sttr.Substring(0, indx) + "<br/>" + sttr.Substring(indx + 1);
                        //else if (sttr.Equals("VCR COMPONENTS"))
                        //    sttr = "VCR<br/>COMPONENTS";
                        //row["CATEGORY_NAME_TOP"] = sttr;


                        //cntchk = 0;


                        image_string = "";
                        shortdesc = "";
                        custnum = 0;
                        if (maintb != null && maintb.Rows.Count > 0)
                        {
                            DataRow[] mdrs = maintb.Select("CATEGORY_ID='" + catid + "'");
                            if (mdrs.Length > 0)
                            {
                                image_string = mdrs[0]["IMAGE_FILE"].ToString();
                                if (image_string != "")
                                    image_string = objhelper.SetImageFolderPath(image_string.Replace("\\", "/"), "_Images", "_th");
                                shortdesc = mdrs[0]["SHORT_DESC"].ToString();
                                custnum = ((int)float.Parse(mdrs[0]["CUSTOM_NUM_FIELD3"].ToString()));
                                row["SHORT_DESC"] = shortdesc;
                                row["IMAGE_FILE"] = image_string;// Dt.Rows[0]["IMAGE_FILE"];
                                row["IMAGE_FILE2"] = mdrs[0]["IMAGE_FILE2"];
                                row["IMAGE_NAME"] = mdrs[0]["IMAGE_NAME"];
                                row["IMAGE_NAME2"] = mdrs[0]["IMAGE_NAME2"];
                              
                                row["CUSTOM_NUM_FIELD3"] = custnum;
                            }
                        }

                        if (subdt != null && subdt.Rows.Count > 0)
                        {
                            DataRow[] drs = subdt.Select("PARENT_CATEGORY='" + catid + "' and Publish=1");

                            if (drs.Length > 0)
                            {
                                //row["SUB_COUNT"] = drs.Length;
                                //cnt = drs.Length;
                                //if (drs.Length > 6)
                                //{
                                //    cnt = 6;
                                //}

                                foreach (DataRow item1 in drs)
                                {
                                    DataRow row1 = Menu_Sub_Category.NewRow();
                                    row1["CATEGORY_NAME"] = item1["CATEGORY_NAME"].ToString();
                                    row1["CATEGORY_ID"] = item1["CATEGORY_ID"].ToString();
                                    row1["PARENT_CATEGORY_NAME"] = item["name"].ToString();
                                    row1["PARENT_CATEGORY_ID"] = catid;
                                    row1["SHORT_DESC"] = string.Empty;
                                    row1["CUSTOM_NUM_FIELD3"] = custnum;
                                    row1["EA_PATH"] = HttpContext.Current.Session["EA"].ToString() + "////" + item["name"].ToString();

                                    //cntchk = cntchk + 1;
                                    //row1["URL_RW_PATH"] = objhelper.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + row1["EA_PATH"].ToString() + "////" + row1["TBT_PARENT_CATEGORY_NAME"].ToString() + "////" + row1["CATEGORY_NAME"].ToString(), "pl.aspx", true, "");
                                    //if (cntchk <= cnt)
                                    //{
                                    //    row1["PART1"] = "1";
                                    //}
                                    //else
                                    //    row1["PART1"] = "2";

                                    Menu_Sub_Category.Rows.Add(row1);
                                }
                            }
                        }
                        Menu_Main_Category.Rows.Add(row);
                    }
                }


                //if (ds != null && ds.Tables.Count > 0 && ds.Tables["attribute"] != null && ds.Tables["attribute"].Rows.Count > 0)
                //{
                //    DataRow[] brandrow = ds.Tables["attribute"].Select("name='brand'");
                //    if (brandrow.Length > 0)
                //    {
                //        DataRow[] brandrowvalue = ds.Tables["attributevaluelist"].Select("attribute_id=" + brandrow[0]["attribute_id"]);
                //        if (brandrowvalue.Length > 0)
                //        {
                //            foreach (DataRow item in brandrowvalue)
                //            {
                //                DataRow row = Menu_Brand.NewRow();
                //                row["TOSUITE_BRAND"] = item["attributevalue"].ToString();
                //                row["EA_PATH"] = HttpContext.Current.Session["EA"].ToString() + "////AttribSelect=Brand='" + item["attributevalue"].ToString() + "'";
                //                Menu_Brand.Rows.Add(row);
                //            }
                //        }
                //    }

                //}
               
                HttpContext.Current.Session["MainMenuClick"] = Menu_Category;
                
                HttpContext.Current.Session["Category_Attributes"] = GetCategoryAttributeJson(res, "", "", ds);

              
            }
            catch (Exception)
            {
            }
        }
        private void GetBrandlisttoxmldata(IList<INavigateCategory> list, INavigateResults res, string CatName, string CatId)
        {
            DataSet Menu_Category_xml = new DataSet();
            DataTable Menu_Brand_xml = new DataTable("Brand");
            //For Brand Table.
            Menu_Category_xml.Tables.Add(Menu_Brand_xml);
            Menu_Brand_xml.Columns.Add("TOSUITE_BRAND", typeof(string));
            try
            {
               IList<INavigateAttribute> Brand_list = res.getDetailedAttributeValues("Brand");
                foreach (INavigateAttribute item in Brand_list)
                {
                    DataRow row = Menu_Brand_xml.NewRow();
                    row["TOSUITE_BRAND"] = item.getValue();
                    //row["EA_PATH"] = HttpContext.Current.Session["EA"].ToString() + "////AttribSelect=Brand='" + item.getValue().ToString() + "'";
                    Menu_Brand_xml.Rows.Add(row);
                }
                HttpContext.Current.Session["brand_xml"] = Menu_Brand_xml;
                IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
                //HttpContext.Current.Session["Category_Attributes"] = GetCategoryAttribute(Attributes, res, "", "");

              
            }
            catch (Exception)
            {
            }
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE CATEGORY DETAILS WHILE CLICK MAIN MENU  ***/
        /********************************************************************************/
        public void GetMainMenuClickDBCategoryDetails()
        {
            DataTable Sqltb = new DataTable();
            // StrSql = "Select CATEGORY_ID,SHORT_DESC,IMAGE_FILE,IMAGE_FILE2,isnull(CUSTOM_NUM_FIELD3,0) as CUSTOM_NUM_FIELD3 from tb_Category where Category_ID in (";
            StrSql = "";
            string CatIds = string.Empty;
            foreach (DataRow Dr in Menu_Category.Tables[1].Rows)
            {
                CatIds = CatIds + "'" + Dr["CATEGORY_ID"].ToString() + "',";
            }
            if (CatIds != "")
                CatIds = CatIds.Substring(0, CatIds.Length - 1) + "";

            //Sqltb = objhelper.GetDataTable(StrSql);
            Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, CatIds, "GET_CATEGORY", HelperDB.ReturnType.RTTable);

            if (Sqltb != null)
            {

                foreach (DataRow Dr in Menu_Category.Tables[1].Rows)
                {
                    DataRow[] row = Sqltb.Select("CATEGORY_ID='" + Dr["CATEGORY_ID"] + "'");
                    if (row.Length > 0)
                    {
                        DataTable Dt = row.CopyToDataTable();
                        Dr["SHORT_DESC"] = Dt.Rows[0]["SHORT_DESC"];
                        Dr["IMAGE_FILE"] = Dt.Rows[0]["IMAGE_FILE"];
                        Dr["IMAGE_FILE2"] = Dt.Rows[0]["IMAGE_FILE2"];
                        Dr["IMAGE_NAME"] = Dt.Rows[0]["IMAGE_NAME"];
                        Dr["IMAGE_NAME2"] = Dt.Rows[0]["IMAGE_NAME2"];

                        Dr["CUSTOM_NUM_FIELD3"] = ((int)float.Parse(Dt.Rows[0]["CUSTOM_NUM_FIELD3"].ToString()));
                        foreach (DataRow Dr1 in Menu_Category.Tables[2].Rows)
                        {
                            if (Dr1["TBT_PARENT_CATEGORY_ID"].ToString().ToUpper() == Dr["CATEGORY_ID"].ToString().ToUpper())
                            {
                                Dr1["TBT_CUSTOM_NUM_FIELD3"] = ((int)float.Parse(Dt.Rows[0]["CUSTOM_NUM_FIELD3"].ToString()));
                            }

                        }
                    }

                }

         
            }


        }
      
        

        DataSet Category_Attributes_DS = new DataSet();
        //DataSet Category_Attributes_DS_ = new DataSet();
        DataSet Category_Attributes_DS_Full = new DataSet();


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE CATEGORY ATTRIBUTE DETAILS ***/
        /********************************************************************************/
        public DataSet GetCategoryAttribute(IList<string> Attributes, INavigateResults res, string temptext,string searchString )
        {
           
            try
            {   // If any brand in ea path 
                DataSet ds = new DataSet();
                ds = null;

                if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                {
                    ds = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

                }
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 1; i <= ds.Tables[0].Rows.Count - 1; i++)
                    {
                        DataRow row = ds.Tables[0].Rows[i];
                        if (row["ItemType"].ToString().ToUpper() == "BRAND")
                            temptext = row["ItemValue"].ToString();
                    }
                }          
                    Category_Attributes_DS.Tables.Add("Category");
                    Category_Attributes_DS.Tables["Category"].Columns.Add("CATEGORY_ID", typeof(string));
                    Category_Attributes_DS.Tables["Category"].Columns.Add("Category_Name", typeof(string));
                    Category_Attributes_DS.Tables["Category"].Columns.Add("Product_Count", typeof(int));
                    Category_Attributes_DS.Tables["Category"].Columns.Add("brandvalue", typeof(string));
                    Category_Attributes_DS.Tables["Category"].Columns.Add("SearchString", typeof(string));
                    Category_Attributes_DS.Tables["Category"].Columns.Add("CUSTOM_TEXT_FIELD1", typeof(DateTime));
                IList<INavigateCategory> category = null;
                category = res.getDetailedCategories();               
                if (category.Count > 0) //For Searching Category Values
                {
                    foreach (INavigateCategory categoryItem in category)
                    {
                        DataRow row = Category_Attributes_DS.Tables["Category"].NewRow();
                        IList<string> Id = categoryItem.getIDs();
                        row["CATEGORY_ID"] = Id[0].ToString().Substring(2);
                        row["Category_Name"] = categoryItem.getName();
                        row["Product_Count"] = categoryItem.getProductCount();
                        row["brandvalue"] = temptext;
                        row["SearchString"] = searchString;

                        try
                        {
                            string sSQL = string.Empty;
                            sSQL = "Exec Get_Custom_Text_Field '" + row["CATEGORY_ID"].ToString() + "'";
                            HelperDB objHelperDB = new HelperDB();
                            DataSet dscustom = objHelperDB.GetDataSetDB(sSQL);
                            if (dscustom != null)
                            {
                                if (dscustom.Tables[0].Rows[0][0].ToString() != "")
                                {
                                    row["CUSTOM_TEXT_FIELD1"] = DateTime.Parse(dscustom.Tables[0].Rows[0][0].ToString());
                                }
                                else
                                {
                                    row["CUSTOM_TEXT_FIELD1"] = DateTime.Now.AddDays(-356);
                                }
                            }


                            // I++;
                        }
                        catch (Exception ex)
                        {
                            row["CUSTOM_TEXT_FIELD1"] = DateTime.Now.AddDays(-356);
                        }
                        Category_Attributes_DS.Tables["Category"].Rows.Add(row);
                    }
                }
             
               
                for (int i = 0; i < Attributes.Count; i++)
                {
                    String attrName = (String)Attributes[i];

                    if (!attrName.Contains("Long Description")) //For do not display Long Description
                    {
                        Category_Attributes_DS.Tables.Add(attrName);
                        Category_Attributes_DS.Tables[i+1].Columns.Add(attrName, typeof(string));
                        Category_Attributes_DS.Tables[i+1].Columns.Add("Product_Count", typeof(int));
                        Category_Attributes_DS.Tables[i + 1].Columns.Add("brandvalue", typeof(string)); //  Store Actual EA brand Value
                        Category_Attributes_DS.Tables[i + 1].Columns.Add("SearchString", typeof(string));
                        IList<INavigateAttribute> AttributeValue = res.getDetailedAttributeValues(attrName, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
                        foreach (INavigateAttribute AttributeItem in AttributeValue)
                        {
                            DataRow row = Category_Attributes_DS.Tables[i+1].NewRow();
                            if (attrName.Equals("Model"))//For Model Name will split
                            {
                                
                               // if (AttributeItem.getValue().Contains(temptext))
                                if(AttributeItem.getValue().ToLower().Contains(temptext.ToLower()))
                                {
                                    string[] model = AttributeItem.getValue().Split(':');
                                    if (model[0].ToString().ToLower().Contains(temptext.ToLower()))
                                    {
                                        //if (temptext == "" && HttpContext.Current.Session["Brand"] == null || HttpContext.Current.Session["Brand"] == string.Empty)
                                        //{
                                        //    HttpContext.Current.Session["Brand"] = model[0].ToString();
                                        //    temptext = model[0].ToString();
                                        //}
                                        row[0] = model[1].ToString();
                                        row[1] = AttributeItem.getProductCount();
                                        if (temptext == "")
                                            row["brandvalue"] = model[0].ToString();
                                        else
                                        row["brandvalue"] = temptext;

                                        row["SearchString"] = searchString;
                                        row["URL_RW_PATH"] = objhelper.SimpleURL_Str( row["brandvalue"].ToString(), "ct.aspx",false);   
                                        Category_Attributes_DS.Tables[i+1].Rows.Add(row);
                                    }
                                }
                            }
                            else
                            {
                                row[0] = AttributeItem.getValue();
                                row[1] = AttributeItem.getProductCount();
                                row["brandvalue"] = "";
                                row["SearchString"] = searchString;
                                row["URL_RW_PATH"] = objhelper.SimpleURL_Str( row["searchString"].ToString(), "ct.aspx",false );   
                                Category_Attributes_DS.Tables[i+1].Rows.Add(row);
                            }
                        }
                       
                    }

                }

               
            
               
            }
            catch (Exception ex)
            {
            }

            return Category_Attributes_DS;
            
        }
        public DataSet GetCategoryAttributeJson( INavigateResults res, string temptext, string searchString, DataSet Dssdv)
        {

            try
            {   // If any brand in ea path 

                string _byp = "2";
                string _parentCatID = Dssdv.Tables["categories"].Rows[0][2].ToString();
                
                if (_parentCatID.Contains(":"))
                {
                    string[] _pid = _parentCatID.Split(':');
                    _parentCatID = _pid[1]; 
                }
                DataRow[] dr = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0].Select("CATEGORY_ID='" + _parentCatID + "'");
                if (dr.Length > 0)
                {
                    _byp = dr[0]["CUSTOM_NUM_FIELD3"].ToString();
                }
                DataSet ds = new DataSet();
                ds = null;

                if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                {
                    ds = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

                }
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 1; i <= ds.Tables[0].Rows.Count - 1; i++)
                    {
                        DataRow row = ds.Tables[0].Rows[i];
                        if (row["ItemType"].ToString().ToUpper() == "BRAND")
                            temptext = row["ItemValue"].ToString();
                    }
                }

                DataSet Filterds = new DataSet();
                DataSet tempds = new DataSet();
                int inxtbl = 0;
                string url = HttpContext.Current.Request.Url.ToString().ToLower();
                if (url.Contains("mct.aspx") || url.Contains("mpl.aspx") || url.Contains("mfl.aspx") || url.Contains("mpd.aspx"))
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 2)
                    {
                        tempds = (DataSet)HttpContext.Current.Session["TOPAttributes"];

                        for (int i = 2; i <= ds.Tables[0].Rows.Count - 1; i++)
                        {
                            if (tempds != null && tempds.Tables.Count > 0)
                            {
                                DataRow row = ds.Tables[0].Rows[i];
                                inxtbl = tempds.Tables.IndexOf(row["ItemType"].ToString());
                                if (inxtbl >= 0)
                                {
                                    Filterds.Tables.Add(tempds.Tables[inxtbl].Copy());
                                    for (int j = 0; j <= Filterds.Tables[Filterds.Tables.Count - 1].Rows.Count - 1; j++)
                                    {
                                        Filterds.Tables[Filterds.Tables.Count - 1].Rows[j]["select"] = "0";
                                        if (row["ItemType"].ToString().ToLower() == "category")
                                        {
                                            if (Filterds.Tables[Filterds.Tables.Count - 1].Rows[j][1].ToString().ToLower() == row["ItemValue"].ToString().ToLower())
                                            {
                                                Filterds.Tables[Filterds.Tables.Count - 1].Rows[j]["select"] = "1";
                                            }
                                        }
                                        else
                                        {
                                            if (Filterds.Tables[Filterds.Tables.Count - 1].Rows[j][0].ToString().ToLower() == row["ItemValue"].ToString().ToLower())
                                            {
                                                Filterds.Tables[Filterds.Tables.Count - 1].Rows[j]["select"] = "1";
                                            }
                                        }


                                    }
                                }
                            }

                        }
                    }
                }

                Category_Attributes_DS = new DataSet();

                Category_Attributes_DS.Tables.Add("Category");
                Category_Attributes_DS.Tables["Category"].Columns.Add("CATEGORY_ID", typeof(string));
                Category_Attributes_DS.Tables["Category"].Columns.Add("Category_Name", typeof(string));
                Category_Attributes_DS.Tables["Category"].Columns.Add("Product_Count", typeof(int));
                Category_Attributes_DS.Tables["Category"].Columns.Add("brandvalue", typeof(string));
                Category_Attributes_DS.Tables["Category"].Columns.Add("SearchString", typeof(string));
                Category_Attributes_DS.Tables["Category"].Columns.Add("CUSTOM_TEXT_FIELD1", typeof(DateTime));
                Category_Attributes_DS.Tables["Category"].Columns.Add("select", typeof(string));
                Category_Attributes_DS.Tables["Category"].Columns.Add("eaPath", typeof(string));
              
                string CatIds = string.Empty;
                DataTable maintb = new DataTable();
                string catid = string.Empty;
                String attrName = "";
                String attrvalue = "";
                if (Dssdv != null && Dssdv.Tables.Count > 0 && Dssdv.Tables["CategoryList"] != null && Dssdv.Tables["CategoryList"].Rows.Count > 0)
                {
                    foreach (DataRow Dr in Dssdv.Tables["CategoryList"].Rows)
                    {
                        catid = Dr["ids"].ToString().Split(':')[1].ToString();
                        CatIds = CatIds + "'" + catid + "',";
                    }
                    if (CatIds != "")
                        CatIds = CatIds.Substring(0, CatIds.Length - 1) + "";

                    maintb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, CatIds, "GET_CATEGORY", HelperDB.ReturnType.RTTable);
                    foreach (DataRow item in Dssdv.Tables["CategoryList"].Rows)
                    {
                        catid = item["ids"].ToString().Split(':')[1].ToString();
                        DataRow row = Category_Attributes_DS.Tables["Category"].NewRow();

                        row["CATEGORY_ID"] = catid;
                        row["Category_Name"] =  item["name"].ToString();
                        row["Product_Count"] = item["productcount"];
                        row["brandvalue"] = temptext;
                        row["SearchString"] = searchString;
                        row["CUSTOM_TEXT_FIELD1"] = DateTime.Now.AddDays(-356);
                        row["select"] = "";
                        row["eapath"] = "";
                        if (maintb != null && maintb.Rows.Count > 0)
                        {
                            DataRow[] mdrs = maintb.Select("CATEGORY_ID='" + catid + "'");
                            if (mdrs.Length > 0)
                            {
                                if (mdrs[0]["CUSTOM_TEXT_FIELD1_DATE"].ToString() != "")
                                {
                                    row["CUSTOM_TEXT_FIELD1"] = DateTime.Parse(mdrs[0]["CUSTOM_TEXT_FIELD1_DATE"].ToString());
                                }
                                else
                                {
                                    row["CUSTOM_TEXT_FIELD1"] = DateTime.Now.AddDays(-356);
                                }
                            }
                            
                        }
                        Category_Attributes_DS.Tables["Category"].Rows.Add(row);
                           
                    }
                }
                if (Dssdv != null && Dssdv.Tables.Count > 0 && Dssdv.Tables["attribute"] != null && Dssdv.Tables["attribute"].Rows.Count > 0 && Dssdv.Tables["attributevaluelist"] != null && Dssdv.Tables["attributevaluelist"].Rows.Count > 0)
                {
                    for (int i = 0; i < Dssdv.Tables["attribute"].Rows.Count; i++)
                    {
                        attrName = (String)Dssdv.Tables["attribute"].Rows[i]["name"];
                        if (!attrName.Contains("Long Description")) //For do not display Long Description
                        {
                            Category_Attributes_DS.Tables.Add(attrName);
                            Category_Attributes_DS.Tables[i + 1].Columns.Add(attrName, typeof(string));
                            Category_Attributes_DS.Tables[i + 1].Columns.Add("Product_Count", typeof(int));
                            Category_Attributes_DS.Tables[i + 1].Columns.Add("brandvalue", typeof(string)); //  Store Actual EA brand Value
                            Category_Attributes_DS.Tables[i + 1].Columns.Add("SearchString", typeof(string));
                            Category_Attributes_DS.Tables[i + 1].Columns.Add("select", typeof(string));
                            Category_Attributes_DS.Tables[i + 1].Columns.Add("eapath", typeof(string));
                            Category_Attributes_DS.Tables[i + 1].Columns.Add("URL_RW_PATH", typeof(string));
                            DataRow[] attvalues = Dssdv.Tables["attributevaluelist"].Select("attribute_id=" + Dssdv.Tables["attribute"].Rows[i]["attribute_id"]);
                            foreach (DataRow  AttributeItem in attvalues)
                            {
                                DataRow row = Category_Attributes_DS.Tables[i + 1].NewRow();
                                attrvalue = AttributeItem["attributevalue"].ToString();
                           
                                if ((attrName.ToLower().Contains("model")) && (_byp =="2"))//For Model Name will split
                                {

                                    attrvalue = AttributeItem["attributevalue"].ToString();
                                    // if (AttributeItem.getValue().Contains(temptext))
                                    if (attrvalue.ToLower().Contains(temptext.ToLower()))
                                    {
                                       string[] model = attrvalue.Split(':');
                                        if (model[0].ToString().ToLower().Contains(temptext.ToLower()))
                                        {

                                            row[0] = attrvalue;
                                            row[1] = AttributeItem["productcount"];
                                            if (temptext == "")
                                                row["brandvalue"] = model[0].ToString();
                                            else
                                                row["brandvalue"] = temptext;

                                            row["SearchString"] = searchString;
                                            row["select"] = "";
                                            row["eapath"] = attrvalue; ;
                                            row["URL_RW_PATH"] = objhelper.SimpleURL_Str(attrvalue, "ct.aspx", false);   
                                            Category_Attributes_DS.Tables[i + 1].Rows.Add(row);
                                        }
                                    }
                                }
                                else if (!attrName.ToLower().Contains("model"))
                                {
                                    row[0] = attrvalue;
                                    row[1] = AttributeItem["productcount"];
                                    if (attrName.ToLower().Contains("brand"))
                                    {
                                        row["brandvalue"] = attrvalue;
                                    }
                                    else
                                    {
                                        row["brandvalue"] = "";
                                    }
                                    row["SearchString"] = searchString;
                                    row["select"] = "" ;
                                    row["eapath"] = attrvalue; 
                                    row["URL_RW_PATH"] = objhelper.SimpleURL_Str( attrvalue, "ct.aspx",false);   
                                    Category_Attributes_DS.Tables[i + 1].Rows.Add(row);
                                }
                            }
                        }
                    }

                }


                // for micro site
                if (url.Contains("mct.aspx") || url.Contains("mpl.aspx") || url.Contains("mfl.aspx") || url.Contains("mpd.aspx"))
                {

                    for (int i = 0; i <= Category_Attributes_DS.Tables.Count - 1; i++)
                    {
                        if (Filterds.Tables.IndexOf(Category_Attributes_DS.Tables[i].TableName) == -1)
                            Filterds.Tables.Add(Category_Attributes_DS.Tables[i].Copy());
                    }
                }
                // for micro site

                HttpContext.Current.Session["TOPAttributes"] = Filterds;

            }
            catch (Exception ex)
            {
            }

            

            return Category_Attributes_DS;
        }


          #endregion



        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT PRICE CODE ***/
        /********************************************************************************/
        public int GetPriceCode()
        {
            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            if( userid=="" )
                userid =  ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

            return objhelperDb.GetPriceCode(userid);
            
        }


        public string GetBreadCrumb_Simple(string templatepath)
        {
            
            try
            {
                StringTemplateGroup _stg_records = null;
                StringTemplateGroup _stg_records1 = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_records1 = null;

                string breadcrumb = string.Empty;
                string stemplatepath = templatepath;


                string istsm = string.Empty;
                string istsb = string.Empty;
                _stg_records = new StringTemplateGroup("Cell", stemplatepath);
                _stg_records1 = new StringTemplateGroup("main", stemplatepath);

                DataSet ds = new DataSet();
                ds = null;
                int dscount1 = 0;
                int dscount = 0;

                if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                {
                    ds = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

                }
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    dscount = ds.Tables[0].Rows.Count - 1;
                    _stmpl_records1 = _stg_records1.GetInstanceOf("BreadCrumb\\home");
                    string breadcrumEA = "//// //// ////";
                    string RemovebreadcrumEA = "//// //// ////";
                    string Familyname = string.Empty;
                    string _fid = string.Empty;
                    bool familybrandmodelset = false;
                    int i;
                    int a = 0;
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    string bccell = "BreadCrumb\\cell";

                    if (HttpContext.Current.Session["YHSCell_dt"] != null)
                    {
                        DataTable dt = (DataTable)HttpContext.Current.Session["YHSCell_dt"];

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (i == dt.Rows.Count-1)
                                _stmpl_records = _stg_records.GetInstanceOf("BreadCrumb\\lastcell");
                            else
                                _stmpl_records = _stg_records.GetInstanceOf(bccell);

                            if (i == dt.Rows.Count - 1)
                            {
                                if(dt.Rows[i]["TBW_ITEM_TYPE"].ToString() == "Item Product")
                                    _stmpl_records.SetAttribute("TBT_LINKNAME", dt.Rows[i]["TBW_ITEM_NAME"].ToString());
                                else
                                   _stmpl_records.SetAttribute("TBT_LINKNAME", textInfo.ToTitleCase(dt.Rows[i]["TBW_ITEM_NAME"].ToString().ToLower()));
                            }
                            else
                                _stmpl_records.SetAttribute("TBT_LINKNAME", textInfo.ToTitleCase(dt.Rows[i]["TBW_ITEM_NAME"].ToString().ToLower()));

                            _stmpl_records.SetAttribute("TBT_ATTRIBUTE_TYPE", dt.Rows[i]["TBW_ITEM_TYPE"].ToString());
                            _stmpl_records.SetAttribute("TBT_REWRITEURL", dt.Rows[i]["TBT_REWRITEURL"].ToString());
                          
                            _stmpl_records.SetAttribute("lastvalue", dt.Rows[i]["lastvalue"].ToString());
                            
                            breadcrumb = breadcrumb + _stmpl_records.ToString();
                          
                            //    _stmpl_records.SetAttribute("TBT_REMOVEURL", row["RemoveUrl"]);
                            //    _stmpl_records.SetAttribute("TBT_URL", row["Url"]);
                            //    _stmpl_records.SetAttribute("TBT_EAPATH", row["EAPath"]);
                            //    _stmpl_records.SetAttribute("TBT_ATTRIBUTE_TYPE", row["ItemType"]);
                        }
                    }
                    HttpContext.Current.Session["YHSCell_dt"] = null;
                    //for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                    //{
                    //    //Modified by :indu
                    //    //foreach (DataRow row in ds.Tables[0].Rows)
                    //    //{


                    //    DataRow row = ds.Tables[0].Rows[i];
                    //    DataRow Revrow;
                    //    string Itemvalue = string.Empty;
                    //    string newpagename = string.Empty;
                    //    string newremovepagename = string.Empty;
                    //    if (i == 0)
                    //    {
                    //        Revrow = ds.Tables[0].Rows[0];
                    //    }
                    //    else
                    //    {
                    //        Revrow = ds.Tables[0].Rows[i - 1];
                    //    }
                    //    if (dscount == dscount1)
                    //        _stmpl_records = _stg_records.GetInstanceOf("BreadCrumb\\lastcell");
                    //    else

                    //        _stmpl_records = _stg_records.GetInstanceOf(bccell);
                    //    string[] PAGENAME = row["Url"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);

                    //    string rItemType = string.Empty;
                    //    rItemType = row["ItemType"].ToString().ToLower();
                    //    string rItemValue = string.Empty;
                    //    rItemValue = row["ItemValue"].ToString();
                    //    string httprequrl = string.Empty;
                    //    httprequrl = HttpContext.Current.Request.Url.ToString().ToLower();

                    //    string pagename = string.Empty;
                    //    pagename = PAGENAME[0].ToLower();
                    //    if (rItemType == "category" && i == 0)
                    //    {
                    //      //  _stmpl_records.SetAttribute("TBT_LINKNAME", rItemValue.ToUpper());
                    //        _stmpl_records.SetAttribute("TBT_LINKNAME", textInfo.ToTitleCase(rItemValue.ToLower()));
                    //        Itemvalue = HttpUtility.UrlEncode(rItemValue);
                    //        HttpContext.Current.Session["PRODUCT_CATEGORY_NAME_SES"] = rItemValue;
                    //    }

                    //    else if (rItemType == "family")
                    //    {
                    //      //  _stmpl_records.SetAttribute("TBT_LINKNAME", row["FamilyName"].ToString());
                    //        _stmpl_records.SetAttribute("TBT_LINKNAME", textInfo.ToTitleCase(row["FamilyName"].ToString().ToLower()));
                    //        Itemvalue = rItemValue + "=" + row["FamilyName"];
                    //        HttpContext.Current.Session["S_FName"] = row["FamilyName"];
                    //    }
                    //    //else if (rItemType == "brand" && pagename.Contains("ct.aspx"))
                    //    //{
                    //    //    _stmpl_records.SetAttribute("TBT_LINKNAME", rItemValue);
                    //    //    Itemvalue = rItemValue + "=" + HttpUtility.UrlEncode(rItemValue);
                    //    //}

                    //    else if (rItemType == "product")
                    //    {
                    //        if (row["ProductCode"].ToString() != "")
                    //        {
                    //            //_stmpl_records.SetAttribute("TBT_LINKNAME", row["ProductCode"].ToString());
                    //            _stmpl_records.SetAttribute("TBT_LINKNAME", textInfo.ToTitleCase(row["ProductCode"].ToString().ToLower()));
                    //            Itemvalue = rItemValue + "=" + row["ProductCode"];
                    //        }
                    //        else
                    //        {
                    //           // _stmpl_records.SetAttribute("TBT_LINKNAME", rItemValue);
                    //            _stmpl_records.SetAttribute("TBT_LINKNAME", textInfo.ToTitleCase(rItemValue.ToLower()));

                    //            Itemvalue = row["ItemType"] + "=" + HttpUtility.UrlEncode(rItemValue);
                    //        }
                    //    }
                    //    //else if ((rItemType != "category")
                    //    //    && (rItemType != "model")
                    //    //    && (rItemType != "brand")
                    //    //    && ((IsFromBrand))
                    //    //    && ((httprequrl.Contains("fl.aspx"))
                    //    //    || httprequrl.Contains("pd.aspx")))
                    //    //{
                    //    //    _stmpl_records.SetAttribute("TBT_LINKNAME", rItemValue);
                    //    //    Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(rItemValue);

                    //    //}
                    //    //else if (((rItemType != "brand")
                    //    //    && (rItemType != "model"))
                    //    //    && ((httprequrl.Contains("ct.aspx"))
                    //    //    || (httprequrl.Contains("ps.aspx"))
                    //    //    || (httprequrl.Contains("fl.aspx"))
                    //    //    || (httprequrl.Contains("pd.aspx"))))
                    //    //{
                    //    //    _stmpl_records.SetAttribute("TBT_LINKNAME", rItemValue);
                    //    //    Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(rItemValue);

                    //    //}

                    //    //else if ((rItemType == "model")

                    //    //    && (!(IsFromBrand)))
                    //    //{
                    //    //    _stmpl_records.SetAttribute("TBT_LINKNAME", rItemValue);
                    //    //    Itemvalue = row["Actualvalue"].ToString().ToLower().Replace("attribselect=", "").Replace("'", "").Replace("model = ", "model=");
                    //    //    Itemvalue = HttpUtility.UrlEncode(Itemvalue);
                    //    //}
                    //    //else if ((rItemType != "category") && (rItemType != "user search") && (!(rItemType.Contains("usersearch"))) && ((IsFromproductlist) || (IsFromps)))
                    //    //{
                    //    //    _stmpl_records.SetAttribute("TBT_LINKNAME", rItemValue);
                    //    //    Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(rItemValue);
                    //    //}
                    //    //else if (((rItemType != "brand")) && ((rItemType != "model")) && ((row["Url"].ToString().Contains("bb.aspx"))))
                    //    //{
                    //    //    _stmpl_records.SetAttribute("TBT_LINKNAME", rItemValue);
                    //    //    Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(rItemValue);

                    //    //}

                    //    else
                    //    {
                    //       // _stmpl_records.SetAttribute("TBT_LINKNAME", rItemValue.ToUpper());
                    //        _stmpl_records.SetAttribute("TBT_LINKNAME", textInfo.ToTitleCase(rItemValue.ToLower()));
                    //        Itemvalue = HttpUtility.UrlEncode(rItemValue);
                    //    }
                    //    //Itemvalue = Itemvalue.Replace("+", "||");  
                    //    // Itemvalue = HttpUtility.UrlEncode(Itemvalue);  
                    //    _stmpl_records.SetAttribute("TBT_REMOVEEAPATH", HttpUtility.UrlEncode(objsecurity.StringEnCrypt(row["RemoveEAPath"].ToString())));
                    //    _stmpl_records.SetAttribute("TBT_REMOVEURL", row["RemoveUrl"]);
                    //    _stmpl_records.SetAttribute("TBT_URL", row["Url"]);
                    //    _stmpl_records.SetAttribute("TBT_EAPATH", row["EAPath"]);
                    //    _stmpl_records.SetAttribute("TBT_ATTRIBUTE_TYPE", row["ItemType"]);

                    //    try
                    //    {
                    //        string linkname = _stmpl_records.GetAttribute("TBT_LINKNAME").ToString();
                    //        _stmpl_records.SetAttribute("TBT_LINKTITLE", linkname.Replace('"', ' '));
                    //    }
                    //    catch
                    //    { }


                    //    //modified by :indu
                    //    try
                    //    {


                    //        //TBWTemplateEngine objTempengine = new TBWTemplateEngine("", "", "");
                    //        string Itemtype = row["ItemType"].ToString().ToUpper();
                    //        string RevItemType = Revrow["ItemType"].ToString().ToUpper();


                    //        _stmpl_records.SetAttribute("TBT_REM_ATTRIBUTE_TYPE", RevItemType);

                    //        string[] catpath = ds.Tables[0].Rows[i]["EAPATH"].ToString().ToUpper().Split(new string[] { "////" }, StringSplitOptions.None);
                    //        newpagename = PAGENAME[0].ToLower();
                    //        if (breadcrumEA == "//// //// ////")
                    //        {


                    //            RemovebreadcrumEA = breadcrumEA;
                    //            breadcrumEA = breadcrumEA + Itemvalue;

                    //            //   newpagename = PAGENAME[0].ToLower();


                    //        }
                    //        else
                    //        {


                    //            RemovebreadcrumEA = breadcrumEA;

                    //            breadcrumEA = breadcrumEA + "////" + ds.Tables[0].Rows[i]["Itemvalue"].ToString();
                    //            //Itemvalue;


                    //        }
                    //        newpagename = PAGENAME[0].ToLower();
                    //        if ((pagename.Contains("pl.aspx")) && (catpath.Length >= 5))
                    //        {

                    //            a = a + 1;
                    //            //breadcrumEA = catpath[0] + "////" + catpath[1] + "////" + "wa-" + a + "////" + catpath[2] + "////" + catpath[3];
                    //            breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +
                    //           (catpath.Length >= 2 ? catpath[1] : " ") + "////" + "wa-" + a + "////" +
                    //           (catpath.Length >= 3 ? catpath[2] : " ") + "////" +
                    //              (catpath.Length >= 4 ? catpath[3] : " ");

                    //            HttpContext.Current.Session[(catpath.Length >= 4 ? catpath[3] : " ") + "wa-" + a.ToString()] = ds.Tables[0].Rows[i]["EAPATH"].ToString();


                    //        }

                    //        else if (pagename.Contains("pd.aspx"))
                    //        {

                    //            //if ((IsFromps))
                    //            //{
                    //            //    breadcrumEA = breadcrumEA.ToUpper().Replace("USERSEARCH1=", "USERSEARCH=").Replace("USER_SEARCH=", "USERSEARCH=");
                    //            //    breadcrumEA = breadcrumEA.Replace("USERSEARCH1=USERSEARCH1=", "USERSEARCH=").Replace("USER_SEARCH=USER_SEARCH=", "USERSEARCH=");
                    //            //}


                    //            if (catpath.Length == 7)
                    //            {

                    //                //breadcrumEA = catpath[0] + "////" + catpath[1] + "////" +
                    //                // catpath[5].Replace("USERSEARCH=FAMILY ID=", "") + "////" +
                    //                // catpath[6].Replace("USERSEARCH1=PROD ID=", "") +
                    //                //   "=" +
                    //                //  ds.Tables[0].Rows[i]["PRODUCTCODE"].ToString().ToUpper() +
                    //                //   "////" + catpath[2] + "////" + catpath[3] + "////" +
                    //                breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +
                    //                  (catpath.Length >= 2 ? catpath[1] : " ") + "////" +
                    //                  (catpath.Length >= 6 ? catpath[5] : " ").Replace("USERSEARCH=FAMILY ID=", "") + "////" +
                    //                     (catpath.Length >= 7 ? catpath[6] : " ").Replace("USERSEARCH1=PROD ID=", "") + "=" +
                    //                    ds.Tables[0].Rows[i]["PRODUCTCODE"].ToString().ToUpper() +
                    //                      "////" + (catpath.Length >= 3 ? catpath[2] : " ") + "////" +
                    //                     (catpath.Length >= 4 ? catpath[3] : " ") + "////" +
                    //                          Familyname;

                    //            }
                    //            else
                    //            {

                    //                //breadcrumEA = catpath[0] + "////" + catpath[1] + "////" +
                    //                //    catpath[4].Replace("USERSEARCH=FAMILY ID=", "") + "////" +
                    //                //    catpath[5].Replace("USERSEARCH1=PROD ID=", "") + "=" +
                    //                //    ds.Tables[0].Rows[i]["PRODUCTCODE"].ToString().ToUpper() + "////"
                    //                //    + catpath[2] + "////" + catpath[3] + "////" +
                    //                //           Familyname;

                    //                breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +
                    //               (catpath.Length >= 2 ? catpath[1] : " ") + "////" +
                    //             (catpath.Length >= 5 ? catpath[4] : " ").Replace("USERSEARCH=FAMILY ID=", "")
                    //             + "////" +
                    //              (catpath.Length >= 6 ? catpath[5] : " ").Replace("USERSEARCH1=PROD ID=", "") + "=" + ds.Tables[0].Rows[i]["PRODUCTCODE"].ToString().ToUpper()
                    //              + "////" + (catpath.Length >= 3 ? catpath[2] : " ") + "////" +
                    //            (catpath.Length >= 4 ? catpath[3] : " ") + "////" +
                    //                      Familyname;
                    //            }
                    //        }
                    //        else if (pagename.Contains("fl.aspx"))
                    //        {
                    //            // string[] catpath = ds.Tables[0].Rows[i] ["EAPATH"].ToString().ToUpper().Split(new string[] { "////" }, StringSplitOptions.None);



                    //            if (Itemtype == "FAMILY")
                    //            {

                    //                Familyname = ds.Tables[0].Rows[i]["FAMILYNAME"].ToString();
                    //                _fid = ds.Tables[0].Rows[i]["Itemvalue"].ToString();

                    //                //breadcrumEA = catpath[0] + "////" + catpath[1] + "////" + _fid + "////" + catpath[2] + "////" + catpath[3] + "////" +
                    //                //        Familyname;

                    //                breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////"
                    //                + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + _fid + "////" +
                    //                (catpath.Length >= 3 ? catpath[2] : " ") + "////" +
                    //               (catpath.Length >= 4 ? catpath[3] : " ") + "////" +
                    //                   Familyname;


                    //                //if (catpath[3].Contains("USERSEARCH=") == false)
                    //                //{
                                   
                    //                //}
                    //                //else
                    //                //{
                    //                //    breadcrumEA = catpath[0] + "////" + catpath[1] + "////" + catpath[2] + "////" +
                    //                //              Familyname + "-" + _fid;
                    //                //}

                    //            }
                    //            else
                    //            {

                    //                a = a + 1;
                    //                //breadcrumEA = catpath[0] + "////" + catpath[1] + "////" + "wa-" + a + "////" + _fid + "////" + catpath[2] + "////" +
                    //                //    catpath[3] + "////" + Familyname;
                    //                breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + "wa-" + a + "////" + _fid + "////"
                    //             + (catpath.Length >= 3 ? catpath[2] : " ") + "////" + (catpath.Length >= 4 ? catpath[3] : " ") + "////" +
                    //                  Familyname;
                    //                HttpContext.Current.Session[_fid + "wa-" + a.ToString()] = ds.Tables[0].Rows[i]["EAPATH"].ToString();

                    //            }


                    //        }

                    //        else if (pagename.Contains("ct.aspx"))
                    //        {

                    //            //breadcrumEA = breadcrumEA.Replace("Brand=", "");
                    //            if (Itemtype.ToLower() == "brand")
                    //            {
                    //                istsb = ds.Tables[0].Rows[i]["Itemvalue"].ToString();

                    //            }

                    //        }
                    //        else if (pagename.Contains("bb.aspx"))
                    //        {

                    //            if (catpath.Length <= 5)
                    //            {

                    //                if (Itemtype.ToLower() == "model")
                    //                {
                    //                    istsm = ds.Tables[0].Rows[i]["Itemvalue"].ToString().ToLower().Replace("model=", "");
                    //                }
                    //                breadcrumEA = breadcrumEA.ToUpper().Replace("MODEL=", "").Replace("BRAND=", "");
                    //            }
                    //            else
                    //            {
                    //                a = a + 1;
                    //                //breadcrumEA = catpath[0] + "////" + catpath[1] + "////" + "wa-" + a + "////" + catpath[2] + "////" + istsb + "////" + istsm;
                    //                breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + "wa-" + a + "////" +
                    //               (catpath.Length >= 3 ? catpath[2] : " ") + "////" + istsb + "////" + istsm;


                    //                HttpContext.Current.Session[istsm + "wa-" + a.ToString()] = ds.Tables[0].Rows[i]["EAPATH"].ToString();



                    //            }
                    //        }
                    //        else if ((pagename.Contains("ps.aspx")) || (pagename == ""))
                    //        {

                    //            if (catpath.Length > 3)
                    //            {

                    //                a = a + 1;
                    //                //breadcrumEA = catpath[0] + "////" + catpath[1] + "////" + "wa-" + a + "////" + ds.Tables[0].Rows[0]["ItemValue"].ToString();

                    //                breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +
                    //                 (catpath.Length >= 2 ? catpath[1] : " ") + "////" + "wa-" + a + "////" + ds.Tables[0].Rows[0]["ItemValue"].ToString();

                    //                HttpContext.Current.Session[ds.Tables[0].Rows[0]["ItemValue"].ToString() + "wa-" + a.ToString()] = ds.Tables[0].Rows[i]["EAPATH"].ToString();

                    //            }

                    //            // IsFromps = true;


                    //            breadcrumEA = breadcrumEA.ToUpper().Replace("USER SEARCH=", "").Replace("USERSEARCH=", "").Replace("USERSEARCH1=", "").Replace("USERSEARCH2=", "").Replace("CATEGORY=", "");
                    //            //  breadcrumEA = breadcrumEA.Replace("//// //// ////", "//// //// ////" + "UserSearch=");
                    //        }

                    //        else
                    //        {
                    //            newpagename = PAGENAME[0].ToLower();

                    //        }
                    //        //  breadcrumEA = breadcrumEA.Replace("+", "||"); 
                    //        //Cons_NewURl(_stmpl_records, breadcrumEA, PAGENAME[0].ToLower(), true, false);                     

                    //        if (newpagename == "")
                    //        {
                    //            newpagename = "ps.aspx";
                    //        }
                    //        string newurl = string.Empty;
                    //        newurl = objhelper.SimpleURL(_stmpl_records, breadcrumEA, "BC" + "-" + newpagename);
                    //        if (i == ds.Tables[0].Rows.Count - 1 && (pagename.Contains("ps.aspx") || pagename.Contains("pl.aspx") || pagename.Contains("bb.aspx") || pagename.Contains("")))
                    //        {
                    //            _stmpl_records.SetAttribute("lastvalue", ds.Tables[0].Rows[i]["RemoveEAPATH"].ToString() + "@@" + Itemtype + "@@" + ds.Tables[0].Rows[i]["Itemvalue"].ToString());
                    //            // _stmpl_records.SetAttribute("lastvalue", newurl);

                    //        }

                    //    }
                    //    catch (System.Threading.ThreadAbortException)
                    //    {
                    //        // ignore it
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        //objErrorHandler.ErrorMsg = ex;
                    //        //objErrorHandler.CreateLog();
                    //    }


                    //    breadcrumb = breadcrumb + _stmpl_records.ToString();
                    //    dscount1++;
                    //}

             
                }
               // return objhelper.StripWhitespace(_stmpl_records1 + breadcrumb + "</div>");
                return _stmpl_records1 + breadcrumb + "</ul></div>";
            }
catch 
            
            {
                return "";

}
        }




        public string GetBreadCrumb_Simple_MS(string templatepath, Boolean withHome)
        {
            try{

            StringTemplateGroup _stg_records = null;
            StringTemplateGroup _stg_records1 = null;
            StringTemplateGroup _stg_recordsH = null;
            StringTemplate _stmpl_records = null;
            StringTemplate _stmpl_records1 = null;
            StringTemplate _stmpl_recordsH = null;
            string breadcrumb = string.Empty;
            string stemplatepath = templatepath;

            bool IsFromps = false;
            bool IsFromps_RC = false;
            bool IsFromproductlist = false;
            bool IsFromproductlist_RC = false;
            bool IsFromBrand = false;
            bool IsFromBrand_RC = false;
            string istsm = string.Empty;
            string istsb = string.Empty;
            string Familyname = string.Empty;
            string _fid = string.Empty;
            _stg_records = new StringTemplateGroup("Cell", stemplatepath);
            _stg_records1 = new StringTemplateGroup("main", stemplatepath);
            _stg_recordsH = new StringTemplateGroup("home", stemplatepath);

            DataSet dsb = new DataSet();
            dsb = null;
            int dscount1 = 0;
            int dscount = 0;
            int a = 0;
            string supName = "";

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            if (HttpContext.Current.Session["BreadCrumbDS"] != null)
            {
                if (HttpContext.Current.Session["BreadCrumbDS_URL"] == null)
                {
                    dsb = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
                }
                else
                {

                    dsb = (DataSet)HttpContext.Current.Session["BreadCrumbDS_URL"];
                }

            }
            if (dsb != null && dsb.Tables[0].Rows.Count > 0)
            {
                dscount = dsb.Tables[0].Rows.Count - 1;
                _stmpl_records1 = _stg_records1.GetInstanceOf("BreadCrumb" + "\\" + "Main");
                _stmpl_recordsH = _stg_recordsH.GetInstanceOf("BreadCrumb" + "\\" + "home");

                string breadcrumEA = "//// //// ////";
                string RemovebreadcrumEA = "//// //// ////";
                bool familybrandmodelset = false;
                int i;

                supName = dsb.Tables[0].Rows[0]["ItemValue"].ToString();

                _stmpl_recordsH.SetAttribute("MICROSITEURL",  objhelper.SimpleURL_MS_Str(supName, "mct.aspx",true) + "/mct/");
                for (i = 0; i <= dsb.Tables[0].Rows.Count - 1; i++)
                {
                    //Modified by :indu
                    //foreach (DataRow row in ds.Tables[0].Rows)
                    //{


                    DataRow row = dsb.Tables[0].Rows[i];
                    DataRow Revrow;
                    string Itemvalue = string.Empty;
                    string newpagename = string.Empty;
                    string newremovepagename = string.Empty;
                    if (i == 0)
                    {
                        Revrow = dsb.Tables[0].Rows[0];
                    }
                    else
                    {
                        Revrow = dsb.Tables[0].Rows[i - 1];
                    }
                    if (dscount == dscount1)
                        _stmpl_records = _stg_records.GetInstanceOf("BreadCrumb" + "\\" + "lastcell");
                    else

                        _stmpl_records = _stg_records.GetInstanceOf("BreadCrumb" + "\\" + "cell");



                    string[] PAGENAME = row["Url"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);
                    if (row["ItemType"].ToString().ToLower() == "category" && i == 0)
                    {
                        // _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString().ToLower()); 
                        _stmpl_records.SetAttribute("TBT_LINKNAME", textInfo.ToTitleCase(row["ItemValue"].ToString().ToLower()));
                        Itemvalue = HttpUtility.UrlEncode(row["ItemValue"].ToString());

                    }

                    else if (row["ItemType"].ToString().ToLower() == "family")
                    {
                        //_stmpl_records.SetAttribute("TBT_LINKNAME", row["FamilyName"].ToString().ToLower());
                        _stmpl_records.SetAttribute("TBT_LINKNAME", textInfo.ToTitleCase(row["FamilyName"].ToString().ToLower()));
                        Itemvalue = row["ItemValue"].ToString() + "=" + row["FamilyName"].ToString();
                        HttpContext.Current.Session["S_FName"] = row["FamilyName"].ToString();
                    }
                    else if (row["ItemType"].ToString().ToLower() == "brand" && PAGENAME[0].ToLower().Contains("ct.aspx"))
                    {
                       // _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString().ToLower());
                        _stmpl_records.SetAttribute("TBT_LINKNAME", textInfo.ToTitleCase(row["ItemValue"].ToString().ToLower()));
                        Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());
                    }
                    //else if ((row["ItemType"].ToString().ToLower() == "category") && i==0)
                    //{
                    //    _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString());
                    //    if (HttpContext.Current.Request.QueryString["cid"] != null)
                    //    {
                    //        Itemvalue = HttpContext.Current.Request.QueryString["cid"].ToString() + "=" + row["ItemValue"].ToString();
                    //    }
                    //    else
                    //    {
                    //        Itemvalue =  row["ItemValue"].ToString();
                    //    }
                    //}
                    else if (row["ItemType"].ToString().ToLower() == "product")
                    {
                        if (row["ProductCode"].ToString() != "")
                        {
                           // _stmpl_records.SetAttribute("TBT_LINKNAME", row["ProductCode"].ToString().ToLower());
                            _stmpl_records.SetAttribute("TBT_LINKNAME", textInfo.ToTitleCase(row["ProductCode"].ToString().ToLower()));
                            Itemvalue = row["ItemValue"].ToString() + "=" + row["ProductCode"].ToString();
                        }
                        else
                        {
                            //_stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString().ToLower());
                            _stmpl_records.SetAttribute("TBT_LINKNAME", textInfo.ToTitleCase(row["ItemValue"].ToString().ToLower()));
                            Itemvalue = row["ItemType"].ToString() + "=" + HttpUtility.UrlEncode(row["ItemValue"].ToString());
                        }
                    }


                    else
                    {
                        //_stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString().ToLower());
                        _stmpl_records.SetAttribute("TBT_LINKNAME", textInfo.ToTitleCase(row["ItemValue"].ToString().ToLower()));
                        Itemvalue = HttpUtility.UrlEncode(row["ItemValue"].ToString());
                        HttpContext.Current.Session["PRODUCT_CATEGORY_NAME_MS_SES"] = row["ItemValue"].ToString();
                    }
                    //Itemvalue = Itemvalue.Replace("+", "||");  
                    // Itemvalue = HttpUtility.UrlEncode(Itemvalue);  
                    _stmpl_records.SetAttribute("TBT_REMOVEEAPATH", HttpUtility.UrlEncode(objsecurity.StringEnCrypt(row["RemoveEAPath"].ToString())));
                    _stmpl_records.SetAttribute("TBT_REMOVEURL", row["RemoveUrl"].ToString());
                    _stmpl_records.SetAttribute("TBT_URL", row["Url"].ToString());
                    _stmpl_records.SetAttribute("TBT_EAPATH", HttpUtility.UrlEncode(objsecurity.StringEnCrypt(row["EAPath"].ToString())));
                    _stmpl_records.SetAttribute("TBT_ATTRIBUTE_TYPE", row["ItemType"].ToString());
                    try
                    {
                        string linkname = _stmpl_records.GetAttribute("TBT_LINKNAME").ToString();
                        _stmpl_records.SetAttribute("TBT_LINKTITLE", linkname.Replace('"', ' '));
                    }
                    catch
                    { }
                    //modified by :indu
                    try
                    {


                        //TBWTemplateEngine objTempengine = new TBWTemplateEngine("", "", "");
                        string Itemtype = row["ItemType"].ToString().ToUpper();
                        string RevItemType = Revrow["ItemType"].ToString().ToUpper();

                        _stmpl_records.SetAttribute("TBT_REM_ATTRIBUTE_TYPE", RevItemType);
                        string[] catpath = dsb.Tables[0].Rows[i]["EAPATH"].ToString().ToUpper().Split(new string[] { "////" }, StringSplitOptions.None);

                  string[]      catpath1 = HttpContext.Current.Request.RawUrl.Split('/');


                  if ((catpath.Length >= 3 ? catpath[2] : " ").ToLower() != (catpath.Length >= 2 ? catpath[1] : " "))
                  {
                      catpath[2] = (catpath1.Length >= 2 ? catpath1[1] : " ");
                  
                  }
                        newpagename = PAGENAME[0].ToLower();
                        if (breadcrumEA == "//// //// ////")
                        {

                            RemovebreadcrumEA = breadcrumEA;
                            breadcrumEA = breadcrumEA + Itemvalue;


                        }
                        else
                        {

                            RemovebreadcrumEA = breadcrumEA;
                            breadcrumEA = breadcrumEA + "////" + Itemvalue;

                        }
                        newpagename = PAGENAME[0].ToLower();
                        if (newpagename.Contains("pl.aspx"))
                        {
                            if (catpath.Length >= 5)
                            {
                                a = a + 1;
                                breadcrumEA = catpath[0] + "////" +  (catpath.Length >= 2 ? catpath[1] : " ") + "////" + (catpath.Length >= 3 ? catpath[2] : " ") + "////" + (catpath.Length >= 4 ? catpath[3] : " ")  + "////" + "mwa-" + a;
                                HttpContext.Current.Session[(catpath.Length >= 4 ? catpath[3] : " ")  + "mwa-" + a.ToString()] = dsb.Tables[0].Rows[i]["EAPATH"].ToString();
                            }
                            else if (catpath.Length > 3)
                            {
                                breadcrumEA = catpath[0] + "////" +  (catpath.Length >= 2 ? catpath[1] : " ") + "////" + (catpath.Length >= 3 ? catpath[2] : " ") + "////" + (catpath.Length >= 4 ? catpath[3] : " ") ;
                            }

                        }

                        else if (newpagename.Contains("pd.aspx"))
                        {



                            if (catpath.Length == 7)
                            {

                                breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +  (catpath.Length >= 2 ? catpath[1] : " ") + "////" +
                                  (catpath.Length >= 3 ? catpath[2] : " ") + "////" + (catpath.Length >= 4 ? catpath[3] : " ")  + "////" +
                                        Familyname +
                                        "////" +
                                        (catpath.Length >= 7 ? catpath[6] : " ").Replace("USERSEARCH1=PROD ID=", "")
                                        +
                                  "=" + dsb.Tables[0].Rows[i]["PRODUCTCODE"].ToString().ToUpper() +
                                   "////" +
                                 (catpath.Length >= 6 ? catpath[5] : " ").Replace("USERSEARCH=FAMILY ID=", "");


                            }
                            else
                            {

                                breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +  (catpath.Length >= 2 ? catpath[1] : " ") + "////" +
                                 (catpath.Length >= 3 ? catpath[2] : " ") + "////" + (catpath.Length >= 4 ? catpath[3] : " ")  + "////" +
                                           Familyname + "////" +
                                    (catpath.Length >= 6 ? catpath[5] : " ").Replace("USERSEARCH1=PROD ID=", "") + "=" +
                                    dsb.Tables[0].Rows[i]["PRODUCTCODE"].ToString().ToUpper() + "////" +
                                      (catpath.Length >= 5 ? catpath[4] : " ").Replace("USERSEARCH=FAMILY ID=", "");


                            }
                        }
                        else if (newpagename.Contains("fl.aspx"))
                        {
                            // string[] catpath = ds.Tables[0].Rows[i] ["EAPATH"].ToString().ToUpper().Split(new string[] { "////" }, StringSplitOptions.None);



                            if (Itemtype == "FAMILY")
                            {

                                Familyname = dsb.Tables[0].Rows[i]["FAMILYNAME"].ToString();
                                _fid = dsb.Tables[0].Rows[i]["Itemvalue"].ToString();


                                //if (catpath[3].Contains("USERSEARCH=") == false)
                                //{
                                breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +  (catpath.Length >= 2 ? catpath[1] : " ") + "////" + (catpath.Length >= 3 ? catpath[2] : " ") + "////" + (catpath.Length >= 4 ? catpath[3] : " ")  + "////" +
                                        Familyname + "////" + _fid;
                                //}
                                //else
                                //{
                                //    breadcrumEA = catpath[0] + "////" + catpath[1] + "////" + catpath[2] + "////" +
                                //              Familyname + "-" + _fid;
                                //}

                            }
                            else
                            {

                                a = a + 1;
                                breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +  (catpath.Length >= 2 ? catpath[1] : " ") + "////" + (catpath.Length >= 3 ? catpath[2] : " ") + "////" +
                                    (catpath.Length >= 4 ? catpath[3] : " ")  + "////" + Familyname + "////" + _fid + "////" + "mwa-" + a;

                                HttpContext.Current.Session[_fid + "mwa-" + a.ToString()] = dsb.Tables[0].Rows[i]["EAPATH"].ToString();

                            }


                        }

                        else if (newpagename.Contains("ct.aspx"))
                        {

                            //breadcrumEA = breadcrumEA.Replace("Brand=", "");
                            if (Itemtype.ToLower() == "brand")
                            {
                                istsb = dsb.Tables[0].Rows[i]["Itemvalue"].ToString();

                            }

                        }
                        else if (newpagename.Contains("bb.aspx"))
                        {

                            if (catpath.Length <= 5)
                            {

                                if (Itemtype.ToLower() == "model")
                                {
                                    istsm = dsb.Tables[0].Rows[i]["Itemvalue"].ToString().ToLower().Replace("model=", "");
                                }
                                breadcrumEA = breadcrumEA.ToUpper().Replace("MODEL=", "").Replace("BRAND=", "");
                            }
                            else
                            {
                                a = a + 1;
                                breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +  (catpath.Length >= 2 ? catpath[1] : " ") + "////" + (catpath.Length >= 3 ? catpath[2] : " ") + "////" + istsb + "////" + istsm + "////" + "wa-" + a;


                                HttpContext.Current.Session[istsm + "mwa-" + a.ToString()] = dsb.Tables[0].Rows[i]["EAPATH"].ToString();



                            }
                        }
                        else if ((newpagename.Contains("ps.aspx")) || (newpagename == ""))
                        {

                            if (catpath.Length > 4)
                            {

                                a = a + 1;


                                breadcrumEA = (catpath.Length >= 1 ? catpath[0] : " ") + "////" +  (catpath.Length >= 2 ? catpath[1] : " ") + "////" + dsb.Tables[0].Rows[0]["ItemValue"].ToString() + "////" + "////" + dsb.Tables[0].Rows[1]["ItemValue"].ToString() + "////" + "mwa-" + a;


                                HttpContext.Current.Session[dsb.Tables[0].Rows[1]["ItemValue"].ToString() + "mwa-" + a.ToString()] = dsb.Tables[0].Rows[i]["EAPATH"].ToString();
                                newpagename = "ps";
                            }

                            // IsFromps = true;


                            breadcrumEA = breadcrumEA.ToUpper().Replace("USER SEARCH=", "").Replace("USERSEARCH=", "").Replace("USERSEARCH1=", "").Replace("USERSEARCH2=", "").Replace("CATEGORY=", "");
                            //  breadcrumEA = breadcrumEA.Replace("//// //// ////", "//// //// ////" + "UserSearch=");
                        }

                        else
                        {
                            newpagename = PAGENAME[0].ToLower();

                        }
                        //  breadcrumEA = breadcrumEA.Replace("+", "||"); 
                        //Cons_NewURl(_stmpl_records, breadcrumEA, PAGENAME[0].ToLower(), true, false);                     

                        if (newpagename == "")
                        {
                            newpagename = "ps.aspx";
                        }

                        string newurl = objhelper.SimpleURL_MS(_stmpl_records, breadcrumEA, "BC" + "-" + "m" + newpagename);


                        string[] REMOVEPAGENAME = row["RemoveUrl"].ToString().Split(new string[] { "?" }, StringSplitOptions.None);


                        newremovepagename = REMOVEPAGENAME[0].ToLower();

                        objhelper.SimpleURL_MS(_stmpl_records, RemovebreadcrumEA, "BCRemoveurl" + "-" + "m" + newremovepagename.ToLower());


                    }
                    catch (System.Threading.ThreadAbortException)
                    {
                        // ignore it
                    }
                    catch (Exception ex)
                    {
                        //objErrorHandler.ErrorMsg = ex;
                        //objErrorHandler.CreateLog();
                    }

                    if (i != 0)  /// remove first category
                        breadcrumb = breadcrumb + _stmpl_records.ToString();

                    dscount1++;
                }
            }
           

            
            if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mfl.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("mpd.aspx") == true)
                _stmpl_records1.SetAttribute("GO_BACK", true);
            else
                _stmpl_records1.SetAttribute("GO_BACK", false);

            string rtn = string.Empty;
            if (withHome == true)
                rtn = objhelper.StripWhitespace(_stmpl_records1.ToString() + _stmpl_recordsH.ToString() + breadcrumb + "</div>");
            else
                rtn = objhelper.StripWhitespace(_stmpl_records1.ToString() + breadcrumb + "</div>");
            return rtn;
             }
                catch (Exception ex)
                    {
                        return "";
                        //objErrorHandler.ErrorMsg = ex;
                        //objErrorHandler.CreateLog();
                    }
        }
         


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE AUTO SPELLING CORRECTION SUGGECTIONS ***/
        /********************************************************************************/



        protected void updateSearchSpell_Correction(INavigateResults res, string SearchText)
        {
            try
            {
                string strcommand = string.Empty;
                string strtrim = string.Empty;
                string[] strlink = null;
                string strlinks = string.Empty;
                string strcorrectword = string.Empty;
                string strsearch = string.Empty;
                string temphead = "<h3 style='font-size: 11px;font-weight:bold;color:black;'>" + SearchText + " Not Found </h3>";
                HttpContext.Current.Session["Spell_Correction"] = null;
                if (res.getLastItem() == -1)
                {
                    strcommand = temphead;
                    strcommand = strcommand + "<p class='p4'>Please Search Again</p>";

                    HttpContext.Current.Session["Spell_Correction"] = strcommand;
                }
                else
                {
                    String commentary = res.getCommentary();
                    String prettycomment = "Sorry. There are no results for '";
                    String outcomment = "";
                    string Search_Word = string.Empty;
                    string strvalue = string.Empty;
                    if (0 < commentary.Length)
                    {
                        if (-1 != commentary.IndexOf("Ignored"))
                        {
                            strcommand =temphead;
                            IBreadCrumbTrail bct = res.getBreadCrumbTrail();
                            IList<INavigateNode> i = bct.getSearchPath();
                            foreach (INavigateNode node in bct.getSearchPath())
                            {
                                if (node.getValue() != "Cellular Accessories" && node.getValue() != "WESAUSTRALASIA" && node.getValue() != "AllProducts")
                                {
                                    Search_Word = node.getValue();
                                    //outcomment = prettycomment + res.getQuestion() + "'.";
                                    strsearch = " Search found results for '" + Search_Word + "'";
                                }
                                else
                                {
                                  //  outcomment = prettycomment + res.getQuestion() + "'.";
                                }
                            }
                            strcorrectword = GetReplaceComments(commentary);
                            strtrim = GetReplaceComments(commentary, SearchText, true);
                            if (strtrim != "")
                            {
                                strlink = strtrim.Split(',');
                                for (int j = 0; j <= strlink.Length - 1; j++)
                                {
                                    if (strlinks == "")
                                    {
                                        //strlinks = "<a href='/ps.aspx?" + strlink[j].Trim() + "'>" + strlink[j].Trim() + "</a>";
                                        // HelperServices objHelperServices=new HelperServices();
                                        strvalue = objhelper.SimpleURL_Str( strlink[j].Trim(), "ps.aspx",true);
                                        strlinks = "<a href='"+ strvalue + "/ps/'  style=\"text-decoration: underline;\">" + strlink[j].Trim() + "</a>";
                                    }
                                    else
                                    {
                                        //strlinks = strlinks + ", <a href='/ps.aspx?" + strlink[j].Trim() + "'>" + strlink[j].Trim() + "</a>";
                                        strvalue = objhelper.SimpleURL_Str( strlink[j].Trim(), "ps.aspx",true);
                                        strlinks = strlinks + ", <a href='" + strvalue + "/ps/' style=\"text-decoration: underline;\">" + strlink[j].Trim() + "</a>";
                                    }
                                }
                               // outcomment = outcomment + "<br> Did You Mean " + strlinks + "?";

                            }

                            if (strsearch != "")
                                strcommand = strcommand + "<p class='p4'> " + strsearch + "</p>";

                            if (strcorrectword != "")
                                strcommand = strcommand + "<p class='p4'> " + strcorrectword + "</p>";

                            if (strlinks != "")
                                strcommand = strcommand + "<p class='p4'> Did you mean " + strlinks + "?</p>";


                            strcommand = strcommand + "<p class='p4'> If not please search again</p>";
                            HttpContext.Current.Session["Spell_Correction"] = strcommand;

                            //HttpContext.Current.Session["Spell_Correction"] = strcommand + "<p class='p4'> " + outcomment + " </p> <p class='p4'> If Not Please Search Again</p>";
                        }
                        else if (-1 != commentary.IndexOf("Corrected Word"))
                        {
                            strcommand = temphead;
                            outcomment = res.getCommentary();
                            //outcomment = outcomment.Replace("Corrected Word", "");
                            //outcomment = outcomment.Replace(SearchText + " is", "");
                            //outcomment = outcomment.Replace(SearchText.ToUpper() + " is", "");
                            //outcomment = outcomment.Replace(SearchText.ToLower() + " is", "");
                            //outcomment = outcomment.Replace("Other possible corrections:", "");
                            //outcomment = outcomment.Replace(";", ",");
                            //outcomment = outcomment.Replace(":", "");
                            //outcomment = outcomment.Replace(SearchText + " could be", "");
                            //outcomment = outcomment.Replace(SearchText.ToUpper() + " could be", "");
                            //outcomment = outcomment.Replace(SearchText.ToLower() + " could be", "");

                            //outcomment = outcomment.Replace("Added Synonyms", "");
                            //outcomment = outcomment.Replace("  ", " ");
                            //outcomment = outcomment.Trim();
                            strcorrectword = GetReplaceComments(outcomment);
                            outcomment = GetReplaceComments(outcomment, SearchText, false);
                            if (outcomment != "")
                            {
                                strlink = outcomment.Split(',');
                                for (int i = 0; i <= strlink.Length - 1; i++)
                                {
                                    if (strlinks == "")
                                    {
                                        //strlinks = "<a href='/ps.aspx?" + strlink[i].Trim() + "'>" + strlink[i].Trim() + "</a>";
                                        strvalue = objhelper.SimpleURL_Str(strlink[i].Trim(), "ps.aspx",true);
                                        strlinks = "<a href='" + strvalue + "/ps/' style=\"text-decoration: underline;\">" + strlink[i].Trim() + "</a>";

                                    }
                                    else
                                    {
                                        //strlinks = strlinks + ", <a href='/ps.aspx?" + strlink[i].Trim() + "'>" + strlink[i].Trim() + "</a>";
                                        strvalue = objhelper.SimpleURL_Str( strlink[i].Trim(), "ps.aspx",true);
                                        strlinks = strlinks + ", <a href='" + strvalue + "/ps/' style=\"text-decoration: underline;\">" + strlink[i].Trim() + "</a>";
                                    }
                                        // strlinks = strlinks + ", <a href='/" + strlink[i].Trim() + "'>" + strlink[i].Trim() + "/ps/" + "</a>";

                                }
                            }
                            //string[] Outcomment = outcomment.Split(';');
                            //outcomment = Outcomment[0].ToString() + ";";

                            if (strcorrectword != "")
                                strcommand = strcommand + "<p class='p4'> " + strcorrectword + "</p>";

                            if (strlinks != "")
                                strcommand = strcommand + "<p class='p4'> Did you mean " + strlinks + "?</p>";


                            strcommand = strcommand + "<p class='p4'> If not please search again</p>";

                            HttpContext.Current.Session["Spell_Correction"] = strcommand; 

                            //HttpContext.Current.Session["Spell_Correction"] = strcommand + "<p class='p4'> Did You Mean " + strlinks + "?</p> <p class='p4'> If Not Please Search Again</p>";
                        }
                        //Corrected Word
                        

                    }
                    else
                    {
                        HttpContext.Current.Session["Spell_Correction"] = null;
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorhandler.ErrorMsg = ex;
                objErrorhandler.CreateLog();
            }
            finally
            {
            }

        }
        protected void updateSearchSpell_Correctionjson(INavigateResults res, string SearchText)
        {
            try
            {
                string strcommand = string.Empty;
                string strtrim = string.Empty;
                string[] strlink = null;
                string strlinks = string.Empty;
                string strcorrectword = string.Empty;
                string strsearch = string.Empty;
                string temphead = "<h3 style='font-size: 11px;font-weight:bold;color:black;'>" + SearchText + " Not Found </h3>";
                HttpContext.Current.Session["Spell_Correction"] = null;
                if (res.getLastItemJson() == -1)
                {
                    strcommand = temphead;
                    strcommand = strcommand + "<p class='p4'>Please Search Again</p>";

                    HttpContext.Current.Session["Spell_Correction"] = strcommand;
                }
                else
                {
                    String commentary = res.getCommentaryJson();
                    String prettycomment = "Sorry. There are no results for '";
                    String outcomment = "";
                    string Search_Word = string.Empty;
                    string strvalue = string.Empty;
                    if (0 < commentary.Length)
                    {
                        if (-1 != commentary.IndexOf("Ignored"))
                        {
                            strcommand = temphead;
                            //IBreadCrumbTrail bct = res.getBreadCrumbTrail();
                            //IList<INavigateNode> i = bct.getSearchPath();
                            //foreach (INavigateNode node in bct.getSearchPath())
                            //{
                            //    if (node.getValue() != "Cellular Accessories" && node.getValue() != "WESAUSTRALASIA" && node.getValue() != "AllProducts")
                            //    {
                            //        Search_Word = node.getValue();
                            //        //outcomment = prettycomment + res.getQuestion() + "'.";
                            //        strsearch = " Search found results for '" + Search_Word + "'";
                            //    }
                            //    else
                            //    {
                            //        //  outcomment = prettycomment + res.getQuestion() + "'.";
                            //    }
                            //}
                            Search_Word = res.getUserSearchValueJson();
                            strcorrectword = GetReplaceComments(commentary);
                            strtrim = GetReplaceComments(commentary, SearchText, true);
                            if (strtrim != "")
                            {
                                strlink = strtrim.Split(',');
                                for (int j = 0; j <= strlink.Length - 1; j++)
                                {
                                    if (strlinks == "")
                                    {
                                        //strlinks = "<a href='/ps.aspx?" + strlink[j].Trim() + "'>" + strlink[j].Trim() + "</a>";
                                        // HelperServices objHelperServices=new HelperServices();
                                        strvalue = objhelper.SimpleURL_Str( strlink[j].Trim(), "ps.aspx",true);
                                        strlinks = "<a href='" + strvalue + "/ps/'  style=\"text-decoration: underline;\">" + strlink[j].Trim() + "</a>";
                                    }
                                    else
                                    {
                                        //strlinks = strlinks + ", <a href='/ps.aspx?" + strlink[j].Trim() + "'>" + strlink[j].Trim() + "</a>";
                                        strvalue = objhelper.SimpleURL_Str( strlink[j].Trim(), "ps.aspx",true);
                                        strlinks = strlinks + ", <a href='" + strvalue + "/ps/' style=\"text-decoration: underline;\">" + strlink[j].Trim() + "</a>";
                                    }
                                }
                                // outcomment = outcomment + "<br> Did You Mean " + strlinks + "?";

                            }

                            if (strsearch != "")
                                strcommand = strcommand + "<p class='p4'> " + strsearch + "</p>";

                            if (strcorrectword != "")
                                strcommand = strcommand + "<p class='p4'> " + strcorrectword + "</p>";

                            if (strlinks != "")
                                strcommand = strcommand + "<p class='p4'> Did you mean " + strlinks + "?</p>";


                            strcommand = strcommand + "<p class='p4'> If not please search again</p>";
                            HttpContext.Current.Session["Spell_Correction"] = strcommand;

                            //HttpContext.Current.Session["Spell_Correction"] = strcommand + "<p class='p4'> " + outcomment + " </p> <p class='p4'> If Not Please Search Again</p>";
                        }
                        else if (-1 != commentary.IndexOf("Corrected Word"))
                        {
                            strcommand = temphead;
                            outcomment = res.getCommentaryJson();
                            //outcomment = outcomment.Replace("Corrected Word", "");
                            //outcomment = outcomment.Replace(SearchText + " is", "");
                            //outcomment = outcomment.Replace(SearchText.ToUpper() + " is", "");
                            //outcomment = outcomment.Replace(SearchText.ToLower() + " is", "");
                            //outcomment = outcomment.Replace("Other possible corrections:", "");
                            //outcomment = outcomment.Replace(";", ",");
                            //outcomment = outcomment.Replace(":", "");
                            //outcomment = outcomment.Replace(SearchText + " could be", "");
                            //outcomment = outcomment.Replace(SearchText.ToUpper() + " could be", "");
                            //outcomment = outcomment.Replace(SearchText.ToLower() + " could be", "");

                            //outcomment = outcomment.Replace("Added Synonyms", "");
                            //outcomment = outcomment.Replace("  ", " ");
                            //outcomment = outcomment.Trim();
                            strcorrectword = GetReplaceComments(outcomment);
                            outcomment = GetReplaceComments(outcomment, SearchText, false);
                            if (outcomment != "")
                            {
                                strlink = outcomment.Split(',');
                                for (int i = 0; i <= strlink.Length - 1; i++)
                                {
                                    if (strlinks == "")
                                    {
                                        //strlinks = "<a href='/ps.aspx?" + strlink[i].Trim() + "'>" + strlink[i].Trim() + "</a>";
                                        strvalue = objhelper.SimpleURL_Str(strlink[i].Trim(), "ps.aspx",true);
                                        strlinks = "<a href='" + strvalue + "/ps/' style=\"text-decoration: underline;\">" + strlink[i].Trim() + "</a>";

                                    }
                                    else
                                    {
                                        //strlinks = strlinks + ", <a href='/ps.aspx?" + strlink[i].Trim() + "'>" + strlink[i].Trim() + "</a>";
                                        strvalue = objhelper.SimpleURL_Str (strlink[i].Trim(), "ps.aspx",true);
                                        strlinks = strlinks + ", <a href='" + strvalue + "/ps/' style=\"text-decoration: underline;\">" + strlink[i].Trim() + "</a>";
                                    }
                                    // strlinks = strlinks + ", <a href='/" + strlink[i].Trim() + "'>" + strlink[i].Trim() + "/ps/" + "</a>";

                                }
                            }
                            //string[] Outcomment = outcomment.Split(';');
                            //outcomment = Outcomment[0].ToString() + ";";

                            if (strcorrectword != "")
                                strcommand = strcommand + "<p class='p4'> " + strcorrectword + "</p>";

                            if (strlinks != "")
                                strcommand = strcommand + "<p class='p4'> Did you mean " + strlinks + "?</p>";


                            strcommand = strcommand + "<p class='p4'> If not please search again</p>";

                            HttpContext.Current.Session["Spell_Correction"] = strcommand;

                            //HttpContext.Current.Session["Spell_Correction"] = strcommand + "<p class='p4'> Did You Mean " + strlinks + "?</p> <p class='p4'> If Not Please Search Again</p>";
                        }
                        //Corrected Word


                    }
                    else
                    {
                        HttpContext.Current.Session["Spell_Correction"] = null;
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorhandler.ErrorMsg = ex;
                objErrorhandler.CreateLog();
            }
            finally
            {
            }

        }
        private string GetReplaceComments(string outcomment)
        {
            string tempstr = string.Empty;


            string[] splitstr = outcomment.Split(';');

            if (splitstr.Length > 0)
            {
                for (int i = 0; i <= splitstr.Length - 1; i++)
                {
                    if (splitstr[i].ToString().ToLower().IndexOf("corrected word") != -1)
                    {
                        tempstr = splitstr[i].ToString();
                        tempstr = tempstr.Replace(";", ",");
                        tempstr = tempstr.Replace(":", "");
                        tempstr = tempstr.Replace("Corrected Word", "");

                        tempstr = tempstr.Replace(" is ", " has been corrected to ");
                    }
                }
            }
            return tempstr;
        }
        private string GetReplaceComments(string outcomment, string SearchText, bool IsIgnored)
        {
            string tempstr = string.Empty;


            string[] splitstr = outcomment.Split(';');

            if (splitstr.Length > 0)
            {
                for (int i = 0; i <= splitstr.Length - 1; i++)
                {
                    if (splitstr[i].ToString().ToLower().IndexOf("other possible corrections") != -1)
                    {
                        tempstr = splitstr[i].ToString();
                        tempstr = tempstr.Replace("Other possible corrections", "");
                        tempstr = tempstr.Replace(";", ",");
                        tempstr = tempstr.Replace(":", "");
                        tempstr = tempstr.Replace(SearchText + " could be", "");
                        tempstr = tempstr.Replace(SearchText.ToUpper() + " could be", "");
                        tempstr = tempstr.Replace(SearchText.ToLower() + " could be", "");
                        tempstr = tempstr.Replace("could be", ",");
                        tempstr = tempstr.Replace("Added Synonyms", "");
                    }
                }
            }
            outcomment = tempstr;

            /* old 
             * if (IsIgnored == true)
             {
                 string[] splitstr = outcomment.Split(';');
                 if (splitstr.Length > 0)
                 {
                     if (splitstr[0].ToString().ToLower().IndexOf("ignored") != -1)
                     {
                         if (splitstr.Length == 1)
                             outcomment = outcomment.Replace(splitstr[0].ToString(), "");
                         else
                             outcomment = outcomment.Replace(splitstr[0].ToString() + ";", ""); 
                        
                         tempstr = splitstr[0].ToString().Replace("Ignored:", "");
                         tempstr = tempstr.ToString().Replace(";", "").Trim();
                         tempstr = SearchText.Replace(tempstr, "").Trim();
                     }
                 }
             }



             outcomment = outcomment.Replace("Corrected Word", "");

             outcomment = outcomment.Replace(SearchText + " is", "");
             outcomment = outcomment.Replace(SearchText.ToUpper() + " is", "");
             outcomment = outcomment.Replace(SearchText.ToLower() + " is", "");

             outcomment = outcomment.Replace("Other possible corrections:", "");
             outcomment = outcomment.Replace(";", ",");
             outcomment = outcomment.Replace(":", "");
             outcomment = outcomment.Replace(SearchText + " could be", "");
             outcomment = outcomment.Replace(SearchText.ToUpper() + " could be", "");
             outcomment = outcomment.Replace(SearchText.ToLower() + " could be", "");

             outcomment = outcomment.Replace("Added Synonyms", "");
             //is corrected as
             if (IsIgnored == true)
             {
                 outcomment = outcomment.Replace(tempstr + " is", "");
                 outcomment = outcomment.Replace(tempstr.ToUpper() + " is", "");
                 outcomment = outcomment.Replace(tempstr.ToLower() + " is", "");

                 outcomment = outcomment.Replace(tempstr + " could be", "");
                 outcomment = outcomment.Replace(tempstr.ToUpper() + " could be", "");
                 outcomment = outcomment.Replace(tempstr.ToLower() + " could be", "");
             }
             outcomment = outcomment.Replace("  ", " ");
             outcomment = outcomment.Trim();*/



            return outcomment;
        }

        #region "For the Menu,Sub Menu"

        DataSet SubCategory = new DataSet();
        DataSet CategoryMenu = new DataSet();
        DataSet MainCategory = new DataSet();
        DataSet Brand = new DataSet();
        DataSet Category, Brand_Model;
        DataTable Main_Category = new DataTable();
        DataTable Sub_Category = new DataTable();
        DataTable Category_Menu = new DataTable();
        DataTable tbl_Brand = new DataTable();
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE CATEGORY AND BRAND DETAILS ***/
        /********************************************************************************/
        public DataSet   GetCategoryAndBrand(string ReturnType, bool CreateXml)
        {
                IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WesCatBrandDictionary);
                IOptions opts = ea.getOptions();
                opts.setResultsPerPage("1"); // ea_rpp.Value);   // use current settings
                opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
                opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
                opts.setSubCategories(true);
                opts.setNavigateHierarchy(false);
                opts.setReturnSKUs(false);                
                Category = new DataSet();
                INavigateResults res = ea.userBreadCrumbClick("AllProducts////WESAUSTRALASIA");
                HttpContext.Current.Session["EA_URL"] = HttpContext.Current.Session["EA"];
                HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA";// res.getCatPathJson();
                String path = "AllProducts////WESAUSTRALASIA";//res.getCatPathJson();
         
                Create_DataTable_Columns();

                GetCategoryfromJson(res);


                CategoryMenu.WriteXml(strxml + "\\subCatAll.xml");
                    SubCategory.WriteXml(strxml + "\\subds.xml");
                    MainCategory.WriteXml(strxml + "\\Mainds.xml");
         
                    return MainCategory;
           
        }
        public DataSet GetCategoryAndBrand(string ReturnType)
        {
            if (HttpContext.Current.Session["MainCategory"] == null || HttpContext.Current.Session["SubCategory"] == null || HttpContext.Current.Session["SubCategoryAll"] == null)
            {

                System.IO.FileInfo Fil = new System.IO.FileInfo(strxml + "\\subds.xml");
                System.IO.FileInfo Fil1 = new System.IO.FileInfo(strxml + "\\Mainds.xml");
                System.IO.FileInfo Fil2 = new System.IO.FileInfo(strxml + "\\subCatAll.xml");
                if (Fil.Exists == false || Fil1.Exists == false || Fil2.Exists == false)                
                    GetCategoryAndBrand("", true);


                //IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WesCatBrandDictionary);
                //IOptions opts = ea.getOptions();
                //opts.setResultsPerPage("1"); // ea_rpp.Value);   // use current settings
                //opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
                //opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
                //opts.setSubCategories(true);
                //opts.setNavigateHierarchy(false);
                //opts.setReturnSKUs(false);                
                //Category = new DataSet();
                HttpContext.Current.Session["EA_URL"] = HttpContext.Current.Session["EA"];
                HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA";// res.getCatPathJson();
               // String path = "AllProducts////WESAUSTRALASIA";//res.getCatPathJson();
             
                

                

                SubCategory.ReadXml(strxml + "\\subds.xml");
                MainCategory.ReadXml(strxml + "\\Mainds.xml");
                CategoryMenu.ReadXml(strxml + "\\subCatAll.xml");

               /* json try
                {
                    foreach (INavigateCategory item in list)
                    {
                        //if (item.getName().ToUpper() != "WESNEWS")
                        //{
                        if (item.getName().ToUpper() != "")
                        {
                            DataRow row = Main_Category.NewRow();                      
                            row["CATEGORY_NAME"] = item.getName();
                            IList<string> li = item.getIDs();
                            row["CATEGORY_ID"] = li[0].ToString().Substring(2);
                            row["PARENT_CATEGORY"] = "0";
                            row["SHORT_DESC"] = string.Empty;
                            row["IMAGE_FILE"] = string.Empty;
                            row["IMAGE_FILE2"] = string.Empty;
                            row["IMAGE_NAME"] = string.Empty;
                            row["IMAGE_NAME2"] = string.Empty;
                            row["CUSTOM_TEXT_FIELD2"] = string.Empty;
                            row["CUSTOM_NUM_FIELD3"] = "2";
                            row["EA_PATH"] = HttpContext.Current.Session["EA"].ToString() + "////" + item.getName();
                       

                        IList<INavigateCategory> SubCat_List = item.getSubCategories();
                        foreach (INavigateCategory item1 in SubCat_List)
                        {
                            DataRow row1 = Sub_Category.NewRow();
                            row1["TBT_PARENT_CATEGORY_NAME"] = item.getName();

                           //PARENT_CATEGORY
                            row1["TBT_PARENT_CATEGORY_ID"] = li[0].ToString().Substring(2);
                            row1["CATEGORY_NAME"] = item1.getName();
                            IList<string> SUB_CATEGORY_ID = item1.getIDs();

                            row1["CATEGORY_ID"] = SUB_CATEGORY_ID[0].ToString().Substring(2);
                            row1["TBT_SHORT_DESC"] = string.Empty;
                            row1["TBT_CUSTOM_NUM_FIELD3"] = "2";
                            row1["TBT_PARENT_CATEGORY_IMAGE"] = string.Empty;                                                        
                            row1["EA_PATH"] = HttpContext.Current.Session["EA"].ToString();
                            Sub_Category.Rows.Add(row1);
                        }
                        Main_Category.Rows.Add(row);
                        }
                    }
                    //foreach (INavigateAttribute item in Brand_list)
                    //{
                    //    DataRow row = Brand.NewRow();
                    //    row["TOSUITE_BRAND"] = item.getValue();
                    //    Brand.Rows.Add(row);
                    //}

                }
                catch (Exception)
                {
                }*/
                //GetCategoryfromJson(res);
                //GetDataSet1(res);
                //GetDBCategoryDetails();
                HttpContext.Current.Session["SubCategory"] = SubCategory;
                HttpContext.Current.Session["MainCategory"] = MainCategory;
                HttpContext.Current.Session["SubCategoryAll"] = CategoryMenu; 
            }
            if (ReturnType == "MainCategory")
                return (DataSet)HttpContext.Current.Session["MainCategory"];
            else if (ReturnType == "SubCategory")
                return (DataSet)HttpContext.Current.Session["SubCategory"];
            else if (ReturnType == "SubCategoryAll")
                return (DataSet)HttpContext.Current.Session["SubCategoryAll"];
            //else if (ReturnType == "Brand")
            //    return (DataSet)HttpContext.Current.Session["WESBrand"];
            else
                return null;
        }



        private void GetCategoryfromJson(INavigateResults res)
        {
            string catid="";
            DataTable maintb = new DataTable();
            string image_string = string.Empty;
            string shortdesc = string.Empty;
            int custnum = 0;
            StrSql = "";
            string CatIds = string.Empty;

            
            DataSet ds = new DataSet();

            //ds.ReadXml(strxml+"\\ds.xml"); 
            ds=res.GetDBAdvisor();

            

            DataTable subdt = objCategoryServices.GetSubCategories(2);


            double cnt = 0;
            int cntchk = 0;
            string sttr = string.Empty;

            if (ds != null && ds.Tables.Count > 0 && ds.Tables["CategoryList"] != null && ds.Tables["CategoryList"].Rows.Count > 0)
            {


                foreach (DataRow Dr in ds.Tables["CategoryList"].Rows)
                {
                    catid = Dr["ids"].ToString().Split(':')[1].ToString();
                    CatIds = CatIds + "'" + catid + "',";
                }
                if (CatIds != "")
                    CatIds = CatIds.Substring(0, CatIds.Length - 1) + "";
              
                maintb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, CatIds, "GET_CATEGORY", HelperDB.ReturnType.RTTable);

                



                foreach (DataRow  item in ds.Tables["CategoryList"].Rows  )
                {
                   if (item["name"].ToString().ToUpper() != "WESNEWS")
                    {
                        catid = item["ids"].ToString().Split(':')[1].ToString();

                        DataRow row = Main_Category.NewRow();
                        row["CATEGORY_NAME"] = item["name"].ToString();
                        row["CATEGORY_ID"] = catid;
                        row["PARENT_CATEGORY"] = "0";
                        row["SHORT_DESC"] = string.Empty;
                        row["IMAGE_FILE"] = string.Empty;
                        row["IMAGE_FILE2"] = string.Empty;
                        row["IMAGE_NAME"] = string.Empty;
                        row["IMAGE_NAME2"] = string.Empty;
                        row["CUSTOM_TEXT_FIELD2"] = string.Empty;
                        row["CUSTOM_NUM_FIELD3"] = "2";
                        row["EA_PATH"] = HttpContext.Current.Session["EA"].ToString() + "////" + item["name"].ToString();



                        row["URL_RW_PATH"] = objhelper.SimpleURL_Str( "AllProducts////WESAUSTRALASIA////" + "////" + row["CATEGORY_NAME"].ToString(), "ct.aspx",false );
                        sttr = row["CATEGORY_NAME"].ToString();
                       int indx=0;
                        if (sttr.Length > 7)
                        {
                            indx = sttr.IndexOf(" ", 7);                            
                        }
                        if (indx >= 7)
                            sttr = sttr.Substring(0, indx) + "<br/>" + sttr.Substring(indx + 1);
                        else if (sttr.Equals("VCR COMPONENTS"))
                            sttr = "VCR<br/>COMPONENTS";
                        row["CATEGORY_NAME_TOP"] = sttr;

                        cntchk = 0;


                        image_string="";
                        shortdesc="";
                        custnum=0;
                        if (maintb != null && maintb.Rows.Count > 0)
                        {
                            DataRow[] mdrs = maintb.Select("CATEGORY_ID='" + catid + "'");
                            if (mdrs.Length > 0)
                            {
                                image_string = mdrs[0]["IMAGE_FILE"].ToString();
                                if (image_string != "")
                                    image_string = objhelper.SetImageFolderPath(image_string.Replace("\\", "/"), "_Images", "_th");
                                shortdesc = mdrs[0]["SHORT_DESC"].ToString();
                                custnum = ((int)float.Parse(mdrs[0]["CUSTOM_NUM_FIELD3"].ToString()));
                                row["SHORT_DESC"] = shortdesc;
                                row["IMAGE_FILE"] = image_string;// Dt.Rows[0]["IMAGE_FILE"];
                                row["IMAGE_FILE2"] = mdrs[0]["IMAGE_FILE2"];
                                row["IMAGE_NAME"] = mdrs[0]["IMAGE_NAME"];
                                row["IMAGE_NAME2"] = mdrs[0]["IMAGE_NAME2"];
                                row["CUSTOM_TEXT_FIELD2"] = mdrs[0]["CUSTOM_TEXT_FIELD2"];
                                row["CUSTOM_NUM_FIELD3"] = custnum;
                            }
                        }

                        if (subdt != null && subdt.Rows.Count > 0)
                        {
                            DataRow[] drs=subdt.Select("PARENT_CATEGORY='"+ catid + "' and Publish=1");

                            if (drs.Length > 0)
                            {
                                row["SUB_COUNT"] = drs.Length;
                                cnt = drs.Length;
                                if (drs.Length > 6)
                                {
                                    cnt = 6;
                                }

                                foreach (DataRow item1 in drs)
                                {
                                    DataRow row1 = Sub_Category.NewRow();
                                    row1["TBT_PARENT_CATEGORY_NAME"] = (item["name"].ToString().ToUpper() != "WESNEWS") ? item["name"].ToString() : "New Products"; // item["name"].ToString();

                                    //PARENT_CATEGORY
                                    row1["TBT_PARENT_CATEGORY_ID"] = catid;
                                    row1["CATEGORY_NAME"] = item1["CATEGORY_NAME"].ToString();
                                    row1["CATEGORY_ID"] = item1["CATEGORY_ID"].ToString();
                                    row1["TBT_SHORT_DESC"] = shortdesc ;
                                    row1["TBT_CUSTOM_NUM_FIELD3"] = custnum;
                                    row1["TBT_PARENT_CATEGORY_IMAGE"] = image_string;
                                    row1["EA_PATH"] = HttpContext.Current.Session["EA"].ToString();


                                    cntchk = cntchk + 1;
                                    row1["URL_RW_PATH"] = objhelper.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + "////" + row1["EA_PATH"].ToString() + "////" + row1["TBT_PARENT_CATEGORY_NAME"].ToString() + "////" + row1["CATEGORY_NAME"].ToString(), "pl.aspx",false);
                                    if (cntchk <= cnt)
                                    {
                                        row1["PART1"] = "1";
                                    }
                                    else
                                        row1["PART1"] = "2";

                                    Sub_Category.Rows.Add(row1);
                                }
                            }
                        }
                        Main_Category.Rows.Add(row);
                   }
                }

                if (subdt != null && subdt.Rows.Count > 0)
                {


                    foreach (DataRow item1 in subdt.Rows )
                        {
                            if (item1["Publish"].ToString() == "True" || item1["Publish"].ToString() == "1")
                            {
                                DataRow row1 = Category_Menu.NewRow();
                                row1["TBT_PARENT_CATEGORY_NAME"] = (item1["PARENT_CATEGORY_NAME"].ToString().ToUpper() != "WESNEWS") ? item1["PARENT_CATEGORY_NAME"].ToString() : "New Products";
                                row1["TBT_PARENT_CATEGORY_ID"] = item1["PARENT_CATEGORY"].ToString();
                                row1["CATEGORY_NAME"] = item1["CATEGORY_NAME"].ToString();
                                row1["CATEGORY_ID"] = item1["CATEGORY_ID"].ToString();
                                row1["EA_PATH"] = item1["EA_PATH"].ToString();
                                row1["URL_RW_PATH"] = objhelper.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + "////" + item1["EA_PATH"].ToString(), "pl.aspx", false);

                                Category_Menu.Rows.Add(row1);
                            }
                        }
                    
                }


            }
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE CATEGORY DETAILS FROM DATABASE  ***/
        /********************************************************************************/        
        public void GetDBCategoryDetails()
        {
            DataTable Sqltb = new DataTable();
            string image_string="";
           // StrSql = "Select CATEGORY_ID,SHORT_DESC,IMAGE_FILE,IMAGE_FILE2,isnull(CUSTOM_NUM_FIELD3,0) as CUSTOM_NUM_FIELD3 from tb_Category where Category_ID in (";
            StrSql = "";
            string CatIds = string.Empty;
            foreach (DataRow Dr in MainCategory.Tables[0].Rows)
            {
                CatIds = CatIds + "'" + Dr["CATEGORY_ID"].ToString() + "',";
            }
            if (CatIds != "")
                CatIds = CatIds.Substring(0, CatIds.Length - 1) + "";

            //Sqltb = objhelper.GetDataTable(StrSql);
            Sqltb =(DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, CatIds,"GET_CATEGORY",HelperDB.ReturnType.RTTable);

            if (Sqltb != null)
            {

                foreach (DataRow Dr in MainCategory.Tables[0].Rows)
                {
                    DataRow[] row = Sqltb.Select("CATEGORY_ID='" + Dr["CATEGORY_ID"] + "'");
                    if (row.Length>0) 
                    {
                        DataTable Dt=row.CopyToDataTable();
                        image_string=Dt.Rows[0]["IMAGE_FILE"].ToString();
                        if(image_string!="")
                            image_string = objhelper.SetImageFolderPath(image_string.Replace("\\", "/"), "_Images", "_th");

                         Dr["SHORT_DESC"] = Dt.Rows[0]["SHORT_DESC"];
                         Dr["IMAGE_FILE"] = image_string;// Dt.Rows[0]["IMAGE_FILE"];

                            Dr["IMAGE_FILE2"] = Dt.Rows[0]["IMAGE_FILE2"];
                            Dr["IMAGE_NAME"] = Dt.Rows[0]["IMAGE_NAME"];
                            Dr["IMAGE_NAME2"] = Dt.Rows[0]["IMAGE_NAME2"];
                            Dr["CUSTOM_TEXT_FIELD2"] = Dt.Rows[0]["CUSTOM_TEXT_FIELD2"];
                           

                            Dr["CUSTOM_NUM_FIELD3"] = ((int)float.Parse(Dt.Rows[0]["CUSTOM_NUM_FIELD3"].ToString())) ;
                            foreach (DataRow Dr1 in SubCategory.Tables[0].Rows)
                            {
                                if (Dr1["TBT_PARENT_CATEGORY_ID"].ToString().ToUpper() == Dr["CATEGORY_ID"].ToString().ToUpper())
                                {
                                    Dr1["TBT_SHORT_DESC"] = Dt.Rows[0]["SHORT_DESC"];
                                    Dr1["TBT_PARENT_CATEGORY_IMAGE"] = Dt.Rows[0]["IMAGE_FILE"];
                                    Dr1["TBT_CUSTOM_NUM_FIELD3"] = ((int)float.Parse(Dt.Rows[0]["CUSTOM_NUM_FIELD3"].ToString()));
                                }

                            }
                    }
                    
                }
                /*********************************** OLD CODE ***********************************/
                //foreach (DataRow dbDr in Sqltb.Rows)
                //{


                //    foreach (DataRow Dr in MainCategory.Tables[0].Rows)
                //    {
                //        if (Dr["CATEGORY_ID"] == dbDr["CATEGORY_ID"])
                //        {
                //            Dr["SHORT_DESC"] = dbDr["SHORT_DESC"];
                //            Dr["IMAGE_FILE"] = dbDr["IMAGE_FILE"];
                //            Dr["IMAGE_FILE2"] = dbDr["IMAGE_FILE2"];
                //            Dr["CUSTOM_NUM_FIELD3"] = dbDr["CUSTOM_NUM_FIELD3"];
                //            break;
                //        }
                //    }
                //}
                /*********************************** OLD CODE ***********************************/
            }


        }
        DataSet BrandModelPro = new DataSet();
        DataTable BrandModelFamPro = new DataTable("FamilyPro");


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT BRAND AND MODEL DETAILS ***/
        /********************************************************************************/

        public DataSet GetBrandAndModelProducts(string ParentCatName, string Model, string Brand,string resultPerPage, string CurrentPageNo,string NextPage  )
        {           
            try
            {
                string tmpCatPath = string.Empty;
                if (ParentCatName != string.Empty)
                {
                    tmpCatPath = "////" + ParentCatName + "////AttribSelect=Brand='"+Brand+"'";
                }

                IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
                IOptions opts = ea.getOptions();
                opts.setResultsPerPage(resultPerPage); // ea_rpp.Value);   // use current settings
                opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
                opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);//"Category;1000"
                opts.setSubCategories(false);
                opts.setNavigateHierarchy(false);
                opts.setReturnSKUs(true);
                int PriceCode= GetPriceCode();
                if (PriceCode != -1)
                    opts.setCallOutParam("&eap_PriceCode=" + PriceCode.ToString());
                
               // Create_Brand_Product_Table_Columns();

                BrandModelPro.Tables.Add("TOTAL_PAGES");
                BrandModelPro.Tables["TOTAL_PAGES"].Columns.Add("TOTAL_PAGES", typeof(int));

                //For Total Products.
                BrandModelPro.Tables.Add("TOTAL_PRODUCTS");
                BrandModelPro.Tables["TOTAL_PRODUCTS"].Columns.Add("TOTAL_PRODUCTS", typeof(string));



                BrandModelPro.Tables.Add(BrandModelFamPro);
                BrandModelFamPro.Columns.Add("FAMILY_ID", typeof(string));
                BrandModelFamPro.Columns.Add("CATEGORY_ID", typeof(string));
                BrandModelFamPro.Columns.Add("PARENT_CATEGORY_ID", typeof(string));
                BrandModelFamPro.Columns.Add("FAMILY_NAME", typeof(string));
                BrandModelFamPro.Columns.Add("PRODUCT_ID", typeof(string));
                BrandModelFamPro.Columns.Add("PRODUCT_CODE", typeof(string));
                BrandModelFamPro.Columns.Add("PRODUCT_PRICE", typeof(string));
                BrandModelFamPro.Columns.Add("ATTRIBUTE_ID", typeof(string));
                BrandModelFamPro.Columns.Add("STRING_VALUE", typeof(string));
                BrandModelFamPro.Columns.Add("NUMERIC_VALUE", typeof(string));
                BrandModelFamPro.Columns.Add("OBJECT_TYPE", typeof(string));
                BrandModelFamPro.Columns.Add("OBJECT_NAME", typeof(string));
                BrandModelFamPro.Columns.Add("ATTRIBUTE_NAME", typeof(string));
                BrandModelFamPro.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
                BrandModelFamPro.Columns.Add("SUBCATNAME_L1", typeof(string));
                BrandModelFamPro.Columns.Add("SUBCATNAME_L2", typeof(string));
                BrandModelFamPro.Columns.Add("CATEGORY_NAME", typeof(string));
                BrandModelFamPro.Columns.Add("PARENT_CATEGORY_NAME", typeof(string));
                BrandModelFamPro.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
                BrandModelFamPro.Columns.Add("SUB_FAMILY_COUNT", typeof(string));
                BrandModelFamPro.Columns.Add("PRODUCT_COUNT", typeof(string));                
                BrandModelFamPro.Columns.Add("FAMILY_PRODUCT_COUNT", typeof(string));
                BrandModelFamPro.Columns.Add("QTY_AVAIL", typeof(string));
                BrandModelFamPro.Columns.Add("MIN_ORD_QTY", typeof(string));
                
                BrandModelPro.Tables.Add("Category");
                //BrandModelPro.Tables["Category"].Columns.Add("SUBCATID_L1", typeof(string));
                //BrandModelPro.Tables["Category"].Columns.Add("SUBCATNAME_L1", typeof(string));
                 BrandModelPro.Tables["Category"].Columns.Add("CATALOG_ID", typeof(string));
                 BrandModelPro.Tables["Category"].Columns.Add("PRODUCT_ID", typeof(string));
                 BrandModelPro.Tables["Category"].Columns.Add("STATUS", typeof(string));

                INavigateResults res = null;
 
                //updateBreadCrumb(res.getBreadCrumbTrail());


                if (int.Parse(CurrentPageNo) <= 0)
                {
                    res = ea.userAttributeClick_Brand("AllProducts////WESAUSTRALASIA" + tmpCatPath, "Model = '" + Brand + ":" +Model+ "'");
                    HttpContext.Current.Session["EA"] = res.getCatPath();
                   
                }
                else
                {
                    res = ea.userPageOp(HttpContext.Current.Session["EA"].ToString(), CurrentPageNo, NextPage);
                    
                }

                HttpContext.Current.Session["EA"] = res.getCatPath();
                CreateYouHaveSelectAndBreadCrumb(true);
                DataRow dr = BrandModelPro.Tables["TOTAL_PAGES"].NewRow();
                dr[0] = res.getPageCount();
                BrandModelPro.Tables["TOTAL_PAGES"].Rows.Add(dr);

                DataRow dr1 = BrandModelPro.Tables["TOTAL_PRODUCTS"].NewRow();
                dr1[0] = res.getTotalItems();
                BrandModelPro.Tables["TOTAL_PRODUCTS"].Rows.Add(dr1);
              
                //Get_Family_Product_Values("ByBrand",b BrandModelFamPro, res, null, null);

                IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
                HttpContext.Current.Session["LHSAttributes"] = GetCategoryAttribute(Attributes, res, "", "");
                            

                HttpContext.Current.Session["Brand_Model_Product_DS"] = BrandModelPro;
                return BrandModelPro;
            }
            catch (Exception ex)
            {
                objErrorhandler.ErrorMsg = ex;
                objErrorhandler.CreateLog();
                return null;
            }

        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE FAMILY PRODUCT DETAILS ***/
        /********************************************************************************/
        private DataTable Get_Family_Product_Values(string DataType, DataTable ds, INavigateResults res, IResultRow item1, String name)
        {
           
            int last = res.getLastItem();
            int colFmlyID = res.getColumnIndex("Family Id");
            int colSubFmlyID = res.getColumnIndex("SubFamily Id");
            int colFmlyName = res.getColumnIndex("Family Name");
            int colFmlyDesc = res.getColumnIndex("Family ShortDescription");
            int colFmlylongDesc = res.getColumnIndex("Family Description");
            int colFmlyImg = res.getColumnIndex("Family Thumbnail");

            int colProductID = res.getColumnIndex("Prod Id");
           int colProductCode = res.getColumnIndex("Prod Code");
          //  int colProductCode = res.getColumnIndex("Wag Prod Code");
            int colProductPrice = res.getColumnIndex("Price");
            int colProductDesc = res.getColumnIndex("Prod Description");
            int colProductImg = res.getColumnIndex("Prod Thumbnail");

            int colProductCount = res.getColumnIndex("Prod Count");
            int colFamilyProdCount = res.getColumnIndex("Family Prod Count");
            int colSubFamilyCount = res.getColumnIndex("SubFamily Count");
            int colminPrice = res.getColumnIndex("minPrice");
            string _ProCode = string.Empty;
            string _ProductPrice = string.Empty;
            string _FmlyDesc = string.Empty;
            string _ProductDesc = string.Empty;
            string _FmlylongDesc = string.Empty;
            string image_string = string.Empty;
            string tobedeletepids = string.Empty;
            int sno = 0;
            //IList<INavigateCategory> item = res.getDetailedCategories();


           


            DataRow dRow;
            try
            {
                if (last >= 0)
                {
                    
                        for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++)
                        {
                            sno = sno + 1;
                            dRow = ds.NewRow();

                            dRow["FAMILY_ID"] = res.getCellData(i, colFmlyID);
                            dRow["SUB_FAMILY_ID"] = res.getCellData(i, colSubFmlyID);
                            dRow["FAMILY_NAME"] = res.getCellData(i, colFmlyName);
                            //dRow["DESCRIPTION1"] = res.getCellData(i, colFmlyDesc);
                            //dRow["LongDESCRIPTION"] = res.getCellData(i, colFmlylongDesc);
                            dRow["PRODUCT_ID"] = res.getCellData(i, colProductID);
                            dRow["PRODUCT_CODE"] = res.getCellData(i, colProductCode);

                            _ProductPrice = res.getCellData(i, colProductPrice);
                            if (_ProductPrice == "" || _ProductPrice == string.Empty)
                                dRow["PRODUCT_PRICE"] = "0";
                            else
                                dRow["PRODUCT_PRICE"] = _ProductPrice.Substring(1);

                            dRow["MIN_PRICE"] = res.getCellData(i, colminPrice);

                            string temp_family_count = res.getCellData(i, colFamilyProdCount);

                            string temp_product_count = res.getCellData(i, colProductCount);
                            string temp_subfamily_count = res.getCellData(i, colSubFamilyCount);
                            string temp_fmly_Image = res.getCellData(i, colFmlyImg).ToString();
                            string temp_product_Image = res.getCellData(i, colProductImg).ToString();

                            dRow["SUB_FAMILY_COUNT"] = (temp_subfamily_count == null || temp_subfamily_count == "") ? "0" : temp_subfamily_count;
                            dRow["PRODUCT_COUNT"] = (temp_product_count == null || temp_product_count == "") ? "0" : temp_product_count;
                            dRow["FAMILY_PRODUCT_COUNT"] = (temp_family_count == null || temp_family_count == "") ? "0" : temp_family_count;


                            image_string = "";
                            if (temp_product_count.ToString() == "1")
                            {
                                if (temp_product_Image != "")
                                    image_string = temp_product_Image.Substring(42);
                                else if (temp_fmly_Image != "")
                                    image_string = temp_fmly_Image.Substring(42);
                                else
                                    image_string = "noimage.gif";
                            }
                            else
                            {
                                if (temp_fmly_Image != "")
                                    image_string = temp_fmly_Image.Substring(42);
                                else
                                    image_string = "noimage.gif";

                            }
                            if (image_string != "")
                                image_string = objhelper.SetImageFolderPath(image_string.Replace("\\", "/"), "_th", "_th");
                            //if (temp_fmly_Image != "" && temp_product_Image != "")
                            //{
                            //    if (temp_product_count.ToString() == "1")
                            //    {
                            //        image_string = temp_product_Image.Substring(42);
                            //    }
                            //    else
                            //    {
                            //        image_string = temp_fmly_Image.Substring(42);
                            //    }
                            //}
                            //else
                            //{
                            //    image_string = "noimage.gif";
                            //}
                            _ProCode = res.getCellData(i, colProductCode);
                            _ProductPrice = res.getCellData(i, colProductPrice);

                            _FmlyDesc = res.getCellData(i, colFmlyDesc);
                            _ProductDesc = res.getCellData(i, colProductDesc);
                            _FmlylongDesc = res.getCellData(i, colFmlylongDesc);
                            dRow["SUBCATNAME_L1"] = "";
                            dRow["SUBCATNAME_L2"] = "";
                            dRow["CATEGORY_NAME"] = "";
                            dRow["PARENT_CATEGORY_NAME"] = "";

                            dRow["CATEGORY_ID"] = "";
                            dRow["PARENT_CATEGORY_ID"] = "";
                            dRow["CUSTOM_NUM_FIELD3"] = "";
                            dRow["QTY_AVAIL"] = "-1";
                            dRow["MIN_ORD_QTY"] = "-1";
                            dRow["SNO"] = sno;



                            if (DataType == "ByBrand" || DataType == "ps")
                            {
                                //for (int k = 0; k < 9; k++)
                               // {
                                    //if (k == 0)
                                    //{
                                        dRow["ATTRIBUTE_ID"] = "1";
                                        dRow["STRING_VALUE"] = _ProCode; //For the Product Code
                                        dRow["NUMERIC_VALUE"] = "0";
                                        dRow["OBJECT_TYPE"] = "NULL";
                                        dRow["OBJECT_NAME"] = "NULL";
                                        dRow["ATTRIBUTE_NAME"] = "Code";
                                        dRow["ATTRIBUTE_TYPE"] = "1";
                                        ds.Rows.Add(dRow.ItemArray);
                                    //}
                                    //if (k == 1)
                                    //{


                                        dRow["ATTRIBUTE_ID"] = "5";
                                        dRow["STRING_VALUE"] = "";
                                        if (_ProductPrice == "" || _ProductPrice == string.Empty)
                                        {
                                            dRow["NUMERIC_VALUE"] = "0";
                                        }
                                        else
                                        {
                                            dRow["NUMERIC_VALUE"] = _ProductPrice.Substring(1);//For Cost
                                        }
                                        dRow["OBJECT_TYPE"] = "NULL";
                                        dRow["OBJECT_NAME"] = "NULL";
                                        dRow["ATTRIBUTE_NAME"] = "Cost";
                                        dRow["ATTRIBUTE_TYPE"] = "4";
                                        ds.Rows.Add(dRow.ItemArray);
                                    //}
                                    //if (k == 2)
                                    //{
                                        dRow["ATTRIBUTE_ID"] = "62";
                                        dRow["STRING_VALUE"] = _FmlyDesc;//Family Description
                                        dRow["NUMERIC_VALUE"] = "0";
                                        dRow["OBJECT_TYPE"] = "NULL";
                                        dRow["OBJECT_NAME"] = "NULL";
                                        dRow["ATTRIBUTE_NAME"] = "Description";
                                        dRow["ATTRIBUTE_TYPE"] = "7";
                                        ds.Rows.Add(dRow.ItemArray);
                                    //}
                                    //if (k == 3)
                                    //{
                                        dRow["ATTRIBUTE_ID"] = "449";
                                        dRow["STRING_VALUE"] = _ProductDesc;//Product Description.
                                        dRow["NUMERIC_VALUE"] = "0";
                                        dRow["OBJECT_TYPE"] = "NULL";
                                        dRow["OBJECT_NAME"] = "NULL";
                                        dRow["ATTRIBUTE_NAME"] = "PROD_DSC";
                                        dRow["ATTRIBUTE_TYPE"] = "1";
                                        ds.Rows.Add(dRow.ItemArray);
                                    //}
                                    //if (k == 4)
                                    //{
                                        dRow["ATTRIBUTE_ID"] = "492";
                                        dRow["STRING_VALUE"] = "";
                                        if (_ProductPrice == "" || _ProductPrice == string.Empty)
                                        {
                                            dRow["NUMERIC_VALUE"] = "0";
                                        }
                                        else
                                        {
                                            dRow["NUMERIC_VALUE"] = _ProductPrice.Substring(1);//For Cost
                                        }
                                        dRow["OBJECT_TYPE"] = "NULL";
                                        dRow["OBJECT_NAME"] = "NULL";
                                        dRow["ATTRIBUTE_NAME"] = "PROD_EXT_PRI_3";
                                        dRow["ATTRIBUTE_TYPE"] = "4";
                                        ds.Rows.Add(dRow.ItemArray);
                                    //}
                                    //if (k == 5)
                                    //{
                                        dRow["ATTRIBUTE_ID"] = "453";
                                        dRow["STRING_VALUE"] = image_string;
                                        dRow["NUMERIC_VALUE"] = "0";
                                        dRow["OBJECT_TYPE"] = "jpg";
                                        dRow["OBJECT_NAME"] = image_string;
                                        dRow["ATTRIBUTE_NAME"] = "Web Image1";
                                        dRow["ATTRIBUTE_TYPE"] = "3";
                                        ds.Rows.Add(dRow.ItemArray);
                                    //}
                                    //if (k == 6)
                                    //{
                                        dRow["ATTRIBUTE_ID"] = "7";
                                        dRow["STRING_VALUE"] = image_string;
                                        dRow["NUMERIC_VALUE"] = "0";
                                        dRow["OBJECT_TYPE"] = "jpg";
                                        dRow["OBJECT_NAME"] = image_string;
                                        dRow["ATTRIBUTE_NAME"] = "Product Image1";
                                        dRow["ATTRIBUTE_TYPE"] = "3";
                                        ds.Rows.Add(dRow.ItemArray);
                                    //}
                                    //if (k == 7)
                                    //{
                                        dRow["ATTRIBUTE_ID"] = "452";
                                        dRow["STRING_VALUE"] = image_string;
                                        dRow["NUMERIC_VALUE"] = "0";
                                        dRow["OBJECT_TYPE"] = "jpg";
                                        dRow["OBJECT_NAME"] = image_string;
                                        dRow["ATTRIBUTE_NAME"] = "TWeb Image1";
                                        dRow["ATTRIBUTE_TYPE"] = "3";
                                        ds.Rows.Add(dRow.ItemArray);
                                    //}
                                    //if (k == 8)
                                    //{
                                        dRow["ATTRIBUTE_ID"] = "4";
                                        dRow["STRING_VALUE"] = _FmlylongDesc;//Family Description
                                        dRow["NUMERIC_VALUE"] = "0";
                                        dRow["OBJECT_TYPE"] = "NULL";
                                        dRow["OBJECT_NAME"] = "NULL";
                                        dRow["ATTRIBUTE_NAME"] = "DESCRIPTIONS";
                                        dRow["ATTRIBUTE_TYPE"] = "7";
                                        ds.Rows.Add(dRow.ItemArray);
                                    //}

                                //}
                            }
                            else if (DataType == "ProductList" || DataType == "CategoryProductList")
                            {

                                dRow["ATTRIBUTE_ID"] = "1";
                                dRow["STRING_VALUE"] = _ProCode; //For the Product Code
                                dRow["NUMERIC_VALUE"] = "0";
                                dRow["OBJECT_TYPE"] = "NULL";
                                dRow["OBJECT_NAME"] = "NULL";
                                dRow["ATTRIBUTE_NAME"] = "Code";
                                dRow["ATTRIBUTE_TYPE"] = "1";
                                ds.Rows.Add(dRow.ItemArray);

                                //------------------

                                dRow["ATTRIBUTE_ID"] = "0";
                                dRow["STRING_VALUE"] = "";
                                if (_ProductPrice == "" || _ProductPrice == string.Empty)
                                {
                                    dRow["NUMERIC_VALUE"] = "0";
                                }
                                else
                                {
                                    dRow["NUMERIC_VALUE"] = _ProductPrice.Substring(1);//For Cost
                                }
                                dRow["OBJECT_TYPE"] = "NULL";
                                dRow["OBJECT_NAME"] = "NULL";
                                dRow["ATTRIBUTE_NAME"] = "Cost";
                                dRow["ATTRIBUTE_TYPE"] = "0";
                                ds.Rows.Add(dRow.ItemArray);

                                //------------------
                                dRow["ATTRIBUTE_ID"] = "747";
                                dRow["STRING_VALUE"] = image_string;
                                dRow["NUMERIC_VALUE"] = "0";
                                dRow["OBJECT_TYPE"] = "jpg";
                                dRow["OBJECT_NAME"] = image_string;
                                dRow["ATTRIBUTE_NAME"] = "Product Image1";
                                dRow["ATTRIBUTE_TYPE"] = "9";
                                ds.Rows.Add(dRow.ItemArray);
                                //------------------
                                dRow["ATTRIBUTE_ID"] = "90";
                                dRow["STRING_VALUE"] = _FmlylongDesc;//Family Description
                                dRow["NUMERIC_VALUE"] = "0";
                                dRow["OBJECT_TYPE"] = "NULL";
                                dRow["OBJECT_NAME"] = "NULL";
                                dRow["ATTRIBUTE_NAME"] = "DESCRIPTIONS";
                                dRow["ATTRIBUTE_TYPE"] = "7";
                                ds.Rows.Add(dRow.ItemArray);
                                //------------------
                                dRow["ATTRIBUTE_ID"] = "13";
                                dRow["STRING_VALUE"] = _FmlyDesc;//Family Description
                                dRow["NUMERIC_VALUE"] = "0";
                                dRow["OBJECT_TYPE"] = "NULL";
                                dRow["OBJECT_NAME"] = "NULL";
                                dRow["ATTRIBUTE_NAME"] = "DESCRIPTIONS";
                                dRow["ATTRIBUTE_TYPE"] = "7";
                                ds.Rows.Add(dRow.ItemArray);


                            }
                            j++;
                        }
                        // Get Family Category & Sub Category Name from DB
                        DataTable Sqltb = new DataTable();
                        tmpstr = "";
                       // StrSql = "Select A.FAMILY_ID,isnull(B.CATEGORY_NAME,'') as SubCat , isnull(C.CATEGORY_NAME,'') as ParentCat,isnull(B.CATEGORY_ID,'') as SubCatID, isnull(C.CATEGORY_ID,'') as ParentCatID,isnull(B.CUSTOM_NUM_FIELD3,0) as CUSTOM_NUM_FIELD3 from TB_CATALOG_FAMILY A " +
                       //          " Left Outer Join  TB_CATEGORY B on A.CATEGORY_ID=B.CATEGORY_ID " +
                       //          " Left Outer Join TB_CATEGORY C   on B.PARENT_CATEGORY=C.CATEGORY_ID " +
                       //          " Where A.CATALOG_ID=2 And FAMILY_ID in (";

                        foreach (DataRow dr1 in ds.Rows)
                        {
                            if (tmpstr.Contains(dr1["FAMILY_ID"].ToString().ToUpper()) == false)
                            tmpstr = tmpstr + "'" + dr1["FAMILY_ID"].ToString().ToUpper() + "',";
                        }
                        if (tmpstr != "")
                            tmpstr = tmpstr.Substring(0, tmpstr.Length - 1) + "";

                        if (tmpstr != "")
                        {
                            //Sqltb = objhelper.GetDataTable(StrSql);
                            Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, tmpstr, "GET_FAMILY_CATEGORY", HelperDB.ReturnType.RTTable);
                            if (Sqltb != null)
                            {
                                foreach (DataRow dr in Sqltb.Rows)
                                {
                                    foreach (DataRow dr1 in ds.Rows)
                                    {
                                        if (dr["FAMILY_ID"].ToString().ToUpper() == dr1["FAMILY_ID"].ToString().ToUpper())
                                        {
                                            if (DataType == "ByBrand")
                                            {
                                                dr1["SUBCATNAME_L1"] = dr["SubCat"];
                                                dr1["PARENT_CATEGORY_NAME"] = dr["SubCat"];

                                                dr1["SUBCATNAME_L2"] = dr["SubCat"];
                                                dr1["CATEGORY_NAME"] = dr["SubCat"];

                                                dr1["CATEGORY_ID"] = dr["SubCatID"];
                                                dr1["PARENT_CATEGORY_ID"] = dr["SubCatID"];
                                            }
                                            else
                                            {
                                                dr1["SUBCATNAME_L1"] = dr["ParentCat"];
                                                dr1["PARENT_CATEGORY_NAME"] = dr["ParentCat"];

                                                dr1["SUBCATNAME_L2"] = dr["SubCat"];
                                                dr1["CATEGORY_NAME"] = dr["SubCat"];

                                                dr1["CATEGORY_ID"] = dr["SubCatID"];
                                                dr1["PARENT_CATEGORY_ID"] = dr["ParentCatID"];
                                            }
                                            dr1["CUSTOM_NUM_FIELD3"] = ((int)float.Parse(dr["CUSTOM_NUM_FIELD3"].ToString())).ToString();
                                        }
                                    }
                                }
                            }
                        }
                        // Get QTY_AVAIL,MIN_ORD_QTY from DB -- TBWC_INVENTORY

                        //StrSql = "SELECT PRODUCT_ID,QTY_AVAIL,MIN_ORD_QTY FROM TBWC_INVENTORY " +
                        //         " Where PRODUCT_ID in (";

                         tobedeletepids = "";
                        tmpstr = "";
                        foreach (DataRow dr1 in ds.Rows)
                        {
                            if (tmpstr.Contains(dr1["PRODUCT_ID"].ToString().ToUpper()) == false)
                            tmpstr = tmpstr + dr1["PRODUCT_ID"].ToString().ToUpper() + ",";
                        }
                        if (tmpstr != "")
                            tmpstr = tmpstr.Substring(0, tmpstr.Length - 1) + "";

                        if (tmpstr != "")
                        {
                           // Sqltb = objhelper.GetDataTable(StrSql);
                            //Sqltb = (DataTable)objhelperDb.GetGenericDataDB(tmpstr, "GET_PRODUCT_INVENTORY", HelperDB.ReturnType.RTTable);
                            string userid = HttpContext.Current.Session["USER_ID"].ToString();
                            DataSet tmpDs = new DataSet();
                            tmpDs = objhelperDb.GetProductPrice("", tmpstr, userid);
                            if (tmpDs != null)
                            {
                                Sqltb = tmpDs.Tables[0];
                                if (Sqltb != null)
                                {
                                    foreach (DataRow dr in Sqltb.Rows)
                                    {
                                        foreach (DataRow dr1 in ds.Rows)
                                        {
                                            if (dr["PRODUCT_ID"].ToString().ToUpper() == dr1["PRODUCT_ID"].ToString().ToUpper())
                                            {
                                                dr1["QTY_AVAIL"] = dr["QTY_AVAIL"].ToString();
                                                dr1["MIN_ORD_QTY"] = dr["MIN_ORD_QTY"].ToString();
                                                dr1["STOCK_STATUS_DESC"] = dr["PROD_STK_STATUS_DSC"].ToString();

                                                if (dr["isPromotion"].ToString() == "Y")
                                                {
                                                    tobedeletepids = tobedeletepids + dr["PRODUCT_ID"].ToString() + ",";
                                                    //objErrorhandler.CreateLog(tobedeletepids);   
                                                }

                                                /* DB Call Price
                                                if (dr1["ATTRIBUTE_ID"].ToString() == "0" || dr1["ATTRIBUTE_ID"].ToString() == "5" || dr1["ATTRIBUTE_ID"].ToString() == "492")
                                                {
                                                    dr1["NUMERIC_VALUE"] = dr["Price"].ToString();
                                                }
                                                 DB Call Price */
                                            }
                                        }
                                    }
                                }
                            }
                        }           
         


                    }

                if (tobedeletepids != "")
                {

                    tobedeletepids = tobedeletepids.Substring(0, tobedeletepids.Length - 1) + "";

                }
                DataRow[] rows;
                rows = ds.Select("Product_ID in (" + tobedeletepids + ")");
                foreach (DataRow r in rows)
                {
                    //objErrorhandler.CreateLog("Inside delete");   
                    r.Delete();
                }


            }
            catch (Exception ex)
            {
            }
            return ds;
       }
        private string Get_Parentcategory_ID()
        {

            try
            {
                DataSet mainds = new DataSet();
                DataTable dt = new DataTable();
                string querystring = string.Empty;
                string _catCid = string.Empty;
                //if ((Request.QueryString["path"] != null) && (hfisselected.Value == string.Empty))
                //{
                querystring = HttpContext.Current.Request.RawUrl.ToString().ToLower();
               // objErrorhandler.CreateLog(querystring);   
                string[] ConsURL = querystring.Split('/');
                if (HttpContext.Current.Session["MainCategory"] != null)
                {
                    mainds = (DataSet)HttpContext.Current.Session["MainCategory"];
                    dt = mainds.Tables[0];
                    DataRow[] foundRows;
                    string urlpath = string.Empty;
                    if (querystring.Contains("/ct/"))
                    {
                        if (ConsURL.Length == 4)
                        {
                            urlpath = ConsURL[1];
                        }
                        else
                        {
                            urlpath = ConsURL[2];

                        }

                    }
                    else if (querystring.Contains("/pl/") || querystring.Contains("/bb/"))
                    {

                         if (querystring.Contains("/wa-"))
                        {
                            urlpath = ConsURL[2];
                        }
                        else
                        {

                            urlpath = ConsURL[3];
                        }
                    }

                    else if (querystring.Contains("/mct/") ||
      querystring.Contains("/mpl/") ||
    querystring.Contains("/mps/"))
                    {
                        urlpath = ConsURL[1];
                    }
                    foundRows = dt.Select("URL_RW_PATH='" + urlpath + "' ");

                    if (foundRows.Length > 0)
                    {
                        _catCid = foundRows[0]["CATEGORY_ID"].ToString();
                        //objErrorhandler.CreateLog(_catCid);
                        return _catCid;

                    }
                    else
                    {
                        return "";

                    }
                }
                else
                {

                    return "";
                }
            }
            catch
            {
                return "";
            }
        
        }

        private DataTable Get_Family_Product_ValuesJson(string DataType, DataTable ds, INavigateResults res, IResultRow item1, String name, DataSet Dssdv)
        {


            string _ProCode = string.Empty;
            string _ProductPrice = string.Empty;
            string _FmlyDesc = string.Empty;
            string _ProductDesc = string.Empty;
            string _FmlylongDesc = string.Empty;
            string image_string = string.Empty;
            string tobedeletepids = string.Empty;
            int sno = 0;

            string temp_family_count = string.Empty;

            string temp_product_count = string.Empty;
            string temp_subfamily_count = string.Empty;
            string temp_fmly_Image = string.Empty;
            string temp_product_Image = string.Empty;
            string tmpstrPid = string.Empty;
            tmpstr = "";
            try
            {

                if (Dssdv != null && Dssdv.Tables.Count > 0 && Dssdv.Tables["items"] != null && Dssdv.Tables["items"].Rows.Count > 0)
                {
                    ds = Dssdv.Tables["items"].Copy();
                    ds.TableName = "FamilyPro";
                    ds.Columns["Prod_Id"].ColumnName = "PRODUCT_ID";
                    ds.Columns["SubFamily_Id"].ColumnName = "SUB_FAMILY_ID";
                    ds.Columns["Prod_Count"].ColumnName = "PRODUCT_COUNT";
                    ds.Columns["Family_Prod_Count"].ColumnName = "FAMILY_PRODUCT_COUNT";
                    ds.Columns["SubFamily_Count"].ColumnName = "SUB_FAMILY_COUNT";
                    ds.Columns["minPrice"].ColumnName = "MIN_PRICE";
                    ds.Columns["Prod_Code"].ColumnName = "PRODUCT_CODE";
                    ds.Columns["Price"].ColumnName = "PRODUCT_PRICE";
                    
                    ds.Columns["QtyAvail"].ColumnName = "QTY_AVAIL";
                    ds.Columns["MinOrdQty"].ColumnName = "MIN_ORD_QTY";
                    ds.Columns["Prod_Stock_Status"].ColumnName = "PROD_STOCK_STATUS";
                    ds.Columns["Prod_Stock_Flag"].ColumnName = "PROD_STOCK_FLAG";
                    ds.Columns["Prod_Stk_Status_Dsc"].ColumnName = "STOCK_STATUS_DESC";
                    ds.Columns["Promotion_Product"].ColumnName = "isPromotion";
                    ds.Columns["CATEGORY_PATH"].ColumnName = "CATEGORY_PATH";

                    
                    ds.Columns.Add("CATEGORY_ID", typeof(string));
                    ds.Columns.Add("PARENT_CATEGORY_ID", typeof(string));
                    ds.Columns.Add("SUBCATNAME_L1", typeof(string));
                    ds.Columns.Add("SUBCATNAME_L2", typeof(string));
                    ds.Columns.Add("CATEGORY_NAME", typeof(string));
                    ds.Columns.Add("PARENT_CATEGORY_NAME", typeof(string));
                    ds.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));

                    //DataColumn dc = new DataColumn("QTY_AVAIL", typeof(string));
                    //dc.DefaultValue ="100";
                    //ds.Columns.Add(dc);

                    //dc = new DataColumn("MIN_ORD_QTY", typeof(string));
                    //dc.DefaultValue = "1";
                    //ds.Columns.Add(dc);

                    //ds.Columns.Add("STOCK_STATUS_DESC", typeof(string));
                    //ds.Columns.Add("PROD_STOCK_STATUS", typeof(string));
                    //ds.Columns.Add("PROD_STOCK_FLAG", typeof(string));
                    ds.Columns.Add("ETA", typeof(string));
                   // ds.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
                   // ds.Columns.Add("CATEGORY_PATH", typeof(string));

                    ds.Columns.Add("SNO", typeof(int));


                    foreach (DataRow dr in ds.Rows)
                    {
                        sno = sno + 1;

                        //if (dr["PRODUCT_PRICE"].ToString() == "" || dr["PRODUCT_PRICE"].ToString() == string.Empty)
                        //    dr["PRODUCT_PRICE"] = "0";
                        //else
                        //    dr["PRODUCT_PRICE"] = dr["PRODUCT_PRICE"].ToString().Substring(1);


                        if (dr["PRODUCT_PRICE"].ToString().Contains("$"))
                            dr["PRODUCT_PRICE"] = dr["PRODUCT_PRICE"].ToString().Substring(1);

                        dr["SUB_FAMILY_COUNT"] = (dr["SUB_FAMILY_COUNT"].ToString() == null || dr["SUB_FAMILY_COUNT"].ToString() == "") ? "0" : dr["SUB_FAMILY_COUNT"].ToString();
                        dr["PRODUCT_COUNT"] = (dr["PRODUCT_COUNT"].ToString() == null || dr["PRODUCT_COUNT"].ToString() == "") ? "0" : dr["PRODUCT_COUNT"].ToString();
                        dr["FAMILY_PRODUCT_COUNT"] = (dr["FAMILY_PRODUCT_COUNT"].ToString() == null || dr["FAMILY_PRODUCT_COUNT"].ToString() == "") ? "0" : dr["FAMILY_PRODUCT_COUNT"].ToString();




                        temp_product_count = dr["PRODUCT_COUNT"].ToString();

                        temp_fmly_Image = dr["Family_Thumbnail"].ToString();
                        temp_product_Image = dr["Prod_Thumbnail"].ToString();


                        image_string = "";
                        if (temp_product_count.ToString() == "1")
                        {
                            if (temp_product_Image != string.Empty)
                                image_string = temp_product_Image.Substring(42);
                            else if (temp_fmly_Image != "")
                                image_string = temp_fmly_Image.Substring(42);
                            else
                                image_string = "/noimage.gif";
                        }
                        else
                        {
                            if (temp_fmly_Image != string.Empty)
                                image_string = temp_fmly_Image.Substring(42);
                            else
                                image_string = "/noimage.gif";

                        }
                        if (image_string != string.Empty)
                            image_string = objhelper.SetImageFolderPath(image_string.Replace("\\", "/"), "_th", "_Images_200");

                        dr["Prod_Thumbnail"] = image_string;
                        dr["SNO"] = sno;

                        if (dr["PROD_STOCK_STATUS"] == "")
                            dr["PROD_STOCK_STATUS"] = "0";

                        if (dr["STOCK_STATUS_DESC"] == "")
                            dr["STOCK_STATUS_DESC"] = "NO STATUS AVAILABLE";
                        
                        if (dr["PROD_STOCK_FLAG"] == "")
                            dr["PROD_STOCK_FLAG"] = "0";

                        if (dr["QTY_AVAIL"] == "")
                            dr["QTY_AVAIL"] = "100";
                        
               

                        if (dr["MIN_ORD_QTY"] == "")
                            dr["MIN_ORD_QTY"] = "1";

                        if (dr["isPromotion"] == "")
                            dr["isPromotion"] = "N";
                        

                            if (DataType == "ByBrand")
                            {
                                dr["SUBCATNAME_L1"] = dr["Sub_Cat"];
                                dr["PARENT_CATEGORY_NAME"] = dr["Sub_Cat"];

                                dr["SUBCATNAME_L2"] = dr["Sub_Cat"];
                                dr["CATEGORY_NAME"] = dr["Sub_Cat"];

                                dr["CATEGORY_ID"] = dr["Sub_Cat_ID"];
                                dr["PARENT_CATEGORY_ID"] = dr["Sub_Cat_ID"];
                            }
                            else
                            {
                                dr["SUBCATNAME_L1"] = dr["Parent_Cat"];
                                dr["PARENT_CATEGORY_NAME"] = dr["Parent_Cat"];

                                dr["SUBCATNAME_L2"] = dr["Sub_Cat"];
                                dr["CATEGORY_NAME"] = dr["Sub_Cat"];

                                dr["CATEGORY_ID"] = dr["Sub_Cat_ID"];
                                dr["PARENT_CATEGORY_ID"] = dr["Parent_Cat_ID"];
                            }
                            //dr["CATEGORY_PATH"] = dr["CATEGORY_Parent"];
                            if (dr["CustomNumField3"].ToString()=="")
                                dr["CustomNumField3"]="0";

                            dr["CUSTOM_NUM_FIELD3"] = ((int)float.Parse(dr["CustomNumField3"].ToString())).ToString();




                            string dr1fid = string.Empty;
                            dr1fid = dr["FAMILY_ID"].ToString().ToUpper();
                            if (!(tmpstr.Contains(dr1fid)))
                                tmpstr = tmpstr + "'" + dr1fid + "',";
                            string dr1pid = string.Empty;
                            dr1pid = dr["PRODUCT_ID"].ToString().ToUpper();

                            if (!(tmpstrPid.Contains(dr1pid)))
                                tmpstrPid = tmpstrPid + dr1pid + ",";

                    }
                    DataTable Sqltb = new DataTable();
                  

                    DataSet dsbc = new DataSet();
                    DataTable tmptbl = new DataTable();
                    DataTable rtntbl = new DataTable();
                    string strsupplierId = string.Empty;
                    string httpurl = string.Empty;
                    httpurl = HttpContext.Current.Request.Url.ToString().ToLower();
                    //if ((httpurl.Contains("mps.aspx"))
                    //    || (httpurl.Contains("mpd.aspx"))
                    //    || (httpurl.Contains("mct.aspx"))
                    //    || (httpurl.Contains("mfl.aspx"))
                    //    || (httpurl.Contains("mpl.aspx")))
                    //{


                    /*      if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                          {
                              dsbc = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

                          }
                          //modified by indu,For common categoryid
                      

                          strsupplierId = Get_Parentcategory_ID();

                          if (strsupplierId == "")
                          {


                              if (dsbc.Tables[0].Rows[0]["itemType"].ToString().ToLower() == "category")
                              {
                                  if (HttpContext.Current.Session["MainCategory"] != null)
                                  {
                                      tmptbl = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0];
                                      DataRow[] dr = tmptbl.Select("CATEGORY_Name='" + dsbc.Tables[0].Rows[0]["ItemValue"].ToString() + "'");
                                      if (dr.Length > 0)
                                      {
                                          rtntbl = dr.CopyToDataTable();
                                          strsupplierId = rtntbl.Rows[0]["CATEGORY_ID"].ToString();
                                      }

                                  }
                              }
                        
                        
                        
                        
                        
                          }
                      //}
                      foreach (DataRow dr1 in ds.Rows)
                      {
                      
                          string dr1fid = string.Empty;
                          dr1fid = dr1["FAMILY_ID"].ToString().ToUpper();
                          if (!(tmpstr.Contains(dr1fid)))
                              tmpstr = tmpstr + "'" + dr1fid + "',";
                          string dr1pid = string.Empty;
                          dr1pid = dr1["PRODUCT_ID"].ToString().ToUpper();

                          if (!(tmpstrPid.Contains(dr1pid)))
                              tmpstrPid = tmpstrPid + dr1pid + ",";
                      }*/
                    if (tmpstr != string.Empty)
                        tmpstr = tmpstr.Substring(0, tmpstr.Length - 1) + "";

                    //if (tmpstr != string.Empty)
                    //{
                      
                    //    if (strsupplierId!="")


                    //        Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, tmpstr, strsupplierId, "GET_FAMILY_CATEGORY", HelperDB.ReturnType.RTTable);
                    //    else
                    //        Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, tmpstr,"", "GET_FAMILY_CATEGORY", HelperDB.ReturnType.RTTable);

                    //    if (Sqltb == null)
                    //    {
                    //        Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, tmpstr, "", "GET_FAMILY_CATEGORY", HelperDB.ReturnType.RTTable);
                    //    }

                    //    if (Sqltb != null)
                    //    {
                    //        foreach (DataRow dr in Sqltb.Rows)
                    //        {
                               
                    //            foreach (DataRow dr1 in ds.Rows)
                    //            {
                                  

                    //                if (dr["FAMILY_ID"].ToString().ToUpper() == dr1["FAMILY_ID"].ToString().ToUpper())
                    //                {
                    //                    if (DataType == "ByBrand")
                    //                    {
                    //                        dr1["SUBCATNAME_L1"] = dr["SubCat"];
                    //                        dr1["PARENT_CATEGORY_NAME"] = dr["SubCat"];

                    //                        dr1["SUBCATNAME_L2"] = dr["SubCat"];
                    //                        dr1["CATEGORY_NAME"] = dr["SubCat"];

                    //                        dr1["CATEGORY_ID"] = dr["SubCatID"];
                    //                        dr1["PARENT_CATEGORY_ID"] = dr["SubCatID"];
                    //                    }
                    //                    else
                    //                    {
                    //                        dr1["SUBCATNAME_L1"] = dr["ParentCat"];
                    //                        dr1["PARENT_CATEGORY_NAME"] = dr["ParentCat"];

                    //                        dr1["SUBCATNAME_L2"] = dr["SubCat"];
                    //                        dr1["CATEGORY_NAME"] = dr["SubCat"];

                    //                        dr1["CATEGORY_ID"] = dr["SubCatID"];
                    //                        dr1["PARENT_CATEGORY_ID"] = dr["ParentCatID"];
                    //                    }
                    //                    dr1["CATEGORY_PATH"] = dr["CATEGORY_PATH"];    
                    //                    dr1["CUSTOM_NUM_FIELD3"] = ((int)float.Parse(dr["CUSTOM_NUM_FIELD3"].ToString())).ToString();
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    tobedeletepids = "";
                    //tmpstr = "";
                    //foreach (DataRow dr1 in ds.Rows)
                    //{
                    //    if (tmpstr.Contains(dr1["PRODUCT_ID"].ToString().ToUpper()) == false)
                    //        tmpstr = tmpstr + dr1["PRODUCT_ID"].ToString().ToUpper() + ",";
                    //}
                    if (tmpstrPid != string.Empty)
                        tmpstrPid = tmpstrPid.Substring(0, tmpstrPid.Length - 1) + "";

                    if (tmpstrPid != string.Empty)
                    {
                        // Sqltb = objhelper.GetDataTable(StrSql);
                        //Sqltb = (DataTable)objhelperDb.GetGenericDataDB(tmpstr, "GET_PRODUCT_INVENTORY", HelperDB.ReturnType.RTTable);
                        string userid = HttpContext.Current.Session["USER_ID"].ToString();
                        if (userid == string.Empty || userid == null)
                            userid = System.Configuration.ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
                        DataSet tmpDs = new DataSet();
                        tmpDs = objhelperDb.GetProductPriceEA("", tmpstrPid, userid);

                        if (tmpDs != null)
                        {
                            Sqltb = tmpDs.Tables[0];
                            if (Sqltb != null)
                            {
                                foreach (DataRow dr in Sqltb.Rows)
                                {
                                    foreach (DataRow dr1 in ds.Rows)
                                    {
                                        if (dr["PRODUCT_ID"].ToString().ToUpper() == dr1["PRODUCT_ID"].ToString().ToUpper())
                                        {
                                            //dr1["QTY_AVAIL"] = dr["QTY_AVAIL"];
                                            //dr1["MIN_ORD_QTY"] = dr["MIN_ORD_QTY"];
                                            //dr1["STOCK_STATUS_DESC"] = dr["PROD_STK_STATUS_DSC"];
                                            //dr1["PROD_STOCK_STATUS"] = dr["PROD_STOCK_STATUS"];
                                            //dr1["PROD_STOCK_FLAG"] = dr["PROD_STOCK_FLAG"];
                                            dr1["ETA"] = dr["ETA"].ToString();
                                            //if (dr["isPromotion"].ToString() == "Y")
                                            //{
                                            //    tobedeletepids = tobedeletepids + dr["PRODUCT_ID"] + ",";
                                            //    //objErrorhandler.CreateLog(tobedeletepids);   
                                            //}
                                            /* DB Call Price*/
                                            if (dr["price"].ToString() == string.Empty )
                                                dr1["PRODUCT_PRICE"] = "0";
                                            else
                                            {
                                                //dr1["PRODUCT_PRICE"] = dr["price"].ToString();
                                                string tmpprice = string.Empty;
                                                tmpprice = objhelper.CheckPriceValueDecimal(dr["Price"].ToString());
                                                dr1["PRODUCT_PRICE"] = tmpprice;
                                            }
                                       
                                            
                                            /* DB Call Price
                                            if (dr1["ATTRIBUTE_ID"].ToString() == "0" || dr1["ATTRIBUTE_ID"].ToString() == "5" || dr1["ATTRIBUTE_ID"].ToString() == "492")
                                            {
                                                dr1["NUMERIC_VALUE"] = dr["Price"].ToString();
                                            }
                                             DB Call Price */
                                        }
                                    }
                                }
                            }
                        }
                    }

                //    if (tobedeletepids != string.Empty)
                //    {

                //        tobedeletepids = tobedeletepids.Substring(0, tobedeletepids.Length - 1) + "";

                //        DataRow[] rows;
                //        rows = ds.Select("Product_ID in (" + tobedeletepids + ")");
                //        foreach (DataRow r in rows)
                //        {
                //            //objErrorhandler.CreateLog("Inside delete");   
                //            r.Delete();
                //        }
                //    }

                    try
                    {
                        DataRow[] rows = ds.Select("isPromotion='Y'");
                        if (rows.Length > 0)
                        {
                            foreach (DataRow r in rows)
                                r.Delete();
                        }
                    }
                    catch (Exception ex1)
                    {
                    }

                }

              


            }
            catch (Exception ex)
            {
            }
            AttributePro.Tables.Add(ds);
            return ds;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE POWER SEARCH PRODUCT DETAILS ON DROP DOWN LIST ***/
        /********************************************************************************/
        private DataTable Get_DropDown_Power_Search_Product_Values(DataTable ds, INavigateResults res, IResultRow item1, String name)
        {

            int last = res.getLastItem();
            int colFmlyID = res.getColumnIndex("Family Id");
            int colSubFmlyID = res.getColumnIndex("SubFamily Id");
            int colFmlyName = res.getColumnIndex("Family Name");
            int colFmlyDesc = res.getColumnIndex("Family ShortDescription");
            int colFmlylongDesc = res.getColumnIndex("Family Description");
            int colFmlyImg = res.getColumnIndex("Family Thumbnail");

            int colProductID = res.getColumnIndex("Prod Id");
            int colProductCode = res.getColumnIndex("Prod Code");
            //int colProductCode = res.getColumnIndex("Wag Prod Code");
            int colProductPrice = res.getColumnIndex("Price");
            int colProductDesc = res.getColumnIndex("Prod Description");
            int colProductImg = res.getColumnIndex("Prod Thumbnail");

            int colProductCount = res.getColumnIndex("Prod Count");
            int colFamilyProdCount = res.getColumnIndex("Family Prod Count");
            int colSubFamilyCount = res.getColumnIndex("SubFamily Count");
            int colminPrice = res.getColumnIndex("minPrice");
            string _ProCode = string.Empty;
            string _ProductPrice = string.Empty;
            string _FmlyDesc = string.Empty;
            string _ProductDesc = string.Empty;
            string _FmlylongDesc = string.Empty;
            string image_string = string.Empty;
            //IList<INavigateCategory> item = res.getDetailedCategories();





            DataRow dRow;
            try
            {
                if (last >= 0)
                {

                    for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++)
                    {
                        dRow = ds.NewRow();

                        dRow["FAMILY_ID"] = res.getCellData(i, colFmlyID);                        
                        dRow["FAMILY_NAME"] = res.getCellData(i, colFmlyName);
                        dRow["SHortDesc"] = res.getCellData(i, colFmlyDesc);
                        dRow["FamilyDesc"] = res.getCellData(i, colFmlylongDesc);
                        dRow["PRODUCT_ID"] = res.getCellData(i, colProductID);
                        dRow["ProdCode"] = res.getCellData(i, colProductCode);

                        _ProductPrice = res.getCellData(i, colProductPrice);
                        if (_ProductPrice == "" || _ProductPrice == string.Empty)
                            dRow["cost"] = "0";
                        else
                            dRow["cost"] = _ProductPrice.Substring(1);

                        string temp_fmly_Image = res.getCellData(i, colFmlyImg).ToString();
                        string temp_product_Image = res.getCellData(i, colProductImg).ToString();
                        if (temp_product_Image != "")
                        {
                            image_string = temp_product_Image.Substring(42); 
                            image_string = objhelper.SetImageFolderPath(image_string.Replace("\\", "/"), "_th", "_th");
                            dRow["ProdTh"] = image_string;
                        }

                        if (temp_fmly_Image != "")
                        {
                            image_string = temp_fmly_Image.Substring(42);
                            image_string = objhelper.SetImageFolderPath(image_string.Replace("\\", "/"), "_th", "_th");
                            dRow["FamilyTH"] = image_string;
                        }

                        ds.Rows.Add(dRow.ItemArray);  
                       
                       
                        j++;
                    }
                   
                    DataTable Sqltb = new DataTable();
                    tmpstr = "";
                  
                    foreach (DataRow dr1 in ds.Rows)
                    {
                        if (tmpstr.Contains(dr1["FAMILY_ID"].ToString().ToUpper()) == false)
                            tmpstr = tmpstr + "'" + dr1["FAMILY_ID"].ToString().ToUpper() + "',";
                    }
                    if (tmpstr != "")
                        tmpstr = tmpstr.Substring(0, tmpstr.Length - 1) + "";

                    if (tmpstr != "")
                    {
                        //Sqltb = objhelper.GetDataTable(StrSql);
                        Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, tmpstr, "GET_FAMILY_CATEGORY", HelperDB.ReturnType.RTTable);
                        if (Sqltb != null)
                        {
                            foreach (DataRow dr in Sqltb.Rows)
                            {
                                foreach (DataRow dr1 in ds.Rows)
                                {
                                    if (dr["FAMILY_ID"].ToString().ToUpper() == dr1["FAMILY_ID"].ToString().ToUpper())
                                    {                                       
                                            dr1["CATEGORY_PATH"] = dr["CATEGORY_PATH"];
                                            dr1["CATEGORY_ID"] = dr["SubCatID"];                                        
                                    }
                                }
                            }
                        }
                    }


                    
                }


            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE BRAND AND MODEL PRODUCT VALUES ***/
        /********************************************************************************/

        private DataTable Get_BrandModel_Product_Values(string DataType, DataTable ds, INavigateResults res)
        {

            int last = res.getLastItem();
            int colFmlyID = res.getColumnIndex("Family Id");
            int colSubFmlyID = res.getColumnIndex("SubFamily Id");
            int colFmlyName = res.getColumnIndex("Family Name");
            int colFmlyDesc = res.getColumnIndex("Family ShortDescription");
            int colFmlylongDesc = res.getColumnIndex("Family Description");
            int colFmlyImg = res.getColumnIndex("Family Thumbnail");

            int colProductID = res.getColumnIndex("Prod Id");
            int colProductCode = res.getColumnIndex("Prod Code");
            //int colProductCode = res.getColumnIndex("Wag Prod Code");
            int colProductPrice = res.getColumnIndex("Price");
            int colProductDesc = res.getColumnIndex("Prod Description");
            int colProductImg = res.getColumnIndex("Prod Thumbnail");

            int colProductCount = res.getColumnIndex("Prod Count");
            int colFamilyProdCount = res.getColumnIndex("Family Prod Count");
            int colSubFamilyCount = res.getColumnIndex("SubFamily Count");
            string _ProCode = string.Empty;
            string _ProductPrice = string.Empty;
            string _FmlyDesc = string.Empty;
            string _ProductDesc = string.Empty;
            string _FmlylongDesc = string.Empty;
            string image_string = string.Empty;
            //IList<INavigateCategory> item = res.getDetailedCategories();

            DataRow dRow;
            try
            {

                if (res.isGroupedResult())
                {
                    IGroupedResultSet groups = res.getGroupedResult();
                    int groupType = groups.getGroupCriteriaType();
                    String path = res.getCatPath();
                    for (int i = groups.getStartGroup(), len = groups.getEndGroup(); i <= len; i++)
                    {
                        IGroupedResult group = groups.getGroup(i);
                        String name = group.getGroupValue();
                        String nameid = "";
                        DataRow[] dr = CategoryGrouptb.Select("CATEGORY_NAME='" + name + "'");
                        if (dr.Length > 0)
                            nameid = dr.CopyToDataTable().Rows[0]["CATEGORY_ID"].ToString();


                        if (null == name || 0 == name.Length)
                        {
                            continue;  // skip empty
                        }
                        String nodeString = groups.getNodeString(group);
                        int startRow = group.getStartRow();
                        int endRow = group.getEndRow();
                        for (int j = startRow - 1; j < endRow; j++)
                        {
                            IResultRow item = group.getItem(j);
                            int count = group.getTotalNumberOfRows();

                            dRow = ds.NewRow();

                            dRow["FAMILY_ID"] = item.getCellData(colFmlyID);
                            dRow["SUB_FAMILY_ID"] = item.getCellData(colSubFmlyID);
                            dRow["FAMILY_NAME"] = item.getCellData(colFmlyName);
                            //dRow["DESCRIPTION1"] = res.getCellData(i, colFmlyDesc);
                            //dRow["LongDESCRIPTION"] = res.getCellData(i, colFmlylongDesc);
                            dRow["PRODUCT_ID"] = item.getCellData(colProductID);
                            dRow["PRODUCT_CODE"] = item.getCellData(colProductCode);

                            _ProductPrice = item.getCellData(colProductPrice);
                            if (_ProductPrice == "" || _ProductPrice == string.Empty)
                                dRow["PRODUCT_PRICE"] = "0";
                            else
                                dRow["PRODUCT_PRICE"] = _ProductPrice.Substring(1);


                            string temp_family_count = item.getCellData(colFamilyProdCount);

                            string temp_product_count = item.getCellData(colProductCount);
                            string temp_subfamily_count = item.getCellData(colSubFamilyCount);
                            string temp_fmly_Image = item.getCellData(colFmlyImg).ToString();
                            string temp_product_Image = item.getCellData(colProductImg).ToString();

                            dRow["SUB_FAMILY_COUNT"] = (temp_subfamily_count == null || temp_subfamily_count == "") ? "0" : temp_subfamily_count;
                            dRow["PRODUCT_COUNT"] = count;//(temp_product_count == null || temp_product_count == "") ? "0" : temp_product_count;
                            dRow["FAMILY_PRODUCT_COUNT"] = (temp_family_count == null || temp_family_count == "") ? "0" : temp_family_count;


                            image_string = "";
                            if (temp_product_count.ToString() == "1")
                            {
                                if (temp_product_Image != "")
                                    image_string = temp_product_Image.Substring(42);
                                else if (temp_fmly_Image != "")
                                    image_string = temp_fmly_Image.Substring(42);
                                else
                                    image_string = "noimage.gif";
                            }
                            else
                            {
                                if (temp_fmly_Image != "")
                                    image_string = temp_fmly_Image.Substring(42);
                                else
                                    image_string = "noimage.gif";

                            }
                            if (image_string != "")
                                image_string = objhelper.SetImageFolderPath(image_string.Replace("\\", "/"), "_th", "_th");

                            _ProCode = item.getCellData(colProductCode);
                            _ProductPrice = item.getCellData(colProductPrice);

                            _FmlyDesc = item.getCellData(colFmlyDesc);
                            _ProductDesc = item.getCellData(colProductDesc);
                            _FmlylongDesc = item.getCellData(colFmlylongDesc);
                            dRow["SUBCATNAME_L1"] = name;
                            dRow["SUBCATNAME_L2"] = "";
                            dRow["CATEGORY_NAME"] = name;
                            dRow["PARENT_CATEGORY_NAME"] = "";

                            dRow["CATEGORY_ID"] = nameid;
                            dRow["PARENT_CATEGORY_ID"] = "";
                            dRow["CUSTOM_NUM_FIELD3"] = "";
                            dRow["QTY_AVAIL"] = "-1";
                            dRow["MIN_ORD_QTY"] = "-1";


                            dRow["ATTRIBUTE_ID"] = "1";
                            dRow["STRING_VALUE"] = _ProCode; //For the Product Code
                            dRow["NUMERIC_VALUE"] = "0";
                            dRow["OBJECT_TYPE"] = "NULL";
                            dRow["OBJECT_NAME"] = "NULL";
                            dRow["ATTRIBUTE_NAME"] = "Code";
                            dRow["ATTRIBUTE_TYPE"] = "1";
                            ds.Rows.Add(dRow.ItemArray);
                            //}
                            //if (k == 1)
                            //{


                            dRow["ATTRIBUTE_ID"] = "5";
                            dRow["STRING_VALUE"] = "";
                            if (_ProductPrice == "" || _ProductPrice == string.Empty)
                            {
                                dRow["NUMERIC_VALUE"] = "0";
                            }
                            else
                            {
                                dRow["NUMERIC_VALUE"] = _ProductPrice.Substring(1);//For Cost
                            }
                            dRow["OBJECT_TYPE"] = "NULL";
                            dRow["OBJECT_NAME"] = "NULL";
                            dRow["ATTRIBUTE_NAME"] = "Cost";
                            dRow["ATTRIBUTE_TYPE"] = "4";
                            ds.Rows.Add(dRow.ItemArray);
                            //}
                            //if (k == 2)
                            //{
                            dRow["ATTRIBUTE_ID"] = "62";
                            dRow["STRING_VALUE"] = _FmlyDesc;//Family Description
                            dRow["NUMERIC_VALUE"] = "0";
                            dRow["OBJECT_TYPE"] = "NULL";
                            dRow["OBJECT_NAME"] = "NULL";
                            dRow["ATTRIBUTE_NAME"] = "Description";
                            dRow["ATTRIBUTE_TYPE"] = "1";
                            ds.Rows.Add(dRow.ItemArray);
                            //}
                            //if (k == 3)
                            //{
                            dRow["ATTRIBUTE_ID"] = "449";
                            dRow["STRING_VALUE"] = _ProductDesc;//Product Description.
                            dRow["NUMERIC_VALUE"] = "0";
                            dRow["OBJECT_TYPE"] = "NULL";
                            dRow["OBJECT_NAME"] = "NULL";
                            dRow["ATTRIBUTE_NAME"] = "PROD_DSC";
                            dRow["ATTRIBUTE_TYPE"] = "1";
                            ds.Rows.Add(dRow.ItemArray);
                            //}
                            //if (k == 4)
                            //{
                            dRow["ATTRIBUTE_ID"] = "492";
                            dRow["STRING_VALUE"] = "";
                            if (_ProductPrice == "" || _ProductPrice == string.Empty)
                            {
                                dRow["NUMERIC_VALUE"] = "0";
                            }
                            else
                            {
                                dRow["NUMERIC_VALUE"] = _ProductPrice.Substring(1);//For Cost
                            }
                            dRow["OBJECT_TYPE"] = "NULL";
                            dRow["OBJECT_NAME"] = "NULL";
                            dRow["ATTRIBUTE_NAME"] = "PROD_EXT_PRI_3";
                            dRow["ATTRIBUTE_TYPE"] = "4";
                            ds.Rows.Add(dRow.ItemArray);
                            //}
                            //if (k == 5)
                            //{
                            dRow["ATTRIBUTE_ID"] = "453";
                            dRow["STRING_VALUE"] = image_string;
                            dRow["NUMERIC_VALUE"] = "0";
                            dRow["OBJECT_TYPE"] = "jpg";
                            dRow["OBJECT_NAME"] = image_string;
                            dRow["ATTRIBUTE_NAME"] = "Web Image1";
                            dRow["ATTRIBUTE_TYPE"] = "3";
                            ds.Rows.Add(dRow.ItemArray);
                            //}
                            //if (k == 6)
                            //{
                            dRow["ATTRIBUTE_ID"] = "7";
                            dRow["STRING_VALUE"] = image_string;
                            dRow["NUMERIC_VALUE"] = "0";
                            dRow["OBJECT_TYPE"] = "jpg";
                            dRow["OBJECT_NAME"] = image_string;
                            dRow["ATTRIBUTE_NAME"] = "Product Image1";
                            dRow["ATTRIBUTE_TYPE"] = "3";
                            ds.Rows.Add(dRow.ItemArray);
                            //}
                            //if (k == 7)
                            //{
                            dRow["ATTRIBUTE_ID"] = "452";
                            dRow["STRING_VALUE"] = image_string;
                            dRow["NUMERIC_VALUE"] = "0";
                            dRow["OBJECT_TYPE"] = "jpg";
                            dRow["OBJECT_NAME"] = image_string;
                            dRow["ATTRIBUTE_NAME"] = "TWeb Image1";
                            dRow["ATTRIBUTE_TYPE"] = "3";
                            ds.Rows.Add(dRow.ItemArray);
                            //}
                            //if (k == 8)
                            //{
                            dRow["ATTRIBUTE_ID"] = "4";
                            dRow["STRING_VALUE"] = _FmlylongDesc;//Family Description
                            dRow["NUMERIC_VALUE"] = "0";
                            dRow["OBJECT_TYPE"] = "NULL";
                            dRow["OBJECT_NAME"] = "NULL";
                            dRow["ATTRIBUTE_NAME"] = "DESCRIPTIONS";
                            dRow["ATTRIBUTE_TYPE"] = "1";
                            ds.Rows.Add(dRow.ItemArray);



                            // Get_Brand_DataSet_Values(res, item, name, count);
                            //Get_Brand_DataSet_Values(res, item, name);
                        }
                    }

                }
                else
                {
                    Get_Family_Product_Values(DataType, ds, res, null, null); 
                }
                // Get Family Category & Sub Category Name from DB
                DataTable Sqltb = new DataTable();
                tmpstr = "";
                /*********************************** OLD CODE ***********************************/
                // StrSql = "Select A.FAMILY_ID,isnull(B.CATEGORY_NAME,'') as SubCat , isnull(C.CATEGORY_NAME,'') as ParentCat,isnull(B.CATEGORY_ID,'') as SubCatID, isnull(C.CATEGORY_ID,'') as ParentCatID,isnull(B.CUSTOM_NUM_FIELD3,0) as CUSTOM_NUM_FIELD3 from TB_CATALOG_FAMILY A " +
                //          " Left Outer Join  TB_CATEGORY B on A.CATEGORY_ID=B.CATEGORY_ID " +
                //          " Left Outer Join TB_CATEGORY C   on B.PARENT_CATEGORY=C.CATEGORY_ID " +
                //          " Where A.CATALOG_ID=2 And FAMILY_ID in (";

                //foreach (DataRow dr1 in ds.Rows)
                //{
                //    if (tmpstr.Contains("'" + dr1["FAMILY_ID"].ToString().ToUpper() + "'") == false)
                //        tmpstr = tmpstr + "'" + dr1["FAMILY_ID"].ToString().ToUpper() + "',";
                //}
                //if (tmpstr != "")
                //    tmpstr = tmpstr.Substring(0, tmpstr.Length - 1) + "";

                //if (tmpstr != "")
                //{
                //    //Sqltb = objhelper.GetDataTable(StrSql);
                //    Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, tmpstr, "GET_FAMILY_CATEGORY", HelperDB.ReturnType.RTTable);
                //    if (Sqltb != null)
                //    {
                //        foreach (DataRow dr in Sqltb.Rows)
                //        {
                //            foreach (DataRow dr1 in ds.Rows)
                //            {
                //                if (dr["FAMILY_ID"].ToString().ToUpper() == dr1["FAMILY_ID"].ToString().ToUpper())
                //                {
                //                    dr1["SUBCATNAME_L1"] = dr["SubCat"];
                //                    dr1["CATEGORY_NAME"] = dr["SubCat"];

                //                    dr1["SUBCATNAME_L2"] = dr["ParentCat"];
                //                    dr1["PARENT_CATEGORY_NAME"] = dr["ParentCat"];

                                    

                //                    dr1["CATEGORY_ID"] = dr["SubCatID"];
                //                    dr1["PARENT_CATEGORY_ID"] = dr["ParentCatID"];
                //                    dr1["CUSTOM_NUM_FIELD3"] = ((int)float.Parse(dr["CUSTOM_NUM_FIELD3"].ToString())).ToString();
                //                }
                //            }
                //        }
                //    }
                //}
                // Get QTY_AVAIL,MIN_ORD_QTY from DB -- TBWC_INVENTORY

                //StrSql = "SELECT PRODUCT_ID,QTY_AVAIL,MIN_ORD_QTY FROM TBWC_INVENTORY " +
                //         " Where PRODUCT_ID in (";

                /*********************************** OLD CODE ***********************************/
                tmpstr = "";
                HelperDB objHelperDB=new HelperDB();
                string _UserID = HttpContext.Current.Session["USER_ID"].ToString();
                foreach (DataRow dr1 in ds.Rows)
                {
                    if (tmpstr.Contains(dr1["PRODUCT_ID"].ToString().ToUpper()) == false)
                        tmpstr = tmpstr + dr1["PRODUCT_ID"].ToString().ToUpper() + ",";
                }
                if (tmpstr != "")
                    tmpstr = tmpstr.Substring(0, tmpstr.Length - 1) + "";

                if (tmpstr != "")
                {
                    // Sqltb = objhelper.GetDataTable(StrSql);
                    //Sqltb = (DataTable)objhelperDb.GetGenericDataDB(tmpstr, "GET_PRODUCT_INVENTORY", HelperDB.ReturnType.RTTable);
                   DataSet tmpds=null;
                   tmpds = objHelperDB.GetProductPrice("", tmpstr, _UserID);
                    if (ds!=null)
                    {
                        Sqltb = tmpds.Tables[0].Copy();
                        if (Sqltb != null)
                        {
                            foreach (DataRow dr in Sqltb.Rows)
                            {
                                foreach (DataRow dr1 in ds.Rows)
                                {
                                    if (dr["PRODUCT_ID"].ToString().ToUpper() == dr1["PRODUCT_ID"].ToString().ToUpper())
                                    {
                                        dr1["QTY_AVAIL"] = dr["QTY_AVAIL"].ToString();
                                        dr1["MIN_ORD_QTY"] = dr["MIN_ORD_QTY"].ToString();
                                        dr1["STOCK_STATUS_DESC"] = dr["PROD_STK_STATUS_DSC"].ToString();
                                        /*  DB Call Price
                                         * if (dr1["ATTRIBUTE_ID"].ToString() == "0" || dr1["ATTRIBUTE_ID"].ToString() == "5" || dr1["ATTRIBUTE_ID"].ToString() == "492")
                                         {
                                             dr1["NUMERIC_VALUE"] = dr["Price"].ToString();
                                         }
                                         dr1["PRODUCT_PRICE"] = dr["PRICE"].ToString(); 
                                         DB Call Price */

                                    }
                                }
                            }
                        }
                    }
                }
               

            }
            catch (Exception ex)
            {
            }
            return ds;
        }


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT VALUES IN FAMILY PAGE  ***/
        /********************************************************************************/

        private DataTable Get_FamilyPage_Product_Values(string DataType, DataTable Fds, DataTable ds, INavigateResults res, IResultRow item1, String name)
        {

            int last = res.getLastItem();
            int colFmlyID = res.getColumnIndex("Family Id");
            int colFmlyName = res.getColumnIndex("Family Name");
            int colFmlyDesc = res.getColumnIndex("Family ShortDescription");
            int colFmlylongDesc = res.getColumnIndex("Family Description");
            int colFmlyImg = res.getColumnIndex("Family Thumbnail");

            int colsubFmlyID = res.getColumnIndex("SubFamily Id");
            int colsubFmlyName = res.getColumnIndex("SubFamily Name");            
            int colsubFmlyImg = res.getColumnIndex("SubFamily Thumbnail");            
            int colsubFmlylongDesc = res.getColumnIndex("SubFamily Description");

            int colProductID = res.getColumnIndex("Prod Id");
            int colProductCode = res.getColumnIndex("Prod Code");
           // int colProductCode = res.getColumnIndex("Wag Prod Code");
            int colProductPrice = res.getColumnIndex("Price");
            int colProductDesc = res.getColumnIndex("Prod Description");
           // int colWESProductCode = res.getColumnIndex("WES Product Code");
            int colProductImg = res.getColumnIndex("Prod Thumbnail");

            int colProductCount = res.getColumnIndex("Prod Count");
            int colFamilyProdCount = res.getColumnIndex("Family Prod Count");
            int colSubFamilyCount = res.getColumnIndex("SubFamily Count");
            string _ProCode = string.Empty;
            string _ProductPrice = string.Empty;
            string _FmlyDesc = string.Empty;
            string _ProductDesc = string.Empty;
            string _FmlylongDesc = string.Empty;
            string image_string = string.Empty;
            string _fmly_Image=null;
            string tobedeletepids = string.Empty;
           // string _WESProductCode = "";
            //IList<INavigateCategory> item = res.getDetailedCategories();





            DataRow dRow;
            try
            {
                
                if (last >= 0)
                {

                    _ProCode = res.getCellData(0, colProductCode);
                    _ProductPrice = res.getCellData(0, colProductPrice);
                   // _WESProductCode = res.getCellData(0, colWESProductCode);
                    _FmlyDesc = res.getCellData(0, colFmlyDesc);
                    _FmlylongDesc = res.getCellData(0, colFmlylongDesc);
                    _fmly_Image = res.getCellData(0, colFmlyImg).ToString();

                    string FamCount = res.getCellData(0, colFamilyProdCount);
                    string ProCount = last.ToString();
                    string Status = "false";
                    if (ProCount == "1")
                    {
                        if (FamCount != ProCount)
                        {
                            Status = "One Product";
                        }
                        else
                        {
                            Status = "true";
                        }
                    }
                    else if (FamCount == ProCount)
                    {
                        Status = "true";
                    }
                    else
                    {
                        Status = "false";
                    }

                    //string temp_product_Image = res.getCellData(0, colProductImg).ToString();
                    image_string = "";
                    if (_fmly_Image != "" && _fmly_Image != null)
                        image_string = _fmly_Image.Substring(42);
                    else
                        image_string = "noimage.gif";


                    if (image_string != "")
                        image_string = objhelper.SetImageFolderPath(image_string.Replace("\\", "/"), "_th", "_th");

                    dRow = Fds.NewRow();

                    dRow["FAMILY_ID"] = res.getCellData(0, colFmlyID);
                    dRow["FAMILY_NAME"] = res.getCellData(0, colFmlyName);
                    dRow["ATTRIBUTE_DATA_TYPE"] = "TEXT";
                    dRow["FAMILY_PROD_COUNT"]=FamCount;
                    dRow["PROD_COUNT"]=ProCount;
                    dRow["STATUS"] = Status;


                    dRow["ATTRIBUTE_ID"] = "1";
                    dRow["STRING_VALUE"] = _ProCode; //For the Product Code
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "NULL";
                    dRow["OBJECT_NAME"] = "NULL";
                    dRow["ATTRIBUTE_NAME"] = "Code";
                    dRow["ATTRIBUTE_TYPE"] = "1";
                    Fds.Rows.Add(dRow.ItemArray);


                    //dRow["ATTRIBUTE_ID"] = "450";
                    //dRow["STRING_VALUE"] = _WESProductCode; //For the wesProductCode
                    //dRow["NUMERIC_VALUE"] = "0";
                    //dRow["OBJECT_TYPE"] = "NULL";
                    //dRow["OBJECT_NAME"] = "NULL";
                    //dRow["ATTRIBUTE_NAME"] = "PROD_CODE";
                    //dRow["ATTRIBUTE_TYPE"] = "1";
                    //Fds.Rows.Add(dRow.ItemArray);


                    ////------------------
                    //dRow["ATTRIBUTE_ID"] = "13";
                    //dRow["STRING_VALUE"] = _FmlyDesc;//Family Description
                    //dRow["NUMERIC_VALUE"] = "0";
                    //dRow["OBJECT_TYPE"] = "NULL";
                    //dRow["OBJECT_NAME"] = "NULL";
                    //dRow["ATTRIBUTE_NAME"] = "SHORT_DESCRIPTION";
                    //dRow["ATTRIBUTE_TYPE"] = "7";
                    //Fds.Rows.Add(dRow.ItemArray);

                    ////------------------
                    //dRow["ATTRIBUTE_ID"] = "4";
                    //dRow["STRING_VALUE"] = _FmlylongDesc;//Family Description
                    //dRow["NUMERIC_VALUE"] = "0";
                    //dRow["OBJECT_TYPE"] = "NULL";
                    //dRow["OBJECT_NAME"] = "NULL";
                    //dRow["ATTRIBUTE_NAME"] = "DESCRIPTION";
                    //dRow["ATTRIBUTE_TYPE"] = "7";
                    //Fds.Rows.Add(dRow.ItemArray);



                    //------------------
                    dRow["ATTRIBUTE_ID"] = "746";
                    dRow["STRING_VALUE"] = image_string;
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "jpg";
                    dRow["OBJECT_NAME"] = image_string;
                    dRow["ATTRIBUTE_NAME"] = "FWeb Image1";
                    dRow["ATTRIBUTE_TYPE"] = "9";
                    Fds.Rows.Add(dRow.ItemArray);


                    //------------------
                    dRow["ATTRIBUTE_ID"] = "747";
                    dRow["STRING_VALUE"] = image_string;
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "jpg";
                    dRow["OBJECT_NAME"] = image_string;
                    dRow["ATTRIBUTE_NAME"] = "TFWeb Image1";
                    dRow["ATTRIBUTE_TYPE"] = "9";
                    Fds.Rows.Add(dRow.ItemArray);

                    // Get Family Category & Sub Category Name from DB
                    DataTable Sqltb = new DataTable();
                    //StrSql = "select FS.STRING_VALUE,FS.ATTRIBUTE_ID,FS.NUMERIC_VALUE,FS.OBJECT_TYPE,FS.OBJECT_NAME,A.ATTRIBUTE_NAME,A.ATTRIBUTE_TYPE " +
                    //         " from TB_FAMILY F " +
                    //         " Inner Join TB_FAMILY_SPECS FS On fs.FAMILY_ID=F.FAMILY_ID" +
                    //         " Inner Join TB_ATTRIBUTE A On A.ATTRIBUTE_ID=FS.ATTRIBUTE_ID " +
                    //         " where A.ATTRIBUTE_TYPE=9 And A.PUBLISH2WEB = 1 and A.ATTRIBUTE_ID not in (746, 747) And F.FAMILY_ID=" + res.getCellData(0, colFmlyID).ToString();
                    tmpstr = res.getCellData(0, colFmlyID).ToString();
                    if (tmpstr != "")
                    {
                        //Sqltb = objhelper.GetDataTable(StrSql);
                        Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, tmpstr, "2", "GET_FAMILY_ATTRIBUTE", HelperDB.ReturnType.RTTable);

                        if (Sqltb != null)
                        {
                            foreach (DataRow dr in Sqltb.Rows)
                            {
                                dRow["ATTRIBUTE_ID"] = dr["ATTRIBUTE_ID"];
                                dRow["STRING_VALUE"] = dr["STRING_VALUE"];
                                dRow["NUMERIC_VALUE"] = dr["NUMERIC_VALUE"];
                                dRow["OBJECT_TYPE"] = dr["OBJECT_TYPE"];
                                dRow["OBJECT_NAME"] = dr["OBJECT_NAME"];
                                dRow["ATTRIBUTE_NAME"] = dr["ATTRIBUTE_NAME"];
                                dRow["ATTRIBUTE_TYPE"] = dr["ATTRIBUTE_TYPE"];
                                Fds.Rows.Add(dRow.ItemArray);
                            }
                        }
                    }

                    for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++)
                    {
                     

                        dRow = ds.NewRow();

                        string temp_family_count = res.getCellData(i, colFamilyProdCount);

                        string temp_product_count = res.getCellData(i, colProductCount);
                        string temp_subfamily_count = res.getCellData(i, colSubFamilyCount);
                        string temp_fmly_Image = res.getCellData(i, colFmlyImg).ToString();

                        string temp_subfmly_Image = res.getCellData(i, colsubFmlyImg).ToString();
                        string temp_product_Image = res.getCellData(i, colProductImg).ToString();

                        if (temp_fmly_Image != null && temp_fmly_Image != "")
                            dRow["FAMILY_TH_IMAGE"] = temp_fmly_Image.Substring(42);
                        else
                            dRow["FAMILY_TH_IMAGE"] = "noimage.gif";

                        //if (temp_subfmly_Image != null && temp_subfmly_Image != "")
                        //    dRow["SUB_FAMILY_TH_IMAGE"] = temp_subfmly_Image.Substring(42);
                        //else
                        //    dRow["SUB_FAMILY_TH_IMAGE"] = "noimage.gif";

                       

                        if (res.getCellData(i, colsubFmlyID).ToString()!="" && res.getCellData(i, colsubFmlyName).ToString()!="")
                        {
                            dRow["FAMILY_ID"] = res.getCellData(i, colsubFmlyID);
                            dRow["FAMILY_NAME"] = res.getCellData(i, colsubFmlyName);                            
                            dRow["FAMILY_SHORT_DESC"] = "";
                            dRow["FAMILY_DESC"] = res.getCellData(i, colsubFmlylongDesc);


                            if (temp_subfmly_Image != null && temp_subfmly_Image != "")
                            {
                                image_string = temp_subfmly_Image.Substring(42);

                            }
                            else
                                image_string = "noimage.gif";


                            if (image_string != "")
                                image_string = objhelper.SetImageFolderPath(image_string.Replace("\\", "/"), "_th", "_th");

                            dRow["FAMILY_TH_IMAGE"] = image_string;
                            
                        }
                        else
                        {

                            dRow["FAMILY_ID"] = res.getCellData(i, colFmlyID);
                            dRow["FAMILY_NAME"] = res.getCellData(i, colFmlyName);
                            dRow["FAMILY_SHORT_DESC"] = res.getCellData(i, colFmlyDesc);
                            dRow["FAMILY_DESC"] = res.getCellData(i, colFmlylongDesc);

                            if (temp_fmly_Image != null && temp_fmly_Image != "")
                                image_string = temp_fmly_Image.Substring(42);
                            else
                                image_string = "noimage.gif";


                            if (image_string != "")
                                image_string = objhelper.SetImageFolderPath(image_string.Replace("\\", "/"), "_th", "_th");

                            dRow["FAMILY_TH_IMAGE"] = image_string;

                        }

                        //dRow["FAMILY_ID"] = res.getCellData(i, colFmlyID);
                        //dRow["FAMILY_NAME"] = res.getCellData(i, colFmlyName);
                        ////dRow["FAMILY_TH_IMAGE"] = res.getCellData(i, colFmlyImg);
                        //dRow["FAMILY_SHORT_DESC"] = res.getCellData(i, colFmlyDesc);
                        //dRow["FAMILY_DESC"] = res.getCellData(i, colFmlylongDesc);
                        //dRow["SUB_FAMILY_ID"] = res.getCellData(i, colsubFmlyID);
                        //dRow["SUB_FAMILY_NAME"] = res.getCellData(i, colsubFmlyID);
                        ////dRow["SUB_FAMILY_TH_IMAGE"] = res.getCellData(i, colsubFmlyImg);
                        
                        //dRow["SUB_FAMILY_DESC"] = res.getCellData(i, colsubFmlylongDesc);
                        dRow["PRODUCT_ID"] = res.getCellData(i, colProductID);
                        dRow["PRODUCT_CODE"] = res.getCellData(i, colProductCode);

                       // dRow["PROD_CODE"] = res.getCellData(i, colWESProductCode);
                       

                        _ProductPrice = res.getCellData(i, colProductPrice);
                        if (_ProductPrice == "" || _ProductPrice == string.Empty)
                            dRow["PRODUCT_PRICE"] = "0";
                        else
                            dRow["PRODUCT_PRICE"] = _ProductPrice.Substring(1);

                        dRow["PRODUCT_DESC"] = res.getCellData(i, colProductDesc);
                        //dRow["PRODUCT_TH_IMAGE"] = res.getCellData(i, colProductImg);





                        if (temp_product_Image != null && temp_product_Image != "")
                            dRow["PRODUCT_TH_IMAGE"] = temp_product_Image.Substring(42);
                        else
                            dRow["PRODUCT_TH_IMAGE"] = "noimage.gif";


                        dRow["SUB_FAMILY_COUNT"] = (temp_subfamily_count == null || temp_subfamily_count == "") ? "0" : temp_subfamily_count;
                        dRow["PRODUCT_COUNT"] = (temp_product_count == null || temp_product_count == "") ? "0" : temp_product_count;
                        dRow["FAMILY_PRODUCT_COUNT"] = (temp_family_count == null || temp_family_count == "") ? "0" : temp_family_count;


                        

                        dRow["QTY_AVAIL"] = "-1";
                        dRow["MIN_ORD_QTY"] = "-1";
                        ds.Rows.Add(dRow);

                    }


                    // Get QTY_AVAIL,MIN_ORD_QTY from DB -- TBWC_INVENTORY

                    //StrSql = "SELECT PRODUCT_ID,QTY_AVAIL,MIN_ORD_QTY FROM TBWC_INVENTORY " +
                    //         " Where PRODUCT_ID in (";
                    tobedeletepids = "";
                    tmpstr = "";
                    foreach (DataRow dr1 in ds.Rows)
                    {
                        if (tmpstr.Contains(dr1["PRODUCT_ID"].ToString().ToUpper()) == false)
                        tmpstr = tmpstr + dr1["PRODUCT_ID"].ToString().ToUpper() + ",";
                    }
                    if (tmpstr != "")
                        tmpstr = tmpstr.Substring(0, tmpstr.Length - 1) + "";

                    if (tmpstr != "")
                    {
                        //Sqltb = objhelper.GetDataTable(StrSql);
                        Sqltb = (DataTable)objhelperDb.GetGenericDataDB(tmpstr,"GET_PRODUCT_INVENTORY", HelperDB.ReturnType.RTTable);
                        
                        if (Sqltb != null)
                        {
                            foreach (DataRow dr in Sqltb.Rows)
                            {
                                foreach (DataRow dr1 in ds.Rows)
                                {
                                    if (dr["PRODUCT_ID"].ToString().ToUpper() == dr1["PRODUCT_ID"].ToString().ToUpper())
                                    {
                                        dr1["QTY_AVAIL"] = dr["QTY_AVAIL"].ToString();
                                        dr1["MIN_ORD_QTY"] = dr["MIN_ORD_QTY"].ToString();

                                        if (dr["isPromotion"].ToString() == "Y")
                                            tobedeletepids = tobedeletepids + dr["PRODUCT_ID"].ToString() + ",";
                                    }
                                }
                            }
                        }
                    }

                }

                if (tobedeletepids != "")
                    tobedeletepids = tobedeletepids.Substring(0, tobedeletepids.Length - 1) + "";


                DataRow[] rows;
                rows = ds.Select("Product_ID in (" + tobedeletepids + ")");
                foreach (DataRow r in rows)
                    r.Delete();

            }
            catch (Exception ex)
            {
            }
            return ds;
        }
        private DataTable Get_FamilyPage_Product_ValuesJson(string DataType, DataTable Fds, DataTable ds, INavigateResults res, IResultRow item1, String name,DataSet  Dsadv)
        {


            int last = res.getLastItemJson();
            //int colFmlyID = res.getColumnIndexJson("Family Id");
            //int colFmlyName = res.getColumnIndexJson("Family Name");
            //int colFmlyDesc = res.getColumnIndexJson("Family ShortDescription");
            //int colFmlylongDesc = res.getColumnIndexJson("Family Description");
            //int colFmlyImg = res.getColumnIndexJson("Family Thumbnail");

            //int colsubFmlyID = res.getColumnIndexJson("SubFamily Id");
            //int colsubFmlyName = res.getColumnIndexJson("SubFamily Name");
            //int colsubFmlyImg = res.getColumnIndexJson("SubFamily Thumbnail");
            //int colsubFmlylongDesc = res.getColumnIndexJson("SubFamily Description");

            //int colProductID = res.getColumnIndexJson("Prod Id");
            //int colProductCode = res.getColumnIndexJson("Prod Code");
            //// int colProductCode = res.getColumnIndex("Wag Prod Code");
            //int colProductPrice = res.getColumnIndexJson("Price");
            //int colProductDesc = res.getColumnIndexJson("Prod Description");
            //// int colWESProductCode = res.getColumnIndex("WES Product Code");
            //int colProductImg = res.getColumnIndexJson("Prod Thumbnail");

            //int colProductCount = res.getColumnIndexJson("Prod Count");
            //int colFamilyProdCount = res.getColumnIndexJson("Family Prod Count");
            //int colSubFamilyCount = res.getColumnIndexJson("SubFamily Count");

            string _ProCode =string.Empty;
            string _ProductPrice = string.Empty;
            string _FmlyDesc = string.Empty;
            string _ProductDesc = string.Empty;
            string _FmlylongDesc = string.Empty;
            string image_string = string.Empty;
            string _fmly_Image = null;
            string tobedeletepids = string.Empty;
            // string _WESProductCode = "";
            //IList<INavigateCategory> item = res.getDetailedCategories();




           // DataSet Dsadv = new DataSet();
           
            
            DataRow dRow;
            try
            {

                if (last >= 0)
                {

                    var drs = Dsadv.Tables["items"].Rows[0];
                    
                    
                      
                  //  Dsadv.Tables.Add(res.GetDBAdvisor().Tables["items"].Copy());
                  //  DataRow[] drs = Dsadv.Tables[0].Select();

                    _ProCode = drs["Prod_Code"].ToString();// res.getCellData(0, colProductCode);
                    
                  //  _ProductPrice = drs["Price"].ToString();//res.getCellData(0, colProductPrice);

                    // _WESProductCode = res.getCellData(0, colWESProductCode);
                    _FmlyDesc = drs["Family_ShortDescription"].ToString();//res.getCellData(0, colFmlyDesc);
                    _FmlylongDesc = drs["Family_Description"].ToString();//res.getCellData(0, colFmlylongDesc);
                    _fmly_Image = drs["Family_Thumbnail"].ToString();//res.getCellData(0, colFmlyImg).ToString();

                    string FamCount = drs["Family_Prod_Count"].ToString();//res.getCellData(0, colFamilyProdCount);
                    string ProCount = last.ToString();
                    string Status = "false";
                    if (ProCount == "1")
                    {
                        if (FamCount != ProCount)
                        {
                            Status = "One Product";
                        }
                        else
                        {
                            Status = "true";
                        }
                    }
                    else if (FamCount == ProCount)
                    {
                        Status = "true";
                    }
                    else
                    {
                        Status = "false";
                    }

                    //string temp_product_Image = res.getCellData(0, colProductImg).ToString();
                    image_string = "";
                    if (_fmly_Image != "" && _fmly_Image != null)
                        image_string = _fmly_Image.Substring(42);
                    else
                        image_string = "noimage.gif";


                    if (image_string != "")
                        image_string = objhelper.SetImageFolderPath(image_string.Replace("\\", "/"), "_th", "_th");

                    dRow = Fds.NewRow();

                    dRow["FAMILY_ID"] = drs["Family_Id"].ToString();//res.getCellData(0, colFmlyID);
                    dRow["FAMILY_NAME"] = drs["Family_Name"].ToString();//res.getCellData(0, colFmlyName);
                    dRow["ATTRIBUTE_DATA_TYPE"] = "TEXT";
                    dRow["FAMILY_PROD_COUNT"] = FamCount;
                    dRow["PROD_COUNT"] = ProCount;
                    dRow["STATUS"] = Status;
                   

                    dRow["ATTRIBUTE_ID"] = "1";
                    dRow["STRING_VALUE"] = _ProCode; //For the Product Code
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "NULL";
                    dRow["OBJECT_NAME"] = "NULL";
                    dRow["ATTRIBUTE_NAME"] = "Code";
                    dRow["ATTRIBUTE_TYPE"] = "1";
                    Fds.Rows.Add(dRow.ItemArray);

                    //------------------
                    dRow["ATTRIBUTE_ID"] = "746";
                    dRow["STRING_VALUE"] = image_string;
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "jpg";
                    dRow["OBJECT_NAME"] = image_string;
                    dRow["ATTRIBUTE_NAME"] = "FWeb Image1";
                    dRow["ATTRIBUTE_TYPE"] = "9";
                    Fds.Rows.Add(dRow.ItemArray);


                    //------------------
                    dRow["ATTRIBUTE_ID"] = "747";
                    dRow["STRING_VALUE"] = image_string;
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "jpg";
                    dRow["OBJECT_NAME"] = image_string;
                    dRow["ATTRIBUTE_NAME"] = "TFWeb Image1";
                    dRow["ATTRIBUTE_TYPE"] = "9";
                    Fds.Rows.Add(dRow.ItemArray);

                  
                    DataTable Sqltb = new DataTable();
                    tmpstr = drs["Family_Id"].ToString();//res.getCellData(0, colFmlyID).ToString();
                    if (tmpstr != "")
                    {
                        //Sqltb = objhelper.GetDataTable(StrSql);
                        Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, tmpstr, "2", "GET_FAMILY_ATTRIBUTE", HelperDB.ReturnType.RTTable);

                        if (Sqltb != null)
                        {
                            foreach (DataRow dr in Sqltb.Rows)
                            {

                                dRow["ATTRIBUTE_ID"] = dr["ATTRIBUTE_ID"];
                                dRow["STRING_VALUE"] = dr["STRING_VALUE"];
                                dRow["NUMERIC_VALUE"] = dr["NUMERIC_VALUE"];
                                dRow["OBJECT_TYPE"] = dr["OBJECT_TYPE"];
                                dRow["OBJECT_NAME"] = dr["OBJECT_NAME"];
                                dRow["ATTRIBUTE_NAME"] = dr["ATTRIBUTE_NAME"];
                                dRow["ATTRIBUTE_TYPE"] = dr["ATTRIBUTE_TYPE"];
                                Fds.Rows.Add(dRow.ItemArray);
                            }
                        }
                    }

                    //for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++)
                   // {

                
                    ds = Dsadv.Tables["items"].Copy();
                    ds.TableName = "FamilyPro";
                    ds.Columns["Prod_Id"].ColumnName = "PRODUCT_ID";
                    ds.Columns["Prod_Count"].ColumnName = "PRODUCT_COUNT";
                    ds.Columns["Family_Prod_Count"].ColumnName = "FAMILY_PRODUCT_COUNT";
                    ds.Columns["SubFamily_Count"].ColumnName = "SUB_FAMILY_COUNT";
                    ds.Columns["minPrice"].ColumnName = "MIN_PRICE";
                    ds.Columns["Prod_Code"].ColumnName = "PRODUCT_CODE";
                    ds.Columns["Price"].ColumnName = "PRODUCT_PRICE";
                    ds.Columns["Prod_Description"].ColumnName = "PRODUCT_DESC";
                    ds.Columns["Family_ShortDescription"].ColumnName = "FAMILY_SHORT_DESC";
                    ds.Columns["Family_Description"].ColumnName = "FAMILY_DESC";
                    ds.Columns["Family_Thumbnail"].ColumnName = "FAMILY_TH_IMAGE";
                    ds.Columns["Prod_Thumbnail"].ColumnName = "PRODUCT_TH_IMAGE";

                    ds.Columns["QtyAvail"].ColumnName = "QTY_AVAIL";
                    ds.Columns["MinOrdQty"].ColumnName = "MIN_ORD_QTY";
                    ds.Columns["Prod_Stock_Status"].ColumnName = "PROD_STOCK_STATUS";
                    ds.Columns["Prod_Stock_Flag"].ColumnName = "PROD_STOCK_FLAG";
                    ds.Columns["Prod_Stk_Status_Dsc"].ColumnName = "STOCK_STATUS_DESC";
                    ds.Columns["Promotion_Product"].ColumnName = "isPromotion";
                    ds.Columns["SORT_ORDER"].ColumnName = "SORT ORDER";

                    
                    //DataColumn dc = new DataColumn("QTY_AVAIL", typeof(string));
                    //dc.DefaultValue = "100";

                    //ds.Columns.Add(dc);
                    //dc = new DataColumn("MIN_ORD_QTY", typeof(string));
                    //dc.DefaultValue = "1";
                    //ds.Columns.Add(dc);

                    //ds.Columns.Add("STOCK_STATUS_DESC", typeof(string));
                    //ds.Columns.Add("PROD_STOCK_STATUS", typeof(string));
                    //ds.Columns.Add("PROD_STOCK_FLAG", typeof(string));
                    ds.Columns.Add("ETA", typeof(string));

                   
                    DataColumn dcso = new DataColumn("SORT_ORDER", typeof(int));
                    dcso.DefaultValue = 0;
                    ds.Columns.Add(dcso);
                    string temp_subfmly_Image = string.Empty;
                    string temp_product_Image = string.Empty;
                    string temp_fmly_Image = string.Empty;
                    tobedeletepids = "";
                    tmpstr = "";
                   // string tmpfamily_ids = "";
                    string strfamilyids = string.Empty;
                    foreach (DataRow Dr in ds.Rows)
                    {
                         temp_subfmly_Image = Dr["SubFamily_Thumbnail"].ToString();
                         temp_product_Image = Dr["PRODUCT_TH_IMAGE"].ToString();
                         temp_fmly_Image = Dr["FAMILY_TH_IMAGE"].ToString();

                        if (Dr["SubFamily_Id"].ToString() != "" && Dr["SubFamily_Name"].ToString() != "")
                        {
                            Dr["FAMILY_ID"] = Dr["SubFamily_Id"].ToString(); //res.getCellData(i, colsubFmlyID);
                            Dr["FAMILY_NAME"] = Dr["SubFamily_Name"].ToString(); //res.getCellData(i, colsubFmlyName);
                            Dr["FAMILY_SHORT_DESC"] = "";
                            Dr["FAMILY_DESC"] = Dr["SubFamily_Description"].ToString(); //res.getCellData(i, colsubFmlylongDesc);


                            if (temp_subfmly_Image != null && temp_subfmly_Image != "")
                            {
                                image_string = temp_subfmly_Image.Substring(42);

                            }
                            else
                                image_string = "noimage.gif";


                            if (image_string != "")
                                image_string = objhelper.SetImageFolderPath(image_string.Replace("\\", "/"), "_th", "_th");

                            Dr["FAMILY_TH_IMAGE"] = image_string;
                            //strfamilyids = Dr["SubFamily_Id"].ToString();
                        }
                        else
                        {

                            Dr["FAMILY_ID"] = Dr["Family_Id"].ToString(); //res.getCellData(i, colFmlyID);
                            Dr["FAMILY_NAME"] = Dr["Family_Name"].ToString(); // res.getCellData(i, colFmlyName);
                            Dr["FAMILY_SHORT_DESC"] = Dr["FAMILY_SHORT_DESC"].ToString(); //res.getCellData(i, colFmlyDesc);
                            Dr["FAMILY_DESC"] = Dr["FAMILY_DESC"].ToString(); //res.getCellData(i, colFmlylongDesc);

                            if (temp_fmly_Image != null && temp_fmly_Image != "")
                                image_string = temp_fmly_Image.Substring(42);
                            else
                                image_string = "noimage.gif";


                            if (image_string != "")
                                image_string = objhelper.SetImageFolderPath(image_string.Replace("\\", "/"), "_th", "_th");

                            Dr["FAMILY_TH_IMAGE"] = image_string;
                            //strfamilyids = Dr["Family_Id"].ToString();
                        }


                        //if (Dr["PRODUCT_PRICE"].ToString() == "" || Dr["PRODUCT_PRICE"].ToString() == string.Empty)
                        //    Dr["PRODUCT_PRICE"] = "0";
                        //else
                        //    Dr["PRODUCT_PRICE"] = Dr["PRODUCT_PRICE"].ToString().Substring(1);

                        if (Dr["PRODUCT_PRICE"].ToString().Contains("$") )
                            Dr["PRODUCT_PRICE"] = Dr["PRODUCT_PRICE"].ToString().Substring(1);
                        

                        Dr["SUB_FAMILY_COUNT"] = (Dr["SUB_FAMILY_COUNT"].ToString() == null || Dr["SUB_FAMILY_COUNT"].ToString() == "") ? "0" : Dr["SUB_FAMILY_COUNT"].ToString();
                        Dr["PRODUCT_COUNT"] = (Dr["PRODUCT_COUNT"].ToString() == null || Dr["PRODUCT_COUNT"].ToString() == "") ? "0" : Dr["PRODUCT_COUNT"].ToString();
                        Dr["FAMILY_PRODUCT_COUNT"] = (Dr["FAMILY_PRODUCT_COUNT"].ToString() == null || Dr["FAMILY_PRODUCT_COUNT"].ToString() == "") ? "0" : Dr["FAMILY_PRODUCT_COUNT"].ToString();

                        if (temp_product_Image != null && temp_product_Image != "")
                            Dr["PRODUCT_TH_IMAGE"] = temp_product_Image.Substring(42);
                        else
                            Dr["PRODUCT_TH_IMAGE"] = "noimage.gif";
                        
                        if (Dr["SORT ORDER"] == "") Dr["SORT ORDER"] = "0";

                        Dr["SORT_ORDER"] = ((int)float.Parse(Dr["SORT ORDER"].ToString())).ToString();


                        if (Dr["PROD_STOCK_STATUS"] == "")
                            Dr["PROD_STOCK_STATUS"] = "0";

                        if (Dr["STOCK_STATUS_DESC"] == "")
                            Dr["STOCK_STATUS_DESC"] = "NO STATUS AVAILABLE";

                        if (Dr["PROD_STOCK_FLAG"] == "")
                            Dr["PROD_STOCK_FLAG"] = "0";

                        if (Dr["QTY_AVAIL"] == "")
                            Dr["QTY_AVAIL"] = "100";



                        if (Dr["MIN_ORD_QTY"] == "")
                            Dr["MIN_ORD_QTY"] = "1";

                        if (Dr["isPromotion"] == "")
                            Dr["isPromotion"] = "N";
                        
                       // Dr["QTY_AVAIL"] = "-1";
                       // Dr["MIN_ORD_QTY"] = "-1";

                        if (tmpstr.Contains(Dr["PRODUCT_ID"].ToString().ToUpper()) == false)
                            tmpstr = tmpstr + Dr["PRODUCT_ID"].ToString().ToUpper() + ",";

                        //if (strfamilyids.Contains(strfamilyids) == false)  Dr["FAMILY_ID"]
                        //    strfamilyids = strfamilyids + ",";
                          if (strfamilyids.Contains(Dr["FAMILY_ID"].ToString().ToUpper()) == false)
                              strfamilyids = strfamilyids + Dr["FAMILY_ID"].ToString().ToUpper() + ",";
                    }

                    if (tmpstr != string.Empty)
                        tmpstr = tmpstr.Substring(0, tmpstr.Length - 1) + "";

                    if (strfamilyids != string.Empty)
                        strfamilyids = strfamilyids.Substring(0, strfamilyids.Length - 1) + "";

                    if (tmpstr != string.Empty)
                    {
                        string userid = HttpContext.Current.Session["USER_ID"].ToString();
                        if (userid == "" || userid == null)
                            userid = System.Configuration.ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

                        //Sqltb = objhelper.GetDataTable(StrSql);
                      //  Sqltb = (DataTable)objhelperDb.GetGenericDataDB(tmpstr, "GET_PRODUCT_INVENTORY", HelperDB.ReturnType.RTTable);
                       // DataSet  Sqltbs = objhelperDb.GetProductPrice("", tmpstr, userid);
                        DataSet Sqltbs = objhelperDb.GetProductPriceEA(strfamilyids, tmpstr, userid);

                        if (Sqltbs != null)
                        {
                            foreach (DataRow dr in Sqltbs.Tables[0].Rows)
                            {
                                foreach (DataRow dr1 in ds.Rows)
                                {
                                    if (dr["PRODUCT_ID"].ToString().ToUpper() == dr1["PRODUCT_ID"].ToString().ToUpper())
                                    {
                                       // dr1["QTY_AVAIL"] = dr["QTY_AVAIL"];
                                       // dr1["MIN_ORD_QTY"] = dr["MIN_ORD_QTY"];
                                       //// dr1["STOCK_STATUS_DESC"] = dr["PROD_STK_STATUS_DSC"];
                                       // dr1["PROD_STOCK_STATUS"] = dr["PROD_STOCK_STATUS"].ToString();
                                       // dr1["PROD_STOCK_FLAG"] = dr["PROD_STOCK_FLAG"].ToString();
                                        dr1["ETA"] = dr["ETA"];
                                       // dr1["SORT_ORDER"] = dr["SORT_ORDER"];

                                        if (dr["Price"].ToString() == string.Empty || dr["Price"].ToString() == string.Empty)
                                            dr1["PRODUCT_PRICE"] = "0";
                                        else
                                        {
                                            string tmpprice = string.Empty;
                                            tmpprice = objhelper.CheckPriceValueDecimal(dr["Price"].ToString());
                                           // dr1["PRODUCT_PRICE"] = dr["Price"].ToString();
                                            dr1["PRODUCT_PRICE"] = tmpprice;

                                        }
                                     
                                       // if (dr["isPromotion"].ToString() == "Y")
                                       //     tobedeletepids = tobedeletepids + dr["PRODUCT_ID"].ToString() + ",";
                                    }
                                }
                            }
                        }
                    }

                }

                //if (tobedeletepids != "")
                //{
                //    tobedeletepids = tobedeletepids.Substring(0, tobedeletepids.Length - 1) + "";
                //    DataRow[] rows;
                //    rows = ds.Select("PRODUCT_ID in (" + tobedeletepids + ")");
                //    foreach (DataRow r in rows)
                //        r.Delete();
                //}
                try
                {
                     DataRow[] rows = ds.Select("isPromotion='Y'");
                     if( rows.Length >0)
                     {
                     foreach (DataRow r in rows)
                         r.Delete();
                     }
                }
                catch(Exception ex1){
                }
                AttributePro.Tables.Add(ds); 
            }
            catch (Exception ex)
            {
            }
            
            return ds;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT VALUES FROM PRODUCTS PAGE ***/
        /********************************************************************************/

        private DataTable Get_ProductPage_Product_Values(string DataType, DataTable Fds, DataTable ds, INavigateResults res, IResultRow item1, String name)
        {

            int last = res.getLastItem();
            int colFmlyID = res.getColumnIndex("Family Id");
            int colFmlyName = res.getColumnIndex("Family Name");
            int colFmlyDesc = res.getColumnIndex("Family ShortDescription");
            int colFmlylongDesc = res.getColumnIndex("Family Description");
            int colFmlyImg = res.getColumnIndex("Family Thumbnail");

            int colsubFmlyID = res.getColumnIndex("SubFamily Id");
            int colsubFmlyName = res.getColumnIndex("SubFamily Name");
            int colsubFmlyImg = res.getColumnIndex("SubFamily Thumbnail");
            int colsubFmlylongDesc = res.getColumnIndex("SubFamily Description");

            int colProductID = res.getColumnIndex("Prod Id");
            int colProductCode = res.getColumnIndex("Prod Code");
           // int colProductCode = res.getColumnIndex("Wag Prod Code");
            int colProductPrice = res.getColumnIndex("Price");
            int colProductDesc = res.getColumnIndex("Prod Description");
            int colProductImg = res.getColumnIndex("Prod Thumbnail");

            int colProductCount = res.getColumnIndex("Prod Count");
            int colFamilyProdCount = res.getColumnIndex("Family Prod Count");
            int colSubFamilyCount = res.getColumnIndex("SubFamily Count");
            string _ProCode = string.Empty;
            string _ProductPrice = string.Empty;
            string _FmlyDesc = string.Empty;
            string _ProductDesc = string.Empty;
            string _FmlylongDesc = string.Empty;
            string image_string = string.Empty;
            string _fmly_Image = null;
            
            //IList<INavigateCategory> item = res.getDetailedCategories();





            DataRow dRow;
            try
            {
                if (last >= 0)
                {

                    _ProCode = res.getCellData(0, colProductCode);
                    _ProductPrice = res.getCellData(0, colProductPrice);
                    _ProductDesc = res.getCellData(0, colProductDesc);

                    _FmlyDesc = res.getCellData(0, colFmlyDesc);
                    _FmlylongDesc = res.getCellData(0, colFmlylongDesc);
                    _fmly_Image = res.getCellData(0, colFmlyImg).ToString();
                    string FamCount = res.getCellData(0, colFamilyProdCount);
                    string ProCount = last.ToString();
                    string temp_subfamily_count = res.getCellData(0, colSubFamilyCount);
                    string temp_fmly_Image = res.getCellData(0, colFmlyImg).ToString();
                    string _subFamID = res.getCellData(0, colsubFmlyID);
                    string _FamID = res.getCellData(0, colFmlyID);
                    string temp_subfmly_Image = res.getCellData(0, colsubFmlyImg).ToString();
                    
                    //string temp_product_Image = res.getCellData(0, colProductImg).ToString();
                    image_string = "";
                    if (_fmly_Image != "" && _fmly_Image != null)
                        image_string = _fmly_Image.Substring(42);
                    else
                        image_string = "noimage.gif";

                    if (image_string != "")
                        image_string = objhelper.SetImageFolderPath(image_string.Replace("\\", "/"), "_th", "_th");

                    dRow = ds.NewRow();

                    dRow["PRODUCT_ID"] = res.getCellData(0, colProductID);

                    if (res.getCellData(0, colsubFmlyID).ToString() != "" && res.getCellData(0, colsubFmlyName).ToString() != "")
                    {
                        dRow["FAMILY_ID"] = res.getCellData(0, colsubFmlyID);
                        dRow["FAMILY_NAME"] = res.getCellData(0, colsubFmlyName);
                        dRow["FAMILY_SHORT_DESC"] = "";
                        dRow["FAMILY_DESC"] = res.getCellData(0, colsubFmlylongDesc);
                        if (temp_subfmly_Image != null && temp_subfmly_Image != "")
                            dRow["FAMILY_TH_IMAGE"] = temp_subfmly_Image.Substring(42);
                        else
                            dRow["FAMILY_TH_IMAGE"] = "noimage.gif";
                        //dRow["FAMILY_ID"] = res.getCellData(0, colFmlyID);
                        //dRow["FAMILY_NAME"] = res.getCellData(0, colFmlyName);
                        //dRow["FAMILY_SHORT_DESC"] = res.getCellData(0, colFmlyDesc);
                        //dRow["FAMILY_DESC"] = res.getCellData(0, colFmlylongDesc);

                        //if (temp_fmly_Image != null && temp_fmly_Image != "")
                        //    dRow["FAMILY_TH_IMAGE"] = temp_fmly_Image.Substring(42);
                        //else
                        //    dRow["FAMILY_TH_IMAGE"] = "noimage.gif";
                    }
                    else
                    {

                        dRow["FAMILY_ID"] = res.getCellData(0, colFmlyID);
                        dRow["FAMILY_NAME"] = res.getCellData(0, colFmlyName);
                        dRow["FAMILY_SHORT_DESC"] = res.getCellData(0, colFmlyDesc);
                        dRow["FAMILY_DESC"] = res.getCellData(0, colFmlylongDesc);

                        if (temp_fmly_Image != null && temp_fmly_Image != "")
                            dRow["FAMILY_TH_IMAGE"] = temp_fmly_Image.Substring(42);
                        else
                            dRow["FAMILY_TH_IMAGE"] = "noimage.gif";
                    }

                    dRow["QTY_AVAIL"] = "-1";
                    dRow["MIN_ORD_QTY"] = "-1";
                    dRow["STOCK_STATUS_DESC"] = "";
                    dRow["FAMILY_PROD_COUNT"] = FamCount;
                    dRow["PROD_COUNT"] = ProCount;

                    // Get QTY_AVAIL,MIN_ORD_QTY from DB -- TBWC_INVENTORY
                    DataTable Sqltb = new DataTable();
                    //StrSql = "SELECT A.PRODUCT_ID,A.QTY_AVAIL,A.MIN_ORD_QTY,Isnull(B.PROD_STK_STATUS_DSC,'') as STOCK_STATUS " +
                    //         " FROM TBWC_INVENTORY A LEFT OUTER JOIN WESTB_PRODUCT_ITEM B ON A.PRODUCT_ID=B.PRODUCT_ID" +
                    //         " Where A.PRODUCT_ID =" + res.getCellData(0, colProductID).ToString();                    
                    tmpstr=res.getCellData(0, colProductID).ToString();
                    if (tmpstr != "")
                    {
                        //Sqltb = objhelper.GetDataTable(StrSql);
                        Sqltb = (DataTable) objhelperDb.GetGenericDataDB(tmpstr, "GET_SINGLE_PRODUCT_INVENTORY", HelperDB.ReturnType.RTTable);
                        if (Sqltb != null)
                        {
                            foreach (DataRow dr in Sqltb.Rows)
                            {
                                dRow["QTY_AVAIL"] = dr["QTY_AVAIL"].ToString();
                                dRow["MIN_ORD_QTY"] = dr["MIN_ORD_QTY"].ToString();
                                dRow["STOCK_STATUS_DESC"] = dr["STOCK_STATUS"].ToString();
                            }
                        }
                    }
                    
                
                

                    dRow["ATTRIBUTE_ID"] = "1";
                    dRow["STRING_VALUE"] = _ProCode; 
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "NULL";
                    dRow["OBJECT_NAME"] = "NULL";
                    dRow["ATTRIBUTE_NAME"] = "Code";
                    dRow["ATTRIBUTE_TYPE"] = "1";
                    dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
                    ds.Rows.Add(dRow.ItemArray);

                    //------------------
                    dRow["ATTRIBUTE_ID"] = "62";
                    dRow["STRING_VALUE"] = _ProductDesc;
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "NULL";
                    dRow["OBJECT_NAME"] = "NULL";
                    dRow["ATTRIBUTE_NAME"] = "Description";
                    dRow["ATTRIBUTE_TYPE"] = "1";
                    dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
                    ds.Rows.Add(dRow.ItemArray);

                  



                    //------------------
                    dRow["ATTRIBUTE_ID"] = "452";
                    dRow["STRING_VALUE"] = image_string;
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "jpg";
                    dRow["OBJECT_NAME"] = image_string;
                    dRow["ATTRIBUTE_NAME"] = "TWeb Image1";
                    dRow["ATTRIBUTE_TYPE"] = "3";
                    dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
                    ds.Rows.Add(dRow.ItemArray);


                    //------------------
                    dRow["ATTRIBUTE_ID"] = "453";
                    dRow["STRING_VALUE"] = image_string;
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "jpg";
                    dRow["OBJECT_NAME"] = image_string;
                    dRow["ATTRIBUTE_NAME"] = "Web Image1";
                    dRow["ATTRIBUTE_TYPE"] = "3";
                    dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
                    ds.Rows.Add(dRow.ItemArray);

                    // Get product Spec
                    Sqltb = new DataTable();
                    //StrSql = "select A.STRING_VALUE,A.ATTRIBUTE_ID,A.NUMERIC_VALUE,A.OBJECT_TYPE,A.OBJECT_NAME,B.ATTRIBUTE_NAME,B.ATTRIBUTE_TYPE,B.ATTRIBUTE_DATATYPE " +
                    //         " from TB_PROD_SPECS A " +                             
                    //         " Inner Join TB_ATTRIBUTE B On A.ATTRIBUTE_ID=B.ATTRIBUTE_ID " +
                    //         " where B.PUBLISH2WEB = 1 and A.ATTRIBUTE_ID not in (1,453,452, 7) And A.PRODUCT_ID=" + res.getCellData(0, colProductID).ToString();
                    //res.getCellData(0, colProductID).ToString()

                    tmpstr = ((_subFamID == "") ? _FamID : _subFamID).ToString();

                    if (tmpstr != "")
                    {
                       // Sqltb = objhelper.GetDataTable(StrSql);
                        Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, res.getCellData(0, colProductID).ToString(), tmpstr, "GET_PRODUCT_ATTRIBUTE", HelperDB.ReturnType.RTTable);

                        if (Sqltb != null)
                        {
                            foreach (DataRow dr in Sqltb.Rows)
                            {
                                dRow["ATTRIBUTE_ID"] = dr["ATTRIBUTE_ID"];
                                dRow["STRING_VALUE"] = dr["STRING_VALUE"];
                                dRow["NUMERIC_VALUE"] = dr["NUMERIC_VALUE"];
                                dRow["OBJECT_TYPE"] = dr["OBJECT_TYPE"];
                                dRow["OBJECT_NAME"] = dr["OBJECT_NAME"];
                                dRow["ATTRIBUTE_NAME"] = dr["ATTRIBUTE_NAME"];
                                dRow["ATTRIBUTE_TYPE"] = dr["ATTRIBUTE_TYPE"];
                                dRow["ATTRIBUTE_DATATYPE"] = dr["ATTRIBUTE_DATATYPE"];                                
                                ds.Rows.Add(dRow.ItemArray);
                            }
                        }
                    }
                    //this Query ref STP->GetProductImages
                    Sqltb = new DataTable();
                    //StrSql = " select FS.STRING_VALUE,FS.ATTRIBUTE_ID,FS.NUMERIC_VALUE,FS.OBJECT_TYPE,FS.OBJECT_NAME,A.ATTRIBUTE_NAME,A.ATTRIBUTE_TYPE,A.ATTRIBUTE_DATATYPE " +
                    //         " from TB_FAMILY F " +
                    //         " Inner Join TB_FAMILY_SPECS FS On fs.FAMILY_ID=F.FAMILY_ID" +
                    //         " Inner Join TB_ATTRIBUTE A On A.ATTRIBUTE_ID=FS.ATTRIBUTE_ID " +
                    //         " where A.PUBLISH2WEB = 1 and A.ATTRIBUTE_ID not in (746, 747) And F.FAMILY_ID=" + ((_subFamID == "") ? _FamID : _subFamID).ToString();

                    //DataRow[] Dr = ds.Select("ATTRIBUTE_TYPE=7");
                    //if (Dr.Length <= 0)                    
                    //   StrSql=StrSql+" And A.ATTRIBUTE_TYPE in (7,9)";                    
                    //else
                    //   StrSql = StrSql + " And A.ATTRIBUTE_TYPE=9";


                    tmpstr = ((_subFamID == "") ? _FamID : _subFamID).ToString();
                    if (tmpstr != "")
                    {
                        //Sqltb = objhelper.GetDataTable(StrSql);
                        DataRow[] Dr = ds.Select("ATTRIBUTE_TYPE=7");
                        if (Dr.Length <= 0)
                            Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, tmpstr, "0", "GET_FAMILY_ATTRIBUTE", HelperDB.ReturnType.RTTable);
                        else
                            Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, tmpstr, "1", "GET_FAMILY_ATTRIBUTE", HelperDB.ReturnType.RTTable);

                        if (Sqltb != null)
                        {
                            foreach (DataRow dr in Sqltb.Rows)
                            {
                                dRow["ATTRIBUTE_ID"] = dr["ATTRIBUTE_ID"];
                                dRow["STRING_VALUE"] = dr["STRING_VALUE"];
                                dRow["NUMERIC_VALUE"] = dr["NUMERIC_VALUE"];
                                dRow["OBJECT_TYPE"] = dr["OBJECT_TYPE"];
                                dRow["OBJECT_NAME"] = dr["OBJECT_NAME"];
                                dRow["ATTRIBUTE_NAME"] = dr["ATTRIBUTE_NAME"];
                                dRow["ATTRIBUTE_TYPE"] = dr["ATTRIBUTE_TYPE"];
                                dRow["ATTRIBUTE_DATATYPE"] = dr["ATTRIBUTE_DATATYPE"];
                                ds.Rows.Add(dRow.ItemArray);
                            }
                        }
                    }
                  



                }
                  

            }
            catch (Exception ex)
            {
            }
            return ds;
        }
        private DataTable Get_ProductPage_Product_ValuesJson(string DataType, DataTable Fds, DataTable ds, INavigateResults res, IResultRow item1, String name,DataSet  Dsadv)
        {

            int last = res.getLastItemJson();
            //int colFmlyID = res.getColumnIndexJson("Family Id");
            //int colFmlyName = res.getColumnIndexJson("Family Name");
            //int colFmlyDesc = res.getColumnIndexJson("Family ShortDescription");
            //int colFmlylongDesc = res.getColumnIndexJson("Family Description");
            //int colFmlyImg = res.getColumnIndexJson("Family Thumbnail");

            //int colsubFmlyID = res.getColumnIndexJson("SubFamily Id");
            //int colsubFmlyName = res.getColumnIndexJson("SubFamily Name");
            //int colsubFmlyImg = res.getColumnIndexJson("SubFamily Thumbnail");
            //int colsubFmlylongDesc = res.getColumnIndexJson("SubFamily Description");

            //int colProductID = res.getColumnIndexJson("Prod Id");
            //int colProductCode = res.getColumnIndexJson("Prod Code");
            //// int colProductCode = res.getColumnIndex("Wag Prod Code");
            //int colProductPrice = res.getColumnIndexJson("Price");
            //int colProductDesc = res.getColumnIndexJson("Prod Description");
            //int colProductImg = res.getColumnIndexJson("Prod Thumbnail");

            //int colProductCount = res.getColumnIndexJson("Prod Count");
            //int colFamilyProdCount = res.getColumnIndexJson("Family Prod Count");
            //int colSubFamilyCount = res.getColumnIndexJson("SubFamily Count");
            string _ProCode = string.Empty;
            string _ProductPrice = string.Empty;
            string _FmlyDesc = string.Empty;
            string _ProductDesc = string.Empty;
            string _FmlylongDesc = string.Empty;
            string image_string = string.Empty;
            string _fmly_Image = null;

            //IList<INavigateCategory> item = res.getDetailedCategories();





            DataRow dRow;
           
            try
            {
                if (last >= 0)
                {
                            

                     var Dr   = Dsadv.Tables["items"].Rows[0] ;
                    _ProCode = Dr["Prod_Code"].ToString();// res.getCellData(0, colProductCode);
                    _ProductPrice = Dr["Price"].ToString();//res.getCellData(0, colProductPrice);
                    _ProductDesc = Dr["Prod_Description"].ToString();//res.getCellData(0, colProductDesc);

                    _FmlyDesc = Dr["Family_ShortDescription"].ToString();//res.getCellData(0, colFmlyDesc);
                    _FmlylongDesc = Dr["Family_Description"].ToString();//res.getCellData(0, colFmlylongDesc);
                    _fmly_Image = Dr["Family_Thumbnail"].ToString();//res.getCellData(0, colFmlyImg).ToString();
                    string FamCount = Dr["Family_Prod_Count"].ToString();//res.getCellData(0, colFamilyProdCount);
                    string ProCount = last.ToString();
                    string temp_subfamily_count = Dr["SubFamily_Count"].ToString();//res.getCellData(0, colSubFamilyCount);
                    string temp_fmly_Image = Dr["Family_Thumbnail"].ToString();//res.getCellData(0, colFmlyImg).ToString();
                    string _subFamID = Dr["SubFamily_Id"].ToString();//res.getCellData(0, colsubFmlyID);
                    string _FamID = Dr["Family_Id"].ToString();// res.getCellData(0, colFmlyID);
                    string temp_subfmly_Image = Dr["SubFamily_Thumbnail"].ToString();// res.getCellData(0, colsubFmlyImg).ToString();

                    //string temp_product_Image = res.getCellData(0, colProductImg).ToString();
                    image_string = "";
                    if (_fmly_Image != string.Empty && _fmly_Image != null)
                        image_string = _fmly_Image.Substring(42);
                    else
                        image_string = "noimage.gif";

                    if (image_string != "")
                        image_string = objhelper.SetImageFolderPath(image_string.Replace("\\", "/"), "_th", "_th");

                    dRow = ds.NewRow();

                    dRow["PRODUCT_ID"] = Dr["Prod_Id"].ToString();// res.getCellData(0, colProductID);

                    //if (res.getCellData(0, colsubFmlyID).ToString() != "" && res.getCellData(0, colsubFmlyName).ToString() != "")
                    if (Dr["SubFamily_Id"].ToString() != string.Empty && Dr["Family_Name"].ToString() != string.Empty)
                    {
                        dRow["FAMILY_ID"] = Dr["SubFamily_Id"].ToString();//res.getCellData(0, colsubFmlyID);
                        dRow["FAMILY_NAME"] = Dr["SubFamily_Name"].ToString();//res.getCellData(0, colsubFmlyName);
                        dRow["FAMILY_SHORT_DESC"] = "";
                        dRow["FAMILY_DESC"] = Dr["SubFamily_Description"].ToString();//res.getCellData(0, colsubFmlylongDesc);
                        if (temp_subfmly_Image != null && temp_subfmly_Image != string.Empty)
                            dRow["FAMILY_TH_IMAGE"] = temp_subfmly_Image.Substring(42);
                        else
                            dRow["FAMILY_TH_IMAGE"] = "noimage.gif";
                        //dRow["FAMILY_ID"] = res.getCellData(0, colFmlyID);
                        //dRow["FAMILY_NAME"] = res.getCellData(0, colFmlyName);
                        //dRow["FAMILY_SHORT_DESC"] = res.getCellData(0, colFmlyDesc);
                        //dRow["FAMILY_DESC"] = res.getCellData(0, colFmlylongDesc);

                        //if (temp_fmly_Image != null && temp_fmly_Image != "")
                        //    dRow["FAMILY_TH_IMAGE"] = temp_fmly_Image.Substring(42);
                        //else
                        //    dRow["FAMILY_TH_IMAGE"] = "noimage.gif";
                    }
                    else
                    {

                        dRow["FAMILY_ID"] = Dr["Family_Id"].ToString();// res.getCellData(0, colFmlyID);
                        dRow["FAMILY_NAME"] = Dr["Family_Name"].ToString();// res.getCellData(0, colFmlyName);
                        dRow["FAMILY_SHORT_DESC"] = Dr["Family_ShortDescription"].ToString();//res.getCellData(0, colFmlyDesc);
                        dRow["FAMILY_DESC"] = Dr["Family_Description"].ToString();//res.getCellData(0, colFmlylongDesc);

                        if (temp_fmly_Image != null && temp_fmly_Image != string.Empty)
                            dRow["FAMILY_TH_IMAGE"] = temp_fmly_Image.Substring(42);
                        else
                            dRow["FAMILY_TH_IMAGE"] = "noimage.gif";
                    }

                    dRow["QTY_AVAIL"] = Dr["QtyAvail"];
                    dRow["MIN_ORD_QTY"] = Dr["MinOrdQty"];
                    dRow["STOCK_STATUS_DESC"] = Dr["Prod_Stk_Status_Dsc"];
                    dRow["PROD_STOCK_STATUS"] = Dr["Prod_Stock_Status"];
                    dRow["PROD_STOCK_FLAG"] = Dr["Prod_Stock_Flag"];
                    dRow["FAMILY_PROD_COUNT"] = FamCount;
                    dRow["PROD_COUNT"] = ProCount;
                    dRow["COST"] = Dr["Price"].ToString();

                    if (dRow["QTY_AVAIL"] == "")
                        dRow["QTY_AVAIL"] = "100";



                    if (dRow["MIN_ORD_QTY"] == "")
                        dRow["MIN_ORD_QTY"] = "1";


                    if (dRow["PROD_STOCK_STATUS"] == "")
                        dRow["PROD_STOCK_STATUS"] = "0";

                    if (dRow["STOCK_STATUS_DESC"] == "")
                        dRow["STOCK_STATUS_DESC"] = "NO STATUS AVAILABLE";

                    if (dRow["PROD_STOCK_FLAG"] == "")
                        dRow["PROD_STOCK_FLAG"] = "0";
                    // Get QTY_AVAIL,MIN_ORD_QTY from DB -- TBWC_INVENTORY
                    DataTable Sqltb = new DataTable();
                    //StrSql = "SELECT A.PRODUCT_ID,A.QTY_AVAIL,A.MIN_ORD_QTY,Isnull(B.PROD_STK_STATUS_DSC,'') as STOCK_STATUS " +
                    //         " FROM TBWC_INVENTORY A LEFT OUTER JOIN WESTB_PRODUCT_ITEM B ON A.PRODUCT_ID=B.PRODUCT_ID" +
                    //         " Where A.PRODUCT_ID =" + res.getCellData(0, colProductID).ToString();               
     
                    tmpstr = Dr["Prod_Id"].ToString();// res.getCellData(0, colProductID).ToString();
                      string userid = HttpContext.Current.Session["USER_ID"].ToString();
                        if (userid == string.Empty || userid == null)
                            userid = System.Configuration.ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
                    

                  //  string sqlexec = "exec STP_TBWC_PICKGENERICDATA '" + WesCatalogId + "','" + tmpstr + "','','','','GET_SINGLE_PRODUCT_INVENTORY','" + websiteid + "'";

                        string sqlexec = "exec STP_TBWC_PICKFPRODUCTPRICE_EA '','" + tmpstr + "','" + userid + "'";

                    tmpstr = ((_subFamID == "") ? _FamID : _subFamID).ToString();
                    sqlexec = sqlexec + ";exec STP_TBWC_PICKGENERICDATA '" + WesCatalogId + "','"+  Dr["Prod_Id"].ToString() +"','" + tmpstr + "','','','GET_PRODUCT_ATTRIBUTE','" + websiteid + "'";
                    
                    DataRow[] Dr1 = ds.Select("ATTRIBUTE_TYPE=7");
                    if (Dr1.Length <= 0)
                        sqlexec = sqlexec + ";exec STP_TBWC_PICKGENERICDATA '" + WesCatalogId + "','" + tmpstr + "','0','','','GET_FAMILY_ATTRIBUTE','" + websiteid + "'";                        
                    else
                       sqlexec = sqlexec + ";exec STP_TBWC_PICKGENERICDATA '" + WesCatalogId + "','" + tmpstr + "','1','','','GET_FAMILY_ATTRIBUTE','" + websiteid + "'";


                   DataSet  Dsall = objhelperDb.GetDataSetDB(sqlexec);


                    if (tmpstr != string.Empty)
                    {
                        //Sqltb = objhelper.GetDataTable(StrSql);
                        //Sqltb = (DataTable)objhelperDb.GetGenericDataDB(tmpstr, "GET_SINGLE_PRODUCT_INVENTORY", HelperDB.ReturnType.RTTable);
                        if (Dsall != null && Dsall.Tables[0] != null)
                            Sqltb = Dsall.Tables[0];

                        if (Sqltb != null && Sqltb.Rows.Count>0  )
                        {
                            foreach (DataRow dr in Sqltb.Rows)
                            {
                                //dRow["QTY_AVAIL"] = dr["QTY_AVAIL"];
                                //dRow["MIN_ORD_QTY"] = dr["MIN_ORD_QTY"];
                                //dRow["STOCK_STATUS_DESC"] = dr["STOCK_STATUS"];
                                //dRow["PROD_STOCK_STATUS"] = dr["PROD_STOCK_STATUS"].ToString();
                                //dRow["PROD_STOCK_FLAG"] = dr["PROD_STOCK_FLAG"].ToString();
                                dRow["ETA"] = dr["ETA"];
                                if (dr["price"].ToString() == string.Empty)
                                    dRow["COST"] = "$0.00";
                                else
                                {
                                    //dr1["PRODUCT_PRICE"] = dr["price"].ToString();
                                    string tmpprice = string.Empty;
                                    tmpprice = objhelper.CheckPriceValueDecimal(dr["Price"].ToString());
                                    dRow["COST"] = "$"+tmpprice;
                                }
                            }
                        }
                    }




                    dRow["ATTRIBUTE_ID"] = "1";
                    dRow["STRING_VALUE"] = _ProCode;
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "NULL";
                    dRow["OBJECT_NAME"] = "NULL";
                    dRow["ATTRIBUTE_NAME"] = "Code";
                    dRow["ATTRIBUTE_TYPE"] = "1";
                    dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
                    ds.Rows.Add(dRow.ItemArray);

                    //------------------
                    dRow["ATTRIBUTE_ID"] = "62";
                    dRow["STRING_VALUE"] = _ProductDesc;
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "NULL";
                    dRow["OBJECT_NAME"] = "NULL";
                    dRow["ATTRIBUTE_NAME"] = "Description";
                    dRow["ATTRIBUTE_TYPE"] = "1";
                    dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
                    ds.Rows.Add(dRow.ItemArray);





                    //------------------
                    dRow["ATTRIBUTE_ID"] = "452";
                    dRow["STRING_VALUE"] = image_string;
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "jpg";
                    dRow["OBJECT_NAME"] = image_string;
                    dRow["ATTRIBUTE_NAME"] = "TWeb Image1";
                    dRow["ATTRIBUTE_TYPE"] = "3";
                    dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
                    ds.Rows.Add(dRow.ItemArray);


                    //------------------
                    dRow["ATTRIBUTE_ID"] = "453";
                    dRow["STRING_VALUE"] = image_string;
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "jpg";
                    dRow["OBJECT_NAME"] = image_string;
                    dRow["ATTRIBUTE_NAME"] = "Web Image1";
                    dRow["ATTRIBUTE_TYPE"] = "3";
                    dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
                    ds.Rows.Add(dRow.ItemArray);

                    // Get product Spec
                    Sqltb = new DataTable();
                    //StrSql = "select A.STRING_VALUE,A.ATTRIBUTE_ID,A.NUMERIC_VALUE,A.OBJECT_TYPE,A.OBJECT_NAME,B.ATTRIBUTE_NAME,B.ATTRIBUTE_TYPE,B.ATTRIBUTE_DATATYPE " +
                    //         " from TB_PROD_SPECS A " +                             
                    //         " Inner Join TB_ATTRIBUTE B On A.ATTRIBUTE_ID=B.ATTRIBUTE_ID " +
                    //         " where B.PUBLISH2WEB = 1 and A.ATTRIBUTE_ID not in (1,453,452, 7) And A.PRODUCT_ID=" + res.getCellData(0, colProductID).ToString();
                    //res.getCellData(0, colProductID).ToString()


                    tmpstr = ((_subFamID == "") ? _FamID : _subFamID).ToString();

                   
                            if (tmpstr != "")
                            {
                                // Sqltb = objhelper.GetDataTable(StrSql);
                               // Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, Dr["Prod_Id"].ToString(), tmpstr, "GET_PRODUCT_ATTRIBUTE", HelperDB.ReturnType.RTTable);
                                if (Dsall != null && Dsall.Tables[1] != null)
                                    Sqltb = Dsall.Tables[1];

                                if (Sqltb != null && Sqltb.Rows.Count > 0)
                                {
                                    DataRow[] dtrows = Sqltb.Select("ATTRIBUTE_TYPE<>4");
                                    if (dtrows.Length > 0)
                                    {
                                        foreach (DataRow dr in dtrows)
                                        {
                                            //if (dr["ATTRIBUTE_TYPE"].ToString() != "4")
                                            //{
                                            dRow["ATTRIBUTE_ID"] = dr["ATTRIBUTE_ID"];
                                            dRow["STRING_VALUE"] = dr["STRING_VALUE"];
                                            dRow["NUMERIC_VALUE"] = dr["NUMERIC_VALUE"];
                                            dRow["OBJECT_TYPE"] = dr["OBJECT_TYPE"];
                                            dRow["OBJECT_NAME"] = dr["OBJECT_NAME"];
                                            dRow["ATTRIBUTE_NAME"] = dr["ATTRIBUTE_NAME"];
                                            dRow["ATTRIBUTE_TYPE"] = dr["ATTRIBUTE_TYPE"];
                                            dRow["ATTRIBUTE_DATATYPE"] = dr["ATTRIBUTE_DATATYPE"];
                                            ds.Rows.Add(dRow.ItemArray);
                                            //}
                                        }
                                    }
                                }
                            }
                      
                    //this Query ref STP->GetProductImages
                    Sqltb = new DataTable();
                    //StrSql = " select FS.STRING_VALUE,FS.ATTRIBUTE_ID,FS.NUMERIC_VALUE,FS.OBJECT_TYPE,FS.OBJECT_NAME,A.ATTRIBUTE_NAME,A.ATTRIBUTE_TYPE,A.ATTRIBUTE_DATATYPE " +
                    //         " from TB_FAMILY F " +
                    //         " Inner Join TB_FAMILY_SPECS FS On fs.FAMILY_ID=F.FAMILY_ID" +
                    //         " Inner Join TB_ATTRIBUTE A On A.ATTRIBUTE_ID=FS.ATTRIBUTE_ID " +
                    //         " where A.PUBLISH2WEB = 1 and A.ATTRIBUTE_ID not in (746, 747) And F.FAMILY_ID=" + ((_subFamID == "") ? _FamID : _subFamID).ToString();

                    //DataRow[] Dr = ds.Select("ATTRIBUTE_TYPE=7");
                    //if (Dr.Length <= 0)                    
                    //   StrSql=StrSql+" And A.ATTRIBUTE_TYPE in (7,9)";                    
                    //else
                    //   StrSql = StrSql + " And A.ATTRIBUTE_TYPE=9";


                    tmpstr = ((_subFamID == "") ? _FamID : _subFamID).ToString();

                   
                            if (tmpstr != "")
                            {
                                //Sqltb = objhelper.GetDataTable(StrSql);
                              //  DataRow[] Dr1 = ds.Select("ATTRIBUTE_TYPE=7");
                              //  if (Dr1.Length <= 0)
                              //      Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, tmpstr, "0", "GET_FAMILY_ATTRIBUTE", HelperDB.ReturnType.RTTable);
                              //  else
                               //     Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, tmpstr, "1", "GET_FAMILY_ATTRIBUTE", HelperDB.ReturnType.RTTable);
                                if (Dsall != null && Dsall.Tables[2] != null)
                                    Sqltb = Dsall.Tables[2];
                                if (Sqltb != null && Sqltb.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in Sqltb.Rows)
                                    {
                                        dRow["ATTRIBUTE_ID"] = dr["ATTRIBUTE_ID"];
                                        dRow["STRING_VALUE"] = dr["STRING_VALUE"];
                                        dRow["NUMERIC_VALUE"] = dr["NUMERIC_VALUE"];
                                        dRow["OBJECT_TYPE"] = dr["OBJECT_TYPE"];
                                        dRow["OBJECT_NAME"] = dr["OBJECT_NAME"];
                                        dRow["ATTRIBUTE_NAME"] = dr["ATTRIBUTE_NAME"];
                                        dRow["ATTRIBUTE_TYPE"] = dr["ATTRIBUTE_TYPE"];
                                        dRow["ATTRIBUTE_DATATYPE"] = dr["ATTRIBUTE_DATATYPE"];
                                        ds.Rows.Add(dRow.ItemArray);
                                    }
                                }
                            }
                     
               

                }


            }
            catch (Exception ex)
            {
            }
            return ds;
        }
        DataSet AttributePro = new DataSet();
        DataTable AttributeFamPro = new DataTable("FamilyPro");
        DataTable AttributeFam = new DataTable("Family");
        DataTable CategoryGrouptb = new DataTable("Category");
        //DataTable AttributeSubFam = new DataTable("SubFamily");

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ATTRIBUTE PRODUCT DETAILS ***/
        /********************************************************************************/
        public DataSet GetAttributeProducts(string DataPage, string SearchStr, string AttributeType, string AttributeValue, string Brand, string resultPerPage, string CurrentPageNo, string NextPage)
        {
            return GetAttributeProducts(DataPage, SearchStr, AttributeType, AttributeValue, Brand, resultPerPage, CurrentPageNo, NextPage, "");
        }


        //public DataSet GetAttributeProductstoxml(string DataPage, string SearchStr, string AttributeType, string AttributeValue, string Brand, string resultPerPage, string CurrentPageNo, string NextPage)
        //{
        //    return GetAttributeProductstoxml(DataPage, SearchStr, AttributeType, AttributeValue, Brand, resultPerPage, CurrentPageNo, NextPage, "");
        //}
        //public DataSet GetAttributeProductstoxml(string DataPage, string SearchStr, string AttributeType, string AttributeValue, string Brand, string resultPerPage, string CurrentPageNo, string NextPage, string EA)
        //{
        //    DataSet AttributeProxml = new DataSet();
        //    DataTable AttributeFamProxml = new DataTable("FamilyProxml");
        //    DataTable AttributeFamxml = new DataTable("Familyxml");
        //    try
        //    {

        //        string temp;
        //        string temp1;
        //        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
        //        IOptions opts = ea.getOptions();
        //        opts.setResultsPerPage(resultPerPage); // ea_rpp.Value);   // use current settings

        //        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        //        opts.setSubCategories(true);
        //        opts.setReturnSKUs(false);
        //        opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);//"Category;1000"
        //        opts.setNavigateHierarchy(false);
        //        opts.setResultsPerPage(resultPerPage);

        //        if (DataPage == "CategoryProductList")
        //        {
        //            AttributeProxml.Tables.Add("TOTAL_PAGES");
        //            AttributeProxml.Tables["TOTAL_PAGES"].Columns.Add("TOTAL_PAGES", typeof(int));

        //            //For Total Products.
        //            AttributeProxml.Tables.Add("TOTAL_PRODUCTS");
        //            AttributeProxml.Tables["TOTAL_PRODUCTS"].Columns.Add("TOTAL_PRODUCTS", typeof(string));



        //            AttributeProxml.Tables.Add(AttributeFamProxml);
        //            AttributeFamProxml.Columns.Add("FAMILY_ID", typeof(string));
                
        //            AttributeFamProxml.Columns.Add("FAMILY_NAME", typeof(string));
        //            AttributeFamProxml.Columns.Add("PRODUCT_ID", typeof(string));
        //            AttributeFamProxml.Columns.Add("PRODUCT_CODE", typeof(string));
               
        //            AttributeFamProxml.Columns.Add("ATTRIBUTE_ID", typeof(Int32));
        //            AttributeFamProxml.Columns.Add("STRING_VALUE", typeof(string));
        //                  AttributeFamProxml.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
        //            AttributeFamProxml.Columns.Add("SUBCATNAME_L1", typeof(string));
        //            AttributeFamProxml.Columns.Add("PRODUCT_COUNT", typeof(string));
        //            AttributeFamProxml.Columns.Add("FAMILY_PRODUCT_COUNT", typeof(string));
                 
        //            AttributeFamProxml.Columns.Add("SNO", typeof(int));

        //        }


        //        INavigateResults res = null;

        //        if (EA == "")
        //        {
        //            if (int.Parse(CurrentPageNo) <= 0)
        //            {
        //                if (SearchStr != "" && AttributeValue == "" && AttributeType == "")
        //                {
        //                    res = ea.userSearch("AllProducts////WESAUSTRALASIA", SearchStr);
        //                }
        //                else
        //                {
        //                    if (AttributeType.ToLower() == "category")
        //                    {
        //                        if (HttpContext.Current.Session["EA"] != null)
        //                        {
        //                            if (HttpContext.Current.Session["EA"].ToString().ToLower().EndsWith("////" + AttributeValue.ToLower()))
        //                                res = ea.userBreadCrumbClick(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()));
        //                            else
        //                                res = ea.userBreadCrumbClick(HttpUtility.UrlEncode("AllProducts////WESAUSTRALASIA" + "////" + AttributeValue));
        //                            //HttpContext.Current.Session["EA"] = res.getCatPath();
        //                        }

        //                    }
                           
        //                }

        //            }
                    
        //            HttpContext.Current.Session["EA"] = res.getCatPath();
        //            //CreateYouHaveSelectAndBreadCrumb();
        //        }
               
        //        if (DataPage != "FamilyPage" && DataPage != "ProductPage")
        //        {
        //            DataRow dr = AttributeProxml.Tables["TOTAL_PAGES"].NewRow();
        //            dr[0] = res.getPageCount();
        //            AttributeProxml.Tables["TOTAL_PAGES"].Rows.Add(dr);

        //            DataRow dr1 = AttributeProxml.Tables["TOTAL_PRODUCTS"].NewRow();
        //            dr1[0] = res.getTotalItems();
        //            AttributeProxml.Tables["TOTAL_PRODUCTS"].Rows.Add(dr1);
        //        }





        //        if (DataPage == "CategoryProductList")
        //            Get_FamilyPage_Product_Values_xml(DataPage, AttributeFamProxml, res, null, null);



               
        //        HttpContext.Current.Session["FamilyProductxml"] = AttributeProxml;

        //        return AttributeProxml;
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorhandler.ErrorMsg = ex;
        //        objErrorhandler.CreateLog(ex.ToString());
        //        objErrorhandler.CreateLog();
        //        return null;
        //    }
        //    finally
        //    {
        //        AttributeProxml = null;
        //        AttributeFamProxml = null;
        //        AttributeFamxml = null;
        //    }
        //}
        //public DataTable Get_FamilyPage_Product_Values_xml(string DataType, DataTable ds, INavigateResults res, IResultRow item1, String name)
        //{
        //    int last = res.getLastItem();
        //    int colFmlyID = res.getColumnIndex("Family Id");
        //    int colSubFmlyID = res.getColumnIndex("SubFamily Id");
        //    int colFmlyName = res.getColumnIndex("Family Name");
        //            int colProductID = res.getColumnIndex("Prod Id");
        //    int colProductCode = res.getColumnIndex("Prod Code");
        //    //  int colProductPrice = res.getColumnIndex("Price");
        //    int colProductDesc = res.getColumnIndex("Prod Description");
        //         int colProductCount = res.getColumnIndex("Prod Count");
        //    int colFamilyProdCount = res.getColumnIndex("Family Prod Count");
        //          string _ProCode = "";
     
        //    string _ProductDesc = "";
         
        //    int sno = 0;
        //    //IList<INavigateCategory> item = res.getDetailedCategories();





        //    DataRow dRow;
        //    try
        //    {
        //        if (last >= 0)
        //        {

        //            for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++)
        //            {
        //                sno = sno + 1;
        //                dRow = ds.NewRow();

        //                dRow["FAMILY_ID"] = res.getCellData(i, colFmlyID);
                       
        //                dRow["FAMILY_NAME"] = res.getCellData(i, colFmlyName);
                  
        //                dRow["PRODUCT_ID"] = res.getCellData(i, colProductID);
        //                dRow["PRODUCT_CODE"] = res.getCellData(i, colProductCode);

                  

        //                string temp_family_count = res.getCellData(i, colFamilyProdCount);

        //                string temp_product_count = res.getCellData(i, colProductCount);
                    
        //                dRow["PRODUCT_COUNT"] = (temp_product_count == null || temp_product_count == "") ? "0" : temp_product_count;
        //                dRow["FAMILY_PRODUCT_COUNT"] = (temp_family_count == null || temp_family_count == "") ? "0" : temp_family_count;


                        

        //                _ProCode = res.getCellData(i, colProductCode);
        //                // _ProductPrice = res.getCellData(i, colProductPrice);

        //                // _FmlyDesc = res.getCellData(i, colFmlyDesc);
        //                _ProductDesc = res.getCellData(i, colProductDesc);
        //                //  _FmlylongDesc = res.getCellData(i, colFmlylongDesc);
        //                dRow["SUBCATNAME_L1"] = "";
                      
        //                dRow["SNO"] = sno;

        //                if (DataType == "ProductList" || DataType == "CategoryProductList")
        //                {

        //                    dRow["ATTRIBUTE_ID"] = "1";
        //                    dRow["STRING_VALUE"] = _ProCode; //For the Product Code
                        
        //                    dRow["ATTRIBUTE_TYPE"] = "1";
        //                    ds.Rows.Add(dRow.ItemArray);



        //                }
        //                j++;
        //            }
        //            // Get Family Category & Sub Category Name from DB
        //            DataTable Sqltb = new DataTable();
        //            tmpstr = "";


        //            foreach (DataRow dr1 in ds.Rows)
        //            {
        //                if (tmpstr.Contains(dr1["FAMILY_ID"].ToString().ToUpper()) == false)
        //                    tmpstr = tmpstr + "'" + dr1["FAMILY_ID"].ToString().ToUpper() + "',";
        //            }
        //            if (tmpstr != "")
        //                tmpstr = tmpstr.Substring(0, tmpstr.Length - 1) + "";

        //            if (tmpstr != "")
        //            {
        //                //Sqltb = objhelper.GetDataTable(StrSql);
        //                Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, tmpstr, "GET_FAMILY_CATEGORY", HelperDB.ReturnType.RTTable);
        //                if (Sqltb != null)
        //                {
        //                    foreach (DataRow dr in Sqltb.Rows)
        //                    {
        //                        foreach (DataRow dr1 in ds.Rows)
        //                        {
        //                            if (dr["FAMILY_ID"].ToString().ToUpper() == dr1["FAMILY_ID"].ToString().ToUpper())
        //                            {
                                      
        //                                dr1["SUBCATNAME_L1"] = dr["ParentCat"];
                                       
        //                            }
        //                        }
        //                    }
        //                }
        //            }
                   
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return ds;
        //}

        //public DataTable GetBrandListxml(string Cid, string ReturnType)
        //{
        //    Boolean blnGetData = false;
        //    if (HttpContext.Current.Session["MainMenuClick"] == null)
        //    {
        //        blnGetData = true;
        //    }
        //    else if (((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["ParentCategory"].Rows[0]["CATEGORY_ID"].ToString().ToUpper() != Cid.ToUpper())
        //    {
        //        blnGetData = true;
        //    }

        //    DataTable tmptbl = new DataTable();

        //    string CatName = "";
        //    tmptbl = GetCategoryAndBrand("MainCategory").Tables[0].Select("CATEGORY_ID='" + Cid + "'").CopyToDataTable();
        //    if (tmptbl != null && tmptbl.Rows.Count > 0)
        //    {
        //        CatName = tmptbl.Rows[0]["CATEGORY_NAME"].ToString();
        //    }

        //    if (blnGetData == true)
        //    {

        //        if (CatName != "")
        //        {
        //            IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WesCatBrandDictionary);
        //            IOptions opts = ea.getOptions();
        //            opts.setResultsPerPage("1");
        //            opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);
        //            opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
        //            opts.setSubCategories(true);
        //            opts.setNavigateHierarchy(false);
        //            opts.setReturnSKUs(false);

        //            INavigateResults res = ea.userBreadCrumbClick("AllProducts////WESAUSTRALASIA////" + CatName);
        //            String path = res.getCatPath();
        //            HttpContext.Current.Session["EA"] = res.getCatPath();
        //           // CreateYouHaveSelectAndBreadCrumb();

        //            IList<INavigateCategory> list = res.getDetailedCategories();
        //            //IList<INavigateCategory> li = res.getDetailedAttributeValues(  ();
        //            //For Brand Table.
        //            GetBrandlisttoxmldata(list, res, CatName, Cid);

        //            //DataRow row2 = Menu_Parent.NewRow();
        //            //row2["CATEGORY_ID"] = Cid.ToUpper();
        //            //Menu_Parent.Rows.Add(row2);


        //        }
        //    }

        //    return (DataTable)HttpContext.Current.Session["brand_xml"];
            
           
        //}

        //public DataSet GetAttributeProducts(string DataPage, string SearchStr, string AttributeType, string AttributeValue, string Brand, string resultPerPage, string CurrentPageNo, string NextPage, string EA)
        //{
        //    try
        //    {

        //        string temp;
        //        string temp1;
        //        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
        //        IOptions opts = ea.getOptions();
        //        opts.setResultsPerPage(resultPerPage); // ea_rpp.Value);   // use current settings

        //        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        //        opts.setSubCategories(true);
        //        opts.setReturnSKUs(false);
        //        opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);//"Category;1000"
        //        opts.setNavigateHierarchy(false);
        //        opts.setResultsPerPage(resultPerPage);

        //        if (DataPage == "FamilyPage" || DataPage == "ProductPage")
        //        {
        //            opts.setSubCategories(false);
        //            opts.setReturnSKUs(true);
        //            opts.setResultsPerPage(0);
        //        }
        //        if (DataPage == "ByBrand")
        //        {
        //            //opts.setGrouping("Category;" + resultPerPage.ToString());
        //            //opts.setGrouping("Category;" + "3000"); 
        //            opts.setReturnSKUs(false);
        //            //opts.setResultsPerPage(0);
        //        }




        //        int PriceCode = GetPriceCode();
        //        if (PriceCode != -1)
        //            opts.setCallOutParam("&eap_PriceCode=" + PriceCode.ToString());



        //        if (DataPage == "FamilyPage")
        //        {
        //            AttributePro.Tables.Add(AttributeFam);
        //            AttributeFam.Columns.Add("FAMILY_ID", typeof(string));
        //            AttributeFam.Columns.Add("FAMILY_NAME", typeof(string));
        //            AttributeFam.Columns.Add("STRING_VALUE", typeof(string));
        //            AttributeFam.Columns.Add("NUMERIC_VALUE", typeof(string));
        //            AttributeFam.Columns.Add("ATTRIBUTE_ID", typeof(Int32));
        //            AttributeFam.Columns.Add("ATTRIBUTE_NAME", typeof(string));
        //            AttributeFam.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
        //            AttributeFam.Columns.Add("OBJECT_TYPE", typeof(string));
        //            AttributeFam.Columns.Add("OBJECT_NAME", typeof(string));
        //            AttributeFam.Columns.Add("ATTRIBUTE_DATA_TYPE", typeof(string));
        //            AttributeFam.Columns.Add("FAMILY_PROD_COUNT", typeof(string));
        //            AttributeFam.Columns.Add("PROD_COUNT", typeof(string));
        //            AttributeFam.Columns.Add("STATUS", typeof(string));

        //            AttributePro.Tables.Add(AttributeFamPro);
        //            AttributeFamPro.Columns.Add("FAMILY_ID", typeof(string));
        //            AttributeFamPro.Columns.Add("FAMILY_NAME", typeof(string));
        //            AttributeFamPro.Columns.Add("FAMILY_TH_IMAGE", typeof(string));
        //            AttributeFamPro.Columns.Add("FAMILY_SHORT_DESC", typeof(string));
        //            AttributeFamPro.Columns.Add("FAMILY_DESC", typeof(string));
        //            AttributeFamPro.Columns.Add("PRODUCT_ID", typeof(string));
        //            AttributeFamPro.Columns.Add("PRODUCT_CODE", typeof(string));
        //            AttributeFamPro.Columns.Add("PRODUCT_PRICE", typeof(string));
        //            AttributeFamPro.Columns.Add("PRODUCT_DESC", typeof(string));
        //            AttributeFamPro.Columns.Add("PRODUCT_TH_IMAGE", typeof(string));
        //            AttributeFamPro.Columns.Add("SUB_FAMILY_COUNT", typeof(string));
        //            AttributeFamPro.Columns.Add("PRODUCT_COUNT", typeof(string));
        //            AttributeFamPro.Columns.Add("FAMILY_PRODUCT_COUNT", typeof(string));
        //            AttributeFamPro.Columns.Add("QTY_AVAIL", typeof(string));
        //            AttributeFamPro.Columns.Add("MIN_ORD_QTY", typeof(string));


        //        }
        //        else if (DataPage == "ProductPage")
        //        {
        //            AttributePro.Tables.Add(AttributeFamPro);
        //            AttributeFamPro.Columns.Add("PRODUCT_ID", typeof(string));
        //            AttributeFamPro.Columns.Add("STRING_VALUE", typeof(string));
        //            AttributeFamPro.Columns.Add("NUMERIC_VALUE", typeof(string));
        //            AttributeFamPro.Columns.Add("ATTRIBUTE_ID", typeof(Int32));
        //            AttributeFamPro.Columns.Add("ATTRIBUTE_NAME", typeof(string));
        //            AttributeFamPro.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
        //            AttributeFamPro.Columns.Add("OBJECT_TYPE", typeof(string));
        //            AttributeFamPro.Columns.Add("OBJECT_NAME", typeof(string));
        //            AttributeFamPro.Columns.Add("ATTRIBUTE_DATATYPE", typeof(string));
        //            AttributeFamPro.Columns.Add("QTY_AVAIL", typeof(string));
        //            AttributeFamPro.Columns.Add("MIN_ORD_QTY", typeof(string));
        //            AttributeFamPro.Columns.Add("FAMILY_ID", typeof(string));
        //            AttributeFamPro.Columns.Add("FAMILY_NAME", typeof(string));
        //            AttributeFamPro.Columns.Add("FAMILY_TH_IMAGE", typeof(string));
        //            AttributeFamPro.Columns.Add("FAMILY_SHORT_DESC", typeof(string));
        //            AttributeFamPro.Columns.Add("FAMILY_DESC", typeof(string));
        //            AttributeFamPro.Columns.Add("STOCK_STATUS_DESC", typeof(string));
        //            AttributeFamPro.Columns.Add("FAMILY_PROD_COUNT", typeof(string));
        //            AttributeFamPro.Columns.Add("PROD_COUNT", typeof(string));
        //            //AttributePro.Tables.Add(AttributeFam);
        //            //AttributeFam.Columns.Add("FAMILY_ID", typeof(string));
        //            //AttributeFam.Columns.Add("FAMILY_NAME", typeof(string));
        //            //AttributeFam.Columns.Add("FAMILY_TH_IMAGE", typeof(string));
        //            //AttributeFam.Columns.Add("FAMILY_SHORT_DESC", typeof(string));
        //            //AttributeFam.Columns.Add("FAMILY_DESC", typeof(string));



        //        }
        //        else
        //        {
        //            AttributePro.Tables.Add("TOTAL_PAGES");
        //            AttributePro.Tables["TOTAL_PAGES"].Columns.Add("TOTAL_PAGES", typeof(int));

        //            //For Total Products.
        //            AttributePro.Tables.Add("TOTAL_PRODUCTS");
        //            AttributePro.Tables["TOTAL_PRODUCTS"].Columns.Add("TOTAL_PRODUCTS", typeof(string));



        //            AttributePro.Tables.Add(AttributeFamPro);
        //            AttributeFamPro.Columns.Add("FAMILY_ID", typeof(string));
        //            AttributeFamPro.Columns.Add("CATEGORY_ID", typeof(string));
        //            AttributeFamPro.Columns.Add("PARENT_CATEGORY_ID", typeof(string));
        //            AttributeFamPro.Columns.Add("FAMILY_NAME", typeof(string));
        //            AttributeFamPro.Columns.Add("PRODUCT_ID", typeof(string));
        //            AttributeFamPro.Columns.Add("PRODUCT_CODE", typeof(string));
        //            AttributeFamPro.Columns.Add("PRODUCT_PRICE", typeof(string));
        //            AttributeFamPro.Columns.Add("ATTRIBUTE_ID", typeof(Int32));
        //            AttributeFamPro.Columns.Add("STRING_VALUE", typeof(string));
        //            AttributeFamPro.Columns.Add("NUMERIC_VALUE", typeof(string));
        //            AttributeFamPro.Columns.Add("OBJECT_TYPE", typeof(string));
        //            AttributeFamPro.Columns.Add("OBJECT_NAME", typeof(string));
        //            AttributeFamPro.Columns.Add("ATTRIBUTE_NAME", typeof(string));
        //            AttributeFamPro.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
        //            AttributeFamPro.Columns.Add("SUBCATNAME_L1", typeof(string));
        //            AttributeFamPro.Columns.Add("SUBCATNAME_L2", typeof(string));
        //            AttributeFamPro.Columns.Add("CATEGORY_NAME", typeof(string));
        //            AttributeFamPro.Columns.Add("PARENT_CATEGORY_NAME", typeof(string));
        //            AttributeFamPro.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
        //            AttributeFamPro.Columns.Add("SUB_FAMILY_COUNT", typeof(string));
        //            AttributeFamPro.Columns.Add("PRODUCT_COUNT", typeof(string));
        //            AttributeFamPro.Columns.Add("FAMILY_PRODUCT_COUNT", typeof(string));
        //            AttributeFamPro.Columns.Add("QTY_AVAIL", typeof(string));
        //            AttributeFamPro.Columns.Add("MIN_ORD_QTY", typeof(string));
        //            AttributeFamPro.Columns.Add("MIN_PRICE", typeof(string));
        //            AttributeFamPro.Columns.Add("STOCK_STATUS_DESC", typeof(string));
        //            AttributeFamPro.Columns.Add("SUB_FAMILY_ID", typeof(string));
        //            AttributeFamPro.Columns.Add("SNO", typeof(int));
        //            /*if (DataPage == "ByBrand")
        //            {
        //                AttributePro.Tables.Add(CategoryGrouptb);
        //                CategoryGrouptb.Columns.Add("CATEGORY_ID", typeof(string));
        //                CategoryGrouptb.Columns.Add("CATEGORY_NAME", typeof(string));
        //                CategoryGrouptb.Columns.Add("SUB_CATEGORY_COUNT", typeof(string));
        //            }*/


        //        }


        //        INavigateResults res = null;

        //        if (EA == "")
        //        {
        //            if (int.Parse(CurrentPageNo) <= 0)
        //            {
        //                if (SearchStr != "" && AttributeValue == "" && AttributeType == "")
        //                {
        //                    res = ea.userSearch("AllProducts////WESAUSTRALASIA", SearchStr);
        //                }
        //                else
        //                {
        //                    if (AttributeType == "Category")
        //                    {
        //                        if (HttpContext.Current.Session["EA"] != null)
        //                        {
        //                            if (HttpContext.Current.Session["EA"].ToString().ToLower().EndsWith("////" + AttributeValue.ToLower()))
        //                                res = ea.userBreadCrumbClick(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()));
        //                            else
        //                                res = ea.userBreadCrumbClick(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString() + "////" + AttributeValue));
        //                            //HttpContext.Current.Session["EA"] = res.getCatPath();
        //                        }

        //                    }
        //                    else if (AttributeType == "FamilyId")
        //                    {
        //                        if (HttpContext.Current.Session["EA"] != null)
        //                        {
        //                            if (HttpContext.Current.Session["EA"].ToString().EndsWith("Family Id=" + AttributeValue))
        //                            {
        //                                HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace("Family Id=" + AttributeValue, "");
        //                                res = ea.userSearch(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), "Family Id=" + AttributeValue);
        //                            }
        //                            else
        //                                res = ea.userSearch(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), "Family Id=" + AttributeValue);
        //                        }

        //                    }
        //                    else if (AttributeType == "ProductId")
        //                    {
        //                        if (HttpContext.Current.Session["EA"] != null)
        //                        {
        //                            if (HttpContext.Current.Session["EA"].ToString().EndsWith("Prod Id=" + AttributeValue))
        //                            {
        //                                HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace("Prod Id=" + AttributeValue, "");
        //                                res = ea.userSearch(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), "Prod Id=" + AttributeValue);
        //                            }
        //                            else
        //                                res = ea.userSearch(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), "Prod Id=" + AttributeValue);
        //                        }

        //                    }
        //                    else
        //                    {
        //                        if (AttributeType == "Model")
        //                        {

        //                            temp = "" + AttributeType + " = '" + HttpUtility.UrlEncode(Brand) + ":" + HttpUtility.UrlEncode(AttributeValue) + "'";
        //                            temp1 = "" + AttributeType + " = ";
        //                        }
        //                        else
        //                        {
        //                            temp = "" + AttributeType + " = '" + HttpUtility.UrlEncode(AttributeValue) + "'";
        //                            temp1 = "" + AttributeType + " = ";
        //                        }
        //                        if (HttpContext.Current.Session["EA"] != null)
        //                        {
        //                            if (HttpContext.Current.Session["EA"].ToString().Contains(temp1))
        //                            {
        //                                int t = HttpContext.Current.Session["EA"].ToString().IndexOf(temp1) - 17;
        //                                string t1 = HttpContext.Current.Session["EA"].ToString().Remove(t);
        //                                HttpContext.Current.Session["EA"] = t1;

        //                            }

        //                            res = ea.userAttributeClick(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), temp);
        //                            //HttpContext.Current.Session["EA"] = res.getCatPath();

        //                        }
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                if (HttpContext.Current.Session["EA"] != null)
        //                {
        //                    res = ea.userPageOp(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), CurrentPageNo, NextPage);
        //                }
        //            }
        //            if (DataPage == "ps")
        //            {
        //                updateSearchSpell_Correction(res);
        //            }
        //            HttpContext.Current.Session["EA"] = res.getCatPath();
        //            CreateYouHaveSelectAndBreadCrumb();
        //        }
        //        else
        //        {

        //            if (int.Parse(CurrentPageNo) <= 0)
        //            {
        //                if (SearchStr != "" && AttributeValue == "" && AttributeType == "")
        //                {
        //                    res = ea.userSearch("AllProducts////WESAUSTRALASIA", SearchStr);
        //                }
        //                else
        //                {
        //                    if (AttributeType == "Category")
        //                    {
        //                        if (EA != null)
        //                        {
        //                            if (EA.ToString().ToLower().EndsWith("////" + AttributeValue.ToLower()))
        //                                res = ea.userBreadCrumbClick(HttpUtility.UrlEncode(EA.ToString()));
        //                            else
        //                                res = ea.userBreadCrumbClick(HttpUtility.UrlEncode(EA.ToString() + "////" + AttributeValue));
        //                            //HttpContext.Current.Session["EA"] = res.getCatPath();
        //                        }

        //                    }
        //                    else if (AttributeType == "FamilyId")
        //                    {
        //                        if (EA != null)
        //                        {
        //                            if (EA.ToString().EndsWith("Family Id=" + AttributeValue))
        //                            {
        //                                EA = EA.ToString().Replace("Family Id=" + AttributeValue, "");
        //                                res = ea.userSearch(HttpUtility.UrlEncode(EA.ToString()), "Family Id=" + AttributeValue);
        //                            }
        //                            else
        //                                res = ea.userSearch(HttpUtility.UrlEncode(EA.ToString()), "Family Id=" + AttributeValue);
        //                        }

        //                    }
        //                    else if (AttributeType == "ProductId")
        //                    {
        //                        if (EA != null)
        //                        {
        //                            if (EA.ToString().EndsWith("Prod Id=" + AttributeValue))
        //                            {
        //                                EA = EA.ToString().Replace("Prod Id=" + AttributeValue, "");
        //                                res = ea.userSearch(HttpUtility.UrlEncode(EA.ToString()), "Prod Id=" + AttributeValue);
        //                            }
        //                            else
        //                                res = ea.userSearch(HttpUtility.UrlEncode(EA.ToString()), "Prod Id=" + AttributeValue);
        //                        }

        //                    }
        //                    else
        //                    {
        //                        if (AttributeType == "Model")
        //                        {

        //                            temp = "" + AttributeType + " = '" + HttpUtility.UrlEncode(Brand) + ":" + HttpUtility.UrlEncode(AttributeValue) + "'";
        //                            temp1 = "" + AttributeType + " = ";
        //                        }
        //                        else
        //                        {
        //                            temp = "" + AttributeType + " = '" + HttpUtility.UrlEncode(AttributeValue) + "'";
        //                            temp1 = "" + AttributeType + " = ";
        //                        }
        //                        if (EA != null)
        //                        {
        //                            if (EA.ToString().Contains(temp1))
        //                            {
        //                                int t = EA.ToString().IndexOf(temp1) - 17;
        //                                string t1 = EA.ToString().Remove(t);
        //                                EA = t1;

        //                            }

        //                            res = ea.userAttributeClick(HttpUtility.UrlEncode(EA.ToString()), temp);
        //                            //HttpContext.Current.Session["EA"] = res.getCatPath();

        //                        }
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                if (EA != null)
        //                {
        //                    res = ea.userPageOp(HttpUtility.UrlEncode(EA), CurrentPageNo, NextPage);
        //                }
        //            }
        //            if (DataPage == "ps")
        //            {
        //                updateSearchSpell_Correction(res);
        //            }
        //            EA = res.getCatPath();

        //            AttributePro.Tables.Add("eapath");
        //            AttributePro.Tables["eapath"].Columns.Add("EA");
        //            DataRow drEA = AttributePro.Tables["eapath"].NewRow();
        //            drEA[0] = EA;

        //            //FormName = FormName.Replace(".ASPX", ""); 
        //            //dr["FormName"] = FormName;
        //            AttributePro.Tables["eapath"].Rows.Add(drEA);

        //            CreateYouHaveSelectAndBreadCrumb(EA);



        //        }
        //        if (DataPage != "FamilyPage" && DataPage != "ProductPage")
        //        {
        //            DataRow dr = AttributePro.Tables["TOTAL_PAGES"].NewRow();
        //            dr[0] = res.getPageCount();
        //            AttributePro.Tables["TOTAL_PAGES"].Rows.Add(dr);

        //            DataRow dr1 = AttributePro.Tables["TOTAL_PRODUCTS"].NewRow();
        //            dr1[0] = res.getTotalItems();
        //            AttributePro.Tables["TOTAL_PRODUCTS"].Rows.Add(dr1);
        //        }

        //        /* if (DataPage == "ByBrand")
        //         {
        //             IList<INavigateCategory> Category = res.getDetailedCategories();
        //             if (Category.Count > 0)
        //             {

        //                 foreach (INavigateCategory categoryItem in Category)
        //                 {
        //                     DataRow row = CategoryGrouptb.NewRow();
        //                     IList<string> Id = categoryItem.getIDs();
        //                     row["CATEGORY_ID"] = Id[0].ToString().Substring(2);
        //                     row["CATEGORY_NAME"] = categoryItem.getName();
                           
        //                     int subCatCount = 0;
        //                     IList<INavigateCategory> SubCat_List = categoryItem.getSubCategories();
        //                     foreach (INavigateCategory item1 in SubCat_List)
        //                     {

        //                         subCatCount = subCatCount + 1;
        //                     }
        //                     row["SUB_CATEGORY_COUNT"] = subCatCount.ToString();
        //                     CategoryGrouptb.Rows.Add(row);
                            
        //                 }
        //             }
        //         }*/



        //        if (DataPage == "FamilyPage")
        //            Get_FamilyPage_Product_Values(DataPage, AttributeFam, AttributeFamPro, res, null, null);
        //        else if (DataPage == "ProductPage")
        //            Get_ProductPage_Product_Values(DataPage, AttributeFam, AttributeFamPro, res, null, null);
        //        else if (DataPage == "ByBrand")
        //            //Get_BrandModel_Product_Values(DataPage, AttributeFamPro, res);                        
        //            Get_Family_Product_Values(DataPage, AttributeFamPro, res, null, null);
        //        else
        //            Get_Family_Product_Values(DataPage, AttributeFamPro, res, null, null);


        //        /* if (DataPage == "ByBrand")
        //         {
        //             if (CategoryGrouptb.Rows.Count == 0)
        //             {
        //                 AttributePro.Tables.RemoveAt(3);
        //                 CategoryGrouptb = AttributeFamPro.DefaultView.ToTable(true, "CATEGORY_ID", "CATEGORY_NAME").Copy();
        //                 CategoryGrouptb.TableName = "Category";
        //                 AttributePro.Tables.Add(CategoryGrouptb);  
        //             }
        //         }
        //         */
        //        IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //        if (AttributeType == "Brand")
        //            HttpContext.Current.Session["LHSAttributes"] = GetCategoryAttribute(Attributes, res, AttributeValue, SearchStr);
        //        else
        //            HttpContext.Current.Session["LHSAttributes"] = GetCategoryAttribute(Attributes, res, "", SearchStr);

        //        if (DataPage == "ProductList" || DataPage == "ps")
        //        {
        //            AttributeFam = AttributeFamPro.DefaultView.ToTable(true, "Family_Id", "Family_Name", "FAMILY_PRODUCT_COUNT", "SUB_FAMILY_COUNT", "PRODUCT_COUNT").Copy();
        //            AttributeFam.TableName = "Family";
        //            AttributePro.Tables.Add(AttributeFam);
        //        }
        //        HttpContext.Current.Session["FamilyProduct"] = AttributePro;

        //        return AttributePro;
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorhandler.ErrorMsg = ex;
        //        objErrorhandler.CreateLog();
        //        return null;
        //    }
        //    finally
        //    {
        //        AttributePro = null;
        //        AttributeFamPro = null;
        //        AttributeFam = null;
        //    }
        //}
        public DataSet GetAttributeProducts(string DataPage, string SearchStr, string AttributeType, string AttributeValue, string Brand, string resultPerPage, string CurrentPageNo, string NextPage, string EA)
        {
            //try
            //{



            //    string temp;
            //    string temp1;
            //    IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
            //    IOptions opts = ea.getOptions();
            //    opts.setResultsPerPage(resultPerPage); // ea_rpp.Value);   // use current settings

            //    opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
            //    opts.setSubCategories(true);
            //    opts.setReturnSKUs(false);
            //    opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);//"Category;1000"
            //    opts.setNavigateHierarchy(false);
            //    opts.setResultsPerPage(resultPerPage);

            //    if (DataPage == "FamilyPage" || DataPage == "ProductPage")
            //    {
            //        opts.setSubCategories(false);
            //        opts.setReturnSKUs(true);
            //        opts.setResultsPerPage(0);
            //    }
            //    if (DataPage == "ByBrand")
            //    {
            //        //opts.setGrouping("Category;" + resultPerPage.ToString());
            //        //opts.setGrouping("Category;" + "3000"); 
            //        opts.setReturnSKUs(false);
            //        //opts.setResultsPerPage(0);
            //    }




            //    int PriceCode = GetPriceCode();

            //    if (PriceCode != -1)
            //        opts.setCallOutParam("&eap_PriceCode=" + PriceCode.ToString());
            //    else if (PriceCode == -1)
            //        opts.setCallOutParam("&eap_PriceCode=" + Dum_Price_Code.ToString());



            //    if (DataPage == "FamilyPage")
            //    {
            //        AttributePro.Tables.Add(AttributeFam);
            //        AttributeFam.Columns.Add("FAMILY_ID", typeof(string));
            //        AttributeFam.Columns.Add("FAMILY_NAME", typeof(string));
            //        AttributeFam.Columns.Add("STRING_VALUE", typeof(string));
            //        AttributeFam.Columns.Add("NUMERIC_VALUE", typeof(string));
            //        AttributeFam.Columns.Add("ATTRIBUTE_ID", typeof(Int32));
            //        AttributeFam.Columns.Add("ATTRIBUTE_NAME", typeof(string));
            //        AttributeFam.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
            //        AttributeFam.Columns.Add("OBJECT_TYPE", typeof(string));
            //        AttributeFam.Columns.Add("OBJECT_NAME", typeof(string));
            //        AttributeFam.Columns.Add("ATTRIBUTE_DATA_TYPE", typeof(string));
            //        AttributeFam.Columns.Add("FAMILY_PROD_COUNT", typeof(string));
            //        AttributeFam.Columns.Add("PROD_COUNT", typeof(string));
            //        AttributeFam.Columns.Add("STATUS", typeof(string));

            //        AttributePro.Tables.Add(AttributeFamPro);
            //        AttributeFamPro.Columns.Add("FAMILY_ID", typeof(string));
            //        AttributeFamPro.Columns.Add("FAMILY_NAME", typeof(string));
            //        AttributeFamPro.Columns.Add("FAMILY_TH_IMAGE", typeof(string));
            //        AttributeFamPro.Columns.Add("FAMILY_SHORT_DESC", typeof(string));
            //        AttributeFamPro.Columns.Add("FAMILY_DESC", typeof(string));
            //        AttributeFamPro.Columns.Add("PRODUCT_ID", typeof(string));
            //        AttributeFamPro.Columns.Add("PRODUCT_CODE", typeof(string));
            //        // AttributeFamPro.Columns.Add("PROD_CODE", typeof(string));//new 
            //        AttributeFamPro.Columns.Add("PRODUCT_PRICE", typeof(string));
            //        AttributeFamPro.Columns.Add("PRODUCT_DESC", typeof(string));
            //        AttributeFamPro.Columns.Add("PRODUCT_TH_IMAGE", typeof(string));
            //        AttributeFamPro.Columns.Add("SUB_FAMILY_COUNT", typeof(string));
            //        AttributeFamPro.Columns.Add("PRODUCT_COUNT", typeof(string));
            //        AttributeFamPro.Columns.Add("FAMILY_PRODUCT_COUNT", typeof(string));
            //        AttributeFamPro.Columns.Add("QTY_AVAIL", typeof(string));
            //        AttributeFamPro.Columns.Add("MIN_ORD_QTY", typeof(string));


            //    }
            //    else if (DataPage == "ProductPage")
            //    {
            //        AttributePro.Tables.Add(AttributeFamPro);
            //        AttributeFamPro.Columns.Add("PRODUCT_ID", typeof(string));
            //        AttributeFamPro.Columns.Add("STRING_VALUE", typeof(string));
            //        AttributeFamPro.Columns.Add("NUMERIC_VALUE", typeof(string));
            //        AttributeFamPro.Columns.Add("ATTRIBUTE_ID", typeof(Int32));
            //        AttributeFamPro.Columns.Add("ATTRIBUTE_NAME", typeof(string));
            //        AttributeFamPro.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
            //        AttributeFamPro.Columns.Add("OBJECT_TYPE", typeof(string));
            //        AttributeFamPro.Columns.Add("OBJECT_NAME", typeof(string));
            //        AttributeFamPro.Columns.Add("ATTRIBUTE_DATATYPE", typeof(string));
            //        AttributeFamPro.Columns.Add("QTY_AVAIL", typeof(string));
            //        AttributeFamPro.Columns.Add("MIN_ORD_QTY", typeof(string));
            //        AttributeFamPro.Columns.Add("FAMILY_ID", typeof(string));
            //        AttributeFamPro.Columns.Add("FAMILY_NAME", typeof(string));
            //        AttributeFamPro.Columns.Add("FAMILY_TH_IMAGE", typeof(string));
            //        AttributeFamPro.Columns.Add("FAMILY_SHORT_DESC", typeof(string));
            //        AttributeFamPro.Columns.Add("FAMILY_DESC", typeof(string));
            //        AttributeFamPro.Columns.Add("STOCK_STATUS_DESC", typeof(string));
            //        AttributeFamPro.Columns.Add("FAMILY_PROD_COUNT", typeof(string));
            //        AttributeFamPro.Columns.Add("PROD_COUNT", typeof(string));
            //        //AttributePro.Tables.Add(AttributeFam);
            //        //AttributeFam.Columns.Add("FAMILY_ID", typeof(string));
            //        //AttributeFam.Columns.Add("FAMILY_NAME", typeof(string));
            //        //AttributeFam.Columns.Add("FAMILY_TH_IMAGE", typeof(string));
            //        //AttributeFam.Columns.Add("FAMILY_SHORT_DESC", typeof(string));
            //        //AttributeFam.Columns.Add("FAMILY_DESC", typeof(string));



            //    }
            //    else
            //    {
            //        AttributePro.Tables.Add("TOTAL_PAGES");
            //        AttributePro.Tables["TOTAL_PAGES"].Columns.Add("TOTAL_PAGES", typeof(int));

            //        //For Total Products.
            //        AttributePro.Tables.Add("TOTAL_PRODUCTS");
            //        AttributePro.Tables["TOTAL_PRODUCTS"].Columns.Add("TOTAL_PRODUCTS", typeof(string));



            //        AttributePro.Tables.Add(AttributeFamPro);
            //        AttributeFamPro.Columns.Add("FAMILY_ID", typeof(string));
            //        AttributeFamPro.Columns.Add("CATEGORY_ID", typeof(string));
            //        AttributeFamPro.Columns.Add("PARENT_CATEGORY_ID", typeof(string));
            //        AttributeFamPro.Columns.Add("FAMILY_NAME", typeof(string));
            //        AttributeFamPro.Columns.Add("PRODUCT_ID", typeof(string));
            //        AttributeFamPro.Columns.Add("PRODUCT_CODE", typeof(string));
            //        AttributeFamPro.Columns.Add("PRODUCT_PRICE", typeof(string));
            //        AttributeFamPro.Columns.Add("ATTRIBUTE_ID", typeof(Int32));
            //        AttributeFamPro.Columns.Add("STRING_VALUE", typeof(string));
            //        AttributeFamPro.Columns.Add("NUMERIC_VALUE", typeof(string));
            //        AttributeFamPro.Columns.Add("OBJECT_TYPE", typeof(string));
            //        AttributeFamPro.Columns.Add("OBJECT_NAME", typeof(string));
            //        AttributeFamPro.Columns.Add("ATTRIBUTE_NAME", typeof(string));
            //        AttributeFamPro.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
            //        AttributeFamPro.Columns.Add("SUBCATNAME_L1", typeof(string));
            //        AttributeFamPro.Columns.Add("SUBCATNAME_L2", typeof(string));
            //        AttributeFamPro.Columns.Add("CATEGORY_NAME", typeof(string));
            //        AttributeFamPro.Columns.Add("PARENT_CATEGORY_NAME", typeof(string));
            //        AttributeFamPro.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
            //        AttributeFamPro.Columns.Add("SUB_FAMILY_COUNT", typeof(string));
            //        AttributeFamPro.Columns.Add("PRODUCT_COUNT", typeof(string));
            //        AttributeFamPro.Columns.Add("FAMILY_PRODUCT_COUNT", typeof(string));
            //        AttributeFamPro.Columns.Add("QTY_AVAIL", typeof(string));
            //        AttributeFamPro.Columns.Add("MIN_ORD_QTY", typeof(string));
            //        AttributeFamPro.Columns.Add("MIN_PRICE", typeof(string));
            //        AttributeFamPro.Columns.Add("STOCK_STATUS_DESC", typeof(string));
            //        AttributeFamPro.Columns.Add("SUB_FAMILY_ID", typeof(string));
            //        AttributeFamPro.Columns.Add("SNO", typeof(int));
            //        /*if (DataPage == "ByBrand")
            //        {
            //            AttributePro.Tables.Add(CategoryGrouptb);
            //            CategoryGrouptb.Columns.Add("CATEGORY_ID", typeof(string));
            //            CategoryGrouptb.Columns.Add("CATEGORY_NAME", typeof(string));
            //            CategoryGrouptb.Columns.Add("SUB_CATEGORY_COUNT", typeof(string));
            //        }*/


            //    }


            //    INavigateResults res = null;

            //    if (EA == "")
            //    {
            //        if (int.Parse(CurrentPageNo) <= 0)
            //        {
            //            if (SearchStr != "" && AttributeValue == "" && AttributeType == "")
            //            {
            //                res = ea.userSearch("AllProducts////WESAUSTRALASIA", SearchStr);
            //            }
            //            else
            //            {
            //                if (AttributeType.ToLower() == "category")
            //                {
            //                    if (HttpContext.Current.Session["EA"] != null)
            //                    {
            //                        if (HttpContext.Current.Session["EA"].ToString().ToLower().EndsWith("////" + AttributeValue.ToLower()))
            //                            res = ea.userBreadCrumbClick(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()));
            //                        else
            //                            res = ea.userBreadCrumbClick(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString() + "////" + AttributeValue));
            //                        //HttpContext.Current.Session["EA"] = res.getCatPath();
            //                    }

            //                }
            //                else if (AttributeType == "FamilyId")
            //                {
            //                    if (HttpContext.Current.Session["EA"] != null)
            //                    {
            //                        if (HttpContext.Current.Session["EA"].ToString().EndsWith("Family Id=" + AttributeValue))
            //                        {
            //                            HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace("Family Id=" + AttributeValue, "");
            //                            res = ea.userSearch(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), "Family Id=" + AttributeValue);
            //                        }
            //                        else
            //                            res = ea.userSearch(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), "Family Id=" + AttributeValue);
            //                    }

            //                }
            //                else if (AttributeType == "ProductId")
            //                {
            //                    if (HttpContext.Current.Session["EA"] != null)
            //                    {
            //                        if (HttpContext.Current.Session["EA"].ToString().EndsWith("Prod Id=" + AttributeValue))
            //                        {
            //                            HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace("Prod Id=" + AttributeValue, "");
            //                            res = ea.userSearch(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), "Prod Id=" + AttributeValue);
            //                        }
            //                        else
            //                            res = ea.userSearch(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), "Prod Id=" + AttributeValue);
            //                    }

            //                }
            //                else
            //                {
            //                    if (AttributeType.ToLower() == "model")
            //                    {

            //                        temp = "" + AttributeType + " = '" + HttpUtility.UrlEncode(Brand) + ":" + HttpUtility.UrlEncode(AttributeValue) + "'";
            //                        temp1 = "" + AttributeType + " = ";
            //                    }
            //                    else
            //                    {
            //                        temp = "" + AttributeType + " = '" + HttpUtility.UrlEncode(AttributeValue) + "'";
            //                        temp1 = "" + AttributeType + " = ";
            //                    }
            //                    if (HttpContext.Current.Session["EA"] != null)
            //                    {
            //                        if (HttpContext.Current.Session["EA"].ToString().Contains(temp1))
            //                        {
            //                            int t = HttpContext.Current.Session["EA"].ToString().IndexOf(temp1) - 17;
            //                            string t1 = HttpContext.Current.Session["EA"].ToString().Remove(t);
            //                            HttpContext.Current.Session["EA"] = t1;

            //                        }

            //                        res = ea.userAttributeClick(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), temp);
            //                        //HttpContext.Current.Session["EA"] = res.getCatPath();

            //                    }
            //                }
            //            }

            //        }
            //        else
            //        {
            //            if (HttpContext.Current.Session["EA"] != null)
            //            {
            //                res = ea.userPageOp(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), CurrentPageNo, NextPage);
            //            }
            //        }
            //        if (DataPage == "ps")
            //        {
            //            updateSearchSpell_Correction(res, SearchStr);
            //        }
            //        HttpContext.Current.Session["EA"] = res.getCatPath();
            //        CreateYouHaveSelectAndBreadCrumb();
            //    }
            //    else
            //    {

            //        if (int.Parse(CurrentPageNo) <= 0)
            //        {
            //            if (SearchStr != "" && AttributeValue == "" && AttributeType == "")
            //            {
            //                res = ea.userSearch("AllProducts////WESAUSTRALASIA", SearchStr);
            //            }
            //            else
            //            {
            //                if (AttributeType.ToLower() == "category")
            //                {
            //                    if (EA != null)
            //                    {
            //                        if (EA.ToString().ToLower().EndsWith("////" + AttributeValue.ToLower()))
            //                            res = ea.userBreadCrumbClick(HttpUtility.UrlEncode(EA.ToString()));
            //                        else
            //                            res = ea.userBreadCrumbClick(HttpUtility.UrlEncode(EA.ToString() + "////" + AttributeValue));
            //                        //HttpContext.Current.Session["EA"] = res.getCatPath();
            //                    }

            //                }
            //                else if (AttributeType == "FamilyId")
            //                {
            //                    if (EA != null)
            //                    {
            //                        if (EA.ToString().EndsWith("Family Id=" + AttributeValue))
            //                        {
            //                            EA = EA.ToString().Replace("Family Id=" + AttributeValue, "");
            //                            res = ea.userSearch(HttpUtility.UrlEncode(EA.ToString()), "Family Id=" + AttributeValue);
            //                        }
            //                        else
            //                            res = ea.userSearch(HttpUtility.UrlEncode(EA.ToString()), "Family Id=" + AttributeValue);
            //                    }

            //                }
            //                else if (AttributeType == "ProductId")
            //                {
            //                    if (EA != null)
            //                    {
            //                        if (EA.ToString().EndsWith("Prod Id=" + AttributeValue))
            //                        {
            //                            EA = EA.ToString().Replace("Prod Id=" + AttributeValue, "");
            //                            res = ea.userSearch(HttpUtility.UrlEncode(EA.ToString()), "Prod Id=" + AttributeValue);
            //                        }
            //                        else
            //                            res = ea.userSearch(HttpUtility.UrlEncode(EA.ToString()), "Prod Id=" + AttributeValue);
            //                    }

            //                }
            //                else
            //                {
            //                    if (AttributeType.ToLower() == "model")
            //                    {

            //                        temp = "" + AttributeType + " = '" + HttpUtility.UrlEncode(Brand) + ":" + HttpUtility.UrlEncode(AttributeValue) + "'";
            //                        temp1 = "" + AttributeType + " = ";
            //                    }
            //                    else
            //                    {
            //                        temp = "" + AttributeType + " = '" + HttpUtility.UrlEncode(AttributeValue) + "'";
            //                        temp1 = "" + AttributeType + " = ";
            //                    }
            //                    if (EA != null)
            //                    {
            //                        if (EA.ToString().Contains(temp1))
            //                        {
            //                            int t = EA.ToString().IndexOf(temp1) - 17;
            //                            string t1 = EA.ToString().Remove(t);
            //                            EA = t1;

            //                        }

            //                        res = ea.userAttributeClick(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), temp);
            //                        //HttpContext.Current.Session["EA"] = res.getCatPath();

            //                    }
            //                }
            //            }

            //        }
            //        else
            //        {
            //            //Modified by Indu for dynamic pagination session prb
            //            //if (HttpContext.Current.Session["EA"] != null)
            //            //{
            //            //    res = ea.userPageOp(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), CurrentPageNo, NextPage);
            //            //}
            //            if (EA != null)
            //            {
            //                res = ea.userPageOp(HttpUtility.UrlEncode(EA), CurrentPageNo, NextPage);
            //            }
            //        }
            //        if (DataPage.ToLower() == "ps")
            //        {
            //            updateSearchSpell_Correction(res, SearchStr);
            //        }
            //        EA = res.getCatPath();



            //        AttributePro.Tables.Add("eapath");
            //        AttributePro.Tables["eapath"].Columns.Add("EA");
            //        DataRow drEA = AttributePro.Tables["eapath"].NewRow();
            //        drEA[0] = EA;

            //        //FormName = FormName.Replace(".ASPX", ""); 
            //        //dr["FormName"] = FormName;
            //        AttributePro.Tables["eapath"].Rows.Add(drEA);

            //        CreateYouHaveSelectAndBreadCrumb(EA);






            //    }
            //    if (DataPage.ToLower() != "ps")
            //    {
            //        int last = res.getLastItem();

            //        if (last < 0)
            //        {
            //            HelperServices objHelperServices = new HelperServices();
            //            string atrvalue = string.Empty;
            //            if (DataPage == "FamilyPage")
            //            {
            //                object ds = objhelperDb.GetGenericDataDB(AttributeValue.ToString(), "GET_FAMILY_NAME", HelperDB.ReturnType.RTString);
            //                if (ds != null)
            //                {
            //                    atrvalue = ds.ToString();
            //                }
            //                else
            //                {
            //                    atrvalue = AttributeValue.ToString();
            //                }
            //                if (atrvalue == "")
            //                {
            //                    atrvalue = AttributeValue.ToString();
            //                }
            //            }
            //            else if (DataPage == "ProductPage")
            //            {
            //                ProductServices objProductServices = new ProductServices();
            //                atrvalue = objProductServices.GetProductCode(Convert.ToInt32(AttributeValue));
            //                string[] attrval = atrvalue.Split('-');
            //                atrvalue = attrval[0];
            //            }
            //            else
            //            {
            //                atrvalue = AttributeValue;
            //            }
            //            if (atrvalue == "")
            //            {
            //                string[] fstring = HttpContext.Current.Request.RawUrl.ToString().Split('/');
            //                string firststring = fstring[0];
            //                atrvalue = firststring;
            //            }
            //            if (atrvalue != "")
            //            {
            //                string strvalue = objHelperServices.Cons_NewURl_bybrand("ps.aspx?srctext=" + HttpUtility.UrlEncode(atrvalue) + "", atrvalue, "ps.aspx", "");
            //                strvalue = "/" + strvalue + "/ps/";
            //                HttpContext.Current.Response.RedirectPermanent(strvalue, false);
            //            }
            //        }
            //    }
            //    if (DataPage != "FamilyPage" && DataPage != "ProductPage")
            //    {
            //        DataRow dr = AttributePro.Tables["TOTAL_PAGES"].NewRow();
            //        dr[0] = res.getPageCount();
            //        AttributePro.Tables["TOTAL_PAGES"].Rows.Add(dr);

            //        DataRow dr1 = AttributePro.Tables["TOTAL_PRODUCTS"].NewRow();
            //        dr1[0] = res.getTotalItems();
            //        AttributePro.Tables["TOTAL_PRODUCTS"].Rows.Add(dr1);
            //    }

            //    /* if (DataPage == "ByBrand")
            //     {
            //         IList<INavigateCategory> Category = res.getDetailedCategories();
            //         if (Category.Count > 0)
            //         {

            //             foreach (INavigateCategory categoryItem in Category)
            //             {
            //                 DataRow row = CategoryGrouptb.NewRow();
            //                 IList<string> Id = categoryItem.getIDs();
            //                 row["CATEGORY_ID"] = Id[0].ToString().Substring(2);
            //                 row["CATEGORY_NAME"] = categoryItem.getName();
                           
            //                 int subCatCount = 0;
            //                 IList<INavigateCategory> SubCat_List = categoryItem.getSubCategories();
            //                 foreach (INavigateCategory item1 in SubCat_List)
            //                 {

            //                     subCatCount = subCatCount + 1;
            //                 }
            //                 row["SUB_CATEGORY_COUNT"] = subCatCount.ToString();
            //                 CategoryGrouptb.Rows.Add(row);
                            
            //             }
            //         }
            //     }*/


            //    //   string strHostName = System.Net.Dns.GetHostName();
            //    //string clientIPAddress="";
            //    //if (System.Net.Dns.GetHostAddresses(strHostName) != null)
            //    //{
            //    //    if (System.Net.Dns.GetHostAddresses(strHostName).Length <= 1)
            //    //        clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            //    //    else
            //    //        clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(1).ToString();
            //    //}



            //    objErrorhandler.CreateLogEA(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString() + "," + HttpContext.Current.Request.UserAgent + "," + res.getCatPath() + "," + ((HttpContext.Current.Session["USER_ID"] != null) ? HttpContext.Current.Session["USER_ID"].ToString() : "0"));
            //    //Added by Indu

            //    if (DataPage == "FamilyPage")
            //        Get_FamilyPage_Product_Values(DataPage, AttributeFam, AttributeFamPro, res, null, null);
            //    else if (DataPage == "ProductPage")
            //        Get_ProductPage_Product_Values(DataPage, AttributeFam, AttributeFamPro, res, null, null);
            //    else if (DataPage == "ByBrand")
            //        //Get_BrandModel_Product_Values(DataPage, AttributeFamPro, res);                        
            //        Get_Family_Product_Values(DataPage, AttributeFamPro, res, null, null);
            //    else
            //        Get_Family_Product_Values(DataPage, AttributeFamPro, res, null, null);


            //    /* if (DataPage == "ByBrand")
            //     {
            //         if (CategoryGrouptb.Rows.Count == 0)
            //         {
            //             AttributePro.Tables.RemoveAt(3);
            //             CategoryGrouptb = AttributeFamPro.DefaultView.ToTable(true, "CATEGORY_ID", "CATEGORY_NAME").Copy();
            //             CategoryGrouptb.TableName = "Category";
            //             AttributePro.Tables.Add(CategoryGrouptb);  
            //         }
            //     }
            //     */
            //    IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
            //    if (AttributeType.ToLower() == "brand")
            //        HttpContext.Current.Session["LHSAttributes"] = GetCategoryAttribute(Attributes, res, AttributeValue, SearchStr);
            //    else
            //        HttpContext.Current.Session["LHSAttributes"] = GetCategoryAttribute(Attributes, res, "", SearchStr);

            //    if (DataPage == "ProductList" || DataPage == "ps")
            //    {
            //        AttributeFam = AttributeFamPro.DefaultView.ToTable(true, "Family_Id", "Family_Name", "FAMILY_PRODUCT_COUNT", "SUB_FAMILY_COUNT", "PRODUCT_COUNT").Copy();
            //        AttributeFam.TableName = "Family";
            //        AttributePro.Tables.Add(AttributeFam);
            //    }
            //    HttpContext.Current.Session["FamilyProduct"] = AttributePro;

            //    return AttributePro;
            //}
            //catch (Exception ex)
            //{
            //    objErrorhandler.ErrorMsg = ex;
            //    //objErrorhandler.CreateLog(ex.ToString());
            //    objErrorhandler.CreateLog();
            //    return null;
            //}
            //finally
            //{
            //    AttributePro = null;
            //    AttributeFamPro = null;
            //    AttributeFam = null;
            //}
            return GetAttributeProductsJson(DataPage, SearchStr, AttributeType, AttributeValue, Brand, resultPerPage, CurrentPageNo, NextPage, EA);
        }

        public DataSet GetAttributeProductsJson(string DataPage, string SearchStr, string AttributeType, string AttributeValue, string Brand, string resultPerPage, string CurrentPageNo, string NextPage, string EA)
        {
            IRemoteEasyAsk ea = null;
            IOptions opts = null;
            INavigateResults res = null;
            try
            {

                if (EA == "" && HttpContext.Current.Session["EA"] == null)
                {
                   // objErrorhandler.CreateLog_new(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString() + "," + HttpContext.Current.Request.UserAgent + "," + ((HttpContext.Current.Session["USER_ID"] != null) ? HttpContext.Current.Session["USER_ID"].ToString() : "0"));
                //    objErrorhandler.CreateLog_new("EA Null " + HttpContext.Current.Request.RawUrl + "------- " );    
                    HttpContext.Current.Response.Redirect("/home.aspx");
                }

                string temp;
                string temp1;
            ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
              opts = ea.getOptions();
                opts.setResultsPerPage(resultPerPage); // ea_rpp.Value);   // use current settings

                opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
                opts.setSubCategories(true);
                opts.setReturnSKUs(false);
                opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);//"Category;1000"
                opts.setNavigateHierarchy(false);
                opts.setResultsPerPage(resultPerPage);

                if (DataPage == "FamilyPage")//|| DataPage == "ProductPage"
                {
                    opts.setSubCategories(false);
                    opts.setReturnSKUs(true);
                    opts.setResultsPerPage(24);
                   // CurrentPageNo = "0";
                    NextPage = "Next";
                }
                if (DataPage == "ProductPage")
                {
                    opts.setSubCategories(false);
                    opts.setReturnSKUs(true);
                    opts.setResultsPerPage(0);
                }
                if (DataPage == "ByBrand")
                {
                    //opts.setGrouping("Category;" + resultPerPage.ToString());
                    //opts.setGrouping("Category;" + "3000"); 
                    opts.setReturnSKUs(false);
                    //opts.setResultsPerPage(0);
                }
                if (DataPage == "ProductList" || DataPage == "CategoryProductList" || DataPage == "ByBrand" )
                {
                    if (HttpContext.Current.Session["SortOrder"] != null && HttpContext.Current.Session["SortOrder"] != "")
                    {
                    
                    if (HttpContext.Current.Session["SortOrder"] != null && HttpContext.Current.Session["SortOrder"]=="latest")
                        opts.setSortOrder("prod id,False");
                    else if (HttpContext.Current.Session["SortOrder"] != null && HttpContext.Current.Session["SortOrder"]=="ltoh")
                        opts.setSortOrder("price,True");
                    else if (HttpContext.Current.Session["SortOrder"] != null && HttpContext.Current.Session["SortOrder"] == "htol")
                        opts.setSortOrder("price,False");
                    }
                    else                        
                        opts.setSortOrder("prod id,False");
                }



                int PriceCode = GetPriceCode();

                if (PriceCode != -1)
                    opts.setCallOutParam("&eap_PriceCode=" + PriceCode.ToString());
                else if (PriceCode == -1)
                    opts.setCallOutParam("&eap_PriceCode=" + Dum_Price_Code.ToString());



                if (DataPage == "FamilyPage")
                {
                    AttributePro.Tables.Add(AttributeFam);
                    AttributeFam.Columns.Add("FAMILY_ID", typeof(string));
                    AttributeFam.Columns.Add("FAMILY_NAME", typeof(string));
                    AttributeFam.Columns.Add("STRING_VALUE", typeof(string));
                    AttributeFam.Columns.Add("NUMERIC_VALUE", typeof(string));
                    AttributeFam.Columns.Add("ATTRIBUTE_ID", typeof(Int32));
                    AttributeFam.Columns.Add("ATTRIBUTE_NAME", typeof(string));
                    AttributeFam.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
                    AttributeFam.Columns.Add("OBJECT_TYPE", typeof(string));
                    AttributeFam.Columns.Add("OBJECT_NAME", typeof(string));
                    AttributeFam.Columns.Add("ATTRIBUTE_DATA_TYPE", typeof(string));
                    AttributeFam.Columns.Add("FAMILY_PROD_COUNT", typeof(string));
                    AttributeFam.Columns.Add("PROD_COUNT", typeof(string));
                    AttributeFam.Columns.Add("STATUS", typeof(string));

                    //AttributePro.Tables.Add(AttributeFamPro);
                    //AttributeFamPro.Columns.Add("FAMILY_ID", typeof(string));
                    //AttributeFamPro.Columns.Add("FAMILY_NAME", typeof(string));
                    //AttributeFamPro.Columns.Add("FAMILY_TH_IMAGE", typeof(string));
                    //AttributeFamPro.Columns.Add("FAMILY_SHORT_DESC", typeof(string));
                    //AttributeFamPro.Columns.Add("FAMILY_DESC", typeof(string));
                    //AttributeFamPro.Columns.Add("PRODUCT_ID", typeof(string));
                    //AttributeFamPro.Columns.Add("PRODUCT_CODE", typeof(string));
                    //// AttributeFamPro.Columns.Add("PROD_CODE", typeof(string));//new 
                    //AttributeFamPro.Columns.Add("PRODUCT_PRICE", typeof(string));
                    //AttributeFamPro.Columns.Add("PRODUCT_DESC", typeof(string));
                    //AttributeFamPro.Columns.Add("PRODUCT_TH_IMAGE", typeof(string));
                    //AttributeFamPro.Columns.Add("SUB_FAMILY_COUNT", typeof(string));
                    //AttributeFamPro.Columns.Add("PRODUCT_COUNT", typeof(string));
                    //AttributeFamPro.Columns.Add("FAMILY_PRODUCT_COUNT", typeof(string));
                    //AttributeFamPro.Columns.Add("QTY_AVAIL", typeof(string));
                    //AttributeFamPro.Columns.Add("MIN_ORD_QTY", typeof(string));


                }
                else if (DataPage == "ProductPage")
                {
                    AttributePro.Tables.Add(AttributeFamPro);
                    AttributeFamPro.Columns.Add("PRODUCT_ID", typeof(string));
                    AttributeFamPro.Columns.Add("STRING_VALUE", typeof(string));
                    AttributeFamPro.Columns.Add("NUMERIC_VALUE", typeof(string));
                    AttributeFamPro.Columns.Add("ATTRIBUTE_ID", typeof(Int32));
                    AttributeFamPro.Columns.Add("ATTRIBUTE_NAME", typeof(string));
                    AttributeFamPro.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
                    AttributeFamPro.Columns.Add("OBJECT_TYPE", typeof(string));
                    AttributeFamPro.Columns.Add("OBJECT_NAME", typeof(string));
                    AttributeFamPro.Columns.Add("ATTRIBUTE_DATATYPE", typeof(string));
                    AttributeFamPro.Columns.Add("QTY_AVAIL", typeof(string));
                    AttributeFamPro.Columns.Add("MIN_ORD_QTY", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_ID", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_NAME", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_TH_IMAGE", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_SHORT_DESC", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_DESC", typeof(string));
                    AttributeFamPro.Columns.Add("STOCK_STATUS_DESC", typeof(string));
                    AttributeFamPro.Columns.Add("PROD_STOCK_STATUS", typeof(string));
                    AttributeFamPro.Columns.Add("PROD_STOCK_FLAG", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_PROD_COUNT", typeof(string));
                    AttributeFamPro.Columns.Add("PROD_COUNT", typeof(string));
                    AttributeFamPro.Columns.Add("ETA", typeof(string));
                    AttributeFamPro.Columns.Add("COST", typeof(string));
                    //AttributePro.Tables.Add(AttributeFam);
                    //AttributeFam.Columns.Add("FAMILY_ID", typeof(string));
                    //AttributeFam.Columns.Add("FAMILY_NAME", typeof(string));
                    //AttributeFam.Columns.Add("FAMILY_TH_IMAGE", typeof(string));
                    //AttributeFam.Columns.Add("FAMILY_SHORT_DESC", typeof(string));
                    //AttributeFam.Columns.Add("FAMILY_DESC", typeof(string));



                }
                else
                {
                    AttributePro.Tables.Add("TOTAL_PAGES");
                    AttributePro.Tables["TOTAL_PAGES"].Columns.Add("TOTAL_PAGES", typeof(int));

                    //For Total Products.
                    AttributePro.Tables.Add("TOTAL_PRODUCTS");
                    AttributePro.Tables["TOTAL_PRODUCTS"].Columns.Add("TOTAL_PRODUCTS", typeof(string));



                    //AttributePro.Tables.Add(AttributeFamPro);
                    //AttributeFamPro.Columns.Add("FAMILY_ID", typeof(string));
                    //AttributeFamPro.Columns.Add("CATEGORY_ID", typeof(string));
                    //AttributeFamPro.Columns.Add("PARENT_CATEGORY_ID", typeof(string));
                    //AttributeFamPro.Columns.Add("FAMILY_NAME", typeof(string));
                    //AttributeFamPro.Columns.Add("PRODUCT_ID", typeof(string));
                    //AttributeFamPro.Columns.Add("PRODUCT_CODE", typeof(string));
                    //AttributeFamPro.Columns.Add("PRODUCT_PRICE", typeof(string));
                    //AttributeFamPro.Columns.Add("ATTRIBUTE_ID", typeof(Int32));
                    //AttributeFamPro.Columns.Add("STRING_VALUE", typeof(string));
                    //AttributeFamPro.Columns.Add("NUMERIC_VALUE", typeof(string));
                    //AttributeFamPro.Columns.Add("OBJECT_TYPE", typeof(string));
                    //AttributeFamPro.Columns.Add("OBJECT_NAME", typeof(string));
                    //AttributeFamPro.Columns.Add("ATTRIBUTE_NAME", typeof(string));
                    //AttributeFamPro.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
                    //AttributeFamPro.Columns.Add("SUBCATNAME_L1", typeof(string));
                    //AttributeFamPro.Columns.Add("SUBCATNAME_L2", typeof(string));
                    //AttributeFamPro.Columns.Add("CATEGORY_NAME", typeof(string));
                    //AttributeFamPro.Columns.Add("PARENT_CATEGORY_NAME", typeof(string));
                    //AttributeFamPro.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
                    //AttributeFamPro.Columns.Add("SUB_FAMILY_COUNT", typeof(string));
                    //AttributeFamPro.Columns.Add("PRODUCT_COUNT", typeof(string));
                    //AttributeFamPro.Columns.Add("FAMILY_PRODUCT_COUNT", typeof(string));
                    //AttributeFamPro.Columns.Add("QTY_AVAIL", typeof(string));
                    //AttributeFamPro.Columns.Add("MIN_ORD_QTY", typeof(string));
                    //AttributeFamPro.Columns.Add("MIN_PRICE", typeof(string));
                    //AttributeFamPro.Columns.Add("STOCK_STATUS_DESC", typeof(string));
                    //AttributeFamPro.Columns.Add("SUB_FAMILY_ID", typeof(string));
                    //AttributeFamPro.Columns.Add("SNO", typeof(int));
                   
                  


                }


             

                if (EA == string.Empty)
                {
                    if (int.Parse(CurrentPageNo) <= 0)
                    {
                        if (SearchStr != string.Empty && AttributeValue == string.Empty && AttributeType == string.Empty)
                        {
                            if ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("mps.aspx")))
                            {
                                //if (HttpContext.Current.Session["SUPPLIER_NAME"] != null)
                                    res = ea.userSearch(HttpContext.Current.Session["EA"].ToString(), SearchStr);
                                ///else
                                //    res = ea.userSearch("AllProducts////WESAUSTRALASIA", SearchStr);
                            }
                            else
                                    res = ea.userSearch("AllProducts////WESAUSTRALASIA", SearchStr);
                        }
                        else
                        {
                            if (AttributeType.ToLower() == "category")
                            {

                                if (HttpContext.Current.Session["EA"] != null)
                                {


                                  
                                    if (HttpContext.Current.Session["EA"].ToString().ToLower().EndsWith("////" + AttributeValue.ToLower()))
                                        res = ea.userBreadCrumbClick(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()));
                                    else
                                        res = ea.userBreadCrumbClick(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString() + "////" + AttributeValue));

                                   
                                    //HttpContext.Current.Session["EA"] = res.getCatPath();
                                }

                            }
                            else if (AttributeType == "FamilyId")
                            {
                                if (HttpContext.Current.Session["EA"] != null)
                                {
                                    if (HttpContext.Current.Session["EA"].ToString().EndsWith("Family Id=" + AttributeValue))
                                    {
                                        HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace("Family Id=" + AttributeValue, "");
                                        res = ea.userSearch(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), "Family Id=" + AttributeValue);
                                    }
                                    else
                                        res = ea.userSearch(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), "Family Id=" + AttributeValue);
                                }

                            }
                            else if (AttributeType == "ProductId")
                            {
                                if (HttpContext.Current.Session["EA"] != null)
                                {
                                    if (HttpContext.Current.Session["EA"].ToString().EndsWith("Prod Id=" + AttributeValue))
                                    {
                                        HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace("Prod Id=" + AttributeValue, "");
                                        res = ea.userSearch(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), "Prod Id=" + AttributeValue);
                                    }
                                    else
                                        res = ea.userSearch(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), "Prod Id=" + AttributeValue);
                                }

                            }
                            else
                            {
                                if (AttributeType.ToLower() == "model")
                                {

                                    temp = "" + AttributeType + " = '" + HttpUtility.UrlEncode(Brand) + ":" + HttpUtility.UrlEncode(AttributeValue) + "'";
                                    temp1 = "" + AttributeType + " = ";
                                }
                                else
                                {
                                    temp = "" + AttributeType + " = '" + HttpUtility.UrlEncode(AttributeValue) + "'";
                                    temp1 = "" + AttributeType + " = ";
                                }
                                if (HttpContext.Current.Session["EA"] != null)
                                {
                                    if (HttpContext.Current.Session["EA"].ToString().Contains(temp1))
                                    {
                                        int t = HttpContext.Current.Session["EA"].ToString().IndexOf(temp1) - 17;
                                        string t1 = HttpContext.Current.Session["EA"].ToString().Remove(t);
                                        HttpContext.Current.Session["EA"] = t1;

                                    }

                                    res = ea.userAttributeClick(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), temp);
                                    //HttpContext.Current.Session["EA"] = res.getCatPath();

                                }
                            }
                        }

                    }
                    else
                    {
                        if (HttpContext.Current.Session["EA"] != null)
                        {
                            res = ea.userPageOp(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), CurrentPageNo, NextPage);
                        }
                    }
                    if (DataPage == "ps")
                    {
                        updateSearchSpell_Correctionjson(res, SearchStr);
                    }

                    HttpContext.Current.Session["EA_URL"] = null;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
                  // objErrorhandler.CreateLog_new("Easyask " +AttributeValue +" "+HttpContext.Current.Request.RawUrl )   ;    

                    HttpContext.Current.Session["EA"] = res.getCatPathJson();
                    CreateYouHaveSelectAndBreadCrumb(true);
                }
                else
                {

                    if (int.Parse(CurrentPageNo) <= 0)
                    {
                        if (SearchStr != string.Empty && AttributeValue == string.Empty && AttributeType == string.Empty)
                        {
                            if  ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("mps.aspx")))
                            { // if (HttpContext.Current.Session["SUPPLIER_NAME"]!=null)
                                //res = ea.userSearch("AllProducts////WESAUSTRALASIA////" + HttpContext.Current.Session["SUPPLIER_NAME"].ToString(), SearchStr);
                                res = ea.userSearch(HttpContext.Current.Session["EA"].ToString(), SearchStr);
                            //else
                              //  res = ea.userSearch("AllProducts////WESAUSTRALASIA", SearchStr);
                            }                             
                            else
                                res = ea.userSearch("AllProducts////WESAUSTRALASIA", SearchStr);
                        }
                        else
                        {
                            if (AttributeType.ToLower() == "category")
                            {
                                if (EA != null)
                                {
                                    if (EA.ToString().ToLower().EndsWith("////" + AttributeValue.ToLower()))
                                        res = ea.userBreadCrumbClick(HttpUtility.UrlEncode(EA.ToString()));
                                    else
                                        res = ea.userBreadCrumbClick(HttpUtility.UrlEncode(EA.ToString() + "////" + AttributeValue));
                                    //HttpContext.Current.Session["EA"] = res.getCatPath();
                                }

                            }
                            else if (AttributeType == "FamilyId")
                            {
                                if (EA != null)
                                {
                                    if (EA.ToString().EndsWith("Family Id=" + AttributeValue))
                                    {
                                        EA = EA.ToString().Replace("Family Id=" + AttributeValue, "");
                                        res = ea.userSearch(HttpUtility.UrlEncode(EA.ToString()), "Family Id=" + AttributeValue);
                                    }
                                    else
                                        res = ea.userSearch(HttpUtility.UrlEncode(EA.ToString()), "Family Id=" + AttributeValue);
                                }

                            }
                            else if (AttributeType == "ProductId")
                            {
                                if (EA != null)
                                {
                                    if (EA.ToString().EndsWith("Prod Id=" + AttributeValue))
                                    {
                                        EA = EA.ToString().Replace("Prod Id=" + AttributeValue, "");
                                        res = ea.userSearch(HttpUtility.UrlEncode(EA.ToString()), "Prod Id=" + AttributeValue);
                                    }
                                    else
                                        res = ea.userSearch(HttpUtility.UrlEncode(EA.ToString()), "Prod Id=" + AttributeValue);
                                }

                            }
                            else
                            {
                                if (AttributeType.ToLower() == "model")
                                {

                                    temp = "" + AttributeType + " = '" + HttpUtility.UrlEncode(Brand) + ":" + HttpUtility.UrlEncode(AttributeValue) + "'";
                                    temp1 = "" + AttributeType + " = ";
                                }
                                else
                                {
                                    temp = "" + AttributeType + " = '" + HttpUtility.UrlEncode(AttributeValue) + "'";
                                    temp1 = "" + AttributeType + " = ";
                                }
                                if (EA != null)
                                {
                                    if (EA.ToString().Contains(temp1))
                                    {
                                        int t = EA.ToString().IndexOf(temp1) - 17;
                                        string t1 = EA.ToString().Remove(t);
                                        EA = t1;

                                    }

                                    res = ea.userAttributeClick(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), temp);
                                    //HttpContext.Current.Session["EA"] = res.getCatPath();

                                }
                            }
                        }

                    }
                    else
                    {
                        //Modified by Indu for dynamic pagination session prb
                        //if (HttpContext.Current.Session["EA"] != null)
                        //{
                        //    res = ea.userPageOp(HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString()), CurrentPageNo, NextPage);
                        //}
                        if (EA != null)
                        {
                            res = ea.userPageOp(HttpUtility.UrlEncode(EA), CurrentPageNo, NextPage);
                        }
                    }
                    if (DataPage.ToLower() == "ps")
                    {
                        updateSearchSpell_Correctionjson(res, SearchStr);
                    }
                    EA = res.getCatPathJson();



                    AttributePro.Tables.Add("eapath");
                    AttributePro.Tables["eapath"].Columns.Add("EA");
                    DataRow drEA = AttributePro.Tables["eapath"].NewRow();
                    drEA[0] = EA;

                    //FormName = FormName.Replace(".ASPX", ""); 
                    //dr["FormName"] = FormName;
                    AttributePro.Tables["eapath"].Rows.Add(drEA);

                    CreateYouHaveSelectAndBreadCrumb(EA,true);






                }
                if (DataPage.ToLower() != "ps")
                {

                 
                    int last = res.getLastItemJson();

                    if (last < 0)
                    {
                      //  objErrorhandler.CreateLog_new("Into pw  " + HttpContext.Current.Session["EA"].ToString() + "------- ");   
                        HelperServices objHelperServices = new HelperServices();
                        string atrvalue = string.Empty;
                        if (DataPage == "FamilyPage")
                        {
                            object ds = objhelperDb.GetGenericDataDB(AttributeValue.ToString(), "GET_FAMILY_NAME", HelperDB.ReturnType.RTString);
                            if (ds != null)
                            {
                                atrvalue = ds.ToString();
                            }
                            else
                            {
                                atrvalue = AttributeValue.ToString();
                            }
                            if (atrvalue == "")
                            {
                                atrvalue = AttributeValue.ToString();
                            }
                        }
                        else if (DataPage == "ProductPage")
                        {
                            ProductServices objProductServices = new ProductServices();
                            atrvalue = objProductServices.GetProductCode(Convert.ToInt32(AttributeValue));
                            string[] attrval = atrvalue.Split('-');
                            atrvalue = attrval[0];
                        }
                        else
                        {
                            atrvalue = AttributeValue;
                        }

                     string url=   HttpContext.Current.Request.RawUrl.ToString();
                           string[] fstring = url.Split('/');
                        if (atrvalue == "")
                        {

                            if (url.Contains("/pd/") == true)
                            {
                               
                                string firststring = fstring[fstring.Length - 4];
                                string[] pid = firststring.Split('-');
                              
                                    string _pid = pid[pid.Length - 1];
                                    firststring = firststring.Replace("-"+_pid, "");

                                    atrvalue = firststring;
                               
                             
                            }
                            else
                            {
                              
                                string firststring = fstring[1];
                                atrvalue = firststring;
                            }
                        }
                        if (atrvalue != "") 
                        {


                            string strvalue = objHelperServices.SimpleURL_Str( atrvalue, "ps.aspx",false);

                            if(url.Contains("/mct/")|| url.Contains("/mpl/")||url.Contains("mfl")||url.Contains("mpd"))
                            {
                            strvalue = "/" + fstring[1]+"/"+ strvalue + "/mps/";
                            }
                            else
                            {
                                strvalue = "/" +strvalue + "/ps/";
                            
                            }

                      
                            if (strvalue.Contains("-"))
                            {
                                HttpContext.Current.Session["CurrSearch"] = atrvalue;
                            }
                            HttpContext.Current.Response.RedirectPermanent(strvalue,false);
                        }
                    }
                }
                DataSet dsadv = res.GetDBAdvisor();


                int pagecount = 0;
                int totalitemcount = 0;


              
                if (DataPage != "FamilyPage" && DataPage != "ProductPage")
                {
                    if (dsadv != null && dsadv.Tables.Count > 0 && dsadv.Tables["itemdescription"] != null && dsadv.Tables["itemdescription"].Rows.Count > 0)
                    {

                        pagecount = Convert.ToInt32(dsadv.Tables["itemdescription"].Rows[dsadv.Tables["itemdescription"].Rows.Count - 1]["pagecount"].ToString());
                        totalitemcount = Convert.ToInt32(dsadv.Tables["itemdescription"].Rows[dsadv.Tables["itemdescription"].Rows.Count - 1]["totalitems"].ToString());
                    }

                    DataRow dr = AttributePro.Tables["TOTAL_PAGES"].NewRow();
                    dr[0] = pagecount;//res.getPageCountJson();
                    AttributePro.Tables["TOTAL_PAGES"].Rows.Add(dr);

                    DataRow dr1 = AttributePro.Tables["TOTAL_PRODUCTS"].NewRow();
                    dr1[0] = totalitemcount;// res.getTotalItemsJson();
                    AttributePro.Tables["TOTAL_PRODUCTS"].Rows.Add(dr1);
                }

              
        objErrorhandler.CreateLogEA(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString() + "," + HttpContext.Current.Request.UserAgent + "," + res.getCatPathJson() + "," + ((HttpContext.Current.Session["USER_ID"] != null) ? HttpContext.Current.Session["USER_ID"].ToString() : "0"));
                //Added by Indu
                if (DataPage == "FamilyPage")
                {
                    if (dsadv.Tables["itemdescription"] != null)
                    {
                        HttpContext.Current.Session[AttributeValue + "Icnt"] = dsadv.Tables["itemdescription"].Rows[dsadv.Tables["itemdescription"].Rows.Count - 1]["pagecount"].ToString();
                    }
                }
                if (DataPage == "FamilyPage")
                    Get_FamilyPage_Product_ValuesJson(DataPage, AttributeFam, AttributeFamPro, res, null, null, dsadv);
                else if (DataPage == "ProductPage")
                    Get_ProductPage_Product_ValuesJson(DataPage, AttributeFam, AttributeFamPro, res, null, null, dsadv);
                else if (DataPage == "ByBrand")
                    //Get_BrandModel_Product_Values(DataPage, AttributeFamPro, res);                        
                    Get_Family_Product_ValuesJson(DataPage, AttributeFamPro, res, null, null, dsadv);
                else
                    Get_Family_Product_ValuesJson(DataPage, AttributeFamPro, res, null, null, dsadv);


                /* if (DataPage == "ByBrand")
                 {
                     if (CategoryGrouptb.Rows.Count == 0)
                     {
                         AttributePro.Tables.RemoveAt(3);
                         CategoryGrouptb = AttributeFamPro.DefaultView.ToTable(true, "CATEGORY_ID", "CATEGORY_NAME").Copy();
                         CategoryGrouptb.TableName = "Category";
                         AttributePro.Tables.Add(CategoryGrouptb);  
                     }
                 }
                 */
                //IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
                if (!(HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx")) && !(HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx")))
                {
                    if (AttributeType.ToLower() == "brand")
                        HttpContext.Current.Session["LHSAttributes"] = GetCategoryAttributeJson(res, AttributeValue, SearchStr, dsadv);
                    else
                        HttpContext.Current.Session["LHSAttributes"] = GetCategoryAttributeJson(res, "", SearchStr, dsadv);
                }

                if ((DataPage == "ProductList" || DataPage == "ps") && AttributePro.Tables["FamilyPro"] != null && AttributePro.Tables["FamilyPro"].Rows.Count>0  )
                {
                    AttributeFam = AttributePro.Tables["FamilyPro"].DefaultView.ToTable(true, "Family_Id", "Family_Name", "FAMILY_PRODUCT_COUNT", "SUB_FAMILY_COUNT", "PRODUCT_COUNT").Copy();
                    AttributeFam.TableName = "Family";
                    AttributePro.Tables.Add(AttributeFam);
                }
                HttpContext.Current.Session["FamilyProduct"] = AttributePro;

                return AttributePro;
            }
            catch (System.Threading.ThreadAbortException)
            {
                return null;
                // ignore it
            }
            catch (Exception ex)
            {
                //objErrorhandler.CreateLog_new(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString() + "," + HttpContext.Current.Request.UserAgent +  "," + ((HttpContext.Current.Session["USER_ID"] != null) ? HttpContext.Current.Session["USER_ID"].ToString() : "0"));
               // objErrorhandler.CreateLog_new("Easyask " + HttpContext.Current.Request.RawUrl + "------- " + ex.ToString());    
                objErrorhandler.ErrorMsg = ex;
                //objErrorhandler.CreateLog(ex.ToString());
                objErrorhandler.CreateLog();
                return null;
            }
            finally
            {
                AttributePro = null;
                AttributeFamPro = null;
                AttributeFam = null;
                ea = null;
                opts = null;
                res = null;
            }
        }

        
        DataSet psDs = new DataSet();
        DataTable PSProsearch = new DataTable("SearchTxt");
        DataTable PSFamPro = new DataTable("FamilyPro");

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE POWER SEARCH PRODUCT DETAILS IN DROP DOWN ***/
        /********************************************************************************/
        public DataSet GetDropDownPowerSearchProducts(string SearchStr, string resultPerPage)
        {
            try
            {

               // string temp;
               // string temp1;
                IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
                IOptions opts = ea.getOptions();
                opts.setResultsPerPage(resultPerPage); // ea_rpp.Value);   // use current settings

                opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
                opts.setSubCategories(false);
                opts.setReturnSKUs(false);
                opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);//"Category;1000"
                opts.setNavigateHierarchy(false);
                opts.setResultsPerPage(resultPerPage);

               



                int PriceCode = GetPriceCode();
                if (PriceCode != -1)
                    opts.setCallOutParam("&eap_PriceCode=" + PriceCode.ToString());




                psDs.Tables.Add(PSProsearch);
                PSProsearch.Columns.Add("Code", typeof(string));

                psDs.Tables.Add(PSFamPro);
                    PSFamPro.Columns.Add("FAMILY_ID", typeof(string));
                    PSFamPro.Columns.Add("FAMILY_NAME", typeof(string));
                    PSFamPro.Columns.Add("SHortDesc", typeof(string));
                    PSFamPro.Columns.Add("FamilyTH", typeof(string));
                    PSFamPro.Columns.Add("FamilyDesc", typeof(string));
                    PSFamPro.Columns.Add("ProdCode", typeof(string));
                    PSFamPro.Columns.Add("cost", typeof(string));
                    PSFamPro.Columns.Add("ProdTh", typeof(string));
                    PSFamPro.Columns.Add("PRODUCT_ID", typeof(string));
                    PSFamPro.Columns.Add("CATEGORY_ID", typeof(string));
                    PSFamPro.Columns.Add("CATEGORY_PATH", typeof(string));

                   


               

                INavigateResults res = null;


                
                    if (SearchStr != "")
                    {
                        res = ea.userSearch("AllProducts////WESAUSTRALASIA", SearchStr);
                    }

                
               
                //if (DataPage == "ps")
                //{
                //    updateSearchSpell_Correction(res);
                //}







                HttpContext.Current.Session["EASearch"] = res.getCatPath();

                Get_DropDown_Power_Search_Product_Values(PSFamPro, res, null, null);


                IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
                
                    HttpContext.Current.Session["SearchAttributes"] = GetCategoryAttribute(Attributes, res, "", SearchStr);



                HttpContext.Current.Session["FamilyProduct"] = psDs;



                return psDs;
            }
            catch (Exception ex)
            {
                objErrorhandler.ErrorMsg = ex;
                objErrorhandler.CreateLog();
                return null;
            }
            finally
            {
                psDs = null;
                PSFamPro = null;
               
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE BRAND DETAILS IN DATASET ***/
        /********************************************************************************/

        public void GetDataSet1(INavigateResults res)
        {
            try
            {
                IList<INavigateAttribute> Brand_list = res.getDetailedAttributeValues("Brand");
                foreach (INavigateAttribute item in Brand_list)
                {
                    DataRow row = tbl_Brand.NewRow();
                    row["TOSUITE_BRAND"] = item.getValue();
                    tbl_Brand.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
            }
            HttpContext.Current.Session["WESBrand"] = Brand;
        }

        //Store EasyAsk Values to Session.
        //public void Get_SessionData()
        //{
        //    GetDataSet();
        //    if (Category.Tables.Count > 0)
        //    {
        //        HttpContext.Current.Session["Category"] = Category;
        //    }
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CREATE COLUMNS FOR DATATABLE  ***/
        /********************************************************************************/

        void Create_DataTable_Columns()
        {
            try
            {
                //For Category menu
                CategoryMenu.Tables.Add(Category_Menu);
                Category_Menu.Columns.Add("TBT_PARENT_CATEGORY_ID", typeof(string));
                Category_Menu.Columns.Add("TBT_PARENT_CATEGORY_NAME", typeof(string));                
                Category_Menu.Columns.Add("CATEGORY_NAME", typeof(string));
                Category_Menu.Columns.Add("CATEGORY_ID", typeof(string));                
                Category_Menu.Columns.Add("EA_PATH", typeof(string));
                Category_Menu.Columns.Add("URL_RW_PATH", typeof(string));
                
                

                //For SubCategory Table.
                SubCategory.Tables.Add(Sub_Category);
                Sub_Category.Columns.Add("TBT_PARENT_CATEGORY_ID", typeof(string));
                Sub_Category.Columns.Add("TBT_PARENT_CATEGORY_NAME", typeof(string));
                Sub_Category.Columns.Add("TBT_PARENT_CATEGORY_IMAGE", typeof(string));
                Sub_Category.Columns.Add("CATEGORY_NAME", typeof(string));
                Sub_Category.Columns.Add("CATEGORY_ID", typeof(string));
                Sub_Category.Columns.Add("TBT_SHORT_DESC", typeof(string));
                Sub_Category.Columns.Add("TBT_CUSTOM_NUM_FIELD3", typeof(string));
                Sub_Category.Columns.Add("EA_PATH", typeof(string));
                Sub_Category.Columns.Add("URL_RW_PATH", typeof(string));
                Sub_Category.Columns.Add("PART1", typeof(string));        
                
                //For Category Table.
                MainCategory.Tables.Add(Main_Category);
                Main_Category.Columns.Add("CATEGORY_NAME", typeof(string));
                Main_Category.Columns.Add("CATEGORY_ID", typeof(string));
                Main_Category.Columns.Add("PARENT_CATEGORY", typeof(string));
                Main_Category.Columns.Add("SHORT_DESC", typeof(string));
                Main_Category.Columns.Add("IMAGE_FILE", typeof(string));
                Main_Category.Columns.Add("IMAGE_FILE2", typeof(string));
                Main_Category.Columns.Add("IMAGE_NAME", typeof(string));
                Main_Category.Columns.Add("IMAGE_NAME2", typeof(string));
                Main_Category.Columns.Add("CUSTOM_TEXT_FIELD2", typeof(string));  
                Main_Category.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
                Main_Category.Columns.Add("EA_PATH", typeof(string));

                Main_Category.Columns.Add("URL_RW_PATH", typeof(string));
                DataColumn dc = new DataColumn("SUB_COUNT", typeof(int));

                dc.DefaultValue = 0;
                Main_Category.Columns.Add(dc);
                Main_Category.Columns.Add("CATEGORY_NAME_TOP", typeof(string));


                //For Brand Table.
                Brand.Tables.Add(tbl_Brand);
                tbl_Brand.Columns.Add("TOSUITE_BRAND", typeof(string));
            }
            catch (Exception ex)
            {
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE WES PRODUCT CATEGORY DETAILS ***/
        /********************************************************************************/

        public DataSet GetWESModel(string parentCategoryName, int CatalogID, string tosuite_brand)
        {

            //string tmpEaPath ="";
            //string tmpCatPath ="";
            // Boolean blnGetData=false;  
            //if(parentCategoryName!="")
            //{
            //    tmpCatPath ="////" +parentCategoryName;
            //}
            
            //if (HttpContext.Current.Session["WESBrand_Model"] == null)
            //{
            //    blnGetData = true; 
            //}
            //else
            //{
            //    if (((DataSet)HttpContext.Current.Session["WESBrand_Model"]).Tables["ParentCategory"].Rows[0]["CATNAME"].ToString().ToUpper() == parentCategoryName.ToUpper() && ((DataSet)HttpContext.Current.Session["WESBrand_Model"]).Tables["ParentCategory"].Rows[0]["BRANDNAME"].ToString().ToUpper() == tosuite_brand.ToUpper())
            //    {
            //        blnGetData = false;

            //    }
            //    else
            //    {
            //        blnGetData = true;
            //    }


            //}
            //tmpEaPath = "AllProducts////WESAUSTRALASIA" + tmpCatPath + "////AttribSelect=Brand='" + tosuite_brand + "'";
            //if (blnGetData == true)
            //{



            //    IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WesCatBrandDictionary);
            //    IOptions opts = ea.getOptions();
            //    opts.setResultsPerPage("0"); // ea_rpp.Value);   // use current settings
            //    opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
            //    opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
            //    opts.setSubCategories(false);
            //    opts.setNavigateHierarchy(false);
            //    opts.setReturnSKUs(false);
            //    Brand_Model = new DataSet();

            //    DataTable dTable = new DataTable("Model");
            //    DataTable dTable1 = new DataTable("ParentCategory");


            //    INavigateResults res = ea.userAttributeClick("AllProducts////WESAUSTRALASIA" + tmpCatPath, "Brand='" + tosuite_brand + "'");
            //    //updateBreadCrumb(res.getBreadCrumbTrail());
            //    HttpContext.Current.Session["Brand_Path"] = res.getCatPath();
            //    HttpContext.Current.Session["EA"] = res.getCatPath();
            //    CreateYouHaveSelectAndBreadCrumb();
            //    try
            //    {
            //        int last = res.getLastItem();
            //        int Model_Name = res.getColumnIndex("ModelValue");
            //        int Model_Image = res.getColumnIndex("Model Thumbnail");
            //        int Brand_Name = res.getColumnIndex("Brand");
            //        // IList Model = res.getDetailedAttributeValues("Model");
            //        Brand_Model.Tables.Add(dTable);
            //        dTable.Columns.Add("TOSUITE_MODEL", typeof(string));
            //        dTable.Columns.Add("IMAGE_FILE", typeof(string));
            //        dTable.Columns.Add("Brand", typeof(string));
            //        dTable.Columns.Add("EA_PATH", typeof(string));

            //        Brand_Model.Tables.Add(dTable1);
            //        dTable1.Columns.Add("CATNAME", typeof(string));
            //        dTable1.Columns.Add("BRANDNAME", typeof(string));


            //        DataRow row2 = dTable1.NewRow();
            //        row2["CATNAME"] = parentCategoryName.ToUpper();
            //        row2["BRANDNAME"] = tosuite_brand.ToUpper();

            //        dTable1.Rows.Add(row2);


            //        string image_string = "";
            //        DataRow dRow;
            //        for (int i = res.getFirstItem() - 1; i < last; i++)
            //        {
            //            dRow = dTable.NewRow();
            //            dRow["TOSUITE_MODEL"] = res.getCellData(i, Model_Name);
            //            dRow["EA_PATH"] = HttpContext.Current.Session["EA"].ToString() + "////AttribSelect=Model = '" + res.getCellData(i, Brand_Name).ToString() + ":" + res.getCellData(i, Model_Name).ToString() + "'";
            //            // dRow["IMAGE_FILE"] = 
            //            string Model_Image_Name = res.getCellData(i, Model_Image).ToString();
            //            if (Model_Image_Name != "" && Model_Image_Name != null)
            //            {
            //                image_string = Model_Image_Name.Substring(42);
            //            }
            //            else
            //            {
            //                image_string = "noimage.gif";
            //            }
            //            dRow["IMAGE_FILE"] = image_string;
            //            dRow["Brand"] = res.getCellData(i, Brand_Name);
            //            dTable.Rows.Add(dRow);
            //        }

            //        IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
            //        HttpContext.Current.Session["WESBrand_Model_Attributes"] = GetCategoryAttribute(Attributes, res, "", "");
                 
            //    }
                

            //    catch (Exception)
            //    {
            //        return null;
            //    }
            //    HttpContext.Current.Session["WESBrand_Model"] = Brand_Model;
            //    return Brand_Model;
            //}
            //else 
            //  //Modified By :Indu  To solve breadcrumb prb
            //   // if (HttpContext.Current.Session["EA"].ToString().ToUpper() == tmpEaPath.ToUpper())
            //{
            //    HttpContext.Current.Session["EA"] = tmpEaPath;
            //    CreateYouHaveSelectAndBreadCrumb();
            //}
            
            //return (DataSet) HttpContext.Current.Session["WESBrand_Model"] ;


            return GetWESModelJson(parentCategoryName, CatalogID, tosuite_brand);
        }
        
        public DataSet GetWESModelJson(string parentCategoryName, int CatalogID, string tosuite_brand)
        {
            IRemoteEasyAsk ea = null;
            IOptions opts = null;
            INavigateResults res = null;
            try
            {
                string tmpEaPath = string.Empty;
                string tmpCatPath = string.Empty;
                Boolean blnGetData = false;
                if (parentCategoryName != string.Empty)
                {
                    tmpCatPath = "////" + parentCategoryName;
                }

                if (HttpContext.Current.Session["WESBrand_Model"] == null)
                {
                    blnGetData = true;
                }
                else
                {
                    if (((DataSet)HttpContext.Current.Session["WESBrand_Model"]).Tables["ParentCategory"].Rows[0]["CATNAME"].ToString().ToUpper() == parentCategoryName.ToUpper() && ((DataSet)HttpContext.Current.Session["WESBrand_Model"]).Tables["ParentCategory"].Rows[0]["BRANDNAME"].ToString().ToUpper() == tosuite_brand.ToUpper())
                    {
                        blnGetData = false;

                    }
                    else
                    {
                        blnGetData = true;
                    }


                }
                tmpEaPath = "AllProducts////WESAUSTRALASIA" + tmpCatPath + "////AttribSelect=Brand='" + tosuite_brand + "'";
                if (blnGetData == true)
                {



                     ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WesCatBrandDictionary);
                     opts = ea.getOptions();
                    opts.setResultsPerPage("0"); // ea_rpp.Value);   // use current settings
                    opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
                    opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
                    opts.setSubCategories(false);
                    opts.setNavigateHierarchy(false);
                    opts.setReturnSKUs(false);
                    Brand_Model = new DataSet();

                    DataTable dTable = new DataTable("Model");
                    DataTable dTable1 = new DataTable("ParentCategory");


                     res = ea.userAttributeClick("AllProducts////WESAUSTRALASIA" + tmpCatPath, "Brand='" + tosuite_brand + "'");
                    //updateBreadCrumb(res.getBreadCrumbTrail());
                    HttpContext.Current.Session["Brand_Path"] = res.getCatPathJson();
                    HttpContext.Current.Session["EA"] = res.getCatPathJson();
                    CreateYouHaveSelectAndBreadCrumb(false);
                    try
                    {
                        int last = res.getLastItemJson();
                        int Model_Name = res.getColumnIndexJson("ModelValue");
                        int Model_Image = res.getColumnIndexJson("Model Thumbnail");
                        int Brand_Name = res.getColumnIndexJson("Brand");
                        // IList Model = res.getDetailedAttributeValues("Model");
                        Brand_Model.Tables.Add(dTable);
                        dTable.Columns.Add("TOSUITE_MODEL", typeof(string));
                        dTable.Columns.Add("IMAGE_FILE", typeof(string));
                        dTable.Columns.Add("Brand", typeof(string));
                        dTable.Columns.Add("EA_PATH", typeof(string));

                        Brand_Model.Tables.Add(dTable1);
                        dTable1.Columns.Add("CATNAME", typeof(string));
                        dTable1.Columns.Add("BRANDNAME", typeof(string));


                        DataRow row2 = dTable1.NewRow();
                        row2["CATNAME"] = parentCategoryName.ToUpper();
                        row2["BRANDNAME"] = tosuite_brand.ToUpper();

                        dTable1.Rows.Add(row2);


                        string image_string = string.Empty;
                        DataRow dRow;
                        for (int i = res.getFirstItemJson() - 1; i < last; i++)
                        {
                            dRow = dTable.NewRow();
                            dRow["TOSUITE_MODEL"] = res.getCellDataJson(i, Model_Name);
                            dRow["EA_PATH"] = HttpContext.Current.Session["EA"].ToString() + "////AttribSelect=Model = '" + res.getCellDataJson(i, Brand_Name).ToString() + ":" + res.getCellDataJson(i, Model_Name).ToString() + "'";
                            // dRow["IMAGE_FILE"] = 
                            string Model_Image_Name = res.getCellDataJson(i, Model_Image).ToString();
                            if (Model_Image_Name != "" && Model_Image_Name != null)
                            {
                                image_string = Model_Image_Name.Substring(42);
                            }
                            else
                            {
                                image_string = "noimage.gif";
                            }
                            dRow["IMAGE_FILE"] = image_string;
                            dRow["Brand"] = res.getCellDataJson(i, Brand_Name);
                            dTable.Rows.Add(dRow);
                        }

                        //IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
                        HttpContext.Current.Session["WESBrand_Model_Attributes"] = GetCategoryAttributeJson(res, "", "", res.GetDBAdvisor());

                    }


                    catch (Exception)
                    {
                        return null;
                    }

                    HttpContext.Current.Session["WESBrand_Model"] = Brand_Model;
                    return Brand_Model;
                }
                else
                //Modified By :Indu  To solve breadcrumb prb
                // if (HttpContext.Current.Session["EA"].ToString().ToUpper() == tmpEaPath.ToUpper())
                {
                    HttpContext.Current.Session["EA"] = tmpEaPath;
                    CreateYouHaveSelectAndBreadCrumb(true);
                }

                return (DataSet)HttpContext.Current.Session["WESBrand_Model"];

            }
            catch
            {
                return (DataSet)HttpContext.Current.Session["WESBrand_Model"];

            }
            finally
            {
                 ea = null;
                 opts = null;
                 res = null;
            }

        }



        public DataSet getwesmodeltoxml(string parentCategoryName, int CatalogID, string tosuite_brand)
        {
            string tmpEaPath = string.Empty;
            string tmpCatPath = string.Empty;
            Boolean blnGetData = false;
            if (parentCategoryName != string.Empty)
            {
                tmpCatPath = "////" + parentCategoryName;
            }

            if (HttpContext.Current.Session["WESBrand_Model"] == null)
            {
                blnGetData = true;
            }
            else
            {
                if (((DataSet)HttpContext.Current.Session["WESBrand_Model"]).Tables["ParentCategory"].Rows[0]["CATNAME"].ToString().ToUpper() == parentCategoryName.ToUpper() && ((DataSet)HttpContext.Current.Session["WESBrand_Model"]).Tables["ParentCategory"].Rows[0]["BRANDNAME"].ToString().ToUpper() == tosuite_brand.ToUpper())
                {
                    blnGetData = false;

                }
                else
                {
                    blnGetData = true;
                }


            }
            tmpEaPath = "AllProducts////WESAUSTRALASIA" + tmpCatPath + "////AttribSelect=Brand='" + tosuite_brand + "'";
            if (blnGetData == true)
            {



                IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WesCatBrandDictionary);
                IOptions opts = ea.getOptions();
                opts.setResultsPerPage("0"); // ea_rpp.Value);   // use current settings
                opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
                opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
                opts.setSubCategories(false);
                opts.setNavigateHierarchy(false);
                opts.setReturnSKUs(false);
                Brand_Model = new DataSet();

                DataTable dTable = new DataTable("Model");
                DataTable dTable1 = new DataTable("ParentCategory");


                INavigateResults res = ea.userAttributeClick("AllProducts////WESAUSTRALASIA" + tmpCatPath, "Brand='" + tosuite_brand + "'");
                //updateBreadCrumb(res.getBreadCrumbTrail());
                HttpContext.Current.Session["Brand_Path"] = res.getCatPath();
                HttpContext.Current.Session["EA"] = res.getCatPath();
                CreateYouHaveSelectAndBreadCrumb(false);
                try
                {
                    int last = res.getLastItem();
                    int Model_Name = res.getColumnIndex("ModelValue");
                    int Model_Image = res.getColumnIndex("Model Thumbnail");
                    int Brand_Name = res.getColumnIndex("Brand");
                    // IList Model = res.getDetailedAttributeValues("Model");
                    Brand_Model.Tables.Add(dTable);
                    dTable.Columns.Add("TOSUITE_MODEL", typeof(string));
                    dTable.Columns.Add("IMAGE_FILE", typeof(string));
                    dTable.Columns.Add("Brand", typeof(string));
                    dTable.Columns.Add("EA_PATH", typeof(string));

                    Brand_Model.Tables.Add(dTable1);
                    dTable1.Columns.Add("CATNAME", typeof(string));
                    dTable1.Columns.Add("BRANDNAME", typeof(string));


                    DataRow row2 = dTable1.NewRow();
                    row2["CATNAME"] = parentCategoryName.ToUpper();
                    row2["BRANDNAME"] = tosuite_brand.ToUpper();

                    dTable1.Rows.Add(row2);


                    string image_string = string.Empty;
                    DataRow dRow;
                    for (int i = res.getFirstItem() - 1; i < last; i++)
                    {
                        dRow = dTable.NewRow();
                        dRow["TOSUITE_MODEL"] = res.getCellData(i, Model_Name);
                        dRow["EA_PATH"] = HttpContext.Current.Session["EA"].ToString() + "////AttribSelect=Model = '" + res.getCellData(i, Brand_Name).ToString() + ":" + res.getCellData(i, Model_Name).ToString() + "'";
                        // dRow["IMAGE_FILE"] = 
                        string Model_Image_Name = res.getCellData(i, Model_Image).ToString();
                        if (Model_Image_Name != "" && Model_Image_Name != null)
                        {
                            image_string = Model_Image_Name.Substring(42);
                        }
                        else
                        {
                            image_string = "noimage.gif";
                        }
                        dRow["IMAGE_FILE"] = image_string;
                        dRow["Brand"] = res.getCellData(i, Brand_Name);
                        dTable.Rows.Add(dRow);
                    }

                    IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
                    HttpContext.Current.Session["WESBrand_Model_Attributes"] = GetCategoryAttribute(Attributes, res, "", "");

                }


                catch (Exception)
                {
                    return null;
                }
                HttpContext.Current.Session["WESBrand_Model_xml"] = Brand_Model;
                return Brand_Model;
            }
            else
            //Modified By :Indu  To solve breadcrumb prb
            // if (HttpContext.Current.Session["EA"].ToString().ToUpper() == tmpEaPath.ToUpper())
            {
                HttpContext.Current.Session["EA"] = tmpEaPath;
                CreateYouHaveSelectAndBreadCrumb(false);
            }

            return (DataSet)HttpContext.Current.Session["WESBrand_Model"];
        }
        /*********************************** OLD CODE ***********************************/

        //--------------------------------------------------------------------------------

        //public DataSet getCategory_List(string _catId)
        //{
        //    DataSet ds = new DataSet();
        //    DataTable Category_List = new DataTable();
        //    Category_List.Columns.Add("category_id", typeof(string));
        //    Category_List.Columns.Add("category_name", typeof(string));
        //    Category_List.Columns.Add("image_file", typeof(string));
        //    Category_List.Columns.Add("custom_num_field3", typeof(string));
        //    Category_List.Columns.Add("parent_category", typeof(string));
        //    ds.Tables.Add(Category_List);

        //    if (HttpContext.Current.Session["Category"] != null)
        //    {
        //        DataSet tempdata = (DataSet)HttpContext.Current.Session["Category"];
        //        foreach (DataRow dr in tempdata.Tables[0].Rows)
        //        {
        //            if (dr.ItemArray.GetValue(0).ToString() == _catId)
        //            {
        //                DataRow row = Category_List.NewRow();
        //                row["category_id"] = dr.ItemArray.GetValue(4).ToString();
        //                row["category_name"] = dr.ItemArray.GetValue(3).ToString();
        //                row["image_file"] = string.Empty;
        //                row["custom_num_field3"] = "2";
        //                row["parent_category"] = dr.ItemArray.GetValue(0).ToString();
        //                Category_List.Rows.Add(row);
        //            }
        //        }
        //    }
        //    return ds;
        //}

        //public DataSet Category_Menu_Click_BreadCrumbs(string _catId)
        //{
        //    DataSet ds = new DataSet();
        //    DataTable Bread_Crumbs = new DataTable();
        //    Bread_Crumbs.Columns.Add("CATEGORY_NAME", typeof(string));
        //    Bread_Crumbs.Columns.Add("PARENT_CATEGORY", typeof(string));
        //    Bread_Crumbs.Columns.Add("CATEGORY_ID", typeof(string));
        //    ds.Tables.Add(Bread_Crumbs);

        //    if (HttpContext.Current.Session["Category"] != null)
        //    {
        //        DataSet tempdata = (DataSet)HttpContext.Current.Session["Category"];
        //        foreach (DataRow dr in tempdata.Tables[1].Rows)
        //        {
        //            if (dr.ItemArray.GetValue(1).ToString() == _catId)
        //            {
        //                DataRow row = Bread_Crumbs.NewRow();
        //                row["CATEGORY_ID"] = dr.ItemArray.GetValue(1).ToString();
        //                row["PARENT_CATEGORY"] = "WES0830";
        //                row["CATEGORY_NAME"] = dr.ItemArray.GetValue(0).ToString();
        //                Bread_Crumbs.Rows.Add(row);
        //            }
        //        }
        //    }
        //    return ds;
        //}

        #endregion

        //#region "For the homepage search"

        //DataSet HomeSearch = new DataSet();
        //DataTable Table = new DataTable();//
        //DataTable Table1 = new DataTable();//Total Count
        //DataTable Table2 = new DataTable();
        //DataTable Table3 = new DataTable();
        //ErrorHandler objErrorhandler = new ErrorHandler();

        //void Create_Search_Table_Columns()
        //{
        //    //For Total Pages.
        //    HomeSearch.Tables.Add(Table);
        //    Table.Columns.Add("TOTAL_PAGES", typeof(int));

        //    //For Total Products.
        //    HomeSearch.Tables.Add(Table1);
        //    Table1.Columns.Add("TOTAL_PRODUCTS", typeof(string));

        //    //For Display Sarch Result.
        //    HomeSearch.Tables.Add(Table2);
        //    Table2.Columns.Add("FAMILY_ID", typeof(string));
        //    Table2.Columns.Add("FAMILY_NAME", typeof(string));
        //    Table2.Columns.Add("DESCRIPTION1", typeof(string));
        //    Table2.Columns.Add("LongDESCRIPTION", typeof(string));
        //    Table2.Columns.Add("PRODUCT_ID", typeof(string));
        //    Table2.Columns.Add("ATTRIBUTE_ID", typeof(string));
        //    Table2.Columns.Add("STRING_VALUE", typeof(string));
        //    Table2.Columns.Add("NUMERIC_VALUE", typeof(string));
        //    Table2.Columns.Add("OBJECT_TYPE", typeof(string));
        //    Table2.Columns.Add("OBJECT_NAME", typeof(string));
        //    Table2.Columns.Add("ATTRIBUTE_NAME", typeof(string));
        //    Table2.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
        //    Table2.Columns.Add("CATEGORY_ID", typeof(string));
        //    Table2.Columns.Add("CATEGORY_NAME", typeof(string));
        //    Table2.Columns.Add("PARENT_CATEGORY_NAME", typeof(string));
        //    Table2.Columns.Add("PARENT_CATEGORY_ID", typeof(string));
        //    Table2.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
        //    Table2.Columns.Add("PARENT_FAMILY_ID", typeof(string));
        //    Table2.Columns.Add("(No column name)", typeof(string));
        //    Table2.Columns.Add("LFID", typeof(string));
        //    Table2.Columns.Add("Family_Prod_Count", typeof(int));
        //    Table2.Columns.Add("Prod_Count", typeof(int));
        //    Table2.Columns.Add("STATUS", typeof(bool));

        //    //For Sarch Result Related.
        //    HomeSearch.Tables.Add(Table3);
        //    Table3.Columns.Add("CATALOG_ID", typeof(string));
        //    Table3.Columns.Add("PRODUCT_ID", typeof(string));
        //    Table3.Columns.Add("STATUS", typeof(string));
        //}

        ////public DataSet Get_Homepage_UserSearch(string srctxt, int curPage, string pageOp)
        ////{
        ////    try
        ////    {
        ////        RemoteEasyAsk();
        ////        DataSet ds = new DataSet();
        ////        Create_Search_Table_Columns();

        ////        if (curPage < 1)
        ////        {
        ////            INavigateResults res = ea.userSearch("AllProducts////WESAUSTRALASIA////Cellular Accessories", srctxt);
        ////            ds = Get_DataSet_Values(res);
        ////        }
        ////        else
        ////        {
        ////            INavigateResults res1 = ea.userPageOp("AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch1=" + srctxt + "", curPage.ToString(), pageOp);
        ////            ds = Get_DataSet_Values(res1);
        ////        }
        ////        return ds;
        ////    }
        ////    catch (Exception)
        ////    {
        ////        return null;
        ////    }
        ////}

        //DataSet Get_DataSet_Values(INavigateResults res)
        //{
        //    DataRow dr = Table.NewRow();
        //    dr[0] = res.getPageCount();
        //    HomeSearch.Tables[0].Rows.Add(dr);

        //    DataRow dr1 = Table1.NewRow();
        //    dr1[0] = res.getTotalItems();
        //    HomeSearch.Tables[1].Rows.Add(dr1);

        //    int last = res.getLastItem();
        //    int colFmlyID = res.getColumnIndex("Family Id");
        //    int colFmlyName = res.getColumnIndex("Family Name");
        //    int colFmlyDesc = res.getColumnIndex("Family ShortDescription");
        //    int colFmlylongDesc = res.getColumnIndex("Family Description");
        //    int colFmlyImg = res.getColumnIndex("Family Thumbnail");

        //    int colProductID = res.getColumnIndex("Prod Id");
        //    int colProductCode = res.getColumnIndex("Prod Code");
        //    int colProductPrice = res.getColumnIndex("Price");
        //    int colProductDesc = res.getColumnIndex("Prod Description");
        //    int colProductImg = res.getColumnIndex("Prod Thumbnail");

        //    int colProductCount = res.getColumnIndex("Prod Count");
        //    int colFamilyProdCount = res.getColumnIndex("Family Prod Count");

        //    IList<INavigateCategory> item = res.getDetailedCategories();
        //    DataRow dRow;
        //    try
        //    {
        //        if (last >= 0)
        //        {
        //            for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++)
        //            {
        //                for (int k = 0; k < 9; k++)
        //                {
        //                    dRow = Table2.NewRow();
        //                    dRow["FAMILY_ID"] = res.getCellData(i, colFmlyID);
        //                    dRow["FAMILY_NAME"] = res.getCellData(i, colFmlyName);
        //                    dRow["DESCRIPTION1"] = res.getCellData(i, colFmlyDesc);
        //                    dRow["LongDESCRIPTION"] = res.getCellData(i, colFmlylongDesc);
        //                    dRow["PRODUCT_ID"] = res.getCellData(i, colProductID);

        //                    string temp_family_count = res.getCellData(i, colFamilyProdCount);

        //                    string temp_product_count = res.getCellData(i, colProductCount);
        //                    string temp_fmly_Image = res.getCellData(i, colFmlyImg).ToString();
        //                    string temp_product_Image = res.getCellData(i, colProductImg).ToString();
        //                    string image_string = "";

        //                    if (temp_fmly_Image != "" && temp_product_Image != "")
        //                    {
        //                        if (temp_product_count.ToString() == "1")
        //                        {
        //                            image_string = temp_product_Image.Substring(42);
        //                        }
        //                        else
        //                        {
        //                            image_string = temp_fmly_Image.Substring(42);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        image_string = "noimage.gif";
        //                    }

        //                    if (k == 0)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "1";
        //                        dRow["STRING_VALUE"] = res.getCellData(i, colProductCode);//For the Product Code
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Code";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    if (k == 1)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "5";
        //                        dRow["STRING_VALUE"] = "";
        //                        if (res.getCellData(i, colProductPrice) == "" || res.getCellData(i, colProductPrice) == string.Empty)
        //                        {
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                        }
        //                        else
        //                        {
        //                            dRow["NUMERIC_VALUE"] = res.getCellData(i, colProductPrice).Substring(1);//For Cost
        //                        }
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Cost";
        //                        dRow["ATTRIBUTE_TYPE"] = "4";
        //                    }
        //                    if (k == 2)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "62";
        //                        dRow["STRING_VALUE"] = res.getCellData(i, colFmlyDesc);//Family Description
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Description";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    if (k == 3)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "449";
        //                        dRow["STRING_VALUE"] = res.getCellData(i, colProductDesc);//Product Description.
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "PROD_DSC";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    if (k == 4)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "492";
        //                        dRow["STRING_VALUE"] = "";
        //                        if (res.getCellData(i, colProductPrice) == "" || res.getCellData(i, colProductPrice) == string.Empty)
        //                        {
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                        }
        //                        else
        //                        {
        //                            dRow["NUMERIC_VALUE"] = res.getCellData(i, colProductPrice).Substring(1);//For Cost
        //                        }
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "PROD_EXT_PRI_3";
        //                        dRow["ATTRIBUTE_TYPE"] = "4";
        //                    }
        //                    if (k == 5)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "453";
        //                        dRow["STRING_VALUE"] = image_string;
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = image_string;
        //                        dRow["ATTRIBUTE_NAME"] = "Web Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "3";
        //                    }
        //                    if (k == 6)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "7";
        //                        dRow["STRING_VALUE"] = image_string;
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = image_string;
        //                        dRow["ATTRIBUTE_NAME"] = "Product Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "3";
        //                    }
        //                    if (k == 7)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "452";
        //                        dRow["STRING_VALUE"] = image_string;
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = image_string;
        //                        dRow["ATTRIBUTE_NAME"] = "TWeb Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "3";
        //                    }
        //                    if (k == 8)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "4";
        //                        dRow["STRING_VALUE"] = res.getCellData(i, colFmlylongDesc);//Family Description
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "DESCRIPTIONS";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }

        //                    dRow["CATEGORY_ID"] = "";
        //                    dRow["CATEGORY_NAME"] = string.Empty;
        //                    dRow["PARENT_CATEGORY_NAME"] = "";
        //                    dRow["PARENT_CATEGORY_ID"] = "";
        //                    dRow["CUSTOM_NUM_FIELD3"] = "2";
        //                    dRow["PARENT_FAMILY_ID"] = "0";
        //                    dRow["(No column name)"] = "";
        //                    dRow["LFID"] = res.getCellData(i, colFmlyID);
        //                    dRow["Family_Prod_Count"] = temp_family_count;
        //                    dRow["Prod_Count"] = temp_product_count;
        //                  //  HttpContext.Current.Session["FamilyCount"] = temp_family_count;
        //                    if (temp_family_count == temp_product_count)
        //                    {
        //                        dRow["STATUS"] = true;
        //                    }
        //                    else
        //                    {
        //                        dRow["STATUS"] = false;
        //                    }
        //                    Table2.Rows.Add(dRow);
        //                }
        //                j++;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    HttpContext.Current.Session["HomeSearch"] = HomeSearch;
        //    return HomeSearch;
        //}

        //#endregion

        //#region "For Menu Click Results"

        //DataSet Menu_Click = new DataSet();
        //DataTable Mnu_Temp = new DataTable();
        //DataTable Mnu_Table = new DataTable();
        //DataTable Mnu_Table1 = new DataTable();
        //DataTable Mnu_Table2 = new DataTable();
        //DataTable Mnu_Table3 = new DataTable();

        //public DataSet Category_Menu_Click(string pid, string cname, int curPage, string temptext, string pageOp)
        //{
        //    try
        //    {
        //        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
        //        IOptions opts = ea.getOptions();
        //        opts.setResultsPerPage(m_rpp); // ea_rpp.Value);   // use current settings
        //        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        //        opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
        //        opts.setSubCategories(true);
        //        opts.setNavigateHierarchy(false);
        //        opts.setReturnSKUs(false);  
        //        Create_Menu_Table_Columns();
        //        INavigateResults res = null;
        //        if (curPage < 1)
        //        {
        //            string Encode_Path = "AllProducts////WESAUSTRALASIA////" + cname;
        //            res = ea.userCategoryClick(Encode_Path);
        //            if (HttpContext.Current.Request["pgno"] == null)
        //            {
        //                updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //            }
        //            HttpContext.Current.Session["EA"] = res.getCatPath();
        //            IList<INavigateCategory> list = res.getDetailedCategories();

        //            foreach (INavigateCategory item in list)
        //            {
        //                DataRow row = Mnu_Temp.NewRow();
        //                IList<string> li = item.getIDs();
        //                row["category_id"] = li[0].ToString().Substring(2);
        //                row["category_name"] = item.getName();
        //                row["image_file"] = "NULL";
        //                row["custom_num_field3"] = "2";
        //                row["parent_category"] = pid;
        //                Mnu_Temp.Rows.Add(row);
        //            }
        //        }
        //        else
        //        {
        //            res = ea.userPageOp("AllProducts////WESAUSTRALASIA////Cellular Accessories////" + cname, curPage.ToString(), pageOp);
        //            IList<INavigateCategory> list = res.getDetailedCategories();
        //            updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //            //if (HttpContext.Current.Request.Url.OriginalString.Contains("categorylist.aspx"))
        //            //{
        //            //    Category_updateBreadCrumb(res.getBreadCrumbTrail());
        //            //}
        //            //else
        //            //{
        //            //    product_updateBreadCrumb(res.getBreadCrumbTrail());
        //            //}
        //            foreach (INavigateCategory item in list)
        //            {
        //                DataRow row = Mnu_Temp.NewRow();
        //                IList<string> li = item.getIDs();
        //                row["category_id"] = li[0].ToString().Substring(2);
        //                row["category_name"] = item.getName();
        //                row["image_file"] = "NULL";
        //                row["custom_num_field3"] = "2";
        //                row["parent_category"] = pid;
        //                Mnu_Temp.Rows.Add(row);
        //            }
        //        }
        //        Get_FamilyDS_Values(res);

        //        //Menu_Click.Tables.Add(Mnu_Temp);
        //        //Menu_Click.Tables.Add(Mnu_Table);
        //        //Menu_Click.Tables.Add(Mnu_Table1);
        //        //Menu_Click.Tables.Add(Mnu_Table2);
        //        //Menu_Click.Tables.Add(Mnu_Table3);

        //        if (Menu_Click.Tables.Count > 0)
        //        {
        //            HttpContext.Current.Session["Click_Menu_Results"] = Menu_Click;
        //        }

        //        IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //        EasyAsk_UserSearch_Attributes_DS(Attributes, res, temptext);
        //        HttpContext.Current.Session["Menu"] = Menu_Click;
        //      //  objErrorhandler._LogMsg = Environment.NewLine + "After Category List & Product List Page EasyAsk Result -" + DateTime.Now.ToLongTimeString();
        //      //  objErrorhandler.CreateLogTest();
        //        return Menu_Click;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //void Create_Menu_Table_Columns()
        //{
        //    Menu_Click.Tables.Add(Mnu_Temp);
        //    //For Category
        //    Mnu_Temp.Columns.Add("category_id", typeof(string));
        //    Mnu_Temp.Columns.Add("category_name", typeof(string));
        //    Mnu_Temp.Columns.Add("image_file", typeof(string));
        //    Mnu_Temp.Columns.Add("custom_num_field3", typeof(string));
        //    Mnu_Temp.Columns.Add("parent_category", typeof(string));

        //    Menu_Click.Tables.Add(Mnu_Table);
        //    //For Total Pages.
        //    Mnu_Table.Columns.Add("TOTAL_PAGES", typeof(int));

        //    Menu_Click.Tables.Add(Mnu_Table1);
        //    //For Total Products.
        //    Mnu_Table1.Columns.Add("TOTAL_PRODUCTS", typeof(string));

        //    Menu_Click.Tables.Add(Mnu_Table2);
        //    //For Display Family Result.
        //    Mnu_Table2.Columns.Add("FAMILY_ID", typeof(string));
        //    Mnu_Table2.Columns.Add("FAMILY_NAME", typeof(string));
        //    Mnu_Table2.Columns.Add("ATTRIBUTE_ID", typeof(string));
        //    Mnu_Table2.Columns.Add("STRING_VALUE", typeof(string));
        //    Mnu_Table2.Columns.Add("NUMERIC_VALUE", typeof(string));
        //    Mnu_Table2.Columns.Add("OBJECT_TYPE", typeof(string));
        //    Mnu_Table2.Columns.Add("OBJECT_NAME", typeof(string));
        //    Mnu_Table2.Columns.Add("ATTRIBUTE_NAME", typeof(string));
        //    Mnu_Table2.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
        //    //Mnu_Table2.Columns.Add("Family_Prod_Count", typeof(string));
        //    //Mnu_Table2.Columns.Add("Prod_Count", typeof(string));

        //    Menu_Click.Tables.Add(Mnu_Table3);
        //    //For Family Result Related.
        //    Mnu_Table3.Columns.Add("FAMILY_ID", typeof(string));
        //    Mnu_Table3.Columns.Add("FAMILY_NAME", typeof(string));
        //    Mnu_Table3.Columns.Add("Family_Prod_Count", typeof(string));
        //    Mnu_Table3.Columns.Add("Prod_Count", typeof(string));
        //}

        //DataSet Get_FamilyDS_Values(INavigateResults res)
        //{
        //    DataRow dr = Mnu_Table.NewRow();
        //    dr[0] = res.getPageCount();
        //    Mnu_Table.Rows.Add(dr);

        //    DataRow dr1 = Mnu_Table1.NewRow();
        //    dr1[0] = res.getTotalItems();
        //    Mnu_Table1.Rows.Add(dr1);

        //    int last = res.getLastItem();
        //    int colFmlyID = res.getColumnIndex("Family Id");
        //    int colFmlyName = res.getColumnIndex("Family Name");
        //    int colFmlyDesc = res.getColumnIndex("Family Description");
        //    int colShortFmlyDesc = res.getColumnIndex("Family ShortDescription");
        //    int colFmlyImg = res.getColumnIndex("Family Thumbnail");
        //    int colFamilyCount = res.getColumnIndex("Family Prod Count");
        //    int colProductCode = res.getColumnIndex("Prod Code");
        //    int colProductImg = res.getColumnIndex("Prod Thumbnail");
        //    int colProductCount = res.getColumnIndex("Prod Count");

        //    DataRow dRow;
        //    try
        //    {
        //        if (last >= 0)
        //        {
        //            for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++)
        //            {
        //                DataRow row1 = Mnu_Table3.NewRow();
        //                row1["FAMILY_ID"] = res.getCellData(i, colFmlyID);
        //                row1["FAMILY_NAME"] = res.getCellData(i, colFmlyName);
        //                row1["Family_Prod_Count"] = res.getCellData(i, colFamilyCount);
        //                row1["Prod_Count"] = res.getCellData(i, colProductCount);
        //               // HttpContext.Current.Session["FamilyCount"] = res.getCellData(i, colFamilyCount);
        //                Mnu_Table3.Rows.Add(row1);

        //                for (int k = 0; k < 5; k++)
        //                {
        //                    dRow = Mnu_Table2.NewRow();
        //                    dRow["FAMILY_ID"] = res.getCellData(i, colFmlyID);
        //                    dRow["FAMILY_NAME"] = res.getCellData(i, colFmlyName);

        //                    string temp_family_count = res.getCellData(i, colFamilyCount);
        //                    string temp_product_count = res.getCellData(i, colProductCount);
        //                    string temp_fmly_Image = res.getCellData(i, colFmlyImg).ToString();
        //                    string temp_product_Image = res.getCellData(i, colProductImg).ToString();
        //                    string image_string = "";

        //                    if (temp_fmly_Image != "" && temp_product_Image != "")
        //                    {
        //                        if (temp_product_count.ToString() == "1")
        //                        {
        //                            image_string = temp_product_Image.Substring(42);
        //                        }
        //                        else
        //                        {
        //                            image_string = temp_fmly_Image.Substring(42);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        image_string = "noimage.gif";
        //                    }


        //                    if (k == 0)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "4";
        //                        dRow["STRING_VALUE"] = res.getCellData(i, colFmlyDesc);
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Descriptions";
        //                        dRow["ATTRIBUTE_TYPE"] = "7";
        //                    }
        //                    if (k == 1)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "8";
        //                        if (res.getCellData(i, colFmlyImg) != "")
        //                        {
        //                            dRow["STRING_VALUE"] = image_string;// res.getCellData(i, colFmlyImg).Substring(42).ToString();
        //                        }
        //                        else
        //                        {
        //                            dRow["STRING_VALUE"] = @"\NoImage.gif";
        //                        }
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Family Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "9";
        //                    }
        //                    if (k == 2)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "329";
        //                        dRow["STRING_VALUE"] = res.getCellData(i, colFmlyDesc);
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Descriptions1";
        //                        dRow["ATTRIBUTE_TYPE"] = "7";
        //                    }
        //                    if (k == 3)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "13";
        //                        dRow["STRING_VALUE"] = res.getCellData(i, colShortFmlyDesc);
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Short Description";
        //                        dRow["ATTRIBUTE_TYPE"] = "7";
        //                    }
        //                    if (k == 4)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "1";
        //                        dRow["STRING_VALUE"] = res.getCellData(i, colProductCode);
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Code";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    Mnu_Table2.Rows.Add(dRow);
        //                }
        //                j++;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return Menu_Click;
        //}

        //#endregion

        //#region "For the Family Page Products"

        //DataSet ProductSearch = new DataSet();
        //DataTable Producttable = new DataTable();
        //DataTable Familytable = new DataTable();

        //DataSet Sub_Family_DS = new DataSet();
        //DataTable Sub_Family_Products = new DataTable();
        //DataTable Sub_Family_Details = new DataTable();

        //public DataSet EasyAsk_Family_Products(string srctxt, string familyid, string temptext, string familyshow)
        //{
        //    try
        //    {
        //        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
        //        IOptions opts = ea.getOptions();
        //        opts.setResultsPerPage(m_rpp); // ea_rpp.Value);   // use current settings
        //        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        //        opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
        //        opts.setSubCategories(false);
        //        opts.setNavigateHierarchy(false);
        //        opts.setReturnSKUs(true);
        //        INavigateResults res = null;
        //        DataSet ds = new DataSet();
        //        DataSet subfamily_ds = new DataSet();
        //        Create_Product_Table_Columns();
        //        Create_SubFamily_Table_Columns();
        //        //ea.setReturnSKUS(true);
        //        if (familyshow == "1")
        //        {
        //            opts.setResultsPerPage("0");
        //            //ea.setResultsPerPage("0");
        //            INavigateResults res1 = ea.userSearch("AllProducts////WESAUSTRALASIA////Cellular Accessories", "Family Id=" + familyid);
        //            ds = GetProductDetails(res1);
        //            subfamily_ds = GetSubfamilyDetails(res1);

        //            //For Leftnavigation New UI
        //            IList<string> Attributes = res1.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //            EasyAsk_UserSearch_Attributes_DS(Attributes, res1, temptext);

        //            //Store Family details in session
        //            HttpContext.Current.Session["Family"] = ds;
        //            HttpContext.Current.Session["Sub_Family"] = subfamily_ds;
        //            HttpContext.Current.Session["EA"] = res1.getCatPath();
        //            updateBreadCrumb(res1.getBreadCrumbTrail(), familyshow, familyid);
        //        }
        //        else
        //        {
        //            opts.setResultsPerPage("0");
        //         //   ea.setResultsPerPage("0");
        //            res = ea.userSearch(HttpContext.Current.Session["EA"].ToString(), "Family Id=" + familyid);
        //            ds = GetProductDetails(res);
        //            subfamily_ds = GetSubfamilyDetails(res);

        //            //For Leftnavigation New UI
        //            IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //            EasyAsk_UserSearch_Attributes_DS(Attributes, res, temptext);

        //            //Store Family details in session
        //            HttpContext.Current.Session["Family"] = ds;
        //            HttpContext.Current.Session["Sub_Family"] = subfamily_ds;
        //            HttpContext.Current.Session["EA"] = res.getCatPath();
        //            //  string SEO_PATH = "";
        //            updateBreadCrumb(res.getBreadCrumbTrail(), familyshow, familyid);
        //        }
        //      //  objErrorhandler._LogMsg = Environment.NewLine + "After EasyAsk API Call For Selecting Family Details -" + DateTime.Now.ToLongTimeString();
        //      //  objErrorhandler.CreateLogTest();
        //        return ds;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //private void Create_Product_Table_Columns()
        //{
        //    ProductSearch.Tables.Add(Familytable);

        //    Familytable.Columns.Add("FAMILY_ID", typeof(string));
        //    Familytable.Columns.Add("FAMILY_NAME", typeof(string));
        //    Familytable.Columns.Add("STRING_VALUE", typeof(string));
        //    Familytable.Columns.Add("NUMERIC_VALUE", typeof(string));
        //    Familytable.Columns.Add("ATTRIBUTE_ID", typeof(string));
        //    Familytable.Columns.Add("ATTRIBUTE_NAME", typeof(string));
        //    Familytable.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
        //    Familytable.Columns.Add("ATTRIBUTE_DATA_TYPE", typeof(string));
        //    Familytable.Columns.Add("Family_Prod_Count", typeof(string));
        //    Familytable.Columns.Add("Prod_Count", typeof(string));
        //    Familytable.Columns.Add("STATUS", typeof(string));

        //    ProductSearch.Tables.Add(Producttable);//family products

        //    Producttable.Columns.Add("PRODUCT_ID", typeof(string));
        //    Producttable.Columns.Add("FAMILY_ID", typeof(string));
        //    Producttable.Columns.Add("STRING_VALUE", typeof(string));
        //    Producttable.Columns.Add("NUMERIC_VALUE", typeof(string));
        //    Producttable.Columns.Add("ATTRIBUTE_NAME", typeof(string));
        //    Producttable.Columns.Add("ATTRIBUTE_ID", typeof(string));
        //    Producttable.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
        //    Producttable.Columns.Add("ATTRIBUTE_DATATYPE", typeof(string));
        //}

        //private void Create_SubFamily_Table_Columns()
        //{
        //    Sub_Family_DS.Tables.Add(Sub_Family_Products);//subfamily products

        //    Sub_Family_Products.Columns.Add("PRODUCT_ID", typeof(string));
        //    Sub_Family_Products.Columns.Add("FAMILY_ID", typeof(string));
        //    Sub_Family_Products.Columns.Add("SUB_FAMILY_ID", typeof(string));
        //    Sub_Family_Products.Columns.Add("STRING_VALUE", typeof(string));
        //    Sub_Family_Products.Columns.Add("NUMERIC_VALUE", typeof(string));
        //    Sub_Family_Products.Columns.Add("ATTRIBUTE_NAME", typeof(string));
        //    Sub_Family_Products.Columns.Add("ATTRIBUTE_ID", typeof(string));
        //    Sub_Family_Products.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
        //    Sub_Family_Products.Columns.Add("ATTRIBUTE_DATATYPE", typeof(string));

        //    Sub_Family_DS.Tables.Add(Sub_Family_Details);//subfamily products

        //    Sub_Family_Details.Columns.Add("FAMILY_ID", typeof(string));
        //    Sub_Family_Details.Columns.Add("FAMILY_NAME", typeof(string));
        //    Sub_Family_Details.Columns.Add("STRING_VALUE", typeof(string));
        //    Sub_Family_Details.Columns.Add("NUMERIC_VALUE", typeof(string));
        //    Sub_Family_Details.Columns.Add("ATTRIBUTE_ID", typeof(string));
        //    Sub_Family_Details.Columns.Add("ATTRIBUTE_NAME", typeof(string));
        //    Sub_Family_Details.Columns.Add("ATTRIBUTE_DATATYPE", typeof(string));
        //    Sub_Family_Details.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
        //}

        //private DataSet GetProductDetails(INavigateResults res)
        //{
        //    int last = res.getLastItem();
        //    int first = res.getFirstItem();
        //    int colFmlyID = res.getColumnIndex("Family Id");
        //    int colFmlyName = res.getColumnIndex("Family Name");
        //    int colFmlyDesc = res.getColumnIndex("Family Description");//ShortDescription
        //    int colFmlyImg = res.getColumnIndex("Family Thumbnail");

        //    int subFmlyID = res.getColumnIndex("SubFamily Id");
        //    int subFmlyName = res.getColumnIndex("SubFamily Name");
        //    int subFmlyDesc = res.getColumnIndex("SubFamily Description");

        //    int colProductID = res.getColumnIndex("Prod Id");
        //    int colProductCode = res.getColumnIndex("Prod Code");
        //    int colProductPrice = res.getColumnIndex("Price");
        //    int colProductDesc = res.getColumnIndex("Prod Description");
        //    int colProductImg = res.getColumnIndex("Prod Thumbnail");

        //    int colProductCount = res.getColumnIndex("Prod Count");
        //    int colFamilyProdCount = res.getColumnIndex("Family Prod Count");

        //    DataRow dr, dRow;
        //    try
        //    {
        //        if (last >= 0)
        //        {
        //            string F_Id = res.getCellData(0, colFmlyID);
        //            string F_Name = res.getCellData(0, colFmlyName);
        //            string F_Count = res.getCellData(0, colFamilyProdCount);
        //            string P_Count = last.ToString();
        //            string Status = "false";
        //            if (P_Count == "1")
        //            {
        //                if (F_Count != P_Count)
        //                {
        //                    Status = "One Product";
        //                }
        //                else
        //                {
        //                    Status = "true";
        //                }
        //            }
        //            else if (F_Count == P_Count)
        //            {
        //                Status = "true";
        //            }
        //            else
        //            {
        //                Status = "false";
        //            }

        //            //For Family Details
        //            for (int i = 0; i < 7; i++)
        //            {
        //                dr = Familytable.NewRow();
        //                if (i == 0)
        //                {
        //                    dr["FAMILY_ID"] = F_Id;
        //                    dr["FAMILY_NAME"] = F_Name;
        //                    dr["STRING_VALUE"] = "";
        //                    dr["NUMERIC_VALUE"] = "";
        //                    dr["ATTRIBUTE_ID"] = "";
        //                    dr["ATTRIBUTE_NAME"] = "Code";
        //                    dr["ATTRIBUTE_TYPE"] = "";
        //                    dr["ATTRIBUTE_DATA_TYPE"] = "TEXT";
        //                    dr["Family_Prod_Count"] = F_Count;
        //                    dr["Prod_Count"] = P_Count.ToString();
        //                    dr["STATUS"] = Status;
        //                }
        //                if (i == 1)
        //                {
        //                    dr["FAMILY_ID"] = F_Id;
        //                    dr["FAMILY_NAME"] = F_Name;
        //                    dr["STRING_VALUE"] = res.getCellData(0, colFmlyDesc);
        //                    dr["NUMERIC_VALUE"] = "";
        //                    dr["ATTRIBUTE_ID"] = "329";
        //                    dr["ATTRIBUTE_NAME"] = "Descriptions1";
        //                    dr["ATTRIBUTE_TYPE"] = "7";
        //                    dr["ATTRIBUTE_DATA_TYPE"] = "TEXT";
        //                    dr["Family_Prod_Count"] = F_Count;
        //                    dr["Prod_Count"] = P_Count.ToString();
        //                    dr["STATUS"] = Status;
        //                }
        //                if (i == 2)
        //                {
        //                    dr["FAMILY_ID"] = F_Id;
        //                    dr["FAMILY_NAME"] = F_Name;
        //                    dr["STRING_VALUE"] = res.getCellData(0, colFmlyDesc);
        //                    dr["NUMERIC_VALUE"] = "";
        //                    dr["ATTRIBUTE_ID"] = "4";
        //                    dr["ATTRIBUTE_NAME"] = "Descriptions";
        //                    dr["ATTRIBUTE_TYPE"] = "7";
        //                    dr["ATTRIBUTE_DATA_TYPE"] = "TEXT";
        //                    dr["Family_Prod_Count"] = F_Count;
        //                    dr["Prod_Count"] = P_Count.ToString();
        //                    dr["STATUS"] = Status;
        //                }
        //                if (i == 3)
        //                {
        //                    dr["FAMILY_ID"] = F_Id;
        //                    dr["FAMILY_NAME"] = F_Name;
        //                    if (res.getCellData(0, colFmlyImg) != "")
        //                    {
        //                        dr["STRING_VALUE"] = res.getCellData(0, colFmlyImg).Substring(42).ToString();
        //                    }
        //                    else
        //                    {
        //                        dr["STRING_VALUE"] = @"\NoImage.gif";
        //                    }
        //                    dr["NUMERIC_VALUE"] = "";
        //                    dr["ATTRIBUTE_ID"] = "8";
        //                    dr["ATTRIBUTE_NAME"] = "Family Image1";
        //                    dr["ATTRIBUTE_TYPE"] = "9";
        //                    dr["ATTRIBUTE_DATA_TYPE"] = "TEXT";
        //                    dr["Family_Prod_Count"] = F_Count;
        //                    dr["Prod_Count"] = P_Count.ToString();
        //                    dr["STATUS"] = Status;
        //                }
        //                if (i == 4)
        //                {
        //                    dr["FAMILY_ID"] = F_Id;
        //                    dr["FAMILY_NAME"] = F_Name;
        //                    if (res.getCellData(0, colFmlyImg) != "")
        //                    {
        //                        dr["STRING_VALUE"] = res.getCellData(0, colFmlyImg).Substring(42).ToString();
        //                    }
        //                    else
        //                    {
        //                        dr["STRING_VALUE"] = @"\NoImage.gif";
        //                    }
        //                    dr["NUMERIC_VALUE"] = "";
        //                    dr["ATTRIBUTE_ID"] = "746";
        //                    dr["ATTRIBUTE_NAME"] = "FWeb Image1";
        //                    dr["ATTRIBUTE_TYPE"] = "9";
        //                    dr["ATTRIBUTE_DATA_TYPE"] = "TEXT";
        //                    dr["Family_Prod_Count"] = F_Count;
        //                    dr["Prod_Count"] = P_Count.ToString();
        //                    dr["STATUS"] = Status;
        //                }
        //                if (i == 5)
        //                {
        //                    dr["FAMILY_ID"] = F_Id;
        //                    dr["FAMILY_NAME"] = F_Name;
        //                    if (res.getCellData(0, colFmlyImg) != "")
        //                    {
        //                        dr["STRING_VALUE"] = res.getCellData(0, colFmlyImg).Substring(42).ToString();
        //                        //    if (res.getCellData(0, colProductImg) != "")
        //                        //    {
        //                        //        dr["STRING_VALUE"] = res.getCellData(0, colProductImg).Substring(42).ToString();
        //                        //    }
        //                        //    else
        //                        //    {
        //                        //        dr["STRING_VALUE"] = @"\NoImage.gif";
        //                        //    }
        //                    }
        //                    else
        //                    {
        //                        dr["STRING_VALUE"] = @"\NoImage.gif";
        //                    }
        //                    dr["NUMERIC_VALUE"] = "";
        //                    dr["ATTRIBUTE_ID"] = "747";
        //                    dr["ATTRIBUTE_NAME"] = "TFWeb Image1";
        //                    dr["ATTRIBUTE_TYPE"] = "9";
        //                    dr["ATTRIBUTE_DATA_TYPE"] = "TEXT";
        //                    dr["Family_Prod_Count"] = F_Count;
        //                    dr["Prod_Count"] = P_Count.ToString();
        //                    dr["STATUS"] = Status;
        //                }
        //                if (i == 6)
        //                {
        //                    dr["FAMILY_ID"] = F_Id;
        //                    dr["FAMILY_NAME"] = F_Name;
        //                    dr["ATTRIBUTE_ID"] = "452";
        //                    if (res.getCellData(0, colProductImg) != "")
        //                    {
        //                        dr["STRING_VALUE"] = res.getCellData(0, colProductImg).Substring(42).ToString();
        //                    }
        //                    else
        //                    {
        //                        dr["STRING_VALUE"] = @"\NoImage.gif";
        //                    }
        //                    dr["NUMERIC_VALUE"] = "";
        //                    dr["ATTRIBUTE_NAME"] = "TWeb Image1";
        //                    dr["ATTRIBUTE_TYPE"] = "3";
        //                    dr["ATTRIBUTE_DATA_TYPE"] = "TEXT";
        //                    dr["Family_Prod_Count"] = F_Count;
        //                    dr["Prod_Count"] = P_Count.ToString();
        //                    dr["STATUS"] = Status;
        //                }
        //                Familytable.Rows.Add(dr);
        //            }
        //         //   HttpContext.Current.Session["FamilyCount"] = F_Count;
        //            //For family page products
        //            for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++) //For Product details
        //            {
        //                if (res.getCellData(i, subFmlyID) == "" && res.getCellData(i, subFmlyName) == "")
        //                {
        //                    for (int k = 0; k < 6; k++)
        //                    {
        //                        dRow = Producttable.NewRow();
        //                        dRow["PRODUCT_ID"] = res.getCellData(i, colProductID);
        //                        dRow["FAMILY_ID"] = res.getCellData(i, colFmlyID);
        //                        if (k == 0)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "1";
        //                            dRow["STRING_VALUE"] = res.getCellData(i, colProductCode);//For the Product Code
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                            dRow["ATTRIBUTE_NAME"] = "Code";
        //                            dRow["ATTRIBUTE_TYPE"] = "1";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                        }
        //                        if (k == 1)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "5";
        //                            dRow["STRING_VALUE"] = "";
        //                            if (res.getCellData(i, colProductPrice) == "")
        //                            {
        //                                dRow["NUMERIC_VALUE"] = "0";
        //                            }
        //                            else
        //                            {
        //                                dRow["NUMERIC_VALUE"] = res.getCellData(i, colProductPrice).Substring(1);//For Cost
        //                            }
        //                            dRow["ATTRIBUTE_NAME"] = "Cost";
        //                            dRow["ATTRIBUTE_TYPE"] = "4";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "NUMBER";
        //                        }
        //                        if (k == 2)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "449";
        //                            dRow["STRING_VALUE"] = res.getCellData(i, colProductDesc);
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                            dRow["ATTRIBUTE_NAME"] = "PROD_DSC";
        //                            dRow["ATTRIBUTE_TYPE"] = "1";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                        }
        //                        if (k == 3)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "449";
        //                            dRow["STRING_VALUE"] = res.getCellData(i, colProductDesc);
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                            dRow["ATTRIBUTE_NAME"] = "DESCRIPTION";
        //                            dRow["ATTRIBUTE_TYPE"] = "1";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                        }
        //                        if (k == 4)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "7";
        //                            if (res.getCellData(0, colProductImg) != "")
        //                            {
        //                                dRow["STRING_VALUE"] = res.getCellData(0, colProductImg).Substring(42).ToString();
        //                            }
        //                            else
        //                            {
        //                                dRow["STRING_VALUE"] = @"\NoImage.gif";
        //                            }
        //                            dRow["NUMERIC_VALUE"] = "";
        //                            dRow["ATTRIBUTE_NAME"] = "Product Image1";
        //                            dRow["ATTRIBUTE_TYPE"] = "3";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                        }
        //                        if (k == 5)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "452";
        //                            if (res.getCellData(0, colProductImg) != "")
        //                            {
        //                                dRow["STRING_VALUE"] = res.getCellData(0, colProductImg).Substring(42).ToString();
        //                            }
        //                            else
        //                            {
        //                                dRow["STRING_VALUE"] = @"\NoImage.gif";
        //                            }
        //                            dRow["NUMERIC_VALUE"] = "";
        //                            dRow["ATTRIBUTE_NAME"] = "TWeb Image1";
        //                            dRow["ATTRIBUTE_TYPE"] = "3";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                        }
        //                        Producttable.Rows.Add(dRow);
        //                    }
        //                }
        //                else
        //                {
        //                    //For subfamily product details
        //                    for (int k = 0; k < 5; k++)
        //                    {
        //                        dRow = Sub_Family_Products.NewRow();
        //                        dRow["PRODUCT_ID"] = res.getCellData(i, colProductID);
        //                        dRow["FAMILY_ID"] = res.getCellData(i, colFmlyID);
        //                        dRow["SUB_FAMILY_ID"] = res.getCellData(i, subFmlyID);
        //                        if (k == 0)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "1";
        //                            dRow["STRING_VALUE"] = res.getCellData(i, colProductCode);//For the Product Code
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                            dRow["ATTRIBUTE_NAME"] = "Code";
        //                            dRow["ATTRIBUTE_TYPE"] = "1";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                        }
        //                        if (k == 1)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "5";
        //                            dRow["STRING_VALUE"] = "";
        //                            if (res.getCellData(i, colProductPrice) == "")
        //                            {
        //                                dRow["NUMERIC_VALUE"] = "0";
        //                            }
        //                            else
        //                            {
        //                                dRow["NUMERIC_VALUE"] = res.getCellData(i, colProductPrice).Substring(1);//For Cost
        //                            }
        //                            dRow["ATTRIBUTE_NAME"] = "Cost";
        //                            dRow["ATTRIBUTE_TYPE"] = "4";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "NUMBER";
        //                        }
        //                        if (k == 2)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "449";
        //                            dRow["STRING_VALUE"] = res.getCellData(i, colProductDesc);
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                            dRow["ATTRIBUTE_NAME"] = "PROD_DSC";
        //                            dRow["ATTRIBUTE_TYPE"] = "1";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                        }
        //                        if (k == 3)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "449";
        //                            dRow["STRING_VALUE"] = res.getCellData(i, colProductDesc) + " Testing";
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                            dRow["ATTRIBUTE_NAME"] = "DESCRIPTION";
        //                            dRow["ATTRIBUTE_TYPE"] = "1";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                        }
        //                        if (k == 4)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "7";
        //                            if (res.getCellData(i, colProductImg) != "")
        //                            {
        //                                dRow["STRING_VALUE"] = res.getCellData(i, colProductImg).Substring(42).ToString();
        //                            }
        //                            else
        //                            {
        //                                dRow["STRING_VALUE"] = @"\NoImage.gif";
        //                            }
        //                            dRow["NUMERIC_VALUE"] = "";
        //                            dRow["ATTRIBUTE_NAME"] = "TWEB_IMAGE123";
        //                            dRow["ATTRIBUTE_TYPE"] = "3";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                        }
        //                        Sub_Family_Products.Rows.Add(dRow);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return ProductSearch;
        //}

        //private DataSet GetSubfamilyDetails(INavigateResults res)
        //{
        //    int last = res.getLastItem();
        //    int first = res.getFirstItem();
        //    int colFmlyID = res.getColumnIndex("Family Id");
        //    int colFmlyName = res.getColumnIndex("Family Name");
        //    int colFmlyDesc = res.getColumnIndex("Family Description");//ShortDescription
        //    int colFmlyImg = res.getColumnIndex("Family Thumbnail");

        //    int subFmlyID = res.getColumnIndex("SubFamily Id");
        //    int subFmlyName = res.getColumnIndex("SubFamily Name");
        //    int subFmlyDesc = res.getColumnIndex("SubFamily Description");
        //    int subFmlyImg = res.getColumnIndex("SubFamily Thumbnail");

        //    int colProductID = res.getColumnIndex("Prod Id");
        //    int colProductCode = res.getColumnIndex("Prod Code");
        //    int colProductPrice = res.getColumnIndex("Price");
        //    int colProductDesc = res.getColumnIndex("Prod Description");
        //    int colProductImg = res.getColumnIndex("Prod Thumbnail");

        //    int colProductCount = res.getColumnIndex("Prod Count");
        //    int colFamilyProdCount = res.getColumnIndex("Family Prod Count");

        //    DataRow dr;
        //    try
        //    {
        //        if (last >= 0)
        //        {
        //            for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++) //For Product details
        //            {
        //                if (res.getCellData(i, subFmlyID) != "" && res.getCellData(i, subFmlyName) != "")
        //                {
        //                    //For Sub Family Details
        //                    for (int k = 0; k < 3; k++)
        //                    {
        //                        dr = Sub_Family_Details.NewRow();
        //                        if (k == 0)
        //                        {
        //                            dr["FAMILY_ID"] = res.getCellData(i, subFmlyID);
        //                            dr["FAMILY_NAME"] = res.getCellData(i, subFmlyName);
        //                            dr["STRING_VALUE"] = "";
        //                            dr["NUMERIC_VALUE"] = "";
        //                            dr["ATTRIBUTE_ID"] = "";
        //                            dr["ATTRIBUTE_NAME"] = "";
        //                            dr["ATTRIBUTE_DATATYPE"] = "";
        //                            dr["ATTRIBUTE_TYPE"] = "";
        //                        }
        //                        if (k == 1)
        //                        {
        //                            dr["FAMILY_ID"] = res.getCellData(i, subFmlyID);
        //                            dr["FAMILY_NAME"] = res.getCellData(i, subFmlyName);
        //                            dr["STRING_VALUE"] = res.getCellData(i, subFmlyDesc);
        //                            //dr["STRING_VALUE"] = res.getCellData(i, subFmlyDesc) + "" + "Testing";
        //                            dr["NUMERIC_VALUE"] = "";
        //                            dr["ATTRIBUTE_ID"] = "329";
        //                            dr["ATTRIBUTE_NAME"] = "Descriptions1";
        //                            dr["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                            dr["ATTRIBUTE_TYPE"] = "7";
        //                        }
        //                        if (k == 2)
        //                        {
        //                            dr["FAMILY_ID"] = res.getCellData(i, subFmlyID);
        //                            dr["FAMILY_NAME"] = res.getCellData(i, subFmlyName);
        //                            if (res.getCellData(i, subFmlyImg) != "")
        //                            {
        //                                dr["STRING_VALUE"] = res.getCellData(i, subFmlyImg).Substring(42).ToString();
        //                            }
        //                            else
        //                            {
        //                                dr["STRING_VALUE"] = @"\NoImage.gif";
        //                            }
        //                            dr["NUMERIC_VALUE"] = "";
        //                            dr["ATTRIBUTE_ID"] = "747";
        //                            dr["ATTRIBUTE_NAME"] = "TFWeb Image1";
        //                            dr["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                            dr["ATTRIBUTE_TYPE"] = "9";
        //                        }
        //                        Sub_Family_Details.Rows.Add(dr);
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    if (Sub_Family_DS.Tables[0].Rows.Count > 0)
        //    {
        //        return Sub_Family_DS;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //#endregion

        //#region "For the Attributes"

        //public DataSet Get_UserSearch_Attr(string srctxt, string attribute_Type, string attribute_value, int curPage, string pageOp, string rpp)
        //{
        //    try
        //    {
        //      //   objErrorhandler._LogMsg =  Environment.NewLine + "Before EasyAsk Result -" + DateTime.Now.ToLongTimeString();
        //       //  objErrorhandler.CreateLogTest();
        //     //   IRemoteEasyAsk ea = getRemote();
        //      //  RemoteEasyAsk();
        //      //  ea.setReturnSKUS(false);
        //      //  ea.setResultsPerPage(rpp);
        //        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
        //        IOptions opts = ea.getOptions();
        //        opts.setResultsPerPage(m_rpp); // ea_rpp.Value);   // use current settings
        //        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        //        opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
        //        opts.setSubCategories(false);
        //        opts.setNavigateHierarchy(false);
        //        opts.setReturnSKUs(false);
        //        Create_Search_Table_Columns();
        //        DataSet ds = new DataSet();
        //        INavigateResults res = null;
        //        if (curPage < 1)
        //        {
        //            if (HttpContext.Current.Request["pgno"] == null)
        //            {
        //                res = ea.userSearch("AllProducts////WESAUSTRALASIA////Cellular Accessories", srctxt);
        //                updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //            }
        //            else
        //            {
        //                res = ea.userSearch(HttpContext.Current.Session["EA"].ToString(), "");
        //                updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //            }
        //            HttpContext.Current.Session["EA"] = res.getCatPath();
        //            Get_DataSet_Values(res);//For Main Content Values.

        //            IList<String> attrNamesFull = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //            EasyAsk_UserSearch_Attributes_DS(attrNamesFull, res, attribute_value);

        //            updateCommentary(res);
        //           //  objErrorhandler._LogMsg = Environment.NewLine + "After EasyAsk Result -"+ DateTime.Now.ToLongTimeString();
        //             //  objErrorhandler.CreateLogTest();
        //        }
        //        else
        //        {
        //            res = ea.userPageOp(HttpContext.Current.Session["EA"].ToString(), curPage.ToString(), pageOp);
        //            ds = Get_DataSet_Values(res);
        //            updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //            HttpContext.Current.Session["EA"] = res.getCatPath();
        //            //   HttpContext.Current.Response.Write("<script>   function addZero(i) { if (i < 10) { i = '0' + i;} return i;}  var date = new Date(); var h = addZero(date.getHours()); var m = addZero(date.getMinutes()); var s = addZero(date.getSeconds()); alert('After EasyAsk Result: '+h + ':' + m + ':' + s);</script>"); 
        //            //Get_DataSet_Values(res1);
        //            //IList Attributes = res1.getAttributeNames();
        //            //EasyAsk_UserSearch_Attributes_DS(Attributes, res1, attribute_value);
        //        }
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorhandler.ErrorMsg = ex;
        //        objErrorhandler.CreateLog();
        //        return null;
        //    }
        //}

        ////public DataSet Get_Brand_Product(string srctxt, string attribute_Type, string attribute_value, int curPage, string pageOp, bool SKUS)
        ////{
        ////    try
        ////    {
        ////       // IRemoteEasyAsk ea = getRemote();
        ////       // RemoteEasyAsk();
        ////       // ea.setReturnSKUS(SKUS);
        ////        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
        ////        IOptions opts = ea.getOptions();
        ////        opts.setResultsPerPage(m_rpp); // ea_rpp.Value);   // use current settings
        ////        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        ////        opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
        ////        opts.setSubCategories(false);
        ////        opts.setNavigateHierarchy(false);
        ////        opts.setReturnSKUs(SKUS);
        ////        DataSet ds = new DataSet();
        ////        DataSet Family = new DataSet();
        ////        INavigateResults res = null;
        ////        Create_Search_Table_Columns();
        ////        Create_Product_Table_Columns();
        ////        Create_SubFamily_Table_Columns();
        ////        if (curPage < 1)
        ////        {
        ////            string temp = "";
        ////            string temp1 = "";
        ////            if (attribute_Type.Contains("Category"))
        ////            {
        ////                //res = ea.userBreadCrumbClick("AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch1=" + srctxt + "////" + attribute_value.Replace("-", "&") + "");
        ////                if (HttpContext.Current.Session["EA"] != null)
        ////                {
        ////                    string s = HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString());
        ////                    res = ea.userBreadCrumbClick1(s + "////" + attribute_value + "");
        ////                    HttpContext.Current.Session["EA"] = res.getCatPath();
        ////                    updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        ////                }
        ////            }
        ////            else
        ////            {
        ////                if (attribute_Type == "Model")
        ////                {
        ////                    string brand = HttpContext.Current.Session["Brand"].ToString();
        ////                    temp = "" + attribute_Type + " = '" + brand + ":" + attribute_value + "'";//For Apple:Ipad2...
        ////                    temp1 = "" + attribute_Type + " = ";
        ////                }
        ////                else
        ////                {
        ////                    temp = "" + attribute_Type + " = '" + attribute_value + "'";//For default value....
        ////                    temp1 = "" + attribute_Type + " = ";
        ////                }
        ////                if (HttpContext.Current.Session["EA"] != null)
        ////                {
        ////                    if (HttpContext.Current.Session["EA"].ToString().Contains(temp1))
        ////                    {
        ////                        int t = HttpContext.Current.Session["EA"].ToString().IndexOf(temp1) - 17;
        ////                        string t1 = HttpContext.Current.Session["EA"].ToString().Remove(t);
        ////                        HttpContext.Current.Session["EA"] = t1;

        ////                    }
        ////                    //res = ea.attrClick("AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch1=" + srctxt + "", temp);
        ////                    res = ea.userAttributeClick(HttpContext.Current.Session["EA"].ToString(), temp);
        ////                    // res = ea.attrClick("AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch="+srctxt+"////UserSearch1=" + HttpContext.Current.Request.QueryString["fid"] + "", temp);
        ////                    HttpContext.Current.Session["EA"] = res.getCatPath();
        ////                    updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        ////                }
        ////            }
        ////            ds = Get_DataSet_Values(res);
        ////            Family = GetProductDetails(res);
        ////            DataSet subfamily_ds = GetSubfamilyDetails(res);
        ////            IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        ////            EasyAsk_UserSearch_Attributes_DS(Attributes, res, attribute_value);
        ////            HttpContext.Current.Session["Sub_Family"] = subfamily_ds;

        ////        }
        ////        else
        ////        {
        ////            //AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch=Audio////AttribSelect=Brand = 'Motorola'
        ////            //INavigateResults res1 = ea.userPageOp("AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch1=" + srctxt + "////AttribSelect=" + attribute_value + "", curPage.ToString(), pageOp);
        ////            if (HttpContext.Current.Session["EA"] != null)
        ////            {
        ////                INavigateResults res1 = ea.userPageOp(HttpContext.Current.Session["EA"].ToString(), curPage.ToString(), pageOp);
        ////                ds = Get_DataSet_Values(res1);
        ////                //  updateBreadCrumb(res1.getBreadCrumbTrail());
        ////            }
        ////            //IList Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        ////            //EasyAsk_UserSearch_Attributes_DS(Attributes, res, attribute_value);
        ////        }
        ////        HttpContext.Current.Session["Family"] = Family;
        ////        return ds;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        return null;
        ////    }
        ////}

        ////public DataSet Get_Category_UserSearch(string srctxt, string Category, int curPage, string pageOp)
        ////{
        ////    try
        ////    {
        ////        RemoteEasyAsk();

        ////        DataSet ds = new DataSet();
        ////        Create_Search_Table_Columns();

        ////        if (curPage < 1)
        ////        {
        ////            INavigateResults res = ea.userBreadCrumbClick("AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch1=" + srctxt + "////" + Category);
        ////            ds = Get_DataSet_Values(res);
        ////        }
        ////        else
        ////        {
        ////            INavigateResults res1 = ea.userPageOp("AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch1=" + srctxt + "////" + Category, curPage.ToString(), pageOp);
        ////            ds = Get_DataSet_Values(res1);
        ////        }
        ////        return ds;
        ////    }
        ////    catch (Exception)
        ////    {
        ////        return null;
        ////    }
        ////}
        ////created by boopathi&sengottuvel.


        //DataSet EA_UserSearch_Attributes_DS = new DataSet();
        //DataSet EA_UserSearch_Attributes_DS_Full = new DataSet();
        //void EasyAsk_UserSearch_Attributes_DS(IList<string> Attributes, INavigateResults res, string temptext)
        //{
        //    int s = 0;
        //    try
        //    {
        //        if (s == 0)
        //        {
        //            EA_UserSearch_Attributes_DS_Full.Tables.Add("Category");
        //            EA_UserSearch_Attributes_DS_Full.Tables["Category"].Columns.Add("CATEGORY_ID", typeof(string));
        //            EA_UserSearch_Attributes_DS_Full.Tables["Category"].Columns.Add("Category_Name", typeof(string));
        //            EA_UserSearch_Attributes_DS_Full.Tables["Category"].Columns.Add("Product_Count", typeof(int));
        //            EA_UserSearch_Attributes_DS.Tables.Add("Category");
        //            EA_UserSearch_Attributes_DS.Tables["Category"].Columns.Add("CATEGORY_ID", typeof(string));
        //            EA_UserSearch_Attributes_DS.Tables["Category"].Columns.Add("Category_Name", typeof(string));
        //            EA_UserSearch_Attributes_DS.Tables["Category"].Columns.Add("Product_Count", typeof(int));
        //            s++;
        //        }
        //        IList<INavigateCategory> category = null;

        //        if (res.getDetailedCategories(EasyAskConstants.ATTR_DISPLAY_MODE_INITIAL) != null)
        //        {
        //            category = res.getDetailedCategories();
        //        }
        //        else
        //        {
        //            category = res.getDetailedCategories();
        //        }
        //        if (category.Count > 0) //For Searching Category Values
        //        {
        //            //EA_UserSearch_Attributes_DS.Tables.Add("Category");
        //            //EA_UserSearch_Attributes_DS.Tables["Category"].Columns.Add("CATEGORY_ID", typeof(string));
        //            //EA_UserSearch_Attributes_DS.Tables["Category"].Columns.Add("Category_Name", typeof(string));
        //            //EA_UserSearch_Attributes_DS.Tables["Category"].Columns.Add("Product_Count", typeof(int));
        //            foreach (INavigateCategory categoryItem in category)
        //            {
        //                DataRow row = EA_UserSearch_Attributes_DS.Tables["Category"].NewRow();
        //                IList<string> Id = categoryItem.getIDs();
        //                row["CATEGORY_ID"] = Id[0].ToString().Substring(2);
        //                row["Category_Name"] = categoryItem.getName();
        //                row["Product_Count"] = categoryItem.getProductCount();
        //                EA_UserSearch_Attributes_DS.Tables["Category"].Rows.Add(row);
        //            }
        //        }
        //        if (res.getDetailedCategories(EasyAskConstants.ATTR_DISPLAY_MODE_INITIAL) != null)
        //        {
        //            IList<INavigateCategory> category1 = res.getDetailedCategories();
        //            if (res.getDetailedCategories().Count > 0) //For Searching Category Values
        //            {
        //                //foreach (INavigateCategory categoryItem in category1)
        //                //{
        //                //    DataRow row = EA_UserSearch_Attributes_DS_Full.Tables["Category"].NewRow();
        //                //    IList<string> Id = categoryItem.getIDs();
        //                //    row["CATEGORY_ID"] = Id[0].ToString().Substring(2);
        //                //    row["Category_Name"] = categoryItem.getName();
        //                //    row["Product_Count"] = categoryItem.getProductCount();
        //                //    EA_UserSearch_Attributes_DS_Full.Tables["Category"].Rows.Add(row);
        //                //}
        //            }
        //        }

        //        int temp_Count;
        //        if (EA_UserSearch_Attributes_DS.Tables.Count == 0)
        //        {
        //            temp_Count = 0;
        //        }
        //        else
        //        {
        //            temp_Count = 1;
        //        }

        //        for (int i = 0; i < Attributes.Count; i++)
        //        {
        //            String attrName = (String)Attributes[i];

        //            if (!attrName.Contains("Long Description")) //For do not display Long Description
        //            {
        //                EA_UserSearch_Attributes_DS.Tables.Add(attrName);
        //                EA_UserSearch_Attributes_DS.Tables[temp_Count].Columns.Add(attrName, typeof(string));
        //                EA_UserSearch_Attributes_DS.Tables[temp_Count].Columns.Add("Product_Count", typeof(int));

        //                IList<INavigateAttribute> AttributeValue = res.getDetailedAttributeValues(attrName, EasyAskConstants.ATTR_DISPLAY_MODE_INITIAL);
        //                foreach (INavigateAttribute AttributeItem in AttributeValue)
        //                {
        //                    DataRow row = EA_UserSearch_Attributes_DS.Tables[temp_Count].NewRow();
        //                    if (attrName.Equals("Model"))//For Model Name will split
        //                    {
        //                        if (HttpContext.Current.Session["Brand"] != null)
        //                        {
        //                            temptext = HttpContext.Current.Session["Brand"].ToString();
        //                        }
        //                        else
        //                        {
        //                            temptext = HttpUtility.UrlDecode(temptext);
        //                        }
        //                        if (AttributeItem.getValue().Contains(temptext))
        //                        {
        //                            string[] model = AttributeItem.getValue().Split(':');
        //                            if (model[0].ToString().Contains(temptext))
        //                            {
        //                                if (temptext == "" && HttpContext.Current.Session["Brand"] == null || HttpContext.Current.Session["Brand"] == string.Empty)
        //                                {
        //                                    HttpContext.Current.Session["Brand"] = model[0].ToString();
        //                                    temptext = model[0].ToString();
        //                                }
        //                                row[0] = model[1].ToString();
        //                                row[1] = AttributeItem.getProductCount();
        //                                EA_UserSearch_Attributes_DS.Tables[temp_Count].Rows.Add(row);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        row[0] = AttributeItem.getValue();
        //                        row[1] = AttributeItem.getProductCount();
        //                        EA_UserSearch_Attributes_DS.Tables[temp_Count].Rows.Add(row);
        //                    }
        //                }
        //                temp_Count++;
        //            }

        //        }

        //        //modification for show only four models
        //        //
        //        if (EA_UserSearch_Attributes_DS.Tables.Count > 1)
        //        {
        //            for (int k = 1; k < EA_UserSearch_Attributes_DS.Tables.Count; k++)
        //            {
        //                if (EA_UserSearch_Attributes_DS.Tables[k].TableName == "Model")
        //                {
        //                    if (EA_UserSearch_Attributes_DS.Tables["Model"].Rows.Count == 0)
        //                    {
        //                        IList<INavigateAttribute> AttributeValue = res.getDetailedAttributeValues("Model", EasyAskConstants.ATTR_DISPLAY_MODE_FULL);

        //                        foreach (INavigateAttribute AttributeItem in AttributeValue)
        //                        {
        //                            if (EA_UserSearch_Attributes_DS.Tables["Model"].Rows.Count < 4)
        //                            {
        //                                DataRow row = EA_UserSearch_Attributes_DS.Tables["Model"].NewRow();
        //                                // if (attrName.Equals("Model"))//For Model Name will split
        //                                //  {
        //                                string[] model = AttributeItem.getValue().Split(':');
        //                                if (model[0].ToString().Contains(temptext))
        //                                {//problem here

        //                                    if (HttpContext.Current.Session["Brand"] == null || HttpContext.Current.Session["Brand"] == string.Empty)
        //                                    {
        //                                        HttpContext.Current.Session["Brand"] = model[0].ToString();
        //                                    }
        //                                    row[0] = model[1].ToString();
        //                                    row[1] = AttributeItem.getProductCount();
        //                                    EA_UserSearch_Attributes_DS.Tables["Model"].Rows.Add(row);
        //                                }
        //                                // }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        //
        //        //end

        //        int temp_Count1;
        //        if (EA_UserSearch_Attributes_DS_Full.Tables.Count == 0)
        //        {
        //            temp_Count1 = 0;
        //        }
        //        else
        //        {
        //            temp_Count1 = 1;
        //        }
        //        for (int j = 0; j < Attributes.Count; j++)
        //        {
        //            String attrName1 = (String)Attributes[j];

        //            if (!attrName1.Contains("Long Description")) //For do not display Long Description
        //            {
        //                EA_UserSearch_Attributes_DS_Full.Tables.Add(attrName1);
        //                EA_UserSearch_Attributes_DS_Full.Tables[temp_Count1].Columns.Add(attrName1, typeof(string));
        //                EA_UserSearch_Attributes_DS_Full.Tables[temp_Count1].Columns.Add("Product_Count", typeof(int));
        //                Boolean limited = res.isInitialDispLimitedForAttrValues(attrName1);
        //                if (limited)
        //                {
        //                    IList<INavigateAttribute> AttributeValue = res.getDetailedAttributeValues(attrName1, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //                    foreach (INavigateAttribute AttributeItem in AttributeValue)
        //                    {
        //                        DataRow row = EA_UserSearch_Attributes_DS_Full.Tables[temp_Count1].NewRow();
        //                        if (attrName1.Equals("Model"))//For Model Name will split
        //                        {
        //                            string[] model = AttributeItem.getValue().Split(':');
        //                            if (model[0].ToString().Contains(temptext))
        //                            {//problem here

        //                                if (HttpContext.Current.Session["Brand"] == null || HttpContext.Current.Session["Brand"] == string.Empty)
        //                                {
        //                                    HttpContext.Current.Session["Brand"] = model[0].ToString();
        //                                }
        //                                row[0] = model[1].ToString();
        //                                row[1] = AttributeItem.getProductCount();
        //                                EA_UserSearch_Attributes_DS_Full.Tables[temp_Count1].Rows.Add(row);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            row[0] = AttributeItem.getValue();
        //                            row[1] = AttributeItem.getProductCount();
        //                            EA_UserSearch_Attributes_DS_Full.Tables[temp_Count1].Rows.Add(row);
        //                        }
        //                    }
        //                }
        //                else
        //                {

        //                }
        //                temp_Count1++;
        //            }
        //        }
        //        if (EA_UserSearch_Attributes_DS.Tables.Count > 1)
        //        {
        //            for (int k = 1; k < EA_UserSearch_Attributes_DS.Tables.Count; k++)
        //            {
        //                if (EA_UserSearch_Attributes_DS.Tables[k].TableName == "Model")
        //                {
        //                    if (EA_UserSearch_Attributes_DS.Tables["Model"].Rows.Count == EA_UserSearch_Attributes_DS_Full.Tables["Model"].Rows.Count)
        //                    {
        //                            EA_UserSearch_Attributes_DS_Full.Tables["Model"].Rows.Clear();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    //return EA_UserSearch_Attributes_DS;
        //    HttpContext.Current.Session["Search_Attributes_Full"] = EA_UserSearch_Attributes_DS_Full;
        //    HttpContext.Current.Session["Search_Attributes"] = EA_UserSearch_Attributes_DS;
        //}


        ////Modified New 
        //public void BreadCrumbClick(string rpp, string attribute_value, string familyshow, string familyid, string count)
        //{
        //    try
        //    {
        //       // IRemoteEasyAsk ea = getRemote();
        //       // RemoteEasyAsk();
        //        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
        //        IOptions opts = ea.getOptions();
        //        opts.setResultsPerPage(m_rpp); // ea_rpp.Value);   // use current settings
        //        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        //        opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
        //        opts.setSubCategories(false);
        //        opts.setNavigateHierarchy(false);
        //        opts.setReturnSKUs(false);
        //        INavigateResults res = null;
        //        DataSet ds = new DataSet();
        //        DataSet subfamily_ds = new DataSet();
        //        Create_Product_Table_Columns();
        //        Create_SubFamily_Table_Columns();
        //       // ea.setReturnSKUS(true);
        //        //if (familyshow == "1")
        //        //{
        //       // ea.setResultsPerPage("0");
        //        INavigateResults res1 = ea.userBreadCrumbClick(HttpContext.Current.Session["EA"].ToString());
        //        ds = GetProductDetails(res1);
        //        subfamily_ds = GetSubfamilyDetails(res1);

        //        //For Leftnavigation New UI
        //        IList<string> Attributes = res1.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //        EasyAsk_UserSearch_Attributes_DS(Attributes, res1, attribute_value);

        //        //Store Family details in session
        //        HttpContext.Current.Session["Family"] = ds;
        //        HttpContext.Current.Session["Sub_Family"] = subfamily_ds;
        //        HttpContext.Current.Session["EA"] = res1.getCatPath();
        //        updateBreadCrumb(res1.getBreadCrumbTrail(), familyshow, familyid);
        //        //}
        //        //else
        //        //{
        //        //    ea.setResultsPerPage("0");
        //        //    res = ea.userBreadCrumbClick(HttpContext.Current.Session["EA"].ToString());
        //        //    ds = GetProductDetails(res);
        //        //    subfamily_ds = GetSubfamilyDetails(res);

        //        //    //For Leftnavigation New UI
        //        //    IList Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //        //    EasyAsk_UserSearch_Attributes_DS(Attributes, res, attribute_value);

        //        //    //Store Family details in session
        //        //    HttpContext.Current.Session["Family"] = ds;
        //        //    HttpContext.Current.Session["Sub_Family"] = subfamily_ds;
        //        //    HttpContext.Current.Session["EA"] = res.getCatPath();
        //        //    //  string SEO_PATH = "";
        //        //    updateBreadCrumb(res.getBreadCrumbTrail(), familyshow, familyid);
        //        //}

        //    }
        //    catch (Exception ex)
        //    {
        //        // return null;
        //    }
        //}

        //public void BreadCrumbClick(string rpp, string attribute_value, int curPage, string pageOp)
        //{
        //    try
        //    {
        //        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
        //        IOptions opts = ea.getOptions();
        //        opts.setResultsPerPage(rpp); // ea_rpp.Value);   // use current settings
        //        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        //        opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
        //        opts.setSubCategories(false);
        //        opts.setNavigateHierarchy(false);
        //        opts.setReturnSKUs(false);
        //       // ea.setResultsPerPage(rpp);
        //        Create_Search_Table_Columns();
        //        DataSet ds = new DataSet();
        //        if (curPage < 1)
        //        {
        //            INavigateResults res = ea.userBreadCrumbClick(HttpContext.Current.Session["EA"].ToString());
        //            if (HttpContext.Current.Request.Url.OriginalString.Contains("categorylist.aspx"))
        //            {
        //                Create_Menu_Table_Columns();
        //                updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //                IList<INavigateCategory> list = res.getDetailedCategories();

        //                foreach (INavigateCategory item in list)
        //                {
        //                    DataRow row = Mnu_Temp.NewRow();
        //                    IList<string> li = item.getIDs();
        //                    row["category_id"] = li[0].ToString().Substring(2);
        //                    row["category_name"] = item.getName();
        //                    row["image_file"] = "NULL";
        //                    row["custom_num_field3"] = "2";
        //                    row["parent_category"] = "";
        //                    Mnu_Temp.Rows.Add(row);
        //                }
        //                Get_FamilyDS_Values(res);

        //                if (Menu_Click.Tables.Count > 0)
        //                {
        //                    HttpContext.Current.Session["Click_Menu_Results"] = Menu_Click;
        //                }

        //            }
        //            else if (HttpContext.Current.Request.Url.OriginalString.Contains("product_list.aspx"))
        //            {
        //                Create_Menu_Table_Columns();
        //                updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //                IList<INavigateCategory> list = res.getDetailedCategories();

        //                foreach (INavigateCategory item in list)
        //                {
        //                    DataRow row = Mnu_Temp.NewRow();
        //                    IList<string> li = item.getIDs();
        //                    row["category_id"] = li[0].ToString().Substring(2);
        //                    row["category_name"] = item.getName();
        //                    row["image_file"] = "NULL";
        //                    row["custom_num_field3"] = "2";
        //                    row["parent_category"] = "";
        //                    Mnu_Temp.Rows.Add(row);
        //                }
        //                Get_FamilyDS_Values(res);
        //                if (Menu_Click.Tables.Count > 0)
        //                {
        //                    HttpContext.Current.Session["Click_Menu_Results"] = Menu_Click;
        //                }
        //            }
        //            else
        //            {
        //                updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //                Get_DataSet_Values(res);//For Main Content Values.
        //            }
        //            HttpContext.Current.Session["EA"] = res.getCatPath();
        //            //Get_DataSet_Values(res);//For Main Content Values.
        //            IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //            EasyAsk_UserSearch_Attributes_DS(Attributes, res, attribute_value);
        //        }
        //        else
        //        {
        //            INavigateResults res = ea.userPageOp(HttpContext.Current.Session["EA"].ToString(), curPage.ToString(), pageOp);
        //            if (HttpContext.Current.Request.Url.OriginalString.Contains("categorylist.aspx"))
        //            {
        //                Create_Menu_Table_Columns();
        //                updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //                IList<INavigateCategory> list = res.getDetailedCategories();

        //                foreach (INavigateCategory item in list)
        //                {
        //                    DataRow row = Mnu_Temp.NewRow();
        //                    IList<string> li = item.getIDs();
        //                    row["category_id"] = li[0].ToString().Substring(2);
        //                    row["category_name"] = item.getName();
        //                    row["image_file"] = "NULL";
        //                    row["custom_num_field3"] = "2";
        //                    row["parent_category"] = "";
        //                    Mnu_Temp.Rows.Add(row);
        //                }
        //                Get_FamilyDS_Values(res);

        //                if (Menu_Click.Tables.Count > 0)
        //                {
        //                    HttpContext.Current.Session["Click_Menu_Results"] = Menu_Click;
        //                }
        //            }
        //            else if (HttpContext.Current.Request.Url.OriginalString.Contains("product_list.aspx"))
        //            {
        //                Create_Menu_Table_Columns();
        //                updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //                IList<INavigateCategory> list = res.getDetailedCategories();

        //                foreach (INavigateCategory item in list)
        //                {
        //                    DataRow row = Mnu_Temp.NewRow();
        //                    IList<string> li = item.getIDs();
        //                    row["category_id"] = li[0].ToString().Substring(2);
        //                    row["category_name"] = item.getName();
        //                    row["image_file"] = "NULL";
        //                    row["custom_num_field3"] = "2";
        //                    row["parent_category"] = "";
        //                    Mnu_Temp.Rows.Add(row);
        //                }
        //                Get_FamilyDS_Values(res);
        //                if (Menu_Click.Tables.Count > 0)
        //                {
        //                    HttpContext.Current.Session["Click_Menu_Results"] = Menu_Click;
        //                }
        //            }
        //            else
        //            {
        //                Get_DataSet_Values(res);//For Main Content Values.
        //                updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //            }
        //            //  return ds;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //   return null;
        //    }
        //}

        //protected void updateBreadCrumb(IBreadCrumbTrail bct, string familyshow, string familyid)
        //{
        //    try
        //    {
        //        HttpContext.Current.Session["Link"] = null;
        //        HttpContext.Current.Session["Selected"] = null;
        //        HttpContext.Current.Session["BreadCrumbName"] = null;
        //        string separator = "";
        //        List<String> Htmlitems1 = new List<String>();
        //        String htmlItems = "";
        //        String href = "";
        //        string GetValue = "";
        //        foreach (INavigateNode node in bct.getSearchPath())
        //        {
        //          // string separator1= node.getSEOPath();
        //            IList<INavigateNode> nodes = bct.getSearchPath();

        //            string separator1 = node.getPath();
        //            String label = node.getLabel();
        //            if (node.getLabel() == "Model")
        //            {
        //                string[] ModelName = node.getValue().Split(':');
        //                GetValue = ModelName[1].ToString();
        //            }
        //            else
        //            {
        //                GetValue = node.getValue();
        //            }
        //            if (node.getPath().Contains("AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch1=") || node.getPath().Contains("AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch="))
        //            {
        //                if (!node.getPath().Contains("Family Id="))
        //                {
        //                    separator = "&nbsp;/&nbsp;";
        //                    HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"ps.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                    href = "<a href=\"ps.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                    if (HttpContext.Current.Session["Link"] == null)
        //                    {
        //                        HttpContext.Current.Session["Link"] = "<a href=\"ps.aspx?&amp;Path=" + HttpUtility.UrlEncode("ppaNFtmS8Au7qIvaOCRHUp5RGlmGw65lKAOdRc+AWE7wD1EsnO+ebUWpKbZWV/Nuik1daBT3bK1yvp40MZ9Cig==") + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                    }
        //                    else
        //                    {
        //                        //    HttpContext.Current.Session["Link"] = "<a href=\"ps.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                        //    href = "<a href=\"ps.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                    }
        //                    htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                    HttpContext.Current.Session["Link"] = "<a href=\"ps.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                    Htmlitems1.Add(htmlItems);
        //                }
        //                else
        //                {
        //                    string BreadCrumb_replace = node.getPath().Replace("Family Id=", "#");
        //                    string[] BreadCrumb_Name = BreadCrumb_replace.Split('#');
        //                    if (!BreadCrumb_Name[1].Contains("AttribSelect="))
        //                    {
        //                        DataSet d = (DataSet)HttpContext.Current.Session["Family"];
        //                        string path_value = d.Tables[0].Rows[0][1].ToString();
        //                        string Family_Id = d.Tables[0].Rows[0][0].ToString();
        //                        separator = "&nbsp;/&nbsp;";
        //                        if (HttpContext.Current.Request.QueryString["fcnt"] != null)
        //                        {
        //                            HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">" + path_value + "</a>";
        //                            href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                            if (HttpContext.Current.Session["Link"] == null)
        //                            {
        //                                HttpContext.Current.Session["Link"] = "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode("ppaNFtmS8Au7qIvaOCRHUp5RGlmGw65lKAOdRc+AWE7wD1EsnO+ebUWpKbZWV/Nuik1daBT3bK1yvp40MZ9Cig==") + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            }
        //                            htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV("Family Name", path_value) + "</a></td></tr></table></li>";
        //                            HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                            Htmlitems1.Add(htmlItems);
        //                        }
        //                        else
        //                        {
        //                            HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">" + path_value + "</a>";
        //                            href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                            if (HttpContext.Current.Session["Link"] == null)
        //                            {
        //                                HttpContext.Current.Session["Link"] = "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode("ppaNFtmS8Au7qIvaOCRHUp5RGlmGw65lKAOdRc+AWE7wD1EsnO+ebUWpKbZWV/Nuik1daBT3bK1yvp40MZ9Cig==") + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                                // href = "<a href=\"ps.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            }
        //                            htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV("Family Name", path_value) + "</a></td></tr></table></li>";
        //                            HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                            Htmlitems1.Add(htmlItems);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        DataSet d = (DataSet)HttpContext.Current.Session["Family"];
        //                        string Family_Id = d.Tables[0].Rows[0][0].ToString();
        //                        separator = "&nbsp;/&nbsp;";
        //                        if (HttpContext.Current.Request.QueryString["fcnt"] != null)
        //                        {
        //                            HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                            href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                            HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            Htmlitems1.Add(htmlItems);
        //                        }
        //                        else
        //                        {
        //                            HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                            href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                            HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            Htmlitems1.Add(htmlItems);
        //                        }
        //                    }

        //                }
        //                //HttpContext.Current.Session["BreadCrumbCount"] = i + 1;
        //            }
        //            else if (node.getPath().Contains("AllProducts////WESAUSTRALASIA////Cellular Accessories////"))
        //            {
        //                string Category_breadcrumb = node.getPath().Replace("////", "#");
        //                string[] Category_breadcrumb_name = Category_breadcrumb.Split('#');
        //                int count_breadcrumb = Category_breadcrumb_name.Count();
        //                if (count_breadcrumb == 4)
        //                {
        //                    if (HttpContext.Current.Request.Url.OriginalString.Contains("categorylist.aspx"))
        //                    {
        //                        separator = "&nbsp;/&nbsp;";
        //                        HttpContext.Current.Session["BreadCrumbName"] = separator + "<a href=\"categorylist.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                        if (HttpContext.Current.Session["Link"] == null)
        //                        {
        //                            HttpContext.Current.Session["Link"] = "<a href=\"categorylist.aspx?&amp;Path=" + HttpUtility.UrlEncode("ppaNFtmS8Au7qIvaOCRHUp5RGlmGw65lKAOdRc+AWE7wD1EsnO+ebUWpKbZWV/Nuik1daBT3bK1yvp40MZ9Cig==") + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            // href = "<a href=\"ps.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                        }
        //                        href = "<a href=\"categorylist.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                        htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                        HttpContext.Current.Session["Link"] = "<a href=\"categorylist.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                        Htmlitems1.Add(htmlItems);
        //                    }
        //                    else
        //                    {
        //                        separator = "&nbsp;/&nbsp;";
        //                        HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                        if (HttpContext.Current.Session["Link"] == null)
        //                        {
        //                            HttpContext.Current.Session["Link"] = "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode("ppaNFtmS8Au7qIvaOCRHUp5RGlmGw65lKAOdRc+AWE7wD1EsnO+ebUWpKbZWV/Nuik1daBT3bK1yvp40MZ9Cig==") + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            // href = "<a href=\"ps.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                        }
        //                        href = "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                        htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                        HttpContext.Current.Session["Link"] = "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">"; 
        //                        Htmlitems1.Add(htmlItems);
        //                    }
        //                }
        //                else
        //                {
        //                    if (!node.getPath().Contains("AttribSelect="))
        //                    {
        //                        if (!node.getPath().Contains("Family Id="))
        //                        {
        //                            //if (HttpContext.Current.Request.QueryString["type"] == null)
        //                            //{
        //                            separator = "&nbsp;/&nbsp;";
        //                            HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                            if (HttpContext.Current.Session["Link"] == null)
        //                            {
        //                                HttpContext.Current.Session["Link"] = "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode("ppaNFtmS8Au7qIvaOCRHUp5RGlmGw65lKAOdRc+AWE7wD1EsnO+ebUWpKbZWV/Nuik1daBT3bK1yvp40MZ9Cig==") + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                                // href = "<a href=\"ps.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            }
        //                            href = "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                            HttpContext.Current.Session["Link"] = "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            Htmlitems1.Add(htmlItems);
        //                        }
        //                        else
        //                        {
        //                            DataSet d = (DataSet)HttpContext.Current.Session["Family"];
        //                            string path_value = d.Tables[0].Rows[0][1].ToString();
        //                            string Family_Id = d.Tables[0].Rows[0][0].ToString();
        //                            separator = "&nbsp;/&nbsp;";
        //                            if (HttpContext.Current.Request.QueryString["fcnt"] != null)
        //                            {
        //                                HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">" + path_value + "</a>";
        //                                href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                                htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV("Family Name", path_value) + "</a></td></tr></table></li>";
        //                                HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                                Htmlitems1.Add(htmlItems);
        //                            }
        //                            else
        //                            {
        //                                HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">" + path_value + "</a>";
        //                                href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                                htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV("Family Name", path_value) + "</a></td></tr></table></li>";
        //                                HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                                Htmlitems1.Add(htmlItems);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (!node.getPath().Contains("Family Id="))
        //                        {
        //                            separator = "&nbsp;/&nbsp;";
        //                            HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"ps.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                            href = "<a href=\"ps.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                            HttpContext.Current.Session["Link"] = "<a href=\"ps.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            Htmlitems1.Add(htmlItems);
        //                        }
        //                        else
        //                        {
        //                            string BreadCrumb_replace = node.getPath().Replace("Family Id=", "#");
        //                            string[] BreadCrumb_Name = BreadCrumb_replace.Split('#');
        //                            if (!BreadCrumb_Name[1].Contains("AttribSelect="))
        //                            {
        //                                DataSet d = (DataSet)HttpContext.Current.Session["Family"];
        //                                string path_value = d.Tables[0].Rows[0][1].ToString();
        //                                string Family_Id = d.Tables[0].Rows[0][0].ToString();
        //                                separator = "&nbsp;/&nbsp;";
        //                                if (HttpContext.Current.Request.QueryString["fcnt"] != null)
        //                                {
        //                                    HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">" + path_value + "</a>";
        //                                    href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                                    htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV("Family Name", path_value) + "</a></td></tr></table></li>";
        //                                    HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                                    Htmlitems1.Add(htmlItems);
        //                                }
        //                                else
        //                                {
        //                                    HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">" + path_value + "</a>";
        //                                    href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                                    htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV("Family Name", path_value) + "</a></td></tr></table></li>";
        //                                    HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                                    Htmlitems1.Add(htmlItems);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                DataSet d = (DataSet)HttpContext.Current.Session["Family"];
        //                                string Family_Id = d.Tables[0].Rows[0][0].ToString();
        //                                separator = "&nbsp;/&nbsp;";
        //                                if (HttpContext.Current.Request.QueryString["fcnt"] != null)
        //                                {
        //                                    HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                                    href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                                    htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                                    HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                                    Htmlitems1.Add(htmlItems);
        //                                }
        //                                else
        //                                {
        //                                    HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                                    href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                                    htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                                    HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                                    Htmlitems1.Add(htmlItems);
        //                                }
        //                            }
        //                        }

        //                    }
        //                }
        //            }

        //        }

        //        int h = Htmlitems1.Count;
        //        string HtmlItem_order = "";
        //        for (int i = 0; i < h; i++)
        //        {
        //            HtmlItem_order += Htmlitems1[i].ToString();
        //        }
        //        String html = "";

        //        if (null != HtmlItem_order && 0 < HtmlItem_order.Length)
        //        {
        //             html += "<td class='ea-nav-block ea-clickable'><div class='ea-nav-block-header'><div class='ea-nav-title'>Your current selection:</div></div>";
        //          //  html += "<td class='ea-nav-block ea-clickable'><div class='headimage'> Product Filter Options<br /><span>Your current selection :</span></div>";
        //            html += "<ul class='ea-remove-nav-block-values' >";
        //            html += HtmlItem_order;
        //            html += "</ul></td>";
        //        }
        //        HttpContext.Current.Session["Selected"] = html;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //protected void updateBreadCrumb(IBreadCrumbTrail bct)
        //{
        //    try
        //    {
        //        HttpContext.Current.Session["Link"] = null;
        //        HttpContext.Current.Session["Selected"] = null;
        //        HttpContext.Current.Session["BreadCrumbName"] = null;
        //        string separator = "";
        //        List<String> Htmlitems1 = new List<String>();
        //        String htmlItems = "";
        //        String href = "";
        //        string GetValue = "";
        //        foreach (INavigateNode node in bct.getSearchPath())
        //        {
        //            // string separator1= node.getSEOPath();
        //            IList<INavigateNode> nodes = bct.getSearchPath();

        //            string separator1 = node.getPath();
        //            String label = node.getLabel();
        //            if (node.getLabel() == "Brand")
        //            {
        //                HttpContext.Current.Session["Brand_Name_Dispaly"] = node.getValue();
        //            }
        //            if (node.getLabel() == "Model")
        //            {
        //                string[] ModelName = node.getValue().Split(':');
        //                GetValue = ModelName[1].ToString();
        //                HttpContext.Current.Session["Brand_Model_Name_Dispaly"] = ModelName[1].ToString();
        //            }
        //            else
        //            {
        //                GetValue = node.getValue();
        //            }
        //            if (node.getPath().Contains("AllProducts////WESAUSTRALASIA////Cellular Accessories////"))
        //            {
        //                string Category_breadcrumb = node.getPath().Replace("////", "#");
        //                string[] Category_breadcrumb_name = Category_breadcrumb.Split('#');
        //                int count_breadcrumb = Category_breadcrumb_name.Count();
        //                if (count_breadcrumb == 4)
        //                {
        //                        separator = "&nbsp;/&nbsp;";
        //                        HttpContext.Current.Session["BreadCrumbName"] = separator + "<a href=\"categorylist.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "&amp;tsb="+HttpUtility.UrlEncode(node.getValue())+"\">" + GetValue + "</a>";
        //                        if (HttpContext.Current.Session["Link"] == null)
        //                        {
        //                            HttpContext.Current.Session["Link"] = "<a href=\"Home.aspx\">";
        //                        }
        //                        href = "<a href=\"categorylist.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "&amp;tsb="+HttpUtility.UrlEncode(node.getValue())+"\">";
        //                        htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                        HttpContext.Current.Session["Link"] = "<a href=\"categorylist.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "&amp;tsb=" + HttpUtility.UrlEncode(node.getValue()) + "\">"; 
        //                        Htmlitems1.Add(htmlItems);
        //                }
        //                else
        //                {
        //                            separator = "&nbsp;/&nbsp;";
        //                            HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"brandlist.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                            href = "<a href=\"brandlist.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                            HttpContext.Current.Session["Link"] = "<a href=\"brandlist.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            Htmlitems1.Add(htmlItems);
        //                }
        //            }

        //        }

        //        int h = Htmlitems1.Count;
        //        string HtmlItem_order = "";
        //        for (int i = 0; i < h; i++)
        //        {
        //            HtmlItem_order += Htmlitems1[i].ToString();
        //        }
        //        String html = "";

        //        if (null != HtmlItem_order && 0 < HtmlItem_order.Length)
        //        {
        //            html += "<td class='ea-nav-block ea-clickable' align='left'><div class='ea-nav-block-header'><div class='ea-nav-title'>Your current selection:</div></div>";
        //            //  html += "<td class='ea-nav-block ea-clickable'><div class='headimage'> Product Filter Options<br /><span>Your current selection :</span></div>";
        //            html += "<ul class='ea-remove-nav-block-values'>";
        //            html += HtmlItem_order;
        //            html += "</ul></td>";
        //        }
        //        HttpContext.Current.Session["Selected"] = html;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //protected void updateCommentary(INavigateResults res)
        //{
        //    if (res.getLastItem() == -1)
        //    {
        //        HttpContext.Current.Session["Spell_Correction"] = "<font  style='line-height: 30px; color: #FF0000; font-size: small; font-weight:100;'>No data was found matching your query.</font>";
        //    }
        //    else
        //    { 
        //        String commentary = res.getCommentary();
        //        String prettycomment = "Sorry. There are no results for '";
        //        String outcomment = "";
        //        string Search_Word = "";

        //        if (0 < commentary.Length)
        //        {
        //            if (-1 != commentary.IndexOf("Ignored"))
        //            {
        //                IBreadCrumbTrail bct = res.getBreadCrumbTrail();
        //                IList<INavigateNode> i = bct.getSearchPath();
        //                foreach (INavigateNode node in bct.getSearchPath())
        //                {
        //                    if (node.getValue() != "Cellular Accessories" && node.getValue() != "WESAUSTRALASIA" && node.getValue() != "AllProducts")
        //                    {
        //                        Search_Word = node.getValue();
        //                        outcomment = prettycomment + res.getQuestion() + "'.";
        //                        outcomment += " Search Found Results for '" + Search_Word + "'";
        //                    }
        //                    else
        //                    {
        //                        outcomment = prettycomment + res.getQuestion() + "'.";
        //                    }
        //                }

        //            }
        //            else if (-1 != commentary.IndexOf("Corrected Word"))
        //            {
        //                outcomment = res.getCommentary();
        //                string[] Outcomment = outcomment.Split(';');
        //                outcomment = Outcomment[0].ToString() + ";";
        //            }
        //            //Corrected Word
        //            HttpContext.Current.Session["Spell_Correction"] = "<font  style='line-height: 30px; color: #FF0000; font-size: small; font-weight:100;'>" + outcomment + "</font>";

        //        }
        //        else
        //        {
        //            HttpContext.Current.Session["Spell_Correction"] = null;
        //        }
        //    }
        //}

        //String hCAV(String label, String value)
        //{
        //    String html = "";
        //    if (null != label && 0 < label.Length)
        //    {
        //        html += "<span class='ea-remove-nav-value-label'  style='text-align:left';>" + label + ": </span><br>";
        //    }
        //    html += "<span class='ea-remove-nav-value-value'  style='text-align:left';>" + value + "</span>";
        //    return html;
        //}
        ////String createURITo(String path)
        ////{
        ////    return "Default.aspx?" + path;
        ////}

        //#endregion
        /*********************************** OLD CODE ***********************************/
        #region for Brand List
        /*********************************** OLD CODE ***********************************/
        //DataSet Brand_Product = new DataSet();
        //DataTable Brand_Product_Values = new DataTable();

        //public DataSet Get_Brand_Product(string Model,string Brand)
        //{
        //    try
        //    {
        //        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
        //        IOptions opts = ea.getOptions();
        //        opts.setResultsPerPage("0"); // ea_rpp.Value);   // use current settings
        //        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        //        opts.setGrouping("Category;1000");
        //        opts.setSubCategories(false);
        //        opts.setNavigateHierarchy(false);
        //        opts.setReturnSKUs(true);
        //        Create_Brand_Product_Table_Columns();
        //        INavigateResults res = null;
        //        if (HttpContext.Current.Request["Type"] != null)
        //        {
        //            if (HttpContext.Current.Request["Type"] != "Category")
        //            {
        //                res = ea.userAttributeClick(HttpContext.Current.Session["EA"].ToString(), "" + Model + "='" + Brand + "'");
        //            }
        //            else
        //            {
        //                res = ea.userCategoryClick(HttpContext.Current.Session["EA"].ToString()+"////"+Brand);
        //            }
        //        }
        //        else
        //        {
        //            //Model = Model.Replace(" ", "-").Replace("-+", "").Replace("&", "-").Replace("(", "-").Replace(")", "-").Replace("/", "-").Replace(",", "-");
        //           // Model = Model.Replace("+", "%2b");
        //            res = ea.userAttributeClick_Brand(HttpContext.Current.Session["EA"].ToString(), "Model = '" + Brand + ":" + Model + "'");
        //        }
        //        updateBreadCrumb(res.getBreadCrumbTrail());
        //        HttpContext.Current.Session["EA"] = res.getCatPath();
        //        IList<INavigateCategory> Category = res.getDetailedCategories();
        //        if (Category.Count > 0)
        //        {
        //            foreach (INavigateCategory categoryItem in Category)
        //            {
        //                DataRow row = Brand_Product.Tables["Category"].NewRow();
        //                IList<string> Id = categoryItem.getIDs();
        //                row["SUBCATID_L1"] = Id[0].ToString().Substring(2);
        //                row["SUBCATNAME_L1"] = categoryItem.getName();
        //                Brand_Product.Tables["Category"].Rows.Add(row);
        //            }
        //        }
        //        layoutGroups(res);
        //       //For Main Content Values.
        //        // IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //        // EasyAsk_UserSearch_Attributes_DS(Attributes, res, Brand);
        //        // updateCommentary(res);
        //        HttpContext.Current.Session["Brand_Product_DS"] = Brand_Product;
        //        return Brand_Product;
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorhandler.ErrorMsg = ex;
        //        objErrorhandler.CreateLog();
        //        return null;
        //    }
        //}

        //void Get_Brand_DataSet_Values(INavigateResults res)
        //{
        //    int last = res.getLastItem();
        //    int colFmlyID = res.getColumnIndex("Family Id");
        //    int colFmlyName = res.getColumnIndex("Family Name");
        //    int colFmlyDesc = res.getColumnIndex("Family ShortDescription");
        //    int colFmlylongDesc = res.getColumnIndex("Family Description");
        //    int colFmlyImg = res.getColumnIndex("Family Thumbnail");

        //    int colProductID = res.getColumnIndex("Prod Id");
        //    int colProductCode = res.getColumnIndex("Prod Code");
        //    int colProductPrice = res.getColumnIndex("Price");
        //    int colProductDesc = res.getColumnIndex("Prod Description");
        //    int colProductImg = res.getColumnIndex("Prod Thumbnail");

        //    int colProductCount = res.getColumnIndex("Prod Count");
        //    int colFamilyProdCount = res.getColumnIndex("Family Prod Count");

        //    IList<INavigateCategory> item = res.getDetailedCategories();
        //    DataRow dRow;
        //    try
        //    {
        //        if (last >= 0)
        //        {
        //            for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++)
        //            {
        //                for (int k = 0; k < 9; k++)
        //                {
        //                    dRow = Brand_Product_Values.NewRow();
        //                    dRow["FAMILY_ID"] = res.getCellData(i,colFmlyID);
        //                    dRow["FAMILY_NAME"] = res.getCellData(i,colFmlyName);
        //                    // dRow["DESCRIPTION1"] = res.getCellData(i, colFmlyDesc);
        //                    // dRow["LongDESCRIPTION"] = res.getCellData(i, colFmlylongDesc);
        //                    dRow["PRODUCT_ID"] = res.getCellData(i,colProductID);

        //                    string temp_family_count = res.getCellData(i,colFamilyProdCount);
        //                    string temp_product_count = res.getCellData(i,colProductCount);
        //                    string temp_fmly_Image = res.getCellData(i,colFmlyImg).ToString();
        //                    string temp_product_Image = res.getCellData(i,colProductImg).ToString();
        //                    string image_string = "";

        //                    if (temp_fmly_Image != "" && temp_product_Image != "")
        //                    {
        //                        if (temp_product_count.ToString() == "1")
        //                        {
        //                            image_string = temp_product_Image.Substring(42);
        //                        }
        //                        else
        //                        {
        //                            image_string = temp_fmly_Image.Substring(42);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        image_string = "noimage.gif";
        //                    }

        //                    if (k == 0)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "1";
        //                        dRow["STRING_VALUE"] = res.getCellData(i,colProductCode);//For the Product Code
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Code";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    if (k == 1)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "5";
        //                        dRow["STRING_VALUE"] = "";
        //                        if (res.getCellData(i,colProductPrice) == "" || res.getCellData(i,colProductPrice) == string.Empty)
        //                        {
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                        }
        //                        else
        //                        {
        //                            dRow["NUMERIC_VALUE"] = res.getCellData(i,colProductPrice).Substring(1);//For Cost
        //                        }
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Cost";
        //                        dRow["ATTRIBUTE_TYPE"] = "4";
        //                    }
        //                    if (k == 2)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "62";
        //                        dRow["STRING_VALUE"] = res.getCellData(i,colProductDesc);//Product Description.
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Description";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    if (k == 3)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "449";
        //                        dRow["STRING_VALUE"] = res.getCellData(i,colFmlyDesc);//Family Description
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "PROD_DSC";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    if (k == 4)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "492";
        //                        dRow["STRING_VALUE"] = "";
        //                        if (res.getCellData(i,colProductPrice) == "" || res.getCellData(i,colProductPrice) == string.Empty)
        //                        {
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                        }
        //                        else
        //                        {
        //                            dRow["NUMERIC_VALUE"] = res.getCellData(i,colProductPrice).Substring(1);//For Cost
        //                        }
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "PROD_EXT_PRI_3";
        //                        dRow["ATTRIBUTE_TYPE"] = "4";
        //                    }
        //                    if (k == 5)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "453";
        //                        dRow["STRING_VALUE"] = image_string;
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = image_string;
        //                        dRow["ATTRIBUTE_NAME"] = "Web Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "3";
        //                    }
        //                    if (k == 6)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "7";
        //                        dRow["STRING_VALUE"] = image_string;
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = image_string;
        //                        dRow["ATTRIBUTE_NAME"] = "Product Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "3";
        //                    }
        //                    if (k == 7)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "452";
        //                        dRow["STRING_VALUE"] = image_string;
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = image_string;
        //                        dRow["ATTRIBUTE_NAME"] = "TWeb Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "3";
        //                    }
        //                    if (k == 8)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "4";
        //                        dRow["STRING_VALUE"] = res.getCellData(i,colFmlylongDesc);//Family Description
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "DESCRIPTIONS";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }

        //                    dRow["CATEGORY_ID"] = "";
        //                    // dRow["CATEGORY_NAME"] = string.Empty;
        //                    // dRow["PARENT_CATEGORY_NAME"] = "";
        //                    dRow["PARENT_CATEGORY_ID"] = "";
        //                    dRow["SUBCATNAME_L1"] = "";
        //                    dRow["SUBCATNAME_L2"] = "";
        //                    //  dRow["CUSTOM_NUM_FIELD3"] = "2";
        //                    //  dRow["PARENT_FAMILY_ID"] = "0";
        //                    // dRow["(No column name)"] = "";
        //                    // dRow["LFID"] = res.getCellData(i, colFmlyID);
        //                    //  dRow["Family_Prod_Count"] = temp_family_count;
        //                    //  dRow["Prod_Count"] = temp_product_count;

        //                    if (temp_family_count == temp_product_count)
        //                    {
        //                       // dRow["STATUS"] = true;
        //                    }
        //                    else
        //                    {
        //                      //  dRow["STATUS"] = false;
        //                    }
        //                    Brand_Product_Values.Rows.Add(dRow);
        //                }
        //            }
        //                j++;
        //            }
        //        }
           
        //    catch (Exception ex)
        //    {
        //    }
        //    HttpContext.Current.Session["Brand_Product_Value"] = Brand_Product_Values;
        //    //return HomeSearch;
        //}

        //private void Get_Brand_DataSet_Values(INavigateResults res,IResultRow item1,String name)
        //{
        //     int last = res.getLastItem();
        //    int colFmlyID = res.getColumnIndex("Family Id");
        //    int colFmlyName = res.getColumnIndex("Family Name");
        //    int colFmlyDesc = res.getColumnIndex("Family ShortDescription");
        //    int colFmlylongDesc = res.getColumnIndex("Family Description");
        //    int colFmlyImg = res.getColumnIndex("Family Thumbnail");

        //    int colProductID = res.getColumnIndex("Prod Id");
        //    int colProductCode = res.getColumnIndex("Prod Code");
        //    int colProductPrice = res.getColumnIndex("Price");
        //    int colProductDesc = res.getColumnIndex("Prod Description");
        //    int colProductImg = res.getColumnIndex("Prod Thumbnail");

        //    int colProductCount = res.getColumnIndex("Prod Count");
        //    int colFamilyProdCount = res.getColumnIndex("Family Prod Count");

        //    IList<INavigateCategory> item = res.getDetailedCategories();
        //    DataRow dRow;
        //    try
        //    {

        //       // if (last >= 0)
        //      //  {
        //        //    for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++)
        //          //  {
        //                for (int k = 0; k < 9; k++)
        //                {
        //                    dRow = Brand_Product_Values.NewRow();
        //                    dRow["FAMILY_ID"] = item1.getCellData(colFmlyID);
        //                    dRow["FAMILY_NAME"] = item1.getCellData(colFmlyName);
        //                    // dRow["DESCRIPTION1"] = res.getCellData(i, colFmlyDesc);
        //                    // dRow["LongDESCRIPTION"] = res.getCellData(i, colFmlylongDesc);
        //                    dRow["PRODUCT_ID"] = item1.getCellData(colProductID);

        //                    string temp_family_count = item1.getCellData(colFamilyProdCount);
        //                    string temp_product_count = item1.getCellData(colProductCount);
        //                    string temp_fmly_Image = item1.getCellData(colFmlyImg).ToString();
        //                    string temp_product_Image = item1.getCellData(colProductImg).ToString();
        //                    string image_string = "";

        //                    if (temp_fmly_Image != "" && temp_product_Image != "")
        //                    {
        //                        if (temp_product_count.ToString() == "1")
        //                        {
        //                            image_string = temp_product_Image.Substring(42);
        //                        }
        //                        else
        //                        {
        //                            image_string = temp_fmly_Image.Substring(42);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        image_string = "noimage.gif";
        //                    }

        //                    if (k == 0)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "1";
        //                        dRow["STRING_VALUE"] = item1.getCellData(colProductCode);//For the Product Code
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Code";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    if (k == 1)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "5";
        //                        dRow["STRING_VALUE"] = "";
        //                        if (item1.getCellData(colProductPrice) == "" || item1.getCellData(colProductPrice) == string.Empty)
        //                        {
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                        }
        //                        else
        //                        {
        //                            dRow["NUMERIC_VALUE"] = item1.getCellData(colProductPrice).Substring(1);//For Cost
        //                        }
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Cost";
        //                        dRow["ATTRIBUTE_TYPE"] = "4";
        //                    }
        //                    if (k == 2)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "62";
        //                        dRow["STRING_VALUE"] = item1.getCellData(colProductDesc);//Product Description.
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Description";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    if (k == 3)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "449";
        //                        dRow["STRING_VALUE"] = item1.getCellData(colFmlyDesc);//Family Description
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "PROD_DSC";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    if (k == 4)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "492";
        //                        dRow["STRING_VALUE"] = "";
        //                        if (item1.getCellData(colProductPrice) == "" || item1.getCellData(colProductPrice) == string.Empty)
        //                        {
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                        }
        //                        else
        //                        {
        //                            dRow["NUMERIC_VALUE"] = item1.getCellData(colProductPrice).Substring(1);//For Cost
        //                        }
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "PROD_EXT_PRI_3";
        //                        dRow["ATTRIBUTE_TYPE"] = "4";
        //                    }
        //                    if (k == 5)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "453";
        //                        dRow["STRING_VALUE"] = image_string;
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = image_string;
        //                        dRow["ATTRIBUTE_NAME"] = "Web Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "3";
        //                    }
        //                    if (k == 6)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "7";
        //                        dRow["STRING_VALUE"] = image_string;
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = image_string;
        //                        dRow["ATTRIBUTE_NAME"] = "Product Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "3";
        //                    }
        //                    if (k == 7)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "452";
        //                        dRow["STRING_VALUE"] = image_string;
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = image_string;
        //                        dRow["ATTRIBUTE_NAME"] = "TWeb Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "3";
        //                    }
        //                    if (k == 8)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "4";
        //                        dRow["STRING_VALUE"] = item1.getCellData(colFmlylongDesc);//Family Description
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "DESCRIPTIONS";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }

        //                    dRow["CATEGORY_ID"] = "";
        //                    // dRow["CATEGORY_NAME"] = string.Empty;
        //                    // dRow["PARENT_CATEGORY_NAME"] = "";
        //                    dRow["PARENT_CATEGORY_ID"] = "";
        //                    dRow["SUBCATNAME_L1"] = name;
        //                    dRow["SUBCATNAME_L2"] = "";
        //                    //  dRow["CUSTOM_NUM_FIELD3"] = "2";
        //                    //  dRow["PARENT_FAMILY_ID"] = "0";
        //                    // dRow["(No column name)"] = "";
        //                    // dRow["LFID"] = res.getCellData(i, colFmlyID);
        //                    //  dRow["Family_Prod_Count"] = temp_family_count;
        //                    //  dRow["Prod_Count"] = temp_product_count;

        //                    if (temp_family_count == temp_product_count)
        //                    {
        //                       // dRow["STATUS"] = true;
        //                    }
        //                    else
        //                    {
        //                      //  dRow["STATUS"] = false;
        //                    }
        //                    Brand_Product_Values.Rows.Add(dRow);
        //                }
        //            }
        //       // }
        //    //}
        //    catch (Exception ex)
        //    {
        //    }
        //    HttpContext.Current.Session["Brand_Product_Value"] = Brand_Product_Values;
        //   // return HomeSearch;
        //}

        //private void Create_Brand_Product_Table_Columns()
        //{
        //    Brand_Product.Tables.Add("Category");
        //    Brand_Product.Tables["Category"].Columns.Add("SUBCATID_L1", typeof(string));
        //    Brand_Product.Tables["Category"].Columns.Add("SUBCATNAME_L1", typeof(string));

        //    Brand_Product.Tables.Add(Brand_Product_Values);
        //    Brand_Product_Values.Columns.Add("FAMILY_ID", typeof(string));
        //    Brand_Product_Values.Columns.Add("CATEGORY_ID", typeof(string));
        //    Brand_Product_Values.Columns.Add("PARENT_CATEGORY_ID", typeof(string));
        //    Brand_Product_Values.Columns.Add("FAMILY_NAME", typeof(string));
        //    Brand_Product_Values.Columns.Add("PRODUCT_ID", typeof(string));
        //    Brand_Product_Values.Columns.Add("ATTRIBUTE_ID", typeof(string));
        //    Brand_Product_Values.Columns.Add("STRING_VALUE", typeof(string));
        //    Brand_Product_Values.Columns.Add("NUMERIC_VALUE", typeof(string));
        //    Brand_Product_Values.Columns.Add("OBJECT_TYPE", typeof(string));
        //    Brand_Product_Values.Columns.Add("OBJECT_NAME", typeof(string));
        //    Brand_Product_Values.Columns.Add("ATTRIBUTE_NAME", typeof(string));
        //    Brand_Product_Values.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
        //    Brand_Product_Values.Columns.Add("SUBCATNAME_L1", typeof(string));
        //    Brand_Product_Values.Columns.Add("SUBCATNAME_L2", typeof(string));

        //    //Brand_Product_Values.Columns.Add("DESCRIPTION1", typeof(string));
        //    //Brand_Product_Values.Columns.Add("LongDESCRIPTION", typeof(string));
        //    //Brand_Product_Values.Columns.Add("CATEGORY_NAME", typeof(string));
        //    //Brand_Product_Values.Columns.Add("PARENT_CATEGORY_NAME", typeof(string));
        //    //Brand_Product_Values.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
        //    //Brand_Product_Values.Columns.Add("PARENT_FAMILY_ID", typeof(string));
        //    //Brand_Product_Values.Columns.Add("(No column name)", typeof(string));
        //    //Brand_Product_Values.Columns.Add("LFID", typeof(string));
        //    //Brand_Product_Values.Columns.Add("Family_Prod_Count", typeof(int));
        //    //Brand_Product_Values.Columns.Add("Prod_Count", typeof(int));
        //    //Brand_Product_Values.Columns.Add("STATUS", typeof(bool));

        //}

        //void layoutGroups(INavigateResults res)
        //{
        //    if (res.isGroupedResult())
        //    {
        //        String html = "";
        //        IGroupedResultSet groups = res.getGroupedResult();
        //        int groupType = groups.getGroupCriteriaType();
        //        String path = res.getCatPath();
        //        for (int i = groups.getStartGroup(), len = groups.getEndGroup(); i <= len; i++)
        //        {
        //            IGroupedResult group = groups.getGroup(i);
        //            String name = group.getGroupValue();
        //            if (null == name || 0 == name.Length)
        //            {
        //                continue;  // skip empty
        //            }
        //            String nodeString = groups.getNodeString(group);
        //            int startRow = group.getStartRow();
        //            int endRow = group.getEndRow();
        //            html += "<div style='border:1px solid #D1D3D4'><table border='0' cellpadding='3' cellspacing='1' width='100%' class='EAMTitle'>";
        //            html += "<tr><td width='100%' class='EA_EStoreGroupTitle'><div style='float:left;'>" + group.getGroupValue();
        //            html += "</div><div style='float:right'>" + ((endRow - startRow + 1) + " of " + group.getTotalNumberOfRows());
        //            if (null != nodeString && 0 < nodeString.Length)
        //            {  // need to have some criteria to drill into this is the 'unknown' group
        //                //    html += "<a class='EA_EStoreGroupTitle' href='" + formURLToGroup(path, groupType, nodeString) + "'> more</a>";
        //            }
        //            html += "</div></td></tr></table></div>";
        //            html += "<table cellpadding='2' cellspacing='1' border='0' class='EA_EStoreProductGridViewTable' style='table-layout:fixed;'>";
        //            html += "<tr>";
        //            for (int j = startRow - 1; j < endRow; j++)
        //            {
        //                IResultRow item = group.getItem(j);
        //                Get_Brand_DataSet_Values(res, item, name);
        //                //  html += "<td valign='top' class='resultscell' style='width:125px;'><img src='" + resolveImageURL(item.getCellData(colImg)) + "' /><br /><div class='resultscelltext'>" + item.getCellData(colName) + "<br />" + item.getCellData(colPrice) + "</div></td>";
        //            }
        //            html += "</tr></table>"; //</div></div>";
        //        }
        //        // searchresultsPH.Controls.Add(new LiteralControl(html));
        //    }
        //    else
        //    { 
        //        Get_Brand_DataSet_Values(res);
        //    }
        //}

        //public void BreadCrumbClick1(string rpp, string attribute_value, int curPage, string pageOp)
        //{
        //    IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, "wesCatBrand");
        //    IOptions opts = ea.getOptions();
        //    opts.setResultsPerPage("0"); // ea_rpp.Value);   // use current settings
        //    opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        //    opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
        //    opts.setSubCategories(false);
        //    opts.setNavigateHierarchy(false);
        //    opts.setReturnSKUs(true);
        //    Brand_Model = new DataSet();
        //    DataTable dTable = new DataTable();
        //    INavigateResults res = ea.userBreadCrumbClick1(HttpContext.Current.Session["EA"].ToString());
        //    updateBreadCrumb(res.getBreadCrumbTrail());
        //    try
        //    {
        //        int last = res.getLastItem();
        //        int Model_Name = res.getColumnIndex("ModelValue");
        //        int Model_Image = res.getColumnIndex("Model Thumbnail");
        //        int Brand_Name = res.getColumnIndex("Brand");
        //        // IList Model = res.getDetailedAttributeValues("Model");
        //        Brand_Model.Tables.Add(dTable);
        //        dTable.Columns.Add("TOSUITE_MODEL", typeof(string));
        //        dTable.Columns.Add("IMAGE_FILE", typeof(string));
        //        dTable.Columns.Add("Brand", typeof(string));
        //        string image_string = "";
        //        DataRow dRow;
        //        for (int i = res.getFirstItem() - 1; i < last; i++)
        //        {
        //            dRow = dTable.NewRow();
        //            dRow["TOSUITE_MODEL"] = res.getCellData(i, Model_Name);
        //            // dRow["IMAGE_FILE"] = 
        //            string Model_Image_Name = res.getCellData(i, Model_Image).ToString();
        //            if (Model_Image_Name != "" && Model_Image_Name != null)
        //            {
        //                image_string = Model_Image_Name.Substring(42);
        //            }
        //            else
        //            {
        //                image_string = "noimage.gif";
        //            }
        //            dRow["IMAGE_FILE"] = image_string;
        //            dRow["Brand"] = res.getCellData(i, Brand_Name);
        //            dTable.Rows.Add(dRow);
        //        }
        //    }

        //    catch (Exception)
        //    {
        //    }
        //    HttpContext.Current.Session["WESBrand_Model"] = Brand_Model;
        //}

        //public void BreadCrumbClick_Brand(string rpp, string attribute_value, int curPage, string pageOp)
        //{
        //    try
        //    {
        //        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
        //        IOptions opts = ea.getOptions();
        //        opts.setResultsPerPage("0"); // ea_rpp.Value);   // use current settings
        //        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        //        opts.setGrouping("Category;1000");
        //        opts.setSubCategories(false);
        //        opts.setNavigateHierarchy(false);
        //        opts.setReturnSKUs(true);
        //        Create_Brand_Product_Table_Columns();
        //        DataSet ds = new DataSet();
        //        INavigateResults res = null;
        //        if (curPage < 1)
        //        {
        //            res = ea.userBreadCrumbClick(HttpContext.Current.Session["EA"].ToString());
        //            HttpContext.Current.Session["EA"] = res.getCatPath();
        //            //Get_DataSet_Values(res);//For Main Content Values.
        //        }
        //        else
        //        {
        //           res = ea.userPageOp(HttpContext.Current.Session["EA"].ToString(), curPage.ToString(), pageOp);
        //            //  return ds;
        //        }

        //        IList<INavigateCategory> Category = res.getDetailedCategories();
        //        if (Category.Count > 0)
        //        {
        //            foreach (INavigateCategory categoryItem in Category)
        //            {
        //                DataRow row = Brand_Product.Tables["Category"].NewRow();
        //                IList<string> Id = categoryItem.getIDs();
        //                row["SUBCATID_L1"] = Id[0].ToString().Substring(2);
        //                row["SUBCATNAME_L1"] = categoryItem.getName();
        //                Brand_Product.Tables["Category"].Rows.Add(row);
        //            }
        //        }
        //        updateBreadCrumb(res.getBreadCrumbTrail());
        //        layoutGroups(res);
        //        IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //        EasyAsk_UserSearch_Attributes_DS(Attributes, res, attribute_value);
        //        HttpContext.Current.Session["Brand_Product_DS"] = Brand_Product;
            
        //    }
        //    catch (Exception ex)
        //    {
        //        //   return null;
        //    }
        //}
        /*********************************** OLD CODE ***********************************/

        #endregion


    }
    /*********************************** J TECH CODE ***********************************/
}